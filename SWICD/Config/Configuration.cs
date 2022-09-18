using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Config
{
    [Serializable]
    public class Configuration : ICloneable,ISerializable
    {
        private Configuration _snapshot;
        public GenericSettings GenericSettings { get; set; } = new GenericSettings();
        public ControllerConfig DefaultControllerConfig { get; set; } = new ControllerConfig();
        public ButtonActions ButtonActions { get; set; } = new ButtonActions();
        public Dictionary<string, ControllerConfig> PerProcessControllerConfig { get; set; } = new Dictionary<string, ControllerConfig>();
        public Configuration()
        {
        }
        public Configuration(SerializationInfo info, StreamingContext context)
        {
            GenericSettings = (GenericSettings)info.GetValue(nameof(GenericSettings), typeof(GenericSettings));
            DefaultControllerConfig = (ControllerConfig)info.GetValue(nameof(DefaultControllerConfig), typeof(ControllerConfig));
            ButtonActions = (ButtonActions)info.GetValue(nameof(ButtonActions), typeof(ButtonActions));

            ControllerConfig[] PerProcessConfigs = (ControllerConfig[])info.GetValue(nameof(PerProcessControllerConfig), typeof(ControllerConfig[]));

            foreach (var config in PerProcessConfigs)
                PerProcessControllerConfig.Add(config.Executable, config);

            CreateSnapshot();
        }

        public override bool Equals(object obj)
        {
            return obj is Configuration configuration &&
                   EqualityComparer<GenericSettings>.Default.Equals(GenericSettings, configuration.GenericSettings) &&
                   EqualityComparer<ControllerConfig>.Default.Equals(DefaultControllerConfig, configuration.DefaultControllerConfig) &&
                   EqualityComparer<ButtonActions>.Default.Equals(ButtonActions, configuration.ButtonActions) &&
                   PerProcessControllerConfig.EqualsWithValues(configuration.PerProcessControllerConfig);
        }

        public void CreateSnapshot()
        {
            _snapshot = (Configuration)this.Clone();
        }

        public bool HasChanges()
        {
            return !_snapshot.Equals(this);
        }

        public object Clone()
        {
            var obj = new Configuration();
            obj.GenericSettings = (GenericSettings)GenericSettings.Clone();
            obj.DefaultControllerConfig = (ControllerConfig)DefaultControllerConfig.Clone();
            obj.ButtonActions = (ButtonActions)ButtonActions.Clone();
            obj.PerProcessControllerConfig = PerProcessControllerConfig.ToDictionary(entry => entry.Key,
                                               entry => (ControllerConfig)entry.Value.Clone());
            return obj;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("GenericSettings", GenericSettings, typeof(GenericSettings));
            info.AddValue("DefaultControllerConfig", DefaultControllerConfig, typeof(ControllerConfig));
            info.AddValue("ButtonActions", ButtonActions, typeof(ButtonActions));
            info.AddValue("PerProcessControllerConfig", PerProcessControllerConfig.Select(c => c.Value).ToArray(), typeof(ControllerConfig[]));
        }
    }
}
