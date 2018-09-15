using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RawlerLib.MyExtend;
using System.Linq;


namespace MyLib.IO
{
    public interface IFile
    {
        int GetIndex(string column);
        IEnumerable<string> ErrMessageList { get; }
        void AddErrMessage(string message);
        void AddExtendColumn(string key);
    }



    
    public class TSVFileBase : IFile
    {

        private Dictionary<string, int> headerDic = new Dictionary<string, int>();
        
        public Dictionary<string, int> HeaderDic
        {
            get { return headerDic; }
            set { headerDic = value; }
        }
        private List<string> extendHeaderList = new List<string>();
        
        public List<string> ExtendHeaderList
        {
            get { return extendHeaderList; }
            set { extendHeaderList = value; }
        }

        public void RemoveExtendColumn(string key)
        {
            extendHeaderList.Remove(key);
            foreach (var item in Lines)
            {
                item.RemoveExtendColumn(key);
            }
        }

        public IEnumerable<string> Header
        {
            get { return headerDic.Keys; }
        }

        public IEnumerable<string> AllHeader
        {
            get
            {
                List<string> list = new List<string>();
                list.AddRange(headerDic.Keys);
                list.AddRange(extendHeaderList);
                return list;
            }
        }

        public void Init()
        {
            foreach (var item in lines)
            {
                item.SetCsvRead(this);
            }
            errMessage = new List<string>();
        }

        /// <summary>
        /// 列名から、列番号の取得。拡張列の場合は,Intの最大値。
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public int GetIndex(string column)
        {
            if (extendHeaderList.Contains(column))
            {
                return int.MaxValue;
            }
            else if (headerDic.ContainsKey(column))
            {
                return headerDic[column];
            }
            else
            {
                return -1;
            }
        }

        public bool CheckHeader(IEnumerable<string> list)
        {
            bool flag = true;
            foreach (var item in list)
            {
                if (headerDic.ContainsKey(item) == false)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public bool CheckHeader(string header)
        {
            return headerDic.ContainsKey(header);
        }
        

        List<TSVLine> lines = new List<TSVLine>();
        
        public List<TSVLine> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
            }
        }

        Dictionary<string, Dictionary<string, int[]>> indexDic = new Dictionary<string, Dictionary<string, int[]>>();

        void AddIndex(string header)
        {
            if (indexDic != null) indexDic = new Dictionary<string, Dictionary<string, int[]>>();

            if (indexDic.ContainsKey(header) == false)
            {
                var dic = Lines.Select((n, i) => new { Val = n.GetValue(header), index = i }).GroupBy(n => n.Val).ToDictionary(n => n.Key, n => n.Select(m => m.index).ToArray());
                indexDic.Add(header, dic);
            }
        }


        public int[] GetIndex(string header,params string[] values)
        {
            if(indexDic == null || indexDic.ContainsKey(header)==false)
            {
                AddIndex(header);
            }
            return indexDic[header].Where(n => values.Contains(n.Key)).Select(n => n.Value).SelectMany(n=> n).ToArray();
        }

        public IEnumerable<TSVLine> GetLines(IEnumerable<int> indexs)
        {
            foreach (var item in indexs)
            {
                yield return Lines[item];
            }
        }

        #region IFile メンバー


        List<string> errMessage = new List<string>();
        public IEnumerable<string> ErrMessageList
        {
            get
            {
                return errMessage;
            }
        }


        public void AddErrMessage(string message)
        {
            errMessage.Add(message);
        }

        public void AddExtendColumn(string key)
        {
            if (extendHeaderList.Contains(key) == false)
            {
                extendHeaderList.Add(key);
            }
        }



        #endregion

        public static TSVFileBase ReadTSVFile(System.IO.StreamReader stream)
        {
            TSVFileBase file = new TSVFileBase();
            StreamReader sr = stream;
            string first = sr.ReadLine();
            int i = 0;
            file.headerDic.Clear();
            foreach (var item in first.Split('\t'))
            {
                file.headerDic.Add(item, i);
                i++;
            }
            int c = 2;
            file.lines = new List<TSVLine>();
            while (sr.Peek() > -1)
            {
                file.lines.Add(new TSVLine(sr.ReadLine(), file, c));
                c++;
            }
//            stream.Close();
            return file;
        }

