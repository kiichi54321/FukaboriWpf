using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MyWpfLib.Mathematics
{
    /// <summary>
    /// 幾何学用のライブラリー
    /// </summary>
    public static class Geometry
    {

        /// <summary>
        /// 中点を求める
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Point GetMiddlePoint(Point p1, Point p2)
        {
            return GetInternalPoint(p1, p2, 0.5);
        }


        /// <summary>
        /// 2点と比より、内分点を求める
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Point GetInternalPoint(Point p1, Point p2, double r)
        {
            return new Point(((p2.X - p1.X) * r) + p1.X, ((p2.Y - p1.Y) * r) + p1.Y);
        }



        //public struct LineF
        //{
        //    public LineF(PointF p1, PointF p2)
        //    {
        //        this.point1 = p1;
        //        this.point2 = p2;
        //    }
        //    private PointF point1, point2;

        //    public PointF Point2
        //    {
        //        get { return point2; }
        //        set { point2 = value; }
        //    }

        //    public PointF Point1
        //    {
        //        get { return point1; }
        //        set { point1 = value; }
        //    }

        //    /// <summary>
        //    /// 傾き
        //    /// </summary>
        //    public float Slope
        //    {
        //        get
        //        {
        //            return (point1.Y - point2.Y) / (point1.X - point2.X);
        //        }
        //    }
        //    /// <summary>
        //    /// 切片
        //    /// </summary>
        //    public float Intercept
        //    {
        //        get
        //        {
        //            return point1.Y - this.Slope * point1.X;
        //        }
        //    }
        //    /// <summary>
        //    /// 角度(ラジアン)
        //    /// </summary>
        //    public double Angle
        //    {
        //        get
        //        {
        //            if ((point1.X - point2.X) != 0)
        //            {
        //                return Math.Tanh((point1.Y - point2.Y) / (point1.X - point2.X));
        //            }
        //            else
        //            {
        //                var c = Math.Cosh((double)(point1.Y - point2.Y) / this.Distance);
        //                var s = Math.Sinh((double)(point1.X - point2.X) / this.Distance);

        //                return c;
        //            }
        //        }
        //    }

        //    /// <summary>
        //    /// 距離
        //    /// </summary>
        //    public double Distance
        //    {
        //        get
        //        {
        //            return Math.Sqrt(Math.Pow((point1.Y - point2.Y), 2) + Math.Pow((point1.X - point2.X), 2));
        //        }
        //    }
        //}
        public struct Line
        {
            public Line(Point p1, Point p2)
            {
                this.point1 = p1;
                this.point2 = p2;
            }
            private Point point1, point2;

            public Point Point2
            {
                get { return point2; }
                set { point2 = value; }
            }

            public Point Point1
            {
                get { return point1; }
                set { point1 = value; }
            }

            /// <summary>
            /// 傾き
            /// </summary>
            public double Slope
            {
                get
                {
                    return (point1.Y - point2.Y) / (point1.X - point2.X);
                }
            }
            /// <summary>
            /// 切片
            /// </summary>
            public double Intercept
            {
                get
                {
                    return point1.Y - this.Slope * point1.X;
                }
            }
            /// <summary>
            /// 角度
            /// </summary>
            public double Angle
            {
                get
                {
                    return Math.Sinh((point1.Y - point2.Y) / this.Distance);
                }
            }

            /// <summary>
            /// 距離
            /// </summary>
            public double Distance
            {
                get
                {
                    return Math.Sqrt(Math.Pow((point1.Y - point2.Y), 2) + Math.Pow((point1.X - point2.X), 2));
                }
            }
        }

        /// <summary>
        /// 交点算出 交点がない場合、Point(double.NaN,double.NaN)　を返す
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static Point GetIntersection(Line line1, Line line2)
        {
            double BUNBO = (double)((line1.Point2.X - line1.Point1.X) * (line2.Point2.Y - line2.Point1.Y) - (line1.Point2.Y - line1.Point2.Y) * (line2.Point2.X - line2.Point1.X));
            if (BUNBO != 0)
            {
                double r = ((line2.Point2.Y - line2.Point1.Y) * (line2.Point1.X - line1.Point1.X) - (line2.Point2.X - line2.Point1.X) * (line2.Point1.Y - line1.Point1.Y)) / BUNBO;
                return GetInternalPoint(line1.Point1, line1.Point2, r);
            }
            else
            {
                return Geometry.NullPoint();
            }
        }

        public static Point GetIntersection2(Line line1, Line line2)
        {
            if (Math.Sin(line1.Angle) != 0)
            {
                if (Math.Sin(line2.Angle) != 0)
                {
                    if (line1.Slope == line2.Slope)
                    {
                        return NullPoint();
                    }
                    else
                    {
                        double x = (line1.Intercept - line2.Intercept) / -(line1.Slope - line2.Slope);
                        double y = line1.Slope * x + line1.Intercept;
                        return new Point(x, y);
                    }
                }
                else
                {
                    double x = line2.Point1.X;
                    double y = line1.Slope * x + line1.Intercept;
                    return new Point(x, y);
                }
            }
            else
            {
                if (line2.Point1.X == line2.Point2.X)
                {
                    return NullPoint();
                }
                else
                {
                    double x = line1.Point1.X;
                    double y = line2.Slope * x + line2.Intercept;
                    return new Point(x, y);
                    
                }
            }
        }

        /// <summary>
        /// Point(double.NaN, double.NaN); を返す
        /// </summary>
        /// <returns></returns>
        public static Point NullPoint()
        {
            return new Point(double.NaN, double.NaN);
        }

        /// <summary>
        /// Point(double.NaN, double.NaN) の場合、True
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsNullPoint(Point point)
        {
            if (point.X == double.NaN && point.Y == double.NaN)
            {
                return true;
            }
            return false;
        }


        ///// <summary>
        ///// 交点算出
        ///// </summary>
        ///// <param name="line1"></param>
        ///// <param name="line2"></param>
        ///// <returns></returns>
        //public static PointF GetIntersectionF(LineF line1, LineF line2)
        //{
        //    float BUNBO = ((line1.Point2.X - line1.Point1.X) * (line2.Point2.Y - line2.Point1.Y) - (line1.Point2.Y - line1.Point2.Y) * (line2.Point2.X - line2.Point1.X));
        //    if (BUNBO != 0)
        //    {
        //        float r = ((line2.Point2.Y - line2.Point1.Y) * (line2.Point1.X - line1.Point1.X) - (line2.Point2.X - line2.Point1.X) * (line2.Point1.Y - line1.Point1.Y)) / BUNBO;
        //        return GetInternalPointF(line1.Point1, line1.Point2, r);
        //    }
        //    else
        //    {
        //        return PointF.Empty;
        //    }
        //}

        /// <summary>
        /// 交点算出(傾きと切片を使って)
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static Point GetIntersectionF2(Line line1, Line line2)
        {
            double x = (line2.Intercept - line1.Intercept) / (line1.Slope - line2.Slope);
            double y = line1.Slope * x + line1.Intercept;
            return new Point(x, y);
        }

        ///// <summary>
        ///// 指定した角度と長さ分だけ移動する。
        ///// </summary>
        ///// <param name="point">場所</param>
        ///// <param name="r">角度</param>
        ///// <param name="w">長さ</param>
        ///// <returns></returns>
        //public static PointF MovePointF(PointF point, double r, double w)
        //{
        //    PointF pf = new PointF();
        //    pf.X = point.X + (float)(Math.Cos(r) * w);
        //    pf.Y = point.Y + (float)(Math.Sin(r) * w);
        //    return pf;
        //}

        /// <summary>
        /// 指定した角度と長さ分だけ移動する。
        /// </summary>
        /// <param name="point">場所</param>
        /// <param name="r">角度</param>
        /// <param name="w">長さ</param>
        /// <returns></returns>
        public static Point MovePoint(Point point, double r, double w)
        {
            Point pf = new Point();
            pf.X = point.X + (int)(Math.Cos(r) * w);
            pf.Y = point.Y + (int)(Math.Sin(r) * w);
            return pf;
        }

        /// <summary>
        /// 重心を求める
        /// </summary>
        /// <param name="pointList"></param>
        /// <returns></returns>
        public static Point GetCenterOfGravity(Point[] pointList)
        {
            Point p = new Point(0, 0);
            foreach (var item in pointList)
            {
                p.X += item.X;
                p.Y += item.Y;
            }
            p.X = p.X / (double)pointList.Length;
            p.Y = p.Y / (double)pointList.Length;

            return p;
        }

        /// <summary>
        /// 2点間の距離
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
