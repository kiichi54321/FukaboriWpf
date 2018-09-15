using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib
{
    /// <summary>
    /// ŠK‘w‚Â‚«ƒŠƒXƒg
    /// </summary>
    /// <typeparam name="Tvalue"></typeparam>
    public class HierarchicalList<Tvalue>
    {
        private Dictionary<int, List<Tvalue>> dic;
        private int layer = 1;

        public HierarchicalList()
        {
            dic = new Dictionary<int, List<Tvalue>>();
            layer = 1;
        }

        public void NextLayer()
        {
            if (dic.ContainsKey(layer)==false)
            {
                dic.Add(layer,new List<Tvalue>());
            }
            if (dic.ContainsKey(layer + 1) == false)
            {
                dic.Add(layer + 1, new List<Tvalue>());
            }
            layer++;
        }
        public void BackLayer()
        {
            layer--;
        }

        public int Layer
        {
            get
            {
                return layer;
            }
            set
            {
                if (value < 1)
                {
                    layer = 1;
                }
                else if (MaxLayer < value)
                {
                    layer = MaxLayer;
                }
                else
                {
                    layer = value;
                }

            }
        }

        public void Clear()
        {
            dic.Clear();
            layer = 1;
        }

        public List<Tvalue> LayerList
        {
            get
            {
                return dic[layer];
            }
        }

        public List<Tvalue> GetList(int layer)
        {
            if (dic.ContainsKey(layer))
            {
                return dic[layer];
            }
            else
            {
                return new List<Tvalue>();
            }
        }

        public int MaxLayer
        {
            get
            {
                return dic.Count;
            }
        }

        public void Add(Tvalue value)
        {
            if (dic.ContainsKey(layer))
            {
                dic[layer].Add(value);
            }
            else
            {
                List<Tvalue> list = new List<Tvalue>();
                list.Add(value);
                dic.Add(layer, list);
            }

        }

        public void AddRange(ICollection<Tvalue> collection)
        {
            if (dic.ContainsKey(layer))
            {
                dic[layer].AddRange(collection);
            }
            else
            {
                List<Tvalue> list = new List<Tvalue>();
                list.AddRange(collection);
                dic.Add(layer, list);
            }
        }
    }
}
