// Decompiled with JetBrains decompiler
// Type: JSDU.Home
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
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace JSDU
{
  public class Home : Form
  {
    public static SerialPort serialPortSetDevice = new SerialPort();
    private static bool startFlag = false;
    public static Dictionary<string, byte[]> SPControlWord = new Dictionary<string, byte[]>();
    private Spectrometer MySpectrometer = new Spectrometer();
    private DataIO DataIOmy = new DataIO();
    private DataHandling DataHandlingmy = new DataHandling();
    private Spectrometer.Data[] specData = new Spectrometer.Data[1];
    private Home.ViewStyle ViewStylemy = Home.ViewStyle.Spec;
    private Color[] DrawColor = new Color[20];
    private int counterFemale = 0;
    private int counterMale = 0;
    private bool puffMale = false;
    private bool puffFemale = false;
    private bool silkWormStart = false;
    private long timestart = 0;
    private long time1 = 0;
    private int[] selectWaveIndexDiff = (int[]) null;
    private int isMale = 0;
    private SimcaPrd SimcaPre = new SimcaPrd();
    private long timeGetSp = 0;
    private double[,] tempMeanss = (double[,]) null;
    private bool tPuffFlag = false;
    private IContainer components = (IContainer) null;
    public const byte FT_PURGE_RX = 1;
    public const byte FT_PURGE_TX = 2;
    private int numberOfSpectrometersFound;
    public static Spectrometer.SpecInfo SpInfo;
    private double[,] MeanY;
    private double[] rubbery;
    private double[,] model;
    private int anglethreshold;
    private Panel panelHeader;
    private Button btnStop;
    private Panel panel2;
    private Label label10;
    private Label lbMale;
    private Panel panel1;
    private Label label9;
    private Label lbFemale;
    private Button btnGetSpec;
    private Label lblConnectState;
    private Label label7;
    private TextBox txtTimeCount;
    private TextBox txtAngleThreshold;
    private Label label1;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private TrackBar trackBarSpeed;
    private BackgroundWorker backgroundWorker1;
    private TextBox txtBreed;
    private Label label8;
    private TextBox txtMadeSeason;
    private Label label11;
    private TextBox txtBatch;
    private Label label12;
    private Panel panel3;
    private Button btnClose;
    private Button btnSetting;
    private Button btnGetSpectrum;
    private ComboBox comboxSerialPort;
    private Button btnConveyorOn;
    private Label label18;
    private Button btnSpectrometerOff;
    private Button btnValve1On;
    private Label label17;
    private Button btnConveyorOff;
    private Button btnReferenceCupOff;
    private Button btnReferenceCupOn;
    private Label label15;
    private Button btnLightOff;
    private Button btnLightOn;
    private Label label16;
    private Button btnValveOff;
    private Button btnGetSpectrumAuto;
    private ToolTip toolTip1;
    private ToolTip toolTip2;
    private ToolTip toolTip3;
    private ToolTip toolTip4;
    private ToolTip toolTip5;
    private ToolTip toolTip6;
    private TextBox txtBoxPuffIntervalTime;
    private Panel panel4;
    private Button btnAdmin;
    private Button btnBack;
    private Button btnCounterReset;
    private CheckBox cBIsSaveSpec;
    private BackgroundWorker backgroundWorker2;
    public System.Windows.Forms.Timer timerClearSensor;

    public Home()
    {
      this.InitializeComponent();
      Home.SPControlWord.Add("ConveyorOn", new byte[10]
      {
        (byte) 83,
        (byte) 69,
        (byte) 80,
        (byte) 65,
        (byte) 84,
        (byte) 50,
        (byte) 48,
        (byte) 48,
        (byte) 49,
        (byte) 35
      });
      Home.SPControlWord.Add("ConveyorOff", new byte[10]
      {
        (byte) 83,
        (byte) 69,
        (byte) 80,
        (byte) 65,
        (byte) 84,
        (byte) 50,
        (byte) 48,
        (byte) 48,
        (byte) 49,
        (byte) 35
      });
      Home.SPControlWord.Add("ReferenceOn", new byte[10]
      {
        (byte) 83,
        (byte) 69,
        (byte) 80,
        (byte) 65,
        (byte) 84,
        (byte) 49,
        (byte) 48,
        (byte) 48,
        (byte) 49,
        (byte) 35
      });
      Home.SPControlWord.Add("ReferenceOff", new byte[10]
      {
        (byte) 83,
        (byte) 69,
        (byte) 80,
        (byte) 65,
        (byte) 84,
        (byte) 50,
        (byte) 48,
        (byte) 48,
        (byte) 49,
        (byte) 35
      });
      Home.SPControlWord.Add("Valve1On", new byte[10]
      {
        (byte) 83,
        (byte) 69,
        (byte) 80,
        (byte) 65,
        (byte) 84,
        (byte) 51,
        (byte) 48,
        (byte) 48,
        (byte) 49,
        (byte) 35
      });
      Home.SPControlWord.Add("Valve2On", new byte[0]);
      Home.SPControlWord.Add("ValveSkip", new byte[10]
      {
        (byte) 83,
        (byte) 69,
        (byte) 80,
        (byte) 65,
        (byte) 84,
        (byte) 51,
        (byte) 48,
        (byte) 48,
        (byte) 50,
        (byte) 35
      });
      Home.SPControlWord.Add("LightOn", new byte[0]);
      Home.SPControlWord.Add("LightOff", new byte[0]);
      Home.SPControlWord.Add("ClearSensorOn", new byte[10]
      {
        (byte) 83,
        (byte) 69,
        (byte) 80,
        (byte) 65,
        (byte) 84,
        (byte) 52,
        (byte) 48,
        (byte) 48,
        (byte) 49,
        (byte) 35
      });
      Home.SPControlWord.Add("ClearSensorOff", new byte[10]
      {
        (byte) 83,
        (byte) 69,
        (byte) 80,
        (byte) 65,
        (byte) 84,
        (byte) 52,
        (byte) 48,
        (byte) 48,
        (byte) 50,
        (byte) 35
      });
    }

    private void Home_Load(object sender, EventArgs e)
    {
      this.Size = new Size(720, 768);
      this.panelHeader.Location = new Point(0, 0);
      this.panelHeader.Size = new Size(719, 56);
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
        this.btnGetSpectrum.Enabled = false;
        this.btnGetSpectrumAuto.Enabled = false;
      }
      try
      {
        foreach (object portName in SerialPort.GetPortNames())
          this.comboxSerialPort.Items.Add((object) portName.ToString());
        this.comboxSerialPort.SelectedIndex = 0;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("获取计算机COM口列表失败!\r\n错误信息:" + ex.Message);
      }
      this.MySpectrometer.ReadBK(ref Home.SpInfo);
      this.MySpectrometer.ReadSetParameters();
      Spectrometer.IntegrationTimeBK = FrmGetSpecSet.Default.IntegrationTimeBK;
      this.trackBarSpeed.Value = FrmGetSpecSet.Default.trackBarSpeed;
      this.txtBoxPuffIntervalTime.Text = FrmGetSpecSet.Default.puffIntervalTime.ToString();
      this.anglethreshold = FrmGetSpecSet.Default.anglethreshold;
      this.txtAngleThreshold.Text = this.anglethreshold.ToString();
      if (Spectrometer.isClearDarks)
        this.MySpectrometer.ReadDK(ref Home.SpInfo);
      double[] Data_x1 = (double[]) null;
      int length = this.DataIOmy.TXTReadData(Environment.CurrentDirectory.ToString() + "\\conveyor.txt", ref Data_x1, ref this.rubbery, true);
      double[] Data_x2 = new double[length];
      this.rubbery = new double[length];
      this.DataIOmy.TXTReadData(Environment.CurrentDirectory.ToString() + "\\conveyor.txt", ref Data_x2, ref this.rubbery, false);
      this.DataIOmy.TXTReadDatas(Environment.CurrentDirectory.ToString() + "\\Model\\SIMCA\\model.txt", out this.model);
      this.txtBatch.Text = FrmGetSpecSet.Default.txtBatch;
      this.txtBreed.Text = FrmGetSpecSet.Default.txtBreed;
      this.txtMadeSeason.Text = FrmGetSpecSet.Default.txtMadeSeason;
      this.counterFemale = FrmGetSpecSet.Default.counterFemale;
      this.counterMale = FrmGetSpecSet.Default.counterMale;
      this.cBIsSaveSpec.Checked = FrmGetSpecSet.Default.isSaveSpecAtDistinguish;
      this.timerClearSensor.Interval = FrmGetSpecSet.Default.clearSensorIntervalTimes;
      this.lbFemale.Text = this.counterFemale.ToString();
      this.lbMale.Text = this.counterMale.ToString();
      this.trackBarSpeed.Value = FrmGetSpecSet.Default.trackBarValue;
      this.toolTip1.SetToolTip((Control) this.btnGetSpectrumAuto, "自动采谱");
      this.toolTip2.SetToolTip((Control) this.btnGetSpectrum, "手动采谱");
      this.toolTip3.SetToolTip((Control) this.btnSetting, "设置");
      this.toolTip4.SetToolTip((Control) this.btnGetSpec, "开始雌雄分拣");
      this.toolTip5.SetToolTip((Control) this.btnStop, "暂停分拣");
      this.toolTip6.SetToolTip((Control) this.btnClose, "退出");
      string[] strArray = FrmGetSpecSet.Default.WavelengthDiffIndex.Substring(0, FrmGetSpecSet.Default.WavelengthDiffIndex.Length - 1).Split(',');
      this.selectWaveIndexDiff = new int[strArray.Length];
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
      this.btnStop.Enabled = true;
      this.btnGetSpec.BackColor = Color.FromArgb(38, 136, 210);
      this.anglethreshold = int.Parse(this.txtAngleThreshold.Text);
      FrmGetSpecSet.Default.anglethreshold = this.anglethreshold;
      if (FrmGetSpecSet.Default.txtBreed != this.txtBreed.Text.ToString() || FrmGetSpecSet.Default.txtBatch != this.txtBatch.Text.ToString() || FrmGetSpecSet.Default.txtMadeSeason != this.txtMadeSeason.Text.ToString())
      {
        this.counterFemale = 0;
        this.counterMale = 0;
      }
      FrmGetSpecSet.Default.txtBreed = this.txtBreed.Text.ToString();
      FrmGetSpecSet.Default.txtBatch = this.txtBatch.Text.ToString();
      FrmGetSpecSet.Default.txtMadeSeason = this.txtMadeSeason.Text.ToString();
      FrmGetSpecSet.Default.puffIntervalTime = int.Parse(this.txtBoxPuffIntervalTime.Text);
      FrmGetSpecSet.Default.Save();
      Home.startFlag = true;
      Spectrometer.DataGet = new Spectrometer.Data[20];
      this.MySpectrometer.wrapper.openAllSpectrometers();
      if (!FrmGetSpecSet.Default.GetSpecAutoIsEnergy)
        this.GetBackGrd();
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] numArray = Home.SPControlWord["ConveyorOn"];
        byte[] buffer = Home.SPControlWord["LightOn"];
        Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
        Thread.Sleep(10);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
      this.time1 = DateTime.Now.Ticks;
      this.timerClearSensor.Enabled = true;
      if (!this.backgroundWorker1.IsBusy)
      {
        this.backgroundWorker1.RunWorkerAsync();
      }
      else
      {
        int num1 = (int) MessageBox.Show("正在检测，请勿重复点击，谢谢！");
      }
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
      this.btnGetSpec.BackColor = Color.Transparent;
      Home.startFlag = false;
      this.btnGetSpec.Enabled = true;
      this.btnStop.Enabled = false;
      this.timerClearSensor.Enabled = true;
      FrmGetSpecSet.Default.counterFemale = this.counterFemale;
      FrmGetSpecSet.Default.counterMale = this.counterMale;
      FrmGetSpecSet.Default.Save();
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker backgroundWorker = sender as BackgroundWorker;
      Dictionary<string, double[]> dictionary = new Dictionary<string, double[]>();
      double[,] Data = (double[,]) null;
      this.DataIOmy.TXTReadDatas(Environment.CurrentDirectory.ToString() + "\\Model\\SIMCA\\model.txt", out Data);
      Thread thread = new Thread(new ParameterizedThreadStart(this.puff));
      while (Home.startFlag)
      {
        this.timestart = DateTime.Now.Ticks;
        if (this.timestart - this.time1 > 36000000000L)
        {
          this.GetBackGrd();
          this.Report();
          this.time1 = this.timestart = DateTime.Now.Ticks;
        }
        this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref Home.SpInfo, false);
        if (!FrmGetSpecSet.Default.GetSpecAutoIsEnergy)
        {
          if (Home.SpInfo.DataAB.Length != Home.SpInfo.DataA.Length)
            throw new ArgumentException("光谱维数不符");
          Home.SpInfo.DataY = new double[Home.SpInfo.numPixls];
          for (int index = 0; index < Home.SpInfo.DataA.Length; ++index)
          {
            try
            {
              Home.SpInfo.DataY[index] = Convert.ToDouble(Math.Log10(Math.Abs((Home.SpInfo.DataAB[index] - Home.SpInfo.DataAD[index]) / (Home.SpInfo.DataA[index] - Home.SpInfo.DataAD[index]))));
            }
            catch
            {
              Home.SpInfo.DataY[index] = index <= 0 ? 0.0 : Home.SpInfo.DataY[index - 1];
            }
          }
        }
        else
        {
          Home.SpInfo.DataY = new double[Home.SpInfo.DataA.Length];
          Home.SpInfo.DataA.CopyTo((Array) Home.SpInfo.DataY, 0);
        }
        if (this.isWilm(Home.SpInfo.DataA))
        {
          this.silkWormStart = true;
          double[] numArray = new double[Home.SpInfo.DataY.Length];
          for (int index = 0; index < Home.SpInfo.DataY.Length; ++index)
            numArray[index] = Home.SpInfo.DataY[index];
          string key = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
          if (!dictionary.ContainsKey(key))
            dictionary.Add(key, numArray);
        }
        else if (this.silkWormStart)
        {
          this.silkWormStart = false;
          int num1 = 0;
          double[,] X = new double[dictionary.Count - 2 * num1, Home.SpInfo.numPixls];
          int num2 = 0;
          int index1 = 0;
          foreach (double[] numArray in dictionary.Values)
          {
            ++num2;
            for (int index2 = 0; index2 < Home.SpInfo.numPixls; ++index2)
              X[index1, index2] = numArray[index2];
            ++index1;
          }
          DateTime now = DateTime.Now;
          now.ToString("yyyy-MM-dd hh-mm-ss-fff");
          double[] Data_y = this.DataHandlingmy.SpMean(X);
          double[,] numArray1 = new double[1, Data_y.Length];
          for (int index2 = 0; index2 < Data_y.Length; ++index2)
            numArray1[0, index2] = Data_y[index2];
          ripsPreDeal ripsPreDeal = new ripsPreDeal(1, Data_y.Length);
          int anglethreshold = FrmGetSpecSet.Default.anglethreshold;
          this.tempMeanss = new double[1, Data_y.Length / anglethreshold];
          for (int index2 = 0; index2 < Data_y.Length / anglethreshold; ++index2)
            this.tempMeanss[0, index2] = Data_y[(index2 + 1) * anglethreshold - 1];
          now = DateTime.Now;
          this.timeGetSp = now.Ticks;
          int num3 = 0;
          if (!this.backgroundWorker2.IsBusy)
          {
            this.backgroundWorker2.RunWorkerAsync();
          }
          else
          {
            Thread.Sleep(2);
            if (num3 + 1 > 15)
            {
              this.tPuffFlag = false;
              this.backgroundWorker2.CancelAsync();
            }
          }
          string str = "";
          foreach (KeyValuePair<string, double[]> keyValuePair in dictionary)
          {
            this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\" + keyValuePair.Key.ToString() + ".txt", Home.SpInfo.DataX, keyValuePair.Value);
            str = keyValuePair.Key.ToString();
          }
          if (this.cBIsSaveSpec.Checked)
            this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\" + str + "-mean.txt", Home.SpInfo.DataX, Data_y);
          dictionary.Clear();
        }
      }
    }

    private bool isWilm(double[] energyValue)
    {
      double num = 0.0;
      for (int index = 0; index < this.selectWaveIndexDiff.Length; ++index)
        num += energyValue[this.selectWaveIndexDiff[index]] - this.rubbery[this.selectWaveIndexDiff[index]];
      return num / (double) this.selectWaveIndexDiff.Length > FrmGetSpecSet.Default.ThresholdDiff;
    }

    private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
    {
      this.Calculate();
    }

    private void Calculate()
    {
      this.tPuffFlag = true;
      this.DataIOmy.TXTWriteIn(Directory.GetCurrentDirectory() + "\\parameterSimcaPrdtlog.txt", " PuffIn\r\n");
      string str1 = "ValveSkip";
      string[] parameterOut = (string[]) null;
      int[] numArray = new int[2]{ 1, 1 };
      try
      {
        numArray = this.SimcaPre.SimcaPrdt(this.tempMeanss, this.model, out parameterOut);
      }
      catch (Exception ex)
      {
        this.DataIOmy.TXTWriteIn(Directory.GetCurrentDirectory() + "\\parameterSimcaPrdtlog.txt", " CalcuEnd" + ex.ToString() + "\r\n");
      }
      DateTime now = DateTime.Now;
      long ticks = now.Ticks;
      if (numArray[0] == 1)
        this.puffMale = true;
      else if (numArray[0] == 0)
        this.puffFemale = true;
      if (this.puffMale)
      {
        try
        {
          str1 = "Valve1On";
          this.isMale = 1;
          this.counterMale = this.counterMale + 1;
          this.Invoke((Delegate) ((param0, param1) => this.lbMale.Text = this.counterMale.ToString()));
          this.puffMale = false;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.ToString());
        }
      }
      else if (this.puffFemale)
      {
        try
        {
          str1 = "Valve2On";
          ++this.counterFemale;
          this.Invoke((Delegate) ((param0, param1) => this.lbFemale.Text = this.counterFemale.ToString()));
          this.puffFemale = false;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.ToString());
        }
      }
      this.puffFemale = false;
      this.puffMale = false;
      string[] strArray1;
      string[] strArray2 = strArray1 = parameterOut;
      int index = 0;
      string str2 = strArray2[0];
      string str3 = "  ";
      now = DateTime.Now;
      string str4 = now.ToString("yyyy-MM-dd hh-mm-ss-fff").ToString();
      string str5 = "-mean.txt\r\n";
      string str6 = str2 + str3 + str4 + str5;
      strArray1[index] = str6;
      this.DataIOmy.TXTWriteIn(Directory.GetCurrentDirectory() + "\\parameterSimcaPrdtlog.txt", parameterOut[0]);
      Thread.Sleep(10);
      new Thread(new ParameterizedThreadStart(this.puff))
      {
        IsBackground = true,
        Priority = ThreadPriority.Highest
      }.Start((object) str1);
    }

    private void puff(object obj)
    {
      string index = (string) obj;
      Thread.Sleep(FrmGetSpecSet.Default.puffIntervalTime);
      if (!Home.serialPortSetDevice.IsOpen)
        Home.serialPortSetDevice.Open();
      byte[] buffer1 = Home.SPControlWord[index];
      Home.serialPortSetDevice.Write(buffer1, 0, buffer1.Length);
      Thread.Sleep(50);
      byte[] buffer2 = Home.SPControlWord["ValveSkip"];
      Home.serialPortSetDevice.Write(buffer2, 0, buffer2.Length);
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.MySpectrometer.wrapper.closeAllSpectrometers();
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

    private void btnSetting_Click(object sender, EventArgs e)
    {
      int num = (int) new FrmSetting(this.MySpectrometer, (FrmGetSpec) null, this).ShowDialog();
    }

    private void btnGetSpectrum_Click(object sender, EventArgs e)
    {
      Home.serialPortSetDevice.Close();
      new FrmGetSpec(this).Show();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (this.backgroundWorker1.IsBusy)
        this.backgroundWorker1.CancelAsync();
      this.backgroundWorker1.Dispose();
      if (Home.serialPortSetDevice.IsOpen)
        Home.serialPortSetDevice.Close();
      if (Home.serialPortSetDevice != null)
        Home.serialPortSetDevice.Dispose();
      this.Close();
      this.Dispose();
    }

    private void comboxSerialPort_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (Home.serialPortSetDevice != null)
      {
        Home.serialPortSetDevice.Close();
        Home.serialPortSetDevice.Dispose();
      }
      Home.serialPortSetDevice = new SerialPort(this.comboxSerialPort.SelectedItem.ToString());
      Home.serialPortSetDevice.BaudRate = 9600;
      Home.serialPortSetDevice.Parity = Parity.Odd;
      Home.serialPortSetDevice.DataBits = 8;
      Home.serialPortSetDevice.StopBits = StopBits.One;
      this.serialPortSetDeviceOpen();
    }

    private void serialPortSetDeviceOpen()
    {
      if (!Home.serialPortSetDevice.IsOpen)
        ;
      try
      {
        Home.serialPortSetDevice.Open();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
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
      byte[] buffer = Home.SPControlWord["ReferenceOn"];
      Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
      Thread.Sleep(10);
      this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref Home.SpInfo, true);
      Home.SpInfo.DataAB = Home.SpInfo.DataA;
      Home.SpInfo.DataA = (double[]) null;
      this.DataIOmy.TXTSaveData(Environment.CurrentDirectory.ToString() + "\\background", Home.SpInfo.DataX, Home.SpInfo.DataAB);
      Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
      dataArray[0].DataX = Home.SpInfo.WavelengthArray;
      dataArray[0].DataE = Home.SpInfo.DataA;
      this.MySpectrometer.ReadDK(ref Home.SpInfo);
    }

    private void Report()
    {
      this.DataIOmy.TXTWriteIn(Environment.CurrentDirectory.ToString() + "\\Report.txt", string.Format("{0:yyyy-MM-dd   HH:mm:ss}", (object) DateTime.Now) + "品种：" + FrmGetSpecSet.Default.txtBreed.ToString() + "\r\n批次：" + FrmGetSpecSet.Default.txtBatch.ToString() + "\r\n生产季节：" + FrmGetSpecSet.Default.txtMadeSeason.ToString() + "\r\n雌：" + this.counterFemale.ToString() + "\r\n雄：" + this.counterMale.ToString() + "\r\n\r\n");
    }

    private void timerClearSensor_Tick(object sender, EventArgs e)
    {
      new Thread(new ThreadStart(this.clearSensor)).Start();
    }

    private void clearSensor()
    {
      if (!Home.serialPortSetDevice.IsOpen)
        Home.serialPortSetDevice.Open();
      byte[] buffer1 = Home.SPControlWord["ClearSensorOn"];
      Home.serialPortSetDevice.Write(buffer1, 0, buffer1.Length);
      Thread.Sleep(10000);
      byte[] buffer2 = Home.SPControlWord["ClearSensorOff"];
      Home.serialPortSetDevice.Write(buffer2, 0, buffer2.Length);
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

    private void btnGetSpectrumAuto_Click(object sender, EventArgs e)
    {
      Home.serialPortSetDevice.Close();
      Home.serialPortSetDevice.Dispose();
      new FrmGetSpecAuto(this).Show();
    }

    private void lbFemale_Click(object sender, EventArgs e)
    {
    }

    private void btnAdmin_Click(object sender, EventArgs e)
    {
      if (new LoginForm().ShowDialog() != DialogResult.OK)
        return;
      this.panel3.Visible = true;
      this.panel3.Location = new Point(720, 0);
      this.Size = new Size(1024, 768);
    }

    private void cBIsSaveSpec_CheckedChanged(object sender, EventArgs e)
    {
      FrmGetSpecSet.Default.isSaveSpecAtDistinguish = this.cBIsSaveSpec.Checked;
      FrmGetSpecSet.Default.Save();
    }

    private void btnBack_Click_1(object sender, EventArgs e)
    {
      this.panel3.Visible = false;
      this.Size = new Size(720, 768);
    }

    private void btnCounterReset_Click(object sender, EventArgs e)
    {
      FrmGetSpecSet.Default.counterFemale = 0;
      FrmGetSpecSet.Default.counterMale = 0;
      this.lbFemale.Text = "0";
      this.lbMale.Text = "0";
      this.counterFemale = 0;
      this.counterMale = 0;
      FrmGetSpecSet.Default.Save();
    }

    private void panelHeader_Paint(object sender, PaintEventArgs e)
    {
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Home));
      this.panelHeader = new Panel();
      this.btnCounterReset = new Button();
      this.btnAdmin = new Button();
      this.comboxSerialPort = new ComboBox();
      this.btnClose = new Button();
      this.lblConnectState = new Label();
      this.panel2 = new Panel();
      this.label10 = new Label();
      this.lbMale = new Label();
      this.panel1 = new Panel();
      this.label9 = new Label();
      this.lbFemale = new Label();
      this.label7 = new Label();
      this.txtTimeCount = new TextBox();
      this.txtAngleThreshold = new TextBox();
      this.label1 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.trackBarSpeed = new TrackBar();
      this.backgroundWorker1 = new BackgroundWorker();
      this.txtBreed = new TextBox();
      this.label8 = new Label();
      this.txtMadeSeason = new TextBox();
      this.label11 = new Label();
      this.txtBatch = new TextBox();
      this.label12 = new Label();
      this.panel3 = new Panel();
      this.cBIsSaveSpec = new CheckBox();
      this.panel4 = new Panel();
      this.btnBack = new Button();
      this.btnGetSpectrumAuto = new Button();
      this.btnSetting = new Button();
      this.btnGetSpectrum = new Button();
      this.txtBoxPuffIntervalTime = new TextBox();
      this.btnValveOff = new Button();
      this.btnConveyorOn = new Button();
      this.label18 = new Label();
      this.btnSpectrometerOff = new Button();
      this.btnValve1On = new Button();
      this.label17 = new Label();
      this.btnConveyorOff = new Button();
      this.btnReferenceCupOff = new Button();
      this.btnReferenceCupOn = new Button();
      this.label15 = new Label();
      this.btnLightOff = new Button();
      this.btnLightOn = new Button();
      this.label16 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.toolTip2 = new ToolTip(this.components);
      this.toolTip3 = new ToolTip(this.components);
      this.toolTip4 = new ToolTip(this.components);
      this.toolTip5 = new ToolTip(this.components);
      this.toolTip6 = new ToolTip(this.components);
      this.btnStop = new Button();
      this.btnGetSpec = new Button();
      this.backgroundWorker2 = new BackgroundWorker();
      this.timerClearSensor = new System.Windows.Forms.Timer(this.components);
      this.panelHeader.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.trackBarSpeed.BeginInit();
      this.panel3.SuspendLayout();
      this.panel4.SuspendLayout();
      this.SuspendLayout();
      this.panelHeader.Anchor = AnchorStyles.Left;
      this.panelHeader.BackColor = Color.FromArgb(38, 136, 210);
      this.panelHeader.Controls.Add((Control) this.btnCounterReset);
      this.panelHeader.Controls.Add((Control) this.btnAdmin);
      this.panelHeader.Controls.Add((Control) this.comboxSerialPort);
      this.panelHeader.Controls.Add((Control) this.btnClose);
      this.panelHeader.Controls.Add((Control) this.lblConnectState);
      this.panelHeader.Location = new Point(0, 0);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(539, 45);
      this.panelHeader.TabIndex = 28;
      this.panelHeader.Paint += new PaintEventHandler(this.panelHeader_Paint);
      this.btnCounterReset.Anchor = AnchorStyles.Right;
      this.btnCounterReset.BackColor = Color.Transparent;
      this.btnCounterReset.BackgroundImage = (Image) Resources.ResetZero;
      this.btnCounterReset.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnCounterReset.FlatStyle = FlatStyle.Flat;
      this.btnCounterReset.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnCounterReset.Location = new Point(415, 4);
      this.btnCounterReset.Margin = new Padding(4);
      this.btnCounterReset.Name = "btnCounterReset";
      this.btnCounterReset.Size = new Size(40, 35);
      this.btnCounterReset.TabIndex = 68;
      this.btnCounterReset.UseVisualStyleBackColor = false;
      this.btnCounterReset.Click += new EventHandler(this.btnCounterReset_Click);
      this.btnAdmin.Anchor = AnchorStyles.Right;
      this.btnAdmin.BackColor = Color.Transparent;
      this.btnAdmin.BackgroundImage = (Image) Resources.admin;
      this.btnAdmin.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnAdmin.FlatStyle = FlatStyle.Flat;
      this.btnAdmin.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnAdmin.Location = new Point(454, 4);
      this.btnAdmin.Margin = new Padding(4);
      this.btnAdmin.Name = "btnAdmin";
      this.btnAdmin.Size = new Size(40, 35);
      this.btnAdmin.TabIndex = 67;
      this.btnAdmin.UseVisualStyleBackColor = false;
      this.btnAdmin.Click += new EventHandler(this.btnAdmin_Click);
      this.comboxSerialPort.Font = new Font("微软雅黑", 13.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.comboxSerialPort.FormattingEnabled = true;
      this.comboxSerialPort.Location = new Point(220, 6);
      this.comboxSerialPort.Margin = new Padding(2);
      this.comboxSerialPort.Name = "comboxSerialPort";
      this.comboxSerialPort.Size = new Size(80, 32);
      this.comboxSerialPort.TabIndex = 66;
      this.comboxSerialPort.SelectedIndexChanged += new EventHandler(this.comboxSerialPort_SelectedIndexChanged);
      this.btnClose.Anchor = AnchorStyles.Right;
      this.btnClose.BackColor = Color.Transparent;
      this.btnClose.BackgroundImage = (Image) Resources.Close;
      this.btnClose.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnClose.FlatStyle = FlatStyle.Flat;
      this.btnClose.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnClose.Location = new Point(494, 1);
      this.btnClose.Margin = new Padding(4);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(42, 39);
      this.btnClose.TabIndex = 64;
      this.btnClose.UseVisualStyleBackColor = false;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.lblConnectState.AutoSize = true;
      this.lblConnectState.Font = new Font("宋体", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.lblConnectState.ForeColor = Color.White;
      this.lblConnectState.Location = new Point(10, 7);
      this.lblConnectState.Name = "lblConnectState";
      this.lblConnectState.Size = new Size(42, 27);
      this.lblConnectState.TabIndex = 11;
      this.lblConnectState.Text = "--";
      this.panel2.BackColor = Color.FromArgb(128, (int) byte.MaxValue, (int) byte.MaxValue);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.lbMale);
      this.panel2.Location = new Point(273, 331);
      this.panel2.Margin = new Padding(2);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(110, 110);
      this.panel2.TabIndex = 41;
      this.label10.AutoSize = true;
      this.label10.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label10.Location = new Point(1, 5);
      this.label10.Margin = new Padding(2, 0, 2, 0);
      this.label10.Name = "label10";
      this.label10.Size = new Size(28, 19);
      this.label10.TabIndex = 2;
      this.label10.Text = "雄";
      this.lbMale.AutoSize = true;
      this.lbMale.Font = new Font("宋体", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.lbMale.Location = new Point(36, 40);
      this.lbMale.Margin = new Padding(2, 0, 2, 0);
      this.lbMale.Name = "lbMale";
      this.lbMale.Size = new Size(22, 24);
      this.lbMale.TabIndex = 1;
      this.lbMale.Text = "2";
      this.panel1.BackColor = Color.FromArgb((int) byte.MaxValue, 192, 192);
      this.panel1.Controls.Add((Control) this.label9);
      this.panel1.Controls.Add((Control) this.lbFemale);
      this.panel1.Location = new Point(137, 331);
      this.panel1.Margin = new Padding(2);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(110, 110);
      this.panel1.TabIndex = 40;
      this.label9.AutoSize = true;
      this.label9.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label9.Location = new Point(1, 4);
      this.label9.Margin = new Padding(2, 0, 2, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(28, 19);
      this.label9.TabIndex = 1;
      this.label9.Text = "雌";
      this.lbFemale.AutoSize = true;
      this.lbFemale.Font = new Font("宋体", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.lbFemale.Location = new Point(38, 40);
      this.lbFemale.Margin = new Padding(2, 0, 2, 0);
      this.lbFemale.Name = "lbFemale";
      this.lbFemale.Size = new Size(22, 24);
      this.lbFemale.TabIndex = 0;
      this.lbFemale.Text = "1";
      this.lbFemale.Click += new EventHandler(this.lbFemale_Click);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(36, 391);
      this.label7.Margin = new Padding(2, 0, 2, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(65, 12);
      this.label7.TabIndex = 55;
      this.label7.Text = "消耗时间：";
      this.txtTimeCount.Location = new Point(102, 390);
      this.txtTimeCount.Margin = new Padding(2);
      this.txtTimeCount.Name = "txtTimeCount";
      this.txtTimeCount.Size = new Size(78, 21);
      this.txtTimeCount.TabIndex = 54;
      this.txtAngleThreshold.Location = new Point(130, 229);
      this.txtAngleThreshold.Margin = new Padding(2);
      this.txtAngleThreshold.Name = "txtAngleThreshold";
      this.txtAngleThreshold.Size = new Size(52, 21);
      this.txtAngleThreshold.TabIndex = 53;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(68, 231);
      this.label1.Margin = new Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(65, 12);
      this.label1.TabIndex = 52;
      this.label1.Text = "相隔点数：";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(182, 353);
      this.label5.Margin = new Padding(2, 0, 2, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(17, 12);
      this.label5.TabIndex = 48;
      this.label5.Text = "ms";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(14, 353);
      this.label4.Margin = new Padding(2, 0, 2, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(83, 12);
      this.label4.TabIndex = 47;
      this.label4.Text = "吹扫时间间隔:";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(176, 274);
      this.label3.Margin = new Padding(2, 0, 2, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(17, 12);
      this.label3.TabIndex = 45;
      this.label3.Text = "快";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(62, 274);
      this.label2.Margin = new Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(17, 12);
      this.label2.TabIndex = 44;
      this.label2.Text = "慢";
      this.trackBarSpeed.Location = new Point(78, 269);
      this.trackBarSpeed.Margin = new Padding(2);
      this.trackBarSpeed.Name = "trackBarSpeed";
      this.trackBarSpeed.Size = new Size(103, 42);
      this.trackBarSpeed.TabIndex = 43;
      this.trackBarSpeed.Value = 5;
      this.trackBarSpeed.Scroll += new EventHandler(this.trackBarSpeed_Scroll);
      this.backgroundWorker1.WorkerReportsProgress = true;
      this.backgroundWorker1.WorkerSupportsCancellation = true;
      this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      this.txtBreed.Font = new Font("宋体", 13.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.txtBreed.Location = new Point(102, 92);
      this.txtBreed.Margin = new Padding(2);
      this.txtBreed.Name = "txtBreed";
      this.txtBreed.Size = new Size(104, 28);
      this.txtBreed.TabIndex = 57;
      this.label8.AutoSize = true;
      this.label8.Font = new Font("微软雅黑", 13.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.label8.Location = new Point(50, 90);
      this.label8.Margin = new Padding(2, 0, 2, 0);
      this.label8.Name = "label8";
      this.label8.Size = new Size(50, 26);
      this.label8.TabIndex = 56;
      this.label8.Text = "品种";
      this.txtMadeSeason.Font = new Font("宋体", 13.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.txtMadeSeason.Location = new Point(104, 170);
      this.txtMadeSeason.Margin = new Padding(2);
      this.txtMadeSeason.Name = "txtMadeSeason";
      this.txtMadeSeason.Size = new Size(104, 28);
      this.txtMadeSeason.TabIndex = 59;
      this.label11.AutoSize = true;
      this.label11.Font = new Font("微软雅黑", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.label11.Location = new Point(14, 170);
      this.label11.Margin = new Padding(2, 0, 2, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(88, 26);
      this.label11.TabIndex = 58;
      this.label11.Text = "生产季节";
      this.txtBatch.Font = new Font("宋体", 13.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.txtBatch.Location = new Point(102, 130);
      this.txtBatch.Margin = new Padding(2);
      this.txtBatch.Name = "txtBatch";
      this.txtBatch.Size = new Size(104, 28);
      this.txtBatch.TabIndex = 61;
      this.label12.AutoSize = true;
      this.label12.Font = new Font("微软雅黑", 13.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.label12.Location = new Point(50, 129);
      this.label12.Margin = new Padding(2, 0, 2, 0);
      this.label12.Name = "label12";
      this.label12.Size = new Size(50, 26);
      this.label12.TabIndex = 60;
      this.label12.Text = "批次";
      this.panel3.BackColor = Color.White;
      this.panel3.Controls.Add((Control) this.cBIsSaveSpec);
      this.panel3.Controls.Add((Control) this.panel4);
      this.panel3.Controls.Add((Control) this.txtBoxPuffIntervalTime);
      this.panel3.Controls.Add((Control) this.btnValveOff);
      this.panel3.Controls.Add((Control) this.btnConveyorOn);
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
      this.panel3.Controls.Add((Control) this.txtBatch);
      this.panel3.Controls.Add((Control) this.txtMadeSeason);
      this.panel3.Controls.Add((Control) this.txtBreed);
      this.panel3.Controls.Add((Control) this.label11);
      this.panel3.Controls.Add((Control) this.label7);
      this.panel3.Controls.Add((Control) this.txtTimeCount);
      this.panel3.Controls.Add((Control) this.label8);
      this.panel3.Controls.Add((Control) this.txtAngleThreshold);
      this.panel3.Controls.Add((Control) this.label1);
      this.panel3.Controls.Add((Control) this.label5);
      this.panel3.Controls.Add((Control) this.label4);
      this.panel3.Controls.Add((Control) this.label3);
      this.panel3.Controls.Add((Control) this.label2);
      this.panel3.Controls.Add((Control) this.trackBarSpeed);
      this.panel3.Controls.Add((Control) this.label12);
      this.panel3.Location = new Point(704, 9);
      this.panel3.Margin = new Padding(2);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(230, 615);
      this.panel3.TabIndex = 62;
      this.panel3.Visible = false;
      this.cBIsSaveSpec.AutoSize = true;
      this.cBIsSaveSpec.Font = new Font("宋体", 14.25f);
      this.cBIsSaveSpec.Location = new Point(78, 318);
      this.cBIsSaveSpec.Margin = new Padding(2);
      this.cBIsSaveSpec.Name = "cBIsSaveSpec";
      this.cBIsSaveSpec.Size = new Size(104, 23);
      this.cBIsSaveSpec.TabIndex = 77;
      this.cBIsSaveSpec.Text = "保存光谱";
      this.cBIsSaveSpec.UseVisualStyleBackColor = true;
      this.cBIsSaveSpec.CheckedChanged += new EventHandler(this.cBIsSaveSpec_CheckedChanged);
      this.panel4.BackColor = Color.FromArgb(38, 136, 210);
      this.panel4.Controls.Add((Control) this.btnBack);
      this.panel4.Controls.Add((Control) this.btnGetSpectrumAuto);
      this.panel4.Controls.Add((Control) this.btnSetting);
      this.panel4.Controls.Add((Control) this.btnGetSpectrum);
      this.panel4.Location = new Point(2, 1);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(227, 45);
      this.panel4.TabIndex = 76;
      this.btnBack.Anchor = AnchorStyles.Right;
      this.btnBack.BackColor = Color.Transparent;
      this.btnBack.BackgroundImage = (Image) Resources.close1;
      this.btnBack.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnBack.FlatStyle = FlatStyle.Flat;
      this.btnBack.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnBack.Location = new Point(186, 6);
      this.btnBack.Margin = new Padding(4);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new Size(43, 32);
      this.btnBack.TabIndex = 69;
      this.btnBack.UseVisualStyleBackColor = false;
      this.btnBack.Click += new EventHandler(this.btnBack_Click_1);
      this.btnGetSpectrumAuto.Anchor = AnchorStyles.Right;
      this.btnGetSpectrumAuto.BackColor = Color.Transparent;
      this.btnGetSpectrumAuto.BackgroundImage = (Image) Resources.icon100_com_210;
      this.btnGetSpectrumAuto.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnGetSpectrumAuto.FlatStyle = FlatStyle.Flat;
      this.btnGetSpectrumAuto.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnGetSpectrumAuto.Location = new Point(70, 6);
      this.btnGetSpectrumAuto.Margin = new Padding(4);
      this.btnGetSpectrumAuto.Name = "btnGetSpectrumAuto";
      this.btnGetSpectrumAuto.Size = new Size(43, 32);
      this.btnGetSpectrumAuto.TabIndex = 68;
      this.btnGetSpectrumAuto.UseVisualStyleBackColor = false;
      this.btnGetSpectrumAuto.Click += new EventHandler(this.btnGetSpectrumAuto_Click);
      this.btnSetting.Anchor = AnchorStyles.Right;
      this.btnSetting.BackColor = Color.Transparent;
      this.btnSetting.BackgroundImage = (Image) Resources.Set;
      this.btnSetting.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnSetting.FlatStyle = FlatStyle.Flat;
      this.btnSetting.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnSetting.Location = new Point(148, 2);
      this.btnSetting.Margin = new Padding(4);
      this.btnSetting.Name = "btnSetting";
      this.btnSetting.Size = new Size(44, 36);
      this.btnSetting.TabIndex = 63;
      this.btnSetting.UseVisualStyleBackColor = false;
      this.btnSetting.Click += new EventHandler(this.btnSetting_Click);
      this.btnGetSpectrum.Anchor = AnchorStyles.Right;
      this.btnGetSpectrum.BackColor = Color.Transparent;
      this.btnGetSpectrum.BackgroundImage = (Image) Resources.Spec;
      this.btnGetSpectrum.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnGetSpectrum.FlatStyle = FlatStyle.Flat;
      this.btnGetSpectrum.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnGetSpectrum.Location = new Point(112, 4);
      this.btnGetSpectrum.Margin = new Padding(4);
      this.btnGetSpectrum.Name = "btnGetSpectrum";
      this.btnGetSpectrum.Size = new Size(37, 35);
      this.btnGetSpectrum.TabIndex = 65;
      this.btnGetSpectrum.UseVisualStyleBackColor = false;
      this.btnGetSpectrum.Click += new EventHandler(this.btnGetSpectrum_Click);
      this.txtBoxPuffIntervalTime.Location = new Point(99, 351);
      this.txtBoxPuffIntervalTime.Margin = new Padding(2);
      this.txtBoxPuffIntervalTime.Name = "txtBoxPuffIntervalTime";
      this.txtBoxPuffIntervalTime.Size = new Size(78, 21);
      this.txtBoxPuffIntervalTime.TabIndex = 75;
      this.btnValveOff.Location = new Point(184, 562);
      this.btnValveOff.Name = "btnValveOff";
      this.btnValveOff.Size = new Size(35, 39);
      this.btnValveOff.TabIndex = 74;
      this.btnValveOff.Text = "关";
      this.btnValveOff.UseVisualStyleBackColor = true;
      this.btnValveOff.Click += new EventHandler(this.btnValveOff_Click);
      this.btnConveyorOn.Location = new Point(105, 470);
      this.btnConveyorOn.Name = "btnConveyorOn";
      this.btnConveyorOn.Size = new Size(35, 39);
      this.btnConveyorOn.TabIndex = 68;
      this.btnConveyorOn.Tag = (object) " ";
      this.btnConveyorOn.Text = "启";
      this.btnConveyorOn.UseVisualStyleBackColor = true;
      this.btnConveyorOn.Click += new EventHandler(this.btnConveyorOn_Click);
      this.label18.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label18.Location = new Point(33, 570);
      this.label18.Name = "label18";
      this.label18.Size = new Size(66, 33);
      this.label18.TabIndex = 73;
      this.label18.Text = "阀门：";
      this.btnSpectrometerOff.Location = new Point(142, 562);
      this.btnSpectrometerOff.Name = "btnSpectrometerOff";
      this.btnSpectrometerOff.Size = new Size(39, 39);
      this.btnSpectrometerOff.TabIndex = 72;
      this.btnSpectrometerOff.Text = "2号开";
      this.btnSpectrometerOff.UseVisualStyleBackColor = true;
      this.btnSpectrometerOff.Click += new EventHandler(this.btnSpectrometerOff_Click);
      this.btnValve1On.Location = new Point(105, 562);
      this.btnValve1On.Name = "btnValve1On";
      this.btnValve1On.Size = new Size(35, 39);
      this.btnValve1On.TabIndex = 71;
      this.btnValve1On.Tag = (object) " ";
      this.btnValve1On.Text = "1号开";
      this.btnValve1On.UseVisualStyleBackColor = true;
      this.btnValve1On.Click += new EventHandler(this.btnValve1On_Click);
      this.label17.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label17.Location = new Point(26, 478);
      this.label17.Name = "label17";
      this.label17.Size = new Size(91, 33);
      this.label17.TabIndex = 70;
      this.label17.Text = "传送带：";
      this.btnConveyorOff.Location = new Point(142, 470);
      this.btnConveyorOff.Name = "btnConveyorOff";
      this.btnConveyorOff.Size = new Size(35, 39);
      this.btnConveyorOff.TabIndex = 69;
      this.btnConveyorOff.Text = "停";
      this.btnConveyorOff.UseVisualStyleBackColor = true;
      this.btnConveyorOff.Click += new EventHandler(this.btnConveyorOff_Click);
      this.btnReferenceCupOff.Location = new Point(142, 423);
      this.btnReferenceCupOff.Name = "btnReferenceCupOff";
      this.btnReferenceCupOff.Size = new Size(35, 39);
      this.btnReferenceCupOff.TabIndex = 66;
      this.btnReferenceCupOff.Text = "回";
      this.btnReferenceCupOff.UseVisualStyleBackColor = true;
      this.btnReferenceCupOff.Click += new EventHandler(this.btnReferenceCupOff_Click);
      this.btnReferenceCupOn.Location = new Point(105, 423);
      this.btnReferenceCupOn.Name = "btnReferenceCupOn";
      this.btnReferenceCupOn.Size = new Size(35, 39);
      this.btnReferenceCupOn.TabIndex = 65;
      this.btnReferenceCupOn.Tag = (object) " ";
      this.btnReferenceCupOn.Text = "出";
      this.btnReferenceCupOn.UseVisualStyleBackColor = true;
      this.btnReferenceCupOn.Click += new EventHandler(this.btnReferenceCupOn_Click);
      this.label15.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label15.Location = new Point(36, 526);
      this.label15.Name = "label15";
      this.label15.Size = new Size(66, 33);
      this.label15.TabIndex = 64;
      this.label15.Text = "灯光：";
      this.btnLightOff.Location = new Point(142, 518);
      this.btnLightOff.Name = "btnLightOff";
      this.btnLightOff.Size = new Size(35, 39);
      this.btnLightOff.TabIndex = 63;
      this.btnLightOff.Text = "关";
      this.btnLightOff.UseVisualStyleBackColor = true;
      this.btnLightOff.Click += new EventHandler(this.btnLightOff_Click);
      this.btnLightOn.Location = new Point(106, 518);
      this.btnLightOn.Name = "btnLightOn";
      this.btnLightOn.Size = new Size(35, 39);
      this.btnLightOn.TabIndex = 62;
      this.btnLightOn.Tag = (object) " ";
      this.btnLightOn.Text = "开";
      this.btnLightOn.UseVisualStyleBackColor = true;
      this.btnLightOn.Click += new EventHandler(this.btnLightOn_Click);
      this.label16.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label16.Location = new Point(24, 431);
      this.label16.Name = "label16";
      this.label16.Size = new Size(87, 33);
      this.label16.TabIndex = 67;
      this.label16.Text = "参比杯：";
      this.btnStop.BackColor = Color.Transparent;
      this.btnStop.BackgroundImage = (Image) Resources.Pause;
      this.btnStop.BackgroundImageLayout = ImageLayout.Stretch;
      this.btnStop.Cursor = Cursors.Hand;
      this.btnStop.Enabled = false;
      this.btnStop.FlatStyle = FlatStyle.Flat;
      this.btnStop.ForeColor = Color.Transparent;
      this.btnStop.Location = new Point(273, 196);
      this.btnStop.Margin = new Padding(0);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(97, 96);
      this.btnStop.TabIndex = 42;
      this.btnStop.UseVisualStyleBackColor = false;
      this.btnStop.Click += new EventHandler(this.btnStop_Click);
      this.btnGetSpec.BackColor = Color.Transparent;
      this.btnGetSpec.BackgroundImage = (Image) Resources.Play2;
      this.btnGetSpec.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnGetSpec.Cursor = Cursors.Hand;
      this.btnGetSpec.FlatStyle = FlatStyle.Flat;
      this.btnGetSpec.ForeColor = Color.Transparent;
      this.btnGetSpec.Location = new Point(142, 198);
      this.btnGetSpec.Margin = new Padding(4);
      this.btnGetSpec.Name = "btnGetSpec";
      this.btnGetSpec.Size = new Size(105, 98);
      this.btnGetSpec.TabIndex = 39;
      this.btnGetSpec.UseVisualStyleBackColor = false;
      this.btnGetSpec.Click += new EventHandler(this.btnGetSpec_Click);
      this.backgroundWorker2.WorkerSupportsCancellation = true;
      this.backgroundWorker2.DoWork += new DoWorkEventHandler(this.backgroundWorker2_DoWork);
      this.timerClearSensor.Tick += new EventHandler(this.timerClearSensor_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(1014, 614);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.btnStop);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnGetSpec);
      this.Controls.Add((Control) this.panelHeader);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Home);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "蚕蛹剔除";
      this.Load += new EventHandler(this.Home_Load);
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.trackBarSpeed.EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panel4.ResumeLayout(false);
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
  }
}
