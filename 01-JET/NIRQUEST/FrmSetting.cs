// Decompiled with JetBrains decompiler
// Type: JSDU.FrmSetting
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using NIRQUEST;
using NIRQUEST.Properties;
using SPAM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace JSDU
{
  public class FrmSetting : Form
  {
    private DataIO DataIOS = new DataIO();
    private DataHandling DataHandlingmy = new DataHandling();
    private Color[] DrawColor = new Color[5];
    private Spectrometer.SpecInfo SpInfoTmpt = new Spectrometer.SpecInfo();
    private bool AutoFindIntegrationTimes = false;
    private bool AutoFindIntegrationTimess = false;
    private int AutoFindIntegrationTimeCount = 0;
    private bool GetConveySpecStartFlag = false;
    private int[] wavelengthIndex = (int[]) null;
    private Dictionary<string, double[]> ConveySpec = new Dictionary<string, double[]>();
    private IContainer components = (IContainer) null;
    private Spectrometer MSetSpectrometer;
    private CCoSpectralMath spectralMath;
    private FrmGetSpec frmGetSpec;
    private Home frmHome;
    private FrmSetting.MyChartLoadData myChartLoadData;
    private System.Windows.Forms.Label label1;
    private TextBox txtIntegrationTime;
    private TextBox txtScanTimes;
    private System.Windows.Forms.Label label2;
    private Button btnSetOK;
    private FolderBrowserDialog folderBrowserDialog1;
    private System.Windows.Forms.Label label5;
    private TextBox txtSavePath;
    private Button btnSelectSavePath;
    private CheckBox cBClearDarks;
    private CheckBox cBCorrectElectricalDarks;
    private CheckBox cBCorrectNonlinearitys;
    private ZedGraphControl MyChart;
    private Button btnAutoIntegrationTime;
    private Button btnSaveDark;
    private System.Windows.Forms.Timer timer1;
    private BackgroundWorker backgroundWorker1;
    private BackgroundWorker backgroundWorker2;
    private Panel panelHeader;
    private Button btnClose;
    private TextBox txtIntegrationTimeBK;
    private System.Windows.Forms.Label label3;
    private Button btnAutoIntegrationTimeBK;
    private GroupBox groupBox1;
    private Button btnGetConveySpec;
    private System.Windows.Forms.Label label7;
    private ListBox listBoxWaveLenth;
    private System.Windows.Forms.Label label6;
    private Button btnSelectWormSpec;
    private TextBox textBox1;
    private System.Windows.Forms.Label label4;
    private Button btnGetConveySpecStop;
    private BackgroundWorker bkwGetConveySpec;
    private OpenFileDialog openFileDialog1;
    private TextBox txtThresholdDiff;
    private System.Windows.Forms.Label label8;
    private TextBox txtClearSensorIntervalTimes;
    private System.Windows.Forms.Label label9;

    public FrmSetting(Spectrometer MSetSpectrometers, FrmGetSpec frmmain, Home home)
    {
      this.InitializeComponent();
      this.MSetSpectrometer = MSetSpectrometers;
      this.frmGetSpec = frmmain;
      this.frmHome = home;
      this.DrawColor[0] = Color.Red;
      this.DrawColor[1] = Color.BlueViolet;
      this.DrawColor[2] = Color.Crimson;
      this.DrawColor[3] = Color.Chocolate;
      this.DrawColor[4] = Color.Brown;
    }

    private void btnSetOK_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.txtIntegrationTime.Text == "")
        {
          Spectrometer.IntegrationTime = 0;
        }
        else
        {
          if (Convert.ToDouble(this.txtIntegrationTime.Text) > 800.0)
          {
            int num = (int) MessageBox.Show("积分时间设置过大，请重新设置！");
            return;
          }
          Spectrometer.IntegrationTime = Convert.ToInt32(Convert.ToDouble(this.txtIntegrationTime.Text) * 1000.0);
        }
        if (this.txtIntegrationTimeBK.Text == "")
        {
          Spectrometer.IntegrationTimeBK = 0;
        }
        else
        {
          if (Convert.ToDouble(this.txtIntegrationTimeBK.Text) > 800.0)
          {
            int num = (int) MessageBox.Show("积分时间设置过大，请重新设置！");
            return;
          }
          Spectrometer.IntegrationTimeBK = Convert.ToInt32(Convert.ToDouble(this.txtIntegrationTimeBK.Text) * 1000.0);
        }
        Spectrometer.ScanTimes = Convert.ToInt32(this.txtScanTimes.Text);
        Spectrometer.isClearDarks = this.cBClearDarks.Checked;
        Spectrometer.isCorrectElectricalDarks = this.cBCorrectElectricalDarks.Checked;
        Spectrometer.isCorrectNonlinearitys = this.cBCorrectNonlinearitys.Checked;
        FrmGetSpecSet.Default.clearSensorIntervalTimes = int.Parse(this.txtClearSensorIntervalTimes.Text) * 60000;
        FrmGetSpecSet.Default.Save();
        this.frmHome.timerClearSensor.Interval = FrmGetSpecSet.Default.clearSensorIntervalTimes;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("输入错误！," + ex.ToString());
        return;
      }
      try
      {
        Spectrometer.SavePath = this.txtSavePath.Text;
        string str1 = Spectrometer.IntegrationTime.ToString() + "," + Spectrometer.ScanTimes.ToString() + "," + (object) Spectrometer.GainMode + "," + this.txtSavePath.Text + "," + (Spectrometer.isClearDarks ? 1 : 0).ToString() + "," + (Spectrometer.isCorrectElectricalDarks ? 1 : 0).ToString() + "," + (Spectrometer.isCorrectNonlinearitys ? 1 : 0).ToString();
        if (this.backgroundWorker2.IsBusy)
          this.backgroundWorker2.CancelAsync();
        this.backgroundWorker2.RunWorkerAsync((object) str1);
        this.timer1.Enabled = false;
        if (this.backgroundWorker1.IsBusy)
          this.backgroundWorker1.CancelAsync();
        if (this.frmHome != null)
          FrmGetSpecSet.Default.IntegrationTimeBK = Spectrometer.IntegrationTimeBK;
        if (this.listBoxWaveLenth.SelectedIndices.Count > 0)
        {
          FrmGetSpecSet.Default.ThresholdDiff = double.Parse(this.txtThresholdDiff.Text);
          FrmGetSpecSet.Default.WavelengthDiffIndex = "";
          for (int index = 0; index < this.listBoxWaveLenth.SelectedIndices.Count; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            FrmGetSpecSet frmGetSpecSet = FrmGetSpecSet.Default;
            string str2 = frmGetSpecSet.WavelengthDiffIndex + this.wavelengthIndex[this.listBoxWaveLenth.SelectedIndices[index]].ToString() + ",";
            frmGetSpecSet.WavelengthDiffIndex = str2;
          }
        }
        FrmGetSpecSet.Default.Save();
        FrmGetSpecSet.Default.Save();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("输入错误！," + ex.ToString());
        return;
      }
      if (this.frmGetSpec == null || this.frmGetSpec == null)
        return;
      this.frmGetSpec.timer1.Enabled = true;
    }

    private void FrmSetting_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.timer1.Enabled)
        return;
      this.timer1.Enabled = false;
      if (this.backgroundWorker1.IsBusy)
        this.backgroundWorker1.CancelAsync();
      this.Close();
      if (this.frmGetSpec != null && this.frmGetSpec != null)
        this.frmGetSpec.timer1.Enabled = true;
    }

    private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
    {
      this.MSetSpectrometer.SetParameters(Spectrometer.spectrometerIndex, Spectrometer.IntegrationTime);
      this.DataIOS.SaveStr((string) e.Argument, Spectrometer.ApplicationPath + "\\Setting");
    }

    private void FrmSetting_Load(object sender, EventArgs e)
    {
      int num1;
      if (Spectrometer.IntegrationTime != 0)
      {
        TextBox txtIntegrationTime = this.txtIntegrationTime;
        num1 = Spectrometer.IntegrationTime / 1000;
        string str = num1.ToString();
        txtIntegrationTime.Text = str;
      }
      if (Spectrometer.ScanTimes != 0)
        this.txtScanTimes.Text = Spectrometer.ScanTimes.ToString();
      if (Spectrometer.SavePath != null)
        this.txtSavePath.Text = Spectrometer.SavePath;
      else
        this.txtSavePath.Text = Spectrometer.ApplicationPath;
      if (Spectrometer.isClearDarks)
        this.cBClearDarks.Checked = true;
      if (Spectrometer.isCorrectElectricalDarks)
        this.cBCorrectElectricalDarks.Checked = true;
      if (Spectrometer.isCorrectNonlinearitys)
        this.cBCorrectNonlinearitys.Checked = true;
      if (Spectrometer.IntegrationTimeBK != 0)
      {
        TextBox integrationTimeBk = this.txtIntegrationTimeBK;
        num1 = Spectrometer.IntegrationTimeBK / 1000;
        string str = num1.ToString();
        integrationTimeBk.Text = str;
      }
      else
        this.txtIntegrationTimeBK.Text = this.txtIntegrationTime.Text;
      TextBox sensorIntervalTimes = this.txtClearSensorIntervalTimes;
      num1 = FrmGetSpecSet.Default.clearSensorIntervalTimes / 60000;
      string str1 = num1.ToString();
      sensorIntervalTimes.Text = str1;
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer1 = Home.SPControlWord["LightOn"];
        Home.serialPortSetDevice.Write(buffer1, 0, buffer1.Length);
        Thread.Sleep(500);
        byte[] buffer2 = Home.SPControlWord["ReferenceOff"];
        Home.serialPortSetDevice.Write(buffer2, 0, buffer2.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show(ex.ToString());
      }
      this.timer1.Enabled = true;
    }

    private void btnSelectSavePath_Click(object sender, EventArgs e)
    {
      this.folderBrowserDialog1.ShowNewFolderButton = true;
      this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
      this.folderBrowserDialog1.Description = "选择保存文件夹";
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.txtSavePath.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void cBClearDarks_CheckedChanged(object sender, EventArgs e)
    {
      this.MSetSpectrometer.SetParameters(Spectrometer.spectrometerIndex, Spectrometer.IntegrationTime);
      if (this.cBClearDarks.Checked)
        this.btnSaveDark.Enabled = true;
      else
        this.btnSaveDark.Enabled = false;
    }

    private void btnAutoIntegrationTime_MouseEnter(object sender, EventArgs e)
    {
      new ToolTip() { ShowAlways = true }.SetToolTip((Control) this.btnAutoIntegrationTime, "点击自动设置积分时间");
    }

    private void btnSaveDark_MouseEnter(object sender, EventArgs e)
    {
      new ToolTip() { ShowAlways = true }.SetToolTip((Control) this.btnSaveDark, "点击保存暗噪声");
    }

    private void btnAutoIntegrationTime_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer1 = Home.SPControlWord["LightOn"];
        Home.serialPortSetDevice.Write(buffer1, 0, buffer1.Length);
        Thread.Sleep(500);
        byte[] buffer2 = Home.SPControlWord["ReferenceOff"];
        Home.serialPortSetDevice.Write(buffer2, 0, buffer2.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
        return;
      }
      if (this.AutoFindIntegrationTimes && this.txtIntegrationTime.Text.Contains("寻找中"))
      {
        this.backgroundWorker1.CancelAsync();
        this.txtIntegrationTime.Text = Math.Round((double) Spectrometer.IntegrationTime / 1000.0, 1).ToString();
        this.AutoFindIntegrationTimes = false;
        this.btnAutoIntegrationTime.BackgroundImage = (Image) Resources.play;
      }
      else
      {
        this.AutoFindIntegrationTimes = true;
        this.txtIntegrationTime.Text = "寻找中..";
        this.btnAutoIntegrationTime.BackgroundImage = (Image) Resources.stop;
      }
    }

    private void btnAutoIntegrationTimeBK_Click(object sender, EventArgs e)
    {
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
        return;
      }
      if (this.AutoFindIntegrationTimes && this.txtIntegrationTime.Text.Contains("寻找中"))
      {
        this.backgroundWorker1.CancelAsync();
        this.txtIntegrationTimeBK.Text = Math.Round((double) Spectrometer.IntegrationTime / 1000.0, 1).ToString();
        this.AutoFindIntegrationTimes = false;
        this.btnAutoIntegrationTimeBK.BackgroundImage = (Image) Resources.play;
      }
      else
      {
        this.AutoFindIntegrationTimes = true;
        this.txtIntegrationTimeBK.Text = "寻找中..";
        this.btnAutoIntegrationTimeBK.BackgroundImage = (Image) Resources.stop;
      }
    }

    private void btnSaveDark_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer1 = Home.SPControlWord["LightOff"];
        Home.serialPortSetDevice.Write(buffer1, 0, buffer1.Length);
        Thread.Sleep(500);
        byte[] buffer2 = Home.SPControlWord["ReferenceOn"];
        Home.serialPortSetDevice.Write(buffer2, 0, buffer2.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
        return;
      }
      if (MessageBox.Show("请将外部光源关闭，待下图更新后点击“确定”，系统将保存！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.OK)
      {
        FrmGetSpec.SpInfo.DataAD = this.SpInfoTmpt.DataA;
        this.DataIOS.TXTSaveData(Environment.CurrentDirectory.ToString() + "\\dark", this.SpInfoTmpt.DataX, this.SpInfoTmpt.DataA);
      }
      int num1 = (int) MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (Spectrometer.IntegrationTime > 500000)
        this.timer1.Interval = 3000;
      if (this.backgroundWorker1.IsBusy)
        return;
      if (this.AutoFindIntegrationTimes)
      {
        this.txtIntegrationTime.Text += ".";
        if (this.txtIntegrationTime.Text.Contains("...."))
          this.txtIntegrationTime.Text = "寻找中";
        ++this.AutoFindIntegrationTimeCount;
      }
      this.backgroundWorker1.RunWorkerAsync((object) this.SpInfoTmpt);
    }

    private int FindPeaks(Spectrometer.SpecInfo SpInfo)
    {
      return this.DataHandlingmy.MaxValueIndex(SpInfo.DataA);
    }

    private void DrawEnergyValue()
    {
      for (int index = 0; index < this.SpInfoTmpt.numPixls; ++index)
      {
        if (this.SpInfoTmpt.WavelengthArray[index] > 10000000000.0)
          this.SpInfoTmpt.WavelengthArray[index] = this.SpInfoTmpt.WavelengthArray[index - 1];
        if (this.SpInfoTmpt.DataA[index] > 10000000000.0)
          this.SpInfoTmpt.DataA[index] = this.SpInfoTmpt.DataA[index - 1];
      }
      this.SpInfoTmpt.DataX = this.SpInfoTmpt.WavelengthArray;
      this.SpInfoTmpt.w1 = Math.Floor(this.SpInfoTmpt.DataX[0] / 100.0) * 100.0;
      this.SpInfoTmpt.w2 = Math.Ceiling(this.SpInfoTmpt.DataX[this.SpInfoTmpt.DataX.Length - 1] / 100.0) * 100.0;
      Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
      dataArray[0].DataX = this.SpInfoTmpt.WavelengthArray;
      dataArray[0].DataE = this.SpInfoTmpt.DataA;
      this.BeginInvoke((Delegate) new FrmSetting.DrawDelegate(this.Draw), (object) "能量图", (object) dataArray, (object) 0);
    }

    private void Draw(string str, Spectrometer.Data[] DataGet, int Num)
    {
      if (DataGet != null && DataGet[Num].DataX != null)
      {
        if (DataGet[Num].DataE == null)
          return;
        this.MyChart.GraphPane = new GraphPane(this.MyChart.GraphPane.Rect, str, "波数", "能量");
        this.MyChart.GraphPane.CurveList.Clear();
        for (int index = 0; index < Num + 1; ++index)
          this.MyChart_LoadData(DataGet[index].DataX, DataGet[index].DataE, this.DrawColor[index], "Energy");
      }
      else
        this.MyChart.GraphPane.CurveList.Clear();
    }

    private void Draw(string str, Spectrometer.Data[] DataGet, int Num, string[] DataName)
    {
      if (DataGet != null && DataGet[Num].DataX != null)
      {
        if (DataGet[Num].DataE == null)
          return;
        this.MyChart.GraphPane = new GraphPane(this.MyChart.GraphPane.Rect, str, "波数", "能量");
        this.MyChart.GraphPane.CurveList.Clear();
        for (int index = 0; index < Num + 1; ++index)
          this.MyChart_LoadData(DataGet[index].DataX, DataGet[index].DataE, this.DrawColor[index], DataName[index]);
      }
      else
        this.MyChart.GraphPane.CurveList.Clear();
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

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      Spectrometer.SpecInfo SpInfo = (Spectrometer.SpecInfo) e.Argument;
      try
      {
        this.MSetSpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, false);
      }
      catch
      {
        return;
      }
      if (this.AutoFindIntegrationTimes && this.SpInfoTmpt.DataA != null)
      {
        double num = 0.0;
        int peaks = this.FindPeaks(this.SpInfoTmpt);
        if (peaks < this.SpInfoTmpt.DataA.Length)
          num = this.SpInfoTmpt.DataA[peaks];
        if (this.AutoFindIntegrationTimeCount > 20)
        {
          this.AutoFindIntegrationTimes = false;
          this.AutoFindIntegrationTimes = false;
          this.DataIOS.SaveStr(Spectrometer.IntegrationTime.ToString() + "," + Spectrometer.ScanTimes.ToString() + "," + (object) Spectrometer.GainMode + "," + Spectrometer.SavePath + "," + (Spectrometer.isClearDarks ? 1 : 0).ToString() + "," + (Spectrometer.isCorrectElectricalDarks ? 1 : 0).ToString() + "," + (Spectrometer.isCorrectNonlinearitys ? 1 : 0).ToString(), Spectrometer.ApplicationPath + "\\Setting");
          e.Result = (object) SpInfo;
          this.AutoFindIntegrationTimess = true;
          return;
        }
        if (num > 2.0 && (num > 55000.0 || num < 54500.0))
        {
          Spectrometer.IntegrationTime = (int) Math.Round((double) Spectrometer.IntegrationTime * (55000.0 / num), 0);
          if (Spectrometer.IntegrationTime > 800000)
            Spectrometer.IntegrationTime = 800000;
        }
        else
        {
          this.AutoFindIntegrationTimes = false;
          this.DataIOS.SaveStr(Spectrometer.IntegrationTime.ToString() + "," + Spectrometer.ScanTimes.ToString() + "," + (object) Spectrometer.GainMode + "," + Spectrometer.SavePath + "," + (Spectrometer.isClearDarks ? 1 : 0).ToString() + "," + (Spectrometer.isCorrectElectricalDarks ? 1 : 0).ToString() + "," + (Spectrometer.isCorrectNonlinearitys ? 1 : 0).ToString(), Spectrometer.ApplicationPath + "\\Setting");
          e.Result = (object) SpInfo;
          this.AutoFindIntegrationTimess = true;
          return;
        }
      }
      e.Result = (object) SpInfo;
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (!this.timer1.Enabled)
        return;
      this.SpInfoTmpt = (Spectrometer.SpecInfo) e.Result;
      this.DrawEnergyValue();
      if (!this.AutoFindIntegrationTimes && this.AutoFindIntegrationTimess)
      {
        this.btnAutoIntegrationTime.Image = (Image) null;
        this.btnAutoIntegrationTime.Image = (Image) Resources.play;
        this.txtIntegrationTime.Text = Math.Round((double) Spectrometer.IntegrationTime / 1000.0, 1).ToString();
        this.AutoFindIntegrationTimess = false;
      }
    }

    private void cBCorrectElectricalDarks_CheckedChanged(object sender, EventArgs e)
    {
      this.MSetSpectrometer.SetParameters(Spectrometer.spectrometerIndex, Spectrometer.IntegrationTime);
    }

    private void cBCorrectNonlinearitys_CheckedChanged(object sender, EventArgs e)
    {
      this.MSetSpectrometer.SetParameters(Spectrometer.spectrometerIndex, Spectrometer.IntegrationTime);
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.btnSetOK_Click(sender, e);
      this.Close();
      this.Dispose();
    }

    private void btnGetConveySpec_Click(object sender, EventArgs e)
    {
      this.timer1.Enabled = false;
      this.btnGetConveySpec.Enabled = false;
      this.btnGetConveySpec.BackColor = Color.FromArgb(38, 136, 210);
      this.btnGetConveySpecStop.Enabled = true;
      this.GetConveySpecStartFlag = true;
      try
      {
        if (!Home.serialPortSetDevice.IsOpen)
          Home.serialPortSetDevice.Open();
        byte[] buffer1 = Home.SPControlWord["LightOn"];
        Home.serialPortSetDevice.Write(buffer1, 0, buffer1.Length);
        Thread.Sleep(500);
        byte[] buffer2 = Home.SPControlWord["ConveyorOn"];
        Home.serialPortSetDevice.Write(buffer2, 0, buffer2.Length);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
      this.bkwGetConveySpec.RunWorkerAsync();
    }

    private void btnGetConveySpecStop_Click(object sender, EventArgs e)
    {
      this.btnGetConveySpec.Enabled = true;
      this.btnGetConveySpecStop.Enabled = false;
      this.btnGetConveySpec.BackColor = Color.Transparent;
      this.GetConveySpecStartFlag = false;
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

    private void bkwGetConveySpec_DoWork(object sender, DoWorkEventArgs e)
    {
      while (this.GetConveySpecStartFlag)
      {
        this.MSetSpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref this.SpInfoTmpt, false);
        string key = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
        if (!this.ConveySpec.ContainsKey(key))
          this.ConveySpec.Add(key, this.SpInfoTmpt.DataA);
        this.DataIOS.TXTSaveData(Spectrometer.SavePath + "\\conveyor" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff") + ".txt", this.SpInfoTmpt.DataX, this.SpInfoTmpt.DataA);
      }
    }

    private void bkwGetConveySpec_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      double[,] X = new double[this.ConveySpec.Count, this.SpInfoTmpt.numPixls];
      int num1 = 0;
      int index1 = 0;
      foreach (KeyValuePair<string, double[]> keyValuePair in this.ConveySpec)
      {
        ++num1;
        for (int index2 = 0; index2 < this.SpInfoTmpt.numPixls; ++index2)
          X[index1, index2] = keyValuePair.Value[index2];
        ++index1;
        this.DataIOS.TXTSaveData(Spectrometer.SavePath + "\\conveyor" + keyValuePair.Key.ToString() + ".txt", this.SpInfoTmpt.DataX, this.SpInfoTmpt.DataA);
      }
      Spectrometer.Data[] DataGet = new Spectrometer.Data[1];
      DataGet[0].DataE = this.DataHandlingmy.SpMean(X);
      DataGet[0].DataX = this.SpInfoTmpt.DataX;
      this.DataIOS.TXTSaveData(Environment.CurrentDirectory.ToString() + "\\conveyor.txt", DataGet[0].DataX, DataGet[0].DataE);
      this.DataIOS.TXTSaveData(Spectrometer.SavePath + "\\conveyor.txt", DataGet[0].DataX, DataGet[0].DataE);
      this.MyChart.GraphPane.CurveList.Clear();
      this.Draw("皮带平均能量值", DataGet, 0);
      int num2 = (int) MessageBox.Show("皮带光谱保存完毕，请选择蚕蛹光谱！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void btnSelectWormSpec_Click(object sender, EventArgs e)
    {
      this.timer1.Enabled = false;
      this.openFileDialog1.Title = " 打开";
      this.openFileDialog1.Multiselect = true;
      this.openFileDialog1.Filter = " txt files(*.txt)|*.txt|All files(*.*)|*.*";
      this.openFileDialog1.CheckFileExists = true;
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.openFileDialog1.RestoreDirectory = true;
      string fileName = this.openFileDialog1.FileName;
      Spectrometer.Data[] dataArray1 = new Spectrometer.Data[1];
      Spectrometer.Data[] dataArray2 = new Spectrometer.Data[1];
      int length = this.DataIOS.TXTReadData(fileName, ref dataArray1[0].DataX, ref dataArray1[0].DataE, true);
      dataArray1[0].DataX = new double[length];
      dataArray1[0].DataE = new double[length];
      this.DataIOS.TXTReadData(fileName, ref dataArray1[0].DataX, ref dataArray1[0].DataE, false);
      if (length == this.DataIOS.TXTReadData(Environment.CurrentDirectory.ToString() + "\\conveyor.txt", ref dataArray2[0].DataX, ref dataArray2[0].DataE, true))
      {
        dataArray2[0].DataX = new double[length];
        dataArray2[0].DataE = new double[length];
        this.DataIOS.TXTReadData(Environment.CurrentDirectory.ToString() + "\\conveyor.txt", ref dataArray2[0].DataX, ref dataArray2[0].DataE, false);
        Spectrometer.Data[] dataArray3 = new Spectrometer.Data[1];
        dataArray3[0].DataX = new double[length];
        dataArray3[0].DataE = new double[length];
        dataArray3[0].DataX = dataArray2[0].DataX;
        for (int index = 0; index < length; ++index)
          dataArray3[0].DataE[index] = Math.Abs(dataArray1[0].DataE[index] - dataArray2[0].DataE[index]);
        double[] max;
        this.wavelengthIndex = this.DataHandlingmy.MaxValueIndex(dataArray3[0].DataE, 50, out max);
        for (int index = 0; index < this.wavelengthIndex.Length; ++index)
          this.listBoxWaveLenth.Items.Add((object) (dataArray3[0].DataX[this.wavelengthIndex[index]].ToString().Substring(0, 6) + "  " + max[index].ToString().Substring(0, 6)));
        this.txtThresholdDiff.Text = max[this.wavelengthIndex.Length - 1].ToString();
        Spectrometer.Data[] DataGet = new Spectrometer.Data[3];
        string[] DataName = new string[3];
        DataGet[0] = dataArray1[0];
        DataName[0] = "蚕蛹能量值";
        DataGet[1] = dataArray2[0];
        DataName[1] = "皮带能量值";
        DataGet[2] = dataArray3[0];
        DataName[2] = "能量差值";
        this.MyChart.GraphPane.CurveList.Clear();
        this.Draw("能量值", DataGet, 2, DataName);
      }
      else
      {
        int num = (int) MessageBox.Show("光谱维数不符，无法计算！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSetting));
      this.label1 = new System.Windows.Forms.Label();
      this.txtIntegrationTime = new TextBox();
      this.txtScanTimes = new TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnSetOK = new Button();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.label5 = new System.Windows.Forms.Label();
      this.txtSavePath = new TextBox();
      this.btnSelectSavePath = new Button();
      this.cBClearDarks = new CheckBox();
      this.cBCorrectElectricalDarks = new CheckBox();
      this.cBCorrectNonlinearitys = new CheckBox();
      this.MyChart = new ZedGraphControl();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.backgroundWorker1 = new BackgroundWorker();
      this.backgroundWorker2 = new BackgroundWorker();
      this.panelHeader = new Panel();
      this.btnClose = new Button();
      this.btnSaveDark = new Button();
      this.btnAutoIntegrationTime = new Button();
      this.txtIntegrationTimeBK = new TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.btnAutoIntegrationTimeBK = new Button();
      this.groupBox1 = new GroupBox();
      this.txtThresholdDiff = new TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.listBoxWaveLenth = new ListBox();
      this.label6 = new System.Windows.Forms.Label();
      this.btnSelectWormSpec = new Button();
      this.textBox1 = new TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.btnGetConveySpecStop = new Button();
      this.btnGetConveySpec = new Button();
      this.label8 = new System.Windows.Forms.Label();
      this.bkwGetConveySpec = new BackgroundWorker();
      this.openFileDialog1 = new OpenFileDialog();
      this.txtClearSensorIntervalTimes = new TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.panelHeader.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label1.Location = new Point(6, 74);
      this.label1.Name = "label1";
      this.label1.Size = new Size(125, 19);
      this.label1.TabIndex = 0;
      this.label1.Text = "积分时间(ms)";
      this.txtIntegrationTime.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.txtIntegrationTime.Location = new Point(128, 68);
      this.txtIntegrationTime.Name = "txtIntegrationTime";
      this.txtIntegrationTime.Size = new Size(68, 29);
      this.txtIntegrationTime.TabIndex = 1;
      this.txtScanTimes.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.txtScanTimes.Location = new Point(415, 112);
      this.txtScanTimes.Name = "txtScanTimes";
      this.txtScanTimes.Size = new Size(42, 29);
      this.txtScanTimes.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label2.Location = new Point(324, 117);
      this.label2.Name = "label2";
      this.label2.Size = new Size(85, 19);
      this.label2.TabIndex = 2;
      this.label2.Text = "扫描次数";
      this.btnSetOK.Location = new Point(416, 179);
      this.btnSetOK.Name = "btnSetOK";
      this.btnSetOK.Size = new Size(45, 37);
      this.btnSetOK.TabIndex = 6;
      this.btnSetOK.Text = "确定";
      this.btnSetOK.UseVisualStyleBackColor = true;
      this.btnSetOK.Click += new EventHandler(this.btnSetOK_Click);
      this.label5.AutoSize = true;
      this.label5.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label5.Location = new Point(6, 185);
      this.label5.Name = "label5";
      this.label5.Size = new Size(85, 19);
      this.label5.TabIndex = 9;
      this.label5.Text = "保存路径";
      this.txtSavePath.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.txtSavePath.Location = new Point(97, 182);
      this.txtSavePath.Name = "txtSavePath";
      this.txtSavePath.Size = new Size(260, 29);
      this.txtSavePath.TabIndex = 10;
      this.btnSelectSavePath.Location = new Point(363, 179);
      this.btnSelectSavePath.Name = "btnSelectSavePath";
      this.btnSelectSavePath.Size = new Size(46, 37);
      this.btnSelectSavePath.TabIndex = 11;
      this.btnSelectSavePath.Text = "浏览";
      this.btnSelectSavePath.UseVisualStyleBackColor = true;
      this.btnSelectSavePath.Click += new EventHandler(this.btnSelectSavePath_Click);
      this.cBClearDarks.AutoSize = true;
      this.cBClearDarks.Font = new Font("宋体", 14.25f);
      this.cBClearDarks.Location = new Point(13, 148);
      this.cBClearDarks.Margin = new Padding(2);
      this.cBClearDarks.Name = "cBClearDarks";
      this.cBClearDarks.Size = new Size(123, 23);
      this.cBClearDarks.TabIndex = 12;
      this.cBClearDarks.Text = "去除暗噪声";
      this.cBClearDarks.UseVisualStyleBackColor = true;
      this.cBClearDarks.CheckedChanged += new EventHandler(this.cBClearDarks_CheckedChanged);
      this.cBCorrectElectricalDarks.AutoSize = true;
      this.cBCorrectElectricalDarks.Font = new Font("宋体", 14.25f);
      this.cBCorrectElectricalDarks.Location = new Point(181, 148);
      this.cBCorrectElectricalDarks.Margin = new Padding(2);
      this.cBCorrectElectricalDarks.Name = "cBCorrectElectricalDarks";
      this.cBCorrectElectricalDarks.Size = new Size(123, 23);
      this.cBCorrectElectricalDarks.TabIndex = 13;
      this.cBCorrectElectricalDarks.Text = "杂散光矫正";
      this.cBCorrectElectricalDarks.UseVisualStyleBackColor = true;
      this.cBCorrectElectricalDarks.CheckedChanged += new EventHandler(this.cBCorrectElectricalDarks_CheckedChanged);
      this.cBCorrectNonlinearitys.AutoSize = true;
      this.cBCorrectNonlinearitys.Font = new Font("宋体", 14.25f);
      this.cBCorrectNonlinearitys.Location = new Point(338, 148);
      this.cBCorrectNonlinearitys.Margin = new Padding(2);
      this.cBCorrectNonlinearitys.Name = "cBCorrectNonlinearitys";
      this.cBCorrectNonlinearitys.Size = new Size(123, 23);
      this.cBCorrectNonlinearitys.TabIndex = 14;
      this.cBCorrectNonlinearitys.Text = "非线性校正";
      this.cBCorrectNonlinearitys.UseVisualStyleBackColor = true;
      this.cBCorrectNonlinearitys.CheckedChanged += new EventHandler(this.cBCorrectNonlinearitys_CheckedChanged);
      this.MyChart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.MyChart.Location = new Point(10, 222);
      this.MyChart.Margin = new Padding(4, 3, 4, 3);
      this.MyChart.Name = "MyChart";
      this.MyChart.ScrollGrace = 0.0;
      this.MyChart.ScrollMaxX = 0.0;
      this.MyChart.ScrollMaxY = 0.0;
      this.MyChart.ScrollMaxY2 = 0.0;
      this.MyChart.ScrollMinX = 0.0;
      this.MyChart.ScrollMinY = 0.0;
      this.MyChart.ScrollMinY2 = 0.0;
      this.MyChart.Size = new Size(476, 366);
      this.MyChart.TabIndex = 15;
      this.timer1.Interval = 1000;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.backgroundWorker1.WorkerSupportsCancellation = true;
      this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      this.backgroundWorker2.WorkerSupportsCancellation = true;
      this.backgroundWorker2.DoWork += new DoWorkEventHandler(this.backgroundWorker2_DoWork);
      this.panelHeader.BackColor = Color.FromArgb(38, 136, 210);
      this.panelHeader.Controls.Add((Control) this.btnClose);
      this.panelHeader.Location = new Point(0, 0);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(770, 44);
      this.panelHeader.TabIndex = 29;
      this.btnClose.Anchor = AnchorStyles.Right;
      this.btnClose.BackColor = Color.Transparent;
      this.btnClose.BackgroundImage = (Image) Resources.Home;
      this.btnClose.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnClose.FlatStyle = FlatStyle.Flat;
      this.btnClose.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnClose.Location = new Point(718, 0);
      this.btnClose.Margin = new Padding(4);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(47, 44);
      this.btnClose.TabIndex = 64;
      this.btnClose.UseVisualStyleBackColor = false;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnSaveDark.BackgroundImage = (Image) Resources.save;
      this.btnSaveDark.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnSaveDark.Location = new Point(140, 147);
      this.btnSaveDark.Margin = new Padding(2);
      this.btnSaveDark.Name = "btnSaveDark";
      this.btnSaveDark.Size = new Size(22, 24);
      this.btnSaveDark.TabIndex = 17;
      this.btnSaveDark.UseVisualStyleBackColor = true;
      this.btnSaveDark.Click += new EventHandler(this.btnSaveDark_Click);
      this.btnSaveDark.MouseEnter += new EventHandler(this.btnSaveDark_MouseEnter);
      this.btnAutoIntegrationTime.BackgroundImage = (Image) Resources.play;
      this.btnAutoIntegrationTime.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnAutoIntegrationTime.Location = new Point(201, 69);
      this.btnAutoIntegrationTime.Margin = new Padding(2);
      this.btnAutoIntegrationTime.Name = "btnAutoIntegrationTime";
      this.btnAutoIntegrationTime.Size = new Size(31, 29);
      this.btnAutoIntegrationTime.TabIndex = 16;
      this.btnAutoIntegrationTime.UseVisualStyleBackColor = true;
      this.btnAutoIntegrationTime.Click += new EventHandler(this.btnAutoIntegrationTime_Click);
      this.btnAutoIntegrationTime.MouseEnter += new EventHandler(this.btnAutoIntegrationTime_MouseEnter);
      this.txtIntegrationTimeBK.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.txtIntegrationTimeBK.Location = new Point(168, 109);
      this.txtIntegrationTimeBK.Name = "txtIntegrationTimeBK";
      this.txtIntegrationTimeBK.Size = new Size(47, 29);
      this.txtIntegrationTimeBK.TabIndex = 31;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label3.Location = new Point(6, 112);
      this.label3.Name = "label3";
      this.label3.Size = new Size(163, 19);
      this.label3.TabIndex = 30;
      this.label3.Text = "背景积分时间(ms)";
      this.btnAutoIntegrationTimeBK.BackgroundImage = (Image) Resources.play;
      this.btnAutoIntegrationTimeBK.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnAutoIntegrationTimeBK.Location = new Point(220, 107);
      this.btnAutoIntegrationTimeBK.Margin = new Padding(2);
      this.btnAutoIntegrationTimeBK.Name = "btnAutoIntegrationTimeBK";
      this.btnAutoIntegrationTimeBK.Size = new Size(30, 29);
      this.btnAutoIntegrationTimeBK.TabIndex = 32;
      this.btnAutoIntegrationTimeBK.UseVisualStyleBackColor = true;
      this.btnAutoIntegrationTimeBK.Click += new EventHandler(this.btnAutoIntegrationTimeBK_Click);
      this.groupBox1.Controls.Add((Control) this.txtThresholdDiff);
      this.groupBox1.Controls.Add((Control) this.label7);
      this.groupBox1.Controls.Add((Control) this.listBoxWaveLenth);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.btnSelectWormSpec);
      this.groupBox1.Controls.Add((Control) this.textBox1);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.btnGetConveySpecStop);
      this.groupBox1.Controls.Add((Control) this.btnGetConveySpec);
      this.groupBox1.Controls.Add((Control) this.label8);
      this.groupBox1.Font = new Font("宋体", 14.25f);
      this.groupBox1.Location = new Point(479, 69);
      this.groupBox1.Margin = new Padding(2);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(2);
      this.groupBox1.Size = new Size(275, 520);
      this.groupBox1.TabIndex = 33;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "蚕蛹光谱辨别与相关参数设置";
      this.txtThresholdDiff.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.txtThresholdDiff.Location = new Point(91, 479);
      this.txtThresholdDiff.Name = "txtThresholdDiff";
      this.txtThresholdDiff.Size = new Size(124, 29);
      this.txtThresholdDiff.TabIndex = 51;
      this.label7.Location = new Point(12, 216);
      this.label7.Margin = new Padding(2, 0, 2, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(163, 22);
      this.label7.TabIndex = 49;
      this.label7.Text = "推荐的波长点：";
      this.listBoxWaveLenth.FormattingEnabled = true;
      this.listBoxWaveLenth.HorizontalScrollbar = true;
      this.listBoxWaveLenth.ItemHeight = 19;
      this.listBoxWaveLenth.Location = new Point(15, 247);
      this.listBoxWaveLenth.Margin = new Padding(2);
      this.listBoxWaveLenth.Name = "listBoxWaveLenth";
      this.listBoxWaveLenth.SelectionMode = SelectionMode.MultiSimple;
      this.listBoxWaveLenth.Size = new Size(212, 213);
      this.listBoxWaveLenth.TabIndex = 48;
      this.label6.Location = new Point(11, 125);
      this.label6.Margin = new Padding(2, 0, 2, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(163, 22);
      this.label6.TabIndex = 47;
      this.label6.Text = "需对比的蚕蛹光谱";
      this.btnSelectWormSpec.FlatStyle = FlatStyle.Flat;
      this.btnSelectWormSpec.Font = new Font("宋体", 12f);
      this.btnSelectWormSpec.Location = new Point(196, 149);
      this.btnSelectWormSpec.Name = "btnSelectWormSpec";
      this.btnSelectWormSpec.Size = new Size(51, 34);
      this.btnSelectWormSpec.TabIndex = 46;
      this.btnSelectWormSpec.Text = "选择";
      this.btnSelectWormSpec.UseVisualStyleBackColor = true;
      this.btnSelectWormSpec.Click += new EventHandler(this.btnSelectWormSpec_Click);
      this.textBox1.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.textBox1.Location = new Point(14, 153);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(176, 29);
      this.textBox1.TabIndex = 45;
      this.label4.Location = new Point(12, 36);
      this.label4.Margin = new Padding(2, 0, 2, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(64, 65);
      this.label4.TabIndex = 44;
      this.label4.Text = "采集皮带能量";
      this.btnGetConveySpecStop.BackColor = Color.Transparent;
      this.btnGetConveySpecStop.BackgroundImage = (Image) Resources.Pause;
      this.btnGetConveySpecStop.BackgroundImageLayout = ImageLayout.Stretch;
      this.btnGetConveySpecStop.Enabled = false;
      this.btnGetConveySpecStop.FlatStyle = FlatStyle.Flat;
      this.btnGetConveySpecStop.ForeColor = Color.Transparent;
      this.btnGetConveySpecStop.Location = new Point(169, 37);
      this.btnGetConveySpecStop.Margin = new Padding(0);
      this.btnGetConveySpecStop.Name = "btnGetConveySpecStop";
      this.btnGetConveySpecStop.Size = new Size(58, 64);
      this.btnGetConveySpecStop.TabIndex = 43;
      this.btnGetConveySpecStop.UseVisualStyleBackColor = false;
      this.btnGetConveySpecStop.Click += new EventHandler(this.btnGetConveySpecStop_Click);
      this.btnGetConveySpec.Anchor = AnchorStyles.Right;
      this.btnGetConveySpec.BackColor = Color.Transparent;
      this.btnGetConveySpec.BackgroundImage = (Image) Resources.Play2;
      this.btnGetConveySpec.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnGetConveySpec.FlatStyle = FlatStyle.Flat;
      this.btnGetConveySpec.ForeColor = Color.Transparent;
      this.btnGetConveySpec.Location = new Point(91, 38);
      this.btnGetConveySpec.Margin = new Padding(4);
      this.btnGetConveySpec.Name = "btnGetConveySpec";
      this.btnGetConveySpec.Size = new Size(62, 64);
      this.btnGetConveySpec.TabIndex = 40;
      this.btnGetConveySpec.UseVisualStyleBackColor = false;
      this.btnGetConveySpec.Click += new EventHandler(this.btnGetConveySpec_Click);
      this.label8.Location = new Point(29, 482);
      this.label8.Margin = new Padding(2, 0, 2, 0);
      this.label8.Name = "label8";
      this.label8.Size = new Size(100, 22);
      this.label8.TabIndex = 50;
      this.label8.Text = "阈值：";
      this.bkwGetConveySpec.WorkerSupportsCancellation = true;
      this.bkwGetConveySpec.DoWork += new DoWorkEventHandler(this.bkwGetConveySpec_DoWork);
      this.bkwGetConveySpec.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bkwGetConveySpec_RunWorkerCompleted);
      this.openFileDialog1.FileName = "openFileDialog1";
      this.txtClearSensorIntervalTimes.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.txtClearSensorIntervalTimes.Location = new Point(400, 68);
      this.txtClearSensorIntervalTimes.Name = "txtClearSensorIntervalTimes";
      this.txtClearSensorIntervalTimes.Size = new Size(43, 29);
      this.txtClearSensorIntervalTimes.TabIndex = 35;
      this.label9.AutoSize = true;
      this.label9.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label9.Location = new Point(243, 74);
      this.label9.Name = "label9";
      this.label9.Size = new Size(230, 19);
      this.label9.TabIndex = 34;
      this.label9.Text = "探头清洗间隔时间     分";
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(768, 614);
      this.Controls.Add((Control) this.txtClearSensorIntervalTimes);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.btnAutoIntegrationTimeBK);
      this.Controls.Add((Control) this.txtIntegrationTimeBK);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.panelHeader);
      this.Controls.Add((Control) this.btnSaveDark);
      this.Controls.Add((Control) this.btnAutoIntegrationTime);
      this.Controls.Add((Control) this.MyChart);
      this.Controls.Add((Control) this.cBCorrectNonlinearitys);
      this.Controls.Add((Control) this.cBCorrectElectricalDarks);
      this.Controls.Add((Control) this.cBClearDarks);
      this.Controls.Add((Control) this.btnSelectSavePath);
      this.Controls.Add((Control) this.txtSavePath);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.btnSetOK);
      this.Controls.Add((Control) this.txtScanTimes);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtIntegrationTime);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FrmSetting);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "设置";
      this.FormClosing += new FormClosingEventHandler(this.FrmSetting_FormClosing);
      this.Load += new EventHandler(this.FrmSetting_Load);
      this.panelHeader.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate void GetThermitorDel();

    private delegate void DrawDelegate(string Str, Spectrometer.Data[] DataGet, int Num);

    private delegate void MyChartLoadData(double[] DataX, double[] DataY, Color ColorMy, string Name);
  }
}
