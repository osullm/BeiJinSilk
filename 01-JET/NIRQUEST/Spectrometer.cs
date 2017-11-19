// Decompiled with JetBrains decompiler
// Type: JSDU.Spectrometer
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using OmniDriver;
using SPAM;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace JSDU
{
  public class Spectrometer
  {
    public static int spectrometerIndex = -1;
    public static bool isClearDarks = false;
    public static bool isCorrectElectricalDarks = false;
    public static bool isCorrectNonlinearitys = false;
    public CCoWrapper wrapper = new CCoWrapper();
    public CCoSpectralMath spectralMath = new CCoSpectralMath();
    private DataIO DataIOmy = new DataIO();
    private DataHandling DataHandlingmy = new DataHandling();
    public const byte FT_PURGE_RX = 1;
    public const byte FT_PURGE_TX = 2;
    public ProgressBar _ProgressBar;
    public static string SavePath;
    public static string ApplicationPath;
    public static int IntegrationTime;
    public static int IntegrationTimeBK;
    public static int ScanTimes;
    public static int GainMode;
    public static Spectrometer.Data[] DataGet;

    public void ShowProgress(int totalStep, int currentStep)
    {
      this._ProgressBar.Maximum = totalStep;
      this._ProgressBar.Value = currentStep;
      if (currentStep != totalStep)
        return;
      this._ProgressBar.Visible = false;
    }

    public void ReadBK(ref Spectrometer.SpecInfo SpInfo)
    {
      if (File.Exists(Environment.CurrentDirectory.ToString() + "\\background"))
      {
        int length = this.DataIOmy.TXTReadData(Environment.CurrentDirectory.ToString() + "\\background", ref SpInfo.DataX, ref SpInfo.DataAB, true);
        SpInfo.DataX = new double[length];
        SpInfo.DataAB = new double[length];
        this.DataIOmy.TXTReadData(Environment.CurrentDirectory.ToString() + "\\background", ref SpInfo.DataX, ref SpInfo.DataAB, false);
      }
      else
      {
        int num = (int) MessageBox.Show("背景文件不存在，请先采集背景！");
      }
    }

    public void ReadDK(ref Spectrometer.SpecInfo SpInfo)
    {
      if (File.Exists(Environment.CurrentDirectory.ToString() + "\\dark"))
      {
        int length = this.DataIOmy.TXTReadData(Environment.CurrentDirectory.ToString() + "\\dark", ref SpInfo.DataX, ref SpInfo.DataAD, true);
        SpInfo.DataAD = new double[length];
        this.DataIOmy.TXTReadData(Environment.CurrentDirectory.ToString() + "\\dark", ref SpInfo.DataX, ref SpInfo.DataAD, false);
      }
      else
      {
        int num = (int) MessageBox.Show("背景文件不存在，请先采集背景！");
      }
    }

    public void ReadSetParameters()
    {
      if (File.Exists(Spectrometer.ApplicationPath + "\\Setting"))
      {
        string[] Content;
        this.DataIOmy.ReadLineTXT(Spectrometer.ApplicationPath + "\\Setting", out Content);
        if (Content.Length > 0)
        {
          string[] strArray = Content[0].Split(',');
          string text = "";
          Spectrometer.IntegrationTime = Convert.ToInt32(strArray[0]);
          if (Spectrometer.IntegrationTime > 800000)
            Spectrometer.IntegrationTime = 800000;
          Spectrometer.ScanTimes = Convert.ToInt32(strArray[1]);
          Spectrometer.GainMode = Convert.ToInt32(strArray[2]);
          Spectrometer.SavePath = strArray[3];
          if (!Directory.Exists(Spectrometer.SavePath))
            Spectrometer.SavePath = Spectrometer.ApplicationPath;
          Spectrometer.isClearDarks = Convert.ToInt32(strArray[4]) == 1;
          Spectrometer.isCorrectElectricalDarks = Convert.ToInt32(strArray[5]) == 1;
          Spectrometer.isCorrectNonlinearitys = Convert.ToInt32(strArray[6]) == 1;
          if (!(text != ""))
            return;
          int num = (int) MessageBox.Show(text);
        }
        else
        {
          int num1 = (int) MessageBox.Show("设置信息读取错误，请先进行设置！");
        }
      }
      else
      {
        int num2 = (int) MessageBox.Show("设置信息读取错误，请先进行设置！");
      }
    }

    public void SetParameters(int spectrometerIndex, int IntegrationTime)
    {
      this.wrapper.setBoxcarWidth(spectrometerIndex, 10);
      this.wrapper.setIntegrationTime(spectrometerIndex, IntegrationTime);
    }

    public bool GetSpec(int spectrometerIndex, ref Spectrometer.SpecInfo SpInfo, int WaitTime, BackgroundWorker bk)
    {
      this.SetParameters(Spectrometer.spectrometerIndex, Spectrometer.IntegrationTime);
      SpInfo.numPixls = this.wrapper.getNumberOfPixels(spectrometerIndex);
      SpInfo.WavelengthArray = this.wrapper.getWavelengths(spectrometerIndex);
      this.wrapper.setScansToAverage(spectrometerIndex, Spectrometer.ScanTimes);
      for (int percentProgress = 0; percentProgress < 5; ++percentProgress)
      {
        Thread.Sleep(1000);
        bk.ReportProgress(percentProgress);
      }
      SpInfo.DataA = this.wrapper.getSpectrum(spectrometerIndex);
      bk.ReportProgress(5);
      if (SpInfo.WavelengthArray.Length <= 10 || SpInfo.DataA.Length <= 10)
        return false;
      SpInfo.DataX = SpInfo.WavelengthArray;
      if (SpInfo.DataAB.Length != SpInfo.DataA.Length)
        return false;
      SpInfo.DataY = new double[SpInfo.numPixls];
      for (int index = 0; index < SpInfo.DataA.Length; ++index)
      {
        try
        {
          SpInfo.DataY[index] = !Spectrometer.isClearDarks ? Convert.ToDouble(Math.Log10(Math.Abs(SpInfo.DataAB[index] / SpInfo.DataA[index]))) : Convert.ToDouble(Math.Log10(Math.Abs((SpInfo.DataAB[index] - SpInfo.DataAD[index]) / (SpInfo.DataA[index] - SpInfo.DataAD[index]))));
        }
        catch
        {
          SpInfo.DataY[index] = index <= 0 ? 0.0 : SpInfo.DataY[index - 1];
        }
      }
      return true;
    }

    public bool GetSingleBeam(int spectrometerIndex, ref Spectrometer.SpecInfo SpInfo, bool isBKGrdFlag)
    {
      if (isBKGrdFlag)
        this.SetParameters(Spectrometer.spectrometerIndex, Spectrometer.IntegrationTimeBK);
      else
        this.SetParameters(Spectrometer.spectrometerIndex, Spectrometer.IntegrationTime);
      SpInfo.numPixls = this.wrapper.getNumberOfPixels(spectrometerIndex);
      try
      {
        SpInfo.WavelengthArray = this.wrapper.getWavelengths(spectrometerIndex);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
      DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
      SpInfo.DataA = this.wrapper.getSpectrum(spectrometerIndex);
      DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
      if (SpInfo.WavelengthArray.Length <= 10 || SpInfo.DataA.Length <= 10)
        return false;
      SpInfo.DataX = SpInfo.WavelengthArray;
      return true;
    }

    public void GetSingleBeamOnly(int spectrometerIndex, ref Spectrometer.SpecInfo SpInfo)
    {
      for (int index = 0; index < SpInfo.DataA.Length; ++index)
        SpInfo.DataA[index] = 0.0;
      this.wrapper.setIntegrationTime(spectrometerIndex, Spectrometer.IntegrationTime);
      SpInfo.DataA = this.wrapper.getSpectrum(spectrometerIndex);
    }

    private delegate void ShowProgressDelegate(int totalStep, int currentStep);

    public struct SpecInfo
    {
      public int numPixelsT;
      public int resolution;
      public int PixelClockPeriod;
      public double w1;
      public double w2;
      public int numPixls;
      public double[] BadPixels;
      public double[] DataX;
      public double[] DataA;
      public double[] DataY;
      public double[] DataAB;
      public double[] DataAD;
      public double[] WavelengthArray;
    }

    public struct Data
    {
      public double[] DataX;
      public double[] DataY;
      public double[] DataE;
    }
  }
}
