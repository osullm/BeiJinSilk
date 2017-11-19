// Decompiled with JetBrains decompiler
// Type: JSDU.Form_offLine
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace JSDU
{
  public class Form_offLine : Form
  {
    private DataIO DataIOmy = new DataIO();
    private DataHandling DataHandlingmy = new DataHandling();
    private Spectrometer MySpectrometer = new Spectrometer();
    private Form_offLine.ViewStyle ViewStylemy = Form_offLine.ViewStyle.Spec;
    private Color[] DrawColor = new Color[20];
    private Spectrometer.Data[] Data = (Spectrometer.Data[]) null;
    private double[,] StdErrY = (double[,]) null;
    private string[] OpenFileName = (string[]) null;
    private IContainer components = (IContainer) null;
    private string[] SpectrumOpenPath;
    private double[,] MeanY;
    private Form_offLine.MyChartLoadData myChartLoadData;
    private ZedGraphControl MyChart;
    private SplitContainer splitContainer1;
    private Button btnOpemFile;
    private OpenFileDialog openFileDialog1;
    private Button btnSave;
    private ComboBox comboBox1;
    private Button bntOriginView;
    private Button btnStdErrView;
    private Button btnMeanView;
    private GroupBox groupBox1;
    private TextBox txtMeanErr;
    private System.Windows.Forms.Label label3;
    private TextBox txtMaxStdErr;
    private System.Windows.Forms.Label label2;

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
      this.DrawColor[16] = Color.DarkRed;
      this.DrawColor[17] = Color.DarkSlateBlue;
      this.DrawColor[18] = Color.ForestGreen;
      this.DrawColor[19] = Color.Indigo;
    }

    public void InitDelegate()
    {
      this.myChartLoadData = new Form_offLine.MyChartLoadData(this.MyChart_LoadData);
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

    private void Form_offLine_Load(object sender, EventArgs e)
    {
      this.MySpectrometer.ReadSetParameters();
    }

    private void btnOpemFile_Click(object sender, EventArgs e)
    {
      this.openFileDialog1.Title = " 打开";
      this.openFileDialog1.Multiselect = true;
      this.openFileDialog1.Filter = " txt files(*.txt)|*.txt|All files(*.*)|*.*";
      this.openFileDialog1.CheckFileExists = true;
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.openFileDialog1.RestoreDirectory = true;
      this.SpectrumOpenPath = this.openFileDialog1.FileNames;
      this.comboBox1.Items.Clear();
      if (this.SpectrumOpenPath.Length <= 0 || this.SpectrumOpenPath.Length >= 21)
        return;
      this.Data = new Spectrometer.Data[this.SpectrumOpenPath.Length];
      this.OpenFileName = new string[this.SpectrumOpenPath.Length];
      int length = this.DataIOmy.TXTReadData(this.SpectrumOpenPath[0], ref this.Data[0].DataX, ref this.Data[0].DataY, true);
      this.MeanY = new double[this.SpectrumOpenPath.Length, length];
      this.StdErrY = new double[this.SpectrumOpenPath.Length, length];
      for (int index1 = 0; index1 < this.SpectrumOpenPath.Length; ++index1)
      {
        int num1 = this.SpectrumOpenPath[index1].LastIndexOf("\\");
        this.OpenFileName[index1] = this.SpectrumOpenPath[index1].Substring(num1 + 1, this.SpectrumOpenPath[index1].Length - num1 - 1);
        if (length == this.DataIOmy.TXTReadData(this.SpectrumOpenPath[index1], ref this.Data[index1].DataX, ref this.Data[index1].DataY, true))
        {
          this.Data[index1].DataX = new double[length];
          this.Data[index1].DataY = new double[length];
          this.DataIOmy.TXTReadData(this.SpectrumOpenPath[index1], ref this.Data[index1].DataX, ref this.Data[index1].DataY, false);
        }
        double[,] X = new double[index1 + 1, length];
        for (int index2 = 0; index2 < index1 + 1; ++index2)
        {
          for (int index3 = 0; index3 < length; ++index3)
            X[index2, index3] = this.Data[index2].DataY[index3];
        }
        double[] numArray = this.DataHandlingmy.SpMean(X);
        double[] array = this.DataHandlingmy.SpStdError(X);
        for (int index2 = 0; index2 < numArray.Length; ++index2)
          this.MeanY[index1, index2] = numArray[index2];
        for (int index2 = 0; index2 < numArray.Length; ++index2)
          this.StdErrY[index1, index2] = array[index2];
        TextBox txtMeanErr = this.txtMeanErr;
        double num2 = this.DataHandlingmy.MeanValue(array);
        string str1 = num2.ToString("0.000e0");
        txtMeanErr.Text = str1;
        TextBox txtMaxStdErr = this.txtMaxStdErr;
        num2 = this.DataHandlingmy.MaxValue(array);
        string str2 = num2.ToString("0.000e0");
        txtMaxStdErr.Text = str2;
        this.comboBox1.Items.Add((object) (index1 + 1).ToString());
        this.comboBox1.SelectedIndex = index1;
      }
      Form_offLine.DrawDelegate drawDelegate = new Form_offLine.DrawDelegate(this.Draw);
      this.ViewStylemy = Form_offLine.ViewStyle.Spec;
      this.BeginInvoke((Delegate) drawDelegate, (object) "光谱图", (object) this.Data, (object) this.comboBox1.SelectedIndex);
    }

    private void btnMeanView_Click(object sender, EventArgs e)
    {
      Form_offLine.DrawDelegate drawDelegate = new Form_offLine.DrawDelegate(this.Draw);
      int selectedIndex = this.comboBox1.SelectedIndex;
      Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
      if (this.Data == null)
        return;
      dataArray[0].DataX = this.Data[selectedIndex].DataX;
      dataArray[0].DataY = new double[dataArray[0].DataX.Length];
      for (int index = 0; index < dataArray[0].DataX.Length; ++index)
        dataArray[0].DataY[index] = this.MeanY[selectedIndex, index];
      this.ViewStylemy = Form_offLine.ViewStyle.Mean;
      this.BeginInvoke((Delegate) drawDelegate, (object) "平均谱图", (object) dataArray, (object) 0);
    }

    private void btnStdErrView_Click(object sender, EventArgs e)
    {
      Form_offLine.DrawDelegate drawDelegate = new Form_offLine.DrawDelegate(this.Draw);
      int selectedIndex = this.comboBox1.SelectedIndex;
      Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
      if (this.Data == null)
        return;
      dataArray[0].DataX = this.Data[selectedIndex].DataX;
      dataArray[0].DataY = new double[dataArray[0].DataX.Length];
      for (int index = 0; index < dataArray[0].DataX.Length; ++index)
        dataArray[0].DataY[index] = this.StdErrY[selectedIndex, index];
      this.ViewStylemy = Form_offLine.ViewStyle.StdErr;
      this.BeginInvoke((Delegate) drawDelegate, (object) "标准差图", (object) dataArray, (object) 0);
      this.txtMeanErr.Text = this.DataHandlingmy.MeanValue(dataArray[0].DataY).ToString("0.000e0");
      this.txtMaxStdErr.Text = this.DataHandlingmy.MaxValue(dataArray[0].DataY).ToString("0.000e0");
    }

    private void bntOriginView_Click(object sender, EventArgs e)
    {
      this.ViewStylemy = Form_offLine.ViewStyle.Spec;
      this.ReDraw(sender, e);
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.comboBox1.Items.Count <= 0)
        return;
      if (this.ViewStylemy == Form_offLine.ViewStyle.StdErr)
        this.btnStdErrView_Click(sender, e);
      if (this.ViewStylemy == Form_offLine.ViewStyle.Mean)
        this.btnMeanView_Click(sender, e);
      if (this.ViewStylemy == Form_offLine.ViewStyle.Spec)
        this.ReDraw(sender, e);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      int selectedIndex = this.comboBox1.SelectedIndex;
      if (this.ViewStylemy == Form_offLine.ViewStyle.Mean)
      {
        double[] Data_y = new double[this.MeanY.GetLength(1)];
        for (int index = 0; index < this.MeanY.GetLength(1); ++index)
          Data_y[index] = this.MeanY[selectedIndex, index];
        this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\Mean-" + this.comboBox1.SelectedItem.ToString() + ".txt", this.Data[selectedIndex].DataX, Data_y);
        int num = (int) MessageBox.Show("平均光谱：" + Spectrometer.SavePath + "Mean-" + this.comboBox1.SelectedItem.ToString() + ".txt保存成功");
      }
      else
      {
        if (this.ViewStylemy != Form_offLine.ViewStyle.StdErr)
          return;
        double[] Data_y = new double[this.MeanY.GetLength(1)];
        for (int index = 0; index < this.MeanY.GetLength(1); ++index)
          Data_y[index] = this.StdErrY[selectedIndex, index];
        this.DataIOmy.TXTSaveData(Spectrometer.SavePath + "\\StdErr-" + this.comboBox1.SelectedItem.ToString() + ".txt", this.Data[selectedIndex].DataX, Data_y);
        int num = (int) MessageBox.Show("标准差图：" + Spectrometer.SavePath + "\\StdErr-" + this.comboBox1.SelectedItem.ToString() + ".txt保存成功");
      }
    }

    private void Draw(string str, Spectrometer.Data[] DataGet, int Num)
    {
      if (DataGet != null && DataGet[Num].DataX != null)
      {
        RectangleF rect = this.MyChart.GraphPane.Rect;
        if (this.ViewStylemy == Form_offLine.ViewStyle.Spec)
        {
          this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "吸光度");
          this.MyChart.GraphPane.CurveList.Clear();
          for (int index = 0; index < Num + 1; ++index)
            this.MyChart.Invoke((Delegate) this.myChartLoadData, (object) DataGet[index].DataX, (object) DataGet[index].DataY, (object) this.DrawColor[index], (object) this.OpenFileName[index]);
        }
        else if (this.ViewStylemy == Form_offLine.ViewStyle.Mean && Num == 0)
        {
          this.MyChart.GraphPane = new GraphPane(rect, str, "波数", "吸光度");
          this.MyChart.GraphPane.CurveList.Clear();
          for (int index = 0; index < Num + 1; ++index)
            this.MyChart.Invoke((Delegate) this.myChartLoadData, (object) DataGet[index].DataX, (object) DataGet[index].DataY, (object) this.DrawColor[index], (object) ("平均光谱-" + (this.comboBox1.SelectedIndex + 1).ToString()));
        }
        else
        {
          if (this.ViewStylemy != Form_offLine.ViewStyle.StdErr || Num != 0)
            return;
          this.MyChart.GraphPane = new GraphPane(rect, str, "波数", " ");
          this.MyChart.GraphPane.CurveList.Clear();
          for (int index = 0; index < Num + 1; ++index)
            this.MyChart.Invoke((Delegate) this.myChartLoadData, (object) DataGet[index].DataX, (object) DataGet[index].DataY, (object) this.DrawColor[index], (object) ("标准差图-" + (this.comboBox1.SelectedIndex + 1).ToString()));
        }
      }
      else
        this.MyChart.GraphPane.CurveList.Clear();
    }

    private void ReDraw(object sender, EventArgs e)
    {
      if (this.ViewStylemy == Form_offLine.ViewStyle.Spec)
      {
        Form_offLine.DrawDelegate drawDelegate = new Form_offLine.DrawDelegate(this.Draw);
        int index1 = this.comboBox1.SelectedIndex < 0 ? 0 : this.comboBox1.SelectedIndex;
        if (this.Data.Length > 0)
          this.BeginInvoke((Delegate) drawDelegate, (object) "光谱图", (object) this.Data, (object) index1);
        Spectrometer.Data[] dataArray = new Spectrometer.Data[1];
        dataArray[0].DataX = this.Data[index1].DataX;
        dataArray[0].DataY = new double[dataArray[0].DataX.Length];
        for (int index2 = 0; index2 < dataArray[0].DataX.Length; ++index2)
          dataArray[0].DataY[index2] = this.StdErrY[index1, index2];
        TextBox txtMeanErr = this.txtMeanErr;
        double num = this.DataHandlingmy.MeanValue(dataArray[0].DataY);
        string str1 = num.ToString("0.000e0");
        txtMeanErr.Text = str1;
        TextBox txtMaxStdErr = this.txtMaxStdErr;
        num = this.DataHandlingmy.MaxValue(dataArray[0].DataY);
        string str2 = num.ToString("0.000e0");
        txtMaxStdErr.Text = str2;
      }
      else if (this.ViewStylemy == Form_offLine.ViewStyle.Energy)
      {
        Form_offLine.DrawDelegate drawDelegate = new Form_offLine.DrawDelegate(this.Draw);
        int num = this.comboBox1.SelectedIndex < 0 ? 0 : this.comboBox1.SelectedIndex;
        if (this.Data == null)
          return;
        this.BeginInvoke((Delegate) drawDelegate, (object) "能量图", (object) this.Data, (object) num);
      }
      else if (this.ViewStylemy == Form_offLine.ViewStyle.Mean)
      {
        this.btnMeanView_Click(sender, e);
      }
      else
      {
        if (this.ViewStylemy != Form_offLine.ViewStyle.StdErr)
          return;
        this.btnStdErrView_Click(sender, e);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_offLine));
      this.MyChart = new ZedGraphControl();
      this.splitContainer1 = new SplitContainer();
      this.groupBox1 = new GroupBox();
      this.txtMeanErr = new TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtMaxStdErr = new TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnSave = new Button();
      this.comboBox1 = new ComboBox();
      this.bntOriginView = new Button();
      this.btnStdErrView = new Button();
      this.btnMeanView = new Button();
      this.btnOpemFile = new Button();
      this.openFileDialog1 = new OpenFileDialog();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.MyChart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.MyChart.Location = new Point(4, 10);
      this.MyChart.Margin = new Padding(4, 3, 4, 3);
      this.MyChart.Name = "MyChart";
      this.MyChart.ScrollGrace = 0.0;
      this.MyChart.ScrollMaxX = 0.0;
      this.MyChart.ScrollMaxY = 0.0;
      this.MyChart.ScrollMaxY2 = 0.0;
      this.MyChart.ScrollMinX = 0.0;
      this.MyChart.ScrollMinY = 0.0;
      this.MyChart.ScrollMinY2 = 0.0;
      this.MyChart.Size = new Size(620, 594);
      this.MyChart.TabIndex = 0;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.FixedPanel = FixedPanel.Panel2;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.MyChart);
      this.splitContainer1.Panel2.AccessibleRole = AccessibleRole.Pane;
      this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnSave);
      this.splitContainer1.Panel2.Controls.Add((Control) this.comboBox1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.bntOriginView);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnStdErrView);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnMeanView);
      this.splitContainer1.Panel2.Controls.Add((Control) this.btnOpemFile);
      this.splitContainer1.Size = new Size(936, 614);
      this.splitContainer1.SplitterDistance = 627;
      this.splitContainer1.TabIndex = 1;
      this.groupBox1.Anchor = AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.txtMeanErr);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.txtMaxStdErr);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Location = new Point(88, 77);
      this.groupBox1.Margin = new Padding(4, 4, 4, 4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4, 4, 4, 4);
      this.groupBox1.Size = new Size(207, 70);
      this.groupBox1.TabIndex = 31;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "标准差";
      this.txtMeanErr.Location = new Point(92, 39);
      this.txtMeanErr.Margin = new Padding(4, 4, 4, 4);
      this.txtMeanErr.Name = "txtMeanErr";
      this.txtMeanErr.Size = new Size(104, 21);
      this.txtMeanErr.TabIndex = 21;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(40, 42);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(41, 12);
      this.label3.TabIndex = 20;
      this.label3.Text = "平均：";
      this.txtMaxStdErr.Location = new Point(92, 11);
      this.txtMaxStdErr.Margin = new Padding(4, 4, 4, 4);
      this.txtMaxStdErr.Name = "txtMaxStdErr";
      this.txtMaxStdErr.Size = new Size(104, 21);
      this.txtMaxStdErr.TabIndex = 19;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(40, 15);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(41, 12);
      this.label2.TabIndex = 18;
      this.label2.Text = "最大：";
      this.btnSave.Anchor = AnchorStyles.Right;
      this.btnSave.BackgroundImage = (Image) componentResourceManager.GetObject("btnSave.BackgroundImage");
      this.btnSave.BackgroundImageLayout = ImageLayout.Center;
      this.btnSave.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.btnSave.Location = new Point(156, 326);
      this.btnSave.Margin = new Padding(4, 4, 4, 4);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(63, 56);
      this.btnSave.TabIndex = 30;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.comboBox1.Anchor = AnchorStyles.Right;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new Point(259, 265);
      this.comboBox1.Margin = new Padding(4, 4, 4, 4);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new Size(41, 20);
      this.comboBox1.TabIndex = 29;
      this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
      this.bntOriginView.Anchor = AnchorStyles.Right;
      this.bntOriginView.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.bntOriginView.Location = new Point(81, 257);
      this.bntOriginView.Margin = new Padding(4, 4, 4, 4);
      this.bntOriginView.Name = "bntOriginView";
      this.bntOriginView.Size = new Size(37, 34);
      this.bntOriginView.TabIndex = 28;
      this.bntOriginView.Text = "<-";
      this.bntOriginView.UseVisualStyleBackColor = true;
      this.bntOriginView.Click += new EventHandler(this.bntOriginView_Click);
      this.btnStdErrView.Anchor = AnchorStyles.Right;
      this.btnStdErrView.Location = new Point((int) sbyte.MaxValue, 288);
      this.btnStdErrView.Margin = new Padding(4, 4, 4, 4);
      this.btnStdErrView.Name = "btnStdErrView";
      this.btnStdErrView.Size = new Size(123, 34);
      this.btnStdErrView.TabIndex = 27;
      this.btnStdErrView.Text = "标准差图";
      this.btnStdErrView.UseVisualStyleBackColor = true;
      this.btnStdErrView.Click += new EventHandler(this.btnStdErrView_Click);
      this.btnMeanView.Anchor = AnchorStyles.Right;
      this.btnMeanView.Location = new Point((int) sbyte.MaxValue, 234);
      this.btnMeanView.Margin = new Padding(4, 4, 4, 4);
      this.btnMeanView.Name = "btnMeanView";
      this.btnMeanView.Size = new Size(123, 34);
      this.btnMeanView.TabIndex = 26;
      this.btnMeanView.Text = "平均光谱";
      this.btnMeanView.UseVisualStyleBackColor = true;
      this.btnMeanView.Click += new EventHandler(this.btnMeanView_Click);
      this.btnOpemFile.Anchor = AnchorStyles.Right;
      this.btnOpemFile.FlatStyle = FlatStyle.Popup;
      this.btnOpemFile.Location = new Point(82, 10);
      this.btnOpemFile.Margin = new Padding(2, 2, 2, 2);
      this.btnOpemFile.Name = "btnOpemFile";
      this.btnOpemFile.Size = new Size(213, 34);
      this.btnOpemFile.TabIndex = 0;
      this.btnOpemFile.Text = "打开";
      this.btnOpemFile.UseVisualStyleBackColor = true;
      this.btnOpemFile.Click += new EventHandler(this.btnOpemFile_Click);
      this.openFileDialog1.FileName = "openFileDialog1";
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(936, 614);
      this.Controls.Add((Control) this.splitContainer1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Form_offLine);
      this.Text = "离线查看谱图窗口";
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.Form_offLine_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
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
