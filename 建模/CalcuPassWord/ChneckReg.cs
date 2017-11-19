namespace CalcuPassWord
{
    using JSDU;
    using Microsoft.Win32;
    using System;
    using System.Management;
    using System.Windows.Forms;

    internal class ChneckReg
    {
        private string calcuReg()
        {
            string cpuID = this.GetCpuID();
            this.GetHardDiskID();
            string str3 = Encryption.DesEncrypt(cpuID, "zl5210xyw");
            string str4 = Encryption.DesEncrypt(cpuID, "zl5210xyw");
            return (str3.Substring(str3.Length - 12, 8) + str4.Substring(str4.Length - 10, 8));
        }

        public string GetCpuID()
        {
            try
            {
                ManagementObjectCollection instances = new ManagementClass("Win32_Processor").GetInstances();
                string str = null;
                foreach (ManagementObject obj2 in instances)
                {
                    str = obj2.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        public string GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string str = null;
                foreach (ManagementObject obj2 in searcher.Get())
                {
                    str = obj2["SerialNumber"].ToString().Trim();
                    break;
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        public bool isReg()
        {
            return true;

            string[] strArray;
            DataIO aio = new DataIO();
            string str = "";
            try
            {
                aio.ReadLineTXT(Application.StartupPath + @"\regFile", out strArray);
                str = ReadReg(@"Software\Intel\LANS", "Values1", null);
            }
            catch (Exception)
            {
                return false;
            }
            if ((strArray != null) && (str != ""))
            {
                try
                {
                    Encryption.DesEncrypt(strArray[0], "sepatsmcjm");
                    return ((this.calcuReg() == strArray[0].Trim()) && (strArray[0].Trim() == str));
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public static string ReadReg(string Section, string Key, string Default)
        {
            if (Default == null)
            {
                Default = "-1";
            }
            RegistryKey key = Registry.LocalMachine.OpenSubKey(Section);
            if (key == null)
            {
                return Default;
            }
            object obj2 = key.GetValue(Key, Default);
            key.Close();
            if ((obj2 != null) && (obj2 is string))
            {
                return Encryption.DisEncryPW(obj2.ToString(), "sepatsmcjm");
            }
            return "-1";
        }
    }
}

