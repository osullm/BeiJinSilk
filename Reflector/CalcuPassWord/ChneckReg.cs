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
            string hardDiskID = this.GetHardDiskID();
            string str4 = Encryption.DesEncrypt(cpuID, "zl5210xyw");
            string str5 = Encryption.DesEncrypt(cpuID, "zl5210xyw");
            return (str4.Substring(str4.Length - 12, 8) + str5.Substring(str5.Length - 10, 8));
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
            string[] strArray;
            DataIO aio = new DataIO();
            string str = "";
            try
            {
                aio.ReadLineTXT(Application.StartupPath + @"\regFile", out strArray);
                str = ReadReg(@"Software\Intel\LANS", "Values", null);
                //str = "0eUrz1bMUrz1bM3Y";
            }
            catch (Exception)
            {
                return false;
            }
            if ((strArray != null) && (str != ""))
            {
                try
                {
                    string str2 = Encryption.DesEncrypt(strArray[0], "sepatcyfj");
                    return ((this.calcuReg() == strArray[0].Trim()) && (strArray[0].Trim() == str));
                    return true;
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
            string str = Section;

        //    (str4.Substring(str4.Length - 12, 8) + str5.Substring(str5.Length - 10, 8)) "n3RMcDZRRMcDZRpY"  string

          string aa =  Encryption.DesEncrypt("n3RMcDZRRMcDZRpY", "sepatcyfj");

            RegistryKey key1 = Registry.LocalMachine;
            RegistryKey software = key1.CreateSubKey("Software\\Intel\\LANS");

            RegistryKey software2 = key1.OpenSubKey("Software\\Intel\\LANS", true);
            software2.SetValue("Values", "44HIpFW4QTQNvp9pU4qyYUOIEe/z/tGs");


            RegistryKey key = Registry.LocalMachine.OpenSubKey(Section);
            if (key == null)
            {
                return Default;
            }
            object obj2 = key.GetValue(Key, Default);
            key.Close();
            if (obj2 != null)
            {
                if (obj2 is string)
                {
                    return Encryption.DisEncryPW(obj2.ToString(), "sepatcyfj");
                }
                return "-1";
            }
            return "-1";
        }
    }
}

