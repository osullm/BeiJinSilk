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
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            foreach (string str in Directory.GetFiles(sourceFolder))
            {
                string fileName = Path.GetFileName(str);
                string destFileName = Path.Combine(destFolder, fileName);
                File.Copy(str, destFileName, true);
            }
            foreach (string str4 in Directory.GetDirectories(sourceFolder))
            {
                string str5 = Path.GetFileName(str4);
                string str6 = Path.Combine(destFolder, str5);
                this.CopyFolder(str4, str6);
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
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
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
            Encoding.Default.GetString(buffer2);
            try
            {
                MemoryStream stream2 = new MemoryStream(buffer2);
                StreamReader reader = new StreamReader(stream2);
                int num5 = 0;
                int length = 0;
                string str = "";
                string[] strArray = null;
                while ((str = reader.ReadLine()) != null)
                {
                    if (num5 == 0)
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
                    num5++;
                }
                Data = new double[num5, length];
                reader.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            try
            {
                MemoryStream stream3 = new MemoryStream(buffer2);
                StreamReader reader2 = new StreamReader(stream3);
                int num7 = 0;
                string str2 = "";
                while ((str2 = reader2.ReadLine()) != null)
                {
                    str2 = str2.ToString().Trim();
                    string[] strArray2 = null;
                    if (str2.IndexOf('\t') > 0)
                    {
                        strArray2 = str2.Split(new char[] { '\t' });
                    }
                    else if (str2.IndexOf(' ') > 0)
                    {
                        strArray2 = str2.Split(new char[] { ' ' });
                    }
                    if (strArray2 != null)
                    {
                        for (int j = 0; j < strArray2.Length; j++)
                        {
                            Data[num7, j] = float.Parse(strArray2[j].ToString());
                        }
                    }
                    else
                    {
                        Data[num7, 0] = float.Parse(str2.ToString());
                    }
                    num7++;
                }
                reader2.Close();
            }
            catch (Exception exception2)
            {
                MessageBox.Show(exception2.Message, "Error");
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

        public int SaveSIMCAModel(string path, double[,] Data)
        {
            int num7;
            string s = "";
            if (Data == null)
            {
                throw new Exception("输入保存的数据为空！");
            }
            if ((Data.GetLength(0) + Data.GetLength(1)) <= 0)
            {
                throw new Exception("输入保存的数据为空！");
            }
            for (int i = 0; i < Data.GetLength(0); i++)
            {
                for (int k = 0; k < Data.GetLength(1); k++)
                {
                    if (k != (Data.GetLength(1) - 1))
                    {
                        s = s + Data[i, k].ToString() + " ";
                    }
                    else
                    {
                        s = s + Data[i, k].ToString() + "\r\n";
                    }
                }
            }
            byte[] bytes = Encoding.Default.GetBytes(s);
            Encoding.Default.GetString(bytes);
            byte[] buffer2 = new byte[bytes.Length];
            byte num3 = 0x75;
            byte num4 = 0x18;
            for (long j = 0L; j < buffer2.Length; j += 1L)
            {
                byte num6 = bytes[(int) ((IntPtr) j)];
                if ((j % 2L) == 0L)
                {
                    buffer2[(int) ((IntPtr) j)] = (byte) (num6 ^ num3);
                }
                else
                {
                    buffer2[(int) ((IntPtr) j)] = (byte) (num6 ^ num4);
                }
            }
            Encoding.Default.GetString(buffer2);
            try
            {
                MemoryStream stream = new MemoryStream(buffer2);
                FileStream output = new FileStream(path, FileMode.OpenOrCreate);
                new BinaryWriter(output).Write(stream.ToArray());
                output.Close();
                num7 = 1;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num7;
        }

        public void SaveStr(string WriteStr, string Path)
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
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

        /// <summary>
        /// 从制定路径导入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="Data_x">返回的X值</param>
        /// <param name="Data_y">返回的Y值</param>
        /// <param name="IfSize">如果IfSize==true,只返回文件的长度。如果IfSize==false,返回X，Y值</param>
        /// <returns></returns>
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
            Data = null;
            StreamReader reader = new StreamReader(path, false);
            try
            {
                int num = 0;
                int length = 0;
                string str = "";
                string[] strArray = null;
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
            catch (Exception exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                throw exception;
            }
            try
            {
                int num3 = 0;
                string str2 = "";
                reader = new StreamReader(path, false);
                while ((str2 = reader.ReadLine()) != null)
                {
                    str2 = str2.ToString().Trim();
                    string[] strArray2 = null;
                    if (str2.IndexOf('\t') > 0)
                    {
                        strArray2 = str2.Split(new char[] { '\t' });
                    }
                    else if (str2.IndexOf(' ') > 0)
                    {
                        strArray2 = str2.Split(new char[] { ' ' });
                    }
                    if (strArray2 != null)
                    {
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            Data[num3, i] = float.Parse(strArray2[i].ToString());
                        }
                    }
                    else
                    {
                        Data[num3, 0] = float.Parse(str2.ToString());
                    }
                    num3++;
                }
                reader.Close();
            }
            catch (Exception exception2)
            {
                reader.Close();
                throw exception2;
            }
            return 0;
        }

        public void TXTSaveContent(string path, string[] content1, string[] content2)
        {
            if (content1.Length == content2.Length)
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
                        for (int i = 0; i < content1.Length; i++)
                        {
                            str = "";
                            str = content1[i].ToString() + " " + content2[i].ToString() + "\r\n";
                            writer.Write(str);
                        }
                        writer.Close();
                        stream.Close();
                    }
                    catch (IOException exception)
                    {
                        MessageBox.Show(exception.Message + "File Error !");
                    }
                    return;
                }
            }
            MessageBox.Show("文件写入错误！");
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
                    return;
                }
            }
            MessageBox.Show("文件写入错误！");
        }

        public int TXTWriteDatas(string path, double[,] Data)
        {
            string str = "";
            if (Data == null)
            {
                throw new Exception("输入保存的数据为空！");
            }
            if ((Data.GetLength(0) + Data.GetLength(1)) > 0)
            {
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    StreamWriter writer = new StreamWriter(stream);
                    try
                    {
                        for (int i = 0; i < Data.GetLength(0); i++)
                        {
                            str = "";
                            for (int j = 0; j < Data.GetLength(1); j++)
                            {
                                if (j != (Data.GetLength(1) - 1))
                                {
                                    str = str + Data[i, j].ToString() + " ";
                                }
                                else
                                {
                                    str = str + Data[i, j].ToString() + "\r\n";
                                }
                            }
                            writer.Write(str);
                        }
                        writer.Close();
                        stream.Close();
                        return Data.GetLength(0);
                    }
                    catch (IOException exception)
                    {
                        MessageBox.Show(exception.Message + "File Error !");
                        return 0;
                    }
                }
            }
            throw new Exception("输入保存的数据为空！");
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

