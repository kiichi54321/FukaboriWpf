﻿using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Reactive.Bindings;
using System.Reactive.Linq;
using FukaboriCore.Lib;

namespace FukaboriCore.Model
{
    [DataContract]
    public class Enqueite : GalaSoft.MvvmLight.ObservableObject
    {
        public Enqueite()
        {
            // drawInData.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(drawInData_CollectionChanged);
            // drawOutData.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(drawInData_CollectionChanged);
            Init();
        }

        void drawInData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("Count");
        }
        MyLib.IO.TSVFileBase dataFile;

        [DataMember]
        public MyLib.IO.TSVFileBase DataFile
        {
            get { return dataFile; }
            set { dataFile = value; }
        }

        QuestionManage questionManage = new QuestionManage();
        public async System.Threading.Tasks.Task DataLoad(System.IO.StreamReader stream)
        {
            dataFile = await MyLib.IO.TSVFileBase.ReadTSVFile2(stream);
            RaisePropertyChanged("Count");

            foreach (var item in QuestionList.Where(n => n.AnswerType == AnswerType.数値))
            {
                item.CreateQuestionAnswer(dataFile.Lines);
            }
        }

        System.Collections.ObjectModel.ObservableCollection<ClusteringData> clusteringDataList = new System.Collections.ObjectModel.ObservableCollection<ClusteringData>();
        [DataMember]
        public System.Collections.ObjectModel.ObservableCollection<ClusteringData> ClusteringDataList
        {
            get { return clusteringDataList; }
            set { clusteringDataList = value; }
        }

        DataMarge _dataMarge;

        public DataMarge DataMarge
        {
            get
            {
                if (_dataMarge == null)
                {
                    _dataMarge = new DataMarge();
                    _dataMarge.Enqueite = this;
                    _dataMarge.Init();
                }
                return _dataMarge;
            }
        }

        public IEnumerable<MyLib.IO.TSVLine> AllAnswerLine
        {
            get
            {
                return dataFile.Lines;
            }
        }

        public IEnumerable<MyLib.IO.TSVLine> AnswerLines
        {
            get
            {
                if (drawInData.Count == 0 && drawOutData.Count == 0)
                {
                    if (dataFile != null)
                    {
                        foreach (var item in dataFile.Lines)
                        {
                            yield return item;
                        }
                    }
                }
                else
                {
                    foreach (var item in dataFile.Lines)
                    {
                        bool flag = true;

                        foreach (var item2 in drawOutData.GroupBy(n => n.Question))
                        {
                            bool f = item2.Any(n => n.Check(item));
                            if (f == true)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag == false) continue;

                        foreach (var item2 in drawInData.GroupBy(n => n.Question))
                        {
                            bool f = item2.Any(n => n.Check(item));
                            if (f == false)
                            {
                                flag = false;
                                break;
                            }
                        }


                        if (flag) yield return item;
                    }

                }
            }
        }

        public int Count
        {
            get
            {
                return AnswerLines.Count();
            }
        }


        public void QuestionLoad(System.IO.StreamReader stream)
        {
            questionManage.Clear();
            MyLib.IO.TSVFile file = new MyLib.IO.TSVFile(stream);
            foreach (var item in file.Lines.Where(n => n.Line.Length > 0))
            {
                Question q = new Question();
                q.Key = item.GetValue("Key");
                q.Text = item.GetValue("Text");
                q.Answers = item.GetValue("Answers", string.Empty).Split(',').Where(n => n.Length > 0).ToList();
                q.SetType(item.GetValue("Type"));
                q.Children = item.GetValue("Children", string.Empty).Split(',').Where(n => n.Length > 0).ToList();
                q.Init();
                questionManage.Add(q.Key, q);
            }
            file.Dispose();
        }

        public CrossData CreateData(IEnumerable<MyLib.IO.TSVLine> lines, string targetKey, string groupKey)
        {
            CrossData cd = new CrossData();
            var targetQuestion = questionManage.GetQuestion(targetKey);
            var groupQuestion = questionManage.GetQuestion(groupKey);
            //  cd.AnswerText = targetQuestion.Answers;
            cd.QuestionText = targetQuestion.ViewText;
            cd.横Question = targetQuestion;
            cd.縦Question = groupQuestion;

            cd.Create(targetQuestion, groupQuestion, this.AnswerLines);


            return cd;
        }

