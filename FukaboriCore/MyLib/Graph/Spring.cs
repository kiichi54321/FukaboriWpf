using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace MyWpfLib.Graph
{

    public class Spring : System.ComponentModel.BackgroundWorker
    {
        public Spring()
            : base()
        {
            backgroundWorker = this;
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_DoWork);
        }


        

        System.ComponentModel.BackgroundWorker backgroundWorker;
        void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (springType == SpringEnum.Large)
            {
                SpringCalculateRepeatWithGensui(20, 1.5);
                SpringCalculateRepeatWithGensui(20, 0.8);
                SpringCalculateRepeatWithGensui(20, 0.3);
            }
            else if (springType == SpringEnum.Medium)
            {
                SpringCalculateRepeatWithGensui(20, 0.8);
                SpringCalculateRepeatWithGensui(20, 0.3);
            }
            else if (springType == SpringEnum.Small)
            {
                SpringCalculateRepeatWithGensui(20, 0.3);
            }
            else
            {
                foreach (var item in springDataList)
                {
                    SpringCalculateRepeatWithGensui(item.Count, item.Keisu);
                }
            }
//            NodeUpdate();
        }

        private List<SpringData> springDataList = new List<SpringData>();

        public class SpringData
        {
            public int Count { get; set; }
            public double Keisu { get; set; }

            public SpringData()
            {
            }
            public SpringData(int count, double keisu)
            {
                this.Count = count;
                this.Keisu = keisu;
            }
        }


        public void NodeUpdate()
        {
            foreach (var item in nodeDic.Values)
            {
                item.UpDate();
            }
        }

        SpringEnum springType = SpringEnum.Medium;
        public SpringEnum SpringType
        {
            get { return springType; }
            set { springType = value; }
        }

        public enum SpringEnum
        {
            Small, Large, Medium,Custom
        }


        /// <summary>
        /// RunWorkerAsync()を呼び出します。
        /// </summary>
        public void Run()
        {
            if (backgroundWorker.IsBusy == false)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        public void Run(int count, double keisu)
        {
            springType = SpringEnum.Custom;
            springDataList.Clear();
            springDataList.Add(new SpringData(count, keisu));
            backgroundWorker.RunWorkerAsync();

        }

        private MyWpfLib.Draw.Area clientRectangle = new Draw.Area();

        public MyWpfLib.Draw.Area ClientArea
        {
            get { return clientRectangle; }
            set { clientRectangle = value; }
        }

        private Dictionary<Node, TmpNode> nodeDic = new Dictionary<Node, TmpNode>();


        public void Clear()
        {
            nodeDic.Clear();
        }

        public void AddNode(Node node)
        {
            if (nodeDic.ContainsKey(node) == false)
            {
                nodeDic.Add(node, new TmpNode(node));
            }
        }

        public void SetNode(ICollection<Node> nodes)
        {
            nodeDic.Clear();
            foreach (var item in nodes)
            {
                if (nodeDic.ContainsKey(item) == false)
                {
                    nodeDic.Add(item, new TmpNode(item));
                }
            }
        }

        //bool reLayout = true;

        ///// <summary>
        ///// センタリングの再配置をするか？
        ///// </summary>
        //public bool ReLayout
        //{
        //    get { return reLayout; }
        //    set { reLayout = value; }
        //}

        int stretchToEdegPower = 5;

        /// <summary>
        /// 外へ引っ張る力
        /// </summary>
        public int StretchToEdegPower
        {
            get { return stretchToEdegPower; }
            set { stretchToEdegPower = value; }
        }

        int attractivePower = 80;

        /// <summary>
        /// 引っ張る力
        /// </summary>
        public int AttractivePower
        {
            get { return attractivePower; }
            set { attractivePower = value; }
        }

        int reoulsivePower = 60;

        public int ReoulsivePower
        {
            get { return reoulsivePower; }
            set { reoulsivePower = value; }
        }

        private bool centering = true;

        public bool Centering
        {
            get { return centering; }
            set { centering = value; }
        }

        private bool useStretchToEdegPower = true;

        /// <summary>
        /// 端に引っ張る力を使う
        /// </summary>
        public bool UseStretchToEdegPower
        {
            get { return useStretchToEdegPower; }
            set { useStretchToEdegPower = value; }
        }


        /// <summary>
        /// バネ計算
        /// </summary>
        private void SpringCalculateWithGensui(double gensui)
        {


            if (centering)
            {
                //画面の中心に持ってきて端に引っ張る
                CenterConvert(gensui * StretchToEdegPower);
            }

            //引きつける
            foreach (var node in nodeDic.Values)
            {
                foreach (Node item in node.BaseNode.LinkedNodes)
                {
                    TmpNode tmpNode;
                    if (nodeDic.TryGetValue(item, out tmpNode))
                    {
                        AttractiveByPointDiff(node, tmpNode, attractivePower, gensui);
                    }
                }
            }


            foreach (var node in nodeDic.Values)
            {
                node.SetPoint();
            }

            //引き離す
            foreach (var node in nodeDic.Values)
            {
                foreach (var node1 in nodeDic.Values)
                {
                    if (node.Equals(node1) == false)
                    {
                        ReoulsiveByPointDiff(node, node1, ReoulsivePower);
                    }
                }
            }

            foreach (var node in nodeDic.Values)
            {
                node.SetPoint();
            }

            BorderJudgment();

        }

        /// <summary>
        /// バネ計算をcount数繰り返す。
        /// </summary>
        /// <param name="count"></param>
        public void SpringCalculateRepeatWithGensui(int count, double keisu)
        {

            double gensui = 1;
            for (int i = 0; i < count; i++)
            {
                gensui = (double)(1.01 - i / count) * keisu;
                SpringCalculateWithGensui(gensui);
                if (count % 5 == 0)
                {
                    this.ReportProgress(i*100/count);
                }
            }
        }

        /// <summary>
        /// 引きつける(減衰パラメータ付)
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="keisu"></param>
        private void AttractiveByPointDiff(TmpNode item1, TmpNode item2, double keisu, double gensui)
        {
            Point point1 = item1.CenterPoint;
            Point point2 = item2.CenterPoint;
            keisu = keisu / (MyWpfLib.Mathematics.Geometry.GetDistance(point1, point2) * 2);
            Point point1_new = new Point(0, 0);
            Point point2_new = new Point(0, 0);

            point2_new.X = (point1.X + point2.X) / 2 + (point2.X - point1.X) * keisu;
            point2_new.Y = (point1.Y + point2.Y) / 2 + (point2.Y - point1.Y) * keisu;
            point1_new.X = (point1.X + point2.X) / 2 - (point2.X - point1.X) * keisu;
            point1_new.Y = (point1.Y + point2.Y) / 2 - (point2.Y - point1.Y) * keisu;

            Point point1_new2 = new Point(0, 0);
            Point point2_new2 = new Point(0, 0);

            point1_new2.X = (point1_new.X - point1.X) * gensui;
            point1_new2.Y = (point1_new.Y - point1.Y) * gensui;
            point2_new2.X = (point2_new.X - point2.X) * gensui;
            point2_new2.Y = (point2_new.Y - point2.Y) * gensui;


            item1.AddPoint(point1_new2);
            item2.AddPoint(point2_new2);

        }

        /// <summary>
        /// 引き離す(減衰パラメータ付)差分追加型
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="keisu"></param>
        private void ReoulsiveByPointDiff(TmpNode item1, TmpNode item2, double keisu)
        {
            Point point1 = item1.CenterPoint;
            Point point2 = item2.CenterPoint;
            double dis = MyWpfLib.Mathematics.Geometry.GetDistance(point1, point2);
            double keisu2 = keisu / dis;
            if (dis == 0)
            {
                Random r = new Random((int)(point1.X * point1.Y));

                item1.AddPoint(new Point((double)r.NextDouble() * 10 - 5, (double)r.NextDouble() * 10 - 5));
                item2.AddPoint(new Point((double)r.NextDouble() * 10 - 5, (double)r.NextDouble() * 10 - 5));
            }
            if (keisu2 > 1)
            {
                Point point1_new = new Point(0, 0);
                Point point2_new = new Point(0, 0);

                keisu = keisu2 / 2;

                point2_new.X = (point1.X + point2.X) / 2 + (point2.X - point1.X) * keisu;
                point2_new.Y = (point1.Y + point2.Y) / 2 + (point2.Y - point1.Y) * keisu;
                point1_new.X = (point1.X + point2.X) / 2 - (point2.X - point1.X) * keisu;
                point1_new.Y = (point1.Y + point2.Y) / 2 - (point2.Y - point1.Y) * keisu;

                Point point1_new2 = new Point(0, 0);
                Point point2_new2 = new Point(0, 0);

                point1_new2.X = (point1_new.X - point1.X);
                point1_new2.Y = (point1_new.Y - point1.Y);
                point2_new2.X = (point2_new.X - point2.X);
                point2_new2.Y = (point2_new.Y - point2.Y);

                item1.AddPoint(point1_new2);
                item2.AddPoint(point2_new2);

            }
        }

        /// <summary>
        /// 中心点を求める。
        /// </summary>
        /// <returns></returns>
        private Point GetCenterPoint()
        {
            double x = 0, y = 0;
            foreach (var node in nodeDic.Values)
            {
                x += node.NodeX;
                y += node.NodeY;
            }
            int count = nodeDic.Count;
            return new Point(x / count, y / count);
        }


        /// <summary>
        /// 画面の端に引っ張るようにさせる。
        /// </summary>
        /// <param name="keisu"></param>
        private void CenterConvert(double keisu)
        {
            Point mapCenter = new Point(Math.Abs(clientRectangle.Right - clientRectangle.Left) / 2, Math.Abs(clientRectangle.Top - clientRectangle.Bottom) / 2);
            Point nodeCenter = GetCenterPoint();
            //                Size dif = new Size(mapCenter.X,mapCenter.Y) - new Size(nodeCenter.X,nodeCenter.Y);

            foreach (TmpNode node in nodeDic.Values)
            {
                if (node.BaseNode.LinkedNodes.Count > 0)
                {
                    double kakudo = Math.Atan2((double)(node.NodeY - nodeCenter.Y), (double)(node.NodeX - nodeCenter.X));
                    node.NodeX = (node.NodeX - nodeCenter.X) + mapCenter.X + (double)Math.Cos(kakudo) * keisu;
                    node.NodeY = (node.NodeY - nodeCenter.Y) + mapCenter.Y + (double)Math.Sin(kakudo) * keisu;
                }
                else
                {
                    node.NodeX = (node.NodeX - nodeCenter.X) + mapCenter.X;
                    node.NodeY = (node.NodeY - nodeCenter.Y) + mapCenter.Y;
                }
            }
        }

        private int margin = 40;

        /// <summary>
        /// 余白
        /// </summary>
        public int Margin
        {
            get { return margin; }
            set { margin = value; }
        }

        /// <summary>
        /// 端っこに行ったノードを戻す
        /// </summary>
        private void BorderJudgment()
        {
            foreach (TmpNode node in nodeDic.Values)
            {
                if (node.NodeX < Margin + clientRectangle.Left)
                {
                    node.NodeX = (double)(Margin + clientRectangle.Left + Math.Abs(node.NodeX - Margin + clientRectangle.Left) * 0.5);
                }
                else if (node.NodeX > ClientArea.Right - Margin)
                {
                    node.NodeY = (double)(ClientArea.Right - Margin - Math.Abs(node.NodeX - (ClientArea.Right - Margin)) * 0.5);
                }
                if (node.NodeY < Margin + ClientArea.Top)
                {
                    node.NodeY = (double)(Margin + ClientArea.Top + Math.Abs(node.NodeY - Margin - ClientArea.Top) * 0.5);
                }
                else if (node.NodeY > ClientArea.Bottom - Margin)
                {
                    node.NodeY = (double)(ClientArea.Bottom - Margin - Math.Abs(node.NodeY - (ClientArea.Bottom - Margin)) * 0.5);
                }


            }
        }

        public void AutoParameter()
        {
            List<double> attractivePowerList = new List<double>();
            foreach (var item in nodeDic)
            {               
                foreach (var item2 in item.Value.BaseNode.LinkedNodes)
                {
                    attractivePowerList.Add(MyWpfLib.Mathematics.Geometry.GetDistance(nodeDic[(Node)item2].CenterPoint, item.Value.CenterPoint));
                }
            }
            attractivePowerList = attractivePowerList.OrderBy(n => n).ToList();
            attractivePower = (int)attractivePowerList[attractivePowerList.Count / 2];

            List<double> reoulsivePowerList = new List<double>();

            foreach (var item in nodeDic.Values)
            {
                foreach (var item2 in nodeDic.Values.SkipWhile(n=>n != item).Skip(1))
                {
                    reoulsivePowerList.Add(MyWpfLib.Mathematics.Geometry.GetDistance(item2.CenterPoint, item.CenterPoint));
                }
            }
            reoulsivePower = (int)reoulsivePowerList.Min();

        }


        public class TmpNode : Node
        {
            private Node node;
            public TmpNode(Node node)
            {
                this.node = node;
                this.NodeX = node.NodeX;
                this.NodeY = node.NodeY;
                this.LockPosition = node.LockPosition;
            }

            private List<Point> pointList = new List<Point>();

            public void SetPoint()
            {
                double x = 0;
                double y = 0;
                foreach (var item in pointList)
                {
                    if (x != double.NaN && y != double.NaN)
                    {
                        x += item.X;
                        y += item.Y;
                    }
                }
                if (pointList.Count > 0)
                {
                    this.CenterPoint = new Point(this.NodeX + x / pointList.Count, this.NodeY + y / pointList.Count);
                }
                pointList.Clear();
            }

            public void UpDate()
            {
                node.NodeX = this.NodeX;
                node.NodeY = this.NodeY;
            }

            public Node BaseNode
            {
                get { return node; }
            }

            //public Point Point
            //{
            //    get
            //    {
            //        return node.Point;
            //    }
            //    set
            //    {
            //        node.Point = value;
            //    }
            //}

            //public double X
            //{
            //    get { return node.X; }
            //    set { node.X = value; }
            //}

            //public double Y
            //{
            //    get { return node.Y; }
            //    set { node.Y = value; }
            //}

            public void AddPoint(Point point)
            {
                pointList.Add(point);
            }

        }

    }



}



