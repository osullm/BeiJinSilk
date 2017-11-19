namespace JSDU
{
    using Microsoft.Win32;
    using NIRQUEST;
    using NIRQUEST.Properties;
    using SPAM;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using ZedGraph;

    public class FrmSetting : Form
    {
        private int AutoFindIntegrationTimeCount = 0;
        private bool AutoFindIntegrationTimes = false;
        private bool AutoFindIntegrationTimess = false;
        private BackgroundWorker backgroundWorker1;
        private BackgroundWorker backgroundWorker2;
        private BackgroundWorker bkwGetConveySpec;
        private Button btnAutoIntegrationTime;
        private Button btnAutoIntegrationTimeBK;
        private Button btnGetConveySpec;
        private Button btnGetConveySpecStop;
        private Button btnSaveDark;
        private Button btnSelectSavePath;
        private Button btnSelectWormSpec;
        private Button btnSetOK;
        private CheckBox cBClearDarks;
        private CheckBox cBCorrectElectricalDarks;
        private CheckBox cBCorrectNonlinearitys;
        private CheckBox cBisEnergy;
        public CheckBox checkBoxAutoStart;
        private ComboBox comboxSerialPort;
        private IContainer components = null;
        private Dictionary<string, double[]> ConveySpec = new Dictionary<string, double[]>();
        private DataHandling DataHandlingmy = new DataHandling();
        private DataIO DataIOS = new DataIO();
        private System.Drawing.Color[] DrawColor = new System.Drawing.Color[5];
        private FolderBrowserDialog folderBrowserDialog1;
        private FrmGetSpec frmGetSpec;
        private Home frmHome;
        private bool GetConveySpecStartFlag = false;
        private GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblTemperature;
        private ListBox listBoxWaveLenth;
        private Spectrometer MSetSpectrometer;
        private ZedGraphControl MyChart;
        private MyChartLoadData myChartLoadData;
        private OpenFileDialog openFileDialog1;
        private Panel panelSetting;
        private CCoSpectralMath spectralMath;
        private Spectrometer.SpecInfo SpInfoTmpt = new Spectrometer.SpecInfo();
        private TextBox textBox1;
        private double ThermoElectricTemperature = 0.0;
        private System.Windows.Forms.Timer timer1;
        private TextBox txtClearSensorIntervalTimes;
        private TextBox txtDesireTemperature;
        private TextBox txtGetBackGrdIntervalTime;
        private TextBox txtIntegrationTime;
        private TextBox txtIntegrationTimeBK;
        private TextBox txtSavePath;
        private TextBox txtScanTimes;
        private TextBox txtThresholdDiff;
        private int[] wavelengthIndex = null;

        public FrmSetting(Spectrometer MSetSpectrometers, FrmGetSpec frmmain, Home home)
        {
            this.InitializeComponent();
            this.MSetSpectrometer = MSetSpectrometers;
            this.frmGetSpec = frmmain;
            this.frmHome = home;
            this.DrawColor[0] = System.Drawing.Color.Red;
            this.DrawColor[1] = System.Drawing.Color.BlueViolet;
            this.DrawColor[2] = System.Drawing.Color.Crimson;
            this.DrawColor[3] = System.Drawing.Color.Chocolate;
            this.DrawColor[4] = System.Drawing.Color.Brown;
            FrmGetSpec.SpInfo = new Spectrometer.SpecInfo();
            this.SpInfoTmpt = Home.SpInfo;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Exception exception;
            Spectrometer.SpecInfo argument = (Spectrometer.SpecInfo) e.Argument;
            try
            {
                this.MSetSpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref argument, false);
            }
            catch (Exception exception1)
            {
                exception = exception1;
                argument.DataA = new double[Home.SpInfo.DataAB.Length];
                argument.DataY = new double[Home.SpInfo.DataAB.Length];
                argument.DataX = new double[Home.SpInfo.DataAB.Length];
                Home.SpInfo.DataX.CopyTo(Home.SpInfo.DataX, 0);
                return;
            }
            try
            {
                this.ThermoElectricTemperature = Spectrometer.tecController.getDetectorTemperatureCelsius();
            }
            catch (Exception exception2)
            {
                exception = exception2;
            }
            if (this.AutoFindIntegrationTimes && (this.SpInfoTmpt.DataA != null))
            {
                string str;
                object[] objArray;
                int num3;
                double num = 0.0;
                int index = this.FindPeaks(this.SpInfoTmpt);
                if (index < this.SpInfoTmpt.DataA.Length)
                {
                    num = this.SpInfoTmpt.DataA[index];
                }
                if (this.AutoFindIntegrationTimeCount > 20)
                {
                    this.AutoFindIntegrationTimes = false;
                    this.AutoFindIntegrationTimes = false;
                    objArray = new object[13];
                    objArray[0] = Spectrometer.IntegrationTime.ToString();
                    objArray[1] = ",";
                    objArray[2] = Spectrometer.ScanTimes.ToString();
                    objArray[3] = ",";
                    objArray[4] = Spectrometer.GainMode;
                    objArray[5] = ",";
                    objArray[6] = Spectrometer.SavePath;
                    objArray[7] = ",";
                    num3 = Spectrometer.isClearDarks ? 1 : 0;
                    objArray[8] = num3.ToString();
                    objArray[9] = ",";
                    num3 = Spectrometer.isCorrectElectricalDarks ? 1 : 0;
                    objArray[10] = num3.ToString();
                    objArray[11] = ",";
                    num3 = Spectrometer.isCorrectNonlinearitys ? 1 : 0;
                    objArray[12] = num3.ToString();
                    str = string.Concat(objArray);
                    this.DataIOS.SaveStr(str, Spectrometer.ApplicationPath + @"\Setting");
                    e.Result = argument;
                    this.AutoFindIntegrationTimess = true;
                }
                else
                {
                    if ((num > 2.0) && ((num > 55000.0) || (num < 54500.0)))
                    {
                        Spectrometer.IntegrationTime = (int) Math.Round((double) (Spectrometer.IntegrationTime * (55000.0 / num)), 0);
                        if (Spectrometer.IntegrationTime > 0xc3500)
                        {
                            Spectrometer.IntegrationTime = 0xc3500;
                        }
                        goto Label_03B0;
                    }
                    this.AutoFindIntegrationTimes = false;
                    objArray = new object[13];
                    objArray[0] = Spectrometer.IntegrationTime.ToString();
                    objArray[1] = ",";
                    objArray[2] = Spectrometer.ScanTimes.ToString();
                    objArray[3] = ",";
                    objArray[4] = Spectrometer.GainMode;
                    objArray[5] = ",";
                    objArray[6] = Spectrometer.SavePath;
                    objArray[7] = ",";
                    num3 = Spectrometer.isClearDarks ? 1 : 0;
                    objArray[8] = num3.ToString();
                    objArray[9] = ",";
                    num3 = Spectrometer.isCorrectElectricalDarks ? 1 : 0;
                    objArray[10] = num3.ToString();
                    objArray[11] = ",";
                    objArray[12] = (Spectrometer.isCorrectNonlinearitys ? 1 : 0).ToString();
                    str = string.Concat(objArray);
                    this.DataIOS.SaveStr(str, Spectrometer.ApplicationPath + @"\Setting");
                    e.Result = argument;
                    this.AutoFindIntegrationTimess = true;
                }
                return;
            }
        Label_03B0:
            e.Result = argument;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.timer1.Enabled)
            {
                this.SpInfoTmpt = (Spectrometer.SpecInfo) e.Result;
                this.DrawEnergyValue();
                this.lblTemperature.Text = this.ThermoElectricTemperature.ToString("0.0") + "℃";
                if (!this.AutoFindIntegrationTimes && this.AutoFindIntegrationTimess)
                {
                    this.btnAutoIntegrationTime.Image = null;
                    this.btnAutoIntegrationTime.Image = Resources.play;
                    this.txtIntegrationTime.Text = Math.Round((double) (((double) Spectrometer.IntegrationTime) / 1000.0), 1).ToString();
                    this.AutoFindIntegrationTimess = false;
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            this.MSetSpectrometer.SetParameters(Spectrometer.spectrometerIndex, Spectrometer.IntegrationTime);
            this.DataIOS.SaveStr((string) e.Argument, Spectrometer.ApplicationPath + @"\Setting");
        }

        private void bkwGetConveySpec_DoWork(object sender, DoWorkEventArgs e)
        {
            while (this.GetConveySpecStartFlag)
            {
                this.MSetSpectrometer.GetSingleBeam(Spectrometer.spectrometerIndex, ref this.SpInfoTmpt, false);
                string key = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
                if (!this.ConveySpec.ContainsKey(key))
                {
                    this.ConveySpec.Add(key, this.SpInfoTmpt.DataA);
                }
                this.DataIOS.TXTSaveData(Spectrometer.SavePath + @"\conveyor" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff") + ".txt", this.SpInfoTmpt.DataX, this.SpInfoTmpt.DataA);
            }
        }

        private void bkwGetConveySpec_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            double[,] x = new double[this.ConveySpec.Count, this.SpInfoTmpt.numPixls];
            int num = 0;
            int num2 = 0;
            foreach (KeyValuePair<string, double[]> pair in this.ConveySpec)
            {
                num++;
                for (int i = 0; i < this.SpInfoTmpt.numPixls; i++)
                {
                    x[num2, i] = pair.Value[i];
                }
                num2++;
                this.DataIOS.TXTSaveData(Spectrometer.SavePath + @"\conveyor" + pair.Key.ToString() + ".txt", this.SpInfoTmpt.DataX, this.SpInfoTmpt.DataA);
            }
            Spectrometer.Data[] dataGet = new Spectrometer.Data[1];
            dataGet[0].DataE = this.DataHandlingmy.SpMean(x);
            dataGet[0].DataX = this.SpInfoTmpt.DataX;
            this.DataIOS.TXTSaveData(Application.StartupPath.ToString() + @"\conveyor.txt", dataGet[0].DataX, dataGet[0].DataE);
            this.DataIOS.TXTSaveData(Spectrometer.SavePath + @"\conveyor.txt", dataGet[0].DataX, dataGet[0].DataE);
            this.MyChart.GraphPane.CurveList.Clear();
            this.Draw("皮带平均能量值", dataGet, 0);
            MessageBox.Show("皮带光谱保存完毕，请选择蚕蛹光谱！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void btnAutoIntegrationTime_Click(object sender, EventArgs e)
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
                return;
            }
            if (this.AutoFindIntegrationTimes && this.txtIntegrationTime.Text.Contains("寻找中"))
            {
                this.backgroundWorker1.CancelAsync();
                this.txtIntegrationTime.Text = Math.Round((double) (((double) Spectrometer.IntegrationTime) / 1000.0), 1).ToString();
                this.AutoFindIntegrationTimes = false;
                this.btnAutoIntegrationTime.BackgroundImage = Resources.play;
            }
            else
            {
                this.AutoFindIntegrationTimes = true;
                this.txtIntegrationTime.Text = "寻找中..";
                this.btnAutoIntegrationTime.BackgroundImage = Resources.stop;
            }
        }

        private void btnAutoIntegrationTime_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip { ShowAlways = true }.SetToolTip(this.btnAutoIntegrationTime, "点击自动设置积分时间");
        }

        private void btnAutoIntegrationTimeBK_Click(object sender, EventArgs e)
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
                buffer = Home.SPControlWord["ReferenceOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                return;
            }
            if (this.AutoFindIntegrationTimes && this.txtIntegrationTime.Text.Contains("寻找中"))
            {
                this.backgroundWorker1.CancelAsync();
                this.txtIntegrationTimeBK.Text = Math.Round((double) (((double) Spectrometer.IntegrationTime) / 1000.0), 1).ToString();
                this.AutoFindIntegrationTimes = false;
                this.btnAutoIntegrationTimeBK.BackgroundImage = Resources.play;
            }
            else
            {
                this.AutoFindIntegrationTimes = true;
                this.txtIntegrationTimeBK.Text = "寻找中..";
                this.btnAutoIntegrationTimeBK.BackgroundImage = Resources.stop;
            }
        }

        private void btnGetConveySpec_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.btnGetConveySpec.Enabled = false;
            this.btnGetConveySpec.BackColor = System.Drawing.Color.FromArgb(0x26, 0x88, 210);
            this.btnGetConveySpecStop.Enabled = true;
            this.GetConveySpecStartFlag = true;
            if (this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.CancelAsync();
            }
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                byte[] buffer = Home.SPControlWord["LightOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
                buffer = Home.SPControlWord["ConveyorOn"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            this.bkwGetConveySpec.RunWorkerAsync();
        }

        private void btnGetConveySpecStop_Click(object sender, EventArgs e)
        {
            this.btnGetConveySpec.Enabled = true;
            this.btnGetConveySpecStop.Enabled = false;
            this.btnGetConveySpec.BackColor = System.Drawing.Color.Transparent;
            this.GetConveySpecStartFlag = false;
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

        private void btnSaveDark_Click(object sender, EventArgs e)
        {
            byte[] buffer;
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                buffer = Home.SPControlWord["ShutterOn"];
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
            if (MessageBox.Show("请确保遮光片遮挡，待下图更新后（能量值会较低）点击“确定”，系统将保存！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                FrmGetSpec.SpInfo.DataAD = this.SpInfoTmpt.DataA;
                this.DataIOS.TXTSaveData(Application.StartupPath.ToString() + @"\dark", this.SpInfoTmpt.DataX, this.SpInfoTmpt.DataA);
            }
            MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            try
            {
                if (!Home.serialPortSetDevice.IsOpen)
                {
                    Home.serialPortSetDevice.Open();
                }
                buffer = Home.SPControlWord["ShutterOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
                buffer = Home.SPControlWord["ReferenceOff"];
                Home.serialPortSetDevice.Write(buffer, 0, buffer.Length);
                Thread.Sleep(500);
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.ToString());
            }
        }

        private void btnSaveDark_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip { ShowAlways = true }.SetToolTip(this.btnSaveDark, "点击保存暗噪声");
        }

        private void btnSelectSavePath_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowNewFolderButton = true;
            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            this.folderBrowserDialog1.Description = "选择保存文件夹";
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtSavePath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSelectWormSpec_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            string path = "";
            this.openFileDialog1.Title = " 打开";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Filter = " txt files(*.txt)|*.txt|All files(*.*)|*.*";
            this.openFileDialog1.CheckFileExists = true;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.openFileDialog1.RestoreDirectory = true;
                path = this.openFileDialog1.FileName;
                int num = 0;
                Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
                Spectrometer.Data[] dataArray2 = new Spectrometer.Data[1];
                num = this.DataIOS.TXTReadData(path, ref dataArray[0].DataX, ref dataArray[0].DataE, true);
                dataArray[0].DataX = new double[num];
                dataArray[0].DataE = new double[num];
                this.DataIOS.TXTReadData(path, ref dataArray[0].DataX, ref dataArray[0].DataE, false);
                if (num == this.DataIOS.TXTReadData(Application.StartupPath.ToString() + @"\conveyor.txt", ref dataArray2[0].DataX, ref dataArray2[0].DataE, true))
                {
                    int num2;
                    double[] numArray;
                    dataArray2[0].DataX = new double[num];
                    dataArray2[0].DataE = new double[num];
                    this.DataIOS.TXTReadData(Application.StartupPath.ToString() + @"\conveyor.txt", ref dataArray2[0].DataX, ref dataArray2[0].DataE, false);
                    Spectrometer.Data[] dataArray3 = new Spectrometer.Data[1];
                    dataArray3[0].DataX = new double[num];
                    dataArray3[0].DataE = new double[num];
                    dataArray3[0].DataX = dataArray2[0].DataX;
                    for (num2 = 0; num2 < num; num2++)
                    {
                        dataArray3[0].DataE[num2] = Math.Abs((double) (dataArray[0].DataE[num2] - dataArray2[0].DataE[num2]));
                    }
                    this.wavelengthIndex = this.DataHandlingmy.MaxValueIndex(dataArray3[0].DataE, 50, out numArray);
                    for (num2 = 0; num2 < this.wavelengthIndex.Length; num2++)
                    {
                        this.listBoxWaveLenth.Items.Add(dataArray3[0].DataX[this.wavelengthIndex[num2]].ToString().Substring(0, 6) + "  " + numArray[num2].ToString().Substring(0, 6));
                    }
                    double num3 = this.DataHandlingmy.StdError(dataArray[0].DataE);
                    double num4 = this.DataHandlingmy.StdError(dataArray2[0].DataE);
                    this.listBoxWaveLenth.Items.Add("蚕蛹：" + num3.ToString("0.000"));
                    this.listBoxWaveLenth.Items.Add("皮带：" + num4.ToString("0.000"));
                    this.txtThresholdDiff.Text = numArray[this.wavelengthIndex.Length - 1].ToString();
                    Spectrometer.Data[] dataGet = new Spectrometer.Data[3];
                    string[] dataName = new string[3];
                    dataGet[0] = dataArray[0];
                    dataName[0] = "蚕蛹能量值";
                    dataGet[1] = dataArray2[0];
                    dataName[1] = "皮带能量值";
                    dataGet[2] = dataArray3[0];
                    dataName[2] = "能量差值";
                    this.MyChart.GraphPane.CurveList.Clear();
                    this.Draw("能量值", dataGet, 2, dataName);
                }
                else
                {
                    MessageBox.Show("光谱维数不符，无法计算！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
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
                        MessageBox.Show("积分时间设置过大，请重新设置！");
                        return;
                    }
                    Spectrometer.IntegrationTime = Convert.ToInt32((double) (Convert.ToDouble(this.txtIntegrationTime.Text) * 1000.0));
                }
                if (this.txtIntegrationTimeBK.Text == "")
                {
                    Spectrometer.IntegrationTimeBK = 0;
                }
                else
                {
                    if (Convert.ToDouble(this.txtIntegrationTimeBK.Text) > 800.0)
                    {
                        MessageBox.Show("积分时间设置过大，请重新设置！");
                        return;
                    }
                    Spectrometer.IntegrationTimeBK = Convert.ToInt32((double) (Convert.ToDouble(this.txtIntegrationTimeBK.Text) * 1000.0));
                }
                Spectrometer.ScanTimes = Convert.ToInt32(this.txtScanTimes.Text);
                Spectrometer.isClearDarks = this.cBClearDarks.Checked;
                Spectrometer.isCorrectElectricalDarks = this.cBCorrectElectricalDarks.Checked;
                Spectrometer.isCorrectNonlinearitys = this.cBCorrectNonlinearitys.Checked;
                FrmGetSpecSet.Default.clearSensorIntervalTimes = int.Parse(this.txtClearSensorIntervalTimes.Text) * 0xea60;
                FrmGetSpecSet.Default.getBlackGrdIntervalTime = double.Parse(this.txtGetBackGrdIntervalTime.Text);
                FrmGetSpecSet.Default.isEnergy = this.cBisEnergy.Checked;
                FrmGetSpecSet.Default.GetSpecAutoIsEnergy = FrmGetSpecSet.Default.isEnergy;
                FrmGetSpecSet.Default.Save();
                this.frmHome.timerClearSensor.Interval = FrmGetSpecSet.Default.clearSensorIntervalTimes;
            }
            catch (Exception exception1)
            {
                MessageBox.Show("输入错误！," + exception1.ToString());
                return;
            }
            try
            {
                Spectrometer.SavePath = this.txtSavePath.Text;
                object[] objArray = new object[] { Spectrometer.IntegrationTime.ToString(), ",", Spectrometer.ScanTimes.ToString(), ",", Spectrometer.GainMode, ",", this.txtSavePath.Text, ",", (Spectrometer.isClearDarks ? 1 : 0).ToString(), ",", (Spectrometer.isCorrectElectricalDarks ? 1 : 0).ToString(), ",", (Spectrometer.isCorrectNonlinearitys ? 1 : 0).ToString() };
                string argument = string.Concat(objArray);
                if (this.backgroundWorker2.IsBusy)
                {
                    this.backgroundWorker2.CancelAsync();
                }
                this.backgroundWorker2.RunWorkerAsync(argument);
                this.timer1.Enabled = false;
                if (this.backgroundWorker1.IsBusy)
                {
                    this.backgroundWorker1.CancelAsync();
                }
                if (this.frmHome != null)
                {
                    FrmGetSpecSet.Default.IntegrationTimeBK = Spectrometer.IntegrationTimeBK;
                }
                if (this.listBoxWaveLenth.SelectedIndices.Count > 0)
                {
                    FrmGetSpecSet.Default.ThresholdDiff = double.Parse(this.txtThresholdDiff.Text);
                    FrmGetSpecSet.Default.WavelengthDiffIndex = "";
                    int num = 0;
                    for (int i = 0; i < this.listBoxWaveLenth.SelectedIndices.Count; i++)
                    {
                        if (i < this.listBoxWaveLenth.Items.Count)
                        {
                            FrmGetSpecSet set1 = FrmGetSpecSet.Default;
                            set1.WavelengthDiffIndex = set1.WavelengthDiffIndex + this.wavelengthIndex[this.listBoxWaveLenth.SelectedIndices[i]].ToString() + ",";
                            num++;
                        }
                    }
                    if (num == 0)
                    {
                        FrmGetSpecSet set2 = FrmGetSpecSet.Default;
                        set2.WavelengthDiffIndex = set2.WavelengthDiffIndex + this.wavelengthIndex[0].ToString() + ",";
                    }
                }
                FrmGetSpecSet.Default.Save();
                FrmGetSpecSet.Default.Save();
            }
            catch (Exception exception2)
            {
                MessageBox.Show("输入错误！," + exception2.ToString());
                return;
            }
            if ((this.frmGetSpec != null) && (this.frmGetSpec != null))
            {
                this.frmGetSpec.timer1.Enabled = true;
            }
        }

        private void cBClearDarks_CheckedChanged(object sender, EventArgs e)
        {
            this.MSetSpectrometer.SetParameters(Spectrometer.spectrometerIndex, Spectrometer.IntegrationTime);
            if (this.cBClearDarks.Checked)
            {
                this.btnSaveDark.Enabled = true;
            }
            else
            {
                this.btnSaveDark.Enabled = false;
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

        private void checkBoxAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            string executablePath;
            RegistryKey localMachine;
            RegistryKey key2;
            if (this.checkBoxAutoStart.Checked)
            {
                executablePath = Application.ExecutablePath;
                localMachine = Registry.LocalMachine;
                key2 = localMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                key2.SetValue("JcShutdown", executablePath);
                key2.Close();
                localMachine.Close();
            }
            else
            {
                executablePath = Application.ExecutablePath;
                localMachine = Registry.LocalMachine;
                key2 = localMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                key2.DeleteValue("JcShutdown", false);
                key2.Close();
                localMachine.Close();
            }
        }

        private void comboxSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.frmHome.comboxSerialPort.SelectedIndex = this.comboxSerialPort.SelectedIndex;
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
                if (DataGet[Num].DataE != null)
                {
                    RectangleF rect = this.MyChart.GraphPane.Rect;
                    this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "能量");
                    this.MyChart.GraphPane.CurveList.Clear();
                    for (int i = 0; i < (Num + 1); i++)
                    {
                        this.MyChart_LoadData(DataGet[i].DataX, DataGet[i].DataE, this.DrawColor[i], "Energy");
                    }
                }
            }
            else
            {
                this.MyChart.GraphPane.CurveList.Clear();
            }
        }

        private void Draw(string str, Spectrometer.Data[] DataGet, int Num, string[] DataName)
        {
            if ((DataGet != null) && (DataGet[Num].DataX != null))
            {
                if (DataGet[Num].DataE != null)
                {
                    RectangleF rect = this.MyChart.GraphPane.Rect;
                    this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "能量");
                    this.MyChart.GraphPane.CurveList.Clear();
                    for (int i = 0; i < (Num + 1); i++)
                    {
                        this.MyChart_LoadData(DataGet[i].DataX, DataGet[i].DataE, this.DrawColor[i], DataName[i]);
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
            for (int i = 0; i < this.SpInfoTmpt.numPixls; i++)
            {
                if (this.SpInfoTmpt.WavelengthArray[i] > 10000000000)
                {
                    this.SpInfoTmpt.WavelengthArray[i] = this.SpInfoTmpt.WavelengthArray[i - 1];
                }
                if (this.SpInfoTmpt.DataA[i] > 10000000000)
                {
                    this.SpInfoTmpt.DataA[i] = this.SpInfoTmpt.DataA[i - 1];
                }
            }
            this.SpInfoTmpt.DataX = this.SpInfoTmpt.WavelengthArray;
            this.SpInfoTmpt.w1 = Math.Floor((double) (this.SpInfoTmpt.DataX[0] / 100.0)) * 100.0;
            this.SpInfoTmpt.w2 = Math.Ceiling((double) (this.SpInfoTmpt.DataX[this.SpInfoTmpt.DataX.Length - 1] / 100.0)) * 100.0;
            Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
            dataArray[0].DataX = this.SpInfoTmpt.WavelengthArray;
            dataArray[0].DataE = this.SpInfoTmpt.DataA;
            DrawDelegate method = new DrawDelegate(this.Draw);
            base.BeginInvoke(method, new object[] { "能量图", dataArray, 0 });
        }

        private int FindPeaks(Spectrometer.SpecInfo SpInfo)
        {
            return this.DataHandlingmy.MaxValueIndex(SpInfo.DataA);
        }

        private void FrmSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.btnSetOK_Click(sender, e);
            this.frmHome.comboxSerialPort.SelectedIndex = this.comboxSerialPort.SelectedIndex;
            if (this.timer1.Enabled)
            {
                this.timer1.Enabled = false;
                if (this.backgroundWorker1.IsBusy)
                {
                    this.backgroundWorker1.CancelAsync();
                }
                base.Close();
                if ((this.frmGetSpec != null) && (this.frmGetSpec != null))
                {
                    this.frmGetSpec.timer1.Enabled = true;
                }
            }
        }

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.MSetSpectrometer.wrapper.openAllSpectrometers() <= 0)
                {
                    MessageBox.Show("光谱仪未连接");
                    this.frmHome.panelConnectState.Visible = false;
                    this.frmHome.lblConnectState.Text = "未连接";
                    return;
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show("光谱仪未打开错误");
            }
            this.checkBoxAutoStart.Checked = true;
            if (Spectrometer.IntegrationTime != 0)
            {
                int num2 = Spectrometer.IntegrationTime / 0x3e8;
                this.txtIntegrationTime.Text = num2.ToString();
            }
            if (Spectrometer.ScanTimes != 0)
            {
                this.txtScanTimes.Text = Spectrometer.ScanTimes.ToString();
            }
            if (Spectrometer.SavePath != null)
            {
                this.txtSavePath.Text = Spectrometer.SavePath;
            }
            else
            {
                this.txtSavePath.Text = Spectrometer.ApplicationPath;
            }
            if (Spectrometer.isClearDarks)
            {
                this.cBClearDarks.Checked = true;
            }
            if (Spectrometer.isCorrectElectricalDarks)
            {
                this.cBCorrectElectricalDarks.Checked = true;
            }
            if (Spectrometer.isCorrectNonlinearitys)
            {
                this.cBCorrectNonlinearitys.Checked = true;
            }
            if (FrmGetSpecSet.Default.isEnergy)
            {
                this.cBisEnergy.Checked = true;
            }
            else
            {
                this.cBisEnergy.Checked = false;
            }
            if (Spectrometer.IntegrationTimeBK != 0)
            {
                this.txtIntegrationTimeBK.Text = (Spectrometer.IntegrationTimeBK / 0x3e8).ToString();
            }
            else
            {
                this.txtIntegrationTimeBK.Text = this.txtIntegrationTime.Text;
            }
            this.txtClearSensorIntervalTimes.Text = (FrmGetSpecSet.Default.clearSensorIntervalTimes / 0xea60).ToString();
            this.txtGetBackGrdIntervalTime.Text = FrmGetSpecSet.Default.getBlackGrdIntervalTime.ToString("0.0");
            for (int i = 0; i < this.frmHome.comboxSerialPort.Items.Count; i++)
            {
                this.comboxSerialPort.Items.Add(this.frmHome.comboxSerialPort.Items[i]);
            }
            this.comboxSerialPort.SelectedIndex = this.frmHome.comboxSerialPort.SelectedIndex;
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
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.ToString());
            }
            if (this.frmHome.btnGetSpectrumAuto.Visible)
            {
                this.panelSetting.Location = new Point(10, 0x41);
                this.groupBox1.Visible = true;
                this.groupBox1.Location = new Point((this.panelSetting.Location.X + this.panelSetting.Size.Width) + 10, this.panelSetting.Location.Y);
            }
            else
            {
                this.groupBox1.Visible = false;
                this.panelSetting.Location = new Point((base.Size.Width / 2) - (this.panelSetting.Size.Width / 2), this.panelSetting.Location.Y);
            }
            if (Spectrometer.isSupportThermoElectric)
            {
                this.lblTemperature.Visible = true;
            }
            this.timer1.Enabled = true;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmSetting));
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
            this.txtIntegrationTimeBK = new TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new GroupBox();
            this.checkBoxAutoStart = new CheckBox();
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
            this.btnAutoIntegrationTimeBK = new Button();
            this.btnSaveDark = new Button();
            this.btnAutoIntegrationTime = new Button();
            this.panelSetting = new Panel();
            this.txtGetBackGrdIntervalTime = new TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboxSerialPort = new ComboBox();
            this.lblTemperature = new System.Windows.Forms.Label();
            this.txtDesireTemperature = new TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cBisEnergy = new CheckBox();
            this.groupBox1.SuspendLayout();
            this.panelSetting.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label1.Location = new Point(15, 11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x7d, 0x13);
            this.label1.TabIndex = 0;
            this.label1.Text = "积分时间(ms)";
            this.txtIntegrationTime.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.txtIntegrationTime.Location = new Point(0x89, 5);
            this.txtIntegrationTime.Name = "txtIntegrationTime";
            this.txtIntegrationTime.Size = new Size(0x44, 0x1d);
            this.txtIntegrationTime.TabIndex = 1;
            this.txtScanTimes.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.txtScanTimes.Location = new Point(340, 0x2c);
            this.txtScanTimes.Name = "txtScanTimes";
            this.txtScanTimes.Size = new Size(0x2a, 0x1d);
            this.txtScanTimes.TabIndex = 3;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label2.Location = new Point(0x102, 0x31);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x55, 0x13);
            this.label2.TabIndex = 2;
            this.label2.Text = "扫描次数";
            this.btnSetOK.Location = new Point(0x1af, 12);
            this.btnSetOK.Name = "btnSetOK";
            this.btnSetOK.Size = new Size(0x2d, 0x25);
            this.btnSetOK.TabIndex = 6;
            this.btnSetOK.Text = "确定";
            this.btnSetOK.UseVisualStyleBackColor = true;
            this.btnSetOK.Visible = false;
            this.btnSetOK.Click += new EventHandler(this.btnSetOK_Click);
            this.label5.AutoSize = true;
            this.label5.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label5.Location = new Point(14, 0x7d);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x55, 0x13);
            this.label5.TabIndex = 9;
            this.label5.Text = "保存路径";
            this.txtSavePath.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.txtSavePath.Location = new Point(0x69, 0x7a);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new Size(0x110, 0x1d);
            this.txtSavePath.TabIndex = 10;
            this.btnSelectSavePath.Location = new Point(0x17f, 0x77);
            this.btnSelectSavePath.Name = "btnSelectSavePath";
            this.btnSelectSavePath.Size = new Size(0x2e, 0x25);
            this.btnSelectSavePath.TabIndex = 11;
            this.btnSelectSavePath.Text = "浏览";
            this.btnSelectSavePath.UseVisualStyleBackColor = true;
            this.btnSelectSavePath.Click += new EventHandler(this.btnSelectSavePath_Click);
            this.cBClearDarks.AutoSize = true;
            this.cBClearDarks.Font = new Font("宋体", 14.25f);
            this.cBClearDarks.Location = new Point(0x12, 0x55);
            this.cBClearDarks.Margin = new Padding(2);
            this.cBClearDarks.Name = "cBClearDarks";
            this.cBClearDarks.Size = new Size(0x7b, 0x17);
            this.cBClearDarks.TabIndex = 12;
            this.cBClearDarks.Text = "去除暗噪声";
            this.cBClearDarks.UseVisualStyleBackColor = true;
            this.cBClearDarks.CheckedChanged += new EventHandler(this.cBClearDarks_CheckedChanged);
            this.cBCorrectElectricalDarks.AutoSize = true;
            this.cBCorrectElectricalDarks.Font = new Font("宋体", 14.25f);
            this.cBCorrectElectricalDarks.Location = new Point(0xab, 0x54);
            this.cBCorrectElectricalDarks.Margin = new Padding(2);
            this.cBCorrectElectricalDarks.Name = "cBCorrectElectricalDarks";
            this.cBCorrectElectricalDarks.Size = new Size(0x7b, 0x17);
            this.cBCorrectElectricalDarks.TabIndex = 13;
            this.cBCorrectElectricalDarks.Text = "杂散光矫正";
            this.cBCorrectElectricalDarks.UseVisualStyleBackColor = true;
            this.cBCorrectElectricalDarks.CheckedChanged += new EventHandler(this.cBCorrectElectricalDarks_CheckedChanged);
            this.cBCorrectNonlinearitys.AutoSize = true;
            this.cBCorrectNonlinearitys.Font = new Font("宋体", 14.25f);
            this.cBCorrectNonlinearitys.Location = new Point(0x12a, 0x53);
            this.cBCorrectNonlinearitys.Margin = new Padding(2);
            this.cBCorrectNonlinearitys.Name = "cBCorrectNonlinearitys";
            this.cBCorrectNonlinearitys.Size = new Size(0x7b, 0x17);
            this.cBCorrectNonlinearitys.TabIndex = 14;
            this.cBCorrectNonlinearitys.Text = "非线性校正";
            this.cBCorrectNonlinearitys.UseVisualStyleBackColor = true;
            this.cBCorrectNonlinearitys.CheckedChanged += new EventHandler(this.cBCorrectNonlinearitys_CheckedChanged);
            this.MyChart.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.MyChart.Location = new Point(0x13, 0x9f);
            this.MyChart.Margin = new Padding(4, 3, 4, 3);
            this.MyChart.Name = "MyChart";
            this.MyChart.ScrollGrace = 0.0;
            this.MyChart.ScrollMaxX = 0.0;
            this.MyChart.ScrollMaxY = 0.0;
            this.MyChart.ScrollMaxY2 = 0.0;
            this.MyChart.ScrollMinX = 0.0;
            this.MyChart.ScrollMinY = 0.0;
            this.MyChart.ScrollMinY2 = 0.0;
            this.MyChart.Size = new Size(0x215, 360);
            this.MyChart.TabIndex = 15;
            this.timer1.Interval = 0x3e8;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.txtIntegrationTimeBK.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.txtIntegrationTimeBK.Location = new Point(0xab, 0x2e);
            this.txtIntegrationTimeBK.Name = "txtIntegrationTimeBK";
            this.txtIntegrationTimeBK.Size = new Size(0x2f, 0x1d);
            this.txtIntegrationTimeBK.TabIndex = 0x1f;
            this.label3.AutoSize = true;
            this.label3.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label3.Location = new Point(15, 0x31);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0xa3, 0x13);
            this.label3.TabIndex = 30;
            this.label3.Text = "背景积分时间(ms)";
            this.groupBox1.Controls.Add(this.checkBoxAutoStart);
            this.groupBox1.Controls.Add(this.txtThresholdDiff);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.listBoxWaveLenth);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnSelectWormSpec);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnGetConveySpecStop);
            this.groupBox1.Controls.Add(this.btnGetConveySpec);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Font = new Font("宋体", 14.25f);
            this.groupBox1.Location = new Point(590, 0x41);
            this.groupBox1.Margin = new Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new Padding(2);
            this.groupBox1.Size = new Size(0x113, 0x20d);
            this.groupBox1.TabIndex = 0x21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "蚕蛹光谱辨别与相关参数设置";
            this.groupBox1.Visible = false;
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Font = new Font("宋体", 14.25f);
            this.checkBoxAutoStart.Location = new Point(0x2b, 0x1f3);
            this.checkBoxAutoStart.Margin = new Padding(2);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new Size(0x7b, 0x17);
            this.checkBoxAutoStart.TabIndex = 0x25;
            this.checkBoxAutoStart.Text = "开机自启动";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            this.checkBoxAutoStart.Visible = false;
            this.checkBoxAutoStart.CheckedChanged += new EventHandler(this.checkBoxAutoStart_CheckedChanged);
            this.txtThresholdDiff.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.txtThresholdDiff.Location = new Point(0x5b, 0x1c9);
            this.txtThresholdDiff.Name = "txtThresholdDiff";
            this.txtThresholdDiff.Size = new Size(0x7c, 0x1d);
            this.txtThresholdDiff.TabIndex = 0x33;
            this.label7.Location = new Point(12, 0xc0);
            this.label7.Margin = new Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0xa3, 0x16);
            this.label7.TabIndex = 0x31;
            this.label7.Text = "推荐的波长点：";
            this.listBoxWaveLenth.FormattingEnabled = true;
            this.listBoxWaveLenth.HorizontalScrollbar = true;
            this.listBoxWaveLenth.ItemHeight = 0x13;
            this.listBoxWaveLenth.Location = new Point(15, 0xdf);
            this.listBoxWaveLenth.Margin = new Padding(2);
            this.listBoxWaveLenth.Name = "listBoxWaveLenth";
            this.listBoxWaveLenth.SelectionMode = SelectionMode.MultiSimple;
            this.listBoxWaveLenth.Size = new Size(0xd4, 0xd5);
            this.listBoxWaveLenth.TabIndex = 0x30;
            this.label6.Location = new Point(11, 0x7d);
            this.label6.Margin = new Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0xa3, 0x16);
            this.label6.TabIndex = 0x2f;
            this.label6.Text = "需对比的蚕蛹光谱";
            this.btnSelectWormSpec.FlatStyle = FlatStyle.Flat;
            this.btnSelectWormSpec.Font = new Font("宋体", 12f);
            this.btnSelectWormSpec.Location = new Point(0xc4, 0x95);
            this.btnSelectWormSpec.Name = "btnSelectWormSpec";
            this.btnSelectWormSpec.Size = new Size(0x33, 0x22);
            this.btnSelectWormSpec.TabIndex = 0x2e;
            this.btnSelectWormSpec.Text = "选择";
            this.btnSelectWormSpec.UseVisualStyleBackColor = true;
            this.btnSelectWormSpec.Click += new EventHandler(this.btnSelectWormSpec_Click);
            this.textBox1.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBox1.Location = new Point(14, 0x99);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0xb0, 0x1d);
            this.textBox1.TabIndex = 0x2d;
            this.label4.Location = new Point(12, 0x24);
            this.label4.Margin = new Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x40, 0x41);
            this.label4.TabIndex = 0x2c;
            this.label4.Text = "采集皮带能量";
            this.btnGetConveySpecStop.BackColor = System.Drawing.Color.Transparent;
