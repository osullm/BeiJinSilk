namespace JSDU
{
    using NIRQUEST;
    using OmniDriver;
    using SPAM;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Spectrometer
    {
        public ProgressBar _ProgressBar;
        public static string ApplicationPath;
        public static Data[] DataGet;
        private DataHandling DataHandlingmy = new DataHandling();
        private DataIO DataIOmy = new DataIO();
        public const byte FT_PURGE_RX = 1;
        public const byte FT_PURGE_TX = 2;
        public static int GainMode;
        public static int IntegrationTime;
        public static int IntegrationTimeBK;
        public static bool isClearDarks = false;
        public static bool isCorrectElectricalDarks = false;
        public static bool isCorrectNonlinearitys = false;
        public static bool isSupportThermoElectric;
        public static string SavePath;
        public static int ScanTimes;
        public CCoSpectralMath spectralMath = new CCoSpectralMath();
        public static int spectrometerIndex = -1;
        public static CCoThermoElectric tecController;
        public CCoWrapper wrapper = new CCoWrapper();

        public bool GetSingleBeam(int spectrometerIndex, ref SpecInfo SpInfo, bool isBKGrdFlag)
        {
            if (isBKGrdFlag)
            {
                this.SetParameters(Spectrometer.spectrometerIndex, IntegrationTimeBK);
            }
            else
            {
                this.SetParameters(Spectrometer.spectrometerIndex, IntegrationTime);
            }
            SpInfo.numPixls = this.wrapper.getNumberOfPixels(spectrometerIndex);
            try
            {
                SpInfo.WavelengthArray = this.wrapper.getWavelengths(spectrometerIndex);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            string str = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
            SpInfo.DataA = new double[SpInfo.numPixls];
            SpInfo.DataA = this.wrapper.getSpectrum(spectrometerIndex);
            string str2 = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss-fff");
            if ((SpInfo.WavelengthArray.Length > 10) && (SpInfo.DataA.Length > 10))
            {
                SpInfo.DataX = SpInfo.WavelengthArray;
                return true;
            }
            return false;
        }

        public void GetSingleBeamOnly(int spectrometerIndex, ref SpecInfo SpInfo)
        {
            for (int i = 0; i < SpInfo.DataA.Length; i++)
            {
                SpInfo.DataA[i] = 0.0;
            }
            this.wrapper.setIntegrationTime(spectrometerIndex, IntegrationTime);
            SpInfo.DataA = this.wrapper.getSpectrum(spectrometerIndex);
        }

        public bool GetSpec(int spectrometerIndex, ref SpecInfo SpInfo, int WaitTime, BackgroundWorker bk)
        {
            int num;
            this.SetParameters(Spectrometer.spectrometerIndex, IntegrationTime);
            SpInfo.numPixls = this.wrapper.getNumberOfPixels(spectrometerIndex);
            SpInfo.WavelengthArray = this.wrapper.getWavelengths(spectrometerIndex);
            this.wrapper.setScansToAverage(spectrometerIndex, ScanTimes);
            for (num = 0; num < 5; num++)
            {
                Thread.Sleep(0x3e8);
                bk.ReportProgress(num);
            }
            SpInfo.DataA = this.wrapper.getSpectrum(spectrometerIndex);
            bk.ReportProgress(5);
            if ((SpInfo.WavelengthArray.Length > 10) && (SpInfo.DataA.Length > 10))
            {
                SpInfo.DataX = SpInfo.WavelengthArray;
                if (SpInfo.DataAB.Length == SpInfo.DataA.Length)
                {
                    SpInfo.DataY = new double[SpInfo.numPixls];
                    for (num = 0; num < SpInfo.DataA.Length; num++)
                    {
                        try
                        {
                            if (isClearDarks)
                            {
                                SpInfo.DataY[num] = Convert.ToDouble(Math.Log10(Math.Abs((double) ((SpInfo.DataAB[num] - SpInfo.DataAD[num]) / (SpInfo.DataA[num] - SpInfo.DataAD[num])))));
                            }
                            else
                            {
                                SpInfo.DataY[num] = Convert.ToDouble(Math.Log10(Math.Abs((double) (SpInfo.DataAB[num] / SpInfo.DataA[num]))));
                            }
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
                    return true;
                }
                return false;
            }
            return false;
        }

        public void InitiateSpectrometer(Home frmHome)
        {
            if (frmHome.numberOfSpectrometersFound > 0)
            {
                this.wrapper.setAutoToggleStrobeLampEnable(spectrometerIndex, 1);
                if (this.wrapper.isFeatureSupportedThermoElectric(spectrometerIndex) > 0)
                {
                    isSupportThermoElectric = true;
                    tecController = this.wrapper.getFeatureControllerThermoElectric(spectrometerIndex);
                    tecController.setTECEnable(1);
                    tecController.setFanEnable(1);
                    tecController.setDetectorSetPointCelsius(FrmGetSpecSet.Default.thermoElectricTemperature);
                }
            }
        }

        public void ReadBK(ref SpecInfo SpInfo)
        {
            if (File.Exists(Application.StartupPath + @"\background"))
            {
                int num = this.DataIOmy.TXTReadData(Application.StartupPath + @"\background", ref SpInfo.DataX, ref SpInfo.DataAB, true);
                SpInfo.DataX = new double[num];
                SpInfo.DataAB = new double[num];
                this.DataIOmy.TXTReadData(Application.StartupPath.ToString() + @"\background", ref SpInfo.DataX, ref SpInfo.DataAB, false);
            }
            else
            {
                MessageBox.Show("背景文件不存在，请先采集背景！");
            }
        }

        public void ReadDK(ref SpecInfo SpInfo)
        {
            if (File.Exists(Application.StartupPath.ToString() + @"\dark"))
            {
                int num = this.DataIOmy.TXTReadData(Application.StartupPath.ToString() + @"\dark", ref SpInfo.DataX, ref SpInfo.DataAD, true);
                SpInfo.DataAD = new double[num];
                this.DataIOmy.TXTReadData(Application.StartupPath.ToString() + @"\dark", ref SpInfo.DataX, ref SpInfo.DataAD, false);
            }
            else
            {
                MessageBox.Show("背景文件不存在，请先采集背景！");
            }
        }

        public void ReadSetParameters()
        {
            if (File.Exists(ApplicationPath + @"\Setting"))
            {
                string[] strArray;
                this.DataIOmy.ReadLineTXT(ApplicationPath + @"\Setting", out strArray);
                if (strArray.Length > 0)
                {
                    string[] strArray2 = strArray[0].Split(new char[] { ',' });
                    string text = "";
                    IntegrationTime = Convert.ToInt32(strArray2[0]);
                    if (IntegrationTime > 0xc3500)
                    {
                        IntegrationTime = 0xc3500;
                    }
                    ScanTimes = Convert.ToInt32(strArray2[1]);
                    GainMode = Convert.ToInt32(strArray2[2]);
                    SavePath = strArray2[3];
                    if (!Directory.Exists(SavePath))
                    {
                        SavePath = ApplicationPath;
                    }
                    if (Convert.ToInt32(strArray2[4]) == 1)
                    {
                        isClearDarks = true;
                    }
                    else
                    {
                        isClearDarks = false;
                    }
                    if (Convert.ToInt32(strArray2[5]) == 1)
                    {
                        isCorrectElectricalDarks = true;
                    }
                    else
                    {
                        isCorrectElectricalDarks = false;
                    }
                    if (Convert.ToInt32(strArray2[6]) == 1)
                    {
                        isCorrectNonlinearitys = true;
                    }
                    else
                    {
                        isCorrectNonlinearitys = false;
                    }
                    if (text != "")
                    {
                        MessageBox.Show(text);
                    }
                }
                else
                {
                    MessageBox.Show("设置信息读取错误，请先进行设置！");
                }
            }
            else
            {
                MessageBox.Show("设置信息读取错误，请先进行设置！");
            }
        }

        public void SetParameters(int spectrometerIndex, int IntegrationTime)
        {
            this.wrapper.setBoxcarWidth(spectrometerIndex, 10);
            this.wrapper.setIntegrationTime(spectrometerIndex, IntegrationTime);
        }

        public void ShowProgress(int totalStep, int currentStep)
        {
            this._ProgressBar.Maximum = totalStep;
            this._ProgressBar.Value = currentStep;
            if (currentStep == totalStep)
            {
                this._ProgressBar.Visible = false;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Data
        {
            public double[] DataX;
            public double[] DataY;
            public double[] DataE;
        }

        private delegate void ShowProgressDelegate(int totalStep, int currentStep);

        [StructLayout(LayoutKind.Sequential)]
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
    }
}

