using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Threading;
using System.Runtime.CompilerServices;

//this is the minimal amount of code to allow reading from and writing to the device driver.
//you should consider using a thread for your reading (and maybe writing) code.

namespace SWICD.HVDK
{

    public class LogArgs : EventArgs
    {
        public string Msg;
    }

    class HIDController
    {

        public event EventHandler<LogArgs> OnLog;

        public Guid HIDGuid;
        protected Boolean FConnected = false;
        protected ushort FProductID;
        protected ushort FVendorID;
        protected SafeFileHandle FDevHandle;
        protected String FDevicePathName;

        public HIDController()
        {

        }

        public Boolean Connected
        {
            get { return FConnected; }
            set { FConnected = value; }
        }

        public ushort ProductID
        {
            get { return FProductID; }
            set { FProductID = value; }

        }

        public ushort VendorID
        {
            get { return FVendorID; }
            set { FVendorID = value; }

        }

        public enum DiGetClassFlags : uint
        {
            DIGCF_PRESENT = 0x00000002,
            DIGCF_DEVICEINTERFACE = 0x00000010,
        }

        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;

        IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid interfaceClassGuid;
            public Int32 flags;
            private IntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public uint cbSize;
            public char devicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        public struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public UInt16 VendorID;
            public UInt16 ProductID;
            public UInt16 VersionNumber;
        }

        //------------------------------------------------------------------------------------------------------

