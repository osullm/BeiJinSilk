namespace JSDU
{
    using NIRQUEST;
    using NIRQUEST.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using ZedGraph;

    public class FrmGetSpecAuto : Form
    {
        private int anglethreshold;
        private BackgroundWorker backgroundWorker1;
        private Button btnBackGrd;
        private Button btnConveyorOff;
        private Button btnConveyorOn;
        private Button btnGetSpec;
        private Button btnLightOff;
        private Button btnLightOn;
        private Button btnReferenceCupOff;
        private Button btnReferenceCupOn;
        private Button btnSpectrometerOff;
        private Button btnStop;
        private Button btnValve1On;
        private Button btnValveOff;
        private CheckBox CBisEnergy;
        private IContainer components = null;
        private int counterFemale = 0;
        private int counterMale = 0;
        private DataHandling DataHandlingmy = new DataHandling();
        private DataIO DataIOmy = new DataIO();
        private Color[] DrawColor = new Color[20];
        private Home frmHome = null;
        public const byte FT_PURGE_RX = 1;
        public const byte FT_PURGE_TX = 2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private ListBox listBoxGetSpec;
        private double[,] MeanY;
        private ZedGraphControl MyChart;
        private MyChartLoadData myChartLoadData;
        private Spectrometer MySpectrometer = new Spectrometer();
        private int numberOfSpectrometersFound;
        private Panel panel1;
        private Panel panel3;
        private double[] rubbery;
        private int[] selectWaveIndexDiff = null;
        private Spectrometer.Data[] silkWormSpec;
        private bool silkWormStart = false;
        private Spectrometer.Data[] specData = new Spectrometer.Data[1];
        private int specTempNum = 0;
        public static Spectrometer.SpecInfo SpInfo;
        private string SpName = "";
        private static bool startFlag = false;
        private Thread tGetSpecAuto = null;
        private long time1 = 0L;
        private System.Windows.Forms.Timer timer1;
        private long timestart = 0L;
        private TrackBar trackBarSpeed;
        private TextBox txtAngleThreshold;
        private TextBox txtTimeCount;
        private ViewStyle ViewStylemy = ViewStyle.Spec;

        public FrmGetSpecAuto(Home home)
        {
            this.InitializeComponent();
            this.frmHome = home;
        }

        private void AppendText(string msg)
        {
            if (this.listBoxGetSpec.InvokeRequired)
            {
                AddItemToListBoxDelegate method = new AddItemToListBoxDelegate(this.AppendText);
                this.listBoxGetSpec.Invoke(method, new object[] { msg });
            }
            else
            {
                this.listBoxGetSpec.Items.Add(msg);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
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
                Thread.Sleep(10);
                buffer = Home.SPControlWord["ReferenceOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(10);
                buffer = Home.SPControlWord["ShutterOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(0x3e8);
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.ToString());
            }
            this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, true);
            SpInfo.DataAD = new double[SpInfo.DataA.Length];
            SpInfo.DataA.CopyTo(SpInfo.DataAD, 0);
            SpInfo.DataA = null;
            this.DataIOmy.TXTSaveData(Application.StartupPath.ToString() + @"\dark", SpInfo.DataX, SpInfo.DataAD);
            buffer = Home.SPControlWord["ShutterOff"];
            Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            Thread.Sleep(0xbb8);
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

        private void btnConveyorOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["ConveyorOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnConveyorOn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["ConveyorOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnGetSpec_Click(object sender, EventArgs e)
        {
            this.btnGetSpec.Enabled = false;
            this.btnGetSpec.BackColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnStop.Enabled = true;
            this.anglethreshold = int.Parse(this.txtAngleThreshold.Text);
            FrmGetSpecSet.Default.anglethreshold = this.anglethreshold;
            FrmGetSpecSet.Default.Save();
            startFlag = true;
            Spectrometer.DataGet = new Spectrometer.Data[20];
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["ConveyorOn"];
                buffer = Home.SPControlWord["LightOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            if (!FrmGetSpecSet.Default.GetSpecAutoIsEnergy)
            {
                this.GetBackGrdAndBlack();
            }
            this.time1 = DateTime.Now.Ticks;
            this.tGetSpecAuto = new Thread(new ThreadStart(this.GetSpecAuto));
            this.timer1.Enabled = true;
            if (this.tGetSpecAuto.ThreadState == ThreadState.Running)
            {
                MessageBox.Show("正在检测，请勿重复点击，谢谢！");
            }
            else
            {
                this.tGetSpecAuto.Priority = ThreadPriority.Highest;
                this.tGetSpecAuto.Start();
            }
        }

        private void btnLightOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["LightOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnLightOn_Click(object sender, EventArgs e)
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
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnReferenceCupOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["ReferenceOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnReferenceCupOn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["ReferenceOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnSpectrometerOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["Valve2On"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.btnGetSpec.BackColor = Color.Transparent;
            this.silkWormStart = false;
            startFlag = false;
            this.btnGetSpec.Enabled = true;
            this.btnStop.Enabled = false;
            FrmGetSpecSet.Default.counterFemale = this.counterFemale;
            FrmGetSpecSet.Default.counterMale = this.counterMale;
            FrmGetSpecSet.Default.Save();
            this.timer1.Enabled = false;
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["ConveyorOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnValve1On_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["Valve1On"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnValveOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["ValveOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void CBisEnergy_CheckedChanged(object sender, EventArgs e)
        {
            FrmGetSpecSet.Default.GetSpecAutoIsEnergy = this.CBisEnergy.Checked;
            FrmGetSpecSet.Default.Save();
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
                int num2;
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
                        num2 = num + 1;
                        this.MyChart_LoadData(DataGet[num].DataX, DataGet[num].DataE, this.DrawColor[num], this.SpName + "-Energy-" + num2.ToString());
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
                            if ((num == Num) && (Num > 1))
                            {
                                if (str == "光 谱 图")
                                {
                                    this.MyChart_LoadData(DataGet[num].DataX, DataGet[num].DataY, Color.Red, "平均");
                                }
                                else
                                {
                                    num2 = num + 1;
                                    this.MyChart_LoadData(DataGet[num].DataX, DataGet[num].DataY, this.DrawColor[num], this.SpName + "-" + num2.ToString());
                                }
                            }
                            else
                            {
                                this.MyChart_LoadData(DataGet[num].DataX, DataGet[num].DataY, this.DrawColor[num], this.SpName + "-" + ((num + 1)).ToString());
                            }
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

        private void FrmGetSpecAuto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.CancelAsync();
            }
            this.backgroundWorker1.Dispose();
        }

        private void FrmGetSpecAuto_Load(object sender, EventArgs e)
        {
            Spectrometer.ApplicationPath = Application.StartupPath;
            this.numberOfSpectrometersFound = this.MySpectrometer.wrapper.openAllSpectrometers();
            if (this.numberOfSpectrometersFound > 0)
            {
                Spectrometer.spectrometerIndex = 0;
                this.frmHome.lblConnectState.Text = "已连接";
                this.frmHome.panelConnectState.Visible = true;
                this.MySpectrometer.wrapper.setAutoToggleStrobeLampEnable(Spectrometer.spectrometerIndex, 1);
            }
            else
            {
                this.frmHome.lblConnectState.Text = "未连接";
                this.frmHome.panelConnectState.Visible = false;
                this.frmHome.btnSetting.Enabled = false;
                this.btnGetSpec.Enabled = false;
                this.btnStop.Enabled = false;
            }
            this.MySpectrometer.ReadBK(ref SpInfo);
            this.MySpectrometer.ReadSetParameters();
            Spectrometer.IntegrationTimeBK = FrmGetSpecSet.Default.IntegrationTimeBK;
            this.trackBarSpeed.Value = FrmGetSpecSet.Default.trackBarSpeed;
            this.anglethreshold = FrmGetSpecSet.Default.anglethreshold;
            this.txtAngleThreshold.Text = this.anglethreshold.ToString();
            if (Spectrometer.isClearDarks)
            {
                this.MySpectrometer.ReadDK(ref SpInfo);
            }
            double[] numArray = null;
            int num = this.DataIOmy.TXTReadData(Application.StartupPath.ToString() + @"\conveyor.txt", ref numArray, ref this.rubbery, true);
            numArray = new double[num];
            this.rubbery = new double[num];
            this.DataIOmy.TXTReadData(Application.StartupPath.ToString() + @"\conveyor.txt", ref numArray, ref this.rubbery, false);
            this.counterFemale = FrmGetSpecSet.Default.counterFemale;
            this.counterMale = FrmGetSpecSet.Default.counterMale;
            this.trackBarSpeed.Value = FrmGetSpecSet.Default.trackBarValue;
            this.CBisEnergy.Checked = FrmGetSpecSet.Default.GetSpecAutoIsEnergy;
            string[] strArray = FrmGetSpecSet.Default.WavelengthDiffIndex.Substring(0, FrmGetSpecSet.Default.WavelengthDiffIndex.Length - 1).Split(new char[] { ',' });
            this.selectWaveIndexDiff = new int[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i] != "")
                {
                    this.selectWaveIndexDiff[i] = int.Parse(strArray[i]);
                }
            }
            base.Update();
        }

        private double GetAngle(double[] a, double[] b)
        {
            Subspace subspace = new Subspace();
            int length = a.Length;
            double num2 = 100.0;
            double[,] numArray = new double[length, 1];
            double[,] numArray2 = new double[length, 1];
            if (length == b.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    numArray[i, 0] = a[i];
                    numArray2[i, 0] = b[i];
                }
                subspace.GetSubspaceAngle(numArray, numArray2, ref num2);
            }
            return num2;
        }

        private void GetBackGrd()
        {
            this.MySpectrometer.ReadDK(ref SpInfo);
            if (!Home.serialPortSetDevice.IsOpen)
            {
                try
                {
                    Home.serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                    return;
                }
            }
            byte[] buffer = Home.SPControlWord["ReferenceOn"];
            Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            Thread.Sleep(10);
            buffer = Home.SPControlWord["LightOn"];
            Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            buffer = Home.SPControlWord["ShutterOff"];
            Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            Thread.Sleep(0xbb8);
            this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, true);
            SpInfo.DataAB = new double[SpInfo.DataA.Length];
            SpInfo.DataA.CopyTo(SpInfo.DataAB, 0);
            SpInfo.DataA = null;
            this.DataIOmy.TXTSaveData(Application.StartupPath.ToString() + @"\background", SpInfo.DataX, SpInfo.DataAB);
            Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
            dataArray[0].DataX = SpInfo.WavelengthArray;
            dataArray[0].DataE = SpInfo.DataA;
            buffer = Home.SPControlWord["ReferenceOff"];
            Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = Home.SPControlWord["ReferenceOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.ToString());
            }
        }

        private void GetBackGrdAndBlack()
        {
            byte[] buffer;
            this.MySpectrometer.ReadDK(ref SpInfo);
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    try
                    {
                        Home.serialPortSetDevice.Open();
                        Thread.Sleep(100);
                    }
                    catch (Exception exception1)
                    {
                        MessageBox.Show(exception1.ToString());
                        return;
                    }
                }
                buffer = Home.SPControlWord["ReferenceOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(100);
                buffer = Home.SPControlWord["LightOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(100);
                buffer = Home.SPControlWord["ShutterOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception3)
            {
                MessageBox.Show(exception3.ToString());
                return;
            }
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = Home.SPControlWord["ReferenceOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(100);
            }
            catch (Exception exception4)
            {
                MessageBox.Show(exception4.ToString());
                return;
            }
            Thread.Sleep(0xbb8);
            this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, true);
            SpInfo.DataAD = new double[SpInfo.DataA.Length];
            SpInfo.DataA.CopyTo(SpInfo.DataAD, 0);
            SpInfo.DataA = null;
            this.DataIOmy.TXTSaveData(Application.StartupPath.ToString() + @"\dark", SpInfo.DataX, SpInfo.DataAD);
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = Home.SPControlWord["ShutterOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(10);
            }
            catch (Exception exception5)
            {
                MessageBox.Show(exception5.ToString());
                return;
            }
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = Home.SPControlWord["ShutterOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(10);
            }
            catch (Exception exception6)
            {
                MessageBox.Show(exception6.ToString());
                return;
            }
            Thread.Sleep(0x7d0);
            this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, true);
            Thread.Sleep(0x3e8);
            SpInfo.DataAB = new double[SpInfo.DataA.Length];
            SpInfo.DataA.CopyTo(SpInfo.DataAB, 0);
            SpInfo.DataA = null;
            this.DataIOmy.TXTSaveData(Application.StartupPath.ToString() + @"\background", SpInfo.DataX, SpInfo.DataAB);
            Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
            dataArray[0].DataX = SpInfo.WavelengthArray;
            dataArray[0].DataE = SpInfo.DataA;
            buffer = Home.SPControlWord["ReferenceOff"];
            Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = Home.SPControlWord["ReferenceOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.ToString());
            }
        }

        private void GetSpecAuto()
        {
            Dictionary<string, double[]> dictionary = new Dictionary<string, double[]>();
            this.silkWormSpec = new Spectrometer.Data[1];
            this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, false);
            if (this.CBisEnergy.Checked)
            {
                this.ViewStylemy = ViewStyle.Energy;
            }
            else
            {
                this.ViewStylemy = ViewStyle.Spec;
            }
            this.timer1.Enabled = true;
            while (startFlag)
            {
                int num;
                string str = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
                this.MySpectrometer.GetSingleBeamOnly(Spectrometer.spectrometerIndex, ref SpInfo);
                if (!this.CBisEnergy.Checked)
                {
                    if (SpInfo.DataAB.Length == SpInfo.DataA.Length)
                    {
                        SpInfo.DataY = new double[SpInfo.numPixls];
                        if (FrmGetSpecSet.Default.isEnergy)
                        {
                            num = 0;
                            while (num < SpInfo.DataA.Length)
                            {
                                try
                                {
                                    SpInfo.DataY[num] = SpInfo.DataA[num];
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
                                num++;
                            }
                        }
                        else if (Spectrometer.isClearDarks)
                        {
                            num = 0;
                            while (num < SpInfo.DataA.Length)
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
                                num++;
                            }
                        }
                        else
                        {
                            num = 0;
                            while (num < SpInfo.DataA.Length)
                            {
                                try
                                {
                                    SpInfo.DataY[num] = Convert.ToDouble(Math.Log10(Math.Abs((double) (SpInfo.DataAB[num] / SpInfo.DataA[num]))));
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
                                num++;
                            }
                        }
                    }
                }
                else
                {
                    SpInfo.DataY = SpInfo.DataA;
                }
                if (this.isWilm(SpInfo.DataA))
                {
                    this.silkWormStart = true;
                    string key = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
                    double[] numArray = new double[SpInfo.DataY.Length];
                    for (int i = 0; i < SpInfo.DataY.Length; i++)
                    {
                        numArray[i] = SpInfo.DataY[i];
                    }
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, numArray);
                    }
                }
                else if (this.silkWormStart)
                {
                    this.silkWormStart = false;
                    int num3 = 0;
                    double[,] x = new double[dictionary.Count - (2 * num3), SpInfo.numPixls];
                    int num4 = 0;
                    int num5 = 0;
                    string str3 = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
                    foreach (KeyValuePair<string, double[]> pair in dictionary)
                    {
                        num4++;
                        if ((num4 > num3) && (num4 < ((dictionary.Count - num3) + 1)))
                        {
                            for (num = 0; num < SpInfo.numPixls; num++)
                            {
                                x[num5, num] = pair.Value[num];
                            }
                            num5++;
                        }
                    }
                    double[] numArray3 = this.DataHandlingmy.SpMean(x);
                    this.silkWormSpec = new Spectrometer.Data[1];
                    this.silkWormSpec[0].DataX = SpInfo.DataX;
                    this.silkWormSpec[0].DataY = numArray3;
                    if (this.CBisEnergy.Checked)
                    {
                        this.silkWormSpec[0].DataE = numArray3;
                    }
                    this.specTempNum = dictionary.Count;
                    this.SpName = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
                    this.DataIOmy.TXTSaveData(Spectrometer.SavePath + @"\Mean-" + this.SpName + ".txt", this.silkWormSpec[0].DataX, this.silkWormSpec[0].DataY);
                    this.AppendText(this.SpName + "-Mean");
                    dictionary.Clear();
                    string str4 = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmGetSpecAuto));
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
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new Panel();
            this.trackBarSpeed.BeginInit();
            this.panel3.SuspendLayout();
            base.SuspendLayout();
            this.label7.AutoSize = true;
            this.label7.Location = new Point(40, 0x10c);
            this.label7.Margin = new Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x41, 12);
            this.label7.TabIndex = 0x37;
            this.label7.Text = "消耗时间：";
            this.txtTimeCount.Location = new Point(0x6a, 0x10a);
            this.txtTimeCount.Margin = new Padding(2);
            this.txtTimeCount.Name = "txtTimeCount";
            this.txtTimeCount.Size = new Size(0x4e, 0x15);
            this.txtTimeCount.TabIndex = 0x36;
            this.txtAngleThreshold.Location = new Point(0x86, 0xad);
            this.txtAngleThreshold.Margin = new Padding(2);
            this.txtAngleThreshold.Name = "txtAngleThreshold";
            this.txtAngleThreshold.Size = new Size(0x34, 0x15);
            this.txtAngleThreshold.TabIndex = 0x35;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(70, 0xb0);
            this.label1.Margin = new Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 12);
            this.label1.TabIndex = 0x34;
            this.label1.Text = "夹角阈值：";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xb3, 0xd5);
            this.label3.Margin = new Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x11, 12);
            this.label3.TabIndex = 0x2d;
            this.label3.Text = "快";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x41, 0xd5);
            this.label2.Margin = new Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x11, 12);
            this.label2.TabIndex = 0x2c;
            this.label2.Text = "慢";
            this.trackBarSpeed.Location = new Point(0x52, 210);
            this.trackBarSpeed.Margin = new Padding(2);
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.Size = new Size(0x67, 0x2d);
            this.trackBarSpeed.TabIndex = 0x2b;
            this.trackBarSpeed.Value = 5;
            this.trackBarSpeed.Scroll += new EventHandler(this.trackBarSpeed_Scroll);
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.panel3.BackColor = Color.WhiteSmoke;
            this.panel3.Controls.Add(this.CBisEnergy);
            this.panel3.Controls.Add(this.btnBackGrd);
            this.panel3.Controls.Add(this.btnValveOff);
            this.panel3.Controls.Add(this.btnStop);
            this.panel3.Controls.Add(this.btnConveyorOn);
            this.panel3.Controls.Add(this.btnGetSpec);
            this.panel3.Controls.Add(this.label18);
            this.panel3.Controls.Add(this.btnSpectrometerOff);
            this.panel3.Controls.Add(this.btnValve1On);
            this.panel3.Controls.Add(this.label17);
            this.panel3.Controls.Add(this.btnConveyorOff);
            this.panel3.Controls.Add(this.btnReferenceCupOff);
            this.panel3.Controls.Add(this.btnReferenceCupOn);
            this.panel3.Controls.Add(this.label15);
            this.panel3.Controls.Add(this.btnLightOff);
            this.panel3.Controls.Add(this.btnLightOn);
            this.panel3.Controls.Add(this.label16);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.txtTimeCount);
            this.panel3.Controls.Add(this.txtAngleThreshold);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.trackBarSpeed);
            this.panel3.Location = new Point(0x2cc, 0x2b);
            this.panel3.Margin = new Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(230, 0x239);
            this.panel3.TabIndex = 0x3e;
            this.CBisEnergy.AutoSize = true;
            this.CBisEnergy.Location = new Point(0x5f, 0x8f);
            this.CBisEnergy.Margin = new Padding(2);
            this.CBisEnergy.Name = "CBisEnergy";
            this.CBisEnergy.Size = new Size(60, 0x10);
            this.CBisEnergy.TabIndex = 0x4c;
            this.CBisEnergy.Text = "能量值";
            this.CBisEnergy.UseVisualStyleBackColor = true;
            this.CBisEnergy.CheckedChanged += new EventHandler(this.CBisEnergy_CheckedChanged);
            this.btnBackGrd.Anchor = AnchorStyles.Right;
            this.btnBackGrd.FlatStyle = FlatStyle.Flat;
            this.btnBackGrd.Location = new Point(0x3a, 0x12a);
            this.btnBackGrd.Margin = new Padding(4);
            this.btnBackGrd.Name = "btnBackGrd";
            this.btnBackGrd.Size = new Size(0x80, 0x19);
            this.btnBackGrd.TabIndex = 0x4b;
            this.btnBackGrd.Text = "背景";
            this.btnBackGrd.UseVisualStyleBackColor = true;
            this.btnBackGrd.Click += new EventHandler(this.btnBackGrd_Click);
            this.btnValveOff.Location = new Point(0x97, 0x20e);
            this.btnValveOff.Name = "btnValveOff";
            this.btnValveOff.Size = new Size(0x23, 0x27);
            this.btnValveOff.TabIndex = 0x4a;
            this.btnValveOff.Text = "关";
            this.btnValveOff.UseVisualStyleBackColor = true;
            this.btnValveOff.Visible = false;
            this.btnValveOff.Click += new EventHandler(this.btnValveOff_Click);
            this.btnStop.BackColor = Color.Transparent;
