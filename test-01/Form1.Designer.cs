namespace test_01
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.chart_a = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonRandom = new System.Windows.Forms.RadioButton();
            this.radioButtonKS = new System.Windows.Forms.RadioButton();
            this.button5 = new System.Windows.Forms.Button();
            this.bgwCrossBySimca = new System.ComponentModel.BackgroundWorker();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.bwgAbnormalSample = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtThresholdAbnormal = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.domainUpDown2 = new System.Windows.Forms.DomainUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart_a)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "导入雌";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 62);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(143, 44);
            this.button2.TabIndex = 0;
            this.button2.Text = "导入雄";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 169);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(143, 44);
            this.button3.TabIndex = 0;
            this.button3.Text = "雌--马氏距离";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 217);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(143, 44);
            this.button4.TabIndex = 0;
            this.button4.Text = "雄--马氏距离";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // chart_a
            // 
            chartArea2.Name = "ChartArea1";
            this.chart_a.ChartAreas.Add(chartArea2);
            this.chart_a.Location = new System.Drawing.Point(361, 12);
            this.chart_a.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_a.Name = "chart_a";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series2.Name = "Series1";
            this.chart_a.Series.Add(series2);
            this.chart_a.Size = new System.Drawing.Size(547, 452);
            this.chart_a.TabIndex = 1;
            this.chart_a.Text = "chart1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonRandom);
            this.groupBox1.Controls.Add(this.radioButtonKS);
            this.groupBox1.Location = new System.Drawing.Point(161, 22);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(180, 134);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择方式";
            // 
            // radioButtonRandom
            // 
            this.radioButtonRandom.AutoSize = true;
            this.radioButtonRandom.Location = new System.Drawing.Point(19, 80);
            this.radioButtonRandom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonRandom.Name = "radioButtonRandom";
            this.radioButtonRandom.Size = new System.Drawing.Size(58, 19);
            this.radioButtonRandom.TabIndex = 0;
            this.radioButtonRandom.Text = "随机";
            this.radioButtonRandom.UseVisualStyleBackColor = true;
            // 
            // radioButtonKS
            // 
            this.radioButtonKS.AutoSize = true;
            this.radioButtonKS.Checked = true;
            this.radioButtonKS.Location = new System.Drawing.Point(19, 40);
            this.radioButtonKS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonKS.Name = "radioButtonKS";
            this.radioButtonKS.Size = new System.Drawing.Size(132, 19);
            this.radioButtonKS.TabIndex = 0;
            this.radioButtonKS.TabStop = true;
            this.radioButtonKS.Text = "Kennark-Stone";
            this.radioButtonKS.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 270);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(143, 44);
            this.button5.TabIndex = 0;
            this.button5.Text = "建模";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // bgwCrossBySimca
            // 
            this.bgwCrossBySimca.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwCrossBySimca_DoWork);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(16, 320);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(139, 41);
            this.button6.TabIndex = 3;
            this.button6.Text = "导入模型";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(12, 369);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(139, 41);
            this.button7.TabIndex = 3;
            this.button7.Text = "导入模型";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // bwgAbnormalSample
            // 
            this.bwgAbnormalSample.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwgAbnormalSample_DoWork);
            this.bwgAbnormalSample.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwgAbnormalSample_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(161, 220);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "异常样本阈值:";
            // 
            // txtThresholdAbnormal
            // 
            this.txtThresholdAbnormal.Location = new System.Drawing.Point(272, 217);
            this.txtThresholdAbnormal.Name = "txtThresholdAbnormal";
            this.txtThresholdAbnormal.Size = new System.Drawing.Size(67, 25);
            this.txtThresholdAbnormal.TabIndex = 5;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(12, 112);
            this.button8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(143, 44);
            this.button8.TabIndex = 0;
            this.button8.Text = "数据截取";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
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
            this.domainUpDown2.Location = new System.Drawing.Point(272, 169);
            this.domainUpDown2.Margin = new System.Windows.Forms.Padding(4);
            this.domainUpDown2.Name = "domainUpDown2";
            this.domainUpDown2.ReadOnly = true;
            this.domainUpDown2.Size = new System.Drawing.Size(67, 25);
            this.domainUpDown2.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "校正样本数量:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 562);
            this.Controls.Add(this.domainUpDown2);
            this.Controls.Add(this.txtThresholdAbnormal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chart_a);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart_a)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_a;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonRandom;
        private System.Windows.Forms.RadioButton radioButtonKS;
        private System.Windows.Forms.Button button5;
        private System.ComponentModel.BackgroundWorker bgwCrossBySimca;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.ComponentModel.BackgroundWorker bwgAbnormalSample;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtThresholdAbnormal;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.DomainUpDown domainUpDown2;
        private System.Windows.Forms.Label label2;
    }
}

