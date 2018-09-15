using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MyLib.Mathematics;

namespace Tool.MDS
{
    public　class MdsAnalyze
    {

        private MdsDataResult result;

        public MdsDataResult Result
        {
            get { return result; }
        }

        public void AddData(string name, double[] data)
        {
            if (originalDic.ContainsKey(name) == false)
            {
                originalDic.Add(name, new MdsData(name, data,this));
            }
        }
        private Dictionary<string, MdsData> originalDic = new Dictionary<string, MdsData>();

        public MdsAnalyze()
        {
            
        }

        public void Clear()
        {
            originalDic.Clear();
        }

        public Action<int> ReportProgress { get; set; }
        protected void OnReportProgress(int i)
        {
            if(ReportProgress !=null)
            {
                ReportProgress(i);
            }
        }

        public void Run()
        {
            List<MdsDataResult> sortList = new List<MdsDataResult>();
            for (int i = 1; i < tryCount+1; i++)
            {
                Dictionary<string, MdsData> dataDic = new Dictionary<string, MdsData>();
                foreach (var item in originalDic.Values)
                {
                    dataDic.Add(item.Name, new MdsData(item.Name, item.Data,this));
                }
                double err = Analyze(dataDic);
                sortList.Add( new MdsDataResult(err, dataDic));
                OnReportProgress(i * 100 / tryCount);

            }
            result = sortList.OrderBy(n => n.Totalerror).First();
        }

        private int tryCount = 10;

        public int TryCount
        {
            get { return tryCount; }
            set { tryCount = value; }
        }

        public void ClearData()
        {
            originalDic.Clear();
        }

        private float rate = 0.01F;

        public float Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        private double pow = 1;

        public double Pow
        {
            get { return pow; }
            set { pow = value; }
        }

        public double Analyze(Dictionary<string, MdsData> dataDic)
        {
            Random random = new Random();
            foreach (MdsData data in dataDic.Values)
            {
                data.PointF = new PointF(random.Next(100), random.Next(100));
            }
            double lasterror = double.MaxValue;
            for (int i = 0; i < 10000; i++)
            {
                double totalerror = 0;
                foreach (var item in dataDic.Values)
                {
                    foreach (var item1 in dataDic.Values)
                    {
                        if (item == item1)
                        {
                            continue;
                        }
                        double errorterm = (item.GetFakedDistance(item1) - item.GetRealDistance(item1, pow)) / item.GetRealDistance(item1, pow);
                        float x = ((item.PointF.X - item1.PointF.X) / (float)item.GetFakedDistance(item1)) * (float)errorterm;
                        float y = ((item.PointF.Y - item1.PointF.Y) / (float)item.GetFakedDistance(item1)) * (float)errorterm;
                        item.AddGrad(new PointF(x, y));
                        totalerror += Math.Abs(errorterm);
                    }
                }
                //    MyLib.Log.LogWriteLine("TotalError:" + totalerror.ToString());
                if (lasterror < totalerror)
                {
                    break;
                }
                foreach (var item in dataDic.Values)
                {
                    item.RevisePoint(rate);
                }
                //                
            }
            return lasterror;
        }



        //public double GetDistaince(MdsData d1, MdsData d2)
        //{

        //}


        public class MdsDataResult
        {
            private Dictionary<string, MdsData> resultDataDic = new Dictionary<string, MdsData>();

            public MdsDataResult(double error, Dictionary<string, MdsData> resultDataDic)
            {
                totalerror = error;
                this.resultDataDic = resultDataDic;
            }

            public Dictionary<string, MdsData> DataDic
            {
                get { return resultDataDic; }
                set { resultDataDic = value; }
            }
            private double totalerror;

            public double Totalerror
            {
                get { return totalerror; }
                set { totalerror = value; }
            }


            /// <summary>
            /// 再配置する。重心を求めてそこを中心に組み立てる。
            /// </summary>
            private void ReLayout()
            {
                List<PointF> list = new List<PointF>();
                foreach (var item in resultDataDic.Values)
                {
                    list.Add(item.PointF);
                }

                PointF gPoint = MyLib.Mathematics.Geometry.GetCenterOfGravity(list.ToArray());

                float max = float.MinValue;
                foreach (var item in resultDataDic.Values)
                {
                    float dis = MyLib.Mathematics.Geometry.GetDistance(item.PointF, gPoint);
                    if (max < dis)
                    {
                        max = dis;
                    }
                }

                foreach (var item in resultDataDic.Values)
                {
                    float x = (item.PointF.X - gPoint.X) / max;
                    float y = (item.PointF.Y - gPoint.Y) / max;
                    item.PointF = new PointF(x, y);
                }


            }

            public void CreateDrawPoint(PointF centerPoint, double distance)
            {
                ReLayout();
                float dis = Math.Min(centerPoint.X, centerPoint.Y) * 0.8F;

                foreach (var item in resultDataDic.Values)
                {
                    float x = centerPoint.X + item.PointF.X * dis;
                    float y = centerPoint.Y + item.PointF.Y * dis;
                    item.DrawPointF = new PointF(x, y);
                }
            }

        }



        public class MdsData
        {
            public MdsData()
            {
            }

            public MdsData(string name, double[] data, MdsAnalyze p)
            {
                this.name = name;
                this.data = data;
                this.Parent = p;
            }

            private double[] data;

            public double[] Data
            {
                get { return data; }
                set { data = value; }
            }
            private string name;


            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            private PointF pointF;

            public PointF PointF
            {
                get { return pointF; }
                set { pointF = value; }
            }

            private PointF drawPointF;

            public PointF DrawPointF
            {
                get { return drawPointF; }
                set { drawPointF = value; }
            }

            public MdsAnalyze Parent;

            private List<PointF> gradList = new List<PointF>();

            public void RevisePoint(float rate)
            {
                float x = 0;
                float y = 0;
                foreach (var p in gradList)
                {
                    x = p.X + x;
                    y = p.Y + y;
                }
                x = x / gradList.Count;
                y = y / gradList.Count;

                pointF.X = pointF.X - x * rate;
                pointF.Y = pointF.Y - y * rate;
                gradList.Clear();
            }
            public void AddGrad(PointF p)
            {
                gradList.Add(p);
            }

            /// <summary>
            /// nama:X,Y の文字列を返す。
            /// </summary>
            /// <returns></returns>
            public string ToString2()
            {
                return name + ":" + MyLib.MathLib.NumberFormatToString(pointF.X, 2) + "," + MyLib.MathLib.NumberFormatToString(pointF.Y, 2);
            }

            private Dictionary<MdsData, double> realDistanceDic = new Dictionary<MdsData, double>();
            private Dictionary<MdsData, double> virtualDistanceDic = new Dictionary<MdsData, double>();




            /// <summary>
            /// データ間の距離を取ってくる
            /// </summary>
            /// <param name="mds"></param>
            /// <returns></returns>
            public double GetRealDistance(MdsData mds, double pow)
            {
                if (realDistanceDic.ContainsKey(mds))
                {
                    return realDistanceDic[mds];
                }
                else
                {
                    double d = Math.Pow(MyLib.MathLib.GetDistance(this.data, mds.data), pow);
                    realDistanceDic.Add(mds, d);
                    return d;
                }
            }


            public double GetFakedDistance(MdsData mds)
            {
                return Math.Sqrt(Math.Pow(this.PointF.X - mds.PointF.X, 2) + Math.Pow(this.PointF.Y - mds.PointF.Y, 2));
            }
        }
    }
}