//            this.btnStop.BackgroundImage = Resources.Pause;
            this.btnStop.BackgroundImageLayout = ImageLayout.Stretch;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = FlatStyle.Flat;
            this.btnStop.ForeColor = Color.Transparent;
            this.btnStop.Location = new Point(0x89, 0x25);
            this.btnStop.Margin = new Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new Size(0x3a, 0x40);
            this.btnStop.TabIndex = 0x2a;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new EventHandler(this.btnStop_Click);
            this.btnConveyorOn.Location = new Point(0x6c, 0x160);
            this.btnConveyorOn.Name = "btnConveyorOn";
            this.btnConveyorOn.Size = new Size(0x23, 0x27);
            this.btnConveyorOn.TabIndex = 0x44;
            this.btnConveyorOn.Tag = " ";
            this.btnConveyorOn.Text = "启";
            this.btnConveyorOn.UseVisualStyleBackColor = true;
            this.btnConveyorOn.Visible = false;
            this.btnConveyorOn.Click += new EventHandler(this.btnConveyorOn_Click);
            this.btnGetSpec.Anchor = AnchorStyles.Right;
            this.btnGetSpec.BackColor = Color.Transparent;
//            this.btnGetSpec.BackgroundImage = Resources.Play2;
            this.btnGetSpec.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnGetSpec.FlatStyle = FlatStyle.Flat;
            this.btnGetSpec.ForeColor = Color.Transparent;
            this.btnGetSpec.Location = new Point(40, 0x25);
            this.btnGetSpec.Margin = new Padding(4);
            this.btnGetSpec.Name = "btnGetSpec";
            this.btnGetSpec.Size = new Size(0x3e, 0x40);
            this.btnGetSpec.TabIndex = 0x27;
            this.btnGetSpec.UseVisualStyleBackColor = false;
            this.btnGetSpec.Click += new EventHandler(this.btnGetSpec_Click);
            this.label18.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label18.Location = new Point(0x25, 0x1ec);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x42, 0x21);
            this.label18.TabIndex = 0x49;
            this.label18.Text = "阀门：";
            this.btnSpectrometerOff.Location = new Point(0x6a, 0x20f);
            this.btnSpectrometerOff.Name = "btnSpectrometerOff";
            this.btnSpectrometerOff.Size = new Size(0x23, 0x27);
            this.btnSpectrometerOff.TabIndex = 0x48;
            this.btnSpectrometerOff.Text = "2号开";
            this.btnSpectrometerOff.UseVisualStyleBackColor = true;
            this.btnSpectrometerOff.Visible = false;
            this.btnSpectrometerOff.Click += new EventHandler(this.btnSpectrometerOff_Click);
            this.btnValve1On.Location = new Point(120, 0x1e1);
            this.btnValve1On.Name = "btnValve1On";
            this.btnValve1On.Size = new Size(0x23, 0x27);
            this.btnValve1On.TabIndex = 0x47;
            this.btnValve1On.Tag = " ";
            this.btnValve1On.Text = "开";
            this.btnValve1On.UseVisualStyleBackColor = true;
            this.btnValve1On.Click += new EventHandler(this.btnValve1On_Click);
            this.label17.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label17.Location = new Point(0x1c, 360);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x5b, 0x21);
            this.label17.TabIndex = 70;
            this.label17.Text = "传送带：";
            this.label17.Visible = false;
            this.btnConveyorOff.Location = new Point(0x91, 0x160);
            this.btnConveyorOff.Name = "btnConveyorOff";
            this.btnConveyorOff.Size = new Size(0x23, 0x27);
            this.btnConveyorOff.TabIndex = 0x45;
            this.btnConveyorOff.Text = "停";
            this.btnConveyorOff.UseVisualStyleBackColor = true;
            this.btnConveyorOff.Visible = false;
            this.btnConveyorOff.Click += new EventHandler(this.btnConveyorOff_Click);
            this.btnReferenceCupOff.Location = new Point(0x92, 0x18c);
            this.btnReferenceCupOff.Name = "btnReferenceCupOff";
            this.btnReferenceCupOff.Size = new Size(0x23, 0x27);
            this.btnReferenceCupOff.TabIndex = 0x42;
            this.btnReferenceCupOff.Text = "回";
            this.btnReferenceCupOff.UseVisualStyleBackColor = true;
            this.btnReferenceCupOff.Click += new EventHandler(this.btnReferenceCupOff_Click);
            this.btnReferenceCupOn.Location = new Point(0x6d, 0x18c);
            this.btnReferenceCupOn.Name = "btnReferenceCupOn";
            this.btnReferenceCupOn.Size = new Size(0x23, 0x27);
            this.btnReferenceCupOn.TabIndex = 0x41;
            this.btnReferenceCupOn.Tag = " ";
            this.btnReferenceCupOn.Text = "出";
            this.btnReferenceCupOn.UseVisualStyleBackColor = true;
            this.btnReferenceCupOn.Click += new EventHandler(this.btnReferenceCupOn_Click);
            this.label15.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label15.Location = new Point(40, 0x1c0);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x42, 0x21);
            this.label15.TabIndex = 0x40;
            this.label15.Text = "灯光：";
            this.btnLightOff.Location = new Point(0x92, 440);
            this.btnLightOff.Name = "btnLightOff";
            this.btnLightOff.Size = new Size(0x23, 0x27);
            this.btnLightOff.TabIndex = 0x3f;
            this.btnLightOff.Text = "关";
            this.btnLightOff.UseVisualStyleBackColor = true;
            this.btnLightOff.Click += new EventHandler(this.btnLightOff_Click);
            this.btnLightOn.Location = new Point(110, 440);
            this.btnLightOn.Name = "btnLightOn";
            this.btnLightOn.Size = new Size(0x23, 0x27);
            this.btnLightOn.TabIndex = 0x3e;
            this.btnLightOn.Tag = " ";
            this.btnLightOn.Text = "开";
            this.btnLightOn.UseVisualStyleBackColor = true;
            this.btnLightOn.Click += new EventHandler(this.btnLightOn_Click);
            this.label16.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label16.Location = new Point(0x1c, 0x194);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x57, 0x21);
            this.label16.TabIndex = 0x43;
            this.label16.Text = "参比杯：";
            this.MyChart.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.MyChart.Location = new Point(0, 0x2b);
            this.MyChart.Margin = new Padding(4, 3, 4, 3);
            this.MyChart.Name = "MyChart";
            this.MyChart.ScrollGrace = 0.0;
            this.MyChart.ScrollMaxX = 0.0;
            this.MyChart.ScrollMaxY = 0.0;
            this.MyChart.ScrollMaxY2 = 0.0;
            this.MyChart.ScrollMinX = 0.0;
            this.MyChart.ScrollMinY = 0.0;
            this.MyChart.ScrollMinY2 = 0.0;
            this.MyChart.Size = new Size(0x1e6, 570);
            this.MyChart.TabIndex = 0x3f;
            this.timer1.Interval = 500;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.listBoxGetSpec.FormattingEnabled = true;
            this.listBoxGetSpec.ItemHeight = 12;
            this.listBoxGetSpec.Location = new Point(0x1ed, 0x41);
            this.listBoxGetSpec.Name = "listBoxGetSpec";
            this.listBoxGetSpec.Size = new Size(0xa5, 0x220);
            this.listBoxGetSpec.TabIndex = 0x40;
            this.label4.AutoSize = true;
            this.label4.Font = new Font("微软雅黑", 15f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label4.Location = new Point(0x1ee, 0x27);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x98, 0x1b);
            this.label4.TabIndex = 0x41;
            this.label4.Text = "采集文件列表：";
            this.panel1.BackColor = Color.FromArgb(0x26, 0x88, 210);
            this.panel1.Location = new Point(0x2a3, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(5, 0x287);
            this.panel1.TabIndex = 0x42;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x3bd, 0x271);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.listBoxGetSpec);
            base.Controls.Add(this.MyChart);
            base.Controls.Add(this.panel3);
            base.FormBorderStyle = FormBorderStyle.None;
