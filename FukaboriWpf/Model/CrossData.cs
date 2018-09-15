using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FukaboriWpf.Model
{
    public class CrossDataStock
    {
        public List<CrossData> Data { get; set; }

        public void Save(System.IO.Stream stream)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(CrossDataStock));
            serializer.WriteObject(stream, this);
            stream.Close();
        }
        public static CrossDataStock Load(System.IO.Stream stream)
        {
            
            DataContractSerializer serializer = new DataContractSerializer(typeof(CrossDataStock));

            CrossDataStock ct = (CrossDataStock)serializer.ReadObject(stream);
            stream.Close();
            return ct;
        }
    }

    [DataContract]
    public class CrossData:MyLib.Interface.ITsv
    {
        [DataMember]
        public string QuestionText { get; set; }
        public string QuestionText2 { get { return this.縦Question.ViewText; } }
        public List<string> AnswerText
        {
            get
            {
                return 横Question.AnswerGroupIsActive.OrderBy(n => n.Order).Where(n => n.Answeres.Count > 0).Select(n => n.TextValue).ToList();
                //if (横Question.Reverse)
                //{
                //    return this.横Question.Answers.ToArray().Reverse().ToList();
                //}
                //else
                //{
                //    return this.横Question.Answers;
                //}
            }
        }
        public List<DataRow> Rows
        {
            get
            {
                return dic.OrderBy(n => n.Key.Order).Select(n => n.Value).ToList();

            }
        }

        public string ToTsv()
        {
            MyLib.IO.TsvBuilder tsv = new MyLib.IO.TsvBuilder();
            foreach (var row in Rows)
            {
                row.ToTsv(tsv);
            }
            SumRow.ToTsv(tsv);

            return tsv.ToString();
        }

        Dictionary<AnswerGroup, DataRow> dic = new Dictionary<AnswerGroup, DataRow>();
        [DataMember]
        public Dictionary<AnswerGroup, DataRow> RowDic
        {
            get { return dic; }
            set { dic = value; }
        }
        [DataMember]
        public DataRow SumRow { get; set; }

        public void Create(Question 横Question, Question 縦Question, IEnumerable<MyLib.IO.TSVLine> lines)
        {
            this.横Question = 横Question;
            this.縦Question = 縦Question;
            int c = 1;
            foreach (var item in 縦Question.AnswerGroupIsActive.Where(n => n.Answeres.Count > 0))
            {
                DataRow row = new DataRow();
                row.Header = item.TextValue;
                row.Create(横Question.AnswerGroupIsActive.Where(n => n.Answeres.Count > 0));
                row.Parent = this;
                dic.Add(item, row);
                c++;
            }

            foreach (var item in lines)
            {
                var targetValue = 横Question.GetValue(item);
                var groupValue = 縦Question.GetValue(item);
                if (targetValue != null && groupValue != null)
                {
                    AddCount(groupValue, targetValue);
                }
            }
            this.計算();
        }

        [DataMember]
        public Question 横Question { get; set; }
        [DataMember]
        public Question 縦Question { get; set; }
        [DataMember]
        public double 相関係数 { get; set; }
        [DataMember]
        public double カイ２乗値 { get; set; }
        [DataMember]
        public double 自由度 { get; set; }


        public void AddCount(AnswerGroup group, AnswerGroup target)
        {
            if (dic.ContainsKey(group))
            {
                dic[group].Add(target);
            }
            //double g, t;
            //if (double.TryParse(group, out g) && double.TryParse(target, out t))
            {
                correlation.Add(group.Value, target.Value);
            }

        }

        MyLib.Statistics.Correlation correlation = new MyLib.Statistics.Correlation();


        public void 計算()
        {
            foreach (var item in Rows)
            {
                item.計算();
            }
            List<int> sumList = new List<int>();
            for (int i = 0; i < AnswerText.Count; i++)
            {
                int sum = 0;
                for (int l = 0; l < Rows.Count; l++)
                {
                    sum += Rows[l].Cells[i].Count;
                }

                for (int l = 0; l < Rows.Count; l++)
                {
                    Rows[l].Cells[i].縦Rate = Rows[l].Cells[i].Count / (double)sum;
                }
                sumList.Add(sum);
            }

            //for (int l = 0; l < Rows.Count; l++)
            //{
            //    for (int i = 0; i < AnswerText.Count; i++)
            //    {
            //        Rows[l].Cells[i].縦Rate = Rows[l].Cells[i].Count / (double)sumList[i];
            //    }
            //}

            List<List<double>> matrix = new List<List<double>>();
            foreach (var item in Rows)
            {
                var r = new List<double>();
                foreach (var cell in item.Cells)
                {
                    r.Add(cell.Count);
                }
                matrix.Add(r);
            }
            var kai = MyLib.Statistics.ChiSquareTest.Calculation(matrix);
            this.カイ２乗値 = kai.TestValue;
            this.自由度 = kai.自由度;

            SumRow = new DataRow() { Header = "合計", Parent = this };

            Dictionary<AnswerGroup, int> dic = new Dictionary<AnswerGroup, int>();
            int c = 0;
            foreach (var item in 横Question.AnswerGroupIsActive.Where(n=>n.Answeres.Count>0))
            {
                dic.Add(item, sumList[c]);
                c++;
            }
            //if (this.横Question.Reverse)
            //{
            //    sumList = sumList.ToArray().Reverse().ToList();
            //}
            SumRow.計算2(dic);
            foreach (var item in SumRow.Cells)
            {
                if (item.Count == 0) item.Visibility2 = Visibility.Collapsed;
            }

            this.相関係数 = correlation.Result();
        }

    }

    [DataContract]
    public class Cell : System.ComponentModel.INotifyPropertyChanged
    {
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public double 横Rate { get; set; }
        [DataMember]
        public double 縦Rate { get; set; }
        [DataMember]
        public double 特化係数 { get; set; }
        [DataMember]
        public string ValueText { get; set; }
       
        Color bgColor = Colors.White;
         [DataMember]
        public Color BGColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
                onPropertyChanged("BGColor");
            }
        }

         public Visibility Visibility
         {
             get
             {
                 if (double.IsNaN(横Rate) || double.IsNaN(縦Rate))
                 {
                     return Visibility.Collapsed;
                 }
                 else
                 {
                     return Visibility.Visible;
                 }
             }
         }

         Visibility visibility2 = Visibility.Visible;

         public Visibility Visibility2
         {
             get { return visibility2; }
             set { visibility2 = value; }
         }
        


        #region INotifyPropertyChanged メンバー

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void onPropertyChanged(string txt)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(txt));
            }
        }
        #endregion
    }

    [DataContract]
    public class DataRow : System.ComponentModel.INotifyPropertyChanged
    {
        [DataMember]
        public string Header { get; set; }
        public double Avg { get; set; }
        public double Std { get; set; }
        public int Sum { get; set; }
        public CrossData Parent { get; set; }

        Dictionary<AnswerGroup, int> dic = new Dictionary<AnswerGroup, int>();
        [DataMember]
        public List<Cell> Cells { get; set; }
        [DataMember]
        public List<Cell> SumCells { get; set; }

        #region INotifyPropertyChanged メンバー

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void onPropertyChanged(string txt)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(txt));
            }
        }
        #endregion


        public void Add(AnswerGroup answer)
        {
            if (dic.ContainsKey(answer))
            {
                dic[answer]++;
            }
        }

        public void 計算2(Dictionary<AnswerGroup, int> dic)
        {
            int sum = 0;
            int sum2 = 0;
            int c = 1;
            foreach (var item in dic)
            {
                sum += item.Value;
                sum2 += (int)(item.Value * item.Key.Value);
                c++;
            }
            Avg = sum2 / (double)sum;
            Sum = sum;

            double std = 0;
            c = 1;
            foreach (var item in dic)
            {
                std += item.Value * Math.Pow(item.Key.Value - Avg, 2);
                c++;
            }
            Std = Math.Sqrt(std / sum);

            Cells = new List<Cell>();

            //if (this.Parent.横Question.Reverse)
            //{
            //    list = list.Reverse();
            //}

            foreach (var item in dic)
            {
                Cells.Add(new Cell() { Count = item.Value, 横Rate = item.Value * 100 / (double)sum });
            }
            SumCells = new List<Cell>();
            if (sum > 0)
            {
                SumCells.Add(new Cell() { ValueText = Sum.ToString() });
                SumCells.Add(new Cell() { ValueText = Avg.ToString("F2") });
                SumCells.Add(new Cell() { ValueText = Std.ToString("F2") });
            }
            else
            {
                SumCells.Add(new Cell() { });
                SumCells.Add(new Cell() { });
                SumCells.Add(new Cell() { });

            }

        }


        public void Create(IEnumerable<AnswerGroup> list)
        {
            dic.Clear();
            int c = 1;
            foreach (var item in list)
            {
                if (dic.ContainsKey(item) == false)
                {
                    dic.Add(item, 0);
                }
                c++;
            }
        }

        public void 計算()
        {

            this.計算2(dic);

        }

        public void ToTsv(MyLib.IO.TsvBuilder tsvBuilder)
        {
            var tsv = tsvBuilder;
            var row = this;
            tsv.Add("Header", row.Header);

            var cell = row.Cells.GetEnumerator();
            foreach (var item in this.Parent.AnswerText)
            {
                cell.MoveNext();
                tsv.Add(item, cell.Current.Count);
                tsv.Add(item + "_Rate", cell.Current.横Rate);
               
            }
            tsv.Add("Sum", row.Sum);
            tsv.Add("Avg", row.Avg);
            tsv.Add("Std", row.Std);
            tsv.NextLine();
        }
    }

}
