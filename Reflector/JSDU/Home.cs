namespace JSDU
{
    using NIRQUEST;
    using NIRQUEST.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.IO.Ports;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using static System.Collections.Specialized.BitVector32;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

    public class Home : Form
    {
        private int anglethreshold;
        private BackgroundWorker backgroundWorker1;
        private BackgroundWorker backgroundWorker2;
        private Button btnAdmin;
        private Button btnBack;
        private Button btnClearSensor;
        private Button btnClose;
        private Button btnConveyorOff;
        private Button btnConveyorOn;
        private Button btnCounterReset;
        private Button btnGetSpec;
        public Button btnGetSpectrum;
        public Button btnGetSpectrumAuto;
        private Button btnHelp;
        private Button btnHome;
        private Button btnLightControl;
        private Button btnLightOff;
        private Button btnLightOn;
        public Button btnModelMaker;
        private Button btnReferenceCupOff;
        private Button btnReferenceCupOn;
        public Button btnSetting;
        private Button btnShutterOff;
        private Button btnShutterOn;
        private Button btnSpectrometerOff;
        private Button btnStop;
        private Button btnValve1On;
        private Button btnValveOff;
        private CheckBox cBIsSaveSpec;
        public ComboBox comboxSerialPort;
        private IContainer components = null;
        private int counterFemale = 0;
        private int counterMale = 0;
        private DataHandling DataHandlingmy = new DataHandling();
        private DataIO DataIOmy = new DataIO();
        private Color[] DrawColor = new Color[20];
        public const byte FT_PURGE_RX = 1;
        public const byte FT_PURGE_TX = 2;
        private int isMale = 0;
        private Dictionary<int, string> kvSimcaInfo = new Dictionary<int, string>();
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lbFemale;
        public Label lblConnectState;
        private Label lbMale;
        private LoadForm loadfrm;
        private double[,] MeanY;
        private double[,] model;
        private Spectrometer MySpectrometer = new Spectrometer();
        public int numberOfSpectrometersFound;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel5;
        private Panel panel6;
        private Panel panel7;
        private Panel panel8;
        private Panel panel9;
        private Panel panelbtnDownTip;
        public Panel panelConnectState;
        private Panel panelCount;
        private Panel panelHeader;
        private Panel panelHome;
        private Panel panelOperate;
        private bool puffFemale = false;
        private bool puffMale = false;
        private double[] rubbery;
        private int[] selectWaveIndexDiff = null;
        public static SerialPort serialPortSetDevice = new SerialPort();
        private bool silkWormStart = false;
        private SimcaPrd SimcaPre = new SimcaPrd();
        public static Dictionary<string, byte[]> SPControlWord = new Dictionary<string, byte[]>();
        private Spectrometer.Data[] specData = new Spectrometer.Data[1];
        public static Spectrometer.SpecInfo SpInfo;
        private static bool startFlag = false;
        private double[,] tempMeanss = null;
        private long time1 = 0L;
        private long timeGetSp = 0L;
        public System.Windows.Forms.Timer timerClearSensor;
        private long timestart = 0L;
        private ToolTip toolTip1;
        private ToolTip toolTip10;
        private ToolTip toolTip2;
        private ToolTip toolTip3;
        private ToolTip toolTip4;
        private ToolTip toolTip5;
        private ToolTip toolTip6;
        private ToolTip toolTip7;
        private ToolTip toolTip8;
        private ToolTip toolTip9;
        private bool tPuffFlag = false;
        private TrackBar trackBarSpeed;
        private TextBox txtAngleThreshold;
        private TextBox txtBatch;
        private TextBox txtBoxPuffIntervalTime;
        private TextBox txtBreed;
        private TextBox txtMadeSeason;
        private TextBox txtTimeCount;
        private ViewStyle ViewStylemy = ViewStyle.Spec;
        private WebBrowser webBrowser1;

        public Home(LoadForm loadFrm)
        {
            this.InitializeComponent();
            this.loadfrm = loadFrm;
            SPControlWord.Add("ConveyorOn", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 50, 0x30, 0x30, 0x31, 0x23 });
            SPControlWord.Add("ConveyorOff", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 50, 0x30, 0x30, 0x31, 0x23 });
            SPControlWord.Add("ReferenceOn", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 0x31, 0x30, 0x30, 0x31, 0x23 });
            SPControlWord.Add("ReferenceOff", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 50, 0x30, 0x30, 0x31, 0x23 });
            SPControlWord.Add("Valve2On", new byte[0]);
            SPControlWord.Add("Valve1On", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 0x33, 0x30, 0x30, 0x31, 0x23 });
            SPControlWord.Add("ValveSkip", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 0x33, 0x30, 0x30, 50, 0x23 });
            SPControlWord.Add("LightOn", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 0x35, 0x30, 0x30, 0x31, 0x23 });
            SPControlWord.Add("LightOff", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 0x35, 0x30, 0x30, 50, 0x23 });
            SPControlWord.Add("ClearSensorOn", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 0x34, 0x30, 0x30, 0x31, 0x23 });
            SPControlWord.Add("ClearSensorOff", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 0x34, 0x30, 0x30, 50, 0x23 });
            SPControlWord.Add("ShutterOn", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 0x37, 0x30, 0x30, 0x31, 0x23 });
            SPControlWord.Add("ShutterOff", new byte[] { 0x53, 0x45, 80, 0x41, 0x54, 0x37, 0x30, 0x30, 50, 0x23 });
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Dictionary<string, double[]> dictionary = new Dictionary<string, double[]>();
            while (startFlag)
            {
                int num;
                this.timestart = DateTime.Now.Ticks;
                if ((this.timestart - this.time1) > (36000000000 * FrmGetSpecSet.Default.getBlackGrdIntervalTime))
                {
                    this.GetBackGrd();
                    this.Report();
                    this.time1 = this.timestart = DateTime.Now.Ticks;
                    FrmGetSpecSet.Default.counterFemale = 0;
                    FrmGetSpecSet.Default.counterMale = 0;
                    this.lbFemale.Text = "0";
                    this.lbMale.Text = "0";
                    this.counterFemale = 0;
                    this.counterMale = 0;
                    FrmGetSpecSet.Default.Save();
                }
                this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, false);
                if (SpInfo.DataAB.Length != SpInfo.DataA.Length)
                {
                    throw new ArgumentException("光谱维数不符");
                }
                SpInfo.DataY = new double[SpInfo.numPixls];
                if (!FrmGetSpecSet.Default.GetSpecAutoIsEnergy)
                {
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
                else
                {
                    SpInfo.DataY = new double[SpInfo.DataA.Length];
                    SpInfo.DataA.CopyTo(SpInfo.DataY, 0);
                }
                if (this.isWilm(SpInfo.DataA))
                {
                    this.silkWormStart = true;
                    double[] numArray = new double[SpInfo.DataY.Length];
                    num = 0;
                    while (num < SpInfo.DataY.Length)
                    {
                        numArray[num] = SpInfo.DataY[num];
                        num++;
                    }
                    string key = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, numArray);
                    }
                }
                else if (this.silkWormStart)
                {
                    this.silkWormStart = false;
                    int num2 = 0;
                    double[,] x = new double[dictionary.Count - (2 * num2), SpInfo.numPixls];
                    int num3 = 0;
                    int num4 = 0;
                    foreach (double[] numArray3 in dictionary.Values)
                    {
                        num3++;
                        num = 0;
                        while (num < SpInfo.numPixls)
                        {
                            x[num4, num] = numArray3[num];
                            num++;
                        }
                        num4++;
                    }
                    string str2 = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
                    double[] numArray4 = this.DataHandlingmy.SpMean(x);
                    double[,] numArray5 = new double[1, 0x181];
                    int num5 = 0;
                    num = 0x29;
                    while (num < 0x6d)
                    {
                        numArray5[0, num5++] = numArray4[num];
                        num++;
                    }
                    num = 0xa7;
                    while (num < 0x1e4)
                    {
                        numArray5[0, num5++] = numArray4[num];
                        num++;
                    }
                    num5 = 0;
                    ripsPreDeal deal = new ripsPreDeal(1, numArray4.Length);
                    int anglethreshold = FrmGetSpecSet.Default.anglethreshold;
                    int num7 = (int) Math.Ceiling((double) (((double) numArray5.GetLength(1)) / ((double) anglethreshold)));
                    this.tempMeanss = new double[1, num7];
                    for (num = 0; num < num7; num++)
                    {
                        this.tempMeanss[0, num] = numArray5[0, num * anglethreshold];
                    }
                    this.timeGetSp = DateTime.Now.Ticks;
                    int num8 = 0;
                    if (!this.backgroundWorker2.IsBusy)
                    {
                        this.backgroundWorker2.RunWorkerAsync();
                    }
                    else
                    {
                        Thread.Sleep(2);
                        num8++;
                        if (num8 > 15)
                        {
                            this.tPuffFlag = false;
                            this.backgroundWorker2.CancelAsync();
                        }
                    }
                    if (this.cBIsSaveSpec.Checked)
                    {
                        string str3 = "";
                        foreach (KeyValuePair<string, double[]> pair in dictionary)
                        {
                            this.DataIOmy.TXTSaveData(Spectrometer.SavePath + @"\" + pair.Key.ToString() + ".txt", SpInfo.DataX, pair.Value);
                            str3 = pair.Key.ToString();
                        }
                        this.DataIOmy.TXTSaveData(Spectrometer.SavePath + @"\" + str3 + "-mean.txt", SpInfo.DataX, numArray4);
                    }
                    dictionary.Clear();
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.MySpectrometer.wrapper.closeAllSpectrometers();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Calculate();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            Form[] mdiChildren;
            if (this.btnGetSpectrumAuto.Visible)
            {
                mdiChildren = base.MdiChildren;
                foreach (Form form in mdiChildren)
                {
                    form.Close();
                    form.Dispose();
                }
                this.panelHome.Visible = true;
                this.panelOperate.Visible = false;
                this.panel3.Visible = true;
                this.panelCount.Visible = true;
                this.panel3.Location = this.panelOperate.Location;
                this.panelbtnDownTip.Location = new Point(this.btnAdmin.Location.X + ((this.btnAdmin.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
                this.btnBack.Visible = true;
                this.btnGetSpectrumAuto.Enabled = true;
                this.btnGetSpectrum.Enabled = true;
                this.btnSetting.Enabled = true;
            }
            else
            {
                LoginForm form2 = new LoginForm();
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    mdiChildren = base.MdiChildren;
                    foreach (Form form in mdiChildren)
                    {
                        form.Close();
                        form.Dispose();
                    }
                    this.webBrowser1.Visible = false;
                    this.panelOperate.Visible = false;
                    this.panel3.Visible = true;
                    this.panelCount.Visible = true;
                    this.panel3.Location = this.panelOperate.Location;
                    this.btnGetSpectrum.Visible = true;
                    this.btnGetSpectrumAuto.Visible = true;
                    Point point = new Point(this.btnGetSpectrumAuto.Location.X, this.btnGetSpectrumAuto.Location.Y);
                    Point point2 = new Point(this.btnGetSpectrum.Location.X, this.btnGetSpectrum.Location.Y);
                    this.btnGetSpectrumAuto.Location = this.btnHome.Location;
                    this.btnGetSpectrum.Location = this.btnAdmin.Location;
                    this.btnHome.Visible = false;
                    this.btnAdmin.Location = point2;
                    this.panelbtnDownTip.Location = new Point(this.btnAdmin.Location.X + ((this.btnAdmin.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
                    this.btnBack.Visible = true;
                    this.btnGetSpectrumAuto.Enabled = true;
                    this.btnGetSpectrum.Enabled = true;
                    this.btnSetting.Enabled = true;
                }
            }
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.panel3.Visible = false;
            this.panelOperate.Visible = true;
            this.btnGetSpectrum.Visible = false;
            this.btnGetSpectrumAuto.Visible = false;
            Point point = new Point(this.btnGetSpectrumAuto.Location.X, this.btnGetSpectrumAuto.Location.Y);
            Point point2 = new Point(this.btnGetSpectrum.Location.X, this.btnGetSpectrum.Location.Y);
            this.btnGetSpectrumAuto.Location = this.btnHome.Location;
            this.btnGetSpectrum.Location = this.btnAdmin.Location;
            this.btnHome.Visible = true;
            this.btnHome.Location = point;
            this.btnAdmin.Location = point2;
            this.panelbtnDownTip.Location = new Point(this.btnHome.Location.X + ((this.btnHome.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
            this.btnBack.Visible = false;
            this.btnHome_Click(sender, e);
        }

        private void btnClearSensor_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(this.clearSensor)).Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                byte[] buffer = SPControlWord["LightOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(100);
                buffer = SPControlWord["ConveyorOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(100);
                buffer = SPControlWord["ReferenceOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(100);
                buffer = SPControlWord["ShutterOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            Form[] mdiChildren = base.MdiChildren;
            foreach (Form form in mdiChildren)
            {
                form.Close();
                form.Dispose();
            }
            if (this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.CancelAsync();
            }
            this.backgroundWorker1.Dispose();
            if (this.btnLightControl.BackgroundImage == Resources.开灯)
            {
                try
                {
                    this.btnLightOff_Click(sender, e);
                }
                catch
                {
                    MessageBox.Show("灯未正常关闭！");
                }
            }
            if (serialPortSetDevice.IsOpen)
            {
                serialPortSetDevice.Close();
            }
            if (serialPortSetDevice != null)
            {
                serialPortSetDevice.Dispose();
            }
            base.Close();
            base.Dispose();
            this.loadfrm.Close();
            this.loadfrm.Dispose();
        }

        private void btnConveyorOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                }
                byte[] buffer = SPControlWord["ConveyorOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
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
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                }
                byte[] buffer = SPControlWord["ConveyorOn"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
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

        private void btnGetSpec_Click(object sender, EventArgs e)
        {
            byte[] buffer;
            this.refreshModel();
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = SPControlWord["LightOn"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(100);
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.ToString());
            }
            this.btnGetSpec.Enabled = false;
            this.btnLightControl.Enabled = false;
            this.btnStop.Enabled = true;
            this.btnGetSpec.BackColor = Color.FromArgb(0x26, 0x88, 210);
            this.anglethreshold = int.Parse(this.txtAngleThreshold.Text);
            FrmGetSpecSet.Default.anglethreshold = this.anglethreshold;
            if (((FrmGetSpecSet.Default.txtBreed != this.txtBreed.Text.ToString()) || (FrmGetSpecSet.Default.txtBatch != this.txtBatch.Text.ToString())) || (FrmGetSpecSet.Default.txtMadeSeason != this.txtMadeSeason.Text.ToString()))
            {
                this.counterFemale = 0;
                this.counterMale = 0;
            }
            FrmGetSpecSet.Default.txtBreed = this.txtBreed.Text.ToString();
            FrmGetSpecSet.Default.txtBatch = this.txtBatch.Text.ToString();
            FrmGetSpecSet.Default.txtMadeSeason = this.txtMadeSeason.Text.ToString();
            FrmGetSpecSet.Default.puffIntervalTime = int.Parse(this.txtBoxPuffIntervalTime.Text);
            FrmGetSpecSet.Default.Save();
            startFlag = true;
            Spectrometer.DataGet = new Spectrometer.Data[20];
            this.MySpectrometer.wrapper.openAllSpectrometers();
            if (!FrmGetSpecSet.Default.GetSpecAutoIsEnergy)
            {
                this.GetBackGrdAndBlack();
            }
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = SPControlWord["ConveyorOn"];
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.ToString());
            }
            this.time1 = DateTime.Now.Ticks;
            this.timerClearSensor.Enabled = true;
            if (!this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("正在检测，请勿重复点击，谢谢！");
            }
        }

        private void btnGetSpectrum_Click(object sender, EventArgs e)
        {
            Form[] mdiChildren = base.MdiChildren;
            foreach (Form form in mdiChildren)
            {
                form.Close();
                form.Dispose();
            }
            serialPortSetDevice.Close();
            new FrmGetSpec(this) { MdiParent = this }.Show();
            this.panelbtnDownTip.Location = new Point(this.btnGetSpectrum.Location.X + ((this.btnGetSpectrum.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
            this.btnGetSpectrumAuto.Enabled = true;
            this.btnGetSpectrum.Enabled = false;
            this.btnSetting.Enabled = true;
            this.panelHome.Visible = false;
        }

        private void btnGetSpectrumAuto_Click(object sender, EventArgs e)
        {
            Form[] mdiChildren = base.MdiChildren;
            foreach (Form form in mdiChildren)
            {
                form.Close();
                form.Dispose();
            }
            serialPortSetDevice.Close();
            serialPortSetDevice.Dispose();
            new FrmGetSpecAuto(this) { MdiParent = this }.Show();
            this.panelbtnDownTip.Location = new Point(this.btnGetSpectrumAuto.Location.X + ((this.btnGetSpectrumAuto.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
            this.btnGetSpectrumAuto.Enabled = false;
            this.btnGetSpectrum.Enabled = true;
            this.btnSetting.Enabled = true;
            this.panelHome.Visible = false;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Form[] mdiChildren = base.MdiChildren;
            foreach (Form form in mdiChildren)
            {
                form.Close();
                form.Dispose();
            }
            this.panelbtnDownTip.Location = new Point(this.btnHelp.Location.X + ((this.btnHelp.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
            this.panelCount.Visible = false;
            this.panelOperate.Visible = false;
            this.panel3.Visible = false;
            this.panelHome.Visible = true;
            this.webBrowser1.Location = new Point(0, 0x4e);
            this.webBrowser1.Size = new Size(0x3fc, 590);
            this.webBrowser1.Visible = true;
            this.webBrowser1.Navigate(Application.StartupPath.ToString() + @"\help.html");
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Form[] mdiChildren = base.MdiChildren;
            foreach (Form form in mdiChildren)
            {
                form.Close();
                form.Dispose();
            }
            this.panelbtnDownTip.Location = new Point(this.btnHome.Location.X + ((this.btnHome.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
            this.panelHome.Visible = true;
            this.panelCount.Visible = true;
            this.panelOperate.Visible = true;
            this.webBrowser1.Visible = false;
            if (this.btnGetSpectrumAuto.Visible)
            {
                this.panel3.Visible = true;
            }
            this.btnGetSpectrumAuto.Enabled = true;
            this.btnGetSpectrum.Enabled = true;
            this.btnSetting.Enabled = true;
        }

        private void btnLightControl_Click(object sender, EventArgs e)
        {
            byte[] buffer;
            EventHandler method = null;
            EventHandler handler2 = null;
            if (this.btnLightControl.Tag.ToString() == "关灯")
            {
                try
                {
                    if (!serialPortSetDevice.IsOpen)
                    {
                        serialPortSetDevice.Open();
                        Thread.Sleep(100);
                    }
                    buffer = SPControlWord["LightOn"];
                    serialPortSetDevice.Write(buffer, 0, buffer.Length);
                }
                catch
                {
                    return;
                }
                try
                {
                    if (!serialPortSetDevice.IsOpen)
                    {
                        serialPortSetDevice.Open();
                        Thread.Sleep(100);
                    }
                    buffer = SPControlWord["LightOn"];
                    serialPortSetDevice.Write(buffer, 0, buffer.Length);
                }
                catch
                {
                    return;
                }
                if (method == null)
                {
                    method = delegate (object param0, EventArgs param1) {
                        this.btnLightControl.BackgroundImage = Resources.开灯;
                        this.btnLightControl.Tag = "开灯";
                    };
                }
                this.btnLightControl.Invoke(method);
            }
            else if (this.btnLightControl.Tag.ToString() == "开灯")
            {
                try
                {
                    if (!serialPortSetDevice.IsOpen)
                    {
                        serialPortSetDevice.Open();
                        Thread.Sleep(100);
                    }
                    buffer = SPControlWord["LightOff"];
                    serialPortSetDevice.Write(buffer, 0, buffer.Length);
                }
                catch
                {
                    return;
                }
                try
                {
                    if (!serialPortSetDevice.IsOpen)
                    {
                        serialPortSetDevice.Open();
                        Thread.Sleep(100);
                    }
                    buffer = SPControlWord["LightOff"];
                    serialPortSetDevice.Write(buffer, 0, buffer.Length);
                }
                catch
                {
                    return;
                }
                if (handler2 == null)
                {
                    handler2 = delegate (object param0, EventArgs param1) {
                        this.btnLightControl.BackgroundImage = Resources.关灯;
                        this.btnLightControl.Tag = "关灯";
                    };
                }
                this.btnLightControl.Invoke(handler2);
            }
        }

        private void btnLightOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                byte[] buffer = SPControlWord["LightOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
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
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                byte[] buffer = SPControlWord["LightOn"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnModelMaker_Click(object sender, EventArgs e)
        {
            Process.Start(Spectrometer.ApplicationPath + @"\ModelMaker\活体雌雄蚕蛹判别系统建模软件.exe");
        }

        private void btnReferenceCupOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                }
                byte[] buffer = SPControlWord["ReferenceOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
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
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                byte[] buffer = SPControlWord["ReferenceOn"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy)
            {
                MessageBox.Show("请先暂停蚕蛹分拣！");
            }
            else
            {
                if (this.backgroundWorker1.IsBusy)
                {
                    this.backgroundWorker1.CancelAsync();
                }
                this.backgroundWorker1.Dispose();
                this.MySpectrometer.wrapper.closeAllSpectrometers();
                serialPortSetDevice.Close();
                Form[] mdiChildren = base.MdiChildren;
                foreach (Form form in mdiChildren)
                {
                    form.Close();
                    form.Dispose();
                }
                new FrmSetting(this.MySpectrometer, null, this) { MdiParent = this }.Show();
                this.panelbtnDownTip.Location = new Point(this.btnSetting.Location.X + ((this.btnSetting.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
                this.btnGetSpectrumAuto.Enabled = true;
                this.btnGetSpectrum.Enabled = true;
                this.btnSetting.Enabled = false;
                this.panelHome.Visible = false;
            }
        }

        private void btnShutterOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                }
                byte[] buffer = SPControlWord["ShutterOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnShutterOn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                }
                byte[] buffer = SPControlWord["ShutterOn"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
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
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                }
                byte[] buffer = SPControlWord["Valve2On"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.btnGetSpec.BackColor = Color.Transparent;
            startFlag = false;
            this.btnGetSpec.Enabled = true;
            this.btnLightControl.Enabled = true;
            this.btnStop.Enabled = false;
            this.timerClearSensor.Enabled = true;
            FrmGetSpecSet.Default.counterFemale = this.counterFemale;
            FrmGetSpecSet.Default.counterMale = this.counterMale;
            FrmGetSpecSet.Default.Save();
        }

        private void btnValve1On_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                byte[] buffer = SPControlWord["Valve1On"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
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
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                }
                byte[] buffer = SPControlWord["ValveOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        /// <summary>
        /// 计算雌雄算法
        /// </summary>
        private void Calculate()
        {
            Exception exception;
            EventHandler method = null;
            EventHandler handler2 = null;
            string[] strArray2;
            this.tPuffFlag = true;
            this.DataIOmy.TXTWriteIn(Application.StartupPath + @"\parameterSimcaPrdtlog.txt", " PuffIn\r\n");
            string parameter = "ValveSkip";
            string[] parameterOut = null;
            int[] numArray = new int[] { 1, 1 };
            try
            {
                numArray = this.SimcaPre.SimcaPrdt(this.tempMeanss, this.model, out parameterOut);
            }
            catch (Exception exception1)
            {
                exception = exception1;
                this.DataIOmy.TXTWriteIn(Application.StartupPath + @"\parameterSimcaPrdtlog.txt", " CalcuEnd" + exception.ToString() + "\r\n");
            }
            long ticks = DateTime.Now.Ticks;
            if (this.kvSimcaInfo[numArray[0]] == "雄")
            {
                this.puffMale = true;
            }
            else if (this.kvSimcaInfo[numArray[0]] == "雌")
            {
                this.puffFemale = true;
            }
            if (this.puffMale)
            {
                try
                {
                    parameter = "Valve1On";
                    this.isMale = 1;
                    this.counterMale++;
                    if (method == null)
                    {
                        // lbMale.Invoke(new Action(() => { lbMale.Text = this.counterMale.ToString(); }));
                        // method = (EventHandler)((param0, param1) => (this.lbMale.Text = this.counterMale.ToString()));
                        lbMale.Invoke(new Action<object>((obj) => { lbMale.Text = counterMale.ToString(); }));
                    }
                    base.Invoke(method);
                    this.puffMale = false;
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    MessageBox.Show(exception.ToString());
                }
            }
            else if (this.puffFemale)
            {
                try
                {
                    parameter = "Valve2On";
                    this.counterFemale++;
                    //if (handler2 == null)
                    //{
                    //lbFemale.Invoke(new Action(() => { lbFemale.Text = counterFemale.ToString(); }));
                    lbFemale.Invoke(new Action<object>((obj) => { lbFemale.Text = counterFemale.ToString(); }));
                    //handler2 = new EventHandler(((param0, param1) => { this.lbFemale.Text = this.counterFemale.ToString(); });
                    //}
                    base.Invoke(handler2);
                    this.puffFemale = false;
                }
                catch (Exception exception3)
                {
                    MessageBox.Show(exception3.ToString());
                }
            }
            this.puffFemale = false;
            this.puffMale = false;
            new Thread(new ParameterizedThreadStart(this.puff)) { Priority = ThreadPriority.Highest }.Start(parameter);
            this.DataIOmy.TXTWriteIn(Application.StartupPath + @"\parameterSimcaPrdtlog.txt", "   puff:" + parameter);
            (strArray2 = parameterOut)[0] = strArray2[0] + "  " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff").ToString() + "-mean.txt\r\n";
            this.DataIOmy.TXTWriteIn(Application.StartupPath + @"\parameterSimcaPrdtlog.txt", parameterOut[0]);
        }

        private void cBIsSaveSpec_CheckedChanged(object sender, EventArgs e)
        {
            FrmGetSpecSet.Default.isSaveSpecAtDistinguish = this.cBIsSaveSpec.Checked;
            FrmGetSpecSet.Default.Save();
        }

        private void clearSensor()
        {
            if (!serialPortSetDevice.IsOpen)
            {
                serialPortSetDevice.Open();
                Thread.Sleep(100);
            }
            byte[] buffer = SPControlWord["ClearSensorOn"];
            serialPortSetDevice.Write(buffer, 0, buffer.Length);
            Thread.Sleep(0x2710);
            buffer = SPControlWord["ClearSensorOff"];
            serialPortSetDevice.Write(buffer, 0, buffer.Length);
        }

        private void comboxSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serialPortSetDevice != null)
            {
                serialPortSetDevice.Close();
                serialPortSetDevice.Dispose();
            }
            serialPortSetDevice = new SerialPort(this.comboxSerialPort.SelectedItem.ToString());
            serialPortSetDevice.BaudRate = 0x2580;
            serialPortSetDevice.Parity = Parity.Odd;
            serialPortSetDevice.DataBits = 8;
            serialPortSetDevice.StopBits = StopBits.One;
            this.serialPortSetDeviceOpen();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
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
            if (!serialPortSetDevice.IsOpen)
            {
                try
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                    return;
                }
            }
            byte[] buffer = SPControlWord["ReferenceOn"];
            serialPortSetDevice.Write(buffer, 0, buffer.Length);
            Thread.Sleep(10);
            buffer = SPControlWord["LightOn"];
            serialPortSetDevice.Write(buffer, 0, buffer.Length);
            buffer = SPControlWord["ShutterOff"];
            serialPortSetDevice.Write(buffer, 0, buffer.Length);
            Thread.Sleep(0xbb8);
            this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, true);
            SpInfo.DataAB = new double[SpInfo.DataA.Length];
            SpInfo.DataA.CopyTo(SpInfo.DataAB, 0);
            SpInfo.DataA = null;
            this.DataIOmy.TXTSaveData(Application.StartupPath.ToString() + @"\background", SpInfo.DataX, SpInfo.DataAB);
            Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
            dataArray[0].DataX = SpInfo.WavelengthArray;
            dataArray[0].DataE = SpInfo.DataA;
            buffer = SPControlWord["ReferenceOff"];
            serialPortSetDevice.Write(buffer, 0, buffer.Length);
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = SPControlWord["ReferenceOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
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
                if (!serialPortSetDevice.IsOpen)
                {
                    try
                    {
                        serialPortSetDevice.Open();
                        Thread.Sleep(100);
                    }
                    catch (Exception exception1)
                    {
                        MessageBox.Show(exception1.ToString());
                        return;
                    }
                }
                buffer = SPControlWord["ReferenceOn"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(100);
                buffer = SPControlWord["LightOn"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(100);
                buffer = SPControlWord["ShutterOn"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception3)
            {
                MessageBox.Show(exception3.ToString());
                return;
            }
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = SPControlWord["ReferenceOn"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
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
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = SPControlWord["ShutterOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(10);
            }
            catch (Exception exception5)
            {
                MessageBox.Show(exception5.ToString());
                return;
            }
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = SPControlWord["ShutterOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
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
            this.MySpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref SpInfo, true);
            SpInfo.DataAB = new double[SpInfo.DataA.Length];
            SpInfo.DataA.CopyTo(SpInfo.DataAB, 0);
            this.DataIOmy.TXTSaveData(Application.StartupPath.ToString() + @"\background", SpInfo.DataX, SpInfo.DataAB);
            Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
            dataArray[0].DataX = SpInfo.WavelengthArray;
            dataArray[0].DataE = SpInfo.DataA;
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = SPControlWord["ReferenceOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception7)
            {
                MessageBox.Show(exception7.ToString());
            }
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                buffer = SPControlWord["ReferenceOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception8)
            {
                MessageBox.Show(exception8.ToString());
            }
        }

        private void Home_Load(object sender, EventArgs e)
        {
            int num;
            base.WindowState = FormWindowState.Maximized;
            this.panelHome.Location = new Point((base.Width - this.panelHome.Width) / 2, (base.Height - this.panelHome.Height) / 2);
            this.panel6.Location = new Point(this.panelHome.Location.X, this.panelHome.Location.Y - this.panel6.Height);
            this.panel7.Location = new Point(this.panelHome.Location.X, this.panelHome.Location.Y + this.panelHome.Height);
            this.panelHeader.Location = new Point(this.panelHome.Location.X, this.panel6.Location.Y - this.panelHeader.Height);
            foreach (Control control in base.Controls)
            {
                try
                {
                    MdiClient client = (MdiClient) control;
                    client.BackColor = this.BackColor;
                }
                catch (InvalidCastException)
                {
                }
            }
            if (this.numberOfSpectrometersFound > 0)
            {
                Spectrometer.spectrometerIndex = 0;
                this.lblConnectState.Text = "已连接";
                this.panelConnectState.Visible = true;
                this.MySpectrometer.InitiateSpectrometer(this);
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
            string[] portNames = null;
            try
            {
                portNames = SerialPort.GetPortNames();
                num = 0;
                while (num < portNames.Length)
                {
                    this.comboxSerialPort.Items.Add(portNames[num].ToString());
                    num++;
                }
                this.comboxSerialPort.SelectedIndex = 0;
            }
            catch (Exception exception3)
            {
                Exception exception2 = exception3;
                MessageBox.Show("获取计算机COM口列表失败!\r\n错误信息:" + exception2.Message);
            }
            this.MySpectrometer.ReadBK(ref SpInfo);
            if (Spectrometer.isClearDarks)
            {
                this.MySpectrometer.ReadDK(ref SpInfo);
            }
            Spectrometer.IntegrationTimeBK = FrmGetSpecSet.Default.IntegrationTimeBK;
            this.trackBarSpeed.Value = FrmGetSpecSet.Default.trackBarSpeed;
            this.txtBoxPuffIntervalTime.Text = FrmGetSpecSet.Default.puffIntervalTime.ToString();
            this.anglethreshold = FrmGetSpecSet.Default.anglethreshold;
            this.txtAngleThreshold.Text = this.anglethreshold.ToString();
            double[] numArray = null;
            int num2 = this.DataIOmy.TXTReadData(Application.StartupPath.ToString() + @"\conveyor.txt", ref numArray, ref this.rubbery, true);
            numArray = new double[num2];
            this.rubbery = new double[num2];
            this.DataIOmy.TXTReadData(Application.StartupPath.ToString() + @"\conveyor.txt", ref numArray, ref this.rubbery, false);
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
            this.toolTip1.SetToolTip(this.btnGetSpectrumAuto, "自动采谱");
            this.toolTip2.SetToolTip(this.btnGetSpectrum, "手动采谱");
            this.toolTip3.SetToolTip(this.btnSetting, "设置");
            this.toolTip4.SetToolTip(this.btnGetSpec, "开始雌雄分拣");
            this.toolTip5.SetToolTip(this.btnStop, "暂停分拣");
            this.toolTip6.SetToolTip(this.btnClose, "退出");
            this.toolTip7.SetToolTip(this.btnAdmin, "管理员模式");
            this.toolTip8.SetToolTip(this.btnHome, "主页");
            this.toolTip9.SetToolTip(this.btnLightControl, "开/关灯");
            this.toolTip10.SetToolTip(this.btnModelMaker, "模型建立");
            string[] strArray2 = FrmGetSpecSet.Default.WavelengthDiffIndex.Substring(0, FrmGetSpecSet.Default.WavelengthDiffIndex.Length - 1).Split(new char[] { ',' });
            this.selectWaveIndexDiff = new int[strArray2.Length];
            this.selectWaveIndexDiff = new int[strArray2.Length];
            for (num = 0; num < strArray2.Length; num++)
            {
                if (strArray2[num] != "")
                {
                    this.selectWaveIndexDiff[num] = int.Parse(strArray2[num]);
                }
            }
            base.Update();
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                byte[] buffer = SPControlWord["LightOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(10);
                buffer = SPControlWord["ConveyorOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(10);
                buffer = SPControlWord["ReferenceOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(10);
                buffer = SPControlWord["ShutterOff"];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(10);
            }
            catch (Exception exception4)
            {
                MessageBox.Show(exception4.ToString());
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Home));
            this.panelHeader = new Panel();
            this.btnModelMaker = new Button();
            this.btnGetSpectrum = new Button();
            this.btnGetSpectrumAuto = new Button();
            this.btnBack = new Button();
            this.btnHome = new Button();
            this.panelbtnDownTip = new Panel();
            this.panel5 = new Panel();
            this.btnSetting = new Button();
            this.btnAdmin = new Button();
            this.comboxSerialPort = new ComboBox();
            this.btnClose = new Button();
            this.lblConnectState = new Label();
            this.lbMale = new Label();
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
            this.btnShutterOff = new Button();
            this.btnShutterOn = new Button();
            this.label10 = new Label();
            this.btnClearSensor = new Button();
            this.cBIsSaveSpec = new CheckBox();
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
            this.label9 = new Label();
            this.toolTip1 = new ToolTip(this.components);
            this.toolTip2 = new ToolTip(this.components);
            this.toolTip3 = new ToolTip(this.components);
            this.toolTip4 = new ToolTip(this.components);
            this.toolTip5 = new ToolTip(this.components);
            this.toolTip6 = new ToolTip(this.components);
            this.backgroundWorker2 = new BackgroundWorker();
            this.timerClearSensor = new System.Windows.Forms.Timer(this.components);
            this.panel6 = new Panel();
            this.panel7 = new Panel();
            this.panelConnectState = new Panel();
            this.panelCount = new Panel();
            this.label6 = new Label();
            this.panel1 = new Panel();
            this.panel8 = new Panel();
            this.btnCounterReset = new Button();
            this.panel9 = new Panel();
            this.panel2 = new Panel();
            this.webBrowser1 = new WebBrowser();
            this.panelHome = new Panel();
            this.panelOperate = new Panel();
            this.btnLightControl = new Button();
            this.btnGetSpec = new Button();
            this.btnStop = new Button();
            this.btnHelp = new Button();
            this.toolTip7 = new ToolTip(this.components);
            this.toolTip8 = new ToolTip(this.components);
            this.toolTip9 = new ToolTip(this.components);
            this.toolTip10 = new ToolTip(this.components);
            this.panelHeader.SuspendLayout();
            this.trackBarSpeed.BeginInit();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panelCount.SuspendLayout();
            this.panelHome.SuspendLayout();
            this.panelOperate.SuspendLayout();
            base.SuspendLayout();
            this.panelHeader.Anchor = AnchorStyles.Left;
            this.panelHeader.BackColor = Color.FromArgb(0x26, 0x88, 210);
            this.panelHeader.Controls.Add(this.btnModelMaker);
            this.panelHeader.Controls.Add(this.btnGetSpectrum);
            this.panelHeader.Controls.Add(this.btnGetSpectrumAuto);
            this.panelHeader.Controls.Add(this.btnBack);
            this.panelHeader.Controls.Add(this.btnHome);
            this.panelHeader.Controls.Add(this.panelbtnDownTip);
            this.panelHeader.Controls.Add(this.panel5);
            this.panelHeader.Controls.Add(this.btnSetting);
            this.panelHeader.Controls.Add(this.btnAdmin);
            this.panelHeader.Controls.Add(this.comboxSerialPort);
            this.panelHeader.Controls.Add(this.btnClose);
            this.panelHeader.Location = new Point(0, -1);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(0x3fc, 0x41);
            this.panelHeader.TabIndex = 0x1c;
            this.panelHeader.Paint += new PaintEventHandler(this.panelHeader_Paint);
            this.btnModelMaker.Anchor = AnchorStyles.Right;
            this.btnModelMaker.BackColor = Color.Transparent;
            //            this.btnModelMaker.BackgroundImage = Resources.建模;
            btnModelMaker.Text = "建模";
            this.btnModelMaker.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnModelMaker.FlatStyle = FlatStyle.Flat;
            this.btnModelMaker.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnModelMaker.Location = new Point(0x2ac, 8);
            this.btnModelMaker.Margin = new Padding(4);
            this.btnModelMaker.Name = "btnModelMaker";
            this.btnModelMaker.Size = new Size(0x3a, 50);
            this.btnModelMaker.TabIndex = 0x49;
            this.btnModelMaker.UseVisualStyleBackColor = false;
            this.btnModelMaker.Click += new EventHandler(this.btnModelMaker_Click);
            this.btnGetSpectrum.Anchor = AnchorStyles.Right;
            this.btnGetSpectrum.BackColor = Color.Transparent;
            //this.btnGetSpectrum.BackgroundImage = Resources.手动采谱;
            btnGetSpectrum.Text = "手动采集";
            this.btnGetSpectrum.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnGetSpectrum.FlatStyle = FlatStyle.Flat;
            this.btnGetSpectrum.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnGetSpectrum.Location = new Point(0x27b, 7);
            this.btnGetSpectrum.Margin = new Padding(4);
            this.btnGetSpectrum.Name = "btnGetSpectrum";
            this.btnGetSpectrum.Size = new Size(0x33, 0x2e);
            this.btnGetSpectrum.TabIndex = 0x41;
            this.btnGetSpectrum.UseVisualStyleBackColor = false;
            this.btnGetSpectrum.Visible = false;
            this.btnGetSpectrum.Click += new EventHandler(this.btnGetSpectrum_Click);
            this.btnGetSpectrumAuto.Anchor = AnchorStyles.Right;
            this.btnGetSpectrumAuto.BackColor = Color.Transparent;
            //this.btnGetSpectrumAuto.BackgroundImage = Resources.自动采谱;
            btnGetSpectrumAuto.Text = "自动采集";
            this.btnGetSpectrumAuto.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnGetSpectrumAuto.FlatStyle = FlatStyle.Flat;
            this.btnGetSpectrumAuto.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnGetSpectrumAuto.Location = new Point(0x239, 8);
            this.btnGetSpectrumAuto.Margin = new Padding(4);
            this.btnGetSpectrumAuto.Name = "btnGetSpectrumAuto";
            this.btnGetSpectrumAuto.Size = new Size(0x35, 0x2e);
            this.btnGetSpectrumAuto.TabIndex = 0x44;
            this.btnGetSpectrumAuto.UseVisualStyleBackColor = false;
            this.btnGetSpectrumAuto.Visible = false;
            this.btnGetSpectrumAuto.Click += new EventHandler(this.btnGetSpectrumAuto_Click);
            this.btnBack.Anchor = AnchorStyles.Right;
            this.btnBack.BackColor = Color.Transparent;
            //this.btnBack.BackgroundImage = Resources.返回;
            btnBack.Text = "返回";
            this.btnBack.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnBack.Location = new Point(11, 0x11);
            this.btnBack.Margin = new Padding(4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(0x23, 0x20);
            this.btnBack.TabIndex = 0x45;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Visible = false;
            this.btnBack.Click += new EventHandler(this.btnBack_Click_1);
            this.btnHome.Anchor = AnchorStyles.Right;
            this.btnHome.BackColor = Color.Transparent;
            //this.btnHome.BackgroundImage = Resources.主页图标;
            btnHome.Text = "主页图标";
            this.btnHome.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnHome.FlatStyle = FlatStyle.Flat;
            this.btnHome.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnHome.Location = new Point(0x2e3, 7);
            this.btnHome.Margin = new Padding(4);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new Size(0x42, 0x2e);
            this.btnHome.TabIndex = 70;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new EventHandler(this.btnHome_Click);
            //this.panelbtnDownTip.BackgroundImage = Resources.黄色箭头;
            
            this.panelbtnDownTip.BackgroundImageLayout = ImageLayout.Zoom;
            this.panelbtnDownTip.Location = new Point(760, 0x3a);
            this.panelbtnDownTip.Name = "panelbtnDownTip";
            this.panelbtnDownTip.Size = new Size(0x1c, 15);
            this.panelbtnDownTip.TabIndex = 0x48;
            //this.panel5.BackgroundImage = Resources.SEPAT;
            this.panel5.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel5.Location = new Point(0x2d, 6);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(0x7f, 0x38);
            this.panel5.TabIndex = 0x45;
            this.btnSetting.Anchor = AnchorStyles.Right;
            this.btnSetting.BackColor = Color.Transparent;
            //this.btnSetting.BackgroundImage = Resources.设置;
            btnSetting.Text = "设置";
            this.btnSetting.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnSetting.FlatStyle = FlatStyle.Flat;
            this.btnSetting.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnSetting.Location = new Point(0x371, 7);
            this.btnSetting.Margin = new Padding(4);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new Size(0x3a, 0x2e);
            this.btnSetting.TabIndex = 0x3f;
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new EventHandler(this.btnSetting_Click);
            this.btnAdmin.Anchor = AnchorStyles.Right;
            this.btnAdmin.BackColor = Color.Transparent;
            //this.btnAdmin.BackgroundImage = Resources.高级用户;
            btnAdmin.Text = "高级用户";
            this.btnAdmin.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnAdmin.FlatStyle = FlatStyle.Flat;
            this.btnAdmin.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnAdmin.Location = new Point(0x32d, 7);
            this.btnAdmin.Margin = new Padding(4);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new Size(60, 0x2e);
            this.btnAdmin.TabIndex = 0x43;
            this.btnAdmin.UseVisualStyleBackColor = false;
            this.btnAdmin.Click += new EventHandler(this.btnAdmin_Click);
            this.comboxSerialPort.Font = new Font("微软雅黑", 13.8f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.comboxSerialPort.FormattingEnabled = true;
            this.comboxSerialPort.Location = new Point(0x1a6, 0x12);
            this.comboxSerialPort.Margin = new Padding(2);
            this.comboxSerialPort.Name = "comboxSerialPort";
            this.comboxSerialPort.Size = new Size(80, 0x20);
            this.comboxSerialPort.TabIndex = 0x42;
            this.comboxSerialPort.Visible = false;
            this.comboxSerialPort.SelectedIndexChanged += new EventHandler(this.comboxSerialPort_SelectedIndexChanged);
            this.btnClose.Anchor = AnchorStyles.Right;
            this.btnClose.BackColor = Color.Transparent;
            //this.btnClose.BackgroundImage = Resources.退出;
            btnClose.Text = "退出";
            this.btnClose.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnClose.Location = new Point(0x3bc, 8);
            this.btnClose.Margin = new Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(50, 0x2d);
            this.btnClose.TabIndex = 0x40;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.lblConnectState.AutoSize = true;
            this.lblConnectState.Font = new Font("楷体", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.lblConnectState.ForeColor = Color.White;
            this.lblConnectState.Location = new Point(0x5b, 4);
            this.lblConnectState.Name = "lblConnectState";
            this.lblConnectState.Size = new Size(0x22, 0x15);
            this.lblConnectState.TabIndex = 11;
            this.lblConnectState.Text = "--";
            this.lbMale.AutoSize = true;
            this.lbMale.Font = new Font("黑体", 80.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.lbMale.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.lbMale.Location = new Point(0x160, 0x23);
            this.lbMale.Margin = new Padding(2, 0, 2, 0);
            this.lbMale.Name = "lbMale";
            this.lbMale.Size = new Size(0x63, 0x6b);
            this.lbMale.TabIndex = 1;
            this.lbMale.Text = "2";
            this.lbFemale.AutoSize = true;
            this.lbFemale.Font = new Font("黑体", 80.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.lbFemale.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.lbFemale.Location = new Point(0x160, 0xf7);
            this.lbFemale.Margin = new Padding(2, 0, 2, 0);
            this.lbFemale.Name = "lbFemale";
            this.lbFemale.Size = new Size(0x63, 0x6b);
            this.lbFemale.TabIndex = 0;
            this.lbFemale.Text = "1";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x24, 0x12d);
            this.label7.Margin = new Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x41, 12);
            this.label7.TabIndex = 0x37;
            this.label7.Text = "消耗时间：";
            this.txtTimeCount.Location = new Point(0x66, 300);
            this.txtTimeCount.Margin = new Padding(2);
            this.txtTimeCount.Name = "txtTimeCount";
            this.txtTimeCount.Size = new Size(0x4e, 0x15);
            this.txtTimeCount.TabIndex = 0x36;
            this.txtAngleThreshold.Location = new Point(130, 0x8b);
            this.txtAngleThreshold.Margin = new Padding(2);
            this.txtAngleThreshold.Name = "txtAngleThreshold";
            this.txtAngleThreshold.Size = new Size(0x34, 0x15);
            this.txtAngleThreshold.TabIndex = 0x35;
            this.txtAngleThreshold.Text = "1";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x44, 0x8d);
            this.label1.Margin = new Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 12);
            this.label1.TabIndex = 0x34;
            this.label1.Text = "相隔点数：";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0xb6, 0x107);
            this.label5.Margin = new Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x11, 12);
            this.label5.TabIndex = 0x30;
            this.label5.Text = "ms";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(14, 0x107);
            this.label4.Margin = new Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x53, 12);
            this.label4.TabIndex = 0x2f;
            this.label4.Text = "吹扫时间间隔:";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xb0, 0xb8);
            this.label3.Margin = new Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x11, 12);
            this.label3.TabIndex = 0x2d;
            this.label3.Text = "快";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x3e, 0xb8);
            this.label2.Margin = new Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x11, 12);
            this.label2.TabIndex = 0x2c;
            this.label2.Text = "慢";
            this.trackBarSpeed.Location = new Point(0x4e, 0xb3);
            this.trackBarSpeed.Margin = new Padding(2);
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.Size = new Size(0x67, 0x2d);
            this.trackBarSpeed.TabIndex = 0x2b;
            this.trackBarSpeed.Value = 5;
            this.trackBarSpeed.Scroll += new EventHandler(this.trackBarSpeed_Scroll);
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.txtBreed.Font = new Font("宋体", 13.8f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.txtBreed.Location = new Point(0x66, 20);
            this.txtBreed.Margin = new Padding(2);
            this.txtBreed.Name = "txtBreed";
            this.txtBreed.Size = new Size(0x68, 0x1c);
            this.txtBreed.TabIndex = 0x39;
            this.label8.AutoSize = true;
            this.label8.Font = new Font("微软雅黑", 13.8f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label8.Location = new Point(50, 0x12);
            this.label8.Margin = new Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new Size(50, 0x1a);
            this.label8.TabIndex = 0x38;
            this.label8.Text = "品种";
            this.txtMadeSeason.Font = new Font("宋体", 13.8f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.txtMadeSeason.Location = new Point(0x68, 0x62);
            this.txtMadeSeason.Margin = new Padding(2);
            this.txtMadeSeason.Name = "txtMadeSeason";
            this.txtMadeSeason.Size = new Size(0x68, 0x1c);
            this.txtMadeSeason.TabIndex = 0x3b;
            this.label11.AutoSize = true;
            this.label11.Font = new Font("微软雅黑", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label11.Location = new Point(14, 0x62);
            this.label11.Margin = new Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x58, 0x1a);
            this.label11.TabIndex = 0x3a;
            this.label11.Text = "生产季节";
            this.txtBatch.Font = new Font("宋体", 13.8f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.txtBatch.Location = new Point(0x66, 0x3a);
            this.txtBatch.Margin = new Padding(2);
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new Size(0x68, 0x1c);
            this.txtBatch.TabIndex = 0x3d;
            this.label12.AutoSize = true;
            this.label12.Font = new Font("微软雅黑", 13.8f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label12.Location = new Point(50, 0x39);
            this.label12.Margin = new Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new Size(50, 0x1a);
            this.label12.TabIndex = 60;
            this.label12.Text = "批次";
            this.panel3.BackColor = Color.White;
            this.panel3.Controls.Add(this.btnShutterOff);
            this.panel3.Controls.Add(this.btnShutterOn);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.btnClearSensor);
            this.panel3.Controls.Add(this.cBIsSaveSpec);
            this.panel3.Controls.Add(this.txtBoxPuffIntervalTime);
            this.panel3.Controls.Add(this.btnValveOff);
            this.panel3.Controls.Add(this.btnConveyorOn);
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
            this.panel3.Controls.Add(this.txtBatch);
            this.panel3.Controls.Add(this.txtMadeSeason);
            this.panel3.Controls.Add(this.txtBreed);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.txtTimeCount);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.txtAngleThreshold);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.trackBarSpeed);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.label9);
            this.panel3.ForeColor = SystemColors.Highlight;
            this.panel3.Location = new Point(0x1c9, 6);
            this.panel3.Margin = new Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x115, 0x24b);
            this.panel3.TabIndex = 0x3e;
            this.panel3.Visible = false;
            this.btnShutterOff.Location = new Point(0x91, 0x222);
            this.btnShutterOff.Name = "btnShutterOff";
            this.btnShutterOff.Size = new Size(0x23, 0x27);
            this.btnShutterOff.TabIndex = 0x51;
            this.btnShutterOff.Text = "回";
            this.btnShutterOff.UseVisualStyleBackColor = true;
            this.btnShutterOff.Click += new EventHandler(this.btnShutterOff_Click);
            this.btnShutterOn.Location = new Point(0x6c, 0x222);
            this.btnShutterOn.Name = "btnShutterOn";
            this.btnShutterOn.Size = new Size(0x23, 0x27);
            this.btnShutterOn.TabIndex = 80;
            this.btnShutterOn.Tag = " ";
            this.btnShutterOn.Text = "出";
            this.btnShutterOn.UseVisualStyleBackColor = true;
            this.btnShutterOn.Click += new EventHandler(this.btnShutterOn_Click);
            this.label10.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label10.Location = new Point(0x1b, 0x22a);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x57, 0x21);
            this.label10.TabIndex = 0x52;
            this.label10.Text = "遮光片：";
            this.btnClearSensor.Location = new Point(0x70, 0x1f9);
            this.btnClearSensor.Name = "btnClearSensor";
            this.btnClearSensor.Size = new Size(0x23, 0x27);
            this.btnClearSensor.TabIndex = 0x4e;
            this.btnClearSensor.Tag = " ";
            this.btnClearSensor.Text = "开";
            this.btnClearSensor.UseVisualStyleBackColor = true;
            this.btnClearSensor.Click += new EventHandler(this.btnClearSensor_Click);
            this.cBIsSaveSpec.AutoSize = true;
            this.cBIsSaveSpec.Font = new Font("宋体", 14.25f);
            this.cBIsSaveSpec.Location = new Point(0x4e, 0xe4);
            this.cBIsSaveSpec.Margin = new Padding(2);
            this.cBIsSaveSpec.Name = "cBIsSaveSpec";
            this.cBIsSaveSpec.Size = new Size(0x68, 0x17);
            this.cBIsSaveSpec.TabIndex = 0x4d;
            this.cBIsSaveSpec.Text = "保存光谱";
            this.cBIsSaveSpec.UseVisualStyleBackColor = true;
            this.cBIsSaveSpec.CheckedChanged += new EventHandler(this.cBIsSaveSpec_CheckedChanged);
            this.txtBoxPuffIntervalTime.Location = new Point(0x63, 0x105);
            this.txtBoxPuffIntervalTime.Margin = new Padding(2);
            this.txtBoxPuffIntervalTime.Name = "txtBoxPuffIntervalTime";
            this.txtBoxPuffIntervalTime.Size = new Size(0x4e, 0x15);
            this.txtBoxPuffIntervalTime.TabIndex = 0x4b;
            this.btnValveOff.Location = new Point(0xc7, 0x224);
            this.btnValveOff.Name = "btnValveOff";
            this.btnValveOff.Size = new Size(0x23, 0x27);
            this.btnValveOff.TabIndex = 0x4a;
            this.btnValveOff.Text = "关";
            this.btnValveOff.UseVisualStyleBackColor = true;
            this.btnValveOff.Visible = false;
            this.btnValveOff.Click += new EventHandler(this.btnValveOff_Click);
            this.btnConveyorOn.Location = new Point(0x67, 0x14b);
            this.btnConveyorOn.Name = "btnConveyorOn";
            this.btnConveyorOn.Size = new Size(0x23, 0x27);
            this.btnConveyorOn.TabIndex = 0x44;
            this.btnConveyorOn.Tag = " ";
            this.btnConveyorOn.Text = "启";
            this.btnConveyorOn.UseVisualStyleBackColor = true;
            this.btnConveyorOn.Visible = false;
            this.btnConveyorOn.Click += new EventHandler(this.btnConveyorOn_Click);
            this.label18.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label18.Location = new Point(0x23, 0x1d7);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x42, 0x21);
            this.label18.TabIndex = 0x49;
            this.label18.Text = "阀门：";
            this.btnSpectrometerOff.Location = new Point(240, 0x221);
            this.btnSpectrometerOff.Name = "btnSpectrometerOff";
            this.btnSpectrometerOff.Size = new Size(0x27, 0x27);
            this.btnSpectrometerOff.TabIndex = 0x48;
            this.btnSpectrometerOff.Text = "2号开";
            this.btnSpectrometerOff.UseVisualStyleBackColor = true;
            this.btnSpectrometerOff.Visible = false;
            this.btnSpectrometerOff.Click += new EventHandler(this.btnSpectrometerOff_Click);
            this.btnValve1On.Location = new Point(0x70, 0x1d0);
            this.btnValve1On.Name = "btnValve1On";
            this.btnValve1On.Size = new Size(0x23, 0x27);
            this.btnValve1On.TabIndex = 0x47;
            this.btnValve1On.Tag = " ";
            this.btnValve1On.Text = "开";
            this.btnValve1On.UseVisualStyleBackColor = true;
            this.btnValve1On.Click += new EventHandler(this.btnValve1On_Click);
            this.label17.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label17.Location = new Point(0x18, 0x153);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x5b, 0x21);
            this.label17.TabIndex = 70;
            this.label17.Text = "传送带：";
            this.label17.Visible = false;
            this.btnConveyorOff.Location = new Point(140, 0x14b);
            this.btnConveyorOff.Name = "btnConveyorOff";
            this.btnConveyorOff.Size = new Size(0x23, 0x27);
            this.btnConveyorOff.TabIndex = 0x45;
            this.btnConveyorOff.Text = "停";
            this.btnConveyorOff.UseVisualStyleBackColor = true;
            this.btnConveyorOff.Visible = false;
            this.btnConveyorOff.Click += new EventHandler(this.btnConveyorOff_Click);
            this.btnReferenceCupOff.Location = new Point(0x8e, 0x177);
            this.btnReferenceCupOff.Name = "btnReferenceCupOff";
            this.btnReferenceCupOff.Size = new Size(0x23, 0x27);
            this.btnReferenceCupOff.TabIndex = 0x42;
            this.btnReferenceCupOff.Text = "回";
            this.btnReferenceCupOff.UseVisualStyleBackColor = true;
            this.btnReferenceCupOff.Click += new EventHandler(this.btnReferenceCupOff_Click);
            this.btnReferenceCupOn.Location = new Point(0x69, 0x177);
            this.btnReferenceCupOn.Name = "btnReferenceCupOn";
            this.btnReferenceCupOn.Size = new Size(0x23, 0x27);
            this.btnReferenceCupOn.TabIndex = 0x41;
            this.btnReferenceCupOn.Tag = " ";
            this.btnReferenceCupOn.Text = "出";
            this.btnReferenceCupOn.UseVisualStyleBackColor = true;
            this.btnReferenceCupOn.Click += new EventHandler(this.btnReferenceCupOn_Click);
            this.label15.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label15.Location = new Point(0x24, 0x1ab);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x42, 0x21);
            this.label15.TabIndex = 0x40;
            this.label15.Text = "灯光：";
            this.btnLightOff.Location = new Point(0x8e, 0x1a3);
            this.btnLightOff.Name = "btnLightOff";
            this.btnLightOff.Size = new Size(0x23, 0x27);
            this.btnLightOff.TabIndex = 0x3f;
            this.btnLightOff.Text = "关";
            this.btnLightOff.UseVisualStyleBackColor = true;
            this.btnLightOff.Click += new EventHandler(this.btnLightOff_Click);
            this.btnLightOn.Location = new Point(0x6a, 0x1a3);
            this.btnLightOn.Name = "btnLightOn";
            this.btnLightOn.Size = new Size(0x23, 0x27);
            this.btnLightOn.TabIndex = 0x3e;
            this.btnLightOn.Tag = " ";
            this.btnLightOn.Text = "开";
            this.btnLightOn.UseVisualStyleBackColor = true;
            this.btnLightOn.Click += new EventHandler(this.btnLightOn_Click);
            this.label16.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label16.Location = new Point(0x18, 0x17f);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x57, 0x21);
            this.label16.TabIndex = 0x43;
            this.label16.Text = "参比杯：";
            this.label9.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label9.Location = new Point(11, 0x200);
            this.label9.Name = "label9";
            this.label9.Size = new Size(120, 0x21);
            this.label9.TabIndex = 0x4f;
            this.label9.Text = "清洗探头：";
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.timerClearSensor.Tick += new EventHandler(this.timerClearSensor_Tick);
            this.panel6.BackColor = Color.FromArgb(0xf4, 210, 0x20);
            this.panel6.Location = new Point(0, 0x40);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(0x3fc, 12);
            this.panel6.TabIndex = 0x45;
            this.panel7.BackColor = Color.FromArgb(0x26, 0x88, 210);
            this.panel7.Controls.Add(this.panelConnectState);
            this.panel7.Controls.Add(this.lblConnectState);
            this.panel7.Location = new Point(0, 0x2a1);
            this.panel7.Name = "panel7";
            this.panel7.Size = new Size(0x3fc, 0x1b);
            this.panel7.TabIndex = 70;
            //this.panelConnectState.BackgroundImage = Resources.已连接图标;
           
            this.panelConnectState.BackgroundImageLayout = ImageLayout.Zoom;
            this.panelConnectState.Location = new Point(0x2d, 5);
            this.panelConnectState.Name = "panelConnectState";
            this.panelConnectState.Size = new Size(0x22, 20);
            this.panelConnectState.TabIndex = 12;
            this.panelConnectState.Visible = false;
            this.panelCount.Controls.Add(this.label6);
            this.panelCount.Controls.Add(this.panel1);
            this.panelCount.Controls.Add(this.lbFemale);
            this.panelCount.Controls.Add(this.panel8);
            this.panelCount.Controls.Add(this.lbMale);
            this.panelCount.Controls.Add(this.btnCounterReset);
            this.panelCount.Controls.Add(this.panel9);
            this.panelCount.Controls.Add(this.panel2);
            this.panelCount.Location = new Point(2, 0x73);
            this.panelCount.Name = "panelCount";
            this.panelCount.Size = new Size(740, 0x165);
            this.panelCount.TabIndex = 0x4d;
            this.label6.AutoSize = true;
            this.label6.Font = new Font("微软雅黑", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label6.Location = new Point(0x36, 0xa6);
            this.label6.Margin = new Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(50, 0x1a);
            this.label6.TabIndex = 0x45;
            this.label6.Text = "归零";
            //this.panel1.BackgroundImage = Resources.蓝色蚕蛹图标;
            this.panel1.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel1.Location = new Point(0x81, 0x20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x39, 0x72);
            this.panel1.TabIndex = 0x47;
            //this.panel8.BackgroundImage = Resources.雌性蚕蛹图标;
            this.panel8.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel8.Location = new Point(0xd4, 240);
            this.panel8.Name = "panel8";
            this.panel8.Size = new Size(0x7b, 0x72);
            this.panel8.TabIndex = 0x4a;
            this.btnCounterReset.Anchor = AnchorStyles.Right;
            this.btnCounterReset.BackColor = Color.White;
            //this.btnCounterReset.BackgroundImage = Resources.归零;
            btnCounterReset.Text = "归零";
            this.btnCounterReset.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnCounterReset.FlatStyle = FlatStyle.Flat;
            this.btnCounterReset.ForeColor = Color.Transparent;
            this.btnCounterReset.Location = new Point(0, 0x7e);
            this.btnCounterReset.Margin = new Padding(4);
            this.btnCounterReset.Name = "btnCounterReset";
            this.btnCounterReset.Size = new Size(0x5b, 0x67);
            this.btnCounterReset.TabIndex = 0x44;
            this.btnCounterReset.UseVisualStyleBackColor = false;
            this.btnCounterReset.Click += new EventHandler(this.btnCounterReset_Click);
            //this.panel9.BackgroundImage = Resources.红色蚕蛹图标;
            this.panel9.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel9.Location = new Point(0x81, 240);
            this.panel9.Name = "panel9";
            this.panel9.Size = new Size(0x39, 0x72);
            this.panel9.TabIndex = 0x49;
            //this.panel2.BackgroundImage = Resources.雄性图标;
            this.panel2.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel2.Location = new Point(0xd4, 0x20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x7b, 0x72);
            this.panel2.TabIndex = 0x48;
            this.webBrowser1.Location = new Point(4, 3);
            this.webBrowser1.MinimumSize = new Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new Size(0x34, 0x22);
            this.webBrowser1.TabIndex = 0x4f;
            this.panelHome.Controls.Add(this.panel3);
            this.panelHome.Controls.Add(this.webBrowser1);
            this.panelHome.Controls.Add(this.panelOperate);
            this.panelHome.Controls.Add(this.panelCount);
            this.panelHome.Location = new Point(0, 0x4e);
            this.panelHome.Name = "panelHome";
            this.panelHome.Size = new Size(0x3fc, 0x252);
            this.panelHome.TabIndex = 0x51;
            //this.panelOperate.BackgroundImage = Resources.分栏;
            panelOperate.Tag = "分栏";
            this.panelOperate.BackgroundImageLayout = ImageLayout.Stretch;
            this.panelOperate.Controls.Add(this.btnLightControl);
            this.panelOperate.Controls.Add(this.btnGetSpec);
            this.panelOperate.Controls.Add(this.btnStop);
            this.panelOperate.Controls.Add(this.btnHelp);
            this.panelOperate.Location = new Point(0x2e3, 6);
            this.panelOperate.Name = "panelOperate";
            this.panelOperate.Size = new Size(0x119, 0x24b);
            this.panelOperate.TabIndex = 0x4b;
            this.btnLightControl.BackColor = Color.Transparent;
            //this.btnLightControl.BackgroundImage = Resources.关灯;
            btnLightControl.Text = "关灯";
            this.btnLightControl.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnLightControl.Cursor = Cursors.Hand;
            this.btnLightControl.FlatStyle = FlatStyle.Flat;
            this.btnLightControl.ForeColor = Color.Transparent;
            this.btnLightControl.Location = new Point(0x4a, 0x1b6);
            this.btnLightControl.Margin = new Padding(4);
            this.btnLightControl.Name = "btnLightControl";
            this.btnLightControl.Size = new Size(0x83, 0x80);
            this.btnLightControl.TabIndex = 0x2b;
            this.btnLightControl.Tag = "关灯";
            this.btnLightControl.UseVisualStyleBackColor = false;
            this.btnLightControl.Visible = false;
            this.btnLightControl.Click += new EventHandler(this.btnLightControl_Click);
            this.btnGetSpec.BackColor = Color.Transparent;
            //this.btnGetSpec.BackgroundImage = Resources.开始图标;
            btnGetSpec.Text = "开始图标";
            this.btnGetSpec.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnGetSpec.Cursor = Cursors.Hand;
            this.btnGetSpec.FlatStyle = FlatStyle.Flat;
            this.btnGetSpec.ForeColor = Color.Transparent;
            this.btnGetSpec.Location = new Point(0x56, 0x20);
            this.btnGetSpec.Margin = new Padding(4);
            this.btnGetSpec.Name = "btnGetSpec";
            this.btnGetSpec.Size = new Size(0x83, 0x80);
            this.btnGetSpec.TabIndex = 0x27;
            this.btnGetSpec.UseVisualStyleBackColor = false;
            this.btnGetSpec.Click += new EventHandler(this.btnGetSpec_Click);
            this.btnStop.BackColor = Color.Transparent;
            //this.btnStop.BackgroundImage = Resources.结束__暂停;
            btnStop.Text = "结束__暂停";
            this.btnStop.BackgroundImageLayout = ImageLayout.Stretch;
            this.btnStop.Cursor = Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = FlatStyle.Flat;
            this.btnStop.ForeColor = Color.Transparent;
            this.btnStop.Location = new Point(0x67, 250);
            this.btnStop.Margin = new Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new Size(0x61, 0x72);
            this.btnStop.TabIndex = 0x2a;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new EventHandler(this.btnStop_Click);
            this.btnHelp.Anchor = AnchorStyles.Right;
            this.btnHelp.BackColor = Color.Transparent;
            //this.btnHelp.BackgroundImage = Resources.帮助;
            btnHelp.Text = "帮助";
            this.btnHelp.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnHelp.FlatStyle = FlatStyle.Flat;
            this.btnHelp.ForeColor = Color.FromArgb(0x26, 0x88, 210);
            this.btnHelp.Location = new Point(0xe1, 4);
            this.btnHelp.Margin = new Padding(4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x34, 0x2b);
            this.btnHelp.TabIndex = 0x47;
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Visible = false;
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x3fb, 700);
            base.Controls.Add(this.panelHome);
            base.Controls.Add(this.panel7);
            base.Controls.Add(this.panel6);
            base.Controls.Add(this.panelHeader);
            base.FormBorderStyle = FormBorderStyle.None;
            //base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.IsMdiContainer = true;
            base.Name = "Home";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "蚕蛹剔除";
            base.Load += new EventHandler(this.Home_Load);
            this.panelHeader.ResumeLayout(false);
            this.trackBarSpeed.EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panelCount.ResumeLayout(false);
            this.panelCount.PerformLayout();
            this.panelHome.ResumeLayout(false);
            this.panelOperate.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private bool isWilm(double[] energyValue)
        {
            return (this.DataHandlingmy.StdError(energyValue) > FrmGetSpecSet.Default.ThresholdDiff);
        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
        }

        private void puff(object obj)
        {
            string str = (string) obj;
            Thread.Sleep(FrmGetSpecSet.Default.puffIntervalTime);
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                    Thread.Sleep(100);
                }
                byte[] buffer = SPControlWord[str];
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
            }
            catch (Exception exception)
            {
                this.DataIOmy.TXTWriteIn(Application.StartupPath + @"\parameterSimcaPrdtlog.txt", "   puff:" + exception.ToString());
            }
        }

        public void refreshModel()
        {
            if (File.Exists(Application.StartupPath + @"\Model\SIMCA\model.txt"))
            {
                this.model = null;
                try
                {
                    this.DataIOmy.TXTReadDatas(Application.StartupPath + @"\Model\SIMCA\model.txt", out this.model);
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    try
                    {
                        this.DataIOmy.LoadSIMCAModel(Application.StartupPath + @"\Model\SIMCA\model.txt", out this.model);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("模型读取错误！");
                    }
                }
            }
            else
            {
                MessageBox.Show("模型文件不存在");
            }
            try
            {
                string[] strArray;
                this.DataIOmy.ReadLineTXT(Application.StartupPath + @"\Model\SIMCA\Info.txt", out strArray);
                if (strArray.Length <= 1)
                {
                    throw new Exception("模型信息读取错误！");
                }
                this.kvSimcaInfo.Clear();
                for (int i = 0; i < 2; i++)
                {
                    string[] strArray2 = strArray[i].Split(new char[] { ',', '，', ' ' });
                    if (strArray2.Length <= 1)
                    {
                        throw new Exception("模型信息错误！");
                    }
                    if ((strArray2[1].Contains("雌") || strArray2[1].Contains("C")) || strArray2[1].Contains("c"))
                    {
                        this.kvSimcaInfo.Add(int.Parse(strArray2[0]), "雌");
                    }
                    else if ((strArray2[1].Contains("雄") || strArray2[1].Contains("X")) || strArray2[1].Contains("x"))
                    {
                        this.kvSimcaInfo.Add(int.Parse(strArray2[0]), "雄");
                    }
                }
            }
            catch (Exception exception4)
            {
                MessageBox.Show(exception4.ToString());
                this.kvSimcaInfo.Clear();
                this.kvSimcaInfo.Add(0, "雌");
                this.kvSimcaInfo.Add(1, "雄");
            }
        }

        private void Report()
        {
            string content = string.Format("{0:yyyy-MM-dd   HH:mm:ss}", DateTime.Now) + "品种：" + FrmGetSpecSet.Default.txtBreed.ToString() + "\r\n批次：" + FrmGetSpecSet.Default.txtBatch.ToString() + "\r\n生产季节：" + FrmGetSpecSet.Default.txtMadeSeason.ToString() + "\r\n雌：" + this.counterFemale.ToString() + "\r\n雄：" + this.counterMale.ToString() + "\r\n\r\n";
            this.DataIOmy.TXTWriteIn(Application.StartupPath.ToString() + @"\Report.txt", content);
        }

        private void serialPortSetDeviceOpen()
        {
            if (serialPortSetDevice.IsOpen)
            {
            }
            try
            {
                serialPortSetDevice.Open();
                Thread.Sleep(100);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void timerClearSensor_Tick(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(this.clearSensor)).Start();
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortSetDevice.IsOpen)
                {
                    serialPortSetDevice.Open();
                }
                byte[] buffer = SPControlWord["ConveyorOff"];
                buffer[3] = (byte) this.trackBarSpeed.Value;
                SPControlWord["ConveyorOn"] = buffer;
                serialPortSetDevice.Write(buffer, 0, buffer.Length);
                FrmGetSpecSet.Default.trackBarValue = this.trackBarSpeed.Value;
                FrmGetSpecSet.Default.Save();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private delegate void DrawDelegate(string Str, Spectrometer.Data[] DataGet, int Num);

        private enum ViewStyle
        {
            Mean,
            StdErr,
            Spec,
            Energy
        }
    }
}

