using System;
using System.ComponentModel;
using MyWpfLib.Extend;


namespace MyWpf.DataGridDynamicColumn
{
    public class DataViewModel : INotify
    {
        public DataViewModel()
        {
            Value = "";
            BackgroundColor = "";
        }

        #region Value変更通知プロパティ
        private string _Value;

        public string Value
        {
            get
            { return _Value; }
            set
            {
                if (_Value == value)
                    return;
                _Value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }
        #endregion


        #region BackgroundColor変更通知プロパティ
        private string _BackgroundColor;

        public event PropertyChangedEventHandler PropertyChanged;

        public string BackgroundColor
        {
            get
            { return _BackgroundColor; }
            set
            {
                if (_BackgroundColor == value)
                    return;
                _BackgroundColor = value;
                RaisePropertyChanged(nameof(BackgroundColor));
            }
        }

        public void RaisePropertyChanged(string name)
        {
            this.PropertyChanged.Raise(this, name);
        }
        #endregion

    }
}
