using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace MySilverlightLibrary.Neural
{
    class SimpleNeural
    {

    }
    
    public abstract class NeuralNetworkBase
    {
        
        public int count_train
        {
            get; set;
        }
        
        public int count_test
        {
            get; set;
        }
        
        public int count_unit_in { get; set; }
        
        public int count_unit_hidden { get; set; }
        
        public int count_unit_out { get; set; }
        
        public double alpha { get; set; }
        
        public double eta { get; set; }
        
        public double[,] output_in { get; set; }
        
        public double[] output_hidden { get; set; }
        
        public double[] output_out { get; set; }
        
        public double[,] weight_in_to_hidden { get; set; }
        
        public double[,] weight_hidden_to_out { get; set; }
        
        public double[,] weight_modify_in_to_hidden { get; set; }
        
        public double[,] weight__modify_hidden_to_out { get; set; }
        
        public double[] bias_hidden { get; set; }
        
        public double[] bias_out { get; set; }
        
        public double[] bias_modify_hidden { get; set; }
        
        public double[] bias_modify_out { get; set; }
        
        public double[,] teacher_signal { get; set; }

        /// <summary>
        /// ネットワークの土台を構築する
        /// </summary>
        /// <param name="count_train">訓練用データの数</param>
        /// <param name="count_test">テスト用データの数</param>
        /// <param name="count_unit_in">入力層のユニット数</param>
        /// <param name="count_unit_hidden">隠れ層のユニット数</param>
        /// <param name="count_unit_out">出力層のユニット数</param>
        public NeuralNetworkBase(int count_train, int count_test, int count_unit_in, int count_unit_hidden, int count_unit_out, double alpha, double eta)
        {
            this.count_train = count_train;
            this.count_test = count_test;
            this.count_unit_in = count_unit_in;
            this.count_unit_hidden = count_unit_hidden;
            this.count_unit_out = count_unit_out;

            this.output_in = new double[count_train + count_test, count_unit_in];
            this.output_hidden = new double[count_unit_hidden];
            this.output_out = new double[count_unit_out];

            this.weight_in_to_hidden = new double[count_unit_hidden, count_unit_in];
            this.weight_hidden_to_out = new double[count_unit_out, count_unit_hidden];
            this.weight_modify_in_to_hidden = new double[count_unit_hidden, count_unit_in];
            this.weight__modify_hidden_to_out = new double[count_unit_out, count_unit_hidden];

            this.bias_hidden = new double[count_unit_hidden];
            this.bias_out = new double[count_unit_out];
            this.bias_modify_hidden = new double[count_unit_hidden];
            this.bias_modify_out = new double[count_unit_out];
            this.teacher_signal = new double[count_train + count_test, count_unit_out];

            this.alpha = alpha;
            this.eta = eta;
        }

        public NeuralNetworkBase() { }

        public abstract void Initialize();
        public abstract void ForwardPropagation(int dataIndex);
        public abstract void BackPropagation(int dataIndex);
    }

    
    public class NeuralNetworkCore : NeuralNetworkBase
    {
        /// <summary>
        /// ネットワークの土台を構築する
        /// </summary>
        /// <param name="Y1">訓練用データの数</param>
        /// <param name="Y2">テスト用データの数</param>
        /// <param name="Y3">入力層のユニット数</param>
        /// <param name="Y4">隠れ層のユニット数</param>
        /// <param name="Y5">出力層のユニット数</param>
        public NeuralNetworkCore(int Y1, int Y2, int Y3, int Y4, int Y5, double Y6 = 0.8, double Y7 = 0.75) : base(Y1, Y2, Y3, Y4, Y5, alpha: Y6, eta: Y7) { }
        public NeuralNetworkCore() { }

        // ネットワークを初期化する
        public override void Initialize()
        {
            Random Rnd = new Random();

            // 入力層->隠れ層の重みを乱数で初期化
            for (int i = 0; i < count_unit_hidden; i++)
            {
                for (int j = 0; j < count_unit_in; j++)
                {
                    weight_in_to_hidden[i, j] = Math.Sign(Rnd.NextDouble() - 0.5) * Rnd.NextDouble();
                }
                bias_hidden[i] = Math.Sign(Rnd.NextDouble() - 0.5) * Rnd.NextDouble();
            }

            // 隠れ層→出力層の重みを乱数で初期化
            for (int i = 0; i < count_unit_out; i++)
            {
                for (int j = 0; j < count_unit_hidden; j++)
                {
                    weight_hidden_to_out[i, j] = Math.Sign(Rnd.NextDouble() - 0.5) * Rnd.NextDouble();
                }
                bias_out[i] = Math.Sign(Rnd.NextDouble() - 0.5) * Rnd.NextDouble();
            }
        }

        /// <summary>
        /// 入力層->隠れ層の信号伝播
        /// </summary>
        /// <param name="dataIndex">訓練データのインデックス</param>
        public override void ForwardPropagation(int dataIndex)
        {
            double sum = 0.0;

            // 入力層->隠れ層への信号伝播
            for (int i = 0; i < count_unit_hidden; i++)
            {
                sum = 0.0;
                for (int j = 0; j < count_unit_in; j++)
                {
                    // 重みと入力層j番目のユニットの出力値をかけて足し合わせる
                    sum += weight_in_to_hidden[i, j] * output_in[dataIndex, j];
                }
                // バイアスを加えたsumを伝達関数に与えたものが隠れ層i番目のユニットの出力
                output_hidden[i] = sigmoid(sum + bias_hidden[i]);
            }

            // 隠れ層->出力層への信号伝播
            for (int i = 0; i < count_unit_out; i++)
            {
                sum = 0.0;
                for (int j = 0; j < count_unit_hidden; j++)
                {
                    sum += weight_hidden_to_out[i, j] * output_hidden[j];
                }
                output_out[i] = sum + bias_out[i];
            }
            output_out = output_out.SoftMax();
        }

        /// <summary>
        /// 学習したニューラルネットワークで判別を行う。
        /// </summary>
        /// <param name="_in">入力ベクトル</param>
        /// <returns></returns>
        public double[] GetForwardPropagation(double[] _in)
        {
            double sum = 0.0;
            double[] _hidden = new double[count_unit_hidden];
            double[] _out = new double[count_unit_out];
            // 入力層->隠れ層への信号伝播
            for (int i = 0; i < count_unit_hidden; i++)
            {
                sum = 0.0;
                for (int j = 0; j < count_unit_in; j++)
                {
                    // 重みと入力層j番目のユニットの出力値をかけて足し合わせる
                    sum += weight_in_to_hidden[i, j] * _in[j];
                }
                // バイアスを加えたsumを伝達関数に与えたものが隠れ層i番目のユニットの出力
                _hidden[i] = sigmoid(sum + bias_hidden[i]);
            }

            // 隠れ層->出力層への信号伝播
            for (int i = 0; i < count_unit_out; i++)
            {
                sum = 0.0;
                for (int j = 0; j < count_unit_hidden; j++)
                {
                    sum += weight_hidden_to_out[i, j] * _hidden[j];
                }
                _out[i] = sum + bias_out[i];
            }
            _out = _out.SoftMax();
            return _out;
        }

        /// <summary>
        /// 学習したニューラルネットワークで判別を行う。
        /// </summary>
        /// <param name="_in"></param>
        /// <returns>隠れ層、出力層の順</returns>
        public Tuple<double[],double[]> GetForwardPropagation2(double[] _in)
        {
            double sum = 0.0;
            double[] _hidden = new double[count_unit_hidden];
            double[] _out = new double[count_unit_out];
            // 入力層->隠れ層への信号伝播
            for (int i = 0; i < count_unit_hidden; i++)
            {
                sum = 0.0;
                for (int j = 0; j < count_unit_in; j++)
                {
                    // 重みと入力層j番目のユニットの出力値をかけて足し合わせる
                    sum += weight_in_to_hidden[i, j] * _in[j];
                }
                // バイアスを加えたsumを伝達関数に与えたものが隠れ層i番目のユニットの出力
                _hidden[i] = sigmoid(sum + bias_hidden[i]);
            }

            // 隠れ層->出力層への信号伝播
            for (int i = 0; i < count_unit_out; i++)
            {
                sum = 0.0;
                for (int j = 0; j < count_unit_hidden; j++)
                {
                    sum += weight_hidden_to_out[i, j] * _hidden[j];
                }
                _out[i] = sum + bias_out[i];
            }
            _out = _out.SoftMax();
            return new Tuple<double[], double[]>(_hidden, _out);
        }


        /// <summary>
        /// 誤差逆伝播法を用いてネットワークを調整する
        /// </summary>
        /// <param name="dataIndex">訓練データのインデックス</param>
        public override void BackPropagation(int dataIndex)
        {
            double sum = 0.0;
            double[] teacher_signal_out_to_hidden = new double[count_unit_out];
            double[] learn_signal_out_to_hidden = new double[count_unit_hidden];

            // 出力層の教師信号をすべてのユニットについて求める
            for (int i = 0; i < count_unit_out; i++)
            {
                teacher_signal_out_to_hidden[i] = (teacher_signal[dataIndex, i] - output_out[i]) * output_out[i] * (1.0 - output_out[i]);
            }

            // 出力層->隠れ層の重みの変化量を求める
            for (int i = 0; i < count_unit_hidden; i++)
            {
                sum = 0.0;
                for (int j = 0; j < count_unit_out; j++)
                {
                    weight__modify_hidden_to_out[j, i] = eta * teacher_signal_out_to_hidden[j] * output_hidden[i] + alpha * weight__modify_hidden_to_out[j, i];
                    weight_hidden_to_out[j, i] += weight__modify_hidden_to_out[j, i];
                    sum += teacher_signal_out_to_hidden[j] * weight_hidden_to_out[j, i];
                }
                // シグモイド関数の１次微分と掛け合わせる
                learn_signal_out_to_hidden[i] = output_hidden[i] * (1 - output_hidden[i]) * sum;
            }

            // 出力層のバイアスの変化量を求める
            for (int i = 0; i < count_unit_out; i++)
            {
                bias_modify_out[i] = eta * teacher_signal_out_to_hidden[i] + alpha * bias_modify_out[i];
                bias_out[i] += bias_modify_out[i];
            }

            // 隠れ層->入力層の重みの変化量を求める
            for (int i = 0; i < count_unit_in; i++)
            {
                for (int j = 0; j < count_unit_hidden; j++)
                {
                    weight_modify_in_to_hidden[j, i] = eta * learn_signal_out_to_hidden[j] * output_in[dataIndex, i] + alpha * weight_modify_in_to_hidden[j, i];
                    weight_in_to_hidden[j, i] += weight_modify_in_to_hidden[j, i];
                }
            }

            // 隠れ層のバイアスの変化量を求める
            for (int i = 0; i < count_unit_hidden; i++)
            {
                bias_modify_hidden[i] = eta * learn_signal_out_to_hidden[i] + alpha * bias_modify_hidden[i];
                bias_hidden[i] += bias_modify_hidden[i];
            }
        }

  
        public double sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        public double tanh(double x)
        {
            return Math.Tanh(x);
        }

        public void Learn(int loop,Action<int> progress)
        {
            int tmp = 0;
            
            for (int i = 0; i < loop; i++)
            {
                var list = Enumerable.Range(0, count_train).OrderBy(n => Guid.NewGuid()).ToArray();
                for (int j = 0; j < count_train; j++)
                {
                    ForwardPropagation(list[j]);
                    BackPropagation(list[j]);
                }
                if(tmp != i*100/loop)
                {
                    tmp = i * 100 / loop;
                    if (tmp % 2 == 0)
                    {
                        progress(tmp);
                    }
                }
            }
        }

        public double Test(IEnumerable<InputData> list )
        {
            int sum = 0;
            foreach (var item in list)
            {
                var r = GetForwardPropagation(item.Input);
                if(r.MaxIndex() == item.Teacher.MaxIndex())
                {
                    sum++;
                }
            }
            return sum / (double)list.Count();
        }

        public string ToJson()
        {
            var r = new { this.InputLabels,this.OutputLabels, weight_in_to_hidden= this.weight_in_to_hidden.ToDic(), weight_hidden_to_out= this.weight_hidden_to_out.ToDic(),this.bias_hidden,this.bias_out };
            return  JObject.FromObject(r).ToString();
        }

        
        public string[] OutputLabels { get; set; }
        
        public string[] InputLabels { get; set; }


        public static NeuralNetworkCore Create(IEnumerable<InputData> data,int count_unit_hidden)
        {
            var outlabels = data.Select(n => n.TeacherLabel).Distinct().OrderBy(n=> n).ToArray();

            if(outlabels.Length>0)
            {
                foreach (var item in data)
                {
                    List<double> d = new List<double>();

                    foreach (var l in outlabels)
                    {
                        if(item.TeacherLabel == l) { d.Add(1); }
                        else
                        {
                            d.Add(0);
                        }
                    }
                    item.Teacher = d.ToArray();
                }
            }

            //var inputLabels = data.SelectMany(n => n.InputLabels).Distinct().ToArray();
            //if (inputLabels.Length > 0)
            //{
            //    foreach (var item in data)
            //    {
            //        List<double> d = new List<double>();
            //        HashSet<string> h = new HashSet<string>(item.InputLabels);
            //        foreach (var l in inputLabels)
            //        {
            //            if (h.Contains(l)) { d.Add(1); }
            //            else
            //            {
            //                d.Add(0);
            //            }
            //        }
            //        item.Input = d.ToArray();
            //    }
            //}




            var inputL = data.Select(n => n.Input.Length).Min();
            var learnL = data.Where(n => n.IsTest==false).Count();
            var testL = data.Where(n => n.IsTest).Count();
            var outputL = data.Where(n=>n.IsTest == false).Select(n => n.Teacher.Length).Min();



            NeuralNetworkCore core = new NeuralNetworkCore(learnL,testL,inputL,count_unit_hidden,outputL);
            //core.OutputLabels = outlabels;
            //core.InputLabels = inputLabels;

            int count = 0;
            foreach (var item in data.Where(n=>n.IsTest == false))
            {
                for (int i = 0; i < inputL; i++)
                {
                    core.output_in[count, i] = item.Input[i];
                }
                for (int i = 0; i < outputL; i++)
                {
                    core.teacher_signal[count, i] = item.Teacher[i];
                }
                count++;
            }

            foreach (var item in data.Where(n => n.IsTest))
            {
                for (int i = 0; i < inputL; i++)
                {
                    core.output_in[count, i] = item.Input[i];
                }
                count++;
            }
            core.Initialize();



            return core;
        }


    }

    public static class Extend
    {
        /// <summary>
        /// 配列の中の最大値を持つIndexを取得する。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int MaxIndex(this double[] data)
        {
            double max = double.MinValue;
            int index = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if(max< data[i])
                {
                    index = i;
                    max = data[i];
                }
            }
            return index;
        }

        public static Dictionary<string,double> ToDic(this double[,] data)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            for (int i = 0; i <= data.GetUpperBound(0); i++)
            {
                for (int l = 0;  l <= data.GetUpperBound(1);  l++)
                {
                    dic[i + "," + l] = (double)data.GetValue(i, l);
                }
            }
            return dic;
        }

        /// <summary>
        /// ソフトマックス関数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double[] SoftMax(this double[] d)
        {
            var sum = d.Select(n => Math.Exp(n + 1) ).Sum();
            return d.Select(n => (Math.Exp(n + 1) ) / sum).ToArray();
        }

    }

    public class DataSet
    {
        public string[] InputLabels { get; set; }
        public string[] TeacherLabels { get; set; }
        public List<InputData> List { get; set; } = new List<InputData>();
        public void CreateTeacherData()
        {
            var outlabels= List.SelectMany(n => n.TeacherLabel).Distinct().OrderBy(n => n).ToArray();
            if (outlabels.Any())
            {
                foreach (var item in List)
                {
                    List<double> d = new List<double>();
                    HashSet<string> h = new HashSet<string>(item.TeacherLabel);
                    foreach (var l in outlabels)
                    {
                        if (h.Contains(l)) { d.Add(1); }
                        else
                        {
                            d.Add(0);
                        }
                    }
                    item.Teacher = d.ToArray();
                }
                this.TeacherLabels = outlabels;
            }
        }

        public double[] CreateInputData(IEnumerable<string> list)
        {
            List<double> d = new List<double>();
            HashSet<string> h = new HashSet<string>(list);
            foreach (var item in InputLabels)
            {
                if (h.Contains(item)) d.Add(1);
                else d.Add(0);
            }
            return d.ToArray();
        }

    }

    public class InputData
    {
        public double[] Input { get; set; }
        public double[] Teacher { get; set; }
        public string[] TeacherLabel { get; set; }
        public bool IsTest { get; set; } = false;

    }

    class TestCase
    {
        static void Main(string[] args)
        {
            XOR();
        }

        static void XOR()
        {
            NeuralNetworkCore core = new NeuralNetworkCore(4, 4, 2, 2, 1);

            // 訓練用XORデータ入力＋出力（教師）
            core.output_in[0, 0] = 0.0; core.output_in[0, 1] = 0.0; core.teacher_signal[0, 0] = 0.0;
            core.output_in[1, 0] = 1.0; core.output_in[1, 1] = 0.0; core.teacher_signal[1, 0] = 1.0;
            core.output_in[2, 0] = 0.0; core.output_in[2, 1] = 1.0; core.teacher_signal[2, 0] = 1.0;
            core.output_in[3, 0] = 1.0; core.output_in[3, 1] = 1.0; core.teacher_signal[3, 0] = 0.0;

            // テスト用XOR入力
            core.output_in[4, 0] = 0.0; core.output_in[4, 1] = 0.0;
            core.output_in[5, 0] = 1.0; core.output_in[5, 1] = 0.0;
            core.output_in[6, 0] = 0.0; core.output_in[6, 1] = 1.0;
            core.output_in[7, 0] = 1.0; core.output_in[7, 1] = 1.0;


            // ニューラルネットワークを初期化
            core.Initialize();

            // 信号伝播と誤差逆伝播を10000回行う
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < core.count_train; j++)
                {
                    core.ForwardPropagation(j);
                    core.BackPropagation(j);
                }
            }

            // 訓練データを信号伝播にかけて出力する
            for (int j = 0; j < core.count_test; j++)
            {
                core.ForwardPropagation(core.count_train + j);
                for (int k = 0; k < core.count_unit_in; k++)
                {
                    Console.Write(core.output_in[j, k]);
                    if (k != core.count_unit_in - 1) Console.Write(", ");
                }
                Console.WriteLine();
                for (int k = 0; k < core.count_unit_out; k++)
                {
                    Console.WriteLine(core.output_out[k]);
                }
                Console.WriteLine("-----------------------------");
            }

        }
    }
}