//            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "FrmGetSpecAuto";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "自动采集窗口";
            base.FormClosing += new FormClosingEventHandler(this.FrmGetSpecAuto_FormClosing);
            base.Load += new EventHandler(this.FrmGetSpecAuto_Load);
            this.trackBarSpeed.EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool isWilm(double[] energyValue)
        {
            return (this.DataHandlingmy.StdError(energyValue) > FrmGetSpecSet.Default.ThresholdDiff);
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

        private void Report()
        {
            string content = string.Format("{0:yyyy-MM-dd   HH:mm:ss}", DateTime.Now) + "品种：" + FrmGetSpecSet.Default.txtBreed.ToString() + "\r\n批次：" + FrmGetSpecSet.Default.txtBatch.ToString() + "\r\n生产季节：" + FrmGetSpecSet.Default.txtMadeSeason.ToString() + "\r\n雌：" + this.counterFemale.ToString() + "\r\n雄：" + this.counterMale.ToString() + "\r\n\r\n";
            this.DataIOmy.TXTWriteIn(Application.StartupPath.ToString() + @"\Report.txt", content);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.CBisEnergy.Checked)
            {
                this.ViewStylemy = ViewStyle.Energy;
            }
            else
            {
                this.ViewStylemy = ViewStyle.Spec;
            }
            this.Draw("光 谱 图", this.silkWormSpec, this.silkWormSpec.Length - 1);
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["ConveyorOff"];
                buffer[3] = (byte) this.trackBarSpeed.Value;
                Home.SPControlWord["ConveyorOn"] = buffer;
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
                FrmGetSpecSet.Default.trackBarValue = this.trackBarSpeed.Value;
                FrmGetSpecSet.Default.Save();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private delegate void AddItemToListBoxDelegate(string str);

        private delegate void DrawDelegate(string Str, Spectrometer.Data[] DataGet, int Num);

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

