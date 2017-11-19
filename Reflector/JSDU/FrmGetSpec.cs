namespace JSDU
{
    using NIRQUEST.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using ZedGraph;

    public class FrmGetSpec : Form
    {
        private BackgroundWorker backgroundWorker1;
        private BackgroundWorker backgroundWorker2;
        private Button bntOriginView;
        private Button btnBackGrd;
        private Button btnClear;
        private Button btnEnergyView;
        private Button btnGetSpec;
        private int btnGetSpecClickNum = 0;
        private Button btnMeanView;
        private Button btnSave;
        private Button btnSet;
        private Button btnStdErrView;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private IContainer components = null;
        private DataHandling DataHandlingmy = new DataHandling();
        private DataIO DataIOmy = new DataIO();
        private Color[] DrawColor = new Color[20];
        private Home frmHome = null;
        public const byte FT_PURGE_RX = 1;
        public const byte FT_PURGE_TX = 2;
        private GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDev;
        private double[,] MeanY;
        private ZedGraphControl MyChart;
        private MyChartLoadData myChartLoadData;
        private Spectrometer MySpectrometer = new Spectrometer();
        private int numberOfSpectrometersFound;
        private ProgressBar prgsBarGetEnergy;
        public static Spectrometer.SpecInfo SpInfo;
        private SplitContainer splitContainer1;
        private string SpName = "";
        private double[,] StdErrY;
        public System.Windows.Forms.Timer timer1;
        private TextBox txtMaxStdErr;
        private TextBox txtMeanErr;
        private TextBox txtSpName;
        private ViewStyle ViewStylemy = ViewStyle.Spec;

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
            this.DrawColor[0x10] = Color.DarkRed;
            this.DrawColor[0x11] = Color.DarkSlateBlue;
            this.DrawColor[0x12] = Color.ForestGreen;
            this.DrawColor[0x13] = Color.Indigo;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Spectrometer.SpecInfo argument = (Spectrometer.SpecInfo) e.Argument;
            this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref argument, false);
            e.Result = argument;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.timer1.Enabled)
            {
                SpInfo = (Spectrometer.SpecInfo) e.Result;
                this.DrawEnergyValue();
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["LightOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
                buffer = Home.SPControlWord["ReferenceOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            e.Result = this.MySpectrometer.GetSpec(Spectrometer.spectrometerIndex, ref SpInfo, Spectrometer.ScanTimes, this.backgroundWorker2);
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
                int num;
                if (SpInfo.DataAB.Length == SpInfo.DataA.Length)
                {
                    SpInfo.DataY = new double[SpInfo.numPixls];
                    for (num = 0; num < SpInfo.DataA.Length; num++)
                    {
                        try
                        {
                            SpInfo.DataY[num] = Convert.ToDouble(Math.Log10(Math.Abs((double) ((SpInfo.DataAB[num] - SpInfo.DataAD[num]) / (SpInfo.DataA[num] - SpInfo.DataAD[num])))));
                        }
                        catch
                        {
                            if (num > 0)
                            {
                                SpInfo.DataY[num] = SpInfo.DataY[num - 1];
                            }
                            else
                            {
                                SpInfo.DataY[num] = 0.0;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("背景维数不符，请重新采集！");
                    return;
                }
                SpInfo.w1 = SpInfo.DataX[0];
                SpInfo.w2 = SpInfo.DataX[SpInfo.DataX.Length - 1];
                Spectrometer.DataGet[this.btnGetSpecClickNum].DataX = SpInfo.DataX;
                Spectrometer.DataGet[this.btnGetSpecClickNum].DataY = SpInfo.DataY;
                Spectrometer.DataGet[this.btnGetSpecClickNum].DataE = SpInfo.DataA;
                string[] strArray = new string[6];
                strArray[0] = Spectrometer.SavePath;
                strArray[1] = @"\";
                strArray[2] = this.SpName;
                strArray[3] = "-";
                int num3 = this.btnGetSpecClickNum + 1;
                strArray[4] = num3.ToString();
                strArray[5] = ".txt";
                this.DataIOmy.TXTSaveData(string.Concat(strArray), SpInfo.DataX, SpInfo.DataY);
                strArray = new string[6];
                strArray[0] = Spectrometer.SavePath;
                strArray[1] = @"\";
                strArray[2] = this.SpName;
                strArray[3] = "-Energy-";
                num3 = this.btnGetSpecClickNum + 1;
                strArray[4] = num3.ToString();
                strArray[5] = ".txt";
                this.DataIOmy.TXTSaveData(string.Concat(strArray), SpInfo.DataX, SpInfo.DataA);
                this.ViewStylemy = ViewStyle.Spec;
                DrawDelegate method = new DrawDelegate(this.Draw);
                base.BeginInvoke(method, new object[] { "光谱图", Spectrometer.DataGet, this.btnGetSpecClickNum });
                double[,] x = new double[this.btnGetSpecClickNum + 1, SpInfo.numPixls];
                for (num = 0; num < (this.btnGetSpecClickNum + 1); num++)
                {
                    for (int i = 0; i < SpInfo.numPixls; i++)
                    {
                        x[num, i] = Spectrometer.DataGet[num].DataY[i];
                    }
                }
                double[] numArray2 = this.DataHandlingmy.SpMean(x);
                double[] array = this.DataHandlingmy.SpStdError(x);
                for (num = 0; num < numArray2.Length; num++)
                {
                    this.MeanY[this.btnGetSpecClickNum, num] = numArray2[num];
                }
                for (num = 0; num < numArray2.Length; num++)
                {
                    this.StdErrY[this.btnGetSpecClickNum, num] = array[num];
                }
                this.txtMeanErr.Text = this.DataHandlingmy.MeanValue(array).ToString("0.000e0");
                this.txtMaxStdErr.Text = this.DataHandlingmy.MaxValue(array).ToString("0.000e0");
                num3 = this.btnGetSpecClickNum + 1;
                this.comboBox1.Items.Add(num3.ToString());
                this.comboBox1.SelectedIndex = this.btnGetSpecClickNum;
                this.btnGetSpecClickNum++;
                if (this.btnGetSpecClickNum > 0x13)
                {
                    this.btnGetSpecClickNum = 0;
                }
            }
            else
            {
                MessageBox.Show("光谱采集失败，请重新采集！");
            }
        }

        private void bntOriginView_Click(object sender, EventArgs e)
        {
            this.ViewStylemy = ViewStyle.Spec;
            this.ReDraw(sender, e);
        }

        private void btnBackGrd_Click(object sender, EventArgs e)
        {
            byte[] buffer;
            this.timer1.Enabled = false;
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                buffer = Home.SPControlWord["LightOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
                buffer = Home.SPControlWord["ReferenceOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.ToString());
                return;
            }
            this.prgsBarGetEnergy.Visible = true;
            this.MySpectrometer._ProgressBar = this.prgsBarGetEnergy;
            this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, true);
            SpInfo.DataAB = SpInfo.DataA;
            SpInfo.DataA = null;
            this.DataIOmy.TXTSaveData(Application.StartupPath.ToString() + @"\background", SpInfo.DataX, SpInfo.DataAB);
            Spectrometer.Data[] dataGet = new Spectrometer.Data[1];
            dataGet[0].DataX = SpInfo.WavelengthArray;
            dataGet[0].DataE = SpInfo.DataA;
            this.ViewStylemy = ViewStyle.Energy;
            DrawDelegate method = new DrawDelegate(this.Draw);
            base.BeginInvoke(method, new object[] { "能量图", dataGet, 0 });
            this.Draw("背景能量图", dataGet, 0);
            this.MySpectrometer.ReadDK(ref SpInfo);
            MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                buffer = Home.SPControlWord["ReferenceOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();
            this.btnGetSpecClickNum = 0;
            this.txtSpName.Text = "";
            Spectrometer.DataGet = new Spectrometer.Data[20];
            this.MeanY = new double[20, SpInfo.numPixls];
            this.StdErrY = new double[20, SpInfo.numPixls];
            this.ViewStylemy = ViewStyle.Spec;
            DrawDelegate method = new DrawDelegate(this.Draw);
            base.BeginInvoke(method, new object[] { "光谱图", Spectrometer.DataGet, this.btnGetSpecClickNum });
            this.timer1.Enabled = true;
        }

        private void btnEnergyView_Click(object sender, EventArgs e)
        {
            this.ViewStylemy = ViewStyle.Energy;
            this.ReDraw(sender, e);
        }

        private void btnGetSpec_Click(object sender, EventArgs e)
        {
            if (this.txtSpName.Text == "")
            {
                this.txtSpName.Text = DateTime.Now.ToString("MM-dd") + " " + DateTime.Now.ToString("HH-mm");
            }
            this.timer1.Enabled = false;
            if (this.btnGetSpecClickNum == 0)
            {
                this.SpName = this.txtSpName.Text;
                Spectrometer.DataGet = new Spectrometer.Data[20];
                this.MeanY = new double[20, SpInfo.DataAB.Length];
                this.StdErrY = new double[20, SpInfo.DataAB.Length];
            }
            else if (this.txtSpName.Text.IndexOf("-") < 0)
            {
                this.SpName = this.txtSpName.Text;
                this.btnGetSpecClickNum = 0;
            }
            base.Invoke(new EventHandler(this.updatatxtSpNameUI), new object[] { this.txtSpName, EventArgs.Empty });
            this.prgsBarGetEnergy.Visible = true;
            this.prgsBarGetEnergy.Maximum = 5;
            if (!this.backgroundWorker2.IsBusy)
            {
                this.backgroundWorker2.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("光谱正在采集，请勿重复点击！谢谢！");
            }
        }

        private void btnMeanView_Click(object sender, EventArgs e)
        {
            DrawDelegate method = new DrawDelegate(this.Draw);
            int selectedIndex = this.comboBox1.SelectedIndex;
            Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
            if (Spectrometer.DataGet != null)
            {
                dataArray[0].DataX = Spectrometer.DataGet[selectedIndex].DataX;
                dataArray[0].DataY = new double[dataArray[0].DataX.Length];
                for (int i = 0; i < dataArray[0].DataX.Length; i++)
                {
                    dataArray[0].DataY[i] = this.MeanY[selectedIndex, i];
                }
                this.ViewStylemy = ViewStyle.Mean;
                base.BeginInvoke(method, new object[] { "平均谱图", dataArray, 0 });
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            double[] numArray;
            int num2;
            int selectedIndex = this.comboBox1.SelectedIndex;
            if (this.ViewStylemy == ViewStyle.Mean)
            {
                numArray = new double[this.MeanY.GetLength(1)];
                for (num2 = 0; num2 < this.MeanY.GetLength(1); num2++)
                {
                    numArray[num2] = this.MeanY[selectedIndex, num2];
                }
                this.DataIOmy.TXTSaveData(Spectrometer.SavePath + @"\" + this.SpName + "-Mean-" + this.comboBox1.SelectedItem.ToString() + ".txt", Spectrometer.DataGet[selectedIndex].DataX, numArray);
                MessageBox.Show("平均光谱：" + Spectrometer.SavePath + @"\" + this.SpName + "-Mean-" + this.comboBox1.SelectedItem.ToString() + ".txt保存成功");
            }
            else if (this.ViewStylemy == ViewStyle.StdErr)
            {
                numArray = new double[this.MeanY.GetLength(1)];
                for (num2 = 0; num2 < this.MeanY.GetLength(1); num2++)
                {
                    numArray[num2] = this.StdErrY[selectedIndex, num2];
                }
                this.DataIOmy.TXTSaveData(Spectrometer.SavePath + @"\" + this.SpName + "-StdErr-" + this.comboBox1.SelectedItem.ToString() + ".txt", Spectrometer.DataGet[selectedIndex].DataX, numArray);
                MessageBox.Show("标准差图：" + Spectrometer.SavePath + @"\" + this.SpName + "-StdErr-" + this.comboBox1.SelectedItem.ToString() + ".txt保存成功");
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            new FrmSetting(this.MySpectrometer, this, this.frmHome).ShowDialog();
        }

        private void btnStdErrView_Click(object sender, EventArgs e)
        {
            DrawDelegate method = new DrawDelegate(this.Draw);
            int selectedIndex = this.comboBox1.SelectedIndex;
            Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
            if (Spectrometer.DataGet != null)
            {
                dataArray[0].DataX = Spectrometer.DataGet[selectedIndex].DataX;
                dataArray[0].DataY = new double[dataArray[0].DataX.Length];
                for (int i = 0; i < dataArray[0].DataX.Length; i++)
                {
                    dataArray[0].DataY[i] = this.StdErrY[selectedIndex, i];
                }
                this.ViewStylemy = ViewStyle.StdErr;
                base.BeginInvoke(method, new object[] { "标准差图", dataArray, 0 });
                this.txtMeanErr.Text = this.DataHandlingmy.MeanValue(dataArray[0].DataY).ToString("0.000e0");
                this.txtMaxStdErr.Text = this.DataHandlingmy.MaxValue(dataArray[0].DataY).ToString("0.000e0");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Items.Count > 0)
            {
                if (this.ViewStylemy == ViewStyle.StdErr)
                {
                    this.btnStdErrView_Click(sender, e);
                }
                if (this.ViewStylemy == ViewStyle.Mean)
                {
                    this.btnMeanView_Click(sender, e);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Spectrometer.ScanTimes = this.comboBox2.SelectedIndex + 1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Draw(string str, Spectrometer.Data[] DataGet, int Num)
        {
            if ((DataGet != null) && (DataGet[Num].DataX != null))
            {
                RectangleF rect;
                int num;
                object[] objArray;
                if ((this.ViewStylemy == ViewStyle.Energy) && (DataGet[Num].DataE != null))
                {
                    rect = this.MyChart.GraphPane.Rect;
                    if (this.ViewStylemy == ViewStyle.Energy)
                    {
                        this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "能量");
                    }
                    this.MyChart.GraphPane.CurveList.Clear();
                    for (num = 0; num < (Num + 1); num++)
                    {
                        objArray = new object[4];
                        objArray[0] = DataGet[num].DataX;
                        objArray[1] = DataGet[num].DataE;
                        objArray[2] = this.DrawColor[num];
                        int num2 = num + 1;
                        objArray[3] = this.SpName + "-Energy-" + num2.ToString();
                        this.MyChart.Invoke(this.myChartLoadData, objArray);
                    }
                }
                else
                {
                    rect = this.MyChart.GraphPane.Rect;
                    if (this.ViewStylemy == ViewStyle.Spec)
                    {
                        this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "吸光度");
                        this.MyChart.GraphPane.CurveList.Clear();
                        for (num = 0; num < (Num + 1); num++)
                        {
                            objArray = new object[] { DataGet[num].DataX, DataGet[num].DataY, this.DrawColor[num], this.SpName + "-" + ((num + 1)).ToString() };
                            this.MyChart.Invoke(this.myChartLoadData, objArray);
                        }
                    }
                    else if ((this.ViewStylemy == ViewStyle.Mean) && (Num == 0))
                    {
                        this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "吸光度");
                        this.MyChart.GraphPane.CurveList.Clear();
                        for (num = 0; num < (Num + 1); num++)
                        {
                            this.MyChart.Invoke(this.myChartLoadData, new object[] { DataGet[num].DataX, DataGet[num].DataY, this.DrawColor[num], this.SpName + "平均光谱" });
                        }
                    }
                    else if ((this.ViewStylemy == ViewStyle.StdErr) && (Num == 0))
                    {
                        this.MyChart.GraphPane = new GraphPane(rect, str, "波数", " ");
                        this.MyChart.GraphPane.CurveList.Clear();
                        for (num = 0; num < (Num + 1); num++)
                        {
                            this.MyChart.Invoke(this.myChartLoadData, new object[] { DataGet[num].DataX, DataGet[num].DataY, this.DrawColor[num], this.SpName + "标准差图" });
                        }
                    }
                }
            }
            else
            {
                this.MyChart.GraphPane.CurveList.Clear();
            }
        }

        private void DrawEnergyValue()
        {
            for (int i = 0; i < SpInfo.numPixls; i++)
            {
                if (SpInfo.WavelengthArray[i] > 10000000000)
                {
                    SpInfo.WavelengthArray[i] = SpInfo.WavelengthArray[i - 1];
                }
                if (SpInfo.DataA[i] > 10000000000)
                {
                    SpInfo.DataA[i] = SpInfo.DataA[i - 1];
                }
            }
            SpInfo.DataX = SpInfo.WavelengthArray;
            SpInfo.w1 = Math.Floor((double) (SpInfo.DataX[0] / 100.0)) * 100.0;
            SpInfo.w2 = Math.Ceiling((double) (SpInfo.DataX[SpInfo.DataX.Length - 1] / 100.0)) * 100.0;
            Spectrometer.Data[] dataGet = new Spectrometer.Data[1];
            dataGet[0].DataX = SpInfo.WavelengthArray;
            dataGet[0].DataE = SpInfo.DataA;
            this.ViewStylemy = ViewStyle.Energy;
            DrawDelegate delegate2 = new DrawDelegate(this.Draw);
            this.Draw("能量图", dataGet, 0);
            this.txtMeanErr.Text = this.DataHandlingmy.MeanValue(dataGet[0].DataE).ToString("0.000e0");
            this.txtMaxStdErr.Text = this.DataHandlingmy.MaxValue(dataGet[0].DataE).ToString("0.000e0");
        }

        private void FrmGetSpec_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.CancelAsync();
            }
            object[] objArray = new object[] { Spectrometer.IntegrationTime.ToString(), ",", Spectrometer.ScanTimes.ToString(), ",", Spectrometer.GainMode, ",", Spectrometer.SavePath, ",", (Spectrometer.isClearDarks ? 1 : 0).ToString(), ",", (Spectrometer.isCorrectElectricalDarks ? 1 : 0).ToString(), ",", (Spectrometer.isCorrectNonlinearitys ? 1 : 0).ToString() };
            string writeStr = string.Concat(objArray);
            this.DataIOmy.SaveStr(writeStr, Spectrometer.ApplicationPath + @"\Setting");
        }

        private void FrmGetSpec_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Enabled = false;
            if (this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.CancelAsync();
            }
            this.backgroundWorker1.Dispose();
            Home.serialPortSetDevice.Close();
        }

        private void FrmGetSpec_Load(object sender, EventArgs e)
        {
            Spectrometer.ApplicationPath = Application.StartupPath;
            this.numberOfSpectrometersFound = this.MySpectrometer.wrapper.openAllSpectrometers();
            this.prgsBarGetEnergy.Visible = false;
            for (int i = 0; i < Spectrometer.ScanTimes; i++)
            {
                int num2 = i + 1;
                this.comboBox2.Items.Add(num2.ToString());
            }
            this.comboBox2.SelectedIndex = Spectrometer.ScanTimes - 1;
            this.lblDev.Text = "设备：";
            if (this.numberOfSpectrometersFound > 0)
            {
                Spectrometer.spectrometerIndex = 0;
                this.lblDev.Text = "设备：" + this.MySpectrometer.wrapper.getName(Spectrometer.spectrometerIndex);
                this.MySpectrometer.wrapper.setAutoToggleStrobeLampEnable(Spectrometer.spectrometerIndex, 1);
            }
            else
            {
                MessageBox.Show("仪器连接错误，请检查连接！");
                base.Close();
            }
            this.MySpectrometer.ReadBK(ref SpInfo);
            this.MySpectrometer.ReadSetParameters();
            if (Spectrometer.isClearDarks)
            {
                this.MySpectrometer.ReadDK(ref SpInfo);
            }
            this.timer1.Enabled = true;
        }

        public void InitDrawDelegate()
        {
            this.myChartLoadData = new MyChartLoadData(this.MyChart_LoadData);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmGetSpec));
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
            this.MyChart = new ZedGraphControl();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox2 = new ComboBox();
            this.btnSave = new Button();
            this.backgroundWorker1 = new BackgroundWorker();
            this.backgroundWorker2 = new BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            base.SuspendLayout();
            this.lblDev.Anchor = AnchorStyles.Right;
            this.lblDev.AutoSize = true;
            this.lblDev.Location = new Point(0x5c, 0x57);
            this.lblDev.Margin = new Padding(4, 0, 4, 0);
            this.lblDev.Name = "lblDev";
            this.lblDev.Size = new Size(0x29, 12);
            this.lblDev.TabIndex = 0;
            this.lblDev.Text = "设备：";
            this.btnSet.Anchor = AnchorStyles.Right;
            this.btnSet.Location = new Point(0xfd, 0xf3);
            this.btnSet.Margin = new Padding(4);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new Size(0x33, 0x19);
            this.btnSet.TabIndex = 12;
            this.btnSet.Text = "设置";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Visible = false;
            this.btnSet.Click += new EventHandler(this.btnSet_Click);
            this.btnGetSpec.Anchor = AnchorStyles.Right;
            this.btnGetSpec.Location = new Point(0x59, 0x241);
            this.btnGetSpec.Margin = new Padding(4);
            this.btnGetSpec.Name = "btnGetSpec";
            this.btnGetSpec.Size = new Size(0x4b, 0x19);
            this.btnGetSpec.TabIndex = 13;
            this.btnGetSpec.Text = "开始";
            this.btnGetSpec.UseVisualStyleBackColor = true;
            this.btnGetSpec.Click += new EventHandler(this.btnGetSpec_Click);
            this.prgsBarGetEnergy.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.prgsBarGetEnergy.Location = new Point(0, 0x256);
            this.prgsBarGetEnergy.Margin = new Padding(4);
            this.prgsBarGetEnergy.Name = "prgsBarGetEnergy";
            this.prgsBarGetEnergy.Size = new Size(0x1b4, 15);
            this.prgsBarGetEnergy.TabIndex = 14;
            this.btnBackGrd.Anchor = AnchorStyles.Right;
            this.btnBackGrd.Location = new Point(0xac, 0x241);
            this.btnBackGrd.Margin = new Padding(4);
            this.btnBackGrd.Name = "btnBackGrd";
            this.btnBackGrd.Size = new Size(0x4a, 0x19);
            this.btnBackGrd.TabIndex = 15;
            this.btnBackGrd.Text = "背景";
            this.btnBackGrd.UseVisualStyleBackColor = true;
            this.btnBackGrd.Click += new EventHandler(this.btnBackGrd_Click);
            this.label1.Anchor = AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x5b, 0x7a);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x35, 12);
            this.label1.TabIndex = 0x11;
            this.label1.Text = "文件名：";
            this.txtSpName.Anchor = AnchorStyles.Right;
            this.txtSpName.Location = new Point(140, 0x77);
            this.txtSpName.Margin = new Padding(4);
            this.txtSpName.Name = "txtSpName";
            this.txtSpName.Size = new Size(0xa4, 0x15);
            this.txtSpName.TabIndex = 0x12;
            this.groupBox1.Anchor = AnchorStyles.Right;
            this.groupBox1.Controls.Add(this.txtMeanErr);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtMaxStdErr);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new Point(0x62, 0xa5);
            this.groupBox1.Margin = new Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new Padding(4);
            this.groupBox1.Size = new Size(0xcf, 70);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标准差";
            this.txtMeanErr.Location = new Point(0x5c, 0x27);
            this.txtMeanErr.Margin = new Padding(4);
            this.txtMeanErr.Name = "txtMeanErr";
            this.txtMeanErr.Size = new Size(0x68, 0x15);
            this.txtMeanErr.TabIndex = 0x15;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(40, 0x2a);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x29, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "平均：";
            this.txtMaxStdErr.Location = new Point(0x5c, 11);
            this.txtMaxStdErr.Margin = new Padding(4);
            this.txtMaxStdErr.Name = "txtMaxStdErr";
            this.txtMaxStdErr.Size = new Size(0x68, 0x15);
            this.txtMaxStdErr.TabIndex = 0x13;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(40, 15);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x29, 12);
            this.label2.TabIndex = 0x12;
            this.label2.Text = "最大：";
            this.btnMeanView.Anchor = AnchorStyles.Right;
            this.btnMeanView.Location = new Point(140, 410);
            this.btnMeanView.Margin = new Padding(4);
            this.btnMeanView.Name = "btnMeanView";
            this.btnMeanView.Size = new Size(0x7b, 0x22);
            this.btnMeanView.TabIndex = 0x15;
            this.btnMeanView.Text = "平均光谱";
            this.btnMeanView.UseVisualStyleBackColor = true;
            this.btnMeanView.Click += new EventHandler(this.btnMeanView_Click);
            this.btnStdErrView.Anchor = AnchorStyles.Right;
            this.btnStdErrView.Location = new Point(140, 0x1d1);
            this.btnStdErrView.Margin = new Padding(4);
            this.btnStdErrView.Name = "btnStdErrView";
            this.btnStdErrView.Size = new Size(0x7b, 0x22);
            this.btnStdErrView.TabIndex = 0x16;
            this.btnStdErrView.Text = "标准差图";
            this.btnStdErrView.UseVisualStyleBackColor = true;
            this.btnStdErrView.Click += new EventHandler(this.btnStdErrView_Click);
            this.bntOriginView.Anchor = AnchorStyles.Right;
            this.bntOriginView.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.bntOriginView.Location = new Point(0x33, 430);
            this.bntOriginView.Margin = new Padding(4);
            this.bntOriginView.Name = "bntOriginView";
            this.bntOriginView.Size = new Size(0x25, 0x22);
            this.bntOriginView.TabIndex = 0x17;
            this.bntOriginView.Text = "<-";
            this.bntOriginView.UseVisualStyleBackColor = true;
            this.bntOriginView.Click += new EventHandler(this.bntOriginView_Click);
            this.comboBox1.Anchor = AnchorStyles.Right;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new Point(0x10f, 0x1ba);
            this.comboBox1.Margin = new Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x29, 20);
            this.comboBox1.TabIndex = 0x18;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.btnClear.Anchor = AnchorStyles.Right;
            this.btnClear.Location = new Point(260, 0x241);
            this.btnClear.Margin = new Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(0x2d, 0x19);
            this.btnClear.TabIndex = 0x1a;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.timer1.Interval = 0x5dc;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.btnEnergyView.Anchor = AnchorStyles.Right;
            this.btnEnergyView.Location = new Point(140, 350);
            this.btnEnergyView.Margin = new Padding(4);
            this.btnEnergyView.Name = "btnEnergyView";
            this.btnEnergyView.Size = new Size(0x7b, 0x22);
            this.btnEnergyView.TabIndex = 0x1b;
            this.btnEnergyView.Text = "能量图";
            this.btnEnergyView.UseVisualStyleBackColor = true;
            this.btnEnergyView.Click += new EventHandler(this.btnEnergyView_Click);
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.FixedPanel = FixedPanel.Panel2;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.prgsBarGetEnergy);
            this.splitContainer1.Panel1.Controls.Add(this.MyChart);
            this.splitContainer1.Panel2.BackColor = Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.txtSpName);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.comboBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.btnEnergyView);
            this.splitContainer1.Panel2.Controls.Add(this.lblDev);
            this.splitContainer1.Panel2.Controls.Add(this.btnClear);
            this.splitContainer1.Panel2.Controls.Add(this.btnSet);
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Panel2.Controls.Add(this.btnGetSpec);
            this.splitContainer1.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer1.Panel2.Controls.Add(this.bntOriginView);
            this.splitContainer1.Panel2.Controls.Add(this.btnBackGrd);
            this.splitContainer1.Panel2.Controls.Add(this.btnStdErrView);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.btnMeanView);
            this.splitContainer1.Size = new Size(0x300, 0x266);
            this.splitContainer1.SplitterDistance = 440;
            this.splitContainer1.TabIndex = 0x1c;
            this.MyChart.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.MyChart.Location = new Point(0, 0);
            this.MyChart.Margin = new Padding(4, 3, 4, 3);
            this.MyChart.Name = "MyChart";
            this.MyChart.ScrollGrace = 0.0;
            this.MyChart.ScrollMaxX = 0.0;
            this.MyChart.ScrollMaxY = 0.0;
            this.MyChart.ScrollMaxY2 = 0.0;
            this.MyChart.ScrollMinX = 0.0;
            this.MyChart.ScrollMinY = 0.0;
            this.MyChart.ScrollMinY2 = 0.0;
            this.MyChart.Size = new Size(0x1b4, 0x25a);
            this.MyChart.TabIndex = 0;
            this.label4.Anchor = AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x86, 0x116);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x41, 12);
            this.label4.TabIndex = 0x1d;
            this.label4.Text = "平均次数：";
            this.comboBox2.Anchor = AnchorStyles.Right;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new Point(0xcd, 0x116);
            this.comboBox2.Margin = new Padding(4);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new Size(0x29, 20);
            this.comboBox2.TabIndex = 0x1c;
            this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
            this.btnSave.Anchor = AnchorStyles.Right;
