namespace JSDU
{
    using Algorithm;
    using MathWorks.MATLAB.NET.Arrays;
    using SIMCA;
    using SIMCA.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using ZedGraph;

    public class Form_offLine : Form
    {
        private int abnormalSampleDeleteCount;
        private BackgroundWorker bgwCrossBySimca;
        private BackgroundWorker bgwCrossVal;
        private BackgroundWorker bgwPca;
        private BackgroundWorker bgwPredict;
        private BackgroundWorker bgwPreProcess;
        private Button btnAdd;
        private Button BtnCalibrationSetOK;
        private Button btnClear;
        private Button btnClose;
        private Button btnLoadData;
        private Button btnLoadDataPrd;
        private Button btnLoadDataReturn;
        private Button btnLoadFile;
        private Button btnLoadModel;
        private Button btnLoadModelPrd;
        private Button btnModel;
        private Button btnNext;
        private Button btnNextLoadData;
        private Button btnOpemFile;
        private Button btnPredict;
        private Button btnPrevious;
        private Button btnSaveModel;
        private Button btnSaveModelToSystem;
        private Button btnSImca;
        public Button btnValidate;
        private BackgroundWorker bwgAbnormalSample;
        private ArrayList classLabel = new ArrayList();
        private Dictionary<string, ArrayList> ClassPathCali = new Dictionary<string, ArrayList>();
        private Dictionary<string, ArrayList> ClassPathVali = new Dictionary<string, ArrayList>();
        private ComboBox cmbClassName;
        private ComboBox cmbLoadClass;
        private ComboBox cmbPCNum;
        private ComboBox cmbPreprocess;
        private DataGridViewTextBoxColumn ColumnFileNmae;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private ColumnHeader columnHeader13;
        private ColumnHeader columnHeader14;
        private ColumnHeader columnHeader15;
        private ColumnHeader columnHeader16;
        private ColumnHeader columnHeader18;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private DataGridViewComboBoxColumn Columnpurpose;
        private IContainer components;
        public ContextMenuStrip ContextMenuStrip1;
        public ContextMenuStrip contextMenuStrip2;
        private MWArray[] crossReslut;
        private Dictionary<string, double[,]> DataCaliSet = new Dictionary<string, double[,]>();
        private double[,] DataCaliSetAll;
        private Dictionary<string, int[]> DataCaliSetAllIndex = new Dictionary<string, int[]>();
        private Dictionary<string, int> DataCaliSetPCNum = new Dictionary<string, int>();
        private Dictionary<string, double[,]> DataCaliSetPreprocess = new Dictionary<string, double[,]>();
        private Dictionary<string, double[]> DataCaliSetPreprocessMx = new Dictionary<string, double[]>();
        private Dictionary<string, double[]> DataCaliSetPreprocessSx = new Dictionary<string, double[]>();
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataHandling DataHandlingmy = new DataHandling();
        private DataIO DataIOmy = new DataIO();
        private Data[] Datas;
        private Dictionary<string, double[]> DataValiSet = new Dictionary<string, double[]>();
        private Dictionary<string, string> DataValiSetName = new Dictionary<string, string>();
        private double[] dataWavelength;
        private double[,] dataYForSelecteSet;
        public ToolStripMenuItem DeleteContext;
        public ToolStripMenuItem DeleteContext1;
        private DomainUpDown domainUpDown2;
        private Color[] DrawColor = new Color[50];
        private GroupBox groupBox1;
        private bool isClearAbnormalSample;
        private bool isModelDanger;
        private bool isModelRobust;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelPredictResult;
        private System.Windows.Forms.Label lblState;
        private ListView listViewCaliSet;
        private ListView listViewPcaSsq;
        private ListView listViewPrdResult;
        private ListView listViewPredict;
        private ListView listViewValiSet;
        private double[] md;
        private double[,] MeanY;
        private double[,] model;
        private Dictionary<int, double[,]> ModelByMainFactor = new Dictionary<int, double[,]>();
        private Dictionary<string, double[,]> ModelInfo = new Dictionary<string, double[,]>();
        private Stack<modelNode> modelnode = new Stack<modelNode>();
        private Stack<modelNode> modelnodeUndo = new Stack<modelNode>();
        private ZedGraphControl MyChart;
        private MyChartLoadData myChartLoadData;
        private ZedGraphControl MyChartSsq;
        private OpenFileDialog openFileDialog1;
        private string[] OpenFileName;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private Panel panel7;
        private Panel panelbtnDownTip;
        private Panel panelCrossVal;
        private Panel panelHeader;
        private Panel panelLoadData;
        private Panel panelModel;
        private Panel panelPreprocess;
        private MWArray[] pcaReslut;
        private int PreProcessMethod;
        private int ProcessCount;
        private ProcessState processState;
        private ProgressBar progressBar1;
        private RadioButton radioButtonKS;
        private RadioButton radioButtonRandom;
        private RectangleF RectangleFMy = new RectangleF();
        private Dictionary<int, double> Rt2rqsumByMainFactor = new Dictionary<int, double>();
        private SaveFileDialog saveFileDialog1;
        private SIMCA simca= new SIMCA();
        private SimcaPrd simcaPrd = new SimcaPrd();
        private ArrayList SlectedListView1Items = new ArrayList();
        private int SpecSkip = 4;
        private string[] SpectrumOpenPath;
        private SplitContainer splitContainer1;
        private double[,] StdErrY;
        private TabControl tabControl1;
        private TabPage tabPageLoadData;
        private TabPage tabPageModel;
        private TabPage tabPagePredict;
        private TabPage tabPageValidate;
        private TableLayoutPanel tLpanelLoadData;
        private TextBox txtSimcaInfo;
        private TextBox txtThresholdAbnormal;
        private ViewStyle ViewStylemy = ViewStyle.Spec;
        private BackgroundWorker backgroundWorker1;
        private ZedGraphControl zedGraphControlMDChart;

        public Form_offLine()
        {
            this.InitializeComponent();
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
            for (int i = 20; i < 50; i++)
            {
                this.DrawColor[i] = this.DrawColor[i % 20];
            }
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
            e.Result = numArray;
        }

        private void bgwCrossBySimca_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            this.progressBar1.Invoke(
                new Action <object >((obj)=> {
                this.progressBar1.Value = progress;
                this.progressBar1.Update(); })
            //delegate (object param0, EventArgs param1) {
            //    this.progressBar1.Value = progress;
            //    this.progressBar1.Update();
            //}
            );
        }

