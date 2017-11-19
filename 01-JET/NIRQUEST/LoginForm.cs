// Decompiled with JetBrains decompiler
// Type: NIRQUEST.LoginForm
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using NIRQUEST.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NIRQUEST
{
  public class LoginForm : Form
  {
    private IContainer components = (IContainer) null;
    private TextBox textBox1;
    private TextBox textBox2;
    private Panel panel1;
    private Panel panel2;

    public LoginForm()
    {
      this.InitializeComponent();
    }

    private void LoginForm_Load(object sender, EventArgs e)
    {
      Rectangle workingArea = Screen.GetWorkingArea((Control) this);
      int width = workingArea.Width;
      int height = workingArea.Height;
      this.Left = (width - this.Width) / 2;
      this.Top = (height - this.Height) / 2;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (this.textBox1.Text == "admin" && this.textBox2.Text == "admin123")
      {
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        int num = (int) MessageBox.Show("用户名或密码不正确");
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBox1 = new TextBox();
      this.textBox2 = new TextBox();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.SuspendLayout();
      this.textBox1.BorderStyle = BorderStyle.None;
      this.textBox1.Font = new Font("宋体", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.textBox1.Location = new Point(141, 107);
      this.textBox1.Margin = new Padding(4);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(289, 29);
      this.textBox1.TabIndex = 1;
      this.textBox2.BorderStyle = BorderStyle.None;
      this.textBox2.Font = new Font("宋体", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.textBox2.Location = new Point(142, 172);
      this.textBox2.Margin = new Padding(4);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(288, 29);
      this.textBox2.TabIndex = 3;
      this.textBox2.UseSystemPasswordChar = true;
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Location = new Point(100, 270);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(130, 49);
      this.panel1.TabIndex = 6;
      this.panel1.Click += new EventHandler(this.button1_Click);
      this.panel2.BackColor = Color.Transparent;
      this.panel2.Location = new Point(287, 270);
      this.panel2.Margin = new Padding(4);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(143, 39);
      this.panel2.TabIndex = 7;
      this.panel2.Click += new EventHandler(this.button2_Click);
      this.AutoScaleDimensions = new SizeF(8f, 15f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) Resources.adminloginbg;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      this.ClientSize = new Size(524, 369);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.textBox2);
      this.Controls.Add((Control) this.textBox1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Margin = new Padding(4);
      this.Name = nameof (LoginForm);
      this.Text = "登录";
      this.Load += new EventHandler(this.LoginForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
