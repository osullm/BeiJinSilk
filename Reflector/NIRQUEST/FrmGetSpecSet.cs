namespace NIRQUEST
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0"), CompilerGenerated]
    public sealed class FrmGetSpecSet : ApplicationSettingsBase
    {
        private static FrmGetSpecSet defaultInstance = ((FrmGetSpecSet) SettingsBase.Synchronized(new FrmGetSpecSet()));

        [DefaultSettingValue("1"), UserScopedSetting, DebuggerNonUserCode]
        public int anglethreshold
        {
            get
            {
                return (int) this["anglethreshold"];
            }
            set
            {
                this["anglethreshold"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("1800000"), UserScopedSetting]
        public int clearSensorIntervalTimes
        {
            get
            {
                return (int) this["clearSensorIntervalTimes"];
            }
            set
            {
                this["clearSensorIntervalTimes"] = value;
            }
        }

        [DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
        public int counterFemale
        {
            get
            {
                return (int) this["counterFemale"];
            }
            set
            {
                this["counterFemale"] = value;
            }
        }

        [DefaultSettingValue("0"), DebuggerNonUserCode, UserScopedSetting]
        public int counterMale
        {
            get
            {
                return (int) this["counterMale"];
            }
            set
            {
                this["counterMale"] = value;
            }
        }

        public static FrmGetSpecSet Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("1.5")]
        public double getBlackGrdIntervalTime
        {
            get
            {
                return (double) this["getBlackGrdIntervalTime"];
            }
            set
            {
                this["getBlackGrdIntervalTime"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool GetSpecAutoIsEnergy
        {
            get
            {
                return (bool) this["GetSpecAutoIsEnergy"];
            }
            set
            {
                this["GetSpecAutoIsEnergy"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("0"), UserScopedSetting]
        public int IntegrationTimeBK
        {
            get
            {
                return (int) this["IntegrationTimeBK"];
            }
            set
            {
                this["IntegrationTimeBK"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
        public bool isAutoStart
        {
            get
            {
                return (bool) this["isAutoStart"];
            }
            set
            {
                this["isAutoStart"] = value;
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool isEnergy
        {
            get
            {
                return (bool) this["isEnergy"];
            }
            set
            {
                this["isEnergy"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("False"), UserScopedSetting]
        public bool isSaveSpecAtDistinguish
        {
            get
            {
                return (bool) this["isSaveSpecAtDistinguish"];
            }
            set
            {
                this["isSaveSpecAtDistinguish"] = value;
            }
        }

        [DefaultSettingValue("0"), DebuggerNonUserCode, UserScopedSetting]
        public int puffIntervalTime
        {
            get
            {
                return (int) this["puffIntervalTime"];
            }
            set
            {
                this["puffIntervalTime"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("-12.3"), UserScopedSetting]
        public double thermoElectricTemperature
        {
            get
            {
                return (double) this["thermoElectricTemperature"];
            }
            set
            {
                this["thermoElectricTemperature"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("0"), DebuggerNonUserCode]
        public double ThresholdDiff
        {
            get
            {
                return (double) this["ThresholdDiff"];
            }
            set
            {
                this["ThresholdDiff"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("0"), UserScopedSetting]
        public int trackBarSpeed
        {
            get
            {
                return (int) this["trackBarSpeed"];
            }
            set
            {
                this["trackBarSpeed"] = value;
            }
        }

        [DefaultSettingValue("5"), UserScopedSetting, DebuggerNonUserCode]
        public int trackBarValue
        {
            get
            {
                return (int) this["trackBarValue"];
            }
            set
            {
                this["trackBarValue"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue(""), UserScopedSetting]
        public string txtBatch
        {
            get
            {
                return (string) this["txtBatch"];
            }
            set
            {
                this["txtBatch"] = value;
            }
        }

        [DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
        public string txtBreed
        {
            get
            {
                return (string) this["txtBreed"];
            }
            set
            {
                this["txtBreed"] = value;
            }
        }

        [DefaultSettingValue(""), DebuggerNonUserCode, UserScopedSetting]
        public string txtMadeSeason
        {
            get
            {
                return (string) this["txtMadeSeason"];
            }
            set
            {
                this["txtMadeSeason"] = value;
            }
        }

        [DefaultSettingValue("0,1"), UserScopedSetting, DebuggerNonUserCode]
        public string WavelengthDiffIndex
        {
            get
            {
                return (string) this["WavelengthDiffIndex"];
            }
            set
            {
                this["WavelengthDiffIndex"] = value;
            }
        }
    }
}

