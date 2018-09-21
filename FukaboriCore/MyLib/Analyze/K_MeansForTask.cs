using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib.Analyze
{
    public class K_MeansData
    {
        public double[] Data { get; set; }
        public object Tag { get; set; }
    }

    public class K_MeansForTask
    {

        private List<K_MeansData> inputDataList = new List<K_MeansData>();

        public List<K_MeansData> InputDataList
        {
            get { return inputDataList; }
        }
        private List<Cluster> clusterDataList = new List<Cluster>();

        public List<Cluster> ClusterDataList
        {
            get { return clusterDataList; }
            set { clusterDataList = value; }
        }

        

        private int length;

        public K_MeansForTask(int l)
            : base()
        {
            length = l;

        }

        public K_MeansForTask()
            : base()
        {
        }

        public void SetDataLength(int l)
        {
            length = l;
        }


        int tryCount = 5;

        /// <summary>
        /// 何回試行するか。
        /// </summary>
        public int TryCount
        {
            get { return tryCount; }
            set
            {
                if (value > 0)
                {
                    tryCount = value;
                }
            }
        }
        bool cancellation = false;

        public bool Cancellation
        {
            get { return cancellation; }
            set { cancellation = value; }
        }

        public event EventHandler<MyLib.Event.Args<int>> ReportProgress;


        void K_Means_DoWork()
        {
            List<KeyValuePair<double, List<Cluster>>> list = new List<KeyValuePair<double, List<Cluster>>>();
            object obj = new object();
            for (int i = 0; i < tryCount; i++)
            {
                List<Cluster> tmpClusterList = new List<Cluster>();
                double kk = AnalyzeK_Means(clusterCount, out tmpClusterList);
                lock (obj)
                {
                    list.Add(new KeyValuePair<double, List<Cluster>>(kk, tmpClusterList));
                }
                //kk, tmpClusterList);
                if (ReportProgress != null)
                {
                    this.ReportProgress(this, new Event.Args<int>(list.Count * 100 / tryCount));
                }
   
            }

            //MyLib.Task.Parallel.ForEach(Enumerable.Range(1, tryCount), (n) =>
            //{

            //});
            if (this.Cancellation)
            {
                return;
            }
            var list2 = list.ToList();
            clusterDataList = list2.OrderBy(n => n.Key).First().Value;
        }




        public class Cluster
        {
            private List<K_MeansData> dataList = new List<K_MeansData>();

            public List<K_MeansData> DataList
            {
                get { return dataList; }
              //  set { dataList = value; }
            }
            public double[] clusterData;
            private int length;
            public string Name;
            public int Id;

            public Cluster(int length)
            {
                this.length = length;
            }

            public double SetNewPoint()
            {
                double[] tmpData = new double[length];
                for (int i = 0; i < length; i++)
                {
                    tmpData[i] = 0;
                }

                foreach (var item in dataList)
                {
                    for (int i = 0; i < length; i++)
                    {
                        tmpData[i] += item.Data[i];
                    }
                }
                for (int i = 0; i < length; i++)
                {
                    tmpData[i] = tmpData[i] / (double)dataList.Count;
                }
                double dis = double.NaN;
                if (clusterData != null)
                {
                    dis = MyLib.MathLib.GetDistance(clusterData, tmpData);
                }
                clusterData = tmpData;
                return dis;
            }
            Random random = new Random();
            public void SetRandom(double[] max, double[] min)
            {
                clusterData = new double[length];


                for (int i = 0; i < length; i++)
                {
                    clusterData[i] = random.NextDouble() * (max[i] - min[i]) + min[i];
                }
            }

            public void ListClear()
            {
                dataList.Clear();
            }

            public void AddData(K_MeansData data)
            {
                if (data.Data.Length == length)
                {
                    dataList.Add(data);
                }
                else
                {
                }
            }

            public int DataCount
            {
                get { return dataList.Count; }
            }

            public double GetDistance(double[] data)
            {
                return MyLib.MathLib.GetDistance(clusterData, data);
            }

            public double[] Data
            {
                get
                {
                    return clusterData;
                }
            }

            /// <summary>
            /// クラスタ重心と所属データとの距離の和を求めます、評価関数
            /// </summary>
            /// <returns></returns>
            public double GetClusterDistance()
            {
                double sum = 0;
                foreach (var item in dataList)
                {
                    sum += MyLib.MathLib.GetDistance(clusterData, item.Data);
                }
                if (dataList.Count > 0)
                {
                    return sum / (double)dataList.Count;
                }
                return 0;
            }

            public object Tag { get; set; }
        }


        int maxLoop = 50;

        public int MaxLoop
        {
            get { return maxLoop; }
            set { maxLoop = value; }
        }

        int clusterCount = 2;

        #region ごみ
        //private Dictionary<int, ClusterBox> clusterDic = new Dictionary<int, ClusterBox>();
        //public int[] selectCluster;

        //public struct ClusterBox
        //{
        //    public int[] MapCluster;
        //    public double Entropy;
        //    public double diffEntropy;
        //    public int ClusterCount;
        //    public double compareEntropy;

        //    public void SetDiffEntropy(double d)
        //    {
        //        diffEntropy = d;
        //    }
        //}




        //public K_Means(List<MapNeuron> mapList)
        //{
        //    this.mapList = mapList;
        //    if (mapList.Count > 0)
        //    {
        //        length = mapList[0].Neuron.WeightsLength;
        //    }
        //}

        //public void CreateClusterDic(int maxClusterNum)
        //{
        //    clusterDic.Clear();
        //    double max = 0;

        //    for (int i = 2; i <= maxClusterNum; i++)
        //    {
        //        ClusterBox cBox = new ClusterBox();
        //        cBox.ClusterCount = i;
        //        cBox.MapCluster = Run(i);
        //        cBox.Entropy = GetEntropy(cBox.MapCluster);
        //        cBox.diffEntropy = cBox.Entropy - max;
        //        double p = (double)1 / (double)i;
        //        double e = (p * Math.Log(p, 2) * i)*-1;
        //        cBox.compareEntropy = cBox.Entropy/ e;
        //        max = Math.Max(max, cBox.Entropy);
        //        clusterDic.Add(i, cBox);
        //    }


        //}

        //public ClusterBox GetClusterBox(int clusterNum)
        //{
        //    if (clusterDic.ContainsKey(clusterNum))
        //    {
        //        return clusterDic[clusterNum];
        //    }
        //    else
        //    {
        //        return new ClusterBox();
        //    }
        //}

        //private double GetEntropy(int[] clusterList)
        //{
        //    Dictionary<int, MyLib.CountClass> dic = new Dictionary<int, MyLib.CountClass>();

        //    for (int i = 0; i < clusterList.Length; i++)
        //    {
        //        if (dic.ContainsKey(clusterList[i]))
        //        {
        //            dic[clusterList[i]].Add(mapList[i].InputCollection.Count);
        //        }
        //        else
        //        {
        //            MyLib.CountClass cc = new MyLib.CountClass();
        //            cc.Add(mapList[i].InputCollection.Count);
        //            dic.Add(clusterList[i], cc);
        //        }
        //    }
        //    //foreach (int c in clusterList)
        //    //{
        //    //    if (dic.ContainsKey(c))
        //    //    {
        //    //        dic[c].Add();
        //    //    }
        //    //    else
        //    //    {
        //    //        MyLib.CountClass cc = new MyLib.CountClass();
        //    //        cc.Add();
        //    //        dic.Add(c, cc);
        //    //    }
        //    //}
        //    int sum = 0;
        //    foreach (int c in dic.Keys)
        //    {
        //        sum += dic[c].Count;
        //    }
        //    double entropy = 0;
        //    foreach (int c in dic.Keys)
        //    {
        //        double p = (double)dic[c].Count/(double)sum;
        //        if (p > 0)
        //        {
        //            entropy = entropy + p * Math.Log(p, 2);
        //        }
        //    }
        //    return entropy*-1;
        //}
        //public void SetSelectCluster(int num)
        //{
        //    if (clusterDic.ContainsKey(num))
        //    {
        //        selectCluster = clusterDic[num].MapCluster;
        //    }
        //    else
        //    {
        //        selectCluster = Run(num);
        //    }
        //}
        //public int[] Run(int clusterCount)
        //{

        //    clusterCenterList = new List<Neuron>();
        //    Random random = new Random();
        //    for (int i = 0; i < clusterCount; i++)
        //    {
        //        int tmp = mapList.Count * i / clusterCount;
        //        int tmp2 = mapList.Count * (i + 1) / clusterCount;
        //        int tmp3 = (tmp + tmp2) / 2;
        //        Neuron n = new Neuron(length);
        //        n.Tag = new List<double[]>();
        //        n.SetWeight((double[])mapList[tmp3].Neuron.Weights.Clone());
        //        clusterCenterList.Add(n);

        //    }
        //    double sum = 1;
        //    while (sum > 0.01)
        //    {

        //        foreach (Neuron neuron in clusterCenterList)
        //        {
        //            ((List<double[]>)neuron.Tag).Clear();
        //        }
        //        foreach (MapNeuron mapNeuron in mapList)
        //        {
        //            double min = 10000;
        //            Neuron minNeuron = null;
        //            foreach (Neuron neuron in clusterCenterList)
        //            {
        //                double dis = MyLib.MathLib.GetDistance(mapNeuron.Neuron.Weights, neuron.Weights);
        //                if (dis < min)
        //                {
        //                    min = dis;
        //                    minNeuron = neuron;
        //                }
        //            }
        //            if (minNeuron != null)
        //            {
        //                ((List<double[]>)minNeuron.Tag).Add(mapNeuron.Neuron.Weights);
        //            }
        //        }

        //        sum = 0;
        //        foreach (Neuron neuron in clusterCenterList)
        //        {
        //            double[] avg = MyLib.MathLib.GetAverage((List<double[]>)neuron.Tag);
        //            double d = MyLib.MathLib.GetDistance(neuron.Weights, avg);
        //            sum = sum + d;
        //            neuron.SetWeight(avg);
        //        }
        //    }

        //    int count = 0;

        //    List<int> list = new List<int>();
        //    foreach (MapNeuron mapNeuron in mapList)
        //    {
        //        double min = 10000;
        //        Neuron minNeuron = null;
        //        int c = 0;
        //        int cc = 0;
        //        foreach (Neuron neuron in clusterCenterList)
        //        {
        //            double dis = MyLib.MathLib.GetDistance(mapNeuron.Neuron.Weights, neuron.Weights);
        //            if (dis < min)
        //            {
        //                min = dis;
        //                minNeuron = neuron;
        //                cc = c;
        //            }
        //            c++;
        //        }
        //        if (minNeuron != null)
        //        {
        //            list.Add(cc);
        //        }
        //    }
        //    return list.ToArray();
        //}

        #endregion

        public void AddData(double[] data,object tag)
        {
            if (data.Length == length)
            {
                inputDataList.Add(new K_MeansData() { Data = data, Tag = tag });
            }
            else
            {
            }
        }

        public void ClearData()
        {
            inputDataList.Clear();
        }

        private Cluster SearchCluster(double[] data, List<Cluster> list)
        {
            if (data.Length == length)
            {
                Cluster tmpCluster = null;
                double dis = double.MaxValue;

                foreach (Cluster item in list)
                {
                    double tmpDis = item.GetDistance(data);
                    if (dis > tmpDis)
                    {
                        dis = tmpDis;
                        tmpCluster = item;
                    }
                }
                return tmpCluster;
            }
            else
            {
                return null;
            }

        }
        private RandamType rType = RandamType.最小距離を使う;

        public RandamType Randamtype
        {
            get { return rType; }
            set { rType = value; }
        }


        public enum RandamType
        {
            複数の平均,
            一つのデータ,
            最小距離を使う
        }

        /// <summary>
        /// データに割り当てられるクラスターを探します。見つからなかったとき、データが不正のときはNullを返します。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Cluster SearchCluster(double[] data)
        {
            return SearchCluster(data, clusterDataList);
        }
        Random random = new Random();
        /// <summary>
        /// ランダムに割り当てて、クラスタを計算し、評価関数を返します。
        /// </summary>
        /// <param name="clusterCount"></param>
        /// <param name="clusterList"></param>
        /// <param name="maxLoop"></param>
        /// <returns></returns>
        private double AnalyzeK_Means(int clusterCount, out List<Cluster> clusterList)
        {
            clusterList = new List<Cluster>();
            for (int i = 0; i < clusterCount; i++)
            {
                Cluster cluster = new Cluster(length);
                clusterList.Add(cluster);
            }

            if (rType == RandamType.一つのデータ)
            {

                var shuffleList = inputDataList.OrderBy(n => Guid.NewGuid()).Select(n => n);


                int c = 0;
                foreach (var item in shuffleList)
                {
                    bool flag = true;
                    foreach (var item2 in clusterList)
                    {
                        if (item2.Data != null && item2.GetDistance(item.Data) == 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        clusterList[c].AddData(item);
                        c++;
                        if (c == clusterCount)
                        {
                            break;
                        }
                    }

                }

                List<Cluster> tmplist = new List<Cluster>();

                foreach (var item in clusterList)
                {
                    if (item.DataCount == 0)
                    {
                        tmplist.Add(item);
                    }
                }
                foreach (var item in tmplist)
                {
                    clusterList.Remove(item);
                }
            }
            if (rType == RandamType.複数の平均)
            {
                //for (int i = 0; i < Math.Max(clusterCount,inputDataList.Count/3); i++)
                //{
                //    clusterList[i%clusterCount].AddData(shuffleList[i]);                
                //}

                foreach (var item in inputDataList)
                {
                    int id = random.Next(clusterCount);
                    clusterList[id].AddData(item);
                }
            }
            if (rType == RandamType.最小距離を使う)
            {
                int nCount = 0;
                while (nCount < 3)
                {
                    var seed = inputDataList.OrderBy(n => Guid.NewGuid()).First();
                    clusterList[0].AddData(seed);
                    clusterList[0].SetNewPoint();
                    for (int i = 1; i < clusterCount; i++)
                    {
                        List<KeyValuePair<K_MeansData, double>> tmpList = new List<KeyValuePair<K_MeansData, double>>();
                        for (int l = 0; l < inputDataList.Count; l++)
                        {
                            double min = double.MaxValue;
                            for (int k = 0; k < i; k++)
                            {
                                min = Math.Min(clusterList[k].GetDistance(inputDataList[l].Data), min);
                            }
                            if (min > 0)
                            {
                                tmpList.Add(new KeyValuePair<K_MeansData, double>(inputDataList[l], min));
                            }
                        }
                        var tmp2 = new List<K_MeansData>(tmpList.OrderBy(n => n.Value).Select(n => n.Key));
                        if (tmp2.Count > 0 && i < clusterList.Count)
                        {
                            var tmp = tmp2[tmp2.Count / 2];
                            clusterList[i].AddData(tmp);
                            clusterList[i].SetNewPoint();
                        }
                        else
                        {
                            break;
                        }
                    }
                    List<Cluster> tmplist = new List<Cluster>();
                    foreach (var item in clusterList)
                    {
                        if (item.DataCount == 0)
                        {
                            tmplist.Add(item);
                        }
                    }
                    foreach (var item in tmplist)
                    {
                        clusterList.Remove(item);
                    }
                    if (tmplist.Count == 0) break;
                    nCount++;
                }
            }


            foreach (Cluster item in clusterList)
            {
                item.SetNewPoint();
            }



            double distance = 100;
            int count = 0;
            while (distance > 0)
            {
                if (this.Cancellation)
                {
                    break;
                }
                foreach (Cluster item in clusterList)
                {
                    item.ListClear();
                }
                foreach (var data in inputDataList)
                {
                    Cluster tmpCluster = SearchCluster(data.Data, clusterList);

                    if (tmpCluster != null)
                    {
                        tmpCluster.AddData(data);
                    }
                }

                distance = 0;
                foreach (Cluster item in clusterList)
                {
                    if (item.DataCount > 0)
                    {
                        distance += item.SetNewPoint();
                    }
                }
                count++;
                if (count > maxLoop)
                {
                    break;
                }
            }

            double sum = 0;
            foreach (Cluster item in clusterList)
            {
                sum += item.GetClusterDistance();
            }

            return sum / clusterList.Count ;

        }

        /// <summary>
        /// K-Meansを実行します。
        /// </summary>
        /// <param name="clusterCount">クラスタ数</param>
        /// <param name="tryCount">試行回数</param>
        public void Run(int clusterCount, int tryCount, Action<List<Cluster>> Completed)
        {
            clusterDataList.Clear();
            this.clusterCount = clusterCount;
            this.tryCount = tryCount;

            if (this.InputDataList.Count > 0)
            {
                Task<List<Cluster>>.Factory.StartNew(() => { K_Means_DoWork(); return this.ClusterDataList; }).ContinueWith((n) => Completed(n.Result));
            }
        }

        public List<Cluster> Run(int clusterCount, int tryCount)
        {
            clusterDataList.Clear();
            this.clusterCount = clusterCount;
            this.tryCount = tryCount;
            if (this.InputDataList.Count > 0)
            {
                K_Means_DoWork();
            }
            return ClusterDataList;
        }


    }

}