//            this.btnGetConveySpecStop.BackgroundImage = Resources.Pause;
            this.btnGetConveySpecStop.BackgroundImageLayout = ImageLayout.Stretch;
            this.btnGetConveySpecStop.Enabled = false;
            this.btnGetConveySpecStop.FlatStyle = FlatStyle.Flat;
            this.btnGetConveySpecStop.ForeColor = System.Drawing.Color.Transparent;
            this.btnGetConveySpecStop.Location = new Point(0xa9, 0x25);
            this.btnGetConveySpecStop.Margin = new Padding(0);
            this.btnGetConveySpecStop.Name = "btnGetConveySpecStop";
            this.btnGetConveySpecStop.Size = new Size(0x3a, 0x40);
            this.btnGetConveySpecStop.TabIndex = 0x2b;
            this.btnGetConveySpecStop.UseVisualStyleBackColor = false;
            this.btnGetConveySpecStop.Click += new EventHandler(this.btnGetConveySpecStop_Click);
            this.btnGetConveySpec.Anchor = AnchorStyles.Right;
            this.btnGetConveySpec.BackColor = System.Drawing.Color.Transparent;
            //this.btnGetConveySpec.BackgroundImage = Resources.Play2;
            this.btnGetConveySpec.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnGetConveySpec.FlatStyle = FlatStyle.Flat;
            this.btnGetConveySpec.ForeColor = System.Drawing.Color.Transparent;
            this.btnGetConveySpec.Location = new Point(0x5b, 40);
            this.btnGetConveySpec.Margin = new Padding(4);
            this.btnGetConveySpec.Name = "btnGetConveySpec";
            this.btnGetConveySpec.Size = new Size(0x3e, 0x40);
            this.btnGetConveySpec.TabIndex = 40;
            this.btnGetConveySpec.UseVisualStyleBackColor = false;
            this.btnGetConveySpec.Click += new EventHandler(this.btnGetConveySpec_Click);
            this.label8.Location = new Point(0x1d, 460);
            this.label8.Margin = new Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new Size(100, 0x16);
            this.label8.TabIndex = 50;
            this.label8.Text = "阈值：";
            this.bkwGetConveySpec.WorkerSupportsCancellation = true;
            this.bkwGetConveySpec.DoWork += new DoWorkEventHandler(this.bkwGetConveySpec_DoWork);
            this.bkwGetConveySpec.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bkwGetConveySpec_RunWorkerCompleted);
            this.openFileDialog1.FileName = "openFileDialog1";
            this.txtClearSensorIntervalTimes.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.txtClearSensorIntervalTimes.Location = new Point(0x18b, 6);
            this.txtClearSensorIntervalTimes.Name = "txtClearSensorIntervalTimes";
            this.txtClearSensorIntervalTimes.Size = new Size(0x2b, 0x1d);
            this.txtClearSensorIntervalTimes.TabIndex = 0x23;
            this.label9.AutoSize = true;
            this.label9.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label9.Location = new Point(0xec, 12);
            this.label9.Name = "label9";
            this.label9.Size = new Size(230, 0x13);
            this.label9.TabIndex = 0x22;
            this.label9.Text = "探头清洗间隔时间     分";
            //this.btnAutoIntegrationTimeBK.BackgroundImage = Resources.play;
            this.btnAutoIntegrationTimeBK.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnAutoIntegrationTimeBK.Location = new Point(0xdb, 0x2c);
            this.btnAutoIntegrationTimeBK.Margin = new Padding(2);
            this.btnAutoIntegrationTimeBK.Name = "btnAutoIntegrationTimeBK";
            this.btnAutoIntegrationTimeBK.Size = new Size(30, 0x1d);
            this.btnAutoIntegrationTimeBK.TabIndex = 0x20;
            this.btnAutoIntegrationTimeBK.UseVisualStyleBackColor = true;
            this.btnAutoIntegrationTimeBK.Click += new EventHandler(this.btnAutoIntegrationTimeBK_Click);
            //this.btnSaveDark.BackgroundImage = Resources.save;
            this.btnSaveDark.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnSaveDark.Location = new Point(0x90, 0x54);
            this.btnSaveDark.Margin = new Padding(2);
            this.btnSaveDark.Name = "btnSaveDark";
            this.btnSaveDark.Size = new Size(0x16, 0x18);
            this.btnSaveDark.TabIndex = 0x11;
            this.btnSaveDark.UseVisualStyleBackColor = true;
            this.btnSaveDark.Click += new EventHandler(this.btnSaveDark_Click);
            this.btnSaveDark.MouseEnter += new EventHandler(this.btnSaveDark_MouseEnter);
            //this.btnAutoIntegrationTime.BackgroundImage = Resources.play;
            this.btnAutoIntegrationTime.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnAutoIntegrationTime.Location = new Point(210, 6);
            this.btnAutoIntegrationTime.Margin = new Padding(2);
            this.btnAutoIntegrationTime.Name = "btnAutoIntegrationTime";
            this.btnAutoIntegrationTime.Size = new Size(0x1f, 0x1d);
            this.btnAutoIntegrationTime.TabIndex = 0x10;
            this.btnAutoIntegrationTime.UseVisualStyleBackColor = true;
            this.btnAutoIntegrationTime.Click += new EventHandler(this.btnAutoIntegrationTime_Click);
            this.btnAutoIntegrationTime.MouseEnter += new EventHandler(this.btnAutoIntegrationTime_MouseEnter);
            this.panelSetting.Controls.Add(this.cBisEnergy);
            this.panelSetting.Controls.Add(this.txtGetBackGrdIntervalTime);
            this.panelSetting.Controls.Add(this.label11);
            this.panelSetting.Controls.Add(this.comboxSerialPort);
            this.panelSetting.Controls.Add(this.txtIntegrationTime);
            this.panelSetting.Controls.Add(this.txtScanTimes);
            this.panelSetting.Controls.Add(this.btnAutoIntegrationTimeBK);
            this.panelSetting.Controls.Add(this.txtIntegrationTimeBK);
            this.panelSetting.Controls.Add(this.label5);
            this.panelSetting.Controls.Add(this.label3);
            this.panelSetting.Controls.Add(this.txtSavePath);
            this.panelSetting.Controls.Add(this.btnSaveDark);
            this.panelSetting.Controls.Add(this.btnSelectSavePath);
            this.panelSetting.Controls.Add(this.btnAutoIntegrationTime);
            this.panelSetting.Controls.Add(this.cBClearDarks);
            this.panelSetting.Controls.Add(this.MyChart);
            this.panelSetting.Controls.Add(this.cBCorrectElectricalDarks);
            this.panelSetting.Controls.Add(this.cBCorrectNonlinearitys);
            this.panelSetting.Controls.Add(this.lblTemperature);
            this.panelSetting.Controls.Add(this.txtClearSensorIntervalTimes);
            this.panelSetting.Controls.Add(this.label9);
            this.panelSetting.Controls.Add(this.label1);
            this.panelSetting.Controls.Add(this.label2);
            this.panelSetting.Controls.Add(this.txtDesireTemperature);
            this.panelSetting.Controls.Add(this.label10);
            this.panelSetting.Location = new Point(10, 0x41);
            this.panelSetting.Name = "panelSetting";
            this.panelSetting.Size = new Size(570, 0x20d);
            this.panelSetting.TabIndex = 0x24;
            this.txtGetBackGrdIntervalTime.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.txtGetBackGrdIntervalTime.Location = new Point(0x1f7, 0x4e);
            this.txtGetBackGrdIntervalTime.Name = "txtGetBackGrdIntervalTime";
            this.txtGetBackGrdIntervalTime.Size = new Size(0x2a, 0x1d);
            this.txtGetBackGrdIntervalTime.TabIndex = 0x59;
            this.label11.AutoSize = true;
            this.label11.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label11.Location = new Point(0x1a5, 0x53);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x91, 0x13);
            this.label11.TabIndex = 0x58;
            this.label11.Text = "背景间隔     h";
            this.comboxSerialPort.Font = new Font("微软雅黑", 13.8f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.comboxSerialPort.FormattingEnabled = true;
            this.comboxSerialPort.Location = new Point(0x1d8, 6);
            this.comboxSerialPort.Margin = new Padding(2);
            this.comboxSerialPort.Name = "comboxSerialPort";
            this.comboxSerialPort.Size = new Size(80, 0x20);
            this.comboxSerialPort.TabIndex = 0x43;
            this.comboxSerialPort.SelectedIndexChanged += new EventHandler(this.comboxSerialPort_SelectedIndexChanged);
            this.lblTemperature.AutoSize = true;
            this.lblTemperature.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.lblTemperature.Location = new Point(0x1d5, 0x55);
            this.lblTemperature.Name = "lblTemperature";
            this.lblTemperature.Size = new Size(0, 0x15);
            this.lblTemperature.TabIndex = 0x4a;
            this.lblTemperature.Visible = false;
            this.txtDesireTemperature.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.txtDesireTemperature.Location = new Point(500, 0x2e);
            this.txtDesireTemperature.Name = "txtDesireTemperature";
            this.txtDesireTemperature.Size = new Size(0x3f, 0x1d);
            this.txtDesireTemperature.TabIndex = 0x57;
            this.txtDesireTemperature.Leave += new EventHandler(this.txtDesireTemperature_Leave);
            this.label10.AutoSize = true;
            this.label10.Font = new Font("宋体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label10.Location = new Point(0x17e, 50);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x7c, 0x13);
            this.label10.TabIndex = 0x56;
            this.label10.Text = "设定温度(℃)";
            this.cBisEnergy.AutoSize = true;
            this.cBisEnergy.Font = new Font("宋体", 14.25f);
            this.cBisEnergy.Location = new Point(440, 0x7c);
            this.cBisEnergy.Margin = new Padding(2);
            this.cBisEnergy.Name = "cBisEnergy";
            this.cBisEnergy.Size = new Size(0x7b, 0x17);
            this.cBisEnergy.TabIndex = 90;
            this.cBisEnergy.Text = "系统能量值";
            this.cBisEnergy.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            base.ClientSize = new Size(0x375, 0x266);
            base.Controls.Add(this.panelSetting);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.btnSetOK);
            base.FormBorderStyle = FormBorderStyle.None;
            //base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FrmSetting";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "设置";
            base.FormClosing += new FormClosingEventHandler(this.FrmSetting_FormClosing);
            base.Load += new EventHandler(this.FrmSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelSetting.ResumeLayout(false);
            this.panelSetting.PerformLayout();
            base.ResumeLayout(false);
        }

        private void MyChart_LoadData(double[] DataX, double[] DataY, System.Drawing.Color ColorMy, string Name)
        {
            PointPairList points = new PointPairList(DataX, DataY);
            this.MyChart.GraphPane.AddCurve(Name, points, ColorMy, SymbolType.None);
            this.MyChart.GraphPane.XAxis.Scale.Min = this.DataHandlingmy.MinValue(DataX);
            this.MyChart.GraphPane.XAxis.Scale.Max = this.DataHandlingmy.MaxValue(DataX);
            this.MyChart.AxisChange();
            this.MyChart.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Spectrometer.IntegrationTime > 0x7a120)
            {
                this.timer1.Interval = 0xbb8;
            }
            if (!this.backgroundWorker1.IsBusy)
            {
                if (this.AutoFindIntegrationTimes)
                {
                    this.txtIntegrationTime.Text = this.txtIntegrationTime.Text + ".";
                    if (this.txtIntegrationTime.Text.Contains("...."))
                    {
                        this.txtIntegrationTime.Text = "寻找中";
                    }
                    this.AutoFindIntegrationTimeCount++;
                }
                this.backgroundWorker1.RunWorkerAsync(this.SpInfoTmpt);
            }
        }

        private void txtDesireTemperature_Leave(object sender, EventArgs e)
        {
            double num = 0.0;
            try
            {
                num = double.Parse(this.txtDesireTemperature.Text);
            }
            catch
            {
                MessageBox.Show("输入有误!");
            }
            if ((num > 0.0) || (num < -20.0))
            {
                MessageBox.Show("请将温度设置于-20℃到0℃之间！");
            }
            else
            {
                FrmGetSpecSet.Default.thermoElectricTemperature = num;
                Spectrometer.tecController.setDetectorSetPointCelsius(FrmGetSpecSet.Default.thermoElectricTemperature);
            }
        }

        private delegate void DrawDelegate(string Str, Spectrometer.Data[] DataGet, int Num);

        private delegate void GetThermitorDel();

        private delegate void MyChartLoadData(double[] DataX, double[] DataY, System.Drawing.Color ColorMy, string Name);
    }
}

