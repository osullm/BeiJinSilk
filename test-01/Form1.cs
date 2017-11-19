using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Windows.Forms;

namespace test_01
{
    public partial class Form1 : Form
    {
        Process_FitOffLine offLineClass = new Process_FitOffLine();

        int dataYlength = 385, SpecSkip=4;
        int dataYSkipLength = 96;
        int selectSetMethod;
        double selectSetCount = 0.5;

        double thresholdAbnormal = 0.2;
        double[] femaleMaDistance, maleMaDistance;
        double[,] dataYForSelecteSet;

        double[,] femaleCalData, femaleValData,maleCalData,maleValData;
        

        public Form1()
        {
            InitializeComponent();
        }

   

        private void Form1_Load(object sender, EventArgs e)
        {
            SIMCA.SIMCA simca = new SIMCA.SIMCA();
            MWNumericArray x = new double[100];
            MWArray[] arrayArray = simca.mncn(2, x);

        
        }



        /// <summary>
        /// 导入雌性数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            offLineClass.loadFemalTrainData();
        }
        /// <summary>
        /// 导入雄性数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            offLineClass.loadMaleTrainData();
        }

        /// <summary>
        /// 雌性-计算马氏距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            double[,] femaleData = new double[offLineClass.FemaleList.Count, dataYlength];
            dataYForSelecteSet = new double[offLineClass.FemaleList.Count, dataYSkipLength];

            for (int i = 0; i < offLineClass.FemaleList.Count; i++)
            {
                int num3 = 0;
                //截取 41-109,167-484的总共385个数据。
                for (int j = 41; j < 109; j++)
                {
                    femaleData[i, num3++] = offLineClass.FemaleList[i][j];
                }
                for (int k = 167; k < 484; k++)
                {
                    femaleData[i, num3++] = offLineClass.FemaleList[i][k];
                }
                //每隔m=4个，选一个值进行计算。
                for (int m = 0; m < dataYSkipLength ; m++)
                {
                    dataYForSelecteSet[i, m] = femaleData [i, m * this.SpecSkip];
                }

            }


           int[] reArray=new ripsPreDeal(). maDistanceAbnormalIndex(dataYForSelecteSet, thresholdAbnormal,out femaleMaDistance);
            //省略去除5个马氏距离最大值的样品。
            //省略去除大于阈值的样品。
            if (radioButtonRandom.Checked)
            {
                this.selectSet(0,dataYForSelecteSet ,true);
                selectSetMethod = 0;
            }
            else if (radioButtonKS.Checked)
            {
                this.selectSet(1,dataYForSelecteSet ,true);
                selectSetMethod = 1;
            }
           
            

        }
        /// <summary>
        /// 雄性--计算马氏距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            double[,] maleData = new double[offLineClass.MaleList .Count, dataYlength];
            dataYForSelecteSet = new double[offLineClass.MaleList .Count, dataYSkipLength];

            for (int i = 0; i < offLineClass.MaleList .Count; i++)
            {
                int num3 = 0;
                //截取 41-109,167-484的总共385个数据。
                for (int j = 41; j < 109; j++)
                {
                    maleData[i, num3++] = offLineClass.MaleList [i][j];
                }
                for (int k = 167; k < 484; k++)
                {
                    maleData[i, num3++] = offLineClass.MaleList [i][k];
                }
                //每隔m=4个，选一个值进行计算。
                for (int m = 0; m < dataYSkipLength; m++)
                {
                    dataYForSelecteSet[i, m] = maleData[i, m * this.SpecSkip];
                }

            }


            int[] reArray = new ripsPreDeal().maDistanceAbnormalIndex(dataYForSelecteSet, thresholdAbnormal, out maleMaDistance);
            //省略去除5个马氏距离最大值的样品。
            //省略去除大于阈值的样品。
            if (radioButtonRandom.Checked)
            {
                this.selectSet(0, dataYForSelecteSet, false);
                selectSetMethod = 0;
            }
            else if (radioButtonKS.Checked)
            {
                this.selectSet(1, dataYForSelecteSet, false);
                selectSetMethod = 1;
            }

        }
        private void selectSet(int selectSetMethod, double[,] data, bool female)
        {

            int[] numArray2;
            int[] numArray3;
            double d = selectSetCount * data.GetLength(0);
            int nC = (int)Math.Floor(d);
            double[,] x = new double[this.dataYForSelecteSet.GetLength(0), this.dataYForSelecteSet.GetLength(1)];
            for (int i = 0; i < x.GetLength(0); i++)
            {
                for (int m = 0; m < x.GetLength(1); m++)
                {
                    x[i, m] = this.dataYForSelecteSet[i, m];
                }
            }
            ripsPreDeal deal = new ripsPreDeal(x.GetLength(0), x.GetLength(1));
            if (selectSetMethod == 0)
            {
                deal.RandomSet(nC, x, out numArray2, out numArray3);
            }
            else
            {
                deal.KennardStone(nC, x, out numArray2, out numArray3);
            }

            if (female)
            {

                femaleCalData = new double[numArray2.Length, data.GetLength(1)];
                femaleValData =new double[numArray3.Length, data.GetLength(1)];
                for (int j = 0; j < numArray2.Length; j++)
                {
                    for(int m=0;m<data.GetLength (1);m++)
                    {
                        femaleCalData[j, m] = data[numArray2[j], m];
                    }
                    
                }
                for (int k = 0; k < numArray3.Length; k++)
                {
                    for (int m = 0; m < data.GetLength(1); m++)
                    {
                        femaleValData[k, m] = data[numArray3[k], m];
                    }

                }
            }
            else
            {
                maleCalData = new double[numArray2.Length, data.GetLength(1)];
                maleValData = new double[numArray3.Length, data.GetLength(1)];
                for (int j = 0; j < numArray2.Length; j++)
                {
                    for (int m = 0; m < data.GetLength(1); m++)
                    {
                        maleCalData[j, m] = data[numArray2[j], m];
                    }

                }
                for (int k = 0; k < numArray3.Length; k++)
                {
                    for (int m = 0; m < data.GetLength(1); m++)
                    {
                        maleValData[k, m] = data[numArray3[k], m];
                    }

                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

    }
}
