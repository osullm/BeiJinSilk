using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace test_01
{
    public partial class Form1 : Form
    {
        //所有double[0,1],其中0为样品个数。其中1为样品中特征长度，如512个波长。
        List<double[]> originalFemaleData, originalMaleData;//存放原始导入进来的数据


        Process_FitOffLine offLineClass = new Process_FitOffLine();
        SIMCA.SIMCA simca = new SIMCA.SIMCA();
        ArrayList classLabel = new ArrayList() { "雌", "雄" };
        //int dataYlength = 385, SpecSkip=4;
        //int dataYSkipLength = 96;
        int selectSetMethod;
        double selectSetCount = 0.5;

        double thresholdAbnormal = 0.2;
        double[] femaleMaDistance, maleMaDistance;
        double[,] dataYForSelecteSet;

        double[,] femaleCalData, femaleValData,maleCalData,maleValData;
        Dictionary<string, double[,]> allOriginalCutedData;
        Dictionary<string, double[,]> CutedMaData=new Dictionary<string, double[,]> ();
        Dictionary<string, double[]> DataCaliSetPreprocessMx = new Dictionary<string, double[]>();
        Dictionary<string, double[]> DataCaliSetPreprocessSx = new Dictionary<string, double[]>();
        Dictionary<string, double[,]> DataCaliSetPreprocess = new Dictionary<string, double[,]>();
        Dictionary<string, double[,]> DataCaliSet = new Dictionary<string, double[,]>();
        Dictionary<string, double[,]> DataValiSet = new Dictionary<string, double[,]>();
        Dictionary<string, ArrayList> ClassPathVali = new Dictionary<string, ArrayList>(); 
        private Dictionary<string, string> DataValiSetName = new Dictionary<string, string>(); 

        Dictionary<string, double[,]> ModelInfo = new Dictionary<string, double[,]>();
        Dictionary<int, double[,]> ModelByMainFactor = new Dictionary<int, double[,]>();
        private Dictionary<string, int[]> DataCaliSetAllIndex = new Dictionary<string, int[]>();
        private double[,] DataCaliSetAll;
        private double[] dataWavelength=new double [512];
        private ProcessState processState;

        double[,] model;

        public Form1()
        {
            InitializeComponent();
        }

   

        private void Form1_Load(object sender, EventArgs e)
        {
           
            MWNumericArray x = new double[100];
            MWArray[] arrayArray = simca.mncn(2, x);

        
        }



        /// <summary>
        /// 导入雌性数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            originalFemaleData=offLineClass.loadData();
        }
        /// <summary>
        /// 导入雄性数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            originalMaleData= offLineClass.loadData();
        }

        private Dictionary<string, double[,]> cutOrigionalData(List<double[]> origionalFemaleData, List<double[]> origionalMaleData)
        {
            int dataYlength = 385, SpecSkip = 4;
            int dataYSkipLength = 96;
            Dictionary<string, double[,]> allDataSet = new Dictionary<string, double[,]>();
            double[,] bufData = new double[origionalFemaleData.Count, dataYlength];
           
            double[,] femaleData = new double[origionalFemaleData.Count, dataYSkipLength];
           
            for (int i = 0; i < origionalFemaleData.Count; i++)
            {
                int num3 = 0;
                //截取 41-109,167-484的总共385个数据。
                for (int j = 41; j < 109; j++)
                {
                    bufData[i, num3++] = origionalFemaleData[i][j];
                }
                for (int k = 167; k < 484; k++)
                {
                    bufData[i, num3++] = origionalFemaleData[i][k];
                }
                //每隔m=4个，选一个值进行计算。
                for (int m = 0; m < dataYSkipLength; m++)
                {
                    femaleData[i, m] = bufData[i, m * SpecSkip];
                }
            }
            allDataSet.Add("雌", femaleData);

           
            bufData= new double[origionalFemaleData.Count, dataYlength];
            double[,] maleData = new double[origionalMaleData.Count, dataYSkipLength];

            for (int i = 0; i < origionalMaleData.Count; i++)
            {
                int num3 = 0;
                //截取 41-109,167-484的总共385个数据。
                for (int j = 41; j < 109; j++)
                {
                    bufData[i, num3++] = origionalMaleData[i][j];
                }
                for (int k = 167; k < 484; k++)
                {
                    bufData[i, num3++] = origionalMaleData[i][k];
                }
                //每隔m=4个，选一个值进行计算。
                for (int m = 0; m < dataYSkipLength; m++)
                {
                    maleData[i, m] = bufData[i, m * SpecSkip];
                }
            }
            allDataSet.Add("雄", maleData);
            return allDataSet;
        }

        /// <summary>
        /// 雌性-计算马氏距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string classLabel = "雌";
            for(int i=0;i<5;i++)
            {
                bwgAbnormalSample.RunWorkerAsync(classLabel);
            }
        }

        /// <summary>
        /// 雄性--计算马氏距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            string classLabel = "雄";
            for (int i = 0; i < 5; i++)
            {
                bwgAbnormalSample.RunWorkerAsync(classLabel);
            }
        }

        /// <summary>
        /// 计算马氏距离，并且截去5个马氏距离最大样品，截去超过阈值的样品，截取后放入CutedMaData。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bwgAbnormalSample_DoWork(object sender, DoWorkEventArgs e)
        {
            //e的参数为“雌”或者“雄”。
            double[,] bufData = new double[allOriginalCutedData[e.Argument.ToString()].GetLength(1), allOriginalCutedData[e.Argument.ToString()].GetLength(0)];
            allOriginalCutedData[e.Argument.ToString()].CopyTo(bufData, 0);

            //截去马氏距离最大的5个样品。
            double[] maDistance;
            for (int i = 0; i < 5; i++)
            {
                new ripsPreDeal().maDistanceAbnormalIndex(bufData, thresholdAbnormal, out maDistance);
                double maxMaDistance = maDistance[0];
                int maxIndex = 0;
                for (int j = 1; j < maDistance.Length; j++)
                {
                    if (maDistance[j] > maxMaDistance)
                    {
                        maxIndex = j;
                        maxMaDistance = maDistance[j];
                    }
                }
                double[,] cutedData = new double[bufData.GetLength(1) - 1, bufData.GetLength(0)];
                int cutIndex = 0;
                for (int m = 0; m < bufData.GetLength(1); m++)
                {
                    if (m != maxIndex)
                    {
                        for (int n = 0; n < bufData.GetLength(0); n++)
                        {

                            cutedData[cutIndex, n] = bufData[m, n];
                        }
                        cutIndex++;
                    }
                }
                bufData = cutedData;

            }

            int[] reArray = new ripsPreDeal().maDistanceAbnormalIndex(bufData, thresholdAbnormal, out maDistance);
            //去掉超过马氏距离阈值的部分
            if (reArray.Length > 0)
            {
                double[,] cutedData = new double[bufData.GetLength(1) - reArray.Length, bufData.GetLength(0)];
                int cutIndex = 0;
                for (int m = 0; m < bufData.GetLength(1); m++)
                {
                    bool docopy = true;
                    for (int skipIndex = 0; skipIndex < reArray.Length; skipIndex++)
                    {
                        if (m == reArray[skipIndex])
                            docopy = false;
                    }
                    if (docopy)
                    {
                        for (int n = 0; n < bufData.GetLength(0); n++)
                        {
                            cutedData[cutIndex, n] = bufData[m, n];
                        }
                        cutIndex++;
                    }
                }
                bufData = cutedData;
            }
            if (CutedMaData.ContainsKey(e.Argument.ToString()))
            {
                CutedMaData[e.Argument.ToString()] = bufData;
            }
            else
            {
                CutedMaData.Add(e.Argument.ToString(), bufData);
            }
            e.Result = e.Argument;//传递样品的种类。

        }

        /// <summary>
        /// 使用随机法，或者K-S法分 DataCaliSet 和 DataValiSet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bwgAbnormalSample_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (radioButtonRandom.Checked)
            {
                this.selectSet(0, e.Result.ToString());
                selectSetMethod = 0;
            }
            else if (radioButtonKS.Checked)
            {
                this.selectSet(1, e.Result.ToString());
                selectSetMethod = 1;
            }
            Settings.Default.selectSetCount = this.domainUpDown2.SelectedIndex;
            Settings.Default.Save();

        }

        /// <summary>
        /// 执行随机法，或者K-S法分 DataCaliSet 和 DataValiSet.
        /// </summary>
        /// <param name="selectSetMethod"></param>
        /// <param name="classLabel"></param>
        private void selectSet(int selectSetMethod,string classLabel)
        {
                int[] numArray2;
                int[] numArray3;
                double d = selectSetCount * CutedMaData[classLabel ].GetLength(0);
                int nC = (int)Math.Floor(d);
                double[,] data = new double[CutedMaData[classLabel].GetLength(0), CutedMaData[classLabel].GetLength(1)];
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int m = 0; m < data.GetLength(1); m++)
                    {
                        data[i, m] = CutedMaData[classLabel][i, m];
                    }
                }
                ripsPreDeal deal = new ripsPreDeal(data.GetLength(0), data.GetLength(1));
                if (selectSetMethod == 0)
                {
                    deal.RandomSet(nC, data, out numArray2, out numArray3);
                }
                else
                {
                    deal.KennardStone(nC, data, out numArray2, out numArray3);
                }

                //if (female)
                //{

                   double[,] CaliData = new double[numArray2.Length, data.GetLength(1)];
                   double[,] ValiData = new double[numArray3.Length, data.GetLength(1)];
            for (int j = 0; j < numArray2.Length; j++)
            {
                for (int m = 0; m < data.GetLength(1); m++)
                {
                    CaliData[j, m] = data[numArray2[j], m];
                }

            }
            for (int k = 0; k < numArray3.Length; k++)
            {
                for (int m = 0; m < data.GetLength(1); m++)
                {
                    ValiData[k, m] = data[numArray3[k], m];
                }

            }

            if(DataCaliSet .ContainsKey (classLabel ))
            {
                DataCaliSet[classLabel] = CaliData;
            }
            else
            {
                DataCaliSet.Add(classLabel, CaliData);
            }
            if(DataValiSet .ContainsKey (classLabel ))
            {
                DataValiSet[classLabel] = ValiData;
            }
            else
            {
                DataValiSet.Add(classLabel, ValiData);
            }

            //DataCaliSet.Add("雌", femaleCalData);
            //DataValiSet.Add("雌", femaleValData);
            //}
            //else
            //{
            //    maleCalData = new double[numArray2.Length, data.GetLength(1)];
            //    maleValData = new double[numArray3.Length, data.GetLength(1)];
            //    for (int j = 0; j < numArray2.Length; j++)
            //    {
            //        for (int m = 0; m < data.GetLength(1); m++)
            //        {
            //            maleCalData[j, m] = data[numArray2[j], m];
            //        }

            //    }
            //    for (int k = 0; k < numArray3.Length; k++)
            //    {
            //        for (int m = 0; m < data.GetLength(1); m++)
            //        {
            //            maleValData[k, m] = data[numArray3[k], m];
            //        }

            //    }
            //    //DataCaliSet.Add("雄", maleCalData);
            //    //DataValiSet.Add("雄", maleValData);
            //}

        }

        private void button5_Click(object sender, EventArgs e)
        {






        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Title = " 打开";
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Filter = " txt files(*.txt)|*.txt|All files(*.*)|*.*";
            this.openFileDialog1.CheckFileExists = true;
            string[] fileNames = null;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.openFileDialog1.RestoreDirectory = true;
                fileNames = this.openFileDialog1.FileNames;
            }
            else
            {
                return;
            }
            string path = fileNames[0].Substring(0, fileNames[0].LastIndexOf('-')) + "-SimcaMoel.txt";
            string str2 = fileNames[0].Substring(0, fileNames[0].LastIndexOf('-')) + "-SimcaInfo.txt";
            if (File.Exists(path))
            {
                this.model = null;
                try
                {
                    this.DataIOmy.TXTReadDatas(path, out this.model);
                }
                catch (Exception)
                {
                    try
                    {
                        this.DataIOmy.LoadSIMCAModel(path, out this.model);
                    }
                    catch
                    {
                        MessageBox.Show("模型读取错误！");
                    }
                }
            }
            else
            {
                MessageBox.Show("模型文件不存在");
            }
            if (File.Exists(str2))
            {
                this.classLabel.Clear();
                this.ReadSimcaInfo(str2, ref this.classLabel);
            }
            else
            {
                MessageBox.Show("模型配置文件不存在");
            }
            new Thread(new ThreadStart(this.LoadData)).Start();
            if (this.DataValiSet.Count > 0)
            {
                //this.bgwPredict.RunWorkerAsync();
            }
        }


        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        DataIO DataIOmy = new DataIO();

        public IEnumerable<KeyValuePair<string, ArrayList>> ClassPathCali { get; private set; }

        private void button7_Click(object sender, EventArgs e)
        {
            string path = "";
            string str2 = "";
            saveFileDialog1.Title = " 打开";
            this.saveFileDialog1.Filter = " txt files(*.txt)|*.txt|All files(*.*)|*.*";
            this.saveFileDialog1.CheckFileExists = false;
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.saveFileDialog1.RestoreDirectory = true;
                path = this.saveFileDialog1.FileName;
            }
            else
            {
                return;
            }
            str2 = path.Substring(0, path.Length - 4) + "-SimcaInfo.txt";
            path = path.Substring(0, path.Length - 4) + "-SimcaMoel.txt";
            if ((this.DataIOmy.SaveSIMCAModel(path, this.model) > 0) && (this.WriteSimcaInfo(str2, this.classLabel) > 0))
            {
                MessageBox.Show("模型保存成功！");
            }
        }

        private void ReadSimcaInfo(string path, ref ArrayList classlabel)
        {
            string[] strArray;
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            this.DataIOmy.ReadLineTXT(path, out strArray);
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArray2 = strArray[i].Split(new char[] { ' ', ',', '，' });
                if (strArray2.Length > 1)
                {
                    dictionary.Add(int.Parse(strArray2[0].ToString()), strArray2[1].ToString());
                }
            }
            for (int j = 0; j < dictionary.Count; j++)
            {
                classlabel.Add(dictionary[j]);
            }
        }
        private int WriteSimcaInfo(string path, ArrayList classlabel)
        {
            string content = "";
            try
            {
                foreach (string str2 in classlabel)
                {
                    content = classlabel.IndexOf(str2) + "," + str2;
                    this.DataIOmy.TXTWriteIn(path, content);
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }


        private void LoadData()
        {
            foreach (KeyValuePair<string, ArrayList> pair in ClassPathCali)
            {
                ArrayList list = pair.Value;
                if (list.Count > 0)
                {
                    double[] numArray = new double[10];
                    double[] numArray2 = new double[10];
                    int num = this.DataIOmy.TXTReadData(list[0].ToString(), ref numArray, ref numArray2, true);
                    this.dataWavelength = new double[num];
                    double[,] numArray3 = new double[list.Count, num];
                    int num2 = 0;
                    foreach (object obj2 in list)
                    {
                        if (num == this.DataIOmy.TXTReadData(obj2.ToString(), ref numArray, ref numArray2, true))
                        {
                            numArray = new double[num];
                            numArray2 = new double[num];
                            this.DataIOmy.TXTReadData(obj2.ToString(), ref numArray, ref numArray2, false);
                            for (int j = 0; j < num; j++)
                            {
                                numArray3[num2, j] = numArray2[j];
                            }
                            num2++;
                            try
                            {
                                numArray.CopyTo(this.dataWavelength, 0);
                            }
                            catch
                            {
                                throw new Exception("数据位数不符1");
                            }
                        }
                    }
                    double[,] numArray4 = new double[numArray3.GetLength(0), 0x181];
                    double[,] numArray5 = new double[numArray3.GetLength(0), (int)Math.Ceiling((double)(((double)numArray4.GetLength(1)) / ((double)this.SpecSkip)))];
                    this.dataWavelength = new double[numArray5.GetLength(1)];
                    for (int i = 0; i < numArray3.GetLength(0); i++)
                    {
                        int num5 = 0;
                        for (int k = 0x29; k < 0x6d; k++)
                        {
                            numArray4[i, num5++] = numArray3[i, k];
                        }
                        for (int m = 0xa7; m < 0x1e4; m++)
                        {
                            numArray4[i, num5++] = numArray3[i, m];
                        }
                        for (int n = 0; n < numArray5.GetLength(1); n++)
                        {
                            numArray5[i, n] = numArray4[i, n * this.SpecSkip];
                        }
                    }
                    if (this.DataCaliSet.ContainsKey(pair.Key.ToString()))
                    {
                        this.DataCaliSet[pair.Key.ToString()] = numArray5;
                    }
                    else
                    {
                        this.DataCaliSet.Add(pair.Key.ToString(), numArray5);
                    }
                }
            }
            foreach (KeyValuePair<string, ArrayList> pair2 in ClassPathVali)
            {
                ArrayList list2 = pair2.Value; 
                if (list2.Count > 0)
                {
                    double[] numArray6 = new double[10];
                    double[] numArray7 = new double[10];
                    int num9 = this.DataIOmy.TXTReadData(list2[0].ToString(), ref numArray6, ref numArray7, true);
                    int count = list2.Count;
                    foreach (object obj3 in list2)
                    {
                        if (num9 == this.DataIOmy.TXTReadData(obj3.ToString(), ref numArray6, ref numArray7, true))
                        {
                            numArray6 = new double[num9];
                            numArray7 = new double[num9];
                            this.DataIOmy.TXTReadData(obj3.ToString(), ref numArray6, ref numArray7, false);
                            string key = obj3.ToString().Substring(obj3.ToString().LastIndexOf(@"\") + 1);
                            double[] numArray8 = new double[0x181];
                            int num10 = 0;
                            for (int num11 = 0x29; num11 < 0x6d; num11++)
                            {
                                numArray8[num10++] = numArray7[num11];
                            }
                            for (int num12 = 0xa7; num12 < 0x1e4; num12++)
                            {
                                numArray8[num10++] = numArray7[num12];
                            }
                            double[] numArray9 = new double[(int)Math.Ceiling((double)(((double)numArray8.Length) / ((double)this.SpecSkip)))];
                            for (int num13 = 0; num13 < numArray9.Length; num13++)
                            {
                                numArray9[num13] = numArray8[num13 * this.SpecSkip];
                            }
                            if (this.DataValiSet.ContainsKey(key))
                            {
                                this.DataValiSet[key] = new double[numArray9.Length];
                                numArray9.CopyTo(this.DataValiSet[key], 0);
                            }
                            else
                            {
                                this.DataValiSet.Add(key, numArray9);
                            }
                            if (this.DataValiSetName.ContainsKey(key))
                            {
                                this.DataValiSetName[key] = pair2.Key.ToString();
                            }
                            else
                            {
                                this.DataValiSetName.Add(key, pair2.Key);
                            }
                        }
                    }
                }
            }
            this.processState = ProcessState.preProcess;

        }


        private void button8_Click(object sender, EventArgs e)
        {
            allOriginalCutedData = cutOrigionalData(originalFemaleData, originalMaleData);
        }

        private void bgwCrossBySimca_DoWork(object sender, DoWorkEventArgs e)
        {
            int[] numArray = new int[this.classLabel.Count];
            for (int i = 0; i < this.classLabel.Count; i++)
            {
                numArray[i] = this.crossvalBySimca(i, this.bgwCrossBySimca);
                if (numArray[i] == -1)
                {
                    break;
                }
                if (!this.ModelInfo.ContainsKey(this.classLabel[i].ToString()))
                {
                    this.ModelInfo.Add(this.classLabel[i].ToString(), this.ModelByMainFactor[numArray[i]]);
                }
                else
                {
                    this.ModelInfo[this.classLabel[i].ToString()] = this.ModelByMainFactor[numArray[i]];
                }
            }


        }


        private int crossvalBySimca(int processCount, BackgroundWorker bw)
        {

            


            EventHandler method = null;
            EventHandler handler2 = null;
            //ZedGraphControl chart1;
            this.ModelByMainFactor.Clear();
            //this.lblState.Invoke(
            //    new Action<object>((obj) => { lblState.Text = "正在对类别：" + this.classLabel[processCount].ToString() + "进行光谱预处理"; })
            ////    delegate (object param0, EventArgs param1) {
            ////    this.lblState.Text = "正在对类别：" + this.classLabel[processCount].ToString() + "进行光谱预处理";
            ////}

            //);
            //lblState.Invoke(new Action(() => { lblState.Text = "正在对类别：" + this.classLabel[processCount].ToString() + "进行光谱预处理"; }));

            this.preProcess(0,bw);   //使用0方法预处理
            int num = (int)Math.Ceiling((double)(((double)this.DataCaliSet[this.classLabel[0].ToString()].GetLength(0)) / 4.0));
            //bw.ReportProgress((((num - 4) + 1) * processCount) + 1);
            //this.lblState.Invoke(
            //    new Action <object >((obj)=>
            //    {
            //       lblState.Text = "正在对类别：" + this.classLabel[processCount].ToString() + "进行建模";
            //    })  
            ////    delegate (object param0, EventArgs param1) {
            ////    this.lblState.Text = "正在对类别：" + this.classLabel[processCount].ToString() + "进行建模";
            ////}
            //);
            //lblState.Invoke(new Action(() => { lblState.Text = "正在对类别：" + this.classLabel[processCount].ToString() + "进行建模"; }));

            KeyValuePair<string, double[,]> vk = new KeyValuePair<string, double[,]>(this.classLabel[processCount].ToString(), this.DataCaliSetPreprocess[this.classLabel[processCount].ToString()]);
            int num2 = (int)Math.Ceiling((double)(((double)this.DataCaliSet[this.classLabel[processCount].ToString()].GetLength(0)) / 3.0));
            int num3 = 0, pcNum = 5;

            int[] numArray = new int[num2];
            while (pcNum < num2)
            {
                pcaModel2(pcNum, vk);
                pcaModel(pcNum, vk);                ////////////////////////建模
                if (method == null)
                {
                    //method = delegate (object param0, EventArgs param1) {
                    //    this.MyChart.GraphPane = chart1.GraphPane;
                    //    this.MyChart.Refresh();
                    //    this.MyChart.Update();
                    //};
                }
                //this.MyChart.Invoke(method);
                double[,] numArray2 = this.ModelInfo[this.classLabel[processCount].ToString()];
                if (!this.ModelByMainFactor.ContainsKey(pcNum))
                {
                    this.ModelByMainFactor.Add(pcNum, numArray2);
                }
                else
                {
                    this.ModelByMainFactor[pcNum] = numArray2;
                }
                //CurveItem[] itemArray = chart1.GraphPane.CurveList.ToArray();
                ////double y = itemArray[2].Points[0].Y;
                ////double x = itemArray[3].Points[0].X;
                //num3 = 0;
                //for (int j = 0; j < itemArray[0].Points.Count; j++)
                //{
                //    if ((itemArray[0].Points[j].X < x) && (itemArray[0].Points[j].Y < y))
                //    {
                //        num3++;
                //    }
                //}
                //numArray[pcNum] = num3;
                //bw.ReportProgress((((((num - 4) + 1) * processCount) + 1) + pcNum) - 5);
                //if (num3 == 0)
                //{
                //    break;
                //}
                pcNum++;
            }
            if (num3 == 0)
            {
                //this.isModelDanger = false;
                //if (handler2 == null)
                //{
                //    handler2 = delegate (object param0, EventArgs param1) {
                //        this.lblState.Text = "类别：" + this.classLabel[processCount].ToString() + "建模完毕";
                //    };
                //}
                //this.lblState.Invoke(handler2);
                //return pcNum;
            }
            //this.isModelDanger = true;
            int num8 = 200;
            int num9 = 0;
            for (int i = 5; i < numArray.Length; i++)
            {
                if (num8 > numArray[i])
                {
                    num8 = numArray[i];
                    num9 = i;
                }
            }
            if (num8 < 3)
            {
                return num9;
            }
            return -1;

        }

        private void preProcess(int preMethod, BackgroundWorker bw)
        {
            int percentProgress = 0;
            int num2 = 0;
            foreach (KeyValuePair<string, double[,]> pair in this.DataCaliSet)
            {
                num2 += pair.Value.GetLength(0);
                if (this.DataCaliSetAllIndex.ContainsKey(pair.Key))
                {
                    this.DataCaliSetAllIndex[pair.Key] = new int[pair.Value.GetLength(0)];
                }
                else
                {
                    this.DataCaliSetAllIndex.Add(pair.Key, new int[pair.Value.GetLength(0)]);
                }
            }
            this.DataCaliSetAll = new double[num2, this.dataWavelength.Length];
            num2 = 0;
            foreach (KeyValuePair<string, double[,]> pair2 in DataCaliSet)
            {
                double[,] numArray = pair2.Value;
                for (int i = 0; i < numArray.GetLength(0); i++)
                {
                    this.DataCaliSetAllIndex[pair2.Key][i] = num2;
                    for (int j = 0; j < numArray.GetLength(1); j++)
                    {
                        this.DataCaliSetAll[num2, j] = numArray[i, j];
                    }
                    num2++;
                }
                MWNumericArray x = numArray;
                if (preMethod == 0)
                {
                    MWArray[] arrayArray = simca.mncn(2, x);
                    MWNumericArray array2 = (MWNumericArray)arrayArray[0];
                    double[,] numArray2 = (double[,])array2.ToArray(MWArrayComponent.Real);
                    MWNumericArray array3 = (MWNumericArray)arrayArray[1];
                    double[,] numArray3 = (double[,])array3.ToArray(MWArrayComponent.Real);
                    double[] numArray4 = new double[numArray.GetLength(1)];
                    double[] numArray5 = new double[numArray.GetLength(1)];
                    for (int k = 0; k < numArray.GetLength(1); k++)
                    {
                        numArray4[k] = numArray3[0, k];
                        numArray5[k] = 1.0;
                    }
                    if (this.DataCaliSetPreprocessMx.ContainsKey(pair2.Key))
                    {
                        this.DataCaliSetPreprocessMx[pair2.Key] = numArray4;
                    }
                    else
                    {
                        this.DataCaliSetPreprocessMx.Add(pair2.Key, numArray4);
                    }
                    if (this.DataCaliSetPreprocessSx.ContainsKey(pair2.Key))
                    {
                        this.DataCaliSetPreprocessSx[pair2.Key] = numArray5;
                    }
                    else
                    {
                        this.DataCaliSetPreprocessSx.Add(pair2.Key, numArray5);
                    }
                    if (this.DataCaliSetPreprocess.ContainsKey(pair2.Key))
                    {
                        this.DataCaliSetPreprocess[pair2.Key] = numArray2;
                    }
                    else
                    {
                        this.DataCaliSetPreprocess.Add(pair2.Key, numArray2);
                    }
                }
                else if (preMethod == 1)
                {
                    MWArray[] arrayArray2 = simca.auto(3, x);
                    MWNumericArray array4 = (MWNumericArray)arrayArray2[0];
                    double[,] numArray6 = (double[,])array4.ToArray(MWArrayComponent.Real);
                    MWNumericArray array5 = (MWNumericArray)arrayArray2[1];
                    double[,] numArray7 = (double[,])array5.ToArray(MWArrayComponent.Real);
                    MWNumericArray array6 = (MWNumericArray)arrayArray2[2];
                    double[,] numArray8 = (double[,])array6.ToArray(MWArrayComponent.Real);
                    double[] numArray9 = new double[numArray.GetLength(1)];
                    double[] numArray10 = new double[numArray.GetLength(1)];
                    for (int m = 0; m < numArray.GetLength(1); m++)
                    {
                        numArray9[m] = numArray7[0, m];
                        numArray10[m] = numArray8[0, m];
                    }
                    if (this.DataCaliSetPreprocessMx.ContainsKey(pair2.Key))
                    {
                        this.DataCaliSetPreprocessMx[pair2.Key] = numArray9;
                    }
                    else
                    {
                        this.DataCaliSetPreprocessMx.Add(pair2.Key, numArray9);
                    }
                    if (this.DataCaliSetPreprocessSx.ContainsKey(pair2.Key))
                    {
                        this.DataCaliSetPreprocessSx[pair2.Key] = numArray10;
                    }
                    else
                    {
                        this.DataCaliSetPreprocessSx.Add(pair2.Key, numArray10);
                    }
                    if (this.DataCaliSetPreprocess.ContainsKey(pair2.Key))
                    {
                        this.DataCaliSetPreprocess[pair2.Key] = numArray6;
                    }
                    else
                    {
                        this.DataCaliSetPreprocess.Add(pair2.Key, numArray6);
                    }
                }
                //percentProgress++;
                //bw.ReportProgress(percentProgress);
            }


        
            

        }

        private void pcaModel2(int pcNum, KeyValuePair<string, double[,]> vk)
        {
            double[,] numArray11;
            double[,] numArray = vk.Value;
            MWNumericArray data = numArray;
            MWNumericArray plots = -1;
            MWNumericArray scl = 0;
            MWNumericArray lv = pcNum;
            MWArray[] arrayArray = this.simca.pca(7, data, plots, scl, lv);
            MWNumericArray dataCaliSetAll = this.DataCaliSetAll;
            MWNumericArray means = this.DataCaliSetPreprocessMx[vk.Key];
            MWNumericArray stds = this.DataCaliSetPreprocessSx[vk.Key];
            MWNumericArray array8 = (MWNumericArray)this.simca.scale(1, dataCaliSetAll, means, stds)[0];
            double[,] sdata = (double[,])array8.ToArray(MWArrayComponent.Real);
            Matrix matrix = null;
            if (sdata == null)
            {
                throw new Exception("scale error");
            }
            if ((sdata.GetLength(0) + sdata.GetLength(1)) <= 0)
            {
                throw new Exception("scale error");
            }
            matrix = new Matrix(sdata);
            MWNumericArray array9 = (MWNumericArray)arrayArray[0];
            double[,] numArray3 = (double[,])array9.ToArray(MWArrayComponent.Real);
            MWNumericArray array10 = (MWNumericArray)arrayArray[1];
            double[,] numArray4 = (double[,])array10.ToArray(MWArrayComponent.Real);
            Matrix matrix2 = new Matrix(numArray4);
            Matrix a = matrix * matrix2;
            Matrix matrix4 = null;
            matrix4 = a * matrix2.Transpose();
            Matrix matrix5 = new Matrix(sdata.GetLength(0), 1);
            for (int i = 0; i < sdata.GetLength(0); i++)
            {
                double num2 = 0.0;
                for (int num3 = 0; num3 < sdata.GetLength(1); num3++)
                {
                    num2 += (sdata[i, num3] - matrix4[i, num3]) * (sdata[i, num3] - matrix4[i, num3]);
                }
                matrix5[i, 0] = num2;
            }
            MWNumericArray array11 = (MWNumericArray)arrayArray[5];
            double[,] numArray5 = (double[,])array11.ToArray(MWArrayComponent.Real);
            double d = numArray5[0, 0];
            MWNumericArray array12 = (MWNumericArray)arrayArray[6];
            double[,] numArray6 = (double[,])array12.ToArray(MWArrayComponent.Real);
            Matrix matrix6 = new Matrix(numArray6);
            MWNumericArray array13 = (MWNumericArray)arrayArray[3];
            double[,] numArray7 = (double[,])array13.ToArray(MWArrayComponent.Real);
            Matrix matrix7 = new Matrix(numArray7);
            MWNumericArray array14 = (MWNumericArray)arrayArray[4];
            double[,] numArray8 = (double[,])array14.ToArray(MWArrayComponent.Real);
            double num5 = numArray8[0, 0];
            MWNumericArray array15 = (MWNumericArray)arrayArray[2];
            double[,] numArray9 = (double[,])array15.ToArray(MWArrayComponent.Real);
            Matrix matrix8 = (Matrix)((1.0 / d) * matrix6);
            Matrix matrix9 = (Matrix)((1.0 / num5) * matrix7);
            Matrix matrix10 = new Matrix(matrix8.Row, matrix8.Col + matrix9.Col);
            for (int j = 0; j < matrix8.Row; j++)
            {
                for (int num7 = 0; num7 < matrix8.Col; num7++)
                {
                    matrix10[j, num7] = matrix8[j, num7];
                }
                for (int num8 = 0; num8 < matrix9.Col; num8++)
                {
                    matrix10[j, matrix8.Col + num8] = matrix8[j, num8];
                }
            }
            double[] diagvalue = new double[a.Col];
            for (int k = 0; k < diagvalue.Length; k++)
            {
                diagvalue[k] = numArray9[k, 1];
            }
            Matrix matrix11 = null;
            matrix4.Setdiag(diagvalue);
            matrix4 = matrix4.Inverse();
            Matrix matrix12 = Matrix.SqureDot(a, a);
            if (a.Col > 1)
            {
                Matrix matrix13 = matrix12 * matrix4;
                matrix11 = Matrix.sumMatrix(matrix13.Transpose(), 1).Transpose();
            }
            else
            {
                matrix11 = matrix12 * matrix4;
            }
            //PointPairList points = new PointPairList();
            //double num10 = -1E+32;
            //double num11 = 1E+32;
            //double num12 = -1E+32;
            //double num13 = 1E+32;
            //for (int m = 0; m < matrix11.Row; m++)
            //{
            //    double x = Math.Log10(matrix11[m, 0]);
            //    double y = Math.Log10(matrix5[m, 0]);
            //    points.Add(x, y);
            //    if (x > num10)
            //    {
            //        num10 = x;
            //    }
            //    if (x < num11)
            //    {
            //        num11 = x;
            //    }
            //    if (y > num12)
            //    {
            //        num12 = y;
            //    }
            //    if (y < num13)
            //    {
            //        num13 = y;
            //    }
            //}
        }

        private void pcaModel(int pcNum, KeyValuePair<string, double[,]> vk)
        {

            double[,] numArray11;
            double[,] numArray = vk.Value;
            MWNumericArray data = numArray;
            MWNumericArray plots = -1;
            MWNumericArray scl = 0;
            MWNumericArray lv = pcNum;
            MWArray[] arrayArray = this.simca.pca(7, data, plots, scl, lv);
            MWNumericArray dataCaliSetAll = this.DataCaliSetAll;
            MWNumericArray means = this.DataCaliSetPreprocessMx[vk.Key];
            MWNumericArray stds = this.DataCaliSetPreprocessSx[vk.Key];
            MWNumericArray array8 = (MWNumericArray)this.simca.scale(1, dataCaliSetAll, means, stds)[0];
            double[,] sdata = (double[,])array8.ToArray(MWArrayComponent.Real);
            Matrix matrix = null;
            if (sdata == null)
            {
                throw new Exception("scale error");
            }
            if ((sdata.GetLength(0) + sdata.GetLength(1)) <= 0)
            {
                throw new Exception("scale error");
            }
            matrix = new Matrix(sdata);
            MWNumericArray array9 = (MWNumericArray)arrayArray[0];
            double[,] numArray3 = (double[,])array9.ToArray(MWArrayComponent.Real);
            MWNumericArray array10 = (MWNumericArray)arrayArray[1];
            double[,] numArray4 = (double[,])array10.ToArray(MWArrayComponent.Real);
            Matrix matrix2 = new Matrix(numArray4);
            Matrix a = matrix * matrix2;
            Matrix matrix4 = null;
            matrix4 = a * matrix2.Transpose();
            Matrix matrix5 = new Matrix(sdata.GetLength(0), 1);
            for (int i = 0; i < sdata.GetLength(0); i++)
            {
                double num2 = 0.0;
                for (int num3 = 0; num3 < sdata.GetLength(1); num3++)
                {
                    num2 += (sdata[i, num3] - matrix4[i, num3]) * (sdata[i, num3] - matrix4[i, num3]);
                }
                matrix5[i, 0] = num2;
            }
            MWNumericArray array11 = (MWNumericArray)arrayArray[5];
            double[,] numArray5 = (double[,])array11.ToArray(MWArrayComponent.Real);
            double d = numArray5[0, 0];
            MWNumericArray array12 = (MWNumericArray)arrayArray[6];
            double[,] numArray6 = (double[,])array12.ToArray(MWArrayComponent.Real);
            Matrix matrix6 = new Matrix(numArray6);
            MWNumericArray array13 = (MWNumericArray)arrayArray[3];
            double[,] numArray7 = (double[,])array13.ToArray(MWArrayComponent.Real);
            Matrix matrix7 = new Matrix(numArray7);
            MWNumericArray array14 = (MWNumericArray)arrayArray[4];
            double[,] numArray8 = (double[,])array14.ToArray(MWArrayComponent.Real);
            double num5 = numArray8[0, 0];
            MWNumericArray array15 = (MWNumericArray)arrayArray[2];
            double[,] numArray9 = (double[,])array15.ToArray(MWArrayComponent.Real);
            Matrix matrix8 = (Matrix)((1.0 / d) * matrix6);
            Matrix matrix9 = (Matrix)((1.0 / num5) * matrix7);
            Matrix matrix10 = new Matrix(matrix8.Row, matrix8.Col + matrix9.Col);
            for (int j = 0; j < matrix8.Row; j++)
            {
                for (int num7 = 0; num7 < matrix8.Col; num7++)
                {
                    matrix10[j, num7] = matrix8[j, num7];
                }
                for (int num8 = 0; num8 < matrix9.Col; num8++)
                {
                    matrix10[j, matrix8.Col + num8] = matrix8[j, num8];
                }
            }
            double[] diagvalue = new double[a.Col];
            for (int k = 0; k < diagvalue.Length; k++)
            {
                diagvalue[k] = numArray9[k, 1];
            }
            Matrix matrix11 = null;
            matrix4.Setdiag(diagvalue);
            matrix4 = matrix4.Inverse();
            Matrix matrix12 = Matrix.SqureDot(a, a);
            if (a.Col > 1)
            {
                Matrix matrix13 = matrix12 * matrix4;
                matrix11 = Matrix.sumMatrix(matrix13.Transpose(), 1).Transpose();
            }
            else
            {
                matrix11 = matrix12 * matrix4;
            }

            //PointList points = new PointPairList();
            //double num10 = -1E+32;
            //double num11 = 1E+32;
            //double num12 = -1E+32;
            //double num13 = 1E+32;
            //for (int m = 0; m < matrix11.Row; m++)
            //{
            //    double x = Math.Log10(matrix11[m, 0]);
            //    double y = Math.Log10(matrix5[m, 0]);
            //    points.Add(x, y);
            //    if (x > num10)
            //    {
            //        num10 = x;
            //    }
            //    if (x < num11)
            //    {
            //        num11 = x;
            //    }
            //    if (y > num12)
            //    {
            //        num12 = y;
            //    }
            //    if (y < num13)
            //    {
            //        num13 = y;
            //    }
            //}
            ///////////////////////////////////////////////////////////////////////////////
            //PointPairList list2 = new PointPairList();
            //PointPair[] array = new PointPair[points.Count];
            //points.CopyTo(array);
            //for (int n = 0; n < this.DataCaliSetAllIndex[vk.Key].Length; n++)
            //{
            //    int num18 = this.DataCaliSetAllIndex[vk.Key][n];
            //    double num19 = Math.Log10(matrix11[num18, 0]);
            //    double num20 = Math.Log10(matrix5[num18, 0]);
            //    list2.Add(num19, num20);
            //    for (int num21 = 0; num21 < array.Length; num21++)
            //    {
            //        if ((array[num21].X == num19) && (array[num21].Y == num20))
            //        {
            //            points.Remove(array[num21]);
            //        }
            //    }
            //}
            //string title = "Q vs.T^2 for all Data Projected on Model of Class  " + vk.Key;
            //ZedGraphControl control = new ZedGraphControl
            //{
            //    GraphPane = new GraphPane(this.RectangleFMy, title, "Value of T^2(10^x)", "value of Q(10^y)")
            //};
            //control.GraphPane.XAxis.Scale.Min = Math.Floor(num11);
            //control.GraphPane.XAxis.Scale.Max = Math.Ceiling(num10);
            //control.GraphPane.YAxis.Scale.Min = Math.Floor(num13);
            //control.GraphPane.YAxis.Scale.Max = Math.Ceiling(num12);
            //control.GraphPane.AddCurve("other sample", points, Color.DarkGreen, SymbolType.Plus).Line.IsVisible = false;
            //control.GraphPane.AddCurve("class " + vk.Key + " sample", list2, Color.MediumVioletRed, SymbolType.Star).Line.IsVisible = false;
            //double num22 = Math.Log10(num5);
            //LineItem item = new LineItem(string.Empty, new double[] { control.GraphPane.XAxis.Scale.Min, control.GraphPane.XAxis.Scale.Max }, new double[] { num22, num22 }, Color.Blue, SymbolType.None);
            //control.GraphPane.CurveList.Add(item);
            //double num23 = Math.Log10(d);
            //LineItem item4 = new LineItem(string.Empty, new double[] { num23, num23 }, new double[] { control.GraphPane.YAxis.Scale.Min, control.GraphPane.YAxis.Scale.Max }, Color.Blue, SymbolType.None);
            //control.GraphPane.CurveList.Add(item4);
            //control.AxisChange();
            //control.Refresh();
            //if (this.PreProcessMethod == 1)
            //{
            //    numArray11 = new double[3, numArray.GetLength(1)];
            //}
            //else
            //{
            //    numArray11 = new double[2, numArray.GetLength(1)];
            //}
            //for (int num24 = 0; num24 < numArray.GetLength(1); num24++)
            //{
            //    numArray11[1, num24] = this.DataCaliSetPreprocessMx[vk.Key][num24];
            //    if (this.PreProcessMethod == 1)
            //    {
            //        numArray11[2, num24] = this.DataCaliSetPreprocessSx[vk.Key][num24];
            //    }
            //}
            //numArray11[0, 0] = DateTime.Today.Year;
            //numArray11[0, 1] = DateTime.Today.Month;
            //numArray11[0, 2] = DateTime.Today.Day;
            //numArray11[0, 3] = DateTime.Today.Hour;
            //numArray11[0, 4] = DateTime.Today.Minute;
            //numArray11[0, 5] = DateTime.Today.Second;
            //numArray11[0, 6] = numArray3.GetLength(0);
            //numArray11[0, 7] = numArray.GetLength(1);
            //numArray11[0, 8] = 0.0;
            //numArray11[0, 9] = this.classLabel.IndexOf(vk.Key);
            //numArray11[0, 10] = 0.0;
            //numArray11[0, 11] = this.PreProcessMethod + 2;
            //numArray11[0, 12] = 0.0;
            //numArray11[0, 13] = numArray3.GetLength(1);
            //numArray11[0, 14] = num5;
            //numArray11[0, 15] = d;
            //List<double[,]> list3 = new List<double[,]> {
            //    numArray11
            //};
            //double[,] numArray12 = new double[1, numArray.GetLength(1)];
            //for (int num25 = 0; num25 < numArray3.GetLength(1); num25++)
            //{
            //    numArray12[0, num25] = numArray9[num25, 1];
            //}
            //list3.Add(numArray12);
            //numArray12 = new double[numArray4.GetLength(1), numArray4.GetLength(0)];
            //for (int num26 = 0; num26 < numArray12.GetLength(0); num26++)
            //{
            //    for (int num27 = 0; num27 < numArray12.GetLength(1); num27++)
            //    {
            //        numArray12[num26, num27] = numArray4[num27, num26];
            //    }
            //}
            //list3.Add(numArray12);
            //int num28 = 0;
            //for (int num29 = 0; num29 < list3.Count; num29++)
            //{
            //    num28 += list3[num29].GetLength(0);
            //}
            //double[,] numArray13 = new double[num28, numArray.GetLength(1)];
            //num28 = 0;
            //for (int num30 = 0; num30 < list3.Count; num30++)
            //{
            //    for (int num31 = 0; num31 < list3[num30].GetLength(0); num31++)
            //    {
            //        for (int num32 = 0; num32 < list3[num30].GetLength(1); num32++)
            //        {
            //            numArray13[num28, num32] = list3[num30][num31, num32];
            //        }
            //        num28++;
            //    }
            //}
            //if (this.ModelInfo.ContainsKey(vk.Key))
            //{
            //    this.ModelInfo[vk.Key] = numArray13;
            //    return control;
            //}
            //this.ModelInfo.Add(vk.Key, numArray13);
            //return control;
        }



    }
    public enum ProcessState
    {
        none,
        loadData,
        preProcess,
        crossval,
        pca,
        pcaAfter
    }

}
