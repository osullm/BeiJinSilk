// Decompiled with JetBrains decompiler
// Type: JSDU.Start
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JSDU
{
  public class Start : Form
  {
    private Spectrometer MySpectrometer = new Spectrometer();
    private IContainer components = (IContainer) null;
    private int numberOfSpectrometersFound;
    private Button btnOnline;
    private Button btnOffline;
    private Label label1;
    private Button btnExit;

    public Start()
    {
      this.InitializeComponent();
    }

    private void Start_Load(object sender, EventArgs e)
    {
      Spectrometer.ApplicationPath = Directory.GetCurrentDirectory();
      this.numberOfSpectrometersFound = this.MySpectrometer.wrapper.openAllSpectrometers();
      if (this.numberOfSpectrometersFound <= 0)
        return;
      this.btnOnline.Enabled = true;
    }

    private void btnOnline_Click(object sender, EventArgs e)
    {
    }

    private void btnOffline_Click(object sender, EventArgs e)
    {
      this.Close();
      this.Dispose();
      if (new Form_offLine().ShowDialog() != DialogResult.OK)
        return;
      Application.Run((Form) new Form_offLine());
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
      this.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Start));
      this.btnOnline = new Button();
      this.btnOffline = new Button();
      this.label1 = new Label();
      this.btnExit = new Button();
      this.SuspendLayout();
      this.btnOnline.DialogResult = DialogResult.Cancel;
      this.btnOnline.Enabled = false;
      this.btnOnline.FlatStyle = FlatStyle.Flat;
      this.btnOnline.Font = new Font("微软雅黑", 22.2f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.btnOnline.ForeColor = Color.Green;
      this.btnOnline.Location = new Point(37, 72);
      this.btnOnline.Name = "btnOnline";
      this.btnOnline.Size = new Size(117, 119);
      this.btnOnline.TabIndex = 0;
      this.btnOnline.Text = "采谱";
      this.btnOnline.UseVisualStyleBackColor = true;
      this.btnOnline.Click += new EventHandler(this.btnOnline_Click);
      this.btnOffline.DialogResult = DialogResult.Cancel;
      this.btnOffline.FlatStyle = FlatStyle.Flat;
      this.btnOffline.Font = new Font("微软雅黑", 22.2f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.btnOffline.ForeColor = Color.Green;
      this.btnOffline.Location = new Point(216, 72);
      this.btnOffline.Name = "btnOffline";
      this.btnOffline.Size = new Size(117, 119);
      this.btnOffline.TabIndex = 1;
      this.btnOffline.Text = "看图";
      this.btnOffline.UseVisualStyleBackColor = true;
      this.btnOffline.Click += new EventHandler(this.btnOffline_Click);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("微软雅黑", 25.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label1.ForeColor = Color.DodgerBlue;
      this.label1.Location = new Point(123, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(118, 57);
      this.label1.TabIndex = 2;
      this.label1.Text = "AHA";
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.FlatStyle = FlatStyle.Flat;
      this.btnExit.Font = new Font("微软雅黑", 10.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.btnExit.ForeColor = Color.Green;
      this.btnExit.Location = new Point(260, 205);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 31);
      this.btnExit.TabIndex = 3;
      this.btnExit.Text = "Exit";
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.AutoScaleDimensions = new SizeF(8f, 15f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(372, 239);
      this.Controls.Add((Control) this.btnExit);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnOffline);
      this.Controls.Add((Control) this.btnOnline);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Start);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (Start);
      this.TransparencyKey = this.BackColor;
      this.Load += new EventHandler(this.Start_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
