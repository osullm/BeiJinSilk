namespace JSDU
{
    using CalcuPassWord;
    using Microsoft.Win32;
    using NIRQUEST;
    using System;
    using System.Collections.Generic;
    using System.IO;
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
                string executablePath = Application.ExecutablePath;
                RegistryKey localMachine = Registry.LocalMachine;
                RegistryKey key2 = localMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                key2.DeleteValue("JcShutdown", false);
                key2.Close();
                localMachine.Close();
                FrmGetSpecSet.Default.isAutoStart = false;
            }
            else
            {
                string path = Application.StartupPath + @"\parameterSimcaPrdtlog.txt";
                if (File.Exists(path))
                {
                    DataIO aio = new DataIO();
                    FileInfo info = new FileInfo(path);
                    if (info.Length > 0x1f4000L)
                    {
                        List<string> list = new List<string>(File.ReadAllLines(path));
                        for (int i = 0; i < (list.Count / 2); i++)
                        {
                            list.RemoveAt(i);
                        }
                        File.Delete(path);
                        aio.CreatTXT(path);
                        File.WriteAllLines(path, list.ToArray());
                    }
                    info = null;
                }
                Application.Run(new LoadForm());
            }
        }
    }
}