        public static void MargeFile(System.IO.Stream stream, System.IO.Stream stream2, System.IO.Stream saveStream)
        {
            var baseEnqueite = Enqueite.Load(stream);
            var addEnqueite = Enqueite.Load(stream2);
            baseEnqueite.Init();
            addEnqueite.Init();

            foreach (var item in addEnqueite.QuestionManage.Dic.Where(n => baseEnqueite.QuestionManage.Dic.ContainsKey(n.Key) == false))
            {
                baseEnqueite.QuestionManage.Add(item.Key, item.Value);
                for (int i = 0; i < Math.Min(baseEnqueite.DataFile.Lines.Count, addEnqueite.DataFile.Lines.Count); i++)
                {
                    var val = addEnqueite.DataFile.Lines[i].GetValue(item.Key, null);
                    if (val != null)
                    {
                        baseEnqueite.DataFile.Lines[i].AddExtendColumn(item.Value.Key, val);
                    }
                }
            }
            foreach (var item in addEnqueite.ClusteringDataList)
            {
                baseEnqueite.ClusteringDataList.Add(item);
            }
            baseEnqueite.Save(saveStream);
        }



        public System.Collections.ObjectModel.ObservableCollection<Question> QuestionList
        {
            get
            {
                return questionManage.List;
            }
        }

        public IEnumerable<Question> FreeTextQuestionList
        {
            get
            {
                return questionManage.List.Where(n => n.AnswerType == AnswerType.文字列);
            }
        }

        internal CrossData CreateData(string targetKey, string groupKey)
        {
            return CreateData(this.AnswerLines, targetKey, groupKey);
        }

        System.Collections.ObjectModel.ObservableCollection<AnswerGroup> drawInData = new System.Collections.ObjectModel.ObservableCollection<AnswerGroup>();
        System.Collections.ObjectModel.ObservableCollection<AnswerGroup> drawOutData = new System.Collections.ObjectModel.ObservableCollection<AnswerGroup>();

        public System.Collections.ObjectModel.ObservableCollection<AnswerGroup> DrawOutData
        {
            get { return drawOutData; }
            set { drawOutData = value; }
        }

        public System.Collections.ObjectModel.ObservableCollection<AnswerGroup> DrawInData
        {
            get { return drawInData; }
            set { drawInData = value; }
        }

        public void Add絞込(Model.AnswerGroup answer)
        {
            if (drawInData.Any(n => n.ViewText == answer.ViewText) == false)
            {
                drawInData.Add(answer);
            }
        }

        public void Add削除(Model.AnswerGroup answer)
        {
            if (drawOutData.Any(n => n.ViewText == answer.ViewText) == false)
            {
                drawOutData.Add(answer);
                answer.IsActive = false;
            }
        }

        internal void Remove絞込(AnswerGroup answer)
        {
            drawInData.Remove(answer);
        }
        internal void Remove削除(AnswerGroup answer)
        {
            drawOutData.Remove(answer);
            answer.IsActive = true;
        }

        internal void 条件Clear()
        {
            drawInData.Clear();
        }



        [DataMember]
        public QuestionManage QuestionManage
        {
            get { return questionManage; }
            set { questionManage = value; }
        }