        public static System.Threading.Tasks.Task<TSVFileBase> ReadTSVFile2(System.IO.StreamReader stream)
        {
            return  System.Threading.Tasks.Task.Factory.StartNew<TSVFileBase>((n) => {
                TSVFileBase file = new TSVFileBase();
                StreamReader sr = (StreamReader)n;
                string first = sr.ReadLine();
                int i = 0;
                file.headerDic.Clear();
                foreach (var item in first.Split('\t'))
                {
                    file.headerDic.Add(item, i);
                    i++;
                }
                int c = 2;
                file.lines = new List<TSVLine>();
                while (sr.Peek() > -1)
                {
                    file.lines.Add(new TSVLine(sr.ReadLine(), file, c));
                    c++;
                }
                //            stream.Close();
                return file;
            },stream);

        }


    }

    public class TSVFile : TSVFileBase, IDisposable
    {
        StreamReader stream;
        public TSVFile(StreamReader stream)
        {
            this.stream = stream;
        }


        public static IEnumerable<TSVLine> ReadLines(StreamReader stream, out IEnumerable<string> errList)
        {
            TSVFile r = new TSVFile(stream);
            var list = r.Lines;
            errList = r.ErrMessageList;
            return list;
        }


       




        public new IEnumerable<TSVLine>  Lines
        {
            get
            {
                StreamReader sr = stream;
                string first = sr.ReadLine();
                int i = 0;
                HeaderDic.Clear();
                foreach (var item in first.Split('\t'))
                {
                    HeaderDic.Add(item, i);
                    i++;
                }
                int c = 2;

                while (sr.Peek() > -1)
                {
                    yield return new TSVLine(sr.ReadLine(), this, c);
                    c++;
                }

            }
        }