//            this.btnSave.BackgroundImage = Resources.save;
            this.btnSave.BackgroundImageLayout = ImageLayout.Center;
            this.btnSave.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.btnSave.Location = new Point(170, 0x1f6);
            this.btnSave.Margin = new Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(0x3f, 0x38);
            this.btnSave.TabIndex = 0x19;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.DoWork += new DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x300, 0x266);
            base.Controls.Add(this.splitContainer1);
            base.FormBorderStyle = FormBorderStyle.None;
//            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Margin = new Padding(4);
            base.Name = "FrmGetSpec";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "单独采谱窗口";
            base.FormClosing += new FormClosingEventHandler(this.FrmGetSpec_FormClosing);
            base.FormClosed += new FormClosedEventHandler(this.FrmGetSpec_FormClosed);
            base.Load += new EventHandler(this.FrmGetSpec_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void MyChart_LoadData(double[] DataX, double[] DataY, Color ColorMy, string Name)
        {
            PointPairList points = new PointPairList(DataX, DataY);
            this.MyChart.GraphPane.AddCurve(Name, points, ColorMy, SymbolType.None);
            this.MyChart.GraphPane.XAxis.Scale.Min = this.DataHandlingmy.MinValue(DataX);
            this.MyChart.GraphPane.XAxis.Scale.Max = this.DataHandlingmy.MaxValue(DataX);
            this.MyChart.AxisChange();
            this.MyChart.Refresh();
        }

        private void ReDraw(object sender, EventArgs e)
        {
            DrawDelegate delegate2;
            int num;
            if (this.ViewStylemy == ViewStyle.Spec)
            {
                delegate2 = new DrawDelegate(this.Draw);
                num = ((this.btnGetSpecClickNum - 1) < 0) ? 0 : (this.btnGetSpecClickNum - 1);
                if (Spectrometer.DataGet.Length > 0)
                {
                    base.BeginInvoke(delegate2, new object[] { "光谱图", Spectrometer.DataGet, num });
                }
            }
            else if (this.ViewStylemy == ViewStyle.Energy)
            {
                delegate2 = new DrawDelegate(this.Draw);
                num = ((this.btnGetSpecClickNum - 1) < 0) ? 0 : (this.btnGetSpecClickNum - 1);
                if (Spectrometer.DataGet != null)
                {
                    base.BeginInvoke(delegate2, new object[] { "能量图", Spectrometer.DataGet, num });
                }
            }
            else if (this.ViewStylemy == ViewStyle.Mean)
            {
                this.btnMeanView_Click(sender, e);
            }
            else if (this.ViewStylemy == ViewStyle.StdErr)
            {
                this.btnStdErrView_Click(sender, e);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Spectrometer.IntegrationTime > 0x7a120)
            {
                this.timer1.Interval = 0xbb8;
            }
            if (!this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.RunWorkerAsync(SpInfo);
            }
        }

        private void updatatxtSpNameUI(object o, EventArgs e)
        {
            this.txtSpName.Text = this.SpName + "-" + ((this.btnGetSpecClickNum + 1)).ToString();
        }

        private delegate void DrawDelegate(string Str, Spectrometer.Data[] DataGet, int Num);

        private enum LampStatus
        {
            LampOn,
            LampOff
        }

        private delegate void MyChartLoadData(double[] DataX, double[] DataY, Color ColorMy, string Name);

        private enum ViewStyle
        {
            Mean,
            StdErr,
            Spec,
            Energy
        }
    }
}

