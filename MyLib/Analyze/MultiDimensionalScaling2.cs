using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace MyWpfLib.Analyze
{
    public class MultiDimensionalScaling:MyLib2.Thread.BackgroundWorker
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
                mahalanobisDistance.AddData(data);
            }
        }

        public void AddData(MdsData data)
        {
            if (originalDic.ContainsKey(data.Name) == false)
            {
                originalDic.Add(data.Name, data);
                mahalanobisDistance.AddData(data.Data);
            }
        }

        private Dictionary<string, MdsData> originalDic = new Dictionary<string, MdsData>();

        public MultiDimensionalScaling()
        {
            this.DoWork += new System.ComponentModel.DoWorkEventHandler(MdsView_DoWork);
        }
        MyLib.MatrixLibrary.MahalanobisDistance mahalanobisDistance = new MyLib.MatrixLibrary.MahalanobisDistance();

        public void Clear()
        {
            originalDic.Clear();
            mahalanobisDistance.Clear();
        }

        void MdsView_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            MyLib.ListWithSortKey<double,MdsDataResult > sortList = new MyLib.ListWithSortKey<double,MdsDataResult>();
            for (int i = 1; i < tryCount+1; i++)
            {
                Dictionary<string, MdsData> dataDic = new Dictionary<string, MdsData>();
                foreach (var item in originalDic.Values)
                {
                    dataDic.Add(item.Name, new MdsData(item.Name, item.Data,this));
                }
                double err = Analyze(dataDic);
                sortList.Add(err, new MdsDataResult(err, dataDic));
                this.ReportProgress(i * 100 / tryCount);

            }

            sortList.Sort();
            result = sortList.GetValue(0);

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

        private double rate = 0.01F;

        public double Rate
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
                data.Point = new Point(random.Next(100), random.Next(100));
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
                        if (item.GetRealDistance(item1) > 0)
                        {
                            double errorterm = (item.GetFakedDistance(item1) - item.GetRealDistance(item1)) / item.GetRealDistance(item1);
                            double x = ((item.Point.X - item1.Point.X) / (double)item.GetFakedDistance(item1)) * (double)errorterm;
                            double y = ((item.Point.Y - item1.Point.Y) / (double)item.GetFakedDistance(item1)) * (double)errorterm;
                            item.AddGrad(new Point(x, y));
                            totalerror += Math.Abs(errorterm);
                        }
                    }
                }
                //    MyLib.Log.LogWriteLine("TotalError:" + totalerror.ToString());
                if (lasterror < totalerror )
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

        public double GetDistaince(MdsData d1, MdsData d2)
        {
            if(distanceType == DistainceType.Euclidean)
            {
                MyLib.MatrixLibrary.Vector v1 = new MyLib.MatrixLibrary.Vector(d1.Data);
                MyLib.MatrixLibrary.Vector v2 = new MyLib.MatrixLibrary.Vector(d2.Data);
                //Vector v1 = new Vector(d1.Data);
                //Vector v2 = new Vector(d2.Data);
                var v = v1-v2;
                return Math.Sqrt( v.SumSq);
            }
            if(distanceType == DistainceType.Mahalanobis)
            {
                return mahalanobisDistance.GetDistance(d1.Data,d2.Data);
            }
            else
            {
                return 0;
            }
        }

        private DistainceType distanceType = DistainceType.Euclidean;

        public DistainceType DistanceType
        {
          get { return distanceType; }
          set { distanceType = value; }
        }

        public enum DistainceType
	    {
            Euclidean,
            Mahalanobis
        }

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
                List<Point> list = new List<Point>();
                foreach (var item in resultDataDic.Values)
                {
                    list.Add(item.Point);
                }

                Point gPoint = MyWpfLib.Mathematics.Geometry.GetCenterOfGravity(list.ToArray());

                double max = double.MinValue;
                foreach (var item in resultDataDic.Values)
                {
                    double dis = MyWpfLib.Mathematics.Geometry.GetDistance(item.Point, gPoint);
                    if (max < dis)
                    {
                        max = dis;
                    }
                }

                foreach (var item in resultDataDic.Values)
                {
                    double x = (item.Point.X - gPoint.X) / max;
                    double y = (item.Point.Y - gPoint.Y) / max;
                    item.Point = new Point(x, y);
                }


            }

            public void CreateDrawPoint(Point centerPoint,double distance)
            {
                ReLayout();
                double dis = Math.Min(centerPoint.X, centerPoint.Y) * 0.8F;

                MyLib.Tool.LoopCounter lc = new MyLib.Tool.LoopCounter(resultDataDic.Count);
                foreach (var item in resultDataDic.Values)
                {
                    lc.GetPersent();
                    double x = centerPoint.X + item.Point.X * dis;
                    double y = centerPoint.Y + item.Point.Y * dis;
                    item.DrawPoint = new Point(x, y);
                }
            }

            public void CreateDrawPoint(double x,double y)
            {
                ReLayout();
                Point centerPoint = new Point(x / 2, y / 2);
       //         double dis = Math.Min(centerPoint.X, centerPoint.Y) * 0.8F;

                MyLib.Tool.LoopCounter lc = new MyLib.Tool.LoopCounter(resultDataDic.Count);
                foreach (var item in resultDataDic.Values)
                {
                    lc.GetPersent();
                    double xx = centerPoint.X + item.Point.X * x/2;
                    double yy = centerPoint.Y + item.Point.Y * y/2;
                    item.DrawPoint = new Point(xx, yy);
                }
            }
        }

        public class MdsData
        {
            public MdsData()
            {
            }

            public MdsData(string name, double[] data,MultiDimensionalScaling p)
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
            private Point point;

            public Point Point
            {
                get { return point; }
                set { point = value; }
            }

            private Point drawPoint;

            public Point DrawPoint
            {
                get { return drawPoint; }
                set { drawPoint = value; }
            }

            public dynamic Extend { get; set; }

            public MultiDimensionalScaling Parent{get;set;}

            private List<Point> gradList = new List<Point>();

            public void RevisePoint(double rate)
            {
                double x = 0;
                double y = 0;
                foreach (var p in gradList)
                {
                    x = p.X + x;
                    y = p.Y + y;
                }
                if (gradList.Count > 0)
                {
                    x = x / gradList.Count;
                    y = y / gradList.Count;
                }
                point.X = point.X - x * rate;
                point.Y = point.Y - y * rate;
                gradList.Clear();
            }
            public void AddGrad(Point p)
            {
                gradList.Add(p);
            }

            /// <summary>
            /// nama:X,Y の文字列を返す。
            /// </summary>
            /// <returns></returns>
            public string ToString2()
            {
                return name + ":" + MyLib.MathLib.NumberFormatToString(Point.X, 2) + "," + MyLib.MathLib.NumberFormatToString(Point.Y, 2);
            }

            private Dictionary<MdsData, double> realDistanceDic = new Dictionary<MdsData, double>();


            /// <summary>
            /// データ間の距離を取ってくる
            /// </summary>
            /// <param name="mds"></param>
            /// <returns></returns>
            public double GetRealDistance(MdsData mds)
            {
                if (realDistanceDic.ContainsKey(mds))
                {
                    return realDistanceDic[mds];
                }
                else
                {
//                    double d = Math.Pow(MyLib.MathLib.GetDistance(this.data, mds.data), pow);
                    double d = Parent.GetDistaince(this,mds);
                    realDistanceDic.Add(mds, d);
                    return d;
                }
            }


            public double GetFakedDistance(MdsData mds)
            {
                return Math.Sqrt(Math.Pow(this.Point.X - mds.Point.X, 2) + Math.Pow(this.Point.Y - mds.Point.Y, 2));
            }
        }
    }
}
