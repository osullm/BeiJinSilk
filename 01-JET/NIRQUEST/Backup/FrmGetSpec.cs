// Decompiled with JetBrains decompiler
// Type: JSDU.FrmGetSpec
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using NIRQUEST.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace JSDU
{
  public class FrmGetSpec : Form
  {
    private Spectrometer MySpectrometer = new Spectrometer();
    private DataIO DataIOmy = new DataIO();
    private DataHandling DataHandlingmy = new DataHandling();
    private Home frmHome = (Home) null;
    private string SpName = "";
    private FrmGetSpec.ViewStyle ViewStylemy = FrmGetSpec.ViewStyle.Spec;
    private int btnGetSpecClickNum = 0;
    private Color[] DrawColor = new Color[20];
    private IContainer components = (IContainer) null;
    public const byte FT_PURGE_RX = 1;
    public const byte FT_PURGE_TX = 2;
    private int numberOfSpectrometersFound;
    public static Spectrometer.SpecInfo SpInfo;
    private double[,] MeanY;
    private double[,] StdErrY;
    private FrmGetSpec.MyChartLoadData myChartLoadData;
    private System.Windows.Forms.Label lblDev;
    private Button btnSet;
    private Button btnGetSpec;
    private ProgressBar prgsBarGetEnergy;
    private Button btnBackGrd;
    private System.Windows.Forms.Label label1;
    private TextBox txtSpName;
    private GroupBox groupBox1;
    private TextBox txtMeanErr;
    private System.Windows.Forms.Label label3;
    private TextBox txtMaxStdErr;
    private System.Windows.Forms.Label label2;
    private Button btnMeanView;
    private Button btnStdErrView;
    private Button bntOriginView;
    private ComboBox comboBox1;
    private Button btnSave;
    private Button btnClear;
    private Button btnEnergyView;
    private SplitContainer splitContainer1;
    private ZedGraphControl MyChart;
    private System.Windows.Forms.Label label4;
    private ComboBox comboBox2;
    private BackgroundWorker backgroundWorker1;
    public System.Windows.Forms.Timer timer1;
    private BackgroundWorker backgroundWorker2;
    private Panel panelHeader;
    private Button btnClose;
    private Panel panel1;
    private Button btnSetting;
    private Button btnShowSpectrumOffline;

    public FrmGetSpec(Home home)
    {
      this.InitializeComponent();
      this.frmHome = home;
      Spectrometer.spectrometerIndex = -1;
      this.InitDrawDelegate();
      this.DrawColor[0] = Color.Blue;
      this.DrawColor[1] = Color.BlueViolet;
      this.DrawColor[2] = Color.Brown;
      this.DrawColor[3] = Color.Chocolate;
      this.DrawColor[4] = Color.Crimson;
      this.DrawColor[5] = Color.DarkBlue;
      this.DrawColor[6] = Color.DarkCyan;
      this.DrawColor[7] = Color.DarkGoldenrod;
      this.DrawColor[8] = Color.DarkGreen;
      this.DrawColor[9] = Color.Pink;
      this.DrawColor[10] = Color.DarkBlue;
      this.DrawColor[11] = Color.DarkGreen;
      this.DrawColor[12] = Color.DarkSlateGray;
      this.DrawColor[13] = Color.DarkOliveGreen;
      this.DrawColor[14] = Color.DarkOrange;
      this.DrawColor[15] = Color.DarkOrchid;
      this.DrawColor[16] = Color.DarkRed;
      this.DrawColor[17] = Color.DarkSlateBlue;
      this.DrawColor[18] = Color.ForestGreen;
      this.DrawColor[19] = Color.Indigo;
    }

    public void InitDrawDelegate()
    {
      this.myChartLoadData = new FrmGetSpec.MyChartLoadData(this.MyChart_LoadData);
    }

    private void FrmGetSpec_Load(object sender, EventArgs e)
    {
      Spectrometer.ApplicationPath = Directory.GetCurrentDirectory();
      this.numberOfSpectrometersFound = this.MySpectrometer.wrapper.openAllSpectrometers();
      this.prgsBarGetEnergy.Visible = false;
      for (int index = 0; index < Spectrometer.ScanTimes; ++index)
        this.comboBox2.Items.Add((object) (index + 1).ToString());
      this.comboBox2.SelectedIndex = Spectrometer.ScanTimes - 1;
      this.lblDev.Text = "设备：";
      if (this.numberOfSpectrometersFound > 0)
      {
        Spectrometer.spectrometerIndex = 0;
        this.lblDev.Text = "设备：" + this.MySpectrometer.wrapper.getName(Spectrometer.spectrometerIndex);
        this.MySpectrometer.wrapper.setAutoToggleStrobeLampEnable(Spectrometer.spectrometerIndex, (short) 1);
      }
      else
      {
        int num = (int) MessageBox.Show("仪器连接错误，请检查连接！");
        this.Close();
      }
      this.MySpectrometer.ReadBK(ref FrmGetSpec.SpInfo);
      this.MySpectrometer.ReadSetParameters();
      if (Spectrometer.isClearDarks)
        this.MySpectrometer.ReadDK(ref FrmGetSpec.SpInfo);
      this.timer1.Enabled = true;
    }

    private void btnSet_Click(object sender, EventArgs e)
    {
      this.timer1.Enabled = false;
      int num = (int) new FrmSetting(this.MySpectrometer, this, this.frmHome).ShowDialog();
    }

    private void FrmGetSpec_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.backgroundWorker1.IsBusy)
        this.backgroundWorker1.CancelAsync();
      this.DataIOmy.SaveStr(Spectrometer.IntegrationTime.ToString() + "," + Spectrometer.ScanTimes.ToString() + "," + (object) Spectrometer.GainMode + "," + Spectrometer.SavePath + "," + (Spectrometer.isClearDarks ? 1 : 0).ToString() + "," + (Spectrometer.isCorrectElectricalDarks ? 1 : 0).ToString() + "," + (Spectrometer.isCorrectNonlinearitys ? 1 : 0).ToString(), Spectrometer.ApplicationPath + "\\Setting");
      this.MySpectrometer.wrapper.closeAllSpectrometers();
    }

    private void btnGetSpec_Click(object sender, EventArgs e)
    {
      if (this.txtSpName.Text == "")
        this.txtSpName.Text = DateTime.Now.ToString("MM-dd") + " " + DateTime.Now.ToString("HH-mm");
      this.timer1.Enabled = false;
      if (this.btnGetSpecClickNum == 0)
      {
        this.SpName = this.txtSpName.Text;
        Spectrometer.DataGet = new Spectrometer.Data[20];
        this.MeanY = new double[20, FrmGetSpec.SpInfo.DataAB.Length];
        this.StdErrY = new double[20, FrmGetSpec.SpInfo.DataAB.Length];
      }
      else if (this.txtSpName.Text.IndexOf("-") < 0)
      {
        this.SpName = this.txtSpName.Text;
        this.btnGetSpecClickNum = 0;
      }
      this.Invoke((Delegate) new EventHandler(this.updatatxtSpNameUI), (object) this.txtSpName, (object) EventArgs.Empty);
      this.prgsBarGetEnergy.Visible = true;
      this.prgsBarGetEnergy.Maximum = 5;
      if (!this.backgroundWorker2.IsBusy)
      {
        this.backgroundWorker2.RunWorkerAsync();
      }
      else
      {
        int num = (int) MessageBox.Show("光谱正在采集，请勿重复点击！谢谢！");
      }
    }

    private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
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
      }
      e.Result = (object) this.MySpectrometer.GetSpec(Spectrometer.spectrometerIndex, ref FrmGetSpec.SpInfo, Spectrometer.ScanTimes, this.backgroundWorker2);
    }

    private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.prgsBarGetEnergy.Value = e.ProgressPercentage;
    }

    private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.prgsBarGetEnergy.Visible = false;
      this.prgsBarGetEnergy.Value = 0;
      if ((bool) e.Result)
      {
        if (FrmGetSpec.SpInfo.DataAB.Length == FrmGetSpec.SpInfo.DataA.Length)
        {
          FrmGetSpec.SpInfo.DataY = new double[FrmGetSpec.SpInfo.numPixls];
          for (int index = 0; index < FrmGetSpec.SpInfo.DataA.Length; ++index)
          {
            try
            {
              FrmGetSpec.SpInfo.DataY[index] = Convert.ToDouble(Math.Log10(Math.Abs((FrmGetSpec.SpInfo.DataAB[index] - FrmGetSpec.SpInfo.DataAD[index]) / (FrmGetSpec.SpInfo.DataA[index] - FrmGetSpec.SpInfo.DataAD[index]))));
            }
            catch
            {
              FrmGetSpec.SpInfo.DataY[index] = index <= 0 ? 0.0 : FrmGetSpec.SpInfo.DataY[index - 1];
            }
          }
          FrmGetSpec.SpInfo.w1 = FrmGetSpec.SpInfo.DataX[0];
          FrmGetSpec.SpInfo.w2 = FrmGetSpec.SpInfo.DataX[FrmGetSpec.SpInfo.DataX.Length - 1];
          Spectrometer.DataGet[this.btnGetSpecClickNum].DataX = FrmGetSpec.SpInfo.DataX;
          Spectrometer.DataGet[this.btnGetSpecClickNum].DataY = FrmGetSpec.SpInfo.DataY;
          Spectrometer.DataGet[this.btnGetSpecClickNum].DataE = FrmGetSpec.SpInfo.DataA;
          this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\" + this.SpName + "-" + (this.btnGetSpecClickNum + 1).ToString() + ".txt", FrmGetSpec.SpInfo.DataX, FrmGetSpec.SpInfo.DataY);
          this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\" + this.SpName + "-Energy-" + (this.btnGetSpecClickNum + 1).ToString() + ".txt", FrmGetSpec.SpInfo.DataX, FrmGetSpec.SpInfo.DataA);
          this.ViewStylemy = FrmGetSpec.ViewStyle.Spec;
          this.BeginInvoke((Delegate) new FrmGetSpec.DrawDelegate(this.Draw), (object) "光谱图", (object) Spectrometer.DataGet, (object) this.btnGetSpecClickNum);
          double[,] X = new double[this.btnGetSpecClickNum + 1, FrmGetSpec.SpInfo.numPixls];
          for (int index1 = 0; index1 < this.btnGetSpecClickNum + 1; ++index1)
          {
            for (int index2 = 0; index2 < FrmGetSpec.SpInfo.numPixls; ++index2)
              X[index1, index2] = Spectrometer.DataGet[index1].DataY[index2];
          }
          double[] numArray = this.DataHandlingmy.SpMean(X);
          double[] array = this.DataHandlingmy.SpStdError(X);
          for (int index = 0; index < numArray.Length; ++index)
            this.MeanY[this.btnGetSpecClickNum, index] = numArray[index];
          for (int index = 0; index < numArray.Length; ++index)
            this.StdErrY[this.btnGetSpecClickNum, index] = array[index];
          this.txtMeanErr.Text = this.DataHandlingmy.MeanValue(array).ToString("0.000e0");
          this.txtMaxStdErr.Text = this.DataHandlingmy.MaxValue(array).ToString("0.000e0");
          this.comboBox1.Items.Add((object) (this.btnGetSpecClickNum + 1).ToString());
          this.comboBox1.SelectedIndex = this.btnGetSpecClickNum;
          ++this.btnGetSpecClickNum;
          if (this.btnGetSpecClickNum <= 19)
            return;
          this.btnGetSpecClickNum = 0;
        }
        else
        {
          int num1 = (int) MessageBox.Show("背景维数不符，请重新采集！");
        }
      }
      else
      {
        int num2 = (int) MessageBox.Show("光谱采集失败，请重新采集！");
      }
    }

    private void updatatxtSpNameUI(object o, EventArgs e)
    {
      this.txtSpName.Text = this.SpName + "-" + (this.btnGetSpecClickNum + 1).ToString();
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
        return;
      }
      this.prgsBarGetEnergy.Visible = true;
      this.MySpectrometer._ProgressBar = this.prgsBarGetEnergy;
      this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref FrmGetSpec.SpInfo, true);
      FrmGetSpec.SpInfo.DataAB = FrmGetSpec.SpInfo.DataA;
      FrmGetSpec.SpInfo.DataA = (double[]) null;
      this.DataIOmy.TXTSaveData(Environment.CurrentDirectory.ToString() + "\\background", FrmGetSpec.SpInfo.DataX, FrmGetSpec.SpInfo.DataAB);
      Spectrometer.Data[] DataGet = new Spectrometer.Data[1];
      DataGet[0].DataX = FrmGetSpec.SpInfo.WavelengthArray;
      DataGet[0].DataE = FrmGetSpec.SpInfo.DataA;
      this.ViewStylemy = FrmGetSpec.ViewStyle.Energy;
      this.BeginInvoke((Delegate) new FrmGetSpec.DrawDelegate(this.Draw), (object) "能量图", (object) DataGet, (object) 0);
      this.Draw("背景能量图", DataGet, 0);
      this.MySpectrometer.ReadDK(ref FrmGetSpec.SpInfo);
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
        if (this.ViewStylemy == FrmGetSpec.ViewStyle.Energy && DataGet[Num].DataE != null)
        {
          RectangleF rect = this.MyChart.GraphPane.Rect;
          if (this.ViewStylemy == FrmGetSpec.ViewStyle.Energy)
            this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "能量");
          this.MyChart.GraphPane.CurveList.Clear();
          for (int index = 0; index < Num + 1; ++index)
            this.MyChart.Invoke((Delegate) this.myChartLoadData, (object) DataGet[index].DataX, (object) DataGet[index].DataE, (object) this.DrawColor[index], (object) (this.SpName + "-Energy-" + (index + 1).ToString()));
        }
        else
        {
          RectangleF rect = this.MyChart.GraphPane.Rect;
          if (this.ViewStylemy == FrmGetSpec.ViewStyle.Spec)
          {
            this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "吸光度");
            this.MyChart.GraphPane.CurveList.Clear();
            for (int index = 0; index < Num + 1; ++index)
              this.MyChart.Invoke((Delegate) this.myChartLoadData, (object) DataGet[index].DataX, (object) DataGet[index].DataY, (object) this.DrawColor[index], (object) (this.SpName + "-" + (index + 1).ToString()));
          }
          else if (this.ViewStylemy == FrmGetSpec.ViewStyle.Mean && Num == 0)
          {
            this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "吸光度");
            this.MyChart.GraphPane.CurveList.Clear();
            for (int index = 0; index < Num + 1; ++index)
              this.MyChart.Invoke((Delegate) this.myChartLoadData, (object) DataGet[index].DataX, (object) DataGet[index].DataY, (object) this.DrawColor[index], (object) (this.SpName + "平均光谱"));
          }
          else if (this.ViewStylemy == FrmGetSpec.ViewStyle.StdErr && Num == 0)
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

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
      Spectrometer.ScanTimes = this.comboBox2.SelectedIndex + 1;
    }

    private void ReDraw(object sender, EventArgs e)
    {
      if (this.ViewStylemy == FrmGetSpec.ViewStyle.Spec)
      {
        FrmGetSpec.DrawDelegate drawDelegate = new FrmGetSpec.DrawDelegate(this.Draw);
        int num = this.btnGetSpecClickNum - 1 < 0 ? 0 : this.btnGetSpecClickNum - 1;
        if (Spectrometer.DataGet.Length <= 0)
          return;
        this.BeginInvoke((Delegate) drawDelegate, (object) "光谱图", (object) Spectrometer.DataGet, (object) num);
      }
      else if (this.ViewStylemy == FrmGetSpec.ViewStyle.Energy)
      {
        FrmGetSpec.DrawDelegate drawDelegate = new FrmGetSpec.DrawDelegate(this.Draw);
        int num = this.btnGetSpecClickNum - 1 < 0 ? 0 : this.btnGetSpecClickNum - 1;
        if (Spectrometer.DataGet == null)
          return;
        this.BeginInvoke((Delegate) drawDelegate, (object) "能量图", (object) Spectrometer.DataGet, (object) num);
      }
      else if (this.ViewStylemy == FrmGetSpec.ViewStyle.Mean)
      {
        this.btnMeanView_Click(sender, e);
      }
      else
      {
        if (this.ViewStylemy != FrmGetSpec.ViewStyle.StdErr)
          return;
        this.btnStdErrView_Click(sender, e);
      }
    }

    private void btnMeanView_Click(object sender, EventArgs e)
    {
      FrmGetSpec.DrawDelegate drawDelegate = new FrmGetSpec.DrawDelegate(this.Draw);
      int selectedIndex = this.comboBox1.SelectedIndex;
      Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
      if (Spectrometer.DataGet == null)
        return;
      dataArray[0].DataX = Spectrometer.DataGet[selectedIndex].DataX;
      dataArray[0].DataY = new double[dataArray[0].DataX.Length];
      for (int index = 0; index < dataArray[0].DataX.Length; ++index)
        dataArray[0].DataY[index] = this.MeanY[selectedIndex, index];
      this.ViewStylemy = FrmGetSpec.ViewStyle.Mean;
      this.BeginInvoke((Delegate) drawDelegate, (object) "平均谱图", (object) dataArray, (object) 0);
    }

    private void btnStdErrView_Click(object sender, EventArgs e)
    {
      FrmGetSpec.DrawDelegate drawDelegate = new FrmGetSpec.DrawDelegate(this.Draw);
      int selectedIndex = this.comboBox1.SelectedIndex;
      Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
      if (Spectrometer.DataGet == null)
        return;
      dataArray[0].DataX = Spectrometer.DataGet[selectedIndex].DataX;
      dataArray[0].DataY = new double[dataArray[0].DataX.Length];
      for (int index = 0; index < dataArray[0].DataX.Length; ++index)
        dataArray[0].DataY[index] = this.StdErrY[selectedIndex, index];
      this.ViewStylemy = FrmGetSpec.ViewStyle.StdErr;
      this.BeginInvoke((Delegate) drawDelegate, (object) "标准差图", (object) dataArray, (object) 0);
      this.txtMeanErr.Text = this.DataHandlingmy.MeanValue(dataArray[0].DataY).ToString("0.000e0");
      this.txtMaxStdErr.Text = this.DataHandlingmy.MaxValue(dataArray[0].DataY).ToString("0.000e0");
    }

    private void btnEnergyView_Click(object sender, EventArgs e)
    {
      this.ViewStylemy = FrmGetSpec.ViewStyle.Energy;
      this.ReDraw(sender, e);
    }

    private void bntOriginView_Click(object sender, EventArgs e)
    {
      this.ViewStylemy = FrmGetSpec.ViewStyle.Spec;
      this.ReDraw(sender, e);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      int selectedIndex = this.comboBox1.SelectedIndex;
      if (this.ViewStylemy == FrmGetSpec.ViewStyle.Mean)
      {
        double[] Data_y = new double[this.MeanY.GetLength(1)];
        for (int index = 0; index < this.MeanY.GetLength(1); ++index)
          Data_y[index] = this.MeanY[selectedIndex, index];
        this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\" + this.SpName + "-Mean-" + this.comboBox1.SelectedItem.ToString() + ".txt", Spectrometer.DataGet[selectedIndex].DataX, Data_y);
        int num = (int) MessageBox.Show("平均光谱：" + Spectrometer.SavePath + "\\" + this.SpName + "-Mean-" + this.comboBox1.SelectedItem.ToString() + ".txt保存成功");
      }
      else
      {
        if (this.ViewStylemy != FrmGetSpec.ViewStyle.StdErr)
          return;
        double[] Data_y = new double[this.MeanY.GetLength(1)];
        for (int index = 0; index < this.MeanY.GetLength(1); ++index)
          Data_y[index] = this.StdErrY[selectedIndex, index];
        this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\" + this.SpName + "-StdErr-" + this.comboBox1.SelectedItem.ToString() + ".txt", Spectrometer.DataGet[selectedIndex].DataX, Data_y);
        int num = (int) MessageBox.Show("标准差图：" + Spectrometer.SavePath + "\\" + this.SpName + "-StdErr-" + this.comboBox1.SelectedItem.ToString() + ".txt保存成功");
      }
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.comboBox1.Items.Clear();
      this.btnGetSpecClickNum = 0;
      this.txtSpName.Text = "";
      Spectrometer.DataGet = new Spectrometer.Data[20];
      this.MeanY = new double[20, FrmGetSpec.SpInfo.numPixls];
      this.StdErrY = new double[20, FrmGetSpec.SpInfo.numPixls];
      this.ViewStylemy = FrmGetSpec.ViewStyle.Spec;
      this.BeginInvoke((Delegate) new FrmGetSpec.DrawDelegate(this.Draw), (object) "光谱图", (object) Spectrometer.DataGet, (object) this.btnGetSpecClickNum);
      this.timer1.Enabled = true;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.comboBox1.Items.Count <= 0)
        return;
      if (this.ViewStylemy == FrmGetSpec.ViewStyle.StdErr)
        this.btnStdErrView_Click(sender, e);
      if (this.ViewStylemy == FrmGetSpec.ViewStyle.Mean)
        this.btnMeanView_Click(sender, e);
    }

    private void DrawEnergyValue()
    {
      for (int index = 0; index < FrmGetSpec.SpInfo.numPixls; ++index)
      {
        if (FrmGetSpec.SpInfo.WavelengthArray[index] > 10000000000.0)
          FrmGetSpec.SpInfo.WavelengthArray[index] = FrmGetSpec.SpInfo.WavelengthArray[index - 1];
        if (FrmGetSpec.SpInfo.DataA[index] > 10000000000.0)
          FrmGetSpec.SpInfo.DataA[index] = FrmGetSpec.SpInfo.DataA[index - 1];
      }
      FrmGetSpec.SpInfo.DataX = FrmGetSpec.SpInfo.WavelengthArray;
      FrmGetSpec.SpInfo.w1 = Math.Floor(FrmGetSpec.SpInfo.DataX[0] / 100.0) * 100.0;
      FrmGetSpec.SpInfo.w2 = Math.Ceiling(FrmGetSpec.SpInfo.DataX[FrmGetSpec.SpInfo.DataX.Length - 1] / 100.0) * 100.0;
      Spectrometer.Data[] DataGet = new Spectrometer.Data[1];
      DataGet[0].DataX = FrmGetSpec.SpInfo.WavelengthArray;
      DataGet[0].DataE = FrmGetSpec.SpInfo.DataA;
      this.ViewStylemy = FrmGetSpec.ViewStyle.Energy;
      FrmGetSpec.DrawDelegate drawDelegate = new FrmGetSpec.DrawDelegate(this.Draw);
      this.Draw("能量图", DataGet, 0);
      this.txtMeanErr.Text = this.DataHandlingmy.MeanValue(DataGet[0].DataE).ToString("0.000e0");
      this.txtMaxStdErr.Text = this.DataHandlingmy.MaxValue(DataGet[0].DataE).ToString("0.000e0");
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (Spectrometer.IntegrationTime > 500000)
        this.timer1.Interval = 3000;
      if (this.backgroundWorker1.IsBusy)
        return;
      this.backgroundWorker1.RunWorkerAsync((object) FrmGetSpec.SpInfo);
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      Spectrometer.SpecInfo SpInfo = (Spectrometer.SpecInfo) e.Argument;
      this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, false);
      e.Result = (object) SpInfo;
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (!this.timer1.Enabled)
        return;
      FrmGetSpec.SpInfo = (Spectrometer.SpecInfo) e.Result;
      this.DrawEnergyValue();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.timer1.Enabled = false;
      if (this.backgroundWorker1.IsBusy)
        this.backgroundWorker1.CancelAsync();
      this.backgroundWorker1.Dispose();
      this.Close();
      this.Dispose();
    }

    private void btnSetting_Click(object sender, EventArgs e)
    {
      int num = (int) new FrmSetting(this.MySpectrometer, this, this.frmHome).ShowDialog();
    }

    private void btnShowSpectrumOffline_Click(object sender, EventArgs e)
    {
      Home.serialPortSetDevice.Close();
      new Form_offLine().Show();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmGetSpec));
      this.lblDev = new System.Windows.Forms.Label();
      this.btnSet = new Button();
      this.btnGetSpec = new Button();
      this.prgsBarGetEnergy = new ProgressBar();
      this.btnBackGrd = new Button();
      this.label1 = new System.Windows.Forms.Label();
      this.txtSpName = new TextBox();
      this.groupBox1 = new GroupBox();
      this.txtMeanErr = new TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtMaxStdErr = new TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnMeanView = new Button();
      this.btnStdErrView = new Button();
      this.bntOriginView = new Button();
      this.comboBox1 = new ComboBox();
      this.btnClear = new Button();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.btnEnergyView = new Button();
      this.splitContainer1 = new SplitContainer();
      this.panel1 = new Panel();
      this.MyChart = new ZedGraphControl();
      this.panelHeader = new Panel();
      this.btnShowSpectrumOffline = new Button();
      this.btnSetting = new Button();
      this.btnClose = new Button();
      this.label4 = new System.Windows.Forms.Label();
      this.comboBox2 = new ComboBox();
      this.btnSave = new Button();
      this.backgroundWorker1 = new BackgroundWorker();
      this.backgroundWorker2 = new BackgroundWorker();
      this.groupBox1.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.lblDev.Anchor = AnchorStyles.Right;
      this.lblDev.AutoSize = true;
      this.lblDev.Location = new Point(13, 109);
      this.lblDev.Margin = new Padding(5, 0, 5, 0);
      this.lblDev.Name = "lblDev";
      this.lblDev.Size = new Size(52, 15);
      this.lblDev.TabIndex = 0;
      this.lblDev.Text = "设备：";
      this.btnSet.Anchor = AnchorStyles.Right;
      this.btnSet.Location = new Point(228, 304);
      this.btnSet.Margin = new Padding(5, 5, 5, 5);
      this.btnSet.Name = "btnSet";
      this.btnSet.Size = new Size(68, 31);
      this.btnSet.TabIndex = 12;
      this.btnSet.Text = "设置";
      this.btnSet.UseVisualStyleBackColor = true;
      this.btnSet.Click += new EventHandler(this.btnSet_Click);
      this.btnGetSpec.Anchor = AnchorStyles.Right;
      this.btnGetSpec.Location = new Point(25, 721);
      this.btnGetSpec.Margin = new Padding(5, 5, 5, 5);
      this.btnGetSpec.Name = "btnGetSpec";
      this.btnGetSpec.Size = new Size(64, 31);
      this.btnGetSpec.TabIndex = 13;
      this.btnGetSpec.Text = "开始";
      this.btnGetSpec.UseVisualStyleBackColor = true;
      this.btnGetSpec.Click += new EventHandler(this.btnGetSpec_Click);
      this.prgsBarGetEnergy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.prgsBarGetEnergy.Location = new Point(7, 748);
      this.prgsBarGetEnergy.Margin = new Padding(5, 5, 5, 5);
      this.prgsBarGetEnergy.Name = "prgsBarGetEnergy";
      this.prgsBarGetEnergy.Size = new Size(669, 19);
      this.prgsBarGetEnergy.TabIndex = 14;
      this.btnBackGrd.Anchor = AnchorStyles.Right;
      this.btnBackGrd.Location = new Point(117, 721);
      this.btnBackGrd.Margin = new Padding(5, 5, 5, 5);
      this.btnBackGrd.Name = "btnBackGrd";
      this.btnBackGrd.Size = new Size(69, 31);
      this.btnBackGrd.TabIndex = 15;
      this.btnBackGrd.Text = "背景";
      this.btnBackGrd.UseVisualStyleBackColor = true;
      this.btnBackGrd.Click += new EventHandler(this.btnBackGrd_Click);
      this.label1.Anchor = AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 152);
      this.label1.Margin = new Padding(5, 0, 5, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(67, 15);
      this.label1.TabIndex = 17;
      this.label1.Text = "文件名：";
      this.txtSpName.Anchor = AnchorStyles.Right;
      this.txtSpName.Location = new Point(77, 149);
      this.txtSpName.Margin = new Padding(5, 5, 5, 5);
      this.txtSpName.Name = "txtSpName";
      this.txtSpName.Size = new Size(217, 25);
      this.txtSpName.TabIndex = 18;
      this.groupBox1.Anchor = AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.txtMeanErr);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.txtMaxStdErr);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Location = new Point(21, 206);
      this.groupBox1.Margin = new Padding(5, 5, 5, 5);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(5, 5, 5, 5);
      this.groupBox1.Size = new Size(276, 88);
      this.groupBox1.TabIndex = 20;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "标准差";
      this.txtMeanErr.Location = new Point(123, 49);
      this.txtMeanErr.Margin = new Padding(5, 5, 5, 5);
      this.txtMeanErr.Name = "txtMeanErr";
      this.txtMeanErr.Size = new Size(137, 25);
      this.txtMeanErr.TabIndex = 21;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(53, 52);
      this.label3.Margin = new Padding(5, 0, 5, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(52, 15);
      this.label3.TabIndex = 20;
      this.label3.Text = "平均：";
      this.txtMaxStdErr.Location = new Point(123, 14);
      this.txtMaxStdErr.Margin = new Padding(5, 5, 5, 5);
      this.txtMaxStdErr.Name = "txtMaxStdErr";
      this.txtMaxStdErr.Size = new Size(137, 25);
      this.txtMaxStdErr.TabIndex = 19;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(53, 19);
      this.label2.Margin = new Padding(5, 0, 5, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(52, 15);
      this.label2.TabIndex = 18;
      this.label2.Text = "最大：";
      this.btnMeanView.Anchor = AnchorStyles.Right;
      this.btnMeanView.Location = new Point(77, 512);
      this.btnMeanView.Margin = new Padding(5, 5, 5, 5);
      this.btnMeanView.Name = "btnMeanView";
      this.btnMeanView.Size = new Size(164, 42);
      this.btnMeanView.TabIndex = 21;
      this.btnMeanView.Text = "平均光谱";
      this.btnMeanView.UseVisualStyleBackColor = true;
      this.btnMeanView.Click += new EventHandler(this.btnMeanView_Click);
      this.btnStdErrView.Anchor = AnchorStyles.Right;
      this.btnStdErrView.Location = new Point(77, 581);
      this.btnStdErrView.Margin = new Padding(5, 5, 5, 5);
      this.btnStdErrView.Name = "btnStdErrView";
      this.btnStdErrView.Size = new Size(164, 42);
      this.btnStdErrView.TabIndex = 22;
      this.btnStdErrView.Text = "标准差图";
      this.btnStdErrView.UseVisualStyleBackColor = true;
      this.btnStdErrView.Click += new EventHandler(this.btnStdErrView_Click);
      this.bntOriginView.Anchor = AnchorStyles.Right;
      this.bntOriginView.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.bntOriginView.Location = new Point(25, 542);
      this.bntOriginView.Margin = new Padding(5, 5, 5, 5);
      this.bntOriginView.Name = "bntOriginView";
      this.bntOriginView.Size = new Size(49, 42);
      this.bntOriginView.TabIndex = 23;
      this.bntOriginView.Text = "<-";
      this.bntOriginView.UseVisualStyleBackColor = true;
      this.bntOriginView.Click += new EventHandler(this.bntOriginView_Click);
      this.comboBox1.Anchor = AnchorStyles.Right;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new Point(252, 552);
      this.comboBox1.Margin = new Padding(5, 5, 5, 5);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new Size(53, 23);
      this.comboBox1.TabIndex = 24;
      this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
      this.btnClear.Anchor = AnchorStyles.Right;
      this.btnClear.Location = new Point(211, 721);
      this.btnClear.Margin = new Padding(5, 5, 5, 5);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(60, 31);
      this.btnClear.TabIndex = 26;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.timer1.Interval = 1500;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.btnEnergyView.Anchor = AnchorStyles.Right;
      this.btnEnergyView.Location = new Point(77, 438);
      this.btnEnergyView.Margin = new Padding(5, 5, 5, 5);
      this.btnEnergyView.Name = "btnEnergyView";
      this.btnEnergyView.Size = new Size(164, 42);
      this.btnEnergyView.TabIndex = 27;
      this.btnEnergyView.Text = "能量图";
      this.btnEnergyView.UseVisualStyleBackColor = true;
      this.btnEnergyView.Click += new EventHandler(this.btnEnergyView_Click);
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.FixedPanel = FixedPanel.Panel2;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Margin = new Padding(4, 4, 4, 4);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.panel1);
      this.splitContainer1.Panel1.Controls.Add((Control) this.prgsBarGetEnergy);
      this.splitContainer1.Panel1.Controls.Add((Control) this.MyChart);
      this.splitContainer1.Panel2.BackColor = Color.White;
      this.splitContainer1.Panel2.Controls.Add((Control) this.panelHeader);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label4);
      this.splitContainer1.Panel2.Controls.Add((Control) this.comboBox2);
      this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnEnergyView);
      this.splitContainer1.Panel2.Controls.Add((Control) this.lblDev);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnClear);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnSet);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnSave);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnGetSpec);
      this.splitContainer1.Panel2.Controls.Add((Control) this.comboBox1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.bntOriginView);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnBackGrd);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnStdErrView);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnMeanView);
      this.splitContainer1.Panel2.Controls.Add((Control) this.txtSpName);
      this.splitContainer1.Size = new Size(1024, 768);
      this.splitContainer1.SplitterDistance = 696;
      this.splitContainer1.SplitterWidth = 5;
      this.splitContainer1.TabIndex = 28;
      this.panel1.BackColor = Color.FromArgb(38, 136, 210);
      this.panel1.Location = new Point(3, 2);
      this.panel1.Margin = new Padding(4, 4, 4, 4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(941, 55);
      this.panel1.TabIndex = 31;
      this.MyChart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.MyChart.Location = new Point(5, 80);
      this.MyChart.Margin = new Padding(5, 4, 5, 4);
      this.MyChart.Name = "MyChart";
      this.MyChart.ScrollGrace = 0.0;
      this.MyChart.ScrollMaxX = 0.0;
      this.MyChart.ScrollMaxY = 0.0;
      this.MyChart.ScrollMaxY2 = 0.0;
      this.MyChart.ScrollMinX = 0.0;
      this.MyChart.ScrollMinY = 0.0;
      this.MyChart.ScrollMinY2 = 0.0;
      this.MyChart.Size = new Size(685, 684);
      this.MyChart.TabIndex = 0;
      this.panelHeader.BackColor = Color.FromArgb(38, 136, 210);
      this.panelHeader.Controls.Add((Control) this.btnShowSpectrumOffline);
      this.panelHeader.Controls.Add((Control) this.btnSetting);
      this.panelHeader.Controls.Add((Control) this.btnClose);
      this.panelHeader.Location = new Point(-37, 1);
      this.panelHeader.Margin = new Padding(4, 4, 4, 4);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(471, 55);
      this.panelHeader.TabIndex = 30;
      this.btnShowSpectrumOffline.Anchor = AnchorStyles.Right;
      this.btnShowSpectrumOffline.BackColor = Color.Transparent;
      this.btnShowSpectrumOffline.BackgroundImage = (Image) Resources.Spec;
      this.btnShowSpectrumOffline.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnShowSpectrumOffline.FlatStyle = FlatStyle.Flat;
      this.btnShowSpectrumOffline.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnShowSpectrumOffline.Location = new Point(222, -6);
      this.btnShowSpectrumOffline.Margin = new Padding(7, 6, 7, 6);
      this.btnShowSpectrumOffline.Name = "btnShowSpectrumOffline";
      this.btnShowSpectrumOffline.Size = new Size(75, 61);
      this.btnShowSpectrumOffline.TabIndex = 67;
      this.btnShowSpectrumOffline.UseVisualStyleBackColor = false;
      this.btnShowSpectrumOffline.Click += new EventHandler(this.btnShowSpectrumOffline_Click);
      this.btnSetting.Anchor = AnchorStyles.Right;
      this.btnSetting.BackColor = Color.Transparent;
      this.btnSetting.BackgroundImage = (Image) Resources.Set;
      this.btnSetting.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnSetting.FlatStyle = FlatStyle.Flat;
      this.btnSetting.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnSetting.Location = new Point(300, 2);
      this.btnSetting.Margin = new Padding(5, 5, 5, 5);
      this.btnSetting.Name = "btnSetting";
      this.btnSetting.Size = new Size(56, 49);
      this.btnSetting.TabIndex = 65;
      this.btnSetting.UseVisualStyleBackColor = false;
      this.btnSetting.Click += new EventHandler(this.btnSetting_Click);
      this.btnClose.Anchor = AnchorStyles.Right;
      this.btnClose.BackColor = Color.Transparent;
      this.btnClose.BackgroundImage = (Image) Resources.Home;
      this.btnClose.BackgroundImageLayout = ImageLayout.Zoom;
      this.btnClose.FlatStyle = FlatStyle.Flat;
      this.btnClose.ForeColor = Color.FromArgb(38, 136, 210);
      this.btnClose.Location = new Point(401, 0);
      this.btnClose.Margin = new Padding(5, 5, 5, 5);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(63, 55);
      this.btnClose.TabIndex = 64;
      this.btnClose.UseVisualStyleBackColor = false;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.label4.Anchor = AnchorStyles.Right;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(69, 348);
      this.label4.Margin = new Padding(5, 0, 5, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(82, 15);
      this.label4.TabIndex = 29;
      this.label4.Text = "平均次数：";
      this.comboBox2.Anchor = AnchorStyles.Right;
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Location = new Point(164, 348);
      this.comboBox2.Margin = new Padding(5, 5, 5, 5);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new Size(53, 23);
      this.comboBox2.TabIndex = 28;
      this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
      this.btnSave.Anchor = AnchorStyles.Right;
      this.btnSave.BackgroundImage = (Image) Resources.save;
      this.btnSave.BackgroundImageLayout = ImageLayout.Center;
      this.btnSave.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.btnSave.Location = new Point(117, 628);
      this.btnSave.Margin = new Padding(5, 5, 5, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(84, 70);
      this.btnSave.TabIndex = 25;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.backgroundWorker1.WorkerSupportsCancellation = true;
      this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      this.backgroundWorker2.WorkerReportsProgress = true;
      this.backgroundWorker2.DoWork += new DoWorkEventHandler(this.backgroundWorker2_DoWork);
      this.backgroundWorker2.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
      this.backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
      this.AutoScaleDimensions = new SizeF(8f, 15f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(1024, 768);
      this.Controls.Add((Control) this.splitContainer1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(5, 5, 5, 5);
      this.Name = nameof (FrmGetSpec);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "单独采谱窗口";
      this.FormClosed += new FormClosedEventHandler(this.FrmGetSpec_FormClosed);
      this.Load += new EventHandler(this.FrmGetSpec_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.ResumeLayout(false);
      this.panelHeader.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private enum LampStatus
    {
      LampOn,
      LampOff,
    }

    private enum ViewStyle
    {
      Mean,
      StdErr,
      Spec,
      Energy,
    }

    private delegate void DrawDelegate(string Str, Spectrometer.Data[] DataGet, int Num);

    private delegate void MyChartLoadData(double[] DataX, double[] DataY, Color ColorMy, string Name);
  }
}
