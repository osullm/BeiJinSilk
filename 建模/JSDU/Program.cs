namespace JSDU
{
    using CalcuPassWord;
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ChneckReg reg = new ChneckReg();
            if (!reg.isReg())
            {
                MessageBox.Show("软件未注册，请联系 西派特（北京）科技有限公司！");
            }
            else
            {
                Application.Run(new Form_offLine());
            }
        }
    }
}

