// Decompiled with JetBrains decompiler
// Type: NIRQUEST.FrmGetSpecSet
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NIRQUEST
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
  [CompilerGenerated]
  public sealed class FrmGetSpecSet : ApplicationSettingsBase
  {
    private static FrmGetSpecSet defaultInstance = (FrmGetSpecSet) SettingsBase.Synchronized((SettingsBase) new FrmGetSpecSet());

    public static FrmGetSpecSet Default
    {
      get
      {
        FrmGetSpecSet defaultInstance = FrmGetSpecSet.defaultInstance;
        return defaultInstance;
      }
    }

    [DefaultSettingValue("0")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public int puffIntervalTime
    {
      get
      {
        return (int) this[nameof (puffIntervalTime)];
      }
      set
      {
        this[nameof (puffIntervalTime)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [UserScopedSetting]
    [DefaultSettingValue("0")]
    public int trackBarSpeed
    {
      get
      {
        return (int) this[nameof (trackBarSpeed)];
      }
      set
      {
        this[nameof (trackBarSpeed)] = (object) value;
      }
    }

    [DefaultSettingValue("0")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public int anglethreshold
    {
      get
      {
        return (int) this[nameof (anglethreshold)];
      }
      set
      {
        this[nameof (anglethreshold)] = (object) value;
      }
    }

    [DefaultSettingValue("")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public string txtBreed
    {
      get
      {
        return (string) this[nameof (txtBreed)];
      }
      set
      {
        this[nameof (txtBreed)] = (object) value;
      }
    }

    [DefaultSettingValue("")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public string txtBatch
    {
      get
      {
        return (string) this[nameof (txtBatch)];
      }
      set
      {
        this[nameof (txtBatch)] = (object) value;
      }
    }

    [DefaultSettingValue("")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public string txtMadeSeason
    {
      get
      {
        return (string) this[nameof (txtMadeSeason)];
      }
      set
      {
        this[nameof (txtMadeSeason)] = (object) value;
      }
    }

    [DefaultSettingValue("0")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public int counterFemale
    {
      get
      {
        return (int) this[nameof (counterFemale)];
      }
      set
      {
        this[nameof (counterFemale)] = (object) value;
      }
    }

    [DefaultSettingValue("0")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public int counterMale
    {
      get
      {
        return (int) this[nameof (counterMale)];
      }
      set
      {
        this[nameof (counterMale)] = (object) value;
      }
    }

    [DefaultSettingValue("0")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public int IntegrationTimeBK
    {
      get
      {
        return (int) this[nameof (IntegrationTimeBK)];
      }
      set
      {
        this[nameof (IntegrationTimeBK)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int trackBarValue
    {
      get
      {
        return (int) this[nameof (trackBarValue)];
      }
      set
      {
        this[nameof (trackBarValue)] = (object) value;
      }
    }

    [DefaultSettingValue("False")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public bool GetSpecAutoIsEnergy
    {
      get
      {
        return (bool) this[nameof (GetSpecAutoIsEnergy)];
      }
      set
      {
        this[nameof (GetSpecAutoIsEnergy)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [UserScopedSetting]
    [DefaultSettingValue("0")]
    public double ThresholdDiff
    {
      get
      {
        return (double) this[nameof (ThresholdDiff)];
      }
      set
      {
        this[nameof (ThresholdDiff)] = (object) value;
      }
    }

    [DefaultSettingValue("0,1")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public string WavelengthDiffIndex
    {
      get
      {
        return (string) this[nameof (WavelengthDiffIndex)];
      }
      set
      {
        this[nameof (WavelengthDiffIndex)] = (object) value;
      }
    }

    [DefaultSettingValue("False")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public bool isSaveSpecAtDistinguish
    {
      get
      {
        return (bool) this[nameof (isSaveSpecAtDistinguish)];
      }
      set
      {
        this[nameof (isSaveSpecAtDistinguish)] = (object) value;
      }
    }

    [DefaultSettingValue("1800000")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public int clearSensorIntervalTimes
    {
      get
      {
        return (int) this[nameof (clearSensorIntervalTimes)];
      }
      set
      {
        this[nameof (clearSensorIntervalTimes)] = (object) value;
      }
    }
  }
}