        public void Save(System.IO.Stream stream)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Enqueite));
            serializer.WriteObject(stream, this);
            //           stream.Close();
        }



        public static Enqueite Load(System.IO.Stream stream)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Enqueite));

            Enqueite ct = (Enqueite)serializer.ReadObject(stream);
            ct.Init();
            //         stream.Close();
            return ct;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public IEnumerable<string> ToDataJson()
        {

            foreach (var item in this.AllAnswerLine)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var q in this.QuestionManage.Dic.Select(n => n.Value))
                {
                    dic.Add(q.ViewText, q.GetValue(item)?.TextValue);
                }
                yield return JsonConvert.SerializeObject(dic, Formatting.None);
            }
        }

        public void RemoveClusteringData(Model.ClusteringData cd)
        {
            dataFile.RemoveExtendColumn(cd.Key);
            ClusteringDataList.Remove(cd);
            QuestionList.Remove(QuestionManage.Dic[cd.Key]);
        }



        private void Init()
        {
            this.questionManage.Init(this);
            if (DataFile != null) DataFile.Init();
            EnqueiteData.Current.Value = this;
            drawInData = new System.Collections.ObjectModel.ObservableCollection<AnswerGroup>();
            drawInData.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(drawInData_CollectionChanged);
            drawOutData = new System.Collections.ObjectModel.ObservableCollection<AnswerGroup>();
            drawOutData.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(drawInData_CollectionChanged);
            if (KeyClastering == null) KeyClastering = new KeyClustering();
            KeyClastering.Init(this);
            if (DataCoordinator == null) DataCoordinator = new DataCoordinator() { Enqueite = this };
            if (ClusteringDataList == null)
            {
                clusteringDataList = new System.Collections.ObjectModel.ObservableCollection<ClusteringData>();
            }
            foreach (var item in ClusteringDataList)
            {
                item.Init(dataFile);
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public KeyClustering KeyClastering { get; set; }

        public DataCoordinator DataCoordinator { get; set; }


        internal void CreateQuestion(System.IO.StreamReader stream)
        {
            Dictionary<int, List<string>> dic = new Dictionary<int, List<string>>();
            var headers = stream.ReadLine().Split('\t');
            while (true)
            {
                var line = stream.ReadLine().Split('\t');
                for (int i = 0; i < line.Length; i++)
                {
                    if (dic.ContainsKey(i))
                    {
                        dic[i].Add(line[i]);
                    }
                    else
                    {
                        dic.Add(i, new List<string>() { line[i] });
                    }
                }
                if (stream.Peek() < 0) break;
            }
            for (int i = 0; i < Math.Min(headers.Length, dic.Keys.Count); i++)
            {
                this.QuestionManage.Add(headers[i], Question.Create(headers[i], dic[i].Distinct()));
            }
            stream.BaseStream.Position = 0;
        }


    }

    public class EnqueiteDataContainer : GalaSoft.MvvmLight.ObservableObject
    {
        Enqueite enquite = new Enqueite();

        public Enqueite Enqueite
        {
            get { return enquite; }
            set
            {
                Set(ref enquite, value);
            }
        }
    }

    public static class EnqueiteData
    {
        public static ObservableObject<Enqueite> Current { get; private set; }

        public static void Init()
        {
            Current = new ObservableObject<Enqueite>();

        }

        public static IEnumerable<Question> QuestionSearch(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return EnqueiteData.Current.Value?.QuestionList;
            }
            else
            {
                return EnqueiteData.Current.Value?.QuestionList.Where(n => n.ViewText.Contains(text));
            }
        }
    }

}
namespace FukaboriCore.Lib
{
    public class ObservableObject<T> : IObservable<T>
    {
        List<WeakReference<IObserver<T>>> observerList = new List<WeakReference<IObserver<T>>>();
        public IDisposable Subscribe(IObserver<T> observer)
        {
            observerList.Add(new WeakReference<IObserver<T>>(observer));
            return new ObservableObserverDisposer<T>(this, observer);
        }

        public void ForceNotify()
        {
            if (val != null)
            {
                IObserver<T> observer = null;
                foreach (var item in observerList.ToArray())
                {
                    if (item.TryGetTarget(out observer))
                    {
                        observer.OnNext(val);
                    }
                    else
                    {
                        observerList.Remove(item);
                    }
                }
            }
        }

        public void Remove(IObserver<T> target)
        {
            IObserver<T> observer = null;
            foreach (var item in observerList.ToArray())
            {
                if (item.TryGetTarget(out observer))
                {
                    if (target == observer)
                    {
                        observerList.Remove(item);
                        break;
                    }
                }
                else
                {
                    observerList.Remove(item);
                }
            }
        }

        T val = default(T);
        public T Value
        {
            get { return val; }
            set
            {
                if (value.Equals(val) == false)
                {
                    val = value;
                    ForceNotify();
                }
            }
        }
    }

    public class ObservableObserverDisposer<T> : IDisposable
    {
        WeakReference<IObserver<T>> observer;
        ObservableObject<T> observable;
        public ObservableObserverDisposer(ObservableObject<T> observable, IObserver<T> observer)
        {
            this.observable = observable;
            this.observer = new WeakReference<IObserver<T>>(observer);
        }
        public void Dispose()
        {
            IObserver<T> val;
            if (observer.TryGetTarget(out val))
            {
                //監視人をはずす
                observable.Remove(val);
            }
        }
    }

}

