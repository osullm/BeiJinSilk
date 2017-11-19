namespace JSDU
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using ZedGraph;

    public class Form_offLine : Form
    {
        private Button bntOriginView;
        private Button btnMeanView;
        private Button btnOpemFile;
        private Button btnSave;
        private Button btnStdErrView;
        private ComboBox comboBox1;
        private IContainer components = null;
        private JSDU.Spectrometer.Data[] Data = null;
        private DataHandling DataHandlingmy = new DataHandling();
        private DataIO DataIOmy = new DataIO();
        private Color[] DrawColor = new Color[50];
        private GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private double[,] MeanY;
        private ZedGraphControl MyChart;
        private MyChartLoadData myChartLoadData;
        private Spectrometer MySpectrometer = new Spectrometer();
        private OpenFileDialog openFileDialog1;
        private string[] OpenFileName = null;
        private string[] SpectrumOpenPath;
        private SplitContainer splitContainer1;
        private double[,] StdErrY = null;
        private ToolTip toolTip1;
        private TextBox txtMaxStdErr;
        private TextBox txtMeanErr;
        private ViewStyle ViewStylemy = ViewStyle.Spec;

        public Form_offLine()
        {
            this.InitializeComponent();
            this.InitDelegate();
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

        private void bntOriginView_Click(object sender, EventArgs e)
        {
            this.ViewStylemy = ViewStyle.Spec;
            this.ReDraw(sender, e);
        }

        private void btnMeanView_Click(object sender, EventArgs e)
        {
            DrawDelegate method = new DrawDelegate(this.Draw);
            int selectedIndex = this.comboBox1.SelectedIndex;
            JSDU.Spectrometer.Data[] dataArray = new JSDU.Spectrometer.Data[1];
            if (this.Data != null)
            {
                dataArray[0].DataX = this.Data[selectedIndex].DataX;
                dataArray[0].DataY = new double[dataArray[0].DataX.Length];
                for (int i = 0; i < dataArray[0].DataX.Length; i++)
                {
                    dataArray[0].DataY[i] = this.MeanY[selectedIndex, i];
                }
                this.ViewStylemy = ViewStyle.Mean;
                base.BeginInvoke(method, new object[] { "平均谱图", dataArray, 0 });
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
                this.comboBox1.Items.Clear();
                if ((this.SpectrumOpenPath.Length > 0) && (this.SpectrumOpenPath.Length < 0x33))
                {
                    this.Data = new JSDU.Spectrometer.Data[this.SpectrumOpenPath.Length];
                    this.OpenFileName = new string[this.SpectrumOpenPath.Length];
                    int num = this.DataIOmy.TXTReadData(this.SpectrumOpenPath[0], ref this.Data[0].DataX, ref this.Data[0].DataY, true);
                    this.MeanY = new double[this.SpectrumOpenPath.Length, num];
                    this.StdErrY = new double[this.SpectrumOpenPath.Length, num];
                    for (int i = 0; i < this.SpectrumOpenPath.Length; i++)
                    {
                        int num3 = this.SpectrumOpenPath[i].LastIndexOf(@"\");
                        this.OpenFileName[i] = this.SpectrumOpenPath[i].Substring(num3 + 1, (this.SpectrumOpenPath[i].Length - num3) - 1);
                        if (num == this.DataIOmy.TXTReadData(this.SpectrumOpenPath[i], ref this.Data[i].DataX, ref this.Data[i].DataY, true))
                        {
                            this.Data[i].DataX = new double[num];
                            this.Data[i].DataY = new double[num];
                            this.DataIOmy.TXTReadData(this.SpectrumOpenPath[i], ref this.Data[i].DataX, ref this.Data[i].DataY, false);
                        }
                        double[,] x = new double[i + 1, num];
                        for (int j = 0; j < (i + 1); j++)
                        {
                            for (int n = 0; n < num; n++)
                            {
                                x[j, n] = this.Data[j].DataY[n];
                            }
                        }
                        double[] numArray2 = this.DataHandlingmy.SpMean(x);
                        double[] array = this.DataHandlingmy.SpStdError(x);
                        for (int k = 0; k < numArray2.Length; k++)
                        {
                            this.MeanY[i, k] = numArray2[k];
                        }
                        for (int m = 0; m < numArray2.Length; m++)
                        {
                            this.StdErrY[i, m] = array[m];
                        }
                        this.txtMeanErr.Text = this.DataHandlingmy.MeanValue(array).ToString("0.000e0");
                        this.txtMaxStdErr.Text = this.DataHandlingmy.MaxValue(array).ToString("0.000e0");
                        int num9 = i + 1;
                        this.comboBox1.Items.Add(num9.ToString());
                        this.comboBox1.SelectedIndex = i;
                    }
                    DrawDelegate method = new DrawDelegate(this.Draw);
                    if (this.DataHandlingmy.MaxValue(this.Data[0].DataY) > 100.0)
                    {
                        this.ViewStylemy = ViewStyle.Energy;
                        base.BeginInvoke(method, new object[] { "能量图", this.Data, this.comboBox1.SelectedIndex });
                    }
                    else
                    {
                        this.ViewStylemy = ViewStyle.Spec;
                        base.BeginInvoke(method, new object[] { "光谱图", this.Data, this.comboBox1.SelectedIndex });
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int num2;
            int selectedIndex = this.comboBox1.SelectedIndex;
            string str = "";
            double[] numArray = null;
            if (this.ViewStylemy == ViewStyle.Mean)
            {
                str = "保存平均谱图";
                numArray = new double[this.MeanY.GetLength(1)];
                for (num2 = 0; num2 < this.MeanY.GetLength(1); num2++)
                {
                    numArray[num2] = this.MeanY[selectedIndex, num2];
                }
            }
            else if (this.ViewStylemy == ViewStyle.StdErr)
            {
                str = "保存标准差谱图";
                numArray = new double[this.MeanY.GetLength(1)];
                for (num2 = 0; num2 < this.MeanY.GetLength(1); num2++)
                {
                    numArray[num2] = this.StdErrY[selectedIndex, num2];
                }
            }
            SaveFileDialog dialog = new SaveFileDialog {
                InitialDirectory = Spectrometer.SavePath,
                Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*",
                RestoreDirectory = true,
                FilterIndex = 1,
                Title = str
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                this.DataIOmy.TXTSaveData(fileName, this.Data[selectedIndex].DataX, numArray);
            }
        }

        private void btnStdErrView_Click(object sender, EventArgs e)
        {
            DrawDelegate method = new DrawDelegate(this.Draw);
            int selectedIndex = this.comboBox1.SelectedIndex;
            JSDU.Spectrometer.Data[] dataArray = new JSDU.Spectrometer.Data[1];
            if (this.Data != null)
            {
                dataArray[0].DataX = this.Data[selectedIndex].DataX;
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
                if (this.ViewStylemy == ViewStyle.Spec)
                {
                    this.ReDraw(sender, e);
                }
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

        private void Draw(string str, JSDU.Spectrometer.Data[] DataGet, int Num)
        {
            if ((DataGet != null) && (DataGet[Num].DataX != null))
            {
                int num;
                if (this.ViewStylemy == ViewStyle.Spec)
                {
                    this.MyChart.GraphPane.Title.Text = str;
                    this.MyChart.GraphPane.XAxis.Title.Text = "波长(nm)";
                    this.MyChart.GraphPane.YAxis.Title.Text = "吸光度";
                    this.MyChart.GraphPane.CurveList.Clear();
                    for (num = 0; num < (Num + 1); num++)
                    {
                        this.MyChart_LoadData(DataGet[num].DataX, DataGet[num].DataY, this.DrawColor[num], this.OpenFileName[num]);
                    }
                }
                else if ((this.ViewStylemy == ViewStyle.Mean) && (Num == 0))
                {
                    this.MyChart.GraphPane.Title.Text = str;
                    this.MyChart.GraphPane.XAxis.Title.Text = "波长(nm)";
                    this.MyChart.GraphPane.YAxis.Title.Text = "吸光度";
                    this.MyChart.GraphPane.CurveList.Clear();
                    for (num = 0; num < (Num + 1); num++)
                    {
                        int num2 = this.comboBox1.SelectedIndex + 1;
                        this.MyChart_LoadData(DataGet[num].DataX, DataGet[num].DataY, this.DrawColor[num], "平均光谱-" + num2.ToString());
                    }
                }
                else if ((this.ViewStylemy == ViewStyle.StdErr) && (Num == 0))
                {
                    this.MyChart.GraphPane.Title.Text = str;
                    this.MyChart.GraphPane.XAxis.Title.Text = "波长(nm)";
                    this.MyChart.GraphPane.YAxis.Title.Text = "";
                    this.MyChart.GraphPane.CurveList.Clear();
                    for (num = 0; num < (Num + 1); num++)
                    {
                        this.MyChart_LoadData(DataGet[num].DataX, DataGet[num].DataY, this.DrawColor[num], "标准差图-" + ((this.comboBox1.SelectedIndex + 1)).ToString());
                    }
                }
                else if (this.ViewStylemy == ViewStyle.Energy)
                {
                    this.MyChart.GraphPane.Title.Text = str;
                    this.MyChart.GraphPane.XAxis.Title.Text = "波长(nm)";
                    this.MyChart.GraphPane.YAxis.Title.Text = "";
                    this.MyChart.GraphPane.CurveList.Clear();
                    for (num = 0; num < (Num + 1); num++)
                    {
                        this.MyChart.Invoke(this.myChartLoadData, new object[] { DataGet[num].DataX, DataGet[num].DataY, this.DrawColor[num], this.OpenFileName[num] });
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
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        private static extern int GetPixel(IntPtr hdc, Point p);
        public void InitDelegate()
        {
            this.myChartLoadData = new MyChartLoadData(this.MyChart_LoadData);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MyChart = new ZedGraph.ZedGraphControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMeanErr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMaxStdErr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.bntOriginView = new System.Windows.Forms.Button();
            this.btnStdErrView = new System.Windows.Forms.Button();
            this.btnMeanView = new System.Windows.Forms.Button();
            this.btnOpemFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MyChart
            // 
            this.MyChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MyChart.Location = new System.Drawing.Point(5, 12);
            this.MyChart.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MyChart.Name = "MyChart";
            this.MyChart.ScrollGrace = 0D;
            this.MyChart.ScrollMaxX = 0D;
            this.MyChart.ScrollMaxY = 0D;
            this.MyChart.ScrollMaxY2 = 0D;
            this.MyChart.ScrollMinX = 0D;
            this.MyChart.ScrollMinY = 0D;
            this.MyChart.ScrollMinY2 = 0D;
            this.MyChart.Size = new System.Drawing.Size(815, 742);
            this.MyChart.TabIndex = 0;
            this.MyChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MyChart_MouseMove);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.Controls.Add(this.MyChart);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer1.Panel2.Controls.Add(this.bntOriginView);
            this.splitContainer1.Panel2.Controls.Add(this.btnStdErrView);
            this.splitContainer1.Panel2.Controls.Add(this.btnMeanView);
            this.splitContainer1.Panel2.Controls.Add(this.btnOpemFile);
            this.splitContainer1.Size = new System.Drawing.Size(1067, 768);
            this.splitContainer1.SplitterDistance = 824;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.groupBox1.Controls.Add(this.txtMeanErr);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtMaxStdErr);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(-4, 224);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.groupBox1.Size = new System.Drawing.Size(222, 150);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标准差";
            // 
            // txtMeanErr
            // 
            this.txtMeanErr.Enabled = false;
            this.txtMeanErr.Location = new System.Drawing.Point(100, 98);
            this.txtMeanErr.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtMeanErr.Name = "txtMeanErr";
            this.txtMeanErr.Size = new System.Drawing.Size(112, 37);
            this.txtMeanErr.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 101);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 27);
            this.label3.TabIndex = 20;
            this.label3.Text = "平均：";
            // 
            // txtMaxStdErr
            // 
            this.txtMaxStdErr.Enabled = false;
            this.txtMaxStdErr.Location = new System.Drawing.Point(100, 41);
            this.txtMaxStdErr.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtMaxStdErr.Name = "txtMaxStdErr";
            this.txtMaxStdErr.Size = new System.Drawing.Size(112, 37);
            this.txtMaxStdErr.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 27);
            this.label2.TabIndex = 18;
            this.label2.Text = "最大：";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(34, 580);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 70);
            this.btnSave.TabIndex = 30;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.comboBox1.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(164, 484);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(53, 32);
            this.comboBox1.TabIndex = 29;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // bntOriginView
            // 
            this.bntOriginView.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bntOriginView.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bntOriginView.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bntOriginView.Location = new System.Drawing.Point(-66, 476);
            this.bntOriginView.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.bntOriginView.Name = "bntOriginView";
            this.bntOriginView.Size = new System.Drawing.Size(49, 42);
            this.bntOriginView.TabIndex = 28;
            this.bntOriginView.Text = "<-";
            this.bntOriginView.UseVisualStyleBackColor = true;
            this.bntOriginView.Click += new System.EventHandler(this.bntOriginView_Click);
            // 
            // btnStdErrView
            // 
            this.btnStdErrView.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnStdErrView.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStdErrView.Font = new System.Drawing.Font("黑体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStdErrView.Location = new System.Drawing.Point(-4, 509);
            this.btnStdErrView.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnStdErrView.Name = "btnStdErrView";
            this.btnStdErrView.Size = new System.Drawing.Size(164, 42);
            this.btnStdErrView.TabIndex = 27;
            this.btnStdErrView.Text = "标准差图";
            this.btnStdErrView.UseVisualStyleBackColor = true;
            this.btnStdErrView.Click += new System.EventHandler(this.btnStdErrView_Click);
            // 
            // btnMeanView
            // 
            this.btnMeanView.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnMeanView.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMeanView.Font = new System.Drawing.Font("黑体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMeanView.Location = new System.Drawing.Point(-4, 442);
            this.btnMeanView.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnMeanView.Name = "btnMeanView";
            this.btnMeanView.Size = new System.Drawing.Size(164, 42);
            this.btnMeanView.TabIndex = 26;
            this.btnMeanView.Text = "平均光谱";
            this.btnMeanView.UseVisualStyleBackColor = true;
            this.btnMeanView.Click += new System.EventHandler(this.btnMeanView_Click);
            // 
            // btnOpemFile
            // 
            this.btnOpemFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOpemFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpemFile.Font = new System.Drawing.Font("黑体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpemFile.Location = new System.Drawing.Point(3, 66);
            this.btnOpemFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpemFile.Name = "btnOpemFile";
            this.btnOpemFile.Size = new System.Drawing.Size(215, 56);
            this.btnOpemFile.TabIndex = 0;
            this.btnOpemFile.Text = "打开";
            this.btnOpemFile.UseVisualStyleBackColor = true;
            this.btnOpemFile.Click += new System.EventHandler(this.btnOpemFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form_offLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1067, 768);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form_offLine";
            this.Text = "离线查看谱图窗口";
            this.Load += new System.EventHandler(this.Form_offLine_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

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

        private void MyChart_MouseMove(object sender, MouseEventArgs e)
        {
            int num;
            this.toolTip1.UseFading = false;
            Color color = Color.FromArgb(0xff, 0xff, 0xff);
            Point point2 = base.PointToScreen(this.MyChart.Location);
            Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
            for (num = 0; num < 10; num++)
            {
                p = new Point(Control.MousePosition.X, Control.MousePosition.Y + num);
                int pixel = GetPixel(GetDC(new IntPtr(0)), p);
                int red = pixel & 0xff;
                int green = (pixel & 0xff00) / 0x100;
                int blue = (pixel & 0xff0000) / 0x10000;
                if (((red != 0xff) || (green != 0xff)) || (blue != 0xff))
                {
                    color = Color.FromArgb(red, green, blue);
                    break;
                }
                p = new Point(Control.MousePosition.X, Control.MousePosition.Y - num);
                pixel = GetPixel(GetDC(new IntPtr(0)), p);
                red = pixel & 0xff;
                green = (pixel & 0xff00) / 0x100;
                blue = (pixel & 0xff0000) / 0x10000;
                if (((red != 0xff) || (green != 0xff)) || (blue != 0xff))
                {
                    color = Color.FromArgb(red, green, blue);
                    break;
                }
            }
            int index = -1;
            for (num = 0; num < this.MyChart.GraphPane.CurveList.Count; num++)
            {
                if (color.ToArgb() == this.MyChart.GraphPane.CurveList[num].Color.ToArgb())
                {
                    index = num;
                }
            }
            if (index > -1)
            {
                if (index < this.OpenFileName.Length)
                {
                    this.toolTip1.Show(this.OpenFileName[index], this.MyChart);
                }
            }
            else
            {
                this.toolTip1.Hide(this.MyChart);
            }
        }

        private void ReDraw(object sender, EventArgs e)
        {
            DrawDelegate delegate2;
            int num;
            if (this.ViewStylemy == ViewStyle.Spec)
            {
                delegate2 = new DrawDelegate(this.Draw);
                num = (this.comboBox1.SelectedIndex < 0) ? 0 : this.comboBox1.SelectedIndex;
                if (this.Data.Length > 0)
                {
                    base.BeginInvoke(delegate2, new object[] { "光谱图", this.Data, num });
                }
                JSDU.Spectrometer.Data[] dataArray = new JSDU.Spectrometer.Data[1];
                dataArray[0].DataX = this.Data[num].DataX;
                dataArray[0].DataY = new double[dataArray[0].DataX.Length];
                for (int i = 0; i < dataArray[0].DataX.Length; i++)
                {
                    dataArray[0].DataY[i] = this.StdErrY[num, i];
                }
                this.txtMeanErr.Text = this.DataHandlingmy.MeanValue(dataArray[0].DataY).ToString("0.000e0");
                this.txtMaxStdErr.Text = this.DataHandlingmy.MaxValue(dataArray[0].DataY).ToString("0.000e0");
            }
            else if (this.ViewStylemy == ViewStyle.Energy)
            {
                delegate2 = new DrawDelegate(this.Draw);
                num = (this.comboBox1.SelectedIndex < 0) ? 0 : this.comboBox1.SelectedIndex;
                if (this.Data != null)
                {
                    base.BeginInvoke(delegate2, new object[] { "能量图", this.Data, num });
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

