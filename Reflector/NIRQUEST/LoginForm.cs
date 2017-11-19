namespace NIRQUEST
{
    using NIRQUEST.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class LoginForm : Form
    {
        private Button btnLg;
        private IContainer components = null;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;

        public LoginForm()
        {
            this.InitializeComponent();
        }

        private void btnLg_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text == "123")
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("密码不正确");
            }
        }

        private void btnLg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.textBox2.Text == "123")
                {
                    base.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("密码不正确");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
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
            this.textBox1 = new TextBox();
            this.textBox2 = new TextBox();
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.panel3 = new Panel();
            this.textBox3 = new TextBox();
            this.btnLg = new Button();
            base.SuspendLayout();
            this.textBox1.BorderStyle = BorderStyle.None;
            this.textBox1.Font = new Font("宋体", 21.75f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBox1.Location = new Point(0x13c, 0xc2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x16e, 0x22);
            this.textBox1.TabIndex = 1;
            this.textBox2.BorderStyle = BorderStyle.None;
            this.textBox2.Font = new Font("宋体", 21.75f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBox2.Location = new Point(0x139, 0x114);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(0x171, 0x22);
            this.textBox2.TabIndex = 0;
            this.textBox2.UseSystemPasswordChar = true;
            this.textBox2.KeyDown += new KeyEventHandler(this.textBox2_KeyDown);
            this.panel1.BackColor = Color.Transparent;
            this.panel1.Location = new Point(0x187, 0x142);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xb6, 0x42);
            this.panel1.TabIndex = 6;
            this.panel1.Click += new EventHandler(this.button1_Click);
            this.panel2.BackColor = Color.Transparent;
            this.panel2.Location = new Point(0x1fc, 0x18a);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0xae, 0x3a);
            this.panel2.TabIndex = 2;
            this.panel2.Click += new EventHandler(this.button2_Click);
            this.panel3.BackColor = Color.Gainsboro;
            this.panel3.Location = new Point(0xf9, 0xb3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x1bf, 0x3a);
            this.panel3.TabIndex = 8;
            this.textBox3.Location = new Point(0, 0);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Size(100, 0x15);
            this.textBox3.TabIndex = 9;
            this.btnLg.BackColor = Color.Transparent;
            this.btnLg.FlatStyle = FlatStyle.Popup;
            this.btnLg.Font = new Font("宋体", 21.75f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.btnLg.Location = new Point(0x108, 0x18a);
            this.btnLg.Name = "btnLg";
            this.btnLg.Size = new Size(0xad, 0x3a);
            this.btnLg.TabIndex = 1;
            this.btnLg.UseVisualStyleBackColor = false;
            this.btnLg.Click += new EventHandler(this.btnLg_Click);
            this.btnLg.KeyDown += new KeyEventHandler(this.btnLg_KeyDown);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            //this.BackgroundImage = Resources.adminloginbg;
            this.BackgroundImageLayout = ImageLayout.Center;
            base.ClientSize = new Size(950, 580);
            base.Controls.Add(this.btnLg);
            base.Controls.Add(this.textBox3);
            base.Controls.Add(this.panel3);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.textBox2);
            base.Controls.Add(this.textBox1);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "LoginForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "登录";
            base.Load += new EventHandler(this.LoginForm_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            int width = workingArea.Width;
            int height = workingArea.Height;
            base.Left = (width - base.Width) / 2;
            base.Top = (height - base.Height) / 2;
            this.textBox2.Focus();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.textBox2.Text == "123")
                {
                    base.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("密码不正确");
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }
    }
}

