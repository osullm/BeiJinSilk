namespace JSDU
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class DataIO
    {
        public void CopyFolder(string sourceFolder, string destFolder)
        {
            string fileName;
            string str3;
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string str in files)
            {
                fileName = Path.GetFileName(str);
                str3 = Path.Combine(destFolder, fileName);
                File.Copy(str, str3, true);
            }
            string[] directories = Directory.GetDirectories(sourceFolder);
            foreach (string str4 in directories)
            {
                fileName = Path.GetFileName(str4);
                str3 = Path.Combine(destFolder, fileName);
                this.CopyFolder(str4, str3);
            }
        }

        public void CreatTXT(string path)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    stream.Close();
                }
            }
            catch
            {
                MessageBox.Show("文件错误，请重新测量");
            }
        }

        public void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string str in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(str))
                    {
                        FileInfo info = new FileInfo(str);
                        if (info.Attributes.ToString().IndexOf("Readonly") != 1)
                        {
                            info.Attributes = FileAttributes.Normal;
                        }
                        File.Delete(str);
                    }
                    else
                    {
                        this.DeleteFolder(str);
                    }
                }
                Directory.Delete(dir);
            }
        }

        public int LoadSIMCAModel(string path, out double[,] Data)
        {
            byte[] buffer;
            byte[] buffer2;
            MemoryStream stream2;
            StreamReader reader;
            int length;
            string str2;
            string[] strArray;
            Exception exception;
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                buffer = new byte[stream.Length];
                buffer2 = new byte[stream.Length];
                stream.Read(buffer, 0, (int) stream.Length);
            }
            byte num = 0x75;
            byte num2 = 0x18;
            for (long i = 0L; i < buffer.Length; i += 1L)
            {
                byte num4 = buffer[(int) ((IntPtr) i)];
                if ((i % 2L) == 0L)
                {
                    buffer2[(int) ((IntPtr) i)] = (byte) (num4 ^ num);
                }
                else
                {
                    buffer2[(int) ((IntPtr) i)] = (byte) (num4 ^ num2);
                }
            }
            string str = Encoding.Default.GetString(buffer2);
            try
            {
                stream2 = new MemoryStream(buffer2);
                reader = new StreamReader(stream2);
                int num5 = 0;
                length = 0;
                str2 = "";
                strArray = null;
                while ((str2 = reader.ReadLine()) != null)
                {
                    if (num5 == 0)
                    {
                        str2 = str2.ToString().Trim();
                        if (str2.IndexOf('\t') > 0)
                        {
                            strArray = str2.Split(new char[] { '\t' });
                        }
                        else if (str2.IndexOf(' ') > 0)
                        {
                            strArray = str2.Split(new char[] { ' ' });
                        }
                        if (strArray != null)
                        {
                            length = strArray.Length;
                        }
                        else
                        {
                            length = 1;
                        }
                    }
                    num5++;
                }
                Data = new double[num5, length];
                reader.Close();
            }
            catch (Exception exception1)
            {
                exception = exception1;
                throw exception;
            }
            try
            {
                stream2 = new MemoryStream(buffer2);
                reader = new StreamReader(stream2);
                length = 0;
                str2 = "";
                while ((str2 = reader.ReadLine()) != null)
                {
                    str2 = str2.ToString().Trim();
                    strArray = null;
                    if (str2.IndexOf('\t') > 0)
                    {
                        strArray = str2.Split(new char[] { '\t' });
                    }
                    else if (str2.IndexOf(' ') > 0)
                    {
                        strArray = str2.Split(new char[] { ' ' });
                    }
                    if (strArray != null)
                    {
                        for (int j = 0; j < strArray.Length; j++)
                        {
                            Data[length, j] = float.Parse(strArray[j].ToString());
                        }
                    }
                    else
                    {
                        Data[length, 0] = float.Parse(str2.ToString());
                    }
                    length++;
                }
                reader.Close();
            }
            catch (Exception exception2)
            {
                exception = exception2;
                MessageBox.Show(exception.Message, "Error");
            }
            return 0;
        }

        public void ReadLineTXT(string path, out string[] Content)
        {
            Content = new string[1];
            try
            {
                int index = 0;
                string str = "";
                StreamReader reader = new StreamReader(path, false);
                while ((str = reader.ReadLine()) != null)
                {
                    index++;
                }
                reader.Close();
                reader = new StreamReader(path, false);
                Content = new string[index];
                for (index = 0; (str = reader.ReadLine()) != null; index++)
                {
                    Content[index] = str;
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SaveStr(string WriteStr, string Path)
        {
            Path = Spectrometer.ApplicationPath + @"\Setting";
            File.Delete(Path);
            using (FileStream stream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                StreamWriter writer = new StreamWriter(stream);
                try
                {
                    writer.Write(WriteStr);
                    writer.Close();
                    stream.Close();
                }
                catch (IOException exception)
                {
                    MessageBox.Show(exception.Message + "File Error !");
                }
            }
        }

        public int TXTReadData(string path, ref double[] Data_x, ref double[] Data_y, bool IfSize)
        {
            int num = 0;
            if (IfSize)
            {
                try
                {
                    StreamReader reader = new StreamReader(path, false);
                    while (reader.ReadLine() != null)
                    {
                        num++;
                    }
                    reader.Close();
                    return num;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return 0;
                }
            }
            try
            {
                int num2 = 0;
                int num3 = 0;
                string str = "";
                StreamReader reader2 = new StreamReader(path, false);
                while ((str = reader2.ReadLine()) != null)
                {
                    string[] strArray = str.ToString().Trim().Split(new char[] { ' ' });
                    Data_x[num2++] = double.Parse(strArray[0].ToString());
                    Data_y[num3++] = double.Parse(strArray[1].ToString());
                }
                reader2.Close();
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message, "Error");
            }
            return 0;
        }

        public int TXTReadDatas(string path, out double[,] Data)
        {
            int length;
            string str;
            string[] strArray;
            Exception exception;
            StreamReader reader = new StreamReader(path, false);
            Data = null;
            try
            {
                int num = 0;
                length = 0;
                str = "";
                strArray = null;
                reader = new StreamReader(path, false);
                while ((str = reader.ReadLine()) != null)
                {
                    if (num == 0)
                    {
                        str = str.ToString().Trim();
                        if (str.IndexOf('\t') > 0)
                        {
                            strArray = str.Split(new char[] { '\t' });
                        }
                        else if (str.IndexOf(' ') > 0)
                        {
                            strArray = str.Split(new char[] { ' ' });
                        }
                        if (strArray != null)
                        {
                            length = strArray.Length;
                        }
                        else
                        {
                            length = 1;
                        }
                    }
                    num++;
                }
                Data = new double[num, length];
                reader.Close();
            }
            catch (Exception exception1)
            {
                exception = exception1;
                reader.Close();
                reader.Dispose();
                throw exception;
            }
            try
            {
                length = 0;
                str = "";
                reader = new StreamReader(path, false);
                while ((str = reader.ReadLine()) != null)
                {
                    str = str.ToString().Trim();
                    strArray = null;
                    if (str.IndexOf('\t') > 0)
                    {
                        strArray = str.Split(new char[] { '\t' });
                    }
                    else if (str.IndexOf(' ') > 0)
                    {
                        strArray = str.Split(new char[] { ' ' });
                    }
                    if (strArray != null)
                    {
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            Data[length, i] = float.Parse(strArray[i].ToString());
                        }
                    }
                    else
                    {
                        Data[length, 0] = float.Parse(str.ToString());
                    }
                    length++;
                }
                reader.Close();
            }
            catch (Exception exception2)
            {
                exception = exception2;
                reader.Close();
                reader.Dispose();
                throw exception;
            }
            return 0;
        }

        public void TXTSaveData(string path, double[] Data_x, double[] Data_y)
        {
            if (Data_x.Length == Data_y.Length)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                string str = "";
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    StreamWriter writer = new StreamWriter(stream);
                    try
                    {
                        for (int i = 0; i < Data_x.Length; i++)
                        {
                            str = "";
                            str = Data_x[i].ToString() + " " + Data_y[i].ToString() + "\r\n";
                            writer.Write(str);
                        }
                        writer.Close();
                        stream.Close();
                    }
                    catch (IOException exception)
                    {
                        MessageBox.Show(exception.Message + "File Error !");
                    }
                }
            }
            else
            {
                MessageBox.Show("文件写入错误！");
            }
        }

        public void TXTWriteIn(string path, string Content)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine(Content.ToString());
                    writer.Close();
                    stream.Close();
                }
            }
            catch
            {
                MessageBox.Show("文件写入错误，请重新测量");
            }
        }
    }
}

