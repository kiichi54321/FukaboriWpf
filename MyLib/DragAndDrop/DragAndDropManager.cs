using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyWpfLib.DragAndDrop
{

    /// <summary>
    /// http://nicoden.zxq.net C Sharp/チュートリアル/アプリケーション内のドラッグアンドドロップ/DragAndDropManager.cs　より
    /// </summary>
    public class DragAndDropManager
    {
        private static DragAndDropManager instance;
        public static DragAndDropManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DragAndDropManager();
                }
                return instance;
            }
        }


        //=====================================================================
        #region Constructors
        //=====================================================================
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        private DragAndDropManager()
        {
        }
        #endregion Constructors


        //=====================================================================
        #region Dependency Properties
        //=====================================================================
        public static readonly DependencyProperty IsAllowDragProperty =
            DependencyProperty.RegisterAttached("IsAllowDrag",
            typeof(bool),
            typeof(DragAndDropManager),
            new UIPropertyMetadata(false, IsAllowDragChanged));

        public static bool GetIsAllowDrag(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAllowDragProperty);
        }

        public static void SetIsAllowDrag(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAllowDragProperty, value);
        }

        public static readonly DependencyProperty IsAllowDropProperty =
            DependencyProperty.RegisterAttached("IsAllowDrop",
            typeof(bool),
            typeof(DragAndDropManager),
            new UIPropertyMetadata(false, IsAllowDropChanged));

        public static bool GetIsAllowDrop(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAllowDropProperty);
        }

        public static void SetIsAllowDrop(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAllowDropProperty, value);
        }

        #endregion Dependency Properties


        //=====================================================================
        #region Handlers
        //=====================================================================

        private static void IsAllowDragChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Panel dragSourcePanel = sender as Panel;
            Contract.Requires(dragSourcePanel != null, "ドラッグソースを設定できるのはPanel型のみです");

            if (Equals(e.NewValue, true))
            {
                dragSourcePanel.PreviewMouseLeftButtonDown += Instance.OnDragSource_PreviewMouseLeftButtonDown;
                dragSourcePanel.PreviewMouseLeftButtonUp += Instance.OnDragSource_PreviewMouseLeftButtonUp;
                dragSourcePanel.PreviewMouseMove += Instance.OnDragSource_PreviewMouseMove;
            }
            else
            {
                dragSourcePanel.PreviewMouseLeftButtonDown -= Instance.OnDragSource_PreviewMouseLeftButtonDown;
                dragSourcePanel.PreviewMouseLeftButtonUp -= Instance.OnDragSource_PreviewMouseLeftButtonUp;
                dragSourcePanel.PreviewMouseMove -= Instance.OnDragSource_PreviewMouseMove;
            }
        }


        private static void IsAllowDropChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Panel dragTargetPanel = sender as Panel;
            Contract.Requires(dragTargetPanel != null, "ドラッグターゲットを設定できるのはPanel型のみです");

            if (Equals(e.NewValue, true))
            {
                dragTargetPanel.AllowDrop = true;

                dragTargetPanel.PreviewDragEnter += Instance.OnDragTarget_PreviewDragEnter;
                dragTargetPanel.PreviewDragOver += Instance.OnDragTarget_PreviewDragOver;
                dragTargetPanel.PreviewDragLeave += Instance.OnDragTarget_PreviewDragLeave;
                dragTargetPanel.PreviewDrop += Instance.OnDragTarget_PreviewDrop;
            }
            else
            {
                dragTargetPanel.AllowDrop = false;

                dragTargetPanel.PreviewDragEnter -= Instance.OnDragTarget_PreviewDragEnter;
                dragTargetPanel.PreviewDragOver -= Instance.OnDragTarget_PreviewDragOver;
                dragTargetPanel.PreviewDragLeave -= Instance.OnDragTarget_PreviewDragLeave;
                dragTargetPanel.PreviewDrop -= Instance.OnDragTarget_PreviewDrop;
            }
        }


        /// <summary>
        /// ドラッグソース内でマウスの左クリックを受けた場合の実行関数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.dragSourcePanel = sender as Panel;

            if (dragSourcePanel.Children.Contains(e.OriginalSource as FrameworkElement))
            {
                dragSourceItem = e.OriginalSource as UIElement;
                rootWindow = Window.GetWindow(dragSourcePanel);

                startPoint = e.GetPosition(rootWindow);

                dragData = "何かドラッグするデータ";

                Debug.WriteLine("D&Dを開始");
            }
        }

        /// <summary>
        /// ドラッグソース内でマウスの左クリックが解除された場合の実行関数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // D&D処理を中止とする
            dragData = null;
            Debug.WriteLine("D&Dを中止");
        }


        /// <summary>
        /// ドラッグソース内でのマウス移動の実行関数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragSource_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (dragData != null)
            {
                Panel dragTargetPanel = sender as Panel;

                Debug.WriteLine("ドラッグ中 " + DateTime.Now);

                // D&Dのデータ
                DataObject dddata = new DataObject(DataFormats.StringFormat, this.dragData);


                bool oldAllowDrop = this.rootWindow.AllowDrop;
                rootWindow.AllowDrop = true;
                rootWindow.DragEnter += OnRootWindow_DragEnter;
                rootWindow.DragOver += OnRootWindow_DragOver;
                rootWindow.DragLeave += OnRootWindow_DragLeave;

                DragDropEffects effects = DragDrop.DoDragDrop((DependencyObject)sender, dddata, DragDropEffects.Move);


                //----------------
                // D&D終了の処理

                RemoveDragAdorner();

                rootWindow.AllowDrop = oldAllowDrop;
                rootWindow.DragEnter -= OnRootWindow_DragEnter;
                rootWindow.DragOver -= OnRootWindow_DragOver;
                rootWindow.DragLeave -= OnRootWindow_DragLeave;

                this.dragData = null;
            }
        }

        private void OnRootWindow_DragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnRootWindow_DragEnter");

            e.Effects = DragDropEffects.None; // マウスカーソルの形状をドロップ不可能に。
            e.Handled = true;
        }

        private void OnRootWindow_DragOver(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnRootWindow_DragOver");

            var rootWindow = sender as FrameworkElement;

            ShowDragAdorner(new Point
            {
                X = e.GetPosition(rootWindow).X,
                Y = e.GetPosition(rootWindow).Y
            });


            e.Effects = DragDropEffects.None; // マウスカーソルの形状をドロップ不可能に。
            e.Handled = true;                 // イベントを処理したことを通知
        }

        private void OnRootWindow_DragLeave(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnRootWindow_DragLeave");

            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragTarget_PreviewDrop(object sender, DragEventArgs e)
        {
            object draggedItem = e.Data.GetData(DataFormats.StringFormat);
            if (draggedItem != null)
            {
                // TODO: ドロップしたときの処理を追加する

                e.Handled = true; // イベントを処理したことを通知
            }
        }

        private void OnDragTarget_PreviewDragOver(object sender, DragEventArgs e)
        {
            object draggedItem = e.Data.GetData(DataFormats.StringFormat);
            if (draggedItem != null)
            {
                var panel = sender as UIElement;
                ShowDragAdorner(new Point
                {
                    X = e.GetPosition(rootWindow).X,
                    Y = e.GetPosition(rootWindow).Y
                });

            }
            e.Handled = true;
        }

        private void OnDragTarget_PreviewDragEnter(object sender, DragEventArgs e)
        {
            object draggedItem = e.Data.GetData(DataFormats.StringFormat);
            if (draggedItem != null)
            {
            }
            e.Handled = true;
        }

        private void OnDragTarget_PreviewDragLeave(object sender, DragEventArgs e)
        {
            object draggedItem = e.Data.GetData(DataFormats.StringFormat);
            if (draggedItem != null)
            {

            }
            e.Handled = true;
        }

        #endregion Handlers

        /// <summary>
        /// ドラッグ中のAdornerを表示
        /// </summary>
        /// <param name="currentPosition"></param>
        private void ShowDragAdorner(Point currentPosition)
        {
            if (dragAdorner == null)
            {
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(dragSourcePanel);
                this.dragAdorner = new DragAdorner(dragSourcePanel, dragSourceItem);

                layer.Add(this.dragAdorner);
            }

            dragAdorner.SetPosition(currentPosition.X, currentPosition.Y);
        }

        private void RemoveDragAdorner()
        {
            if (dragAdorner != null)
            {
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(dragSourcePanel);
                layer.Remove(dragAdorner);

                this.dragAdorner = null;
            }
        }


        DragAdorner dragAdorner;
        Point startPoint;
        UIElement dragSourceItem;
        Panel dragSourcePanel;
        Window rootWindow;
        object dragData;
    }


    public class DragAdorner : Adorner
    {
        //=====================================================================
        #region Constructors
        //=====================================================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="adornElement"></param>
        public DragAdorner(UIElement owner, UIElement adornElement)
            : base(owner)
        {
            _Owner = owner;

            Brush brush = new VisualBrush(adornElement);

            var rectangle = new Rectangle
            {
                Width = adornElement.RenderSize.Width,
                Height = adornElement.RenderSize.Height,
                Fill = brush,
                Opacity = 0.7
            };

            Point mousePoint = Mouse.PrimaryDevice.GetPosition(adornElement);
            XCenter = mousePoint.X;
            YCenter = mousePoint.Y;

            _Child = rectangle;
        }

        #endregion Constructors



        /// <summary>
        /// Adornerの表示座標
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public void SetPosition(double left, double top)
        {
            _TopOffset = top;
            _LeftOffset = left;

            UpdatePosition();
        }


        //=====================================================================
        #region Properties
        //=====================================================================

        public double TopOffset
        {
            get
            {
                return _TopOffset;
            }
            set
            {
                _TopOffset = value - YCenter;
                UpdatePosition();
            }
        }

        public double LeftOffset
        {
            get
            {
                return _LeftOffset;
            }
            set
            {
                _LeftOffset = value - XCenter;
                UpdatePosition();
            }
        }

        #endregion Properties


        //=====================================================================
        #region Adorner Methods
        //=====================================================================
        protected override Visual GetVisualChild(int index)
        {
            return this._Child;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override Size MeasureOverride(Size finalSize)
        {
            _Child.Measure(finalSize);
            return _Child.DesiredSize;
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            _Child.Arrange(new Rect(finalSize));
            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_LeftOffset, _TopOffset));
            return result;
        }
        #endregion Adorner Methods


        private void UpdatePosition()
        {
            var adorner = (AdornerLayer)Parent;
            if (adorner != null)
                adorner.Update(AdornedElement);
        }




        private double _LeftOffset;
        private double _TopOffset;
        protected UIElement _Child;
        protected UIElement _Owner;
        protected double XCenter;
        protected double YCenter;
    }

}
