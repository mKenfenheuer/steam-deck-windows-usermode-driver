using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Config
{
    public class Configuration : ICloneable
    {
        private Configuration _snapshot;
        public GenericSettings GenericSettings { get; set; } = new GenericSettings();
        public ControllerConfig DefaultControllerConfig { get; set; } = new ControllerConfig();
        public KeyboardButtonActions KeyboardButtonActions { get; set; } = new KeyboardButtonActions();
        public Dictionary<string, ControllerConfig> PerProcessControllerConfig { get; set; } = new Dictionary<string, ControllerConfig>();
        public override bool Equals(object obj)
        {
            return obj is Configuration configuration &&
                   EqualityComparer<GenericSettings>.Default.Equals(GenericSettings, configuration.GenericSettings) &&
                   EqualityComparer<ControllerConfig>.Default.Equals(DefaultControllerConfig, configuration.DefaultControllerConfig) &&
                   EqualityComparer<KeyboardButtonActions>.Default.Equals(KeyboardButtonActions, configuration.KeyboardButtonActions) &&
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
            obj.KeyboardButtonActions = (KeyboardButtonActions)KeyboardButtonActions.Clone();
            obj.PerProcessControllerConfig = PerProcessControllerConfig.ToDictionary(entry => entry.Key,
                                               entry => (ControllerConfig)entry.Value.Clone());
            return obj;
        }

        public override string ToString()
        {
            string configText = GenericSettings.ToString();

            configText += "\r\n";

            configText += DefaultControllerConfig.ToString();

            configText += "\r\n";

            configText += KeyboardButtonActions.ToString();

            configText += "\r\n";

            foreach (string executable in PerProcessControllerConfig.Keys)
                configText += PerProcessControllerConfig[executable].ToString() + "\r\n";

            return configText;
        }
    }
}
