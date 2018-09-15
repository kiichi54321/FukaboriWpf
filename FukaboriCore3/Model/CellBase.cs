using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Draw;

namespace FukaboriCore.Model
{
    public class CellBase:GalaSoft.MvvmLight.ObservableObject
    {
        public Action OnClick { get; set; }
        string text;
        public string Text { get { return text; } set { Set(nameof(Text), ref text, value); } }
        bool ViewConpairText { get; set; }
        public string CompairText { get; set; }
        public uint CompairForceColor { get; set; }
        uint bgColor = KnownColor.White.ToUInt();
        public uint BGColor { get { return bgColor; } set { Set(nameof(BGColor), ref bgColor, value); } }
        public Func<uint> GetColor { get; set; }
    }
}
