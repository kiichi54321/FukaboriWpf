using System;
using System.Net;
using System.Windows;

using System.Collections.Generic;
using System.Linq;
using RawlerLib.MyExtend;
using Reactive.Bindings;
     

namespace FukaboriCore.Model
{

    public class KeyWordSummary : GalaSoft.MvvmLight.ViewModelBase,MyLib.Interface.ITsv
    {
        public string Appid { get; set; }
        const string KeyWordsFotter = "_KeyWords";

        System.Collections.ObjectModel.ObservableCollection<WordCountPanel> wordCountPanels = new System.Collections.ObjectModel.ObservableCollection<WordCountPanel>();

        public System.Collections.ObjectModel.ObservableCollection<WordCountPanel> WordCountPanels
        {
            get { return wordCountPanels; }
            set { wordCountPanels = value; }
        }

        public bool CheckKeitaiso(Question q, MyLib.IO.TSVFileBase file)
        {
            if (file.AllHeader.Contains(q.Key + KeyWordsFotter))
            {
                return true;
            }
            return false;
        }

      //  bool isBusy = false;

        Rawler.RawlerLib.TinySegmenter tinySegmenter = new Rawler.RawlerLib.TinySegmenter();
        public ReactiveProperty<int> ReportProgress { get; set; } = new ReactiveProperty<int>(0);

