using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp14
{
    class PaintHelper
    {
        public PaintHelper() { }

        public void DrawClockBase(Graphics graph, List<Point> hourPoints, List<Point> minutePoints, List<Point> secondPoints, Point center, int radius, Color hourColor, Color minuteColor, Color secondColor)
        {
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.White, 4);
            DrawCircleAndFillIt(graph, pen, new SolidBrush(Color.Indigo), center, radius);
            DrawClockLines(graph, pen, center, radius);
            DrawDigits(graph, center, radius);
            DrawClockHands(graph, hourPoints, minutePoints, secondPoints, center, hourColor, minuteColor, secondColor);
        }
        private void DrawClockLines(Graphics graph, Pen pen, Point center, int radius)
        {
            for (int i = 0; i < 360; i += 6)
            {
                Point outerPoint = FindPointOnCircle(center, i, radius);
                int smallerRadius = (i % 30 == 0) ? (int)(radius * 0.85) : (int)(radius * 0.9);
                Point innerPoint = FindPointOnCircle(center, i, smallerRadius);
                graph.DrawLine(pen, innerPoint, outerPoint);
            }
        }
        private Point FindPointOnCircle(Point center, int angle, int radius)
        {
            int x1 = Convert.ToInt32(Math.Cos(angle * Math.PI / 180) * radius + center.X);
            int y1 = Convert.ToInt32(Math.Sin(angle * Math.PI / 180) * radius + center.Y);
            return new Point(x1, y1);
        }
        private void DrawCircleAndFillIt(Graphics graph, Pen pen, Brush brush, Point center, float radius)
        {
            graph.DrawEllipse(pen, center.X - radius, center.Y - radius,
                           radius + radius, radius + radius);
            graph.FillEllipse(brush, center.X - radius, center.Y - radius,
                          radius + radius, radius + radius);
        }
        private void DrawDigits(Graphics graph, Point center, int radius)
        {
            int angle = 300;
            Font font = new Font(FontFamily.Families[5], radius / 11);
            Size size = new Size(radius / 6, radius / 6);
            Point location;

            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            for (int i = 1; i <= 12; i++)
            {
                string text = i.ToString();
                Point temp = FindPointOnCircle(center, angle, Convert.ToInt32(radius * 0.7));
                location = new Point(temp.X - size.Width / 2, temp.Y - size.Height / 2);
                RectangleF rect = new RectangleF(location, size);
                graph.DrawString(text, font, new SolidBrush(Color.White), rect, format);
                angle = (angle + 30 < 360) ? angle + 30 : 0;
            }


        }
        private void DrawClockHands(Graphics graph, List<Point> hourPoints, List<Point> minutePoints, List<Point> secondPoints, Point center, Color hourColor, Color minuteColor, Color secondColor)
        {
            DrawClockHand(graph, hourPoints, hourColor, center, ClockHandType.HourHand);
            DrawClockHand(graph, minutePoints, minuteColor, center, ClockHandType.MinuteHand);
            DrawClockHand(graph, secondPoints, secondColor, center, ClockHandType.SecondHand);
        }
        private void DrawClockHand(Graphics graph, List<Point> points, Color color, Point center, ClockHandType type)
        {

            if (points.Count >= 3)
            {
                int angle = GetClockHandTypeSpecificAngle(type);
                List<Point> polygonTransformed = RotatePolygon(points, center, angle);
                graph.FillPolygon(new SolidBrush(color), polygonTransformed.ToArray());
            }
        }
        private int GetClockHandTypeSpecificAngle(ClockHandType type)
        {
            DateTime currentTime =              DateTime.Now;
            switch (type)
            {
                case ClockHandType.HourHand:
                    return GetAngle(currentTime.Hour, 30, currentTime.Minute);
                case ClockHandType.MinuteHand:
                    return GetAngle(currentTime.Minute, 6);
                case ClockHandType.SecondHand:
                    return GetAngle(currentTime.Second, 6);
                default:
                    return 0;
            }
        }
        private int GetAngle(int time, int step, int minutes = 0)
        {
            int angle = 0;
            for (int i = 0; i < time; i++)
            {
                angle += step;
            }
            if (minutes != 0)
                angle += 6 * (minutes / 12);
            return angle;
        }
        private List<Point> RotatePolygon(List<Point> polygon, Point centroid, double angle)
        {
            List<Point> rotated = new List<Point>(polygon.Count);
            for (int i = 0; i < polygon.Count; i++)
            {
                rotated.Add(RotatePoint(polygon[i], centroid, angle));
            }
            return rotated;
        }
        static Point RotatePoint(Point pointToRotate, Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
    }
}