        private void bgwCrossBySimca_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int[] result = (int[]) e.Result;
            int num = 0;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] < 0)
                {
                    MessageBox.Show("样本异常，无法进行模型建立！");
                    this.MyChart.GraphPane.CurveList.Clear();
                    string title = "Q vs.T^2 for all Data Projected on Model of Class";
                    this.MyChart.GraphPane = new GraphPane(this.RectangleFMy, title, "Value of T^2(10^x)", "value of Q(10^y)");
                    this.MyChart.Refresh();
                    this.progressBar1.Visible = false;
                    this.lblState.Visible = false;
                    this.tabControl1.SelectTab(this.tabPageLoadData);
                    return;
                }
                if (result[i] < (this.DataCaliSet[this.classLabel[i].ToString()].GetLength(0) / 4))
                {
                    num++;
                }
            }
            if (num == 2)
            {
                this.isModelRobust = true;
            }
            int num3 = 0;
            foreach (KeyValuePair<string, double[,]> pair in this.ModelInfo)
            {
                num3 += pair.Value.GetLength(0);
            }
            this.model = new double[num3 + 2, this.dataWavelength.Length];
            this.model[0, 0] = DateTime.Today.Year;
            this.model[0, 1] = DateTime.Today.Month;
            this.model[0, 2] = DateTime.Today.Day;
            this.model[0, 3] = DateTime.Today.Hour;
            this.model[0, 4] = DateTime.Today.Minute;
            this.model[0, 5] = DateTime.Today.Second;
            this.model[0, 5] = DateTime.Today.Second;
            this.model[0, 6] = this.DataCaliSetAll.GetLength(0);
            this.model[0, 7] = this.DataCaliSetAll.GetLength(1);
            this.model[0, 8] = -1.0;
            this.model[0, 9] = this.classLabel.Count;
            this.model[0, 10] = 0.0;
            this.model[0, 11] = this.PreProcessMethod + 2;
            num3 = 2;
            string str2 = "";
            foreach (string str3 in this.classLabel)
            {
                if (this.classLabel.IndexOf(str3) == 0)
                {
                    this.model[1, 0] = 3.0;
                }
                else
                {
                    this.model[1, this.classLabel.IndexOf(str3)] = this.model[1, this.classLabel.IndexOf(str2)] + this.ModelInfo[str2].GetLength(0);
                }
                for (int j = 0; j < this.ModelInfo[str3].GetLength(0); j++)
                {
                    for (int k = 0; k < this.ModelInfo[str3].GetLength(1); k++)
                    {
                        this.model[num3, k] = this.ModelInfo[str3][j, k];
                    }
                    num3++;
                }
                str2 = str3;
            }
            this.progressBar1.Visible = false;
            this.lblState.Visible = false;
            if (!this.bgwPredict.IsBusy)
            {
                this.bgwPredict.RunWorkerAsync();
            }
        }

        private void bgwCrossVal_DoWork(object sender, DoWorkEventArgs e)
        {
            KeyValuePair<string, double[,]> argument = (KeyValuePair<string, double[,]>) e.Argument;
            this.crossval(argument);
        }

        private void bgwCrossVal_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bgwCrossVal_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MWNumericArray array = (MWNumericArray) this.crossReslut[1];
            double[,] numArray = (double[,]) array.ToArray(MWArrayComponent.Real);
            PointPairList points = new PointPairList();
            for (int i = 0; i < numArray.GetLength(1); i++)
            {
                points.Add((double) i, numArray[0, i]);
            }
            this.RectangleFMy = this.MyChart.GraphPane.Rect;
            this.MyChart.GraphPane = new GraphPane(this.RectangleFMy, "PRESS Plot", "主因子数", "value");
            this.MyChart.GraphPane.CurveList.Clear();
            this.MyChart.GraphPane.AddCurve("PRESS Plot for Class-" + this.classLabel[this.ProcessCount].ToString(), points, Color.Blue, SymbolType.Circle);
            this.MyChart.GraphPane.XAxis.Scale.Min = 0.0;
            this.MyChart.GraphPane.XAxis.Scale.Max = numArray.GetLength(1);
            this.MyChart.AxisChange();
            this.MyChart.Refresh();
            MWNumericArray array2 = (MWNumericArray) this.pcaReslut[2];
            double[,] numArray2 = (double[,]) array2.ToArray(MWArrayComponent.Real);
            ListViewItem item = new ListViewItem();
            if ((numArray2.GetLength(0) > 2) && (numArray2.GetLength(1) == 4))
            {
                this.listViewPcaSsq.Items.Clear();
                this.listViewPcaSsq.BeginUpdate();
                for (int j = 0; j < numArray2.GetLength(0); j++)
                {
                    item = new ListViewItem();
                    item.SubItems.Clear();
                    int num5 = (int) numArray2[j, 0];
                    item.SubItems.Add(num5.ToString());
                    if (numArray2[j, 1] < 0.001)
                    {
                        item.SubItems.Add(numArray2[j, 1].ToString("0.###E+0"));
                    }
                    else
                    {
                        item.SubItems.Add(numArray2[j, 1].ToString("f4"));
                    }
                    if (numArray2[j, 2] < 0.001)
                    {
                        item.SubItems.Add(numArray2[j, 2].ToString("0.###E+0"));
                    }
                    else
                    {
                        item.SubItems.Add(numArray2[j, 2].ToString("f4"));
                    }
                    item.SubItems.Add(numArray2[j, 3].ToString("f2"));
                    item.SubItems.RemoveAt(0);
                    this.listViewPcaSsq.Items.Add(item);
                }
                this.listViewPcaSsq.EndUpdate();
                PointPairList list2 = new PointPairList();
                for (int k = 0; k < numArray2.GetLength(0); k++)
                {
                    list2.Add((double) k, numArray2[k, 2]);
                }
                this.RectangleFMy = this.MyChartSsq.GraphPane.Rect;
                this.MyChartSsq.GraphPane = new GraphPane(this.RectangleFMy, "Eigenvalue vs. PC Number", "主因子数", "value");
                this.MyChartSsq.GraphPane.CurveList.Clear();
                this.MyChartSsq.GraphPane.AddCurve("Eigenvalue vs. PC Number", list2, Color.Blue, SymbolType.Circle);
                this.MyChartSsq.GraphPane.XAxis.Scale.Min = 0.0;
                this.MyChartSsq.GraphPane.XAxis.Scale.Max = numArray2.GetLength(0);
                this.MyChartSsq.AxisChange();
                this.MyChartSsq.Refresh();
                this.cmbPCNum.Items.Clear();
                for (int m = 0; m < numArray2.GetLength(0); m++)
                {
                    int num6 = m + 1;
                    this.cmbPCNum.Items.Add(num6.ToString());
                    if (numArray2[m, 3] < 98.0)
                    {
                        this.cmbPCNum.SelectedIndex = m;
                    }
                }
            }
            this.cmbPCNum.SelectedIndex++;
            this.panelPreprocess.Visible = false;
            this.panelCrossVal.Visible = true;
            this.processState = ProcessState.pca;
            base.Enabled = true;
            this.txtSimcaInfo.Text = this.txtSimcaInfo.Text + "\r\n对于类别:" + this.classLabel[this.ProcessCount].ToString() + "交互验证完毕，请选择”主因字数“并点击”下一步“进行建模。";
        }

        private void bgwPreProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            this.preProcess((int) e.Argument, this.bgwPreProcess);
        }

        private void bgwPreProcess_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            this.progressBar1.Invoke(
                new Action <object >((obj)=> {
                    this.progressBar1.Value = progress;
                    this.progressBar1.Update();
                })

            //    delegate (object param0, EventArgs param1) {
            //    this.progressBar1.Value = progress;
            //    this.progressBar1.Update();
            //}
            );
        }

        private void bgwPreProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Visible = false;
            this.MyChart.GraphPane.CurveList.Clear();
            RectangleF rect = this.MyChart.GraphPane.Rect;
            this.MyChart.GraphPane = new GraphPane(rect, "平均光谱", "波长（nm）", "吸光度");
            int num = 0;
            foreach (KeyValuePair<string, double[]> pair in this.DataCaliSetPreprocessMx)
            {
                this.MyChart_LoadData(this.MyChart, this.dataWavelength, pair.Value, this.DrawColor[num++], pair.Key);
            }
            if (this.cmbPreprocess.SelectedIndex == 1)
            {
                this.MyChartSsq.GraphPane.CurveList.Clear();
                this.MyChartSsq.GraphPane = new GraphPane(rect, "标准差图", "波长（nm）", "value");
                foreach (KeyValuePair<string, double[]> pair2 in this.DataCaliSetPreprocessSx)
                {
                    this.MyChart_LoadData(this.MyChartSsq, this.dataWavelength, pair2.Value, this.DrawColor[num++], pair2.Key);
                }
            }
            this.processState = ProcessState.crossval;
            base.Enabled = true;
            this.txtSimcaInfo.Text = this.txtSimcaInfo.Text + "\r\n预处理完毕，请点击”下一步“进行交互验证";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.abnormalSampleDeleteCount = 0;
            this.zedGraphControlMDChart.GraphPane.CurveList.Clear();
            this.zedGraphControlMDChart.Refresh();
            this.zedGraphControlMDChart.Visible = false;
            if (this.cmbClassName.SelectedItem == null)
            {
                MessageBox.Show("请选择类别名称！");
            }
            else
            {
                string str = this.cmbClassName.SelectedItem.ToString();
                if ((this.dataGridView1.Rows.Count >= 2) && (str != ""))
                {
                    if ((str.Contains(" ") || str.Contains(",")) || ((str.Contains("，") || str.Contains("\t")) || str.Contains("*")))
                    {
                        MessageBox.Show("类别名称中含有非法字符，请重新输入");
                    }
                    else
                    {
                        if (!this.classLabel.Contains(str))
                        {
                            this.classLabel.Add(str);
                        }
                        if (!this.ClassPathCali.ContainsKey(str))
                        {
                            this.ClassPathCali.Add(str, new ArrayList());
                        }
                        if (!this.ClassPathVali.ContainsKey(str))
                        {
                            this.ClassPathVali.Add(str, new ArrayList());
                        }
                        ArrayList list = this.ClassPathCali[str];
                        ArrayList list2 = this.ClassPathVali[str];
                        for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                        {
                            if (this.dataGridView1.Rows[i].Cells[0].Tag != null)
                            {
                                string str2 = this.dataGridView1.Rows[i].Cells[0].Tag.ToString();
                                if (this.dataGridView1.Rows[i].Cells[1].Value.ToString() == "校正集")
                                {
                                    if (!list.Contains(str2))
                                    {
                                        list.Add(str2);
                                    }
                                }
                                else
                                {
                                    if (!list2.Contains(str2))
                                    {
                                        list2.Add(str2);
                                    }
                                    if (list.Contains(str2))
                                    {
                                        list.Remove(str2);
                                    }
                                }
                            }
                        }
                        if (list2.Count == 0)
                        {
                            MessageBox.Show("请选择验证集样本");
                        }
                        else
                        {
                            this.dataGridView1.Rows.Clear();
                            this.ClassPathCali[str] = list;
                            this.ClassPathVali[str] = list2;
                            this.listViewCaliSet.BeginUpdate();
                            this.listViewValiSet.BeginUpdate();
                            this.listViewCaliSet.Items.Clear();
                            this.listViewValiSet.Items.Clear();
                            foreach (KeyValuePair<string, ArrayList> pair in this.ClassPathCali)
                            {
                                ArrayList list3 = pair.Value;
                                int num2 = 0;
                                foreach (object obj2 in list3)
                                {
                                    num2++;
                                    string text = obj2.ToString().Substring(obj2.ToString().LastIndexOf(@"\") + 1);
                                    ListViewItem item = new ListViewItem();
                                    item.SubItems.Add(num2.ToString());
                                    item.SubItems.Add(text);
                                    item.SubItems.Add(pair.Key.ToString());
                                    item.SubItems.RemoveAt(0);
                                    item.Tag = obj2.ToString();
                                    this.listViewCaliSet.Items.Add(item);
                                }
                            }
                            foreach (KeyValuePair<string, ArrayList> pair2 in this.ClassPathVali)
                            {
                                ArrayList list4 = pair2.Value;
                                int num3 = 0;
                                foreach (object obj3 in list4)
                                {
                                    num3++;
                                    string str4 = obj3.ToString().Substring(obj3.ToString().LastIndexOf(@"\") + 1);
                                    ListViewItem item2 = new ListViewItem();
                                    item2.SubItems.Add(num3.ToString());
                                    item2.SubItems.Add(str4);
                                    item2.SubItems.Add(pair2.Key.ToString());
                                    item2.SubItems.RemoveAt(0);
                                    item2.Tag = obj3.ToString();
                                    this.listViewValiSet.Items.Add(item2);
                                }
                            }
                            this.listViewCaliSet.EndUpdate();
                            this.listViewValiSet.EndUpdate();
                            this.processState = ProcessState.loadData;
                            if (!this.cmbLoadClass.Items.Contains(str))
                            {
                                this.cmbLoadClass.Items.Add(str);
                            }
                            this.cmbLoadClass.SelectedIndex = this.cmbLoadClass.Items.Count - 1;
                            this.cmbClassName.Text = "";
                            if (this.listViewCaliSet.Items.Count > 0)
                            {
                                this.btnClear.Enabled = true;
                                this.btnNextLoadData.Enabled = true;
                            }
                            else
                            {
                                this.btnClear.Enabled = false;
                                this.btnNextLoadData.Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        private void BtnCalibrationSetOK_Click(object sender, EventArgs e)
        {
            this.zedGraphControlMDChart.Visible = true;
            this.lblState.Visible = true;
            this.lblState.Text = "正在判别异常样本......";
            if (!this.bwgAbnormalSample.IsBusy)
            {
                this.bwgAbnormalSample.RunWorkerAsync();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.classLabel.Clear();
            this.ClassPathCali.Clear();
            this.ClassPathVali.Clear();
            this.listViewCaliSet.BeginUpdate();
            this.listViewCaliSet.Items.Clear();
            this.listViewCaliSet.EndUpdate();
            this.listViewValiSet.BeginUpdate();
            this.listViewValiSet.Items.Clear();
            this.listViewValiSet.EndUpdate();
            this.dataGridView1.Rows.Clear();
            this.cmbLoadClass.Items.Clear();
            this.cmbLoadClass.Text = "";
            this.cmbClassName.Text = "";
            this.modelnode.Clear();
            this.modelnodeUndo.Clear();
            this.MyChartSsq.GraphPane.CurveList.Clear();
            this.MyChartSsq.GraphPane.CurveList.Clear();
            this.listViewPcaSsq.Items.Clear();
            this.DataValiSet.Clear();
            this.DataValiSetName.Clear();
            this.DataCaliSet.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.bgwPca.IsBusy)
            {
                this.bgwPca.CancelAsync();
            }
            else if (this.bgwPredict.IsBusy)
            {
                this.bgwPredict.CancelAsync();
            }
            else if (this.bgwCrossVal.IsBusy)
            {
                this.bgwCrossVal.CancelAsync();
            }
            else if (this.bgwPca.IsBusy)
            {
                this.bgwPca.CancelAsync();
            }
            base.Dispose();
            base.Close();
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabControl1.TabPages[this.tabControl1.TabPages.IndexOfKey("tabPageLoadData")];
        }

        private void btnLoadDataPrd_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Title = " 打开";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Filter = " txt files(*.txt)|*.txt|All files(*.*)|*.*";
            this.openFileDialog1.CheckFileExists = true;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.openFileDialog1.RestoreDirectory = true;
                this.SpectrumOpenPath = this.openFileDialog1.FileNames;
            }
            else
            {
                return;
            }
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = this.SpectrumOpenPath.Length;
            this.progressBar1.Visible = true;
            this.dataGridView2.RowHeadersWidth = 60;
            for (int count = 0; count < this.SpectrumOpenPath.Length; count++)
            {
                EventHandler method = null;
                string strtempt = this.SpectrumOpenPath[count];
                if ((strtempt.Substring(strtempt.LastIndexOf(".") + 1) == "txt") || (strtempt.Substring(strtempt.LastIndexOf(".") + 1) == "TXT"))
                {
                    if (method == null)
                    {
                        method = delegate (object param0, EventArgs param1) {
                            int num = this.dataGridView2.Rows.Add();
                            this.dataGridView2.Rows[num].Cells[0].Value = strtempt.Substring(strtempt.LastIndexOf(@"\") + 1);
                            this.dataGridView2.Rows[num].Cells[0].Tag = strtempt;
                            this.dataGridView2.Rows[num].HeaderCell.Value = (num + 1).ToString();
                            if (count < this.progressBar1.Maximum)
                            {
                                this.progressBar1.Value = count + 1;
                            }
                            else
                            {
                                this.progressBar1.Value = this.progressBar1.Maximum;
                            }
                        };
                    }
                    base.Invoke(method);
                }
            }
            this.progressBar1.Visible = false;
            this.progressBar1.Value = 0;
        }

        private void btnLoadDataReturn_Click(object sender, EventArgs e)
        {
            if (this.cmbLoadClass.SelectedItem != null)
            {
                string str = this.cmbLoadClass.SelectedItem.ToString();
                if (this.classLabel.Contains(str))
                {
                    this.classLabel.Remove(str);
                }
                if (this.ClassPathCali.ContainsKey(str))
                {
                    this.ClassPathCali.Remove(str);
                }
                if (this.ClassPathVali.ContainsKey(str))
                {
                    this.ClassPathVali.Remove(str);
                }
                List<ListViewItem> list = new List<ListViewItem>();
                foreach (ListViewItem item in this.listViewCaliSet.Items)
                {
                    if (item.SubItems[2].Text == str)
                    {
                        list.Add(item);
                    }
                }
                foreach (ListViewItem item2 in list)
                {
                    this.listViewCaliSet.Items.Remove(item2);
                    int num = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[num].Cells[0].Value = item2.SubItems[1].Text;
                    this.dataGridView1.Rows[num].Cells[0].Tag = item2.Tag;
                    DataGridViewComboBoxCell cell = this.dataGridView1.Rows[num].Cells[1] as DataGridViewComboBoxCell;
                    cell.Items.Clear();
                    cell.Items.Add("校正集");
                    cell.Items.Add("验证集");
                    cell.Value = "校正集";
                    this.dataGridView1.Rows[num].HeaderCell.Value = (num + 1).ToString();
                }
                list.Clear();
                foreach (ListViewItem item3 in this.listViewValiSet.Items)
                {
                    if (item3.SubItems[2].Text == str)
                    {
                        list.Add(item3);
                    }
                }
                foreach (ListViewItem item4 in list)
                {
                    this.listViewValiSet.Items.Remove(item4);
                    int num2 = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[num2].Cells[0].Value = item4.SubItems[1].Text;
                    this.dataGridView1.Rows[num2].Cells[0].Tag = item4.Tag;
                    DataGridViewComboBoxCell cell2 = this.dataGridView1.Rows[num2].Cells[1] as DataGridViewComboBoxCell;
                    cell2.Items.Clear();
                    cell2.Items.Add("校正集");
                    cell2.Items.Add("验证集");
                    cell2.Value = "验证集";
                    this.dataGridView1.Rows[num2].HeaderCell.Value = (num2 + 1).ToString();
                }
                list.Clear();
                for (int i = 0; i < this.cmbClassName.Items.Count; i++)
                {
                    if (this.cmbClassName.Items[i].ToString().Equals(str))
                    {
                        this.cmbClassName.SelectedIndex = i;
                        break;
                    }
                }
                int selectedIndex = this.cmbLoadClass.SelectedIndex;
                this.cmbLoadClass.Items.RemoveAt(selectedIndex);
                this.cmbLoadClass.Text = "";
            }
            if (this.listViewCaliSet.Items.Count > 0)
            {
                this.btnClear.Enabled = true;
                this.btnNextLoadData.Enabled = true;
            }
            else
            {
                this.btnClear.Enabled = false;
                this.btnNextLoadData.Enabled = false;
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Title = " 打开";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Filter = " txt files(*.txt)|*.txt|All files(*.*)|*.*";
            this.openFileDialog1.CheckFileExists = true;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.openFileDialog1.RestoreDirectory = true;
                this.SpectrumOpenPath = this.openFileDialog1.FileNames;
            }
            else
            {
                return;
            }
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = this.SpectrumOpenPath.Length;
            this.progressBar1.Visible = true;
            this.dataGridView1.RowHeadersWidth = 60;
            for (int count = 0; count < this.SpectrumOpenPath.Length; count++)
            {
                EventHandler method = null;
                string strtempt = this.SpectrumOpenPath[count];
                if ((strtempt.Substring(strtempt.LastIndexOf(".") + 1) == "txt") || (strtempt.Substring(strtempt.LastIndexOf(".") + 1) == "TXT"))
                {
                    if (method == null)
                    {
                        method = delegate (object param0, EventArgs param1) {
                            int num = this.dataGridView1.Rows.Add();
                            this.dataGridView1.Rows[num].Cells[0].Value = strtempt.Substring(strtempt.LastIndexOf(@"\") + 1);
                            this.dataGridView1.Rows[num].Cells[0].Tag = strtempt;
                            DataGridViewComboBoxCell cell = this.dataGridView1.Rows[num].Cells[1] as DataGridViewComboBoxCell;
                            cell.Items.Clear();
                            cell.Items.Add("校正集");
                            cell.Items.Add("验证集");
                            cell.Value = "校正集";
                            this.dataGridView1.Rows[num].HeaderCell.Value = (num + 1).ToString();
                            if (count < this.progressBar1.Maximum)
                            {
                                this.progressBar1.Value = count + 1;
                            }
                            else
                            {
                                this.progressBar1.Value = this.progressBar1.Maximum;
                            }
                        };
                    }
                    base.Invoke(method);
                }
            }
            if (this.SpectrumOpenPath.Length > 10)
            {
                this.radioButtonKS.Enabled = true;
                this.radioButtonRandom.Enabled = true;
                this.BtnCalibrationSetOK.Enabled = true;
                this.domainUpDown2.Enabled = true;
            }
            this.progressBar1.Visible = false;
            this.progressBar1.Value = 0;
        }

        private void btnLoadModel_Click(object sender, EventArgs e)
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
                this.bgwPredict.RunWorkerAsync();
            }
        }

        private void btnLoadModelPrd_Click(object sender, EventArgs e)
        {
            if (this.dataGridView2.Rows.Count > 1)
            {
                double d = ((this.domainUpDown2.SelectedIndex + 1) * (this.dataGridView2.Rows.Count - 1)) / 10;
                Math.Floor(d);
                double[] numArray = new double[10];
                double[] numArray2 = new double[10];
                int num2 = this.DataIOmy.TXTReadData(this.dataGridView2.Rows[0].Cells[0].Tag.ToString(), ref numArray, ref numArray2, true);
                double[] numArray3 = new double[0x181];
                double[,] newx = new double[this.dataGridView2.Rows.Count - 1, (int) Math.Ceiling((double) (((double) numArray3.Length) / ((double) this.SpecSkip)))];
                for (int i = 0; i < (this.dataGridView2.Rows.Count - 1); i++)
                {
                    numArray = new double[num2];
                    numArray2 = new double[num2];
                    this.DataIOmy.TXTReadData(this.dataGridView2.Rows[i].Cells[0].Tag.ToString(), ref numArray, ref numArray2, false);
                    int num4 = 0;
                    numArray3 = new double[0x181];
                    for (int j = 0x29; j < 0x6d; j++)
                    {
                        numArray3[num4++] = numArray2[j];
                    }
                    for (int k = 0xa7; k < 0x1e4; k++)
                    {
                        numArray3[num4++] = numArray2[k];
                    }
                    for (int m = 0; m < newx.GetLength(1); m++)
                    {
                        newx[i, m] = numArray3[m * this.SpecSkip];
                    }
                }
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
                string path = "";
                string str2 = "";
                if (fileNames[0].LastIndexOf('-') > 0)
                {
                    path = fileNames[0].Substring(0, fileNames[0].LastIndexOf('-')) + "-SimcaMoel.txt";
                    str2 = fileNames[0].Substring(0, fileNames[0].LastIndexOf('-')) + "-SimcaInfo.txt";
                }
                else
                {
                    path = fileNames[0];
                }
                double[,] data = null;
                if (File.Exists(path))
                {
                    try
                    {
                        this.DataIOmy.TXTReadDatas(path, out data);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            this.DataIOmy.LoadSIMCAModel(path, out data);
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
                ArrayList classlabel = new ArrayList();
                if (File.Exists(str2))
                {
                    this.classLabel.Clear();
                    this.ReadSimcaInfo(str2, ref classlabel);
                }
                else
                {
                    MessageBox.Show("模型配置文件不存在");
                }
                int[] numArray6 = this.simcaPrd.SimcaPrdt(newx, data);
                if (numArray6.Length == newx.GetLength(0))
                {
                    this.listViewPrdResult.Items.Clear();
                    ListViewItem item = new ListViewItem();
                    this.listViewPrdResult.BeginUpdate();
                    for (int n = 0; n < numArray6.Length; n++)
                    {
                        item = new ListViewItem();
                        int num9 = n + 1;
                        item.SubItems.Add(num9.ToString());
                        item.SubItems.Add(this.dataGridView2.Rows[n].Cells[0].Value.ToString());
                        if (classlabel.Count > 0)
                        {
                            item.SubItems.Add(classlabel[numArray6[n]].ToString());
                        }
                        else
                        {
                            item.SubItems.Add(numArray6[n].ToString());
                        }
                        item.SubItems.RemoveAt(0);
                        this.listViewPrdResult.Items.Add(item);
                    }
                    this.listViewPrdResult.EndUpdate();
                }
            }
            else
            {
                MessageBox.Show("请先载入样本");
            }
        }

        private void btnModel_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabControl1.TabPages[this.tabControl1.TabPages.IndexOfKey("tabPageModel")];
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            modelNode node = new modelNode();
            this.modelnode.Push(this.makeNode());
            if (this.modelnodeUndo.Count > 0)
            {
                node = this.modelnodeUndo.Pop();
            }
            if (((node.processStateNode == this.processState) && (node.cmbPCNumSelectedIndex == this.cmbPCNum.SelectedIndex)) && ((node.cmbPreprocessSelectedIndex == this.cmbPreprocess.SelectedIndex) && (node.processCount == this.ProcessCount)))
            {
                this.loadNode(node);
            }
            else
            {
                KeyValuePair<string, double[,]> pair;
                switch (this.processState)
                {
                    case ProcessState.preProcess:
                        base.Enabled = false;
                        this.panelModel.Location = new Point(0, 0);
                        this.panelModel.Visible = true;
                        this.progressBar1.Value = 0;
                        this.progressBar1.Maximum = this.DataCaliSet.Count;
                        this.progressBar1.Visible = true;
                        this.bgwPreProcess.RunWorkerAsync(this.cmbPreprocess.SelectedIndex);
                        return;

                    case ProcessState.crossval:
                        base.Enabled = false;
                        pair = new KeyValuePair<string, double[,]>(this.classLabel[this.ProcessCount].ToString(), this.DataCaliSetPreprocess[this.classLabel[this.ProcessCount].ToString()]);
                        this.bgwCrossVal.RunWorkerAsync(pair);
                        this.txtSimcaInfo.Text = this.txtSimcaInfo.Text + "\r\n对于类别:" + this.classLabel[this.ProcessCount].ToString() + "正在进行一阶交互验证，请稍后。。。";
                        return;

                    case ProcessState.pca:
                        this.panelCrossVal.Visible = false;
                        base.Enabled = false;
                        this.RectangleFMy = this.MyChart.GraphPane.Rect;
                        pair = new KeyValuePair<string, double[,]>(this.classLabel[this.ProcessCount].ToString(), this.DataCaliSetPreprocess[this.classLabel[this.ProcessCount].ToString()]);
                        if (!this.DataCaliSetPCNum.ContainsKey(pair.Key))
                        {
                            this.DataCaliSetPCNum.Add(pair.Key, this.cmbPCNum.SelectedIndex + 1);
                            break;
                        }
                        this.DataCaliSetPCNum[pair.Key] = this.cmbPCNum.SelectedIndex + 1;
                        break;

                    case ProcessState.pcaAfter:
                        if (this.ProcessCount >= (this.classLabel.Count - 1))
                        {
                            this.tabControl1.SelectTab(this.tabPageValidate);
                            if ((this.DataValiSetName.Count > 0) && !this.bgwPredict.IsBusy)
                            {
                                this.bgwPredict.RunWorkerAsync();
                            }
                            return;
                        }
                        this.ProcessCount++;
                        this.processState = ProcessState.crossval;
                        this.MyChartSsq.GraphPane.CurveList.Clear();
                        this.RectangleFMy = this.MyChartSsq.GraphPane.Rect;
                        this.MyChartSsq.GraphPane = new GraphPane(this.RectangleFMy, "Eigenvalue vs. PC Number", "主因子数", "value");
                        this.MyChartSsq.Refresh();
                        this.MyChartSsq.GraphPane.CurveList.Clear();
                        this.RectangleFMy = this.MyChart.GraphPane.Rect;
                        this.MyChart.GraphPane = new GraphPane(this.RectangleFMy, "PRESS Plot", "主因子数", "value");
                        this.MyChart.Refresh();
                        this.listViewPcaSsq.BeginUpdate();
                        this.listViewPcaSsq.Items.Clear();
                        this.listViewPcaSsq.EndUpdate();
                        this.txtSimcaInfo.Text = this.txtSimcaInfo.Text + "\r\n请点击”下一步“对于类别:" + this.classLabel[this.ProcessCount].ToString() + "进行建模";
                        return;

                    default:
                        return;
                }
                this.PreProcessMethod = this.cmbPreprocess.SelectedIndex;
                this.bgwPca.RunWorkerAsync(pair);
                this.txtSimcaInfo.Text = this.txtSimcaInfo.Text + "\r\n对于类别:" + this.classLabel[this.ProcessCount].ToString() + "正在建模，请稍后。。。";
            }
        }

        private void btnNextLoadData_Click(object sender, EventArgs e)
        {
            this.LoadData();
            this.panelPreprocess.Visible = true;
            this.cmbPreprocess.SelectedIndex = 0;
            this.txtSimcaInfo.Text = this.txtSimcaInfo.Text + "\r\n请选择预处理方法";
            this.modelnode.Clear();
            this.modelnodeUndo.Clear();
            this.MyChartSsq.GraphPane.CurveList.Clear();
            this.RectangleFMy = this.MyChartSsq.GraphPane.Rect;
            this.MyChartSsq.GraphPane = new GraphPane(this.RectangleFMy, "Eigenvalue vs. PC Number", "主因子数", "value");
            this.MyChartSsq.Refresh();
            this.MyChartSsq.GraphPane.CurveList.Clear();
            this.RectangleFMy = this.MyChart.GraphPane.Rect;
            this.MyChart.GraphPane = new GraphPane(this.RectangleFMy, "PRESS Plot", "主因子数", "value");
            this.MyChart.Refresh();
            this.listViewPcaSsq.BeginUpdate();
            this.listViewPcaSsq.Items.Clear();
            this.listViewPcaSsq.EndUpdate();
            this.progressBar1.Visible = true;
            this.progressBar1.Maximum = 0;
            if (this.classLabel.Count == 1)
            {
                this.progressBar1.Visible = false;
                this.lblState.Visible = false;
                if (this.classLabel[0].ToString() == "雄")
                {
                    MessageBox.Show("请添加雌性样本！");
                }
                else
                {
                    MessageBox.Show("请添加雄性样本！");
                }
            }
            else
            {
                for (int i = 0; i < this.classLabel.Count; i++)
                {
                    if (this.DataCaliSet[this.classLabel[i].ToString()].GetLength(0) < 0x19)
                    {
                        this.progressBar1.Visible = false;
                        this.lblState.Visible = false;
                        MessageBox.Show("类别：“" + this.classLabel[i].ToString() + "”的校正集样本太少，至少需要25个，请继续添加！");
                        return;
                    }
                    this.progressBar1.Maximum += ((int) Math.Ceiling((double) (((double) this.DataCaliSet[this.classLabel[i].ToString()].GetLength(0)) / 3.0))) - 4;
                }
                this.progressBar1.Maximum += 2;
                this.progressBar1.Minimum = 0;
                this.lblState.Visible = true;
                this.tabControl1.SelectTab(this.tabPageModel);
                if (!this.bgwCrossBySimca.IsBusy)
                {
                    this.bgwCrossBySimca.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("正在建模");
                }
            }
        }

        private void btnOpemFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Title = " 打开";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Filter = " txt files(*.txt)|*.txt|All files(*.*)|*.*";
            this.openFileDialog1.CheckFileExists = true;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.openFileDialog1.RestoreDirectory = true;
                this.SpectrumOpenPath = this.openFileDialog1.FileNames;
            }
            else
            {
                return;
            }
            if (this.SpectrumOpenPath.Length > 0)
            {
                this.Datas = new Data[this.SpectrumOpenPath.Length];
                this.OpenFileName = new string[this.SpectrumOpenPath.Length];
                int num = this.DataIOmy.TXTReadData(this.SpectrumOpenPath[0], ref this.Datas[0].DataX, ref this.Datas[0].DataY, true);
                this.MeanY = new double[this.SpectrumOpenPath.Length, num];
                this.StdErrY = new double[this.SpectrumOpenPath.Length, num];
                for (int i = 0; i < this.SpectrumOpenPath.Length; i++)
                {
                    int num3 = this.SpectrumOpenPath[i].LastIndexOf(@"\");
                    this.OpenFileName[i] = this.SpectrumOpenPath[i].Substring(num3 + 1, (this.SpectrumOpenPath[i].Length - num3) - 1);
                    if (num == this.DataIOmy.TXTReadData(this.SpectrumOpenPath[i], ref this.Datas[i].DataX, ref this.Datas[i].DataY, true))
                    {
                        this.Datas[i].DataX = new double[num];
                        this.Datas[i].DataY = new double[num];
                        this.DataIOmy.TXTReadData(this.SpectrumOpenPath[i], ref this.Datas[i].DataX, ref this.Datas[i].DataY, false);
                    }
                    double[,] x = new double[i + 1, num];
                    for (int j = 0; j < (i + 1); j++)
                    {
                        for (int n = 0; n < num; n++)
                        {
                            x[j, n] = this.Datas[j].DataY[n];
                        }
                    }
                    double[] numArray2 = this.DataHandlingmy.SpMean(x);
                    double[] numArray3 = this.DataHandlingmy.SpStdError(x);
                    for (int k = 0; k < numArray2.Length; k++)
                    {
                        this.MeanY[i, k] = numArray2[k];
                    }
                    for (int m = 0; m < numArray2.Length; m++)
                    {
                        this.StdErrY[i, m] = numArray3[m];
                    }
                }
                this.ViewStylemy = ViewStyle.Spec;
                this.Draw("光谱图", this.Datas, this.SpectrumOpenPath.Length - 1);
            }
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabControl1.TabPages[this.tabControl1.TabPages.IndexOfKey("tabPagePredict")];
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (this.modelnode.Count == 0)
            {
                this.tabControl1.SelectTab(this.tabPageLoadData);
            }
            else
            {
                modelNode item = this.modelnode.Pop();
                this.modelnodeUndo.Push(item);
                this.loadNode(item);
            }
        }

        private void btnSaveModel_Click(object sender, EventArgs e)
        {
            string path = "";
            string str2 = "";
            this.saveFileDialog1.Title = " 打开";
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

        private void btnSaveModelToSystem_Click(object sender, EventArgs e)
        {
            string str = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf(@"\") + 1);
            string path = str + @"Model\SIMCA\model.txt";
            string str3 = str + @"Model\SIMCA\Info.txt";
            try
            {
                if (!Directory.Exists(str + @"Model\SIMCA"))
                {
                    Directory.CreateDirectory(str + @"Model\SIMCA");
                }
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                this.DataIOmy.CreatTXT(path);
                if (File.Exists(str3))
                {
                    File.Delete(str3);
                }
                this.DataIOmy.CreatTXT(str3);
                if ((this.DataIOmy.SaveSIMCAModel(path, this.model) > 0) && (this.WriteSimcaInfo(str3, this.classLabel) > 0))
                {
                    MessageBox.Show("模型保存成功！");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnSImca_Click(object sender, EventArgs e)
        {
            int num = 0;
            int num2 = 2;
            List<double[,]> list = new List<double[,]>();
            new Dictionary<string, List<double[,]>>();
            if (this.Datas.Length > 5)
            {
                double[,] numArray15;
                EventHandler method = null;
                double[,] numArray = new double[this.Datas.Length, this.Datas[0].DataX.Length];
                for (int i = 0; i < this.Datas.Length; i++)
                {
                    for (int num4 = 0; num4 < this.Datas[0].DataX.Length; num4++)
                    {
                        numArray[i, num4] = this.Datas[i].DataY[num4];
                    }
                }
                int[] numArray2 = new int[this.Datas.Length];
                for (int j = 0; j < this.Datas.Length; j++)
                {
                    if (j < 10)
                    {
                        numArray2[j] = 0;
                    }
                    else
                    {
                        numArray2[j] = 1;
                    }
                }
                this.processState = ProcessState.preProcess;
                MWNumericArray x = numArray;
                MWArray[] arrayArray = this.simca.mncn(2, x);
                MWNumericArray array2 = (MWNumericArray) arrayArray[0];
                MWNumericArray means = (MWNumericArray) arrayArray[1];
                double[,] numArray3 = (double[,]) means.ToArray(MWArrayComponent.Real);
                double[,] numArray4 = new double[1, numArray.GetLength(1)];
                for (int k = 0; k < numArray.GetLength(1); k++)
                {
                    numArray4[0, k] = 1.0;
                }
                MWNumericArray stds = numArray4;
                this.processState = ProcessState.crossval;
                MWNumericArray y = new double[numArray.GetLength(1)];
                MWCharArray rm = "pca";
                MWCharArray cvm = null;
                if (numArray.GetLength(0) < 0x15)
                {
                    cvm = "loo";
                }
                else
                {
                    cvm = "con";
                }
                MWNumericArray lv = numArray.GetLength(0) - 1;
                MWNumericArray split = 10;
                MWNumericArray iter = 1;
                MWNumericArray array11 = 0;
                MWArray[] arrayArray2 = this.simca.crossval(4, array2, y, rm, cvm, lv, split, iter, iter, array11);
                MWNumericArray array12 = (MWNumericArray) arrayArray2[0];
                double[,] numArray1 = (double[,]) array12.ToArray(MWArrayComponent.Real);
                MWNumericArray array13 = (MWNumericArray) arrayArray2[1];
                double[,] numArray5 = (double[,]) array13.ToArray(MWArrayComponent.Real);
                PointPairList points = new PointPairList();
                for (int m = 0; m < numArray5.GetLength(1); m++)
                {
                    points.Add((double) m, numArray5[0, m]);
                }
                this.MyChart.GraphPane.CurveList.Clear();
                this.MyChart.GraphPane.XAxis.Title.Text = "主因子数";
                this.MyChart.GraphPane.YAxis.Title.Text = "value";
                this.MyChart.GraphPane.AddCurve("Eigenvalue vs. PC Number", points, Color.Blue, SymbolType.Circle);
                this.MyChart.GraphPane.XAxis.Scale.Min = 0.0;
                this.MyChart.GraphPane.XAxis.Scale.Max = numArray5.GetLength(1);
                this.MyChart.AxisChange();
                this.MyChart.Refresh();
                MWNumericArray plots = -1;
                MWNumericArray scl = 0;
                MWNumericArray array16 = numArray.GetLength(0) - 1;
                MWArray[] arrayArray3 =  this.simca.pca(7, array2, plots, scl, array16);
                MWNumericArray array17 = (MWNumericArray) arrayArray3[2];
                double[,] numArray6 = (double[,]) array17.ToArray(MWArrayComponent.Real);
                ListViewItem listViewItem = new ListViewItem();
                if ((numArray6.GetLength(0) > 2) && (numArray6.GetLength(1) == 4))
                {
                    this.listViewPcaSsq.Items.Clear();
                    for (int num8 = 0; num8 < numArray6.GetLength(0); num8++)
                    {
                        listViewItem = new ListViewItem();
                        listViewItem.SubItems.Clear();
                        int num33 = (int) numArray6[num8, 0];
                        listViewItem.SubItems.Add(num33.ToString());
                        if (numArray6[num8, 1] < 0.001)
                        {
                            listViewItem.SubItems.Add(numArray6[num8, 1].ToString("0.###E+0"));
                        }
                        else
                        {
                            listViewItem.SubItems.Add(numArray6[num8, 1].ToString("f4"));
                        }
                        if (numArray6[num8, 2] < 0.001)
                        {
                            listViewItem.SubItems.Add(numArray6[num8, 2].ToString("0.###E+0"));
                        }
                        else
                        {
                            listViewItem.SubItems.Add(numArray6[num8, 2].ToString("f4"));
                        }
                        listViewItem.SubItems.Add(numArray6[num8, 3].ToString("f2"));
                        listViewItem.SubItems.RemoveAt(0);
                        if (method == null)
                        {
                            method = delegate (object param0, EventArgs param1) {
                                this.listViewPcaSsq.BeginUpdate();
                                this.listViewPcaSsq.Items.Add(listViewItem);
                                this.listViewPcaSsq.EndUpdate();
                            };
                        }
                        this.listViewPcaSsq.Invoke(method);
                    }
                    PointPairList list3 = new PointPairList();
                    for (int num9 = 0; num9 < numArray6.GetLength(0); num9++)
                    {
                        list3.Add((double) num9, numArray6[num9, 2]);
                    }
                    this.MyChartSsq.GraphPane.CurveList.Clear();
                    this.MyChartSsq.GraphPane.XAxis.Title.Text = "主因子数";
                    this.MyChartSsq.GraphPane.YAxis.Title.Text = "value";
                    this.MyChartSsq.GraphPane.AddCurve("Eigenvalue vs. PC Number", list3, Color.Blue, SymbolType.Circle);
                    this.MyChartSsq.GraphPane.XAxis.Scale.Min = 0.0;
                    this.MyChartSsq.GraphPane.XAxis.Scale.Max = numArray6.GetLength(0);
                    this.MyChartSsq.AxisChange();
                    this.MyChartSsq.Refresh();
                }
                MWNumericArray array18 = (MWNumericArray) this.simca.scale(1, x, means, stds)[0];
                double[,] sdata = (double[,]) array18.ToArray(MWArrayComponent.Real);
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
                MWNumericArray array19 = (MWNumericArray) arrayArray3[0];
                double[,] numArray8 = (double[,]) array19.ToArray(MWArrayComponent.Real);
                MWNumericArray array20 = (MWNumericArray) arrayArray3[1];
                double[,] numArray9 = (double[,]) array20.ToArray(MWArrayComponent.Real);
                Matrix matrix2 = new Matrix(numArray9);
                Matrix a = matrix * matrix2;
                Matrix matrix4 = null;
                matrix4 = a * matrix2.Transpose();
                Matrix matrix5 = new Matrix(sdata.GetLength(0), 1);
                for (int n = 0; n < sdata.GetLength(0); n++)
                {
                    double num11 = 0.0;
                    for (int num12 = 0; num12 < sdata.GetLength(1); num12++)
                    {
                        num11 += (sdata[n, num12] - matrix4[n, num12]) * (sdata[n, num12] - matrix4[n, num12]);
                    }
                    matrix5[n, 0] = num11;
                }
                MWNumericArray array21 = (MWNumericArray) arrayArray3[5];
                double[,] numArray10 = (double[,]) array21.ToArray(MWArrayComponent.Real);
                double d = numArray10[0, 0];
                MWNumericArray array22 = (MWNumericArray) arrayArray3[6];
                double[,] numArray11 = (double[,]) array22.ToArray(MWArrayComponent.Real);
                Matrix matrix6 = new Matrix(numArray11);
                MWNumericArray array23 = (MWNumericArray) arrayArray3[3];
                double[,] numArray12 = (double[,]) array23.ToArray(MWArrayComponent.Real);
                Matrix matrix7 = new Matrix(numArray12);
                MWNumericArray array24 = (MWNumericArray) arrayArray3[4];
                double[,] numArray13 = (double[,]) array24.ToArray(MWArrayComponent.Real);
                double num14 = numArray13[0, 0];
                Matrix matrix8 = (Matrix) ((1.0 / d) * matrix6);
                Matrix matrix9 = (Matrix) ((1.0 / num14) * matrix7);
                Matrix matrix10 = new Matrix(matrix8.Row, matrix8.Col + matrix9.Col);
                for (int num15 = 0; num15 < matrix8.Row; num15++)
                {
                    for (int num16 = 0; num16 < matrix8.Col; num16++)
                    {
                        matrix10[num15, num16] = matrix8[num15, num16];
                    }
                    for (int num17 = 0; num17 < matrix9.Col; num17++)
                    {
                        matrix10[num15, matrix8.Col + num17] = matrix8[num15, num17];
                    }
                }
                double[] diagvalue = new double[a.Col];
                for (int num18 = 0; num18 < diagvalue.Length; num18++)
                {
                    diagvalue[num18] = numArray6[num18, 1];
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
                PointPairList list4 = new PointPairList();
                double num19 = -1E+32;
                double num20 = 1E+32;
                double num21 = -1E+32;
                double num22 = 1E+32;
                for (int num23 = 0; num23 < matrix11.Row; num23++)
                {
                    double num24 = Math.Log10(matrix11[num23, 0]);
                    double num25 = Math.Log10(matrix5[num23, 0]);
                    list4.Add(num24, num25);
                    if (num24 > num19)
                    {
                        num19 = num24;
                    }
                    if (num24 < num20)
                    {
                        num20 = num24;
                    }
                    if (num24 > num21)
                    {
                        num21 = num25;
                    }
                    if (num24 < num22)
                    {
                        num22 = num25;
                    }
                }
                PointPairList list5 = new PointPairList();
                for (int num26 = 0; num26 < numArray.GetLength(0); num26++)
                {
                    list5.Add(Math.Log10(matrix11[num26, 0]), Math.Log10(matrix5[num26, 0]));
                    if (list4.Count >= numArray.GetLength(0))
                    {
                        list4.RemoveAt(num26);
                    }
                }
                this.MyChartSsq.GraphPane.CurveList.Clear();
                this.MyChartSsq.GraphPane.XAxis.Title.Text = "Value of T^2(10^x)";
                this.MyChartSsq.GraphPane.YAxis.Title.Text = "value of Q(10^y)";
                this.MyChartSsq.GraphPane.XAxis.Scale.Min = Math.Floor(num20);
                this.MyChartSsq.GraphPane.XAxis.Scale.Max = Math.Ceiling(num19);
                this.MyChartSsq.GraphPane.YAxis.Scale.Min = Math.Floor(num22);
                this.MyChartSsq.GraphPane.YAxis.Scale.Max = Math.Ceiling(num21);
                this.MyChartSsq.GraphPane.AddCurve("other sample", list4, Color.Crimson, SymbolType.Plus).Line.IsVisible = false;
                this.MyChartSsq.GraphPane.AddCurve("class 0 sample", list5, Color.MediumVioletRed, SymbolType.Star).Line.IsVisible = false;
                double num27 = Math.Log10(num14);
                LineItem item = new LineItem(string.Empty, new double[] { this.MyChartSsq.GraphPane.XAxis.Scale.Min, this.MyChartSsq.GraphPane.XAxis.Scale.Max }, new double[] { num27, num27 }, Color.Blue, SymbolType.None);
                this.MyChartSsq.GraphPane.CurveList.Add(item);
                double num28 = Math.Log10(d);
                LineItem item4 = new LineItem(string.Empty, new double[] { num28, num28 }, new double[] { this.MyChartSsq.GraphPane.YAxis.Scale.Min, this.MyChartSsq.GraphPane.YAxis.Scale.Max }, Color.Blue, SymbolType.None);
                this.MyChartSsq.GraphPane.CurveList.Add(item4);
                this.MyChartSsq.AxisChange();
                this.MyChartSsq.Refresh();
                if (num2 == 3)
                {
                    numArray15 = new double[3, numArray.GetLength(1)];
                }
                else
                {
                    numArray15 = new double[2, numArray.GetLength(1)];
                }
                for (int num29 = 0; num29 < numArray.GetLength(1); num29++)
                {
                    numArray15[1, num29] = numArray3[0, num29];
                    if (num2 == 3)
                    {
                        numArray15[2, num29] = numArray4[0, num29];
                    }
                }
                numArray15[0, 0] = DateTime.Today.Year;
                numArray15[0, 1] = DateTime.Today.Month;
                numArray15[0, 2] = DateTime.Today.Day;
                numArray15[0, 3] = DateTime.Today.Hour;
                numArray15[0, 4] = DateTime.Today.Minute;
                numArray15[0, 5] = DateTime.Today.Second;
                numArray15[0, 6] = numArray8.GetLength(0);
                numArray15[0, 7] = numArray.GetLength(1);
                numArray15[0, 8] = 0.0;
                numArray15[0, 9] = num;
                numArray15[0, 10] = 0.0;
                numArray15[0, 11] = num2;
                numArray15[0, 12] = 0.0;
                numArray15[0, 13] = numArray8.GetLength(1);
                numArray15[0, 14] = num14;
                numArray15[0, 15] = d;
                list.Add(numArray15);
                double[,] numArray16 = new double[1, numArray.GetLength(1)];
                for (int num30 = 0; num30 < numArray8.GetLength(1); num30++)
                {
                    numArray16[0, num30] = numArray6[num30, 1];
                }
                list.Add(numArray16);
                numArray16 = new double[numArray9.GetLength(1), numArray9.GetLength(0)];
                for (int num31 = 0; num31 < numArray16.GetLength(0); num31++)
                {
                    for (int num32 = 0; num32 < numArray16.GetLength(1); num32++)
                    {
                        numArray16[num31, num32] = numArray9[num32, num31];
                    }
                }
                list.Add(numArray16);
            }
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabControl1.TabPages[this.tabControl1.TabPages.IndexOfKey("tabPageValidate")];
        }

        private void bwgAbnormalSample_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Settings.Default.thresholdAbnormal = double.Parse(this.txtThresholdAbnormal.Text);
                Settings.Default.Save();
            }
            catch
            {
                MessageBox.Show("输入错误！");
                return;
            }
            double[] numArray = new double[10];
            double[] numArray2 = new double[10];
            double[,] numArray3 = new double[this.dataGridView1.Rows.Count - 1, 385];
            //Ifsize=true,读取数组的长度.
            int num = this.DataIOmy.TXTReadData(this.dataGridView1.Rows[0].Cells[0].Tag.ToString(), ref numArray, ref numArray2, true);
            this.dataYForSelecteSet = new double[this.dataGridView1.Rows.Count - 1, (int) Math.Ceiling((double) (((double) numArray3.GetLength(1)) / ((double) this.SpecSkip)))];
            for (int i = 0; i < (this.dataGridView1.Rows.Count - 1); i++)
            {
                numArray = new double[num];
                numArray2 = new double[num];
                //读取模板文件中的值。
                this.DataIOmy.TXTReadData(this.dataGridView1.Rows[i].Cells[0].Tag.ToString(), ref numArray, ref numArray2, false);
                int num3 = 0;
                //截取 41-109,167-484的总共385个数据。
                for (int j = 41; j < 109; j++)
                {
                    numArray3[i, num3++] = numArray2[j];
                }
                for (int k = 167; k < 484; k++)
                {
                    numArray3[i, num3++] = numArray2[k];
                }
                //每隔m=4个，选一个值进行计算。
                for (int m = 0; m < this.dataYForSelecteSet.GetLength(1); m++)
                {
                    this.dataYForSelecteSet[i, m] = numArray3[i, m * this.SpecSkip];
                }
            }

            int[] numArray4 = new ripsPreDeal().maDistanceAbnormalIndex(this.dataYForSelecteSet, Settings.Default.thresholdAbnormal, out this.md);
            e.Result = numArray4;//返回大于阈值的下标。
        }

        private void bwgAbnormalSample_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void bwgAbnormalSample_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string title = "马氏距离";
            this.RectangleFMy = this.zedGraphControlMDChart.GraphPane.Rect;
            this.zedGraphControlMDChart.GraphPane = new GraphPane(this.RectangleFMy, title, "Value of MD", "value of MD");
            this.zedGraphControlMDChart.Refresh();
            double num = this.DataHandlingmy.MinValue(this.md);
            double num2 = this.DataHandlingmy.MaxValue(this.md);
            this.zedGraphControlMDChart.GraphPane.XAxis.Scale.Min = num;
            this.zedGraphControlMDChart.GraphPane.XAxis.Scale.Max = num2;
            this.zedGraphControlMDChart.GraphPane.YAxis.Scale.Min = num;
            this.zedGraphControlMDChart.GraphPane.YAxis.Scale.Max = num2;
            PointPairList points = new PointPairList();
            for (int i = 0; i < this.md.Length; i++)
            {
                points.Add(this.md[i], this.md[i]);
            }
            this.zedGraphControlMDChart.GraphPane.AddCurve("样本MD值", points, Color.DarkGreen, SymbolType.Circle).Line.IsVisible = false;
            this.zedGraphControlMDChart.AxisChange();
            this.zedGraphControlMDChart.Refresh();
            if (this.abnormalSampleDeleteCount < 5)
            {
                int num4 = this.DataHandlingmy.MaxValueIndex(this.md);
                this.dataGridView1.Rows[num4].DefaultCellStyle.BackColor = Color.LightSalmon;
                this.dataGridView1.Rows[num4].Selected = true;
                this.dataGridView1.Update();
                this.deleteCurrentRow(this.dataGridView1);
                this.abnormalSampleDeleteCount++;
                this.bwgAbnormalSample.RunWorkerAsync();
                this.lblState.Text = "正在剔除异常样本......";
                this.lblState.Update();
            }
            else if (this.abnormalSampleDeleteCount == 5)
            {
                this.abnormalSampleDeleteCount++;
                MessageBox.Show("请根据左侧的马氏距离图填写阈值，点击左下角“选择”按钮，重新识别异常样本并挑选校正集与验证集");
                this.lblState.Visible = false;
            }
            else
            {
                int[] result = (int[]) e.Result;
                string str2 = "";
                if (result.Length > 0)
                {
                    for (int j = 0; j < result.Length; j++)
                    {
                        this.dataGridView1.Rows[result[j]].DefaultCellStyle.BackColor = Color.LightSalmon;
                        str2 = str2 + this.dataGridView1.Rows[result[j]].Cells[0].Value.ToString() + ",";
                    }
                }
                this.dataGridView1.Update();
                if (result.Length > 0)
                {
                    string[] strArray = new string[] { "存在", result.Length.ToString(), "个异常样本", str2, "是否删除？" };
                    if (DialogResult.Yes == MessageBox.Show(string.Concat(strArray), "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1))
                    {
                        for (int m = 0; m < result.Length; m++)
                        {
                            this.dataGridView1.Rows[result[m]].Selected = true;
                        }
                    }
                    this.deleteCurrentRow(this.dataGridView1);
                    double[] numArray2 = new double[10];
                    double[] numArray3 = new double[10];
                    double[,] numArray4 = new double[this.dataGridView1.Rows.Count - 1, 0x181];
                    int num7 = this.DataIOmy.TXTReadData(this.dataGridView1.Rows[0].Cells[0].Tag.ToString(), ref numArray2, ref numArray3, true);
                    this.dataYForSelecteSet = new double[this.dataGridView1.Rows.Count - 1, (int) Math.Ceiling((double) (((double) numArray4.GetLength(1)) / ((double) this.SpecSkip)))];
                    for (int k = 0; k < (this.dataGridView1.Rows.Count - 1); k++)
                    {
                        numArray2 = new double[num7];
                        numArray3 = new double[num7];
                        this.DataIOmy.TXTReadData(this.dataGridView1.Rows[k].Cells[0].Tag.ToString(), ref numArray2, ref numArray3, false);
                        int num9 = 0;
                        for (int n = 0x29; n < 0x6d; n++)
                        {
                            numArray4[k, num9++] = numArray3[n];
                        }
                        for (int num11 = 0xa7; num11 < 0x1e4; num11++)
                        {
                            numArray4[k, num9++] = numArray3[num11];
                        }
                        for (int num12 = 0; num12 < this.dataYForSelecteSet.GetLength(1); num12++)
                        {
                            this.dataYForSelecteSet[k, num12] = numArray4[k, num12 * this.SpecSkip];
                        }
                    }
                }
                this.lblState.Text = "正在进行校正集与验证集的挑选......";
                this.lblState.Update();
                if (this.radioButtonRandom.Checked)
                {
                    this.selectSet(0);
                    Settings.Default.selectSetMethod = 0;
                }
                else if (this.radioButtonKS.Checked)
                {
                    this.selectSet(1);
                    Settings.Default.selectSetMethod = 1;
                }
                //Settings.Default.selectSetCount = this.domainUpDown2.SelectedIndex;
                Settings.Default.selectSetCount = this.domainUpDown2.SelectedIndex;
                Settings.Default.Save();
                this.lblState.Visible = false;
            }
        }

        private void bwgPca_DoWork(object sender, DoWorkEventArgs e)
        {
            KeyValuePair<string, double[,]> argument = (KeyValuePair<string, double[,]>) e.Argument;
            e.Result = this.pcaModel(this.DataCaliSetPCNum[argument.Key], argument);
        }

        private void bwgPca_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void bwgPca_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.MyChart.GraphPane = ((ZedGraphControl) e.Result).GraphPane;
            this.MyChart.Refresh();
            this.processState = ProcessState.pcaAfter;
            if (this.ProcessCount == (this.classLabel.Count - 1))
            {
                int num = 0;
                foreach (KeyValuePair<string, double[,]> pair in this.ModelInfo)
                {
                    num += pair.Value.GetLength(0);
                }
                this.model = new double[num + 2, this.dataWavelength.Length];
                this.model[0, 0] = DateTime.Today.Year;
                this.model[0, 1] = DateTime.Today.Month;
                this.model[0, 2] = DateTime.Today.Day;
                this.model[0, 3] = DateTime.Today.Hour;
                this.model[0, 4] = DateTime.Today.Minute;
                this.model[0, 5] = DateTime.Today.Second;
                this.model[0, 5] = DateTime.Today.Second;
                this.model[0, 6] = this.DataCaliSetAll.GetLength(0);
                this.model[0, 7] = this.DataCaliSetAll.GetLength(1);
                this.model[0, 8] = -1.0;
                this.model[0, 9] = this.classLabel.Count;
                this.model[0, 10] = 0.0;
                this.model[0, 11] = this.PreProcessMethod + 2;
                num = 2;
                string str = "";
                foreach (string str2 in this.classLabel)
                {
                    if (this.classLabel.IndexOf(str2) == 0)
                    {
                        this.model[1, 0] = 3.0;
                    }
                    else
                    {
                        this.model[1, this.classLabel.IndexOf(str2)] = this.model[1, this.classLabel.IndexOf(str)] + this.ModelInfo[str].GetLength(0);
                    }
                    for (int i = 0; i < this.ModelInfo[str2].GetLength(0); i++)
                    {
                        for (int j = 0; j < this.ModelInfo[str2].GetLength(1); j++)
                        {
                            this.model[num, j] = this.ModelInfo[str2][i, j];
                        }
                        num++;
                    }
                    str = str2;
                }
            }
            this.txtSimcaInfo.Text = this.txtSimcaInfo.Text + "\r\n对于类别:" + this.classLabel[this.ProcessCount].ToString() + "模型已建立完毕。";
            base.Enabled = true;
        }

        private void bwgPredict_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = this.Predict();
        }

        private void bwgPredict_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dictionary<string, string> result = (Dictionary<string, string>) e.Result;
            this.tabControl1.SelectTab(this.tabPageValidate);
            this.listViewPredict.Items.Clear();
            this.listViewPredict.BeginUpdate();
            int num = 0;
            int num2 = 0;
            foreach (KeyValuePair<string, string> pair in result)
            {
                num++;
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(num.ToString());
                item.SubItems.Add(pair.Key);
                item.SubItems.Add(this.DataValiSetName[pair.Key]);
                item.SubItems.Add(pair.Value);
                item.SubItems.RemoveAt(0);
                if (this.DataValiSetName[pair.Key] != pair.Value)
                {
                    item.BackColor = Color.Pink;
                    num2++;
                }
                this.listViewPredict.Items.Add(item);
            }
            this.listViewPredict.EndUpdate();
            this.labelPredictResult.Text = "模型预测正确率:" + (((1.0 - (((double) num2) / ((double) result.Count))) * 100.0)).ToString("0.0");
            if (((1.0 - (((double) num2) / ((double) result.Count))) > 0.95) && this.isModelRobust)
            {
                MessageBox.Show("建模成功");
            }
            else if (((1.0 - (((double) num2) / ((double) result.Count))) > 0.95) && (!this.isModelRobust || this.isModelDanger))
            {
                MessageBox.Show("模型有风险");
            }
            else if ((1.0 - (((double) num2) / ((double) result.Count))) < 0.95)
            {
                MessageBox.Show("建模失败，重新挑选样品建模");
                this.tabControl1.SelectedTab = this.tabControl1.TabPages[this.tabControl1.TabPages.IndexOfKey("tabPageLoadData")];
            }
            this.isModelRobust = false;
            this.isModelDanger = false;
        }

        public bool canBeDeleteCurrentRow(DataGridView dataGridView1)
        {
            return (dataGridView1.SelectedRows.Count != 0);
        }

        private void crossval(KeyValuePair<string, double[,]> vk)
        {
            double[,] numArray = vk.Value;
            MWNumericArray x = numArray;
            MWNumericArray y = new double[numArray.GetLength(1)];
            MWCharArray rm = "pca";
            MWCharArray cvm = null;
            if (numArray.GetLength(0) < 0x15)
            {
                cvm = "loo";
            }
            else
            {
                cvm = "con";
            }
            MWNumericArray lv = numArray.GetLength(0) - 1;
            MWNumericArray split = 10;
            MWNumericArray iter = 1;
            MWNumericArray array8 = 0;
            this.crossReslut = this.simca.crossval(4, x, y, rm, cvm, lv, split, iter, iter, array8);
            MWNumericArray array9 = (MWNumericArray) this.crossReslut[0];
            double[,] numArray1 = (double[,]) array9.ToArray(MWArrayComponent.Real);
            MWNumericArray plots = -1;
            MWNumericArray scl = 0;
            MWNumericArray array12 = numArray.GetLength(0) - 1;
            this.pcaReslut =this.simca.pca(7, x, plots, scl, array12);
            MWNumericArray array13 = (MWNumericArray) this.pcaReslut[1];
            double[,] numArray2 = (double[,]) array13.ToArray(MWArrayComponent.Real);
            new ripsPreDeal();
        }

        private int crossvalBySimca(int processCount, BackgroundWorker bw)
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            ZedGraphControl chart1;
            this.ModelByMainFactor.Clear();
            //this.lblState.Invoke(
            //    new Action<object>((obj) => { lblState.Text = "正在对类别：" + this.classLabel[processCount].ToString() + "进行光谱预处理"; })
            ////    delegate (object param0, EventArgs param1) {
            ////    this.lblState.Text = "正在对类别：" + this.classLabel[processCount].ToString() + "进行光谱预处理";
            ////}

            //);
            lblState.Invoke(new Action(() => { lblState.Text = "正在对类别：" + this.classLabel[processCount].ToString() + "进行光谱预处理"; }));

            this.preProcess(0, bw);   //使用0方法预处理
            int num = (int) Math.Ceiling((double) (((double) this.DataCaliSet[this.classLabel[0].ToString()].GetLength(0)) / 4.0));
            bw.ReportProgress((((num - 4) + 1) * processCount) + 1);
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
            int num2 = (int) Math.Ceiling((double) (((double) this.DataCaliSet[this.classLabel[processCount].ToString()].GetLength(0)) / 3.0));
            int num3 = 0,pcNum = 5;

            int[] numArray = new int[num2];
            while (pcNum < num2)
            {
                pcaModel2(pcNum, vk);
                chart1 = this.pcaModel(pcNum, vk);
                if (method == null)
                {
                    method = delegate (object param0, EventArgs param1) {
                        this.MyChart.GraphPane = chart1.GraphPane;
                        this.MyChart.Refresh();
                        this.MyChart.Update();
                    };
                }
                this.MyChart.Invoke(method);
                double[,] numArray2 = this.ModelInfo[this.classLabel[processCount].ToString()];
                if (!this.ModelByMainFactor.ContainsKey(pcNum))
                {
                    this.ModelByMainFactor.Add(pcNum, numArray2);
                }
                else
                {
                    this.ModelByMainFactor[pcNum] = numArray2;
                }
                CurveItem[] itemArray = chart1.GraphPane.CurveList.ToArray();
                double y = itemArray[2].Points[0].Y;
                double x = itemArray[3].Points[0].X;
                num3 = 0;
                for (int j = 0; j < itemArray[0].Points.Count; j++)
                {
                    if ((itemArray[0].Points[j].X < x) && (itemArray[0].Points[j].Y < y))
                    {
                        num3++;
                    }
                }
                numArray[pcNum] = num3;
                bw.ReportProgress((((((num - 4) + 1) * processCount) + 1) + pcNum) - 5);
                if (num3 == 0)
                {
                    break;
                }
                pcNum++;
            }
            if (num3 == 0)
            {
                this.isModelDanger = false;
                if (handler2 == null)
                {
                    handler2 = delegate (object param0, EventArgs param1) {
                        this.lblState.Text = "类别：" + this.classLabel[processCount].ToString() + "建模完毕";
                    };
                }
                this.lblState.Invoke(handler2);
                return pcNum;
            }
            this.isModelDanger = true;
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

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
            FileInfo[] array = new FileInfo[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                array[i] = new FileInfo(data[i]);
            }
            Array.Sort<FileInfo>(array, (x, y) => x.LastWriteTime.CompareTo(y.LastWriteTime));
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = array.Length;
            this.progressBar1.Visible = true;
            int count = 0;
            foreach (FileInfo info in array)
            {
                EventHandler method = null;
                string strtempt = info.FullName;
                if ((strtempt.Substring(strtempt.LastIndexOf(".") + 1) == "txt") || (strtempt.Substring(strtempt.LastIndexOf(".") + 1) == "TXT"))
                {
                    if (method == null)
                    {
                        method = delegate (object param0, EventArgs param1) {
                            int num = this.dataGridView1.Rows.Add();
                            this.dataGridView1.Rows[num].Cells[0].Value = strtempt.Substring(strtempt.LastIndexOf(@"\") + 1);
                            this.dataGridView1.Rows[num].Cells[0].Tag = strtempt;
                            DataGridViewComboBoxCell cell = this.dataGridView1.Rows[num].Cells[1] as DataGridViewComboBoxCell;
                            cell.Items.Clear();
                            cell.Items.Add("校正集");
                            cell.Items.Add("验证集");
                            cell.Value = "校正集";
                            this.dataGridView1.Rows[num].HeaderCell.Value = (num + 1).ToString();
                            if (count < this.progressBar1.Maximum)
                            {
                                this.progressBar1.Value = count++;
                            }
                            else
                            {
                                this.progressBar1.Value = this.progressBar1.Maximum;
                            }
                        };
                    }
                    base.Invoke(method);
                }
                if (this.SpectrumOpenPath.Length > 10)
                {
                    this.radioButtonKS.Enabled = true;
                    this.radioButtonRandom.Enabled = true;
                    this.BtnCalibrationSetOK.Enabled = true;
                    this.domainUpDown2.Enabled = true;
                }
                this.progressBar1.Visible = false;
            }
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.dataGridView1.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                this.dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private void DeleteContext_Click(object sender, EventArgs e)
        {
            if (this.canBeDeleteCurrentRow(this.dataGridView1))
            {
                this.deleteCurrentRow(this.dataGridView1);
            }
        }

        private void DeleteContext1_Click(object sender, EventArgs e)
        {
            if (this.canBeDeleteCurrentRow(this.dataGridView2))
            {
                this.deleteCurrentRow(this.dataGridView2);
            }
        }

        public void deleteCurrentRow(DataGridView dataGridView1)
        {
            DataGridViewRow[] rowArray = new DataGridViewRow[dataGridView1.SelectedRows.Count];
            for (int i = 0; i < rowArray.Length; i++)
            {
                rowArray[i] = dataGridView1.SelectedRows[i];
            }
            if (rowArray.Length == dataGridView1.Rows.Count)
            {
                for (int k = rowArray.Length - 1; k > 0; k--)
                {
                    dataGridView1.Rows.Remove(rowArray[k]);
                }
            }
            else
            {
                for (int m = rowArray.Length; m > 0; m--)
                {
                    dataGridView1.Rows.Remove(rowArray[m - 1]);
                }
            }
            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                dataGridView1.Rows[j].HeaderCell.Value = (j + 1).ToString();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Draw(string str, Data[] DataGet, int Num)
        {
            if ((DataGet != null) && (DataGet[Num].DataX != null))
            {
                RectangleF rect = this.MyChart.GraphPane.Rect;
                if (this.ViewStylemy == ViewStyle.Spec)
                {
                    this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "吸光度");
                    this.MyChart.GraphPane.CurveList.Clear();
                    for (int i = 0; i < (Num + 1); i++)
                    {
                        this.MyChart.Invoke(this.myChartLoadData, new object[] { DataGet[i].DataX, DataGet[i].DataY, this.DrawColor[i], this.OpenFileName[i] });
                    }
                }
            }
            else
            {
                this.MyChart.GraphPane.CurveList.Clear();
            }
        }

        private void Form_offLine_Load(object sender, EventArgs e)
        {
            this.processState = ProcessState.none;
            if (Settings.Default.selectSetMethod == 0)
            {
                this.radioButtonRandom.Checked = true;
            }
            else
            {
                this.radioButtonKS.Checked = true;
            }
            this.domainUpDown2.SelectedIndex = Settings.Default.selectSetCount;
            this.txtThresholdAbnormal.Text = Settings.Default.thresholdAbnormal.ToString("0.0");
            string title = "Q vs.T^2 for all Data Projected on Model of Class";
            this.RectangleFMy = this.MyChart.GraphPane.Rect;
            this.MyChart.GraphPane = new GraphPane(this.RectangleFMy, title, "Value of T^2(10^x)", "value of Q(10^y)");
            this.MyChart.Refresh();
            title = "马氏距离";
            this.RectangleFMy = this.zedGraphControlMDChart.GraphPane.Rect;
            this.zedGraphControlMDChart.GraphPane = new GraphPane(this.RectangleFMy, title, "Value of MD", "value of MD");
            this.zedGraphControlMDChart.Refresh();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOpemFile = new System.Windows.Forms.Button();
            this.btnSImca = new System.Windows.Forms.Button();
            this.MyChart = new ZedGraph.ZedGraphControl();
            this.MyChartSsq = new ZedGraph.ZedGraphControl();
            this.listViewPcaSsq = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelModel = new System.Windows.Forms.Panel();
            this.panelLoadData = new System.Windows.Forms.Panel();
            this.tLpanelLoadData = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbClassName = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtThresholdAbnormal = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.domainUpDown2 = new System.Windows.Forms.DomainUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButtonRandom = new System.Windows.Forms.RadioButton();
            this.radioButtonKS = new System.Windows.Forms.RadioButton();
            this.BtnCalibrationSetOK = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnFileNmae = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Columnpurpose = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteContext = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.zedGraphControlMDChart = new ZedGraph.ZedGraphControl();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnLoadDataReturn = new System.Windows.Forms.Button();
            this.cmbLoadClass = new System.Windows.Forms.ComboBox();
            this.btnNextLoadData = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.listViewValiSet = new System.Windows.Forms.ListView();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.listViewCaliSet = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnNext = new System.Windows.Forms.Button();
            this.txtSimcaInfo = new System.Windows.Forms.TextBox();
            this.cmbPreprocess = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panelPreprocess = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panelCrossVal = new System.Windows.Forms.Panel();
            this.cmbPCNum = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.bgwPreProcess = new System.ComponentModel.BackgroundWorker();
            this.bgwCrossVal = new System.ComponentModel.BackgroundWorker();
            this.bgwPca = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageLoadData = new System.Windows.Forms.TabPage();
            this.tabPageModel = new System.Windows.Forms.TabPage();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.tabPageValidate = new System.Windows.Forms.TabPage();
            this.btnSaveModelToSystem = new System.Windows.Forms.Button();
            this.labelPredictResult = new System.Windows.Forms.Label();
            this.btnSaveModel = new System.Windows.Forms.Button();
            this.btnLoadModel = new System.Windows.Forms.Button();
            this.listViewPredict = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPagePredict = new System.Windows.Forms.TabPage();
            this.btnLoadDataPrd = new System.Windows.Forms.Button();
            this.btnLoadModelPrd = new System.Windows.Forms.Button();
            this.listViewPrdResult = new System.Windows.Forms.ListView();
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel4 = new System.Windows.Forms.Panel();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteContext1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bgwPredict = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.panelbtnDownTip = new System.Windows.Forms.Panel();
            this.btnPredict = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnModel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblState = new System.Windows.Forms.Label();
            this.bgwCrossBySimca = new System.ComponentModel.BackgroundWorker();
            this.bwgAbnormalSample = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelModel.SuspendLayout();
            this.panelLoadData.SuspendLayout();
            this.tLpanelLoadData.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.ContextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelPreprocess.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelCrossVal.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageLoadData.SuspendLayout();
            this.tabPageModel.SuspendLayout();
            this.tabPageValidate.SuspendLayout();
            this.tabPagePredict.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnOpemFile
            // 
            this.btnOpemFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOpemFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpemFile.Location = new System.Drawing.Point(68, 33);
            this.btnOpemFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpemFile.Name = "btnOpemFile";
            this.btnOpemFile.Size = new System.Drawing.Size(95, 42);
            this.btnOpemFile.TabIndex = 0;
            this.btnOpemFile.Text = "打开";
            this.btnOpemFile.UseVisualStyleBackColor = true;
            this.btnOpemFile.Click += new System.EventHandler(this.btnOpemFile_Click);
            // 
            // btnSImca
            // 
            this.btnSImca.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSImca.Location = new System.Drawing.Point(41, 83);
            this.btnSImca.Margin = new System.Windows.Forms.Padding(5);
            this.btnSImca.Name = "btnSImca";
            this.btnSImca.Size = new System.Drawing.Size(164, 42);
            this.btnSImca.TabIndex = 32;
            this.btnSImca.Text = "SIMCA";
            this.btnSImca.UseVisualStyleBackColor = true;
            this.btnSImca.Click += new System.EventHandler(this.btnSImca_Click);
            // 
            // MyChart
            // 
            this.MyChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MyChart.Location = new System.Drawing.Point(5, 32);
            this.MyChart.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MyChart.Name = "MyChart";
            this.MyChart.ScrollGrace = 0D;
            this.MyChart.ScrollMaxX = 0D;
            this.MyChart.ScrollMaxY = 0D;
            this.MyChart.ScrollMaxY2 = 0D;
            this.MyChart.ScrollMinX = 0D;
            this.MyChart.ScrollMinY = 0D;
            this.MyChart.ScrollMinY2 = 0D;
            this.MyChart.Size = new System.Drawing.Size(987, 526);
            this.MyChart.TabIndex = 0;
            // 
            // MyChartSsq
            // 
            this.MyChartSsq.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MyChartSsq.Location = new System.Drawing.Point(80, 62);
            this.MyChartSsq.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MyChartSsq.Name = "MyChartSsq";
            this.MyChartSsq.ScrollGrace = 0D;
            this.MyChartSsq.ScrollMaxX = 0D;
            this.MyChartSsq.ScrollMaxY = 0D;
            this.MyChartSsq.ScrollMaxY2 = 0D;
            this.MyChartSsq.ScrollMinX = 0D;
            this.MyChartSsq.ScrollMinY = 0D;
            this.MyChartSsq.ScrollMinY2 = 0D;
            this.MyChartSsq.Size = new System.Drawing.Size(823, 0);
            this.MyChartSsq.TabIndex = 33;
            // 
            // listViewPcaSsq
            // 
            this.listViewPcaSsq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewPcaSsq.BackColor = System.Drawing.Color.White;
            this.listViewPcaSsq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewPcaSsq.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listViewPcaSsq.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewPcaSsq.GridLines = true;
            this.listViewPcaSsq.Location = new System.Drawing.Point(1298, 14);
            this.listViewPcaSsq.Margin = new System.Windows.Forms.Padding(4);
            this.listViewPcaSsq.Name = "listViewPcaSsq";
            this.listViewPcaSsq.Size = new System.Drawing.Size(27, 628);
            this.listViewPcaSsq.TabIndex = 66;
            this.listViewPcaSsq.UseCompatibleStateImageBehavior = false;
            this.listViewPcaSsq.View = System.Windows.Forms.View.Details;
            this.listViewPcaSsq.Visible = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "主因字数";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader1.Width = 72;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "特征值";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 62;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "方差占比";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 81;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "方差总占比";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 84;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(133, 18);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.MyChart);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.MyChartSsq);
            this.splitContainer1.Size = new System.Drawing.Size(1018, 620);
            this.splitContainer1.SplitterDistance = 574;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 67;
            // 
            // panelModel
            // 
            this.panelModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelModel.Controls.Add(this.splitContainer1);
            this.panelModel.Controls.Add(this.listViewPcaSsq);
            this.panelModel.Location = new System.Drawing.Point(0, 0);
            this.panelModel.Margin = new System.Windows.Forms.Padding(4);
            this.panelModel.Name = "panelModel";
            this.panelModel.Size = new System.Drawing.Size(1338, 642);
            this.panelModel.TabIndex = 68;
            // 
            // panelLoadData
            // 
            this.panelLoadData.AllowDrop = true;
            this.panelLoadData.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panelLoadData.BackColor = System.Drawing.Color.White;
            this.panelLoadData.Controls.Add(this.tLpanelLoadData);
            this.panelLoadData.Location = new System.Drawing.Point(8, -5);
            this.panelLoadData.Margin = new System.Windows.Forms.Padding(4);
            this.panelLoadData.Name = "panelLoadData";
            this.panelLoadData.Size = new System.Drawing.Size(1477, 686);
            this.panelLoadData.TabIndex = 69;
            // 
            // tLpanelLoadData
            // 
            this.tLpanelLoadData.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tLpanelLoadData.ColumnCount = 2;
            this.tLpanelLoadData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.76642F));
            this.tLpanelLoadData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.23357F));
            this.tLpanelLoadData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tLpanelLoadData.Controls.Add(this.panel1, 0, 0);
            this.tLpanelLoadData.Controls.Add(this.panel2, 1, 0);
            this.tLpanelLoadData.Location = new System.Drawing.Point(7, 4);
            this.tLpanelLoadData.Margin = new System.Windows.Forms.Padding(4);
            this.tLpanelLoadData.Name = "tLpanelLoadData";
            this.tLpanelLoadData.RowCount = 1;
            this.tLpanelLoadData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLpanelLoadData.Size = new System.Drawing.Size(1461, 665);
            this.tLpanelLoadData.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cmbClassName);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnLoadFile);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 657);
            this.panel1.TabIndex = 0;
            // 
            // cmbClassName
            // 
            this.cmbClassName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClassName.Font = new System.Drawing.Font("黑体", 14.25F);
            this.cmbClassName.FormattingEnabled = true;
            this.cmbClassName.Items.AddRange(new object[] {
            "雄",
            "雌"});
            this.cmbClassName.Location = new System.Drawing.Point(137, 8);
            this.cmbClassName.Margin = new System.Windows.Forms.Padding(4);
            this.cmbClassName.Name = "cmbClassName";
            this.cmbClassName.Size = new System.Drawing.Size(109, 32);
            this.cmbClassName.TabIndex = 74;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.groupBox1.Controls.Add(this.txtThresholdAbnormal);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.domainUpDown2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.radioButtonRandom);
            this.groupBox1.Controls.Add(this.radioButtonKS);
            this.groupBox1.Controls.Add(this.BtnCalibrationSetOK);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(9, 575);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(444, 78);
            this.groupBox1.TabIndex = 73;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "校正集选择";
            // 
            // txtThresholdAbnormal
            // 
            this.txtThresholdAbnormal.Location = new System.Drawing.Point(347, 45);
            this.txtThresholdAbnormal.Margin = new System.Windows.Forms.Padding(4);
            this.txtThresholdAbnormal.Name = "txtThresholdAbnormal";
            this.txtThresholdAbnormal.Size = new System.Drawing.Size(65, 25);
            this.txtThresholdAbnormal.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(240, 54);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "异常样本阈值:";
            // 
            // domainUpDown2
            // 
            this.domainUpDown2.Enabled = false;
            this.domainUpDown2.Items.Add("10%");
            this.domainUpDown2.Items.Add("20%");
            this.domainUpDown2.Items.Add("30%");
            this.domainUpDown2.Items.Add("40%");
            this.domainUpDown2.Items.Add("50%");
            this.domainUpDown2.Items.Add("60%");
            this.domainUpDown2.Items.Add("70%");
            this.domainUpDown2.Items.Add("80%");
            this.domainUpDown2.Items.Add("90%");
            this.domainUpDown2.Location = new System.Drawing.Point(320, 14);
            this.domainUpDown2.Margin = new System.Windows.Forms.Padding(4);
            this.domainUpDown2.Name = "domainUpDown2";
            this.domainUpDown2.ReadOnly = true;
            this.domainUpDown2.Size = new System.Drawing.Size(96, 25);
            this.domainUpDown2.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(203, 8);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 15);
            this.label6.TabIndex = 13;
            this.label6.Text = "校正集样本数:";
            // 
            // radioButtonRandom
            // 
            this.radioButtonRandom.AutoSize = true;
            this.radioButtonRandom.Enabled = false;
            this.radioButtonRandom.Location = new System.Drawing.Point(8, 50);
            this.radioButtonRandom.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonRandom.Name = "radioButtonRandom";
            this.radioButtonRandom.Size = new System.Drawing.Size(73, 19);
            this.radioButtonRandom.TabIndex = 6;
            this.radioButtonRandom.TabStop = true;
            this.radioButtonRandom.Text = "随机法";
            this.radioButtonRandom.UseVisualStyleBackColor = true;
            this.radioButtonRandom.CheckedChanged += new System.EventHandler(this.radioButtonRandom_CheckedChanged);
            // 
            // radioButtonKS
            // 
            this.radioButtonKS.AutoSize = true;
            this.radioButtonKS.Enabled = false;
            this.radioButtonKS.Location = new System.Drawing.Point(8, 25);
            this.radioButtonKS.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonKS.Name = "radioButtonKS";
            this.radioButtonKS.Size = new System.Drawing.Size(147, 19);
            this.radioButtonKS.TabIndex = 5;
            this.radioButtonKS.TabStop = true;
            this.radioButtonKS.Text = "Kennard-Stone法";
            this.radioButtonKS.UseVisualStyleBackColor = true;
            this.radioButtonKS.CheckedChanged += new System.EventHandler(this.radioButtonKS_CheckedChanged);
            // 
            // BtnCalibrationSetOK
            // 
            this.BtnCalibrationSetOK.Enabled = false;
            this.BtnCalibrationSetOK.Location = new System.Drawing.Point(151, 46);
            this.BtnCalibrationSetOK.Margin = new System.Windows.Forms.Padding(4);
            this.BtnCalibrationSetOK.Name = "BtnCalibrationSetOK";
            this.BtnCalibrationSetOK.Size = new System.Drawing.Size(81, 28);
            this.BtnCalibrationSetOK.TabIndex = 4;
            this.BtnCalibrationSetOK.Text = "选择";
            this.BtnCalibrationSetOK.UseVisualStyleBackColor = true;
            this.BtnCalibrationSetOK.Click += new System.EventHandler(this.BtnCalibrationSetOK_Click);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnLoadFile.Location = new System.Drawing.Point(392, 3);
            this.btnLoadFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(115, 45);
            this.btnLoadFile.TabIndex = 72;
            this.btnLoadFile.Text = "载入样本";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnFileNmae,
            this.Columnpurpose});
            this.dataGridView1.ContextMenuStrip = this.ContextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(17, 55);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(489, 519);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_RowHeaderMouseClick);
            this.dataGridView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dataGridView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragEnter);
            // 
            // ColumnFileNmae
            // 
            this.ColumnFileNmae.HeaderText = "样本名称";
            this.ColumnFileNmae.Name = "ColumnFileNmae";
            this.ColumnFileNmae.ReadOnly = true;
            this.ColumnFileNmae.Width = 200;
            // 
            // Columnpurpose
            // 
            this.Columnpurpose.HeaderText = "用途";
            this.Columnpurpose.Name = "Columnpurpose";
            this.Columnpurpose.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Columnpurpose.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Columnpurpose.Width = 80;
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteContext});
            this.ContextMenuStrip1.Name = "undoContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(109, 28);
            // 
            // DeleteContext
            // 
            this.DeleteContext.Name = "DeleteContext";
            this.DeleteContext.Size = new System.Drawing.Size(108, 24);
            this.DeleteContext.Text = "删除";
            this.DeleteContext.Click += new System.EventHandler(this.DeleteContext_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Location = new System.Drawing.Point(461, 611);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(45, 42);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(-1, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "类别名称";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.zedGraphControlMDChart);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.btnLoadDataReturn);
            this.panel2.Controls.Add(this.cmbLoadClass);
            this.panel2.Controls.Add(this.btnNextLoadData);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.listViewValiSet);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.listViewCaliSet);
            this.panel2.Location = new System.Drawing.Point(526, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(931, 657);
            this.panel2.TabIndex = 1;
            // 
            // zedGraphControlMDChart
            // 
            this.zedGraphControlMDChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControlMDChart.Location = new System.Drawing.Point(5, 4);
            this.zedGraphControlMDChart.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.zedGraphControlMDChart.Name = "zedGraphControlMDChart";
            this.zedGraphControlMDChart.ScrollGrace = 0D;
            this.zedGraphControlMDChart.ScrollMaxX = 0D;
            this.zedGraphControlMDChart.ScrollMaxY = 0D;
            this.zedGraphControlMDChart.ScrollMaxY2 = 0D;
            this.zedGraphControlMDChart.ScrollMinX = 0D;
            this.zedGraphControlMDChart.ScrollMinY = 0D;
            this.zedGraphControlMDChart.ScrollMinY2 = 0D;
            this.zedGraphControlMDChart.Size = new System.Drawing.Size(800, 649);
            this.zedGraphControlMDChart.TabIndex = 75;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Enabled = false;
            this.btnClear.Location = new System.Drawing.Point(500, 604);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(103, 45);
            this.btnClear.TabIndex = 74;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnLoadDataReturn
            // 
            this.btnLoadDataReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadDataReturn.Font = new System.Drawing.Font("黑体", 14.25F);
            this.btnLoadDataReturn.Location = new System.Drawing.Point(5, 610);
            this.btnLoadDataReturn.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadDataReturn.Name = "btnLoadDataReturn";
            this.btnLoadDataReturn.Size = new System.Drawing.Size(44, 41);
            this.btnLoadDataReturn.TabIndex = 73;
            this.btnLoadDataReturn.Text = "<";
            this.btnLoadDataReturn.UseVisualStyleBackColor = true;
            this.btnLoadDataReturn.Click += new System.EventHandler(this.btnLoadDataReturn_Click);
            // 
            // cmbLoadClass
            // 
            this.cmbLoadClass.FormattingEnabled = true;
            this.cmbLoadClass.Location = new System.Drawing.Point(87, 620);
            this.cmbLoadClass.Margin = new System.Windows.Forms.Padding(4);
            this.cmbLoadClass.Name = "cmbLoadClass";
            this.cmbLoadClass.Size = new System.Drawing.Size(196, 23);
            this.cmbLoadClass.TabIndex = 72;
            // 
            // btnNextLoadData
            // 
            this.btnNextLoadData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextLoadData.Enabled = false;
            this.btnNextLoadData.Location = new System.Drawing.Point(656, 604);
            this.btnNextLoadData.Margin = new System.Windows.Forms.Padding(4);
            this.btnNextLoadData.Name = "btnNextLoadData";
            this.btnNextLoadData.Size = new System.Drawing.Size(149, 45);
            this.btnNextLoadData.TabIndex = 71;
            this.btnNextLoadData.Text = "建模";
            this.btnNextLoadData.UseVisualStyleBackColor = true;
            this.btnNextLoadData.Click += new System.EventHandler(this.btnNextLoadData_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(495, 8);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 27);
            this.label3.TabIndex = 3;
            this.label3.Text = "验证集样本";
            // 
            // listViewValiSet
            // 
            this.listViewValiSet.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
            this.listViewValiSet.GridLines = true;
            this.listViewValiSet.Location = new System.Drawing.Point(421, 38);
            this.listViewValiSet.Margin = new System.Windows.Forms.Padding(4);
            this.listViewValiSet.Name = "listViewValiSet";
            this.listViewValiSet.Size = new System.Drawing.Size(376, 559);
            this.listViewValiSet.TabIndex = 2;
            this.listViewValiSet.UseCompatibleStateImageBehavior = false;
            this.listViewValiSet.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "序号";
            this.columnHeader9.Width = 55;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "样本名称";
            this.columnHeader10.Width = 150;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "类别";
            this.columnHeader11.Width = 66;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(16, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "校正集样本";
            // 
            // listViewCaliSet
            // 
            this.listViewCaliSet.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listViewCaliSet.GridLines = true;
            this.listViewCaliSet.Location = new System.Drawing.Point(15, 39);
            this.listViewCaliSet.Margin = new System.Windows.Forms.Padding(4);
            this.listViewCaliSet.Name = "listViewCaliSet";
            this.listViewCaliSet.Size = new System.Drawing.Size(395, 558);
            this.listViewCaliSet.TabIndex = 0;
            this.listViewCaliSet.UseCompatibleStateImageBehavior = false;
            this.listViewCaliSet.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "序号";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "样本名称";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader7.Width = 157;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "类别";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader8.Width = 73;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(1227, 589);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 52);
            this.btnNext.TabIndex = 70;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // txtSimcaInfo
            // 
            this.txtSimcaInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSimcaInfo.BackColor = System.Drawing.Color.White;
            this.txtSimcaInfo.Enabled = false;
            this.txtSimcaInfo.Location = new System.Drawing.Point(1074, 242);
            this.txtSimcaInfo.Margin = new System.Windows.Forms.Padding(4);
            this.txtSimcaInfo.Multiline = true;
            this.txtSimcaInfo.Name = "txtSimcaInfo";
            this.txtSimcaInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSimcaInfo.Size = new System.Drawing.Size(252, 271);
            this.txtSimcaInfo.TabIndex = 71;
            this.txtSimcaInfo.TextChanged += new System.EventHandler(this.txtSimcaInfo_TextChanged);
            // 
            // cmbPreprocess
            // 
            this.cmbPreprocess.FormattingEnabled = true;
            this.cmbPreprocess.Items.AddRange(new object[] {
            "均值中心化",
            "标准化"});
            this.cmbPreprocess.Location = new System.Drawing.Point(119, 25);
            this.cmbPreprocess.Margin = new System.Windows.Forms.Padding(4);
            this.cmbPreprocess.Name = "cmbPreprocess";
            this.cmbPreprocess.Size = new System.Drawing.Size(147, 23);
            this.cmbPreprocess.TabIndex = 72;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(17, 26);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 24);
            this.label4.TabIndex = 73;
            this.label4.Text = "预处理";
            // 
            // panelPreprocess
            // 
            this.panelPreprocess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPreprocess.Controls.Add(this.label4);
            this.panelPreprocess.Controls.Add(this.cmbPreprocess);
            this.panelPreprocess.Location = new System.Drawing.Point(1059, 50);
            this.panelPreprocess.Margin = new System.Windows.Forms.Padding(4);
            this.panelPreprocess.Name = "panelPreprocess";
            this.panelPreprocess.Size = new System.Drawing.Size(272, 62);
            this.panelPreprocess.TabIndex = 74;
            this.panelPreprocess.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.btnSImca);
            this.panel3.Controls.Add(this.btnOpemFile);
            this.panel3.Location = new System.Drawing.Point(1070, 284);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(224, 132);
            this.panel3.TabIndex = 75;
            this.panel3.Visible = false;
            // 
            // panelCrossVal
            // 
            this.panelCrossVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCrossVal.Controls.Add(this.cmbPCNum);
            this.panelCrossVal.Controls.Add(this.label5);
            this.panelCrossVal.Location = new System.Drawing.Point(1059, 139);
            this.panelCrossVal.Margin = new System.Windows.Forms.Padding(4);
            this.panelCrossVal.Name = "panelCrossVal";
            this.panelCrossVal.Size = new System.Drawing.Size(273, 62);
            this.panelCrossVal.TabIndex = 76;
            this.panelCrossVal.Visible = false;
            // 
            // cmbPCNum
            // 
            this.cmbPCNum.FormattingEnabled = true;
            this.cmbPCNum.Items.AddRange(new object[] {
            "均值中心化",
            "标准化"});
            this.cmbPCNum.Location = new System.Drawing.Point(117, 25);
            this.cmbPCNum.Margin = new System.Windows.Forms.Padding(4);
            this.cmbPCNum.Name = "cmbPCNum";
            this.cmbPCNum.Size = new System.Drawing.Size(147, 23);
            this.cmbPCNum.TabIndex = 72;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(9, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 24);
            this.label5.TabIndex = 73;
            this.label5.Text = "主因字数";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(21, 9);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(760, 12);
            this.progressBar1.TabIndex = 76;
            this.progressBar1.Visible = false;
            // 
            // bgwPreProcess
            // 
            this.bgwPreProcess.WorkerReportsProgress = true;
            this.bgwPreProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwPreProcess_DoWork);
            this.bgwPreProcess.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwPreProcess_ProgressChanged);
            this.bgwPreProcess.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwPreProcess_RunWorkerCompleted);
            // 
            // bgwCrossVal
            // 
            this.bgwCrossVal.WorkerReportsProgress = true;
            this.bgwCrossVal.WorkerSupportsCancellation = true;
            this.bgwCrossVal.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwCrossVal_DoWork);
            this.bgwCrossVal.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwCrossVal_ProgressChanged);
            this.bgwCrossVal.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwCrossVal_RunWorkerCompleted);
            // 
            // bgwPca
            // 
            this.bgwPca.WorkerReportsProgress = true;
            this.bgwPca.WorkerSupportsCancellation = true;
            this.bgwPca.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwgPca_DoWork);
            this.bgwPca.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwgPca_ProgressChanged);
            this.bgwPca.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwgPca_RunWorkerCompleted);
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Controls.Add(this.tabPageLoadData);
            this.tabControl1.Controls.Add(this.tabPageModel);
            this.tabControl1.Controls.Add(this.tabPageValidate);
            this.tabControl1.Controls.Add(this.tabPagePredict);
            this.tabControl1.Location = new System.Drawing.Point(4, 125);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1356, 709);
            this.tabControl1.TabIndex = 78;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageLoadData
            // 
            this.tabPageLoadData.BackColor = System.Drawing.Color.White;
            this.tabPageLoadData.Controls.Add(this.panelLoadData);
            this.tabPageLoadData.Location = new System.Drawing.Point(4, 25);
            this.tabPageLoadData.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageLoadData.Name = "tabPageLoadData";
            this.tabPageLoadData.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageLoadData.Size = new System.Drawing.Size(1348, 680);
            this.tabPageLoadData.TabIndex = 0;
            this.tabPageLoadData.Text = "数据";
            // 
            // tabPageModel
            // 
            this.tabPageModel.BackColor = System.Drawing.Color.White;
            this.tabPageModel.Controls.Add(this.panelModel);
            this.tabPageModel.Controls.Add(this.panelPreprocess);
            this.tabPageModel.Controls.Add(this.panelCrossVal);
            this.tabPageModel.Controls.Add(this.txtSimcaInfo);
            this.tabPageModel.Controls.Add(this.btnPrevious);
            this.tabPageModel.Controls.Add(this.btnNext);
            this.tabPageModel.Location = new System.Drawing.Point(4, 25);
            this.tabPageModel.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageModel.Name = "tabPageModel";
            this.tabPageModel.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageModel.Size = new System.Drawing.Size(1348, 680);
            this.tabPageModel.TabIndex = 1;
            this.tabPageModel.Text = "建模";
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevious.Location = new System.Drawing.Point(1071, 589);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 52);
            this.btnPrevious.TabIndex = 77;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // tabPageValidate
            // 
            this.tabPageValidate.BackColor = System.Drawing.Color.White;
            this.tabPageValidate.Controls.Add(this.btnSaveModelToSystem);
            this.tabPageValidate.Controls.Add(this.labelPredictResult);
            this.tabPageValidate.Controls.Add(this.btnSaveModel);
            this.tabPageValidate.Controls.Add(this.panel3);
            this.tabPageValidate.Controls.Add(this.btnLoadModel);
            this.tabPageValidate.Controls.Add(this.listViewPredict);
            this.tabPageValidate.Location = new System.Drawing.Point(4, 25);
            this.tabPageValidate.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageValidate.Name = "tabPageValidate";
            this.tabPageValidate.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageValidate.Size = new System.Drawing.Size(1348, 680);
            this.tabPageValidate.TabIndex = 2;
            this.tabPageValidate.Text = "验证";
            // 
            // btnSaveModelToSystem
            // 
            this.btnSaveModelToSystem.Location = new System.Drawing.Point(1121, 640);
            this.btnSaveModelToSystem.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveModelToSystem.Name = "btnSaveModelToSystem";
            this.btnSaveModelToSystem.Size = new System.Drawing.Size(204, 52);
            this.btnSaveModelToSystem.TabIndex = 77;
            this.btnSaveModelToSystem.Text = "模型保存";
            this.btnSaveModelToSystem.UseVisualStyleBackColor = true;
            this.btnSaveModelToSystem.Click += new System.EventHandler(this.btnSaveModelToSystem_Click);
            // 
            // labelPredictResult
            // 
            this.labelPredictResult.AutoSize = true;
            this.labelPredictResult.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPredictResult.Location = new System.Drawing.Point(1023, 94);
            this.labelPredictResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPredictResult.Name = "labelPredictResult";
            this.labelPredictResult.Size = new System.Drawing.Size(230, 27);
            this.labelPredictResult.TabIndex = 76;
            this.labelPredictResult.Text = "模型预测正确率:";
            // 
            // btnSaveModel
            // 
            this.btnSaveModel.Location = new System.Drawing.Point(1121, 566);
            this.btnSaveModel.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveModel.Name = "btnSaveModel";
            this.btnSaveModel.Size = new System.Drawing.Size(204, 52);
            this.btnSaveModel.TabIndex = 7;
            this.btnSaveModel.Text = "模型另存为";
            this.btnSaveModel.UseVisualStyleBackColor = true;
            this.btnSaveModel.Click += new System.EventHandler(this.btnSaveModel_Click);
            // 
            // btnLoadModel
            // 
            this.btnLoadModel.Location = new System.Drawing.Point(1120, 488);
            this.btnLoadModel.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadModel.Name = "btnLoadModel";
            this.btnLoadModel.Size = new System.Drawing.Size(204, 52);
            this.btnLoadModel.TabIndex = 4;
            this.btnLoadModel.Text = "载入模型";
            this.btnLoadModel.UseVisualStyleBackColor = true;
            this.btnLoadModel.Click += new System.EventHandler(this.btnLoadModel_Click);
            // 
            // listViewPredict
            // 
            this.listViewPredict.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14});
            this.listViewPredict.GridLines = true;
            this.listViewPredict.Location = new System.Drawing.Point(12, 8);
            this.listViewPredict.Margin = new System.Windows.Forms.Padding(4);
            this.listViewPredict.Name = "listViewPredict";
            this.listViewPredict.Size = new System.Drawing.Size(996, 696);
            this.listViewPredict.TabIndex = 3;
            this.listViewPredict.UseCompatibleStateImageBehavior = false;
            this.listViewPredict.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "序号";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "样本名称";
            this.columnHeader12.Width = 326;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "真实类别";
            this.columnHeader13.Width = 184;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "预测类别";
            this.columnHeader14.Width = 160;
            // 
            // tabPagePredict
            // 
            this.tabPagePredict.BackColor = System.Drawing.Color.White;
            this.tabPagePredict.Controls.Add(this.btnLoadDataPrd);
            this.tabPagePredict.Controls.Add(this.btnLoadModelPrd);
            this.tabPagePredict.Controls.Add(this.listViewPrdResult);
            this.tabPagePredict.Controls.Add(this.panel4);
            this.tabPagePredict.Location = new System.Drawing.Point(4, 25);
            this.tabPagePredict.Margin = new System.Windows.Forms.Padding(4);
            this.tabPagePredict.Name = "tabPagePredict";
            this.tabPagePredict.Size = new System.Drawing.Size(1348, 680);
            this.tabPagePredict.TabIndex = 3;
            this.tabPagePredict.Text = "预测";
            // 
            // btnLoadDataPrd
            // 
            this.btnLoadDataPrd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadDataPrd.Location = new System.Drawing.Point(44, 616);
            this.btnLoadDataPrd.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadDataPrd.Name = "btnLoadDataPrd";
            this.btnLoadDataPrd.Size = new System.Drawing.Size(147, 45);
            this.btnLoadDataPrd.TabIndex = 72;
            this.btnLoadDataPrd.Text = "载入样本";
            this.btnLoadDataPrd.UseVisualStyleBackColor = true;
            this.btnLoadDataPrd.Click += new System.EventHandler(this.btnLoadDataPrd_Click);
            // 
            // btnLoadModelPrd
            // 
            this.btnLoadModelPrd.Location = new System.Drawing.Point(343, 666);
            this.btnLoadModelPrd.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadModelPrd.Name = "btnLoadModelPrd";
            this.btnLoadModelPrd.Size = new System.Drawing.Size(149, 45);
            this.btnLoadModelPrd.TabIndex = 5;
            this.btnLoadModelPrd.Text = "载入模型";
            this.btnLoadModelPrd.UseVisualStyleBackColor = true;
            this.btnLoadModelPrd.Click += new System.EventHandler(this.btnLoadModelPrd_Click);
            // 
            // listViewPrdResult
            // 
            this.listViewPrdResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader18});
            this.listViewPrdResult.GridLines = true;
            this.listViewPrdResult.Location = new System.Drawing.Point(572, 4);
            this.listViewPrdResult.Margin = new System.Windows.Forms.Padding(4);
            this.listViewPrdResult.Name = "listViewPrdResult";
            this.listViewPrdResult.Size = new System.Drawing.Size(709, 716);
            this.listViewPrdResult.TabIndex = 4;
            this.listViewPrdResult.UseCompatibleStateImageBehavior = false;
            this.listViewPrdResult.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "序号";
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "样本名称";
            this.columnHeader16.Width = 286;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "预测类别";
            this.columnHeader18.Width = 160;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.dataGridView2);
            this.panel4.Location = new System.Drawing.Point(52, 4);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(431, 613);
            this.panel4.TabIndex = 1;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowDrop = true;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dataGridView2.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridView2.Location = new System.Drawing.Point(1, 9);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(432, 652);
            this.dataGridView2.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "样本名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 260;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteContext1});
            this.contextMenuStrip2.Name = "undoContextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(109, 28);
            // 
            // DeleteContext1
            // 
            this.DeleteContext1.Name = "DeleteContext1";
            this.DeleteContext1.Size = new System.Drawing.Size(108, 24);
            this.DeleteContext1.Text = "删除";
            this.DeleteContext1.Click += new System.EventHandler(this.DeleteContext1_Click);
            // 
            // bgwPredict
            // 
            this.bgwPredict.WorkerReportsProgress = true;
            this.bgwPredict.WorkerSupportsCancellation = true;
            this.bgwPredict.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwgPredict_DoWork);
            this.bgwPredict.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwgPredict_RunWorkerCompleted);
            // 
            // panel6
            // 
            this.panel6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(210)))), ((int)(((byte)(32)))));
            this.panel6.Location = new System.Drawing.Point(0, 109);
            this.panel6.Margin = new System.Windows.Forms.Padding(4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1360, 15);
            this.panel6.TabIndex = 80;
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(136)))), ((int)(((byte)(210)))));
            this.panelHeader.Controls.Add(this.label8);
            this.panelHeader.Controls.Add(this.panel5);
            this.panelHeader.Controls.Add(this.panel6);
            this.panelHeader.Controls.Add(this.btnLoadData);
            this.panelHeader.Controls.Add(this.panelbtnDownTip);
            this.panelHeader.Controls.Add(this.btnPredict);
            this.panelHeader.Controls.Add(this.btnValidate);
            this.panelHeader.Controls.Add(this.btnModel);
            this.panelHeader.Controls.Add(this.btnClose);
            this.panelHeader.Location = new System.Drawing.Point(0, 2);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(4);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1360, 124);
            this.panelHeader.TabIndex = 79;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.label8.Font = new System.Drawing.Font("黑体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(195, 36);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(407, 37);
            this.label8.TabIndex = 81;
            this.label8.Text = "活体雌雄蚕蛹判别系统";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel5.Location = new System.Drawing.Point(1, 2);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(185, 102);
            this.panel5.TabIndex = 69;
            // 
            // btnLoadData
            // 
            this.btnLoadData.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLoadData.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLoadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnLoadData.Location = new System.Drawing.Point(871, 60);
            this.btnLoadData.Margin = new System.Windows.Forms.Padding(5);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(80, 34);
            this.btnLoadData.TabIndex = 70;
            this.btnLoadData.Text = "样品";
            this.btnLoadData.UseVisualStyleBackColor = false;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // panelbtnDownTip
            // 
            this.panelbtnDownTip.BackColor = System.Drawing.Color.Red;
            this.panelbtnDownTip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelbtnDownTip.Location = new System.Drawing.Point(833, 83);
            this.panelbtnDownTip.Margin = new System.Windows.Forms.Padding(4);
            this.panelbtnDownTip.Name = "panelbtnDownTip";
            this.panelbtnDownTip.Size = new System.Drawing.Size(29, 21);
            this.panelbtnDownTip.TabIndex = 72;
            this.panelbtnDownTip.Tag = "panelbtnDownTip";
            // 
            // btnPredict
            // 
            this.btnPredict.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnPredict.BackColor = System.Drawing.Color.Transparent;
            this.btnPredict.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPredict.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPredict.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnPredict.Location = new System.Drawing.Point(1080, 60);
            this.btnPredict.Margin = new System.Windows.Forms.Padding(5);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(80, 35);
            this.btnPredict.TabIndex = 71;
            this.btnPredict.Text = "预测";
            this.btnPredict.UseVisualStyleBackColor = false;
            this.btnPredict.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnValidate.BackColor = System.Drawing.Color.Transparent;
            this.btnValidate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidate.ForeColor = System.Drawing.Color.Red;
            this.btnValidate.Location = new System.Drawing.Point(1184, 60);
            this.btnValidate.Margin = new System.Windows.Forms.Padding(5);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(77, 35);
            this.btnValidate.TabIndex = 63;
            this.btnValidate.Text = "验证";
            this.btnValidate.UseVisualStyleBackColor = false;
            this.btnValidate.Click += new System.EventHandler(this.btnPredict_Click);
            // 
            // btnModel
            // 
            this.btnModel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnModel.BackColor = System.Drawing.Color.Transparent;
            this.btnModel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnModel.Location = new System.Drawing.Point(971, 60);
            this.btnModel.Margin = new System.Windows.Forms.Padding(5);
            this.btnModel.Name = "btnModel";
            this.btnModel.Size = new System.Drawing.Size(80, 34);
            this.btnModel.TabIndex = 67;
            this.btnModel.Text = "建模";
            this.btnModel.UseVisualStyleBackColor = false;
            this.btnModel.Click += new System.EventHandler(this.btnModel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnClose.Location = new System.Drawing.Point(1285, 60);
            this.btnClose.Margin = new System.Windows.Forms.Padding(5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(73, 36);
            this.btnClose.TabIndex = 64;
            this.btnClose.Text = "退出";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(136)))), ((int)(((byte)(210)))));
            this.panel7.Controls.Add(this.lblState);
            this.panel7.Controls.Add(this.progressBar1);
            this.panel7.Location = new System.Drawing.Point(-3, 842);
            this.panel7.Margin = new System.Windows.Forms.Padding(4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1360, 34);
            this.panel7.TabIndex = 81;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(800, 5);
            this.lblState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(71, 15);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "lblstate";
            this.lblState.Visible = false;
            // 
            // bgwCrossBySimca
            // 
            this.bgwCrossBySimca.WorkerReportsProgress = true;
            this.bgwCrossBySimca.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwCrossBySimca_DoWork);
            this.bgwCrossBySimca.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwCrossBySimca_ProgressChanged);
            this.bgwCrossBySimca.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwCrossBySimca_RunWorkerCompleted);
            // 
            // bwgAbnormalSample
            // 
            this.bwgAbnormalSample.WorkerReportsProgress = true;
            this.bwgAbnormalSample.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwgAbnormalSample_DoWork);
            this.bwgAbnormalSample.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwgAbnormalSample_ProgressChanged);
            this.bwgAbnormalSample.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwgAbnormalSample_RunWorkerCompleted);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // Form_offLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1355, 866);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_offLine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "r";
            this.Load += new System.EventHandler(this.Form_offLine_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelModel.ResumeLayout(false);
            this.panelLoadData.ResumeLayout(false);
            this.tLpanelLoadData.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ContextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelPreprocess.ResumeLayout(false);
            this.panelPreprocess.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panelCrossVal.ResumeLayout(false);
            this.panelCrossVal.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageLoadData.ResumeLayout(false);
            this.tabPageModel.ResumeLayout(false);
            this.tabPageModel.PerformLayout();
            this.tabPageValidate.ResumeLayout(false);
            this.tabPageValidate.PerformLayout();
            this.tabPagePredict.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
        }

        private void LoadData()
        {
            foreach (KeyValuePair<string, ArrayList> pair in this.ClassPathCali)
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
                    double[,] numArray5 = new double[numArray3.GetLength(0), (int) Math.Ceiling((double) (((double) numArray4.GetLength(1)) / ((double) this.SpecSkip)))];
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
            foreach (KeyValuePair<string, ArrayList> pair2 in this.ClassPathVali)
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
                            double[] numArray9 = new double[(int) Math.Ceiling((double) (((double) numArray8.Length) / ((double) this.SpecSkip)))];
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

        private void loadNode(modelNode node)
        {
            this.ProcessCount = node.processCount;
            this.MyChart.GraphPane = node.mychartPane;
            this.MyChart.Refresh();
            this.MyChartSsq.GraphPane = node.mychartSsqPane;
            this.MyChartSsq.Refresh();
            this.cmbPreprocess.SelectedIndex = node.cmbPreprocessSelectedIndex;
            this.cmbPCNum.SelectedIndex = node.cmbPCNumSelectedIndex;
            this.processState = node.processStateNode;
            this.panelCrossVal.Visible = node.panelCrossvalVisible;
            this.panelPreprocess.Visible = node.panelPreprocessVisible;
            this.txtSimcaInfo.Text = node.Info;
            if ((node.listviewPcaSsqItems != null) && (node.listviewPcaSsqItems.Length > 0))
            {
                this.listViewPcaSsq.BeginUpdate();
                this.listViewPcaSsq.Items.Clear();
                for (int i = 0; i < node.listviewPcaSsqItems.Length; i++)
                {
                    this.listViewPcaSsq.Items.Add(node.listviewPcaSsqItems[i]);
                }
                this.listViewPcaSsq.EndUpdate();
            }
        }

        private modelNode makeNode()
        {
            modelNode node = new modelNode {
                mychartPane = this.MyChart.GraphPane,
                mychartSsqPane = this.MyChartSsq.GraphPane,
                processCount = this.ProcessCount,
                processStateNode = this.processState,
                cmbPreprocessSelectedIndex = this.cmbPreprocess.SelectedIndex,
                cmbPCNumSelectedIndex = this.cmbPCNum.SelectedIndex,
                panelCrossvalVisible = this.panelCrossVal.Visible,
                panelPreprocessVisible = this.panelPreprocess.Visible,
                Info = this.txtSimcaInfo.Text
            };
            if (this.listViewPcaSsq.Items.Count > 0)
            {
                node.listviewPcaSsqItems = new ListViewItem[this.listViewPcaSsq.Items.Count];
                for (int i = 0; i < this.listViewPcaSsq.Items.Count; i++)
                {
                    node.listviewPcaSsqItems[i] = this.listViewPcaSsq.Items[i];
                }
            }
            return node;
        }

        private void MyChart_LoadData(ZedGraphControl myChart, double[] DataX, double[] DataY, Color ColorMy, string Name)
        {
            PointPairList points = new PointPairList(DataX, DataY);
            myChart.GraphPane.AddCurve(Name, points, ColorMy, SymbolType.None);
            myChart.GraphPane.XAxis.Scale.Min = this.DataHandlingmy.MinValue(DataX);
            myChart.GraphPane.XAxis.Scale.Max = this.DataHandlingmy.MaxValue(DataX);
            myChart.AxisChange();
            myChart.Refresh();
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
            PointPairList points = new PointPairList();
            double num10 = -1E+32;
            double num11 = 1E+32;
            double num12 = -1E+32;
            double num13 = 1E+32;
            for (int m = 0; m < matrix11.Row; m++)
            {
                double x = Math.Log10(matrix11[m, 0]);
                double y = Math.Log10(matrix5[m, 0]);
                points.Add(x, y);
                if (x > num10)
                {
                    num10 = x;
                }
                if (x < num11)
                {
                    num11 = x;
                }
                if (y > num12)
                {
                    num12 = y;
                }
                if (y < num13)
                {
                    num13 = y;
                }
            }
        }

        private ZedGraphControl pcaModel(int pcNum, KeyValuePair<string, double[,]> vk)
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
            MWNumericArray array8 =  (MWNumericArray) this.simca.scale(1, dataCaliSetAll, means, stds)[0];
            double[,] sdata = (double[,]) array8.ToArray(MWArrayComponent.Real);
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
            MWNumericArray array9 = (MWNumericArray) arrayArray[0];
            double[,] numArray3 = (double[,]) array9.ToArray(MWArrayComponent.Real);
            MWNumericArray array10 = (MWNumericArray) arrayArray[1];
            double[,] numArray4 = (double[,]) array10.ToArray(MWArrayComponent.Real);
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
            MWNumericArray array11 = (MWNumericArray) arrayArray[5];
            double[,] numArray5 = (double[,]) array11.ToArray(MWArrayComponent.Real);
            double d = numArray5[0, 0];
            MWNumericArray array12 = (MWNumericArray) arrayArray[6];
            double[,] numArray6 = (double[,]) array12.ToArray(MWArrayComponent.Real);
            Matrix matrix6 = new Matrix(numArray6);
            MWNumericArray array13 = (MWNumericArray) arrayArray[3];
            double[,] numArray7 = (double[,]) array13.ToArray(MWArrayComponent.Real);
            Matrix matrix7 = new Matrix(numArray7);
            MWNumericArray array14 = (MWNumericArray) arrayArray[4];
            double[,] numArray8 = (double[,]) array14.ToArray(MWArrayComponent.Real);
            double num5 = numArray8[0, 0];
            MWNumericArray array15 = (MWNumericArray) arrayArray[2];
            double[,] numArray9 = (double[,]) array15.ToArray(MWArrayComponent.Real);
            Matrix matrix8 = (Matrix) ((1.0 / d) * matrix6);
            Matrix matrix9 = (Matrix) ((1.0 / num5) * matrix7);
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
            PointPairList points = new PointPairList();
            double num10 = -1E+32;
            double num11 = 1E+32;
            double num12 = -1E+32;
            double num13 = 1E+32;
            for (int m = 0; m < matrix11.Row; m++)
            {
                double x = Math.Log10(matrix11[m, 0]);
                double y = Math.Log10(matrix5[m, 0]);
                points.Add(x, y);
                if (x > num10)
                {
                    num10 = x;
                }
                if (x < num11)
                {
                    num11 = x;
                }
                if (y > num12)
                {
                    num12 = y;
                }
                if (y < num13)
                {
                    num13 = y;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            PointPairList list2 = new PointPairList();
            PointPair[] array = new PointPair[points.Count];
            points.CopyTo(array);
            for (int n = 0; n < this.DataCaliSetAllIndex[vk.Key].Length; n++)
            {
                int num18 = this.DataCaliSetAllIndex[vk.Key][n];
                double num19 = Math.Log10(matrix11[num18, 0]);
                double num20 = Math.Log10(matrix5[num18, 0]);
                list2.Add(num19, num20);
                for (int num21 = 0; num21 < array.Length; num21++)
                {
                    if ((array[num21].X == num19) && (array[num21].Y == num20))
                    {
                        points.Remove(array[num21]);
                    }
                }
            }
            string title = "Q vs.T^2 for all Data Projected on Model of Class  " + vk.Key;
            ZedGraphControl control = new ZedGraphControl {
                GraphPane = new GraphPane(this.RectangleFMy, title, "Value of T^2(10^x)", "value of Q(10^y)")
            };
            control.GraphPane.XAxis.Scale.Min = Math.Floor(num11);
            control.GraphPane.XAxis.Scale.Max = Math.Ceiling(num10);
            control.GraphPane.YAxis.Scale.Min = Math.Floor(num13);
            control.GraphPane.YAxis.Scale.Max = Math.Ceiling(num12);
            control.GraphPane.AddCurve("other sample", points, Color.DarkGreen, SymbolType.Plus).Line.IsVisible = false;
            control.GraphPane.AddCurve("class " + vk.Key + " sample", list2, Color.MediumVioletRed, SymbolType.Star).Line.IsVisible = false;
            double num22 = Math.Log10(num5);
            LineItem item = new LineItem(string.Empty, new double[] { control.GraphPane.XAxis.Scale.Min, control.GraphPane.XAxis.Scale.Max }, new double[] { num22, num22 }, Color.Blue, SymbolType.None);
            control.GraphPane.CurveList.Add(item);
            double num23 = Math.Log10(d);
            LineItem item4 = new LineItem(string.Empty, new double[] { num23, num23 }, new double[] { control.GraphPane.YAxis.Scale.Min, control.GraphPane.YAxis.Scale.Max }, Color.Blue, SymbolType.None);
            control.GraphPane.CurveList.Add(item4);
            control.AxisChange();
            control.Refresh();
            if (this.PreProcessMethod == 1)
            {
                numArray11 = new double[3, numArray.GetLength(1)];
            }
            else
            {
                numArray11 = new double[2, numArray.GetLength(1)];
            }
            for (int num24 = 0; num24 < numArray.GetLength(1); num24++)
            {
                numArray11[1, num24] = this.DataCaliSetPreprocessMx[vk.Key][num24];
                if (this.PreProcessMethod == 1)
                {
                    numArray11[2, num24] = this.DataCaliSetPreprocessSx[vk.Key][num24];
                }
            }
            numArray11[0, 0] = DateTime.Today.Year;
            numArray11[0, 1] = DateTime.Today.Month;
            numArray11[0, 2] = DateTime.Today.Day;
            numArray11[0, 3] = DateTime.Today.Hour;
            numArray11[0, 4] = DateTime.Today.Minute;
            numArray11[0, 5] = DateTime.Today.Second;
            numArray11[0, 6] = numArray3.GetLength(0);
            numArray11[0, 7] = numArray.GetLength(1);
            numArray11[0, 8] = 0.0;
            numArray11[0, 9] = this.classLabel.IndexOf(vk.Key);
            numArray11[0, 10] = 0.0;
            numArray11[0, 11] = this.PreProcessMethod + 2;
            numArray11[0, 12] = 0.0;
            numArray11[0, 13] = numArray3.GetLength(1);
            numArray11[0, 14] = num5;
            numArray11[0, 15] = d;
            List<double[,]> list3 = new List<double[,]> {
                numArray11
            };
            double[,] numArray12 = new double[1, numArray.GetLength(1)];
            for (int num25 = 0; num25 < numArray3.GetLength(1); num25++)
            {
                numArray12[0, num25] = numArray9[num25, 1];
            }
            list3.Add(numArray12);
            numArray12 = new double[numArray4.GetLength(1), numArray4.GetLength(0)];
            for (int num26 = 0; num26 < numArray12.GetLength(0); num26++)
            {
                for (int num27 = 0; num27 < numArray12.GetLength(1); num27++)
                {
                    numArray12[num26, num27] = numArray4[num27, num26];
                }
            }
            list3.Add(numArray12);
            int num28 = 0;
            for (int num29 = 0; num29 < list3.Count; num29++)
            {
                num28 += list3[num29].GetLength(0);
            }
            double[,] numArray13 = new double[num28, numArray.GetLength(1)];
            num28 = 0;
            for (int num30 = 0; num30 < list3.Count; num30++)
            {
                for (int num31 = 0; num31 < list3[num30].GetLength(0); num31++)
                {
                    for (int num32 = 0; num32 < list3[num30].GetLength(1); num32++)
                    {
                        numArray13[num28, num32] = list3[num30][num31, num32];
                    }
                    num28++;
                }
            }
            if (this.ModelInfo.ContainsKey(vk.Key))
            {
                this.ModelInfo[vk.Key] = numArray13;
                return control;
            }
            this.ModelInfo.Add(vk.Key, numArray13);
            return control;
        }

        private Dictionary<string, string> Predict()
        {
            SimcaPrd prd = new SimcaPrd();
            int count = this.DataValiSetName.Count;
            int length = 0;
            List<string> list = new List<string>(this.DataValiSet.Keys);
            length = this.DataValiSet[list[0]].GetLength(0);
            string[] strArray = new string[count];
            double[,] newx = new double[count, length];
            for (int i = 0; i < list.Count; i++)
            {
                strArray[i] = list[i];
                for (int j = 0; j < length; j++)
                {
                    newx[i, j] = this.DataValiSet[list[i]][j];
                }
            }
            int[] numArray2 = prd.SimcaPrdt(newx, this.model);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (numArray2.Length == list.Count)
            {
                for (int k = 0; k < numArray2.Length; k++)
                {
                    dictionary.Add(list[k], this.classLabel[numArray2[k]].ToString());
                }
            }
            return dictionary;
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
            foreach (KeyValuePair<string, double[,]> pair2 in this.DataCaliSet)
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
                    MWArray[] arrayArray = this.simca.mncn(2, x);
                    MWNumericArray array2 = (MWNumericArray) arrayArray[0];
                    double[,] numArray2 = (double[,]) array2.ToArray(MWArrayComponent.Real);
                    MWNumericArray array3 = (MWNumericArray) arrayArray[1];
                    double[,] numArray3 = (double[,]) array3.ToArray(MWArrayComponent.Real);
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
                    MWArray[] arrayArray2 = this.simca.auto(3, x);
                    MWNumericArray array4 = (MWNumericArray) arrayArray2[0];
                    double[,] numArray6 = (double[,]) array4.ToArray(MWArrayComponent.Real);
                    MWNumericArray array5 = (MWNumericArray) arrayArray2[1];
                    double[,] numArray7 = (double[,]) array5.ToArray(MWArrayComponent.Real);
                    MWNumericArray array6 = (MWNumericArray) arrayArray2[2];
                    double[,] numArray8 = (double[,]) array6.ToArray(MWArrayComponent.Real);
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
                percentProgress++;
                bw.ReportProgress(percentProgress);
            }
        }

        private void radioButtonKS_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.selectSetMethod = 1;
            Settings.Default.Save();
        }

        private void radioButtonRandom_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.selectSetMethod = 0;
            Settings.Default.Save();
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

        private void selectSet(int selectSetMethod)
        {
            if (this.dataGridView1.Rows.Count > 10)
            {
                int[] numArray2;
                int[] numArray3;
                double d = ((this.domainUpDown2.SelectedIndex + 1) * (this.dataGridView1.Rows.Count - 1)) / 10;
                int nC = (int) Math.Floor(d);
                double[,] x = new double[this.dataYForSelecteSet.GetLength(0), this.dataYForSelecteSet.GetLength(1)];
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    for (int m = 0; m < x.GetLength(1); m++)
                    {
                        x[i, m] = this.dataYForSelecteSet[i, m];
                    }
                }
                ripsPreDeal deal = new ripsPreDeal(x.GetLength(0), x.GetLength(1));
                if (selectSetMethod == 0)
                {
                    deal.RandomSet(nC, x, out numArray2, out numArray3);
                }
                else
                {
                    deal.KennardStone(nC, x, out numArray2, out numArray3);
                }
                for (int j = 0; j < numArray2.Length; j++)
                {
                    this.dataGridView1.Rows[numArray2[j]].Cells[1].Value = "校正集";
                }
                for (int k = 0; k < numArray3.Length; k++)
                {
                    this.dataGridView1.Rows[numArray3[k]].Cells[1].Value = "验证集";
                }
            }
        }

        [DllImport("Shlwapi.dll")]
        private static extern int StrCmpLogicalW(string psz1, string psz2);
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = this.tabControl1.SelectedTab.Name;
            if (name != null)
            {
                if (!(name == "tabPageLoadData"))
                {
                    if (!(name == "tabPageModel"))
                    {
                        if (!(name == "tabPagePredict"))
                        {
                            if (name == "tabPageValidate")
                            {
                                this.panelbtnDownTip.Location = new Point(this.btnPredict.Location.X + ((this.btnPredict.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
                            }
                            return;
                        }
                        this.panelbtnDownTip.Location = new Point(this.btnValidate.Location.X + ((this.btnValidate.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
                        return;
                    }
                }
                else
                {
                    this.panelbtnDownTip.Location = new Point(this.btnLoadData.Location.X + ((this.btnLoadData.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
                    return;
                }
                this.panelbtnDownTip.Location = new Point(this.btnModel.Location.X + ((this.btnModel.Size.Width - this.panelbtnDownTip.Size.Width) / 2), this.panelbtnDownTip.Location.Y);
            }
        }

        private void txtSimcaInfo_TextChanged(object sender, EventArgs e)
        {
            this.txtSimcaInfo.SelectionStart = this.txtSimcaInfo.Text.Length - 1;
            this.txtSimcaInfo.ScrollToCaret();
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

        [StructLayout(LayoutKind.Sequential)]
        public struct Data
        {
            public double[] DataX;
            public double[] DataY;
            public double[] DataE;
        }

        private delegate void DrawDelegate(ZedGraphControl myChart, string Str, Form_offLine.Data[] DataGet, int Num);

        [StructLayout(LayoutKind.Sequential)]
        public struct modelNode
        {
            public GraphPane mychartPane;
            public GraphPane mychartSsqPane;
            public Form_offLine.ProcessState processStateNode;
            public int cmbPreprocessSelectedIndex;
            public bool panelPreprocessVisible;
            public int cmbPCNumSelectedIndex;
            public bool panelCrossvalVisible;
            public int processCount;
            public string Info;
            public ListViewItem[] listviewPcaSsqItems;
        }

        private delegate void MyChartLoadData(double[] DataX, double[] DataY, Color ColorMy, string Name);

        public enum ProcessState
        {
            none,
            loadData,
            preProcess,
            crossval,
            pca,
            pcaAfter
        }

        private enum ViewStyle
        {
            Mean,
            StdErr,
            Spec,
            Energy
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}

