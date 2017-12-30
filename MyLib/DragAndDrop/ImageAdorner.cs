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
    public class EllipseAdorner : Adorner
    {

        private Ellipse adornerChild = null;
        private double leftOffset = 0;
        private double topOffset = 0;

        public EllipseAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            VisualBrush _brush = new VisualBrush(adornedElement);

            adornerChild = new Ellipse();
            adornerChild.Width = adornedElement.RenderSize.Width;
            adornerChild.Height = adornedElement.RenderSize.Height;
            adornerChild.Fill = _brush;
            adornerChild.Opacity = 0.5;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            adornerChild.Measure(constraint);
            return adornerChild.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            adornerChild.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return adornerChild;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public double LeftOffset
        {
            get { return leftOffset; }
            set
            {
                leftOffset = value;
                UpdatePosition();
            }
        }

        public double TopOffset
        {
            get { return topOffset; }
            set
            {
                topOffset = value;
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            AdornerLayer adornerLayer = this.Parent as AdornerLayer;
            if (adornerLayer != null)
            {
                adornerLayer.Update(AdornedElement);
            }
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(leftOffset, topOffset));
            return result;
        }
    }
}
