namespace test_01
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    public sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
        }

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [DefaultSettingValue("1"), DebuggerNonUserCode, UserScopedSetting]
        public int selectSetCount
        {
            get
            {
                return (int) this["selectSetCount"];
            }
            set
            {
                this["selectSetCount"] = value;
            }
        }

        [DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
        public int selectSetMethod
        {
            get
            {
                return (int) this["selectSetMethod"];
            }
            set
            {
                this["selectSetMethod"] = value;
            }
        }

        [DefaultSettingValue("1.6"), DebuggerNonUserCode, UserScopedSetting]
        public double thresholdAbnormal //异常样本阈值
        {
            get
            {
                return (double) this["thresholdAbnormal"];
            }
            set
            {
                this["thresholdAbnormal"] = value;
            }
        }
    }
}

