using System;
using System.Runtime.InteropServices;

namespace SWICD.HVDK
{
    public enum DriversConst : ushort
    {
        TTC_VENDORID = 0xF00F,
        TTC_PRODUCTID_JOYSTICK = 0x00000001,
        TTC_PRODUCTID_MOUSEABS = 0x00000002,
        TTC_PRODUCTID_KEYBOARD = 0x00000003,
        TTC_PRODUCTID_GAMEPAD = 0x00000004,
        TTC_PRODUCTID_MOUSEREL = 0x00000005,
    }

    //KEYBOARD ------------------------------------------------------------------------------------------------------------
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SetFeatureKeyboard
    {
        public Byte ReportID;
        public Byte CommandCode;
        public uint Timeout;
        public Byte Modifier;
        public Byte Padding;
        public Byte Key0;
        public Byte Key1;
        public Byte Key2;
        public Byte Key3;
        public Byte Key4;
        public Byte Key5;
    }

    //MOUSE ABS ------------------------------------------------------------------------------------------------------------
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SetFeatureMouseAbs
    {
        public Byte ReportID;
        public Byte CommandCode;
        public Byte Buttons;
        public UInt16 X;
        public UInt16 Y;
    }

    //MOUSE REL ------------------------------------------------------------------------------------------------------------
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SetFeatureMouseRel
    {
        public Byte ReportID;
        public Byte CommandCode;
        public Byte Buttons;
        public sbyte X;
        public sbyte Y;
    }

    //JOYSTICK -------------------------------------------------------------------------------------------------------------
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SetFeatureJoy
    {
        public Byte ReportID;
        public Byte CommandCode;
        public UInt16 X;
        public UInt16 Y;
        public UInt16 Z;
        public UInt16 rX;
        public UInt16 rY;
        public UInt16 rZ;
        public UInt16 slider;
        public UInt16 dial;
        public UInt16 wheel;
        public Byte hat;
        public Byte btn0;   //you really could use a byte[15] array here instead, but it's a bit more complex to implement, so we didn't do that here
        public Byte btn1;
        public Byte btn2;
        public Byte btn3;
        public Byte btn4;
        public Byte btn5;
        public Byte btn6;
        public Byte btn7;
        public Byte btn8;
        public Byte btn9;
        public Byte btn10;
        public Byte btn11;
        public Byte btn12;
        public Byte btn13;
        public Byte btn14;
        public Byte btn15;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDataJoy
    {
        public Byte ReportID;
        public UInt16 X;
        public UInt16 Y;
        public UInt16 Z;
        public UInt16 rX;
        public UInt16 rY;
        public UInt16 rZ;
        public UInt16 slider;
        public UInt16 dial;
        public UInt16 wheel;
        public Byte hat;
        public Byte btn0; //you really could use a byte[15] array here instead, but it's a bit more complex to implement, so we didn't do that here
        public Byte btn1;
        public Byte btn2;
        public Byte btn3;
        public Byte btn4;
        public Byte btn5;
        public Byte btn6;
        public Byte btn7;
        public Byte btn8;
        public Byte btn9;
        public Byte btn10;
        public Byte btn11;
        public Byte btn12;
        public Byte btn13;
        public Byte btn14;
        public Byte btn15;
    }

}

