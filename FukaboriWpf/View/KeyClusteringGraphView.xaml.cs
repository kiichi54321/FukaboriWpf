using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CrossTableSilverlight.Model;

namespace CrossTableSilverlight.View
{
    public partial class KeyClusteringGraphView : UserControl
    {
        public KeyClusteringGraphView()
        {
            InitializeComponent();
            graphManage = new MyWpfLib.Graph.GraphManage(canvas);
            this.spring.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(spring_RunWorkerCompleted);

    }
        Random random = new Random();
        void spring_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            spring.NodeUpdate();
            graphManage.Update();
        }
        MyWpfLib.Graph.GraphManage graphManage;
        MyWpfLib.Graph.Spring spring = new MyWpfLib.Graph.Spring();
        bool isNewGraph = true;

        public void Spring()
        {
            if (spring.IsBusy == false)
            {
                spring.ClientArea = new MyWpfLib.Draw.Area(new Size() { Height = canvas.ActualHeight, Width = canvas.ActualWidth });

                spring.Clear();
                foreach (var item in nodeDic)
                {
                    spring.AddNode(item.Value.Node);
                }
                if (isNewGraph == false)
                {
                    spring.AutoParameter();
                }
                spring.Run();
                isNewGraph = false;
            }
        }

        public QuestionClusterManage QuestionClusterManage
        {
            get { return (QuestionClusterManage)GetValue(QuestionClusterManageProperty); }
            set { SetValue(QuestionClusterManageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QuestionClusterManage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuestionClusterManageProperty =
            DependencyProperty.Register("QuestionClusterManage", typeof(QuestionClusterManage), typeof(KeyClusteringGraphView), new PropertyMetadata(null,updateKeyClusterData ));


        private static void updateKeyClusterData(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var clusterResult = e.NewValue as QuestionClusterManage;
            var keyClusteringGraphView = d as KeyClusteringGraphView;
            if(e.NewValue != e.OldValue)
            {
                keyClusteringGraphView.Clear();
            }
     //       keyClusteringGraphView.CreateGraph();
        }
        Dictionary<QuestionCluster, MyWpfLib.Graph.NodeControl> nodeDic = new Dictionary<QuestionCluster, MyWpfLib.Graph.NodeControl>();
        List<QuestionClusterManage.ClusterLink> nodeRelation = new List<QuestionClusterManage.ClusterLink>();
        List<MyWpfLib.Graph.Link> linkList = new List<MyWpfLib.Graph.Link>();
        
        Point GetRandomPoint()
        {
            return new Point(random.NextDouble(),random.NextDouble());
        }

        public void Clear()
        {
            nodeDic.Clear();
            graphManage.Clear();
            linkList.Clear();

        }

        public void CreateGraph()
        {
            nodeDic.Clear();
            graphManage.Clear();
            if (QuestionClusterManage == null) return;

            foreach (var item in QuestionClusterManage.Clusters)
            {
                var node = graphManage.CreateNode(item.Name, GetRandomPoint(), Colors.Blue);
           //     var node  =  graphManage.CreateNode(new MyWpfLib.Graph.Node() { NodeName = item.Name, NodeFillColor = Colors.Blue });
                node.Node.Size = new Size(20, 20);
                node.CanRemove = false;
                nodeDic.Add(item, node);
            }
            nodeRelation = QuestionClusterManage.GetClusterRelation();
            linkList.Clear();
            foreach (var item in nodeRelation)
            {
                var link = graphManage.CreateLink(nodeDic[item.Cluster1].Node, nodeDic[ item.Cluster2].Node);
                link.LineColor = Colors.Black;
                link.StrokeThickeness = 1;
                link.SortKey = item.Value;
                link.SortType = MyWpfLib.Graph.Sort.Type.Custom;
                link.CanSelect = false;
                link.TextValue = item.Value.ToString("F4");
                link.Visibility = Visibility.Collapsed;
                linkList.Add(link);
            }
            foreach (var item in linkList)
            {
                ChangeLinkVisibility(item);
            }
            isNewGraph = true;
            Spring();
        }
    
        public double Min
        {
            get { return minLinkValue.Value; }
        }

        void ChangeLinkVisibility(MyWpfLib.Graph.Link link)
        {
            if( link.SortKey >= Min)
            {
                link.Visibility = Visibility.Visible;
            }
            else
            {
                link.Visibility = Visibility.Collapsed;
            }
        }

        private void minLinkValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (var item in linkList)
            {
                ChangeLinkVisibility(item);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateGraph();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Spring();
        }

        Visibility lineTextVisibility = Visibility.Collapsed;
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lineTextVisibility = MyWpfLib.Utility.ChangeVisibility(lineTextVisibility);
            foreach (var item in linkList)
            {
                item.TextVisibility = lineTextVisibility;
                item.Layout();
            }
        }
    }
}
