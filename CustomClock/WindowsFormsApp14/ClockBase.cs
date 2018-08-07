using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace WindowsFormsApp14
{
    class ClockBase : UserControl
    {
        public ClockBase()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            _Radius = (this.Width < this.Height) ? this.Width / 2 : this.Height / 2;
            _Center = new Point(Radius, Radius);
            _Painter = CreatePainter();
            _HourPoints = new List<Point>();
            _MinutePoints = new List<Point>();
            _SecondPoints = new List<Point>();
            SetTimer();
        }

       
        Point _Center;
        int _Radius;
        PaintHelper _Painter;
        List<Point> _HourPoints;
        List<Point> _MinutePoints;
        List<Point> _SecondPoints;
        Color _HourColor;
        Color _MinuteColor;
        Color _SecondColor;

        [Category("HourHandProperties")]
        [Browsable(true)]
        [Editor(typeof(ClockHandPointsEditor), typeof(UITypeEditor))]
        public List<Point> HourPoints
        {
            get {  return _HourPoints; }
            set
            {
                _HourPoints = value;
                if (DesignMode)
                    this.Invalidate();

            }
        }

        [Category("HourHandProperties")]
        [Browsable(true)]
        [TypeConverter(typeof(ColorConverter))]
        public Color HourColor
        {
            get { return _HourColor; }
            set { _HourColor = value;
                if (DesignMode)
                    this.Invalidate();
            }
        }

        [Category("MinuteHandProperties")]
        [Browsable(true)]
        [Editor(typeof(ClockHandPointsEditor), typeof(UITypeEditor))]
        public List<Point> MinutePoints
        {
            get { return _MinutePoints; }
            set
            {
                _MinutePoints = value;
                if (DesignMode)
                    this.Invalidate();
            }
        }

        [Category("MinuteHandProperties")]
        [Browsable(true)]
        [TypeConverter(typeof(ColorConverter))]
        public Color MinuteColor
        {
            get { return _MinuteColor; }
            set { _MinuteColor = value;
                if (DesignMode)
                    this.Invalidate();
            }
        }

        [Category("SecondHandProperties")]
        [Browsable(true)]
        [Editor(typeof(ClockHandPointsEditor), typeof(UITypeEditor))]
        public List<Point> SecondPoints
        {
            get { return _SecondPoints; }
            set
            {
                _SecondPoints = value;
                if (DesignMode)
                    this.Invalidate();
            }
        }

        [Category("SecondHandProperties")]
        [Browsable(true)]
        [TypeConverter(typeof(ColorConverter))]
        public Color SecondColor
        {
            get { return _SecondColor; }
            set { _SecondColor = value;
                if (DesignMode)
                    this.Invalidate();
            }
        }

        public int Radius
        {
            get { return _Radius; }
            set { _Radius = value; }
        }
        public Point Center
        {
            get { return _Center; }
            set { _Center = value; }
        }
        
        public PaintHelper Painter { get { return _Painter; } }

        protected void SetTimer()
        {
            Timer updater = new Timer();
            updater.Interval = 1000;
            updater.Tick += Updater_Tick;
            updater.Start();
        }

        protected List<Point> AdjustPoints(List<Point> points)
        {
            List<Point> adjustedPoints = new List<Point>(points.Count);

            double ratioW = Radius / 250.0;

            for (int i = 0; i < points.Count; i++)
            {
                Point temp = new Point((int)(points[i].X * ratioW), (int)(points[i].Y * ratioW));
                temp.Offset(Center.X - Radius, Center.Y - Radius);
                adjustedPoints.Add(temp);
            }
            return adjustedPoints;
        }

        protected void Updater_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected PaintHelper CreatePainter()
        {
            return new PaintHelper();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Painter.DrawClockBase(e.Graphics, AdjustPoints(HourPoints), AdjustPoints(MinutePoints), AdjustPoints(SecondPoints), Center, Radius, HourColor, MinuteColor, SecondColor);
        }
        protected override void OnResize(EventArgs e)
        {
            Radius = (this.Width < this.Height) ? this.Width / 2 : this.Height / 2;
            Center = new Point(this.Width / 2, this.Height / 2);
            base.OnResize(e);
            this.Invalidate();
        }

       



    }
}