        public async void CreateTinySegmenter(Question q, MyLib.IO.TSVFileBase file, Action<int> progress, Action endAction)
        {
       //     if (CheckKeitaiso(q, file) == true) return;

            var lineCount = 0;
            var allLine = file.Lines.Count;
            ReportProgress.Value = 0;
            await System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                List<string> list = new List<string>();
                foreach (var item in file.Lines)
                {
                    var g = q.GetValue(item);
                    if (g == null) continue;
                    var text = g.TextValue.Trim();
                    list.Add(text);
                }
                tinySegmenter.ClearWordDic();
                tinySegmenter.AddRangeWordDic(DicText.ReadLines().Where(n => n.Length > 0));
                if (doLearn)
                {
                    //               tinySegmenter.LearningParallel(list, segmenter学習時の最低支持率 / 100, 100, 4, n => bw.ReportProgress(n));
        //            tinySegmenter.LearningParallel(list, segmenter学習時の最低支持率 / 100, Segmenter学習時のNgram);
                }

                foreach (var item in file.Lines)
                {
                    var g = q.GetValue(item);
                    if (g == null) continue;
                    var text = g.TextValue.Trim();
                    if (text.Length > 0)
                    {
                        AddExtendColumn(tinySegmenter.SegmentExted(text), item, q.Key);
                        lineCount++;
                        ReportProgress.Value = lineCount * 100 / allLine;

                    }
                    else
                    {
                        lineCount++;
                        item.AddExtendColumn(q.Key + KeyWordsFotter, string.Empty);
                    }
                }
                LearnDicText = tinySegmenter.LearnWordList.JoinText("\n");

            });

        }

        bool doLearn = true;

        public void SetDicText()
        {
            
        }

        public bool DoLearn
        {
            get { return doLearn; }
            set { doLearn = value; RaisePropertyChanged("DoLearn"); }
        }

        string dicText = string.Empty;

        public string DicText
        {
            get { return dicText; }
            set { dicText = value; RaisePropertyChanged("DicText"); }
        }

        string learnDicText = string.Empty;
        public string LearnDicText
        {
            get { return learnDicText; }
            set { learnDicText = value; RaisePropertyChanged("LearnDicText"); }
        }

        void AddExtendColumn(IEnumerable<string> result, MyLib.IO.TSVLine line, string key)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var word in result)
            {
                sb.Append(word).Append(",");
            }
            line.AddExtendColumn(key + KeyWordsFotter, sb.ToString());
        }

        Question freeAnswerQuestion;
        public void CreateData(Question freeAnswerQuestion, Question targetQusetion, IEnumerable<MyLib.IO.TSVLine> lines)
        {
            if (freeAnswerQuestion == null || targetQusetion == null || lines == null)
            {
                return;
            }
            this.freeAnswerQuestion = freeAnswerQuestion;
            WordCountBase wordCountBase = new WordCountBase() { Count = 0 };
            Dictionary<AnswerGroup, WordCountPanel> dic = new Dictionary<AnswerGroup, WordCountPanel>();
            foreach (var item in targetQusetion.AnswerGroupIsActive.Where(n => n.Answeres.Count > 0))
            {
                dic.Add(item, new WordCountPanel()
                {
                    QuestionAnswer = item,
                    Count = 0,
                    WordCountBase = wordCountBase,
                    MaxTake = MaxTake
                });
            }
            foreach (var item in lines)
            {
                var target = targetQusetion.GetValue(item);
                if (target == null) continue;
                if (target.IsActive)
                {
                    if (dic.ContainsKey(target))
                    {
                        dic[target].Count++;
                        wordCountBase.Count++;
                    }
                    else
                    {
                        dic.Add(target, new WordCountPanel()
                        {
                            QuestionAnswer = target,
                            Count = 0,
                            WordCountBase = wordCountBase,
                            MaxTake = MaxTake
                        });
                        dic[target].Count++;
                        wordCountBase.Count++;
                    }
                    foreach (var data in item.GetValue(freeAnswerQuestion.Key + KeyWordsFotter,string.Empty).Split(',').Distinct())
                    {
                        if (data.Length > 0)
                        {
                            var d2 = data;
                            dic[target].Add(d2, item);
                            wordCountBase.Add(d2);
                        }
                    }
                }
            }
            wordCountPanels.Clear();

            foreach (var item in dic.Values)
            {
                item.Create();
                wordCountPanels.Add(item);
            }
        }

        int progress = 0;
        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    RaisePropertyChanged("Progress");
                }
            }
        }

        int maxTake = 50;

        public int MaxTake
        {
            get { return maxTake; }
            set
            {
                maxTake = value;
                foreach (var item in wordCountPanels)
                {
                    item.ChangeMaxTake(value);
                }
            }
        }

        double segmenter学習時の最低支持率 = 1;

        public double Segmenter学習時の最低支持率
        {
            get { return segmenter学習時の最低支持率; }
            set { segmenter学習時の最低支持率 = value; RaisePropertyChanged("Segmenter学習時の最低支持率"); }
        }

        int segmenter学習時のNgram = 5;

        public int Segmenter学習時のNgram
        {
            get { return segmenter学習時のNgram; }
            set { segmenter学習時のNgram = value; RaisePropertyChanged("Segmenter学習時のNgram"); }
        }

        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("[ぁ-ん。、]");


        public WordViewContent ViewText(WordCount wordCount)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            WordCountBase wCount = new WordCountBase();
            foreach (var item in wordCount.Lines)
            {
                var g = freeAnswerQuestion.GetValue(item);
                if (g == null) continue;
                sb.AppendLine(g.TextValue);

                foreach (var data in item.GetValue(freeAnswerQuestion.Key + KeyWordsFotter).Split(','))
                {
                    if (data.Length > 0)
                    {
                        var d2 = data.Split(':').First();
                        if (d2.Length == 1)
                        {
                            if (r.Match(data).Success) continue;
                            if (data.Trim().Length == 0) continue;
                        }                    
                        wCount.Add(d2);
                    }
                }
            }

            return new WordViewContent() { WordRanking = wCount.WordCountList, WordViewText = sb.ToString() };
        }

        //#region INotifyPropertyChanged メンバー

        //public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string text)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(text));
        //    }
        //}
        //#endregion

        #region ITsv メンバー

        public string ToTsv()
        {
            MyLib.IO.TsvBuilder tsv = new MyLib.IO.TsvBuilder();
            for (int i = 0; i < this.WordCountPanels.Max(n=>n.WordCountList.Count()); i++)
            {
                foreach (var item2 in this.WordCountPanels)
                {
                    var item = item2.WordCountList.ElementAtOrDefault(i);
                    if (item != null)
                    {
                        tsv.Add(item2.QuestionAnswer.ViewText2+"_Word", item.Word);
                        tsv.Add(item2.QuestionAnswer.ViewText2 + "_Count", item.Count);
                        tsv.Add(item2.QuestionAnswer.ViewText2 + "_Tokka", item.Tokka);
                    }
                }
                tsv.NextLine();
            }
            return tsv.ToString();
        }

        #endregion
    }

    public class WordViewContent
    {
        public IEnumerable<WordCount> WordRanking { get; set; }
        public string WordViewText { get; set; }
    }


    public class WordCount
    {
        public string Word { get; set; }
        public List<MyLib.IO.TSVLine> Lines { get; set; }
        public int Count { get { return Lines.Count; } }
        public WordCountBase WordCountBase { get; set; }
        public WordCountPanel WordCountPanel { get; set; }
        public double Tokka
        {
            get
            {
                return (Count / (double)WordCountPanel.Count) / WordCountBase.GetRate(Word);
            }
        }
    }

    public class WordCountBase
    {
        Dictionary<string, WordCount> dic = new Dictionary<string, WordCount>();
        public int Count { get; set; }


        public double GetRate(string text)
        {
            if (dic.ContainsKey(text))
            {
                return dic[text].Count / (double)Count;
            }
            return 0;
        }

        public void Add(string word)
        {
            if (dic.ContainsKey(word))
            {
                dic[word].Lines.Add(new MyLib.IO.TSVLine());
            }
            else
            {
                dic.Add(word, new WordCount() { Word = word, Lines = new List<MyLib.IO.TSVLine>() { new MyLib.IO.TSVLine() } });
            }
        }

        public IEnumerable<WordCount> WordCountList { get { return dic.Values.OrderByDescending(n => n.Count); } }
    }

    public class WordCountPanel:MyLib.Interface.ITsv
    {
        System.Collections.ObjectModel.ObservableCollection<WordCount> wordCountList = new System.Collections.ObjectModel.ObservableCollection<WordCount>();

        public System.Collections.ObjectModel.ObservableCollection<WordCount> WordCountList
        {
            get { return wordCountList; }
            set { wordCountList = value; }
        }

        public QuestionAnswer QuestionAnswer { get; set; }
        public WordCountBase WordCountBase { get; set; }

        Dictionary<string, WordCount> dic = new Dictionary<string, WordCount>();
        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("[ぁ-ん。、]");

        public void Add(string word, MyLib.IO.TSVLine line)
        {
            if(word.Length==1)
            {
                if (r.Match(word).Success) return;
                if (word.Trim().Length == 0) return;
            }
            if (dic.ContainsKey(word))
            {
                dic[word].Lines.Add(line);
            }
            else
            {
                dic.Add(word, new WordCount() { Word = word, Lines = new List<MyLib.IO.TSVLine>() { line }, WordCountBase = WordCountBase, WordCountPanel = this });
            }
        }

        public string ViewText { get { return QuestionAnswer.ViewText2; } }

        public int Count { get; set; }

        int minCount = 0;
        public void Create()
        {
            wordCountList.Clear();
            minCount = 0;
            if (dic.Any() == false) return;
            var minWord = dic.Values.OrderByDescending(n => n.Count).Take(MaxTake).Last();
            if (minWord != null)
            {
                minCount = minWord.Count;
            }
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("[ぁ-ん。、]");
            foreach (var item in dic.Values.OrderByDescending(n => n.Count).Where(n => n.Count >= minCount))
            {

                {
                    wordCountList.Add(item);
                }
            }
        }

        public int MaxTake { get; set; }

        public void ChangeMaxTake(int maxTake)
        {
            if (maxTake > 0)
            {
                int min = 0;
                var minWord = dic.Values.OrderByDescending(n => n.Count).Take(maxTake).Last();
                if (minWord != null)
                {
                    min = minWord.Count;
                }
                if (min != minCount)
                {
                    this.MaxTake = maxTake;
                    Create();

                }
            }
        }

        public void SortCount()
        {
            var list = wordCountList.ToList();
            wordCountList.Clear();
            foreach (var item in list.OrderByDescending(n => n.Count))
            {
                wordCountList.Add(item);
            }
        }

        public void SortTokka()
        {
            var list = wordCountList.ToList();
            wordCountList.Clear();
            foreach (var item in list.OrderByDescending(n => n.Tokka))
            {
                wordCountList.Add(item);
            }
        }



        #region ITsv メンバー

        public string ToTsv()
        {
            MyLib.IO.TsvBuilder tsv = new MyLib.IO.TsvBuilder();
            foreach (var item in this.dic.Values)
            {
                tsv.Add("Word", item.Word);
                tsv.Add("Count", item.Count);
                tsv.Add("Tokka", item.Tokka);
                tsv.NextLine();
            }
            return this.QuestionAnswer.Question.ViewText+" "+ this.QuestionAnswer.ViewText2+ "\n"+ tsv.ToString();
        }

        #endregion
    }
}