        #region IDisposable メンバー

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Dispose();
            }
        }

        #endregion
    }

    public class TSVText : TSVFileBase
    {
        public TSVText(string text)
        {
            string first = text.ReadLines().First();
            int i = 0;
            HeaderDic.Clear();
            foreach (var item in first.Split('\t'))
            {
                HeaderDic.Add(item, i);
                i++;
            }
            int c = 2;
            foreach (var item in text.ReadLines().Skip(1))
            {
                Lines.Add(new TSVLine(item, this, c));
                c++;
            }

        }

    }

    public class TSVFile<T> : IFile, IDisposable
        where T : TSVLine, new()
    {
        StreamReader stream;
        public TSVFile(StreamReader stream)
        {
            this.stream = stream;
        }

        //public TSVFile(string fileName)
        //{
        //    this.stream = new StreamReader(fileName);
        //}

        Dictionary<string, int> headerDic = new Dictionary<string, int>();

        public IEnumerable<string> Header
        {
            get { return headerDic.Keys; }
        }
        public int GetIndex(string column)
        {
            if (headerDic.ContainsKey(column))
            {
                return headerDic[column];
            }
            else
            {
                return -1;
            }
        }

        public static IEnumerable<T> ReadLines(StreamReader stream, out IEnumerable<string> errList)
        {
            TSVFile<T> r = new TSVFile<T>(stream);
            var list = r.Lines;
            errList = r.ErrMessageList;
            return list;
        }

        List<T> lines = new List<T>();
        public IEnumerable<T> StockLines
        {
            get { return lines; }
        }
        public void Create()
        {
            StreamReader sr = stream;
            string first = sr.ReadLine();
            int i = 0;
            headerDic.Clear();
            foreach (var item in first.Split('\t'))
            {
                headerDic.Add(item, i);
                i++;
            }
            int c = 2;
            lines = new List<T>();
            while (sr.Peek() > -1)
            {
                T t = new T();
                t.Count = c;
                t.Line = sr.ReadLine();
                t.SetCsvRead(this);
                lines.Add(t);
                c++;
            }
        }



        public IEnumerable<T> Lines
        {
            get
            {
                StreamReader sr = stream;
                string first = sr.ReadLine();
                int i = 0;
                headerDic.Clear();
                foreach (var item in first.Split('\t'))
                {
                    headerDic.Add(item, i);
                    i++;
                }
                int c = 2;
                while (sr.Peek() > -1)
                {
                    T t = new T();
                    t.Count = c;
                    t.Line = sr.ReadLine();
                    t.SetCsvRead(this);
                    c++;
                    yield return t;
                }
                stream.Dispose();
            }
        }

        #region IDisposable メンバー

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Dispose();
            }
        }

        #endregion


        #region IFile メンバー


        List<string> errMessage = new List<string>();
        public IEnumerable<string> ErrMessageList
        {
            get
            {
                return errMessage;
            }
        }


        public void AddErrMessage(string message)
        {
            errMessage.Add(message);
        }

        #endregion

        #region IFile メンバー


        public void AddExtendColumn(string key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class TSVLine
    {
        string line;
        
        public string Line
        {
            get { return line; }
            set
            {
                line = value;
                this.data = line.Split('\t');
            }
        }
        string[] data;
        IFile csvRead;
          
        public Dictionary<string, string> ExtendColumn { get; set; }
         
        public int Count { get; set; }

        public TSVLine(string line, IFile read, int count)
        {
            this.line = line;
            this.csvRead = read;
            this.data = line.Split('\t');
            this.Count = count;
        }
        public TSVLine()
        {
        }

        public void SetCsvRead(IFile file)
        {
            csvRead = file;
        }

        private void SendErrMessage(string err)
        {
            this.csvRead.AddErrMessage(Count + "行目 エラー内容：" + err + " 「" + this.line + "」");
        }

        public string GetValue(int index)
        {
            if (index > -1)
            {
                if (data.Length > index)
                {
                    return data[index];
                }
                else
                {
                    SendErrMessage("列をはみ出しました");
                    throw new Exception("列をはみ出しました");
                }
            }
            SendErrMessage("0以上の数値を入れてください");
            throw new Exception("0以上の数値を入れてください");
        }
        public void AddExtendColumn(string key, string value)
        {
            if (ExtendColumn == null)
            {
                ExtendColumn = new Dictionary<string, string>();
            }
            csvRead.AddExtendColumn(key);
            if (ExtendColumn.ContainsKey(key))
            {
                SendErrMessage("AddExtendColumn:すでに存在しているKeyです。");
                ExtendColumn[key] = value;
            }
            else
            {
                ExtendColumn.Add(key, value);
            }
        }
        public void RemoveExtendColumn(string key)
        {
            if (ExtendColumn != null)
            {
                ExtendColumn.Remove(key);
            }
        }


        public string GetValue(string column)
        {
            if (ExtendColumn !=null && ExtendColumn.ContainsKey(column))
            {
                return ExtendColumn[column];
            }

            var i = csvRead.GetIndex(column);
            if (i > -1)
            {
                if (data.Length > i)
                {
                    return data[i];
                }
                else
                {
                    SendErrMessage("「" + column + "」の列が足りません");
                    throw new Exception("列をはみ出しました");
                }
            }
            else
            {
                SendErrMessage("「" + column + "」は存在しない列名です");
                throw new Exception("存在しない列名です");
            }
        }

        public string GetValue(string column, string defaultValue)
        {
            if (ExtendColumn != null && ExtendColumn.ContainsKey(column))
            {
                return ExtendColumn[column];
            }
            var i = csvRead.GetIndex(column);
            if (i > -1)
            {
                if (data.Length > i)
                {
                    return data[i];
                }
                else
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }


        public int GetIntValue(string column)
        {
            return int.Parse(GetValue(column));
        }
        public double GetDoubleValue(string column)
        {
            return double.Parse(GetValue(column));
        }
        public int GetIntValue(string column, int defaultValue)
        {
            int i = defaultValue;
            try
            {
                if (int.TryParse(GetValue(column), out i))
                {
                    return i;
                }
            }
            catch
            {
                SendErrMessage("列名「" + column + "」の取得に失敗しました。規定値を使います。");
            }
            return defaultValue;
        }
        public double GetDoubleValue(string column, double defaultValue)
        {
            double i = defaultValue;
            try
            {
                if (double.TryParse(GetValue(column), out i))
                {
                    return i;
                }
            }
            catch
            {
                SendErrMessage("列名「" + column + "」の取得に失敗しました。規定値を使います。");
            }
            return defaultValue;
        }

        public int GetIntValue(int index)
        {
            return int.Parse(GetValue(index));
        }
        public double GetDoubleValue(int index)
        {
            return double.Parse(GetValue(index));
        }
    }
}
