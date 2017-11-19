// Decompiled with JetBrains decompiler
// Type: JSDU.FrmGetSpecAuto
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using NIRQUEST;
using NIRQUEST.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace JSDU
{
  public class FrmGetSpecAuto : Form
  {
    private static bool startFlag = false;
    private Spectrometer MySpectrometer = new Spectrometer();
    private DataIO DataIOmy = new DataIO();
    private DataHandling DataHandlingmy = new DataHandling();
    private Spectrometer.Data[] specData = new Spectrometer.Data[1];
    private Home frmHome = (Home) null;
    private FrmGetSpecAuto.ViewStyle ViewStylemy = FrmGetSpecAuto.ViewStyle.Spec;
    private Color[] DrawColor = new Color[20];
    private int counterFemale = 0;
    private int counterMale = 0;
    private int specTempNum = 0;
    private bool silkWormStart = false;
    private long timestart = 0;
    private long time1 = 0;
    private string SpName = "";
    private int[] selectWaveIndexDiff = (int[]) null;
    private Thread tGetSpecAuto = (Thread) null;
    private IContainer components = (IContainer) null;
    public const byte FT_PURGE_RX = 1;
    public const byte FT_PURGE_TX = 2;
    private int numberOfSpectrometersFound;
    public static Spectrometer.SpecInfo SpInfo;
    private double[,] MeanY;
    private double[] rubbery;
    private Spectrometer.Data[] silkWormSpec;
    private int anglethreshold;
    private FrmGetSpecAuto.MyChartLoadData myChartLoadData;
    private Panel panelHeader;
    private System.Windows.Forms.Label lblConnectState;
    private System.Windows.Forms.Label label7;
    private TextBox txtTimeCount;
    private TextBox txtAngleThreshold;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private TrackBar trackBarSpeed;
    private BackgroundWorker backgroundWorker1;
    private Panel panel3;
    private Button btnSetting;
    private Button btnConveyorOn;
    private System.Windows.Forms.Label label18;
    private Button btnSpectrometerOff;
    private Button btnValve1On;
    private System.Windows.Forms.Label label17;
    private Button btnConveyorOff;
    private Button btnReferenceCupOff;
    private Button btnReferenceCupOn;
    private System.Windows.Forms.Label label15;
    private Button btnLightOff;
    private Button btnLightOn;
    private System.Windows.Forms.Label label16;
    private Button btnValveOff;
    private Button btnClose;
    private Button btnStop;
    private Button btnGetSpec;
    private ZedGraphControl MyChart;
    private System.Windows.Forms.Timer timer1;
    private Button btnBackGrd;
    private CheckBox CBisEnergy;
    private ListBox listBoxGetSpec;

    public FrmGetSpecAuto(Home home)
    {
      this.InitializeComponent();
      this.frmHome = home;
    }

    private void FrmGetSpecAuto_Load(object sender, EventArgs e)
    {
      Spectrometer.ApplicationPath = Directory.GetCurrentDirectory();
      this.numberOfSpectrometersFound = this.MySpectrometer.wrapper.openAllSpectrometers();
      if (this.numberOfSpectrometersFound > 0)
      {
        Spectrometer.spectrometerIndex = 0;
        this.lblConnectState.Text = "已连接";
        this.MySpectrometer.wrapper.setAutoToggleStrobeLampEnable(Spectrometer.spectrometerIndex, (short) 1);
      }
      else
      {
        this.lblConnectState.Text = "未连接";
        this.btnGetSpec.Enabled = false;
        this.btnSetting.Enabled = false;
        this.btnStop.Enabled = false;
      }
      this.MySpectrometer.ReadBK(ref FrmGetSpecAuto.SpInfo);
      this.MySpectrometer.ReadSetParameters();
      Spectrometer.IntegrationTimeBK = FrmGetSpecSet.Default.IntegrationTimeBK;
      this.trackBarSpeed.Value = FrmGetSpecSet.Default.trackBarSpeed;
      this.anglethreshold = FrmGetSpecSet.Default.anglethreshold;
      this.txtAngleThreshold.Text = this.anglethreshold.ToString();
      if (Spectrometer.isClearDarks)
        this.MySpectrometer.ReadDK(ref FrmGetSpecAuto.SpInfo);
      double[] Data_x = (double[]) null;
      int length = this.DataIOmy.TXTReadData(Environment.CurrentDirectory.ToString() + "\\conveyor.txt", ref Data_x, ref this.rubbery, true);
      Data_x = new double[length];
      this.rubbery = new double[length];
      this.DataIOmy.TXTReadData(Environment.CurrentDirectory.ToString() + "\\conveyor.txt", ref Data_x, ref this.rubbery, false);
      this.counterFemale = FrmGetSpecSet.Default.counterFemale;
      this.counterMale = FrmGetSpecSet.Default.counterMale;
      this.trackBarSpeed.Value = FrmGetSpecSet.Default.trackBarValue;
      this.CBisEnergy.Checked = FrmGetSpecSet.Default.GetSpecAutoIsEnergy;
      string[] strArray = FrmGetSpecSet.Default.WavelengthDiffIndex.Substring(0, FrmGetSpecSet.Default.WavelengthDiffIndex.Length - 1).Split(',');
      this.selectWaveIndexDiff = new int[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] != "")
          this.selectWaveIndexDiff[index] = int.Parse(strArray[index]);
      }
      this.Update();
    }

    private void btnGetSpec_Click(object sender, EventArgs e)
    {
      this.btnGetSpec.Enabled = false;
      this.btnGetSpec.BackColor = Color.FromArgb(38, 136, 210);
      this.btnStop.Enabled = true;
      this.anglethreshold = int.Parse(this.txtAngleThreshold.Text);
      FrmGetSpecSet.Default.anglethreshold = this.anglethreshold;
      FrmGetSpecSet.Default.Save();
      FrmGetSpecAuto.startFlag = true;
      Spectrometer.DataGet = new Spectrometer.Data[20];
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] numArray = Home.SPControlWord["ConveyorOn"];
        byte[] buffer = Home.SPControlWord["LightOn"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
      this.time1 = DateTime.Now.Ticks;
      this.tGetSpecAuto = new Thread(new ThreadStart(this.GetSpecAuto));
      this.timer1.Enabled = true;
      if (this.tGetSpecAuto.ThreadState == ThreadState.Running)
      {
        int num1 = (int) MessageBox.Show("正在检测，请勿重复点击，谢谢！");
      }
      else
      {
        this.tGetSpecAuto.Priority = ThreadPriority.Highest;
        this.tGetSpecAuto.Start();
      }
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
      this.btnGetSpec.BackColor = Color.Transparent;
      this.silkWormStart = false;
      FrmGetSpecAuto.startFlag = false;
      this.btnGetSpec.Enabled = true;
      this.btnStop.Enabled = false;
      FrmGetSpecSet.Default.counterFemale = this.counterFemale;
      FrmGetSpecSet.Default.counterMale = this.counterMale;
      FrmGetSpecSet.Default.Save();
      this.timer1.Enabled = false;
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["ConveyorOff"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void GetSpecAuto()
    {
      Dictionary<string, double[]> dictionary = new Dictionary<string, double[]>();
      this.silkWormSpec = new Spectrometer.Data[1];
      this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref FrmGetSpecAuto.SpInfo, false);
      this.ViewStylemy = !this.CBisEnergy.Checked ? FrmGetSpecAuto.ViewStyle.Spec : FrmGetSpecAuto.ViewStyle.Energy;
      this.timer1.Enabled = true;
      while (FrmGetSpecAuto.startFlag)
      {
        DateTime now = DateTime.Now;
        now.ToString("yyyy-MM-dd hh-mm-ss-fff");
        this.MySpectrometer.GetSingleBeamOnly(Spectrometer.spectrometerIndex, ref FrmGetSpecAuto.SpInfo);
        if (!this.CBisEnergy.Checked)
        {
          if (FrmGetSpecAuto.SpInfo.DataAB.Length == FrmGetSpecAuto.SpInfo.DataA.Length)
          {
            FrmGetSpecAuto.SpInfo.DataY = new double[FrmGetSpecAuto.SpInfo.numPixls];
            if (Spectrometer.isClearDarks)
            {
              for (int index = 0; index < FrmGetSpecAuto.SpInfo.DataA.Length; ++index)
              {
                try
                {
                  FrmGetSpecAuto.SpInfo.DataY[index] = Convert.ToDouble(Math.Log10(Math.Abs((FrmGetSpecAuto.SpInfo.DataAB[index] - FrmGetSpecAuto.SpInfo.DataAD[index]) / (FrmGetSpecAuto.SpInfo.DataA[index] - FrmGetSpecAuto.SpInfo.DataAD[index]))));
                }
                catch
                {
                  FrmGetSpecAuto.SpInfo.DataY[index] = index <= 0 ? 0.0 : FrmGetSpecAuto.SpInfo.DataY[index - 1];
                }
              }
            }
            else
            {
              for (int index = 0; index < FrmGetSpecAuto.SpInfo.DataA.Length; ++index)
              {
                try
                {
                  FrmGetSpecAuto.SpInfo.DataY[index] = Convert.ToDouble(Math.Log10(Math.Abs(FrmGetSpecAuto.SpInfo.DataAB[index] / FrmGetSpecAuto.SpInfo.DataA[index])));
                }
                catch
                {
                  FrmGetSpecAuto.SpInfo.DataY[index] = index <= 0 ? 0.0 : FrmGetSpecAuto.SpInfo.DataY[index - 1];
                }
              }
            }
          }
        }
        else
          FrmGetSpecAuto.SpInfo.DataY = FrmGetSpecAuto.SpInfo.DataA;
        if (this.isWilm(FrmGetSpecAuto.SpInfo.DataA))
        {
          this.silkWormStart = true;
          now = DateTime.Now;
          string key = now.ToString("yyyy-MM-dd hh-mm-ss-fff");
          double[] numArray = new double[FrmGetSpecAuto.SpInfo.DataY.Length];
          for (int index = 0; index < FrmGetSpecAuto.SpInfo.DataY.Length; ++index)
            numArray[index] = FrmGetSpecAuto.SpInfo.DataY[index];
          if (!dictionary.ContainsKey(key))
            dictionary.Add(key, numArray);
        }
        else if (this.silkWormStart)
        {
          this.silkWormStart = false;
          int num1 = 0;
          double[,] X = new double[dictionary.Count - 2 * num1, FrmGetSpecAuto.SpInfo.numPixls];
          int num2 = 0;
          int index1 = 0;
          now = DateTime.Now;
          now.ToString("yyyy-MM-dd hh-mm-ss-fff");
          foreach (KeyValuePair<string, double[]> keyValuePair in dictionary)
          {
            ++num2;
            if (num2 > num1 && num2 < dictionary.Count - num1 + 1)
            {
              for (int index2 = 0; index2 < FrmGetSpecAuto.SpInfo.numPixls; ++index2)
                X[index1, index2] = keyValuePair.Value[index2];
              ++index1;
            }
            this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\" + keyValuePair.Key.ToString() + ".txt", FrmGetSpecAuto.SpInfo.DataX, keyValuePair.Value);
          }
          double[] numArray = this.DataHandlingmy.SpMean(X);
          this.silkWormSpec = new Spectrometer.Data[1];
          this.silkWormSpec[0].DataX = FrmGetSpecAuto.SpInfo.DataX;
          this.silkWormSpec[0].DataY = numArray;
          if (this.CBisEnergy.Checked)
            this.silkWormSpec[0].DataE = numArray;
          this.specTempNum = dictionary.Count;
          this.SpName = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
          this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\Mean-" + this.SpName + ".txt", this.silkWormSpec[0].DataX, this.silkWormSpec[0].DataY);
          this.AppendText(this.SpName + "-Mean");
          dictionary.Clear();
          DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
        }
      }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      this.ViewStylemy = !this.CBisEnergy.Checked ? FrmGetSpecAuto.ViewStyle.Spec : FrmGetSpecAuto.ViewStyle.Energy;
      this.Draw("光 谱 图", this.silkWormSpec, this.silkWormSpec.Length - 1);
    }

    private void MyChart_LoadData(double[] DataX, double[] DataY, Color ColorMy, string Name)
    {
      PointPairList pointPairList = new PointPairList(DataX, DataY);
      this.MyChart.GraphPane.AddCurve(Name, (IPointList) pointPairList, ColorMy, SymbolType.None);
      this.MyChart.GraphPane.XAxis.Scale.Min = this.DataHandlingmy.MinValue(DataX);
      this.MyChart.GraphPane.XAxis.Scale.Max = this.DataHandlingmy.MaxValue(DataX);
      this.MyChart.AxisChange();
      this.MyChart.Refresh();
    }

    private void Draw(string str, Spectrometer.Data[] DataGet, int Num)
    {
      if (DataGet != null && DataGet[Num].DataX != null)
      {
        if (this.ViewStylemy == FrmGetSpecAuto.ViewStyle.Energy && DataGet[Num].DataE != null)
        {
          RectangleF rect = this.MyChart.GraphPane.Rect;
          if (this.ViewStylemy == FrmGetSpecAuto.ViewStyle.Energy)
            this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "能量");
          this.MyChart.GraphPane.CurveList.Clear();
          for (int index = 0; index < Num + 1; ++index)
            this.MyChart_LoadData(DataGet[index].DataX, DataGet[index].DataE, this.DrawColor[index], this.SpName + "-Energy-" + (index + 1).ToString());
        }
        else
        {
          RectangleF rect = this.MyChart.GraphPane.Rect;
          if (this.ViewStylemy == FrmGetSpecAuto.ViewStyle.Spec)
          {
            this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "吸光度");
            this.MyChart.GraphPane.CurveList.Clear();
            int num;
            for (int index = 0; index < Num + 1; ++index)
            {
              if (index == Num && Num > 1)
              {
                if (str == "光 谱 图")
                {
                  this.MyChart_LoadData(DataGet[index].DataX, DataGet[index].DataY, Color.Red, "平均");
                }
                else
                {
                  double[] dataX = DataGet[index].DataX;
                  double[] dataY = DataGet[index].DataY;
                  Color ColorMy = this.DrawColor[index];
                  string spName = this.SpName;
                  string str1 = "-";
                  num = index + 1;
                  string str2 = num.ToString();
                  string Name = spName + str1 + str2;
                  this.MyChart_LoadData(dataX, dataY, ColorMy, Name);
                }
              }
              else
              {
                double[] dataX = DataGet[index].DataX;
                double[] dataY = DataGet[index].DataY;
                Color ColorMy = this.DrawColor[index];
                string spName = this.SpName;
                string str1 = "-";
                num = index + 1;
                string str2 = num.ToString();
                string Name = spName + str1 + str2;
                this.MyChart_LoadData(dataX, dataY, ColorMy, Name);
              }
            }
          }
          else if (this.ViewStylemy == FrmGetSpecAuto.ViewStyle.Mean && Num == 0)
          {
            this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "吸光度");
            this.MyChart.GraphPane.CurveList.Clear();
            for (int index = 0; index < Num + 1; ++index)
              this.MyChart.Invoke((Delegate) this.myChartLoadData, (object) DataGet[index].DataX, (object) DataGet[index].DataY, (object) this.DrawColor[index], (object) (this.SpName + "平均光谱"));
          }
          else if (this.ViewStylemy == FrmGetSpecAuto.ViewStyle.StdErr && Num == 0)
          {
            this.MyChart.GraphPane = new GraphPane(rect, str, "波数", " ");
            this.MyChart.GraphPane.CurveList.Clear();
            for (int index = 0; index < Num + 1; ++index)
              this.MyChart.Invoke((Delegate) this.myChartLoadData, (object) DataGet[index].DataX, (object) DataGet[index].DataY, (object) this.DrawColor[index], (object) (this.SpName + "标准差图"));
          }
        }
      }
      else
        this.MyChart.GraphPane.CurveList.Clear();
    }

    private double GetAngle(double[] a, double[] b)
    {
      Subspace subspace = new Subspace();
      int length = a.Length;
      double theta1 = 100.0;
      double[,] A = new double[length, 1];
      double[,] B = new double[length, 1];
      if (length == b.Length)
      {
        for (int index = 0; index < a.Length; ++index)
        {
          A[index, 0] = a[index];
          B[index, 0] = b[index];
        }
        subspace.GetSubspaceAngle(A, B, ref theta1);
      }
      return theta1;
    }

    private bool isWilm(double[] energyValue)
    {
      double num = 0.0;
      for (int index = 0; index < this.selectWaveIndexDiff.Length; ++index)
        num += energyValue[this.selectWaveIndexDiff[index]] - this.rubbery[this.selectWaveIndexDiff[index]];
      return num / (double) this.selectWaveIndexDiff.Length > FrmGetSpecSet.Default.ThresholdDiff;
    }

    private void AppendText(string msg)
    {
      if (this.listBoxGetSpec.InvokeRequired)
        this.listBoxGetSpec.Invoke((Delegate) new FrmGetSpecAuto.AddItemToListBoxDelegate(this.AppendText), (object) msg);
      else
        this.listBoxGetSpec.Items.Add((object) msg);
    }

    private void btnSetting_Click(object sender, EventArgs e)
    {
      int num = (int) new FrmSetting(this.MySpectrometer, (FrmGetSpec) null, this.frmHome).ShowDialog();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (this.backgroundWorker1.IsBusy)
        this.backgroundWorker1.CancelAsync();
      this.backgroundWorker1.Dispose();
      this.Close();
      this.Dispose();
    }

    private void GetBackGrd()
    {
      if (!Home.serialPortSetDevice.IsOpen)
      {
        try
        {
          Home.serialPortSetDevice.Open();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.ToString());
          return;
        }
      }
      byte[] buffer1 = new byte[9]
      {
        (byte) 170,
        (byte) 5,
        (byte) 4,
        (byte) 1,
        (byte) 1,
        (byte) 144,
        (byte) 32,
        (byte) 51,
        (byte) 85
      };
      Home.serialPortSetDevice.Write(buffer1, 0, buffer1.Length);
      Thread.Sleep(500);
      this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref FrmGetSpecAuto.SpInfo, true);
      FrmGetSpecAuto.SpInfo.DataAB = FrmGetSpecAuto.SpInfo.DataA;
      FrmGetSpecAuto.SpInfo.DataA = (double[]) null;
      this.DataIOmy.TXTSaveData(Environment.CurrentDirectory.ToString() + "\\background", FrmGetSpecAuto.SpInfo.DataX, FrmGetSpecAuto.SpInfo.DataAB);
      Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
      dataArray[0].DataX = FrmGetSpecAuto.SpInfo.WavelengthArray;
      dataArray[0].DataE = FrmGetSpecAuto.SpInfo.DataA;
      this.MySpectrometer.ReadDK(ref FrmGetSpecAuto.SpInfo);
      byte[] buffer2 = new byte[9]
      {
        (byte) 170,
        (byte) 5,
        (byte) 4,
        (byte) 2,
        (byte) 1,
        (byte) 144,
        (byte) 32,
        (byte) 51,
        (byte) 85
      };
      Home.serialPortSetDevice.Write(buffer2, 0, buffer2.Length);
      Thread.Sleep(500);
    }

    private void Report()
    {
      this.DataIOmy.TXTWriteIn(Environment.CurrentDirectory.ToString() + "\\Report.txt", string.Format("{0:yyyy-MM-dd   HH:mm:ss}", (object) DateTime.Now) + "品种：" + FrmGetSpecSet.Default.txtBreed.ToString() + "\r\n批次：" + FrmGetSpecSet.Default.txtBatch.ToString() + "\r\n生产季节：" + FrmGetSpecSet.Default.txtMadeSeason.ToString() + "\r\n雌：" + this.counterFemale.ToString() + "\r\n雄：" + this.counterMale.ToString() + "\r\n\r\n");
    }

    private void btnReferenceCupOn_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["ReferenceOn"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void btnReferenceCupOff_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["ReferenceOff"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void btnConveyorOn_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["ConveyorOn"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void btnConveyorOff_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["ConveyorOff"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void btnLightOn_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["LightOn"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void btnLightOff_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["LightOff"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void btnValve1On_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["Valve1On"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void btnSpectrometerOff_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["Valve2On"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void btnValveOff_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["ValveOff"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void trackBarSpeed_Scroll(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["ConveyorOff"];
        buffer[3] = (byte) this.trackBarSpeed.Value;
        Home.SPControlWord["ConveyorOn"] = buffer;
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(500);
        FrmGetSpecSet.Default.trackBarValue = this.trackBarSpeed.Value;
        FrmGetSpecSet.Default.Save();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void CBisEnergy_CheckedChanged(object sender, EventArgs e)
    {
      FrmGetSpecSet.Default.GetSpecAutoIsEnergy = this.CBisEnergy.Checked;
      FrmGetSpecSet.Default.Save();
    }

    private void btnBackGrd_Click(object sender, EventArgs e)
    {
      this.timer1.Enabled = false;
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer1 = Home.SPControlWord["LightOn"];
        Home.serialPortSetDevice.Write(buffer1, 0, buffer1.Length);
        Thread.Sleep(500);
        byte[] buffer2 = Home.SPControlWord["ReferenceOn"];
        Home.serialPortSetDevice.Write(buffer2, 0, buffer2.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
      this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref FrmGetSpecAuto.SpInfo, true);
      FrmGetSpecAuto.SpInfo.DataAB = FrmGetSpecAuto.SpInfo.DataA;
      FrmGetSpecAuto.SpInfo.DataA = (double[]) null;
      this.DataIOmy.TXTSaveData(Environment.CurrentDirectory.ToString() + "\\background", FrmGetSpecAuto.SpInfo.DataX, FrmGetSpecAuto.SpInfo.DataAB);
      Spectrometer.Data[] DataGet = new Spectrometer.Data[1];
      DataGet[0].DataX = FrmGetSpecAuto.SpInfo.WavelengthArray;
      DataGet[0].DataE = FrmGetSpecAuto.SpInfo.DataA;
      this.ViewStylemy = FrmGetSpecAuto.ViewStyle.Energy;
      this.BeginInvoke((Delegate) new FrmGetSpecAuto.DrawDelegate(this.Draw), (object) "能量图", (object) DataGet, (object) 0);
      this.Draw("背景能量图", DataGet, 0);
      this.MySpectrometer.ReadDK(ref FrmGetSpecAuto.SpInfo);
      int num1 = (int) MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer = Home.SPControlWord["ReferenceOff"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show(ex.ToString());
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmGetSpecAuto));
      this.panelHeader = new Panel();
      this.btnClose = new Button();
      this.btnSetting = new Button();
      this.lblConnectState = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.txtTimeCount = new TextBox();
      this.txtAngleThreshold = new TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.trackBarSpeed = new TrackBar();
      this.backgroundWorker1 = new BackgroundWorker();
      this.panel3 = new Panel();
      this.CBisEnergy = new CheckBox();
      this.btnBackGrd = new Button();
      this.btnValveOff = new Button();
      this.btnStop = new Button();
      this.btnConveyorOn = new Button();
      this.btnGetSpec = new Button();
      this.label18 = new System.Windows.Forms.Label();
      this.btnSpectrometerOff = new Button();
      this.btnValve1On = new Button();
      this.label17 = new System.Windows.Forms.Label();
      this.btnConveyorOff = new Button();
      this.btnReferenceCupOff = new Button();
      this.btnReferenceCupOn = new Button();
      this.label15 = new System.Windows.Forms.Label();
      this.btnLightOff = new Button();
      this.btnLightOn = new Button();
      this.label16 = new System.Windows.Forms.Label();
      this.MyChart = new ZedGraphControl();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.listBoxGetSpec = new ListBox();
      this.panelHeader.SuspendLayout();
      this.trackBarSpeed.BeginInit();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.panelHeader.BackColor = Color.FromArgb(38, 136, 210);
      this.panelHeader.Controls.Add((Control) this.btnClose);
      this.panelHeader.Controls.Add((Control) this.btnSetting);
      this.panelHeader.Controls.Add((Control) this.lblConnectState);
      this.panelHeader.Location = new Point(0, 0);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(768, 45);
      this.panelHeader.TabIndex = 28;
      this.btnClose.Anchor = AnchorStyles.Right;
      this.btnClose.BackColor = Color.Transparent;
      this.btnClose.BackgroundImage = (Image) Resources.Home;
      this.btnClose.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnClose.FlatStyle = FlatStyle.Flat;
      this.btnClose.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnClose.Location = new Point(721, 4);
      this.btnClose.Margin = new Padding(4);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(47, 44);
      this.btnClose.TabIndex = 67;
      this.btnClose.UseVisualStyleBackColor = false;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnSetting.Anchor = AnchorStyles.Right;
      this.btnSetting.BackColor = Color.Transparent;
      this.btnSetting.BackgroundImage = (Image) Resources.Set;
      this.btnSetting.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnSetting.FlatStyle = FlatStyle.Flat;
      this.btnSetting.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnSetting.Location = new Point(679, 6);
      this.btnSetting.Margin = new Padding(4);
      this.btnSetting.Name = "btnSetting";
      this.btnSetting.Size = new Size(42, 39);
      this.btnSetting.TabIndex = 63;
      this.btnSetting.UseVisualStyleBackColor = false;
      this.btnSetting.Click += new EventHandler(this.btnSetting_Click);
      this.lblConnectState.AutoSize = true;
      this.lblConnectState.Font = new Font("宋体", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.lblConnectState.ForeColor = Color.White;
      this.lblConnectState.Location = new Point(10, 7);
      this.lblConnectState.Name = "lblConnectState";
      this.lblConnectState.Size = new Size(42, 27);
      this.lblConnectState.TabIndex = 11;
      this.lblConnectState.Text = "--";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(40, 268);
      this.label7.Margin = new Padding(2, 0, 2, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(65, 12);
      this.label7.TabIndex = 55;
      this.label7.Text = "消耗时间：";
      this.txtTimeCount.Location = new Point(106, 266);
      this.txtTimeCount.Margin = new Padding(2);
      this.txtTimeCount.Name = "txtTimeCount";
      this.txtTimeCount.Size = new Size(78, 21);
      this.txtTimeCount.TabIndex = 54;
      this.txtAngleThreshold.Location = new Point(134, 173);
      this.txtAngleThreshold.Margin = new Padding(2);
      this.txtAngleThreshold.Name = "txtAngleThreshold";
      this.txtAngleThreshold.Size = new Size(52, 21);
      this.txtAngleThreshold.TabIndex = 53;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(70, 176);
      this.label1.Margin = new Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(65, 12);
      this.label1.TabIndex = 52;
      this.label1.Text = "夹角阈值：";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(179, 213);
      this.label3.Margin = new Padding(2, 0, 2, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(17, 12);
      this.label3.TabIndex = 45;
      this.label3.Text = "快";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(65, 213);
      this.label2.Margin = new Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(17, 12);
      this.label2.TabIndex = 44;
      this.label2.Text = "慢";
      this.trackBarSpeed.Location = new Point(82, 210);
      this.trackBarSpeed.Margin = new Padding(2);
      this.trackBarSpeed.Name = "trackBarSpeed";
      this.trackBarSpeed.Size = new Size(103, 45);
      this.trackBarSpeed.TabIndex = 43;
      this.trackBarSpeed.Value = 5;
      this.trackBarSpeed.Scroll += new EventHandler(this.trackBarSpeed_Scroll);
      this.backgroundWorker1.WorkerReportsProgress = true;
      this.backgroundWorker1.WorkerSupportsCancellation = true;
      this.panel3.BackColor = Color.WhiteSmoke;
      this.panel3.Controls.Add((Control) this.CBisEnergy);
      this.panel3.Controls.Add((Control) this.btnBackGrd);
      this.panel3.Controls.Add((Control) this.btnValveOff);
      this.panel3.Controls.Add((Control) this.btnStop);
      this.panel3.Controls.Add((Control) this.btnConveyorOn);
      this.panel3.Controls.Add((Control) this.btnGetSpec);
      this.panel3.Controls.Add((Control) this.label18);
      this.panel3.Controls.Add((Control) this.btnSpectrometerOff);
      this.panel3.Controls.Add((Control) this.btnValve1On);
      this.panel3.Controls.Add((Control) this.label17);
      this.panel3.Controls.Add((Control) this.btnConveyorOff);
      this.panel3.Controls.Add((Control) this.btnReferenceCupOff);
      this.panel3.Controls.Add((Control) this.btnReferenceCupOn);
      this.panel3.Controls.Add((Control) this.label15);
      this.panel3.Controls.Add((Control) this.btnLightOff);
      this.panel3.Controls.Add((Control) this.btnLightOn);
      this.panel3.Controls.Add((Control) this.label16);
      this.panel3.Controls.Add((Control) this.label7);
      this.panel3.Controls.Add((Control) this.txtTimeCount);
      this.panel3.Controls.Add((Control) this.txtAngleThreshold);
      this.panel3.Controls.Add((Control) this.label1);
      this.panel3.Controls.Add((Control) this.label3);
      this.panel3.Controls.Add((Control) this.label2);
      this.panel3.Controls.Add((Control) this.trackBarSpeed);
      this.panel3.Location = new Point(538, 46);
      this.panel3.Margin = new Padding(2);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(230, 569);
      this.panel3.TabIndex = 62;
      this.CBisEnergy.AutoSize = true;
      this.CBisEnergy.Location = new Point(82, 140);
      this.CBisEnergy.Margin = new Padding(2);
      this.CBisEnergy.Name = "CBisEnergy";
      this.CBisEnergy.Size = new Size(96, 16);
      this.CBisEnergy.TabIndex = 76;
      this.CBisEnergy.Text = "是否为能量值";
      this.CBisEnergy.UseVisualStyleBackColor = true;
      this.CBisEnergy.CheckedChanged += new EventHandler(this.CBisEnergy_CheckedChanged);
      this.btnBackGrd.Anchor = AnchorStyles.Right;
      this.btnBackGrd.FlatStyle = FlatStyle.Flat;
      this.btnBackGrd.Location = new Point(58, 298);
      this.btnBackGrd.Margin = new Padding(4);
      this.btnBackGrd.Name = "btnBackGrd";
      this.btnBackGrd.Size = new Size(128, 25);
      this.btnBackGrd.TabIndex = 75;
      this.btnBackGrd.Text = "背景";
      this.btnBackGrd.UseVisualStyleBackColor = true;
      this.btnBackGrd.Click += new EventHandler(this.btnBackGrd_Click);
      this.btnValveOff.Location = new Point((int) sbyte.MaxValue, 526);
      this.btnValveOff.Name = "btnValveOff";
      this.btnValveOff.Size = new Size(35, 39);
      this.btnValveOff.TabIndex = 74;
      this.btnValveOff.Text = "关";
      this.btnValveOff.UseVisualStyleBackColor = true;
      this.btnValveOff.Click += new EventHandler(this.btnValveOff_Click);
      this.btnStop.BackColor = Color.Transparent;
      this.btnStop.BackgroundImage = (Image) Resources.Pause;
      this.btnStop.BackgroundImageLayout = ImageLayout.Stretch;
      this.btnStop.Enabled = false;
      this.btnStop.FlatStyle = FlatStyle.Flat;
      this.btnStop.ForeColor = Color.Transparent;
      this.btnStop.Location = new Point(137, 37);
      this.btnStop.Margin = new Padding(0);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(58, 64);
      this.btnStop.TabIndex = 42;
      this.btnStop.UseVisualStyleBackColor = false;
      this.btnStop.Click += new EventHandler(this.btnStop_Click);
      this.btnConveyorOn.Location = new Point(109, 392);
      this.btnConveyorOn.Name = "btnConveyorOn";
      this.btnConveyorOn.Size = new Size(35, 39);
      this.btnConveyorOn.TabIndex = 68;
      this.btnConveyorOn.Tag = (object) " ";
      this.btnConveyorOn.Text = "启";
      this.btnConveyorOn.UseVisualStyleBackColor = true;
      this.btnConveyorOn.Click += new EventHandler(this.btnConveyorOn_Click);
      this.btnGetSpec.Anchor = AnchorStyles.Right;
      this.btnGetSpec.BackColor = Color.Transparent;
      this.btnGetSpec.BackgroundImage = (Image) Resources.Play2;
      this.btnGetSpec.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnGetSpec.FlatStyle = FlatStyle.Flat;
      this.btnGetSpec.ForeColor = Color.Transparent;
      this.btnGetSpec.Location = new Point(40, 37);
      this.btnGetSpec.Margin = new Padding(4);
      this.btnGetSpec.Name = "btnGetSpec";
      this.btnGetSpec.Size = new Size(62, 64);
      this.btnGetSpec.TabIndex = 39;
      this.btnGetSpec.UseVisualStyleBackColor = false;
      this.btnGetSpec.Click += new EventHandler(this.btnGetSpec_Click);
      this.label18.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label18.Location = new Point(37, 492);
      this.label18.Name = "label18";
      this.label18.Size = new Size(66, 33);
      this.label18.TabIndex = 73;
      this.label18.Text = "阀门：";
      this.btnSpectrometerOff.Location = new Point(146, 484);
      this.btnSpectrometerOff.Name = "btnSpectrometerOff";
      this.btnSpectrometerOff.Size = new Size(35, 39);
      this.btnSpectrometerOff.TabIndex = 72;
      this.btnSpectrometerOff.Text = "2号开";
      this.btnSpectrometerOff.UseVisualStyleBackColor = true;
      this.btnSpectrometerOff.Click += new EventHandler(this.btnSpectrometerOff_Click);
      this.btnValve1On.Location = new Point(109, 484);
      this.btnValve1On.Name = "btnValve1On";
      this.btnValve1On.Size = new Size(35, 39);
      this.btnValve1On.TabIndex = 71;
      this.btnValve1On.Tag = (object) " ";
      this.btnValve1On.Text = "1号开";
      this.btnValve1On.UseVisualStyleBackColor = true;
      this.btnValve1On.Click += new EventHandler(this.btnValve1On_Click);
      this.label17.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label17.Location = new Point(29, 400);
      this.label17.Name = "label17";
      this.label17.Size = new Size(91, 33);
      this.label17.TabIndex = 70;
      this.label17.Text = "传送带：";
      this.btnConveyorOff.Location = new Point(146, 392);
      this.btnConveyorOff.Name = "btnConveyorOff";
      this.btnConveyorOff.Size = new Size(35, 39);
      this.btnConveyorOff.TabIndex = 69;
      this.btnConveyorOff.Text = "停";
      this.btnConveyorOff.UseVisualStyleBackColor = true;
      this.btnConveyorOff.Click += new EventHandler(this.btnConveyorOff_Click);
      this.btnReferenceCupOff.Location = new Point(146, 346);
      this.btnReferenceCupOff.Name = "btnReferenceCupOff";
      this.btnReferenceCupOff.Size = new Size(35, 39);
      this.btnReferenceCupOff.TabIndex = 66;
      this.btnReferenceCupOff.Text = "回";
      this.btnReferenceCupOff.UseVisualStyleBackColor = true;
      this.btnReferenceCupOff.Click += new EventHandler(this.btnReferenceCupOff_Click);
      this.btnReferenceCupOn.Location = new Point(109, 346);
      this.btnReferenceCupOn.Name = "btnReferenceCupOn";
      this.btnReferenceCupOn.Size = new Size(35, 39);
      this.btnReferenceCupOn.TabIndex = 65;
      this.btnReferenceCupOn.Tag = (object) " ";
      this.btnReferenceCupOn.Text = "出";
      this.btnReferenceCupOn.UseVisualStyleBackColor = true;
      this.btnReferenceCupOn.Click += new EventHandler(this.btnReferenceCupOn_Click);
      this.label15.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label15.Location = new Point(40, 448);
      this.label15.Name = "label15";
      this.label15.Size = new Size(66, 33);
      this.label15.TabIndex = 64;
      this.label15.Text = "灯光：";
      this.btnLightOff.Location = new Point(146, 440);
      this.btnLightOff.Name = "btnLightOff";
      this.btnLightOff.Size = new Size(35, 39);
      this.btnLightOff.TabIndex = 63;
      this.btnLightOff.Text = "关";
      this.btnLightOff.UseVisualStyleBackColor = true;
      this.btnLightOff.Click += new EventHandler(this.btnLightOff_Click);
      this.btnLightOn.Location = new Point(110, 440);
      this.btnLightOn.Name = "btnLightOn";
      this.btnLightOn.Size = new Size(35, 39);
      this.btnLightOn.TabIndex = 62;
      this.btnLightOn.Tag = (object) " ";
      this.btnLightOn.Text = "开";
      this.btnLightOn.UseVisualStyleBackColor = true;
      this.btnLightOn.Click += new EventHandler(this.btnLightOn_Click);
      this.label16.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label16.Location = new Point(28, 354);
      this.label16.Name = "label16";
      this.label16.Size = new Size(87, 33);
      this.label16.TabIndex = 67;
      this.label16.Text = "参比杯：";
      this.MyChart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.MyChart.Location = new Point(0, 43);
      this.MyChart.Margin = new Padding(4, 3, 4, 3);
      this.MyChart.Name = "MyChart";
      this.MyChart.ScrollGrace = 0.0;
      this.MyChart.ScrollMaxX = 0.0;
      this.MyChart.ScrollMaxY = 0.0;
      this.MyChart.ScrollMaxY2 = 0.0;
      this.MyChart.ScrollMinX = 0.0;
      this.MyChart.ScrollMinY = 0.0;
      this.MyChart.ScrollMinY2 = 0.0;
      this.MyChart.Size = new Size(371, 572);
      this.MyChart.TabIndex = 63;
      this.timer1.Interval = 500;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.listBoxGetSpec.FormattingEnabled = true;
      this.listBoxGetSpec.ItemHeight = 12;
      this.listBoxGetSpec.Location = new Point(373, 47);
      this.listBoxGetSpec.Name = "listBoxGetSpec";
      this.listBoxGetSpec.Size = new Size(165, 568);
      this.listBoxGetSpec.TabIndex = 64;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(768, 614);
      this.Controls.Add((Control) this.listBoxGetSpec);
      this.Controls.Add((Control) this.MyChart);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panelHeader);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (FrmGetSpecAuto);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "自动采集窗口";
      this.Load += new EventHandler(this.FrmGetSpecAuto_Load);
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.trackBarSpeed.EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.ResumeLayout(false);
    }

    private enum ViewStyle
    {
      Mean,
      StdErr,
      Spec,
      Energy,
    }

    private delegate void DrawDelegate(string Str, Spectrometer.Data[] DataGet, int Num);

    private delegate void AddItemToListBoxDelegate(string str);

    private delegate void MyChartLoadData(double[] DataX, double[] DataY, Color ColorMy, string Name);
  }
}
