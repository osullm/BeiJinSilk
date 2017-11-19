namespace NIRQUEST
{
    using CalcuPassWord;
    using Microsoft.Win32;
    using System;

    internal class RegOperate
    {
        public static string ReadSetting(string Section, string Key, string Default)
        {
            if (Default == null)
            {
                Default = "-1";
            }
            string str = Section;
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\MyTest_ChildPlat\ChildPlat");
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
                    return Encryption.DisEncryPW(obj2.ToString(), "ejiang11");
                }
                return "-1";
            }
            return "-1";
        }

        public static void WriteSetting(string Section, string Key, string Setting)
        {
            string str = Section;
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\MyTest_ChildPlat\ChildPlat");
            if (key != null)
            {
                try
                {
                    key.SetValue(Key, Setting);
                }
                catch (Exception)
                {
                }
                finally
                {
                    key.Close();
                }
            }
        }
    }
}

