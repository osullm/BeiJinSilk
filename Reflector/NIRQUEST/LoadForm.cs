namespace NIRQUEST
{
    using JSDU;
    using NIRQUEST.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class LoadForm : Form
    {
        private BackgroundWorker backgroundWorker1;
        private IContainer components = null;
        private Home FrmHome;
        private Label lblState;
        private Spectrometer MySpectrometer;//= new Spectrometer();
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private ProgressBar progressBar1;

        public LoadForm()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(0x3e8);
            this.FrmHome.numberOfSpectrometersFound = this.MySpectrometer.wrapper.openAllSpectrometers();
            this.backgroundWorker1.ReportProgress(1);
            Thread.Sleep(0x3e8);
            this.MySpectrometer.ReadSetParameters();
            this.backgroundWorker1.ReportProgress(2);
            Thread.Sleep(0x3e8);
            this.MySpectrometer.ReadBK(ref Home.SpInfo);
            if (Spectrometer.isClearDarks)
            {
                this.MySpectrometer.ReadDK(ref Home.SpInfo);
            }
            this.FrmHome.refreshModel();
            this.backgroundWorker1.ReportProgress(3);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            //base.Invoke(delegate (object param0, EventArgs param1) {
            //    this.progressBar1.Value = progress;
            //    this.progressBar1.Update();
            //    switch (progress)
            //    {
            //        case 1:
            //            this.lblState.Text = "打开光谱仪";
            //            break;

            //        case 2:
            //            this.lblState.Text = "读取设置参数";
            //            break;

            //        case 3:
            //            this.lblState.Text = "读取模型和背景值";
            //            break;
            //    }
            //});
            Invoke(new Action<object>((obj) =>
            {
                this.progressBar1.Value = progress;
                this.progressBar1.Update();
                switch (progress)
                {
                    case 1:
                        this.lblState.Text = "打开光谱仪";
                        break;

                    case 2:
                        this.lblState.Text = "读取设置参数";
                        break;

                    case 3:
                        this.lblState.Text = "读取模型和背景值";
                        break;
                }
            }));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.FrmHome.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
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

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.backgroundWorker1 = new BackgroundWorker();
            this.lblState = new Label();
            this.panel2 = new Panel();
            this.progressBar1 = new ProgressBar();
            this.panel3 = new Panel();
            base.SuspendLayout();
            this.panel1.BackColor = Color.Transparent;
            //this.panel1.BackgroundImage = Resources.图标1;
            panel1.Tag = "图标1";
            this.panel1.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel1.Location = new Point(12, 0x3d);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xba, 0x45);
            this.panel1.TabIndex = 0;
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.lblState.AutoSize = true;
            this.lblState.BackColor = Color.Transparent;
            this.lblState.ForeColor = Color.Black;
            this.lblState.Location = new Point(0x2b, 0x133);
            this.lblState.Name = "lblState";
            this.lblState.Size = new Size(0, 12);
            this.lblState.TabIndex = 3;
            this.panel2.BackColor = Color.Transparent;
            //this.panel2.BackgroundImage = Resources.公司信息1;
            panel2.Tag = "公司信息1";
            this.panel2.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel2.Location = new Point(3, 0x88);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x1a8, 0x45);
            this.panel2.TabIndex = 4;
            this.progressBar1.Location = new Point(40, 0x11e);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(0x179, 13);
            this.progressBar1.TabIndex = 2;
            this.progressBar1.Visible = false;
            this.panel3.BackColor = Color.Transparent;
            //this.panel3.BackgroundImage = Resources.活体雌雄蚕蛹判别系统1;
            panel3.Tag = "活体雌雄蚕蛹判别系统1";
            this.panel3.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel3.Location = new Point(12, 0xd3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x1a8, 0x45);
            this.panel3.TabIndex = 5;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            //this.BackgroundImage = Resources.底部图片1;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            base.ClientSize = new Size(0x35b, 0x14d);
            base.ControlBox = false;
            base.Controls.Add(this.panel3);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.lblState);
            base.Controls.Add(this.progressBar1);
            base.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "LoadForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "LoadForm";
            base.TransparencyKey = Color.Red;
            base.Load += new EventHandler(this.LoadForm_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadForm_Load(object sender, EventArgs e)
        {
            this.FrmHome = new Home(this);
            Spectrometer.ApplicationPath = Application.StartupPath;
            this.progressBar1.Visible = true;
            this.progressBar1.Maximum = 3;
            this.backgroundWorker1.RunWorkerAsync();
        }
    }
}

