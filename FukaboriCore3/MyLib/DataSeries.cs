using System;
using System.Net;
using System.Windows;

using System.Collections.Generic;

using System.Collections;
using System.Linq;

namespace MySilverlightLibrary.DataVisualization
{

    public class DataPointCollection : List<IData>
    {
        public DataPointCollection()
            : base()
        {

        }
        public DataPointCollection(IEnumerable<IData> data)
            : base()
        {
            if (data != null)
            {
                this.AddRange(data);
            }
        }
    }




    public interface IData
    {
        double Value { get; set; }
        double MaxValue { get; set; }
        string Label { get; set; }
    }


    public class DataPoint : IData
    {
        public double Value { get; set; }
        public double MaxValue { get; set; }
        public string Label { get; set; }
        public Uri LabelImageUri { get; set; }
    }
}
