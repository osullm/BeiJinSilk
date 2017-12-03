using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_01
{
    public class Process_FitOffLine
    {
      

        //public List<double[]> MaleList = new List<double[]>();

        //public List<double[]> unSeeData = new List<double[]>();

        //public List<double[]> simDataList = new List<double[]>();

       // int femaleCount, maleCount;





        public List<double[]> loadData()
        {
            List<double[]> dataList = new List<double[]>();
            //  FemaleList.Clear();
           int  Count = 0;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(*.txt)|*.txt|(*.*)|*.*";
            ofd.RestoreDirectory = true;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                String[] names = ofd.FileNames;
                // List<Double> arr1 = new List<Double>();
                List<Double> arr2 = new List<Double>();
                for (int i = 0; i < names.Length; i++)
                {
                    arr2.Clear();
                    Count++;
                    FileStream fs = new FileStream(names[i], FileMode.Open);
                    StreamReader sr = new StreamReader(fs);

                    try
                    {
                        string line = sr.ReadLine();
                        while (line != null)
                        {

                            String[] a = line.Split(' ');

                            //arr1.Add(double.Parse(a[0]));
                            arr2.Add(double.Parse(a[1]));

                            line = sr.ReadLine();
                        }
                        //double[] bfwavelength = arr1.ToArray();
                        double[] bfspec = arr2.ToArray();
                        dataList.Add(bfspec);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    finally
                    {
                        sr.Close();
                        fs.Close();
                    }
                }
            }
            ofd.Dispose();
            return dataList;
        }

        //public void loadMaleTrainData()
        //{
        //    MaleList.Clear();
        //    maleCount = 0;

        //    OpenFileDialog ofd = new OpenFileDialog();
        //    ofd.Filter = "(*.txt)|*.txt|(*.*)|*.*";
        //    ofd.RestoreDirectory = true;
        //    ofd.Multiselect = true;
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        String[] names = ofd.FileNames;
        //        //List<Double> arr1 = new List<Double>();
        //        List<Double> arr2 = new List<Double>();
        //        for (int i = 0; i < names.Length; i++)
        //        {
        //            arr2.Clear();
        //            maleCount++;
        //            FileStream fs = new FileStream(names[i], FileMode.Open);
        //            StreamReader sr = new StreamReader(fs);
        //            try
        //            {
        //                string line = sr.ReadLine();
        //                while (line != null)
        //                {

        //                    String[] a = line.Split(' ');
        //                    //arr1.Add(double.Parse(a[0]));
        //                    arr2.Add(double.Parse(a[1]));


        //                    line = sr.ReadLine();
        //                }
        //                //double[] bfwavelength = arr1.ToArray();
        //                double[] bfspec = arr2.ToArray();
        //                MaleList.Add(bfspec);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message.ToString());
        //            }
        //            finally
        //            {
        //                sr.Close();
        //                fs.Close();
        //            }
        //        }
        //    }
        //    ofd.Dispose();
        //}

        //public void doTrain()
        //{
        //    //Process_MatlabNet.doTrain(specList1, specList2);
            
        //    Process_MatlabNet.svmTrain_matlab(specList1, specList2);
        //}

        //public void loadSimData()
        //{
        //    simDataList.Clear();
        //    //maleCount = 0;

        //    OpenFileDialog ofd = new OpenFileDialog();
        //    ofd.Filter = "(*.txt)|*.txt|(*.*)|*.*";
        //    ofd.RestoreDirectory = true;
        //    ofd.Multiselect = true;
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        String[] names = ofd.FileNames;
        //        //List<Double> arr1 = new List<Double>();
        //        List<Double> arr2 = new List<Double>();
        //        for (int i = 0; i < names.Length; i++)
        //        {
        //            arr2.Clear();
        //            //maleCount++;
        //            FileStream fs = new FileStream(names[i], FileMode.Open);
        //            StreamReader sr = new StreamReader(fs);
        //            try
        //            {
        //                string line = sr.ReadLine();
        //                while (line != null)
        //                {

        //                    String[] a = line.Split(' ');
        //                    //arr1.Add(double.Parse(a[0]));
        //                    arr2.Add(double.Parse(a[1]));


        //                    line = sr.ReadLine();
        //                }
        //                //double[] bfwavelength = arr1.ToArray();
        //                double[] bfspec = arr2.ToArray();
        //                simDataList.Add(bfspec);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message.ToString());
        //            }
        //            finally
        //            {
        //                sr.Close();
        //                fs.Close();
        //            }
        //        }
        //    }
        //    ofd.Dispose();
        //}
        //public double[] doSim()
        //    {
        //    double[] outData= new double[simDataList.Count];
        //    for (int i = 0; i < simDataList.Count; i++)
        //    {
        //        outData[i]=Process_MatlabNet.svmClassify_matlab(simDataList[i]);
        //    }
        //    return outData;
        //    //return Process_MatlabNet.doSimBatch(simDataList);

        //}


        //public void loadUnseeData()
        //{
        //    unSeeData.Clear();
        //    femaleCount = 0;

        //    OpenFileDialog ofd = new OpenFileDialog();
        //    ofd.Filter = "(*.txt)|*.txt|(*.*)|*.*";
        //    ofd.RestoreDirectory = true;
        //    ofd.Multiselect = true;
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        String[] names = ofd.FileNames;
        //        // List<Double> arr1 = new List<Double>();
        //        List<Double> arr2 = new List<Double>();
        //        for (int i = 0; i < names.Length; i++)
        //        {
        //            arr2.Clear();
        //            femaleCount++;
        //            FileStream fs = new FileStream(names[i], FileMode.Open);
        //            StreamReader sr = new StreamReader(fs);

        //            try
        //            {
        //                string line = sr.ReadLine();
        //                while (line != null)
        //                {

        //                    String[] a = line.Split(' ');

        //                    //arr1.Add(double.Parse(a[0]));
        //                    arr2.Add(double.Parse(a[1]));

        //                    line = sr.ReadLine();
        //                }
        //                //double[] bfwavelength = arr1.ToArray();
        //                double[] bfspec = arr2.ToArray();
        //                unSeeData.Add(bfspec);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message.ToString());
        //            }
        //            finally
        //            {
        //                sr.Close();
        //                fs.Close();
        //            }
        //        }
        //    }
        //    ofd.Dispose();
        //}

    }
}