        [DllImport("SetupApi.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

        [DllImport("Setupapi.dll", CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, uint memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            uint deviceInterfaceDetailDataSize,
            out uint requiredSize,
            SP_DEVINFO_DATA deviceInfoData);

        [DllImport("HID.dll", CharSet = CharSet.Auto)]
        static extern void HidD_GetHidGuid(out Guid ClassGuid);

        [DllImport("HID.dll", CharSet = CharSet.Auto)]
        static extern bool HidD_GetAttributes(SafeFileHandle HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

        [DllImport("HID.dll", CharSet = CharSet.Auto)]
        private static extern bool HidD_GetNumInputBuffers(SafeFileHandle HidDeviceObject, ref uint NumberBuffers);

        [DllImport("hid.dll", CharSet = CharSet.Auto)]
        private static extern bool HidD_SetNumInputBuffers(SafeFileHandle HidDeviceObject, uint BufferLength);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool HidD_SetFeature(SafeFileHandle HidDeviceObject, byte[] Buffer, uint BufferLength);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            //UInt32 fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            //[MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
            uint flagsAndAttributes,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool ReadFileEx(
            SafeFileHandle hFile,
            [Out] byte[] lpbuffer,
            [In] uint nNumberOfBytesToRead,
            [In, Out] ref NativeOverlapped lpOverlapped,
            IntPtr lpCompletionRoutine);

        //------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------

        //make a log entry
        private void DoLog(string Msg)
        {
            if (OnLog != null)
            {
                LogArgs args = new LogArgs();
                args.Msg = Msg;
                OnLog(this, args);
            }
        }

        //connect to the driver.  We'll iterate through all present HID devices and find the one that matches our target VENDORID and PRODUCTID
        public void Connect()
        {
            DoLog("Connecting...");
            if (FConnected)
            {
                DoLog("Already connected.");
                return;
            }

            HidD_GetHidGuid(out HIDGuid);

            IntPtr PnPHandle = SetupDiGetClassDevs(ref HIDGuid, IntPtr.Zero, IntPtr.Zero, (int)(DiGetClassFlags.DIGCF_PRESENT | DiGetClassFlags.DIGCF_DEVICEINTERFACE));
            if (PnPHandle == (IntPtr)INVALID_HANDLE_VALUE)
            {
                DoLog("Connect: SetupDiGetClassDevs failed.");
                return;
            }

            //we coudld use a lot more failure and exception logging during this loop
            Boolean bFoundADevice = false;
            Boolean bFoundMyDevice = false;
            uint i = 0;
            do
            {
                SP_DEVICE_INTERFACE_DATA DevInterfaceData = new SP_DEVICE_INTERFACE_DATA();
                DevInterfaceData.cbSize = (uint)Marshal.SizeOf(DevInterfaceData);
                bFoundADevice = SetupDiEnumDeviceInterfaces(PnPHandle, IntPtr.Zero, ref HIDGuid, i, ref DevInterfaceData);
                if (bFoundADevice)
                {
                    SP_DEVINFO_DATA DevInfoData = new SP_DEVINFO_DATA();
                    DevInfoData.cbSize = (uint)Marshal.SizeOf(DevInfoData);
                    uint needed;
                    bool result3 = SetupDiGetDeviceInterfaceDetail(PnPHandle, DevInterfaceData, IntPtr.Zero, 0, out needed, DevInfoData);
                    if (!result3)
                    {
                        int error = Marshal.GetLastWin32Error();
                        if (error == 122)
                        {
                            //it's supposed to give an error 122 as we just only retrieved the data size needed, so this is as designed
                            IntPtr DeviceInterfaceDetailData = Marshal.AllocHGlobal((int)needed);
                            try
                            {
                                uint size = needed;
                                Marshal.WriteInt32(DeviceInterfaceDetailData, IntPtr.Size == 8 ? 8 : 6);
                                bool result4 = SetupDiGetDeviceInterfaceDetail(PnPHandle, DevInterfaceData, DeviceInterfaceDetailData, size, out needed, DevInfoData);
                                if (!result4)
                                {
                                    //shouldn't be an error here
                                    int error1 = Marshal.GetLastWin32Error();
                                    //todo: go +1 and contine the loop...this exception handing is incomplete
                                }
                                IntPtr pDevicePathName = new IntPtr(DeviceInterfaceDetailData.ToInt64() + 4);
                                FDevicePathName = Marshal.PtrToStringAuto(pDevicePathName);
                                //see if this driver has readwrite access
                                FDevHandle = CreateFile(FDevicePathName, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, IntPtr.Zero, System.IO.FileMode.Open, 0, IntPtr.Zero);
                                if (FDevHandle.IsInvalid)
                                {
                                    FDevHandle = CreateFile(FDevicePathName, 0, System.IO.FileShare.ReadWrite, IntPtr.Zero, System.IO.FileMode.Open, 0, IntPtr.Zero);
                                }
                                if (!FDevHandle.IsInvalid)
                                {
                                    //this device has readwrite access, could it be the device we are looking for?
                                    HIDD_ATTRIBUTES HIDAttributes = new HIDD_ATTRIBUTES();
                                    HIDAttributes.Size = Marshal.SizeOf(HIDAttributes);
                                    Boolean success = HidD_GetAttributes(FDevHandle, ref HIDAttributes);
                                    if (success && HIDAttributes.VendorID == FVendorID && HIDAttributes.ProductID == FProductID)
                                    {
                                        //this is the device we are looking for
                                        bFoundMyDevice = true;
                                        FConnected = true;
                                        DoLog("Connected.");
                                        //normally you would start a read thread here, but we aren't doing that in this example. We'll just call .ReadData instead.
                                        //we won't close our file handle here as it will be need in .SendData.
                                    }
                                    else
                                    {
                                        //not the device we are looking for
                                        FDevHandle.Close();
                                    }
                                }
                            }
                            finally
                            {
                                Marshal.FreeHGlobal(DeviceInterfaceDetailData);
                            }
                        }
                    }
                }
                i++;
            } while ((bFoundADevice) & (!bFoundMyDevice));
        }

        //disconnect
        public void Disconnect()
        {
            //uninitialize some of our connect code
            //getting occasional error on shutdown due to something being left open...
            //not sure what effect garbage collector has on this.
            FConnected = false;
            FDevHandle.Close();
        }

        //send data to the driver
        public Boolean SendData(byte[] Buffer, uint BufferLength)
        {
            bool res = false;
            if (FConnected)
            {
                res = HidD_SetFeature(FDevHandle, Buffer, BufferLength + 1);
            }
            return res;
        }

        //read data from the driver.  This ideally should be in a thread
        //in this example, we oversimplified the possible effects of overlapped data reads since readfilex is asynchronous
        public Boolean ReadData(byte[] Buffer, uint BufferLength)
        {
            bool res = false;
            if (FConnected)
            {
                if (!FDevHandle.IsInvalid)
                        
                {
                    var overlapped = new NativeOverlapped();
                    overlapped.EventHandle = IntPtr.Zero;
                    bool res1 = ReadFileEx(FDevHandle, Buffer, BufferLength, ref overlapped, IntPtr.Zero);
                    res = true;
                }
            }
            return res;
        }

    }


}
