using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MyWpfLib.Controls
{
    public class TreeNode : INotifyPropertyChanged
    {
        public TreeNode()
        {
        }

        public TreeNode(string text)
        {
            this.text = text;
        }

        public TreeNode(string text, System.Collections.ObjectModel.ObservableCollection<TreeNode> nodes)
        {
            this.Text = text;
            this.nodes = nodes;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {

            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(info));

            }

        }

        private ObservableCollection<TreeNode> nodes = new ObservableCollection<TreeNode>();

        public ObservableCollection<TreeNode> Nodes
        {

            get
            {

                return nodes;

            }

            set
            {

                if (value != nodes)
                {

                    nodes = value;

                    NotifyPropertyChanged("Nodes");

                }

            }

        }



        private string text = "";

        public string Text
        {

            get
            {

                return text;

            }

            set
            {

                if (value != text)
                {

                    text = value;

                    NotifyPropertyChanged("Text");

                }

            }

        }
    }
}
