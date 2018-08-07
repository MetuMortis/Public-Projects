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
    public class ClockHandPointsSelector : UserControl
    {
        public ClockHandPointsSelector(Form form)
        {
            this.Size = form.ClientSize;
            this.Location = new Point(0, 0);
            _Points = new List<Point>();
        }

        List<Point> _Points;

        public List<Point> Points
        {
            get { return _Points; }
            set { _Points = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(this.Width / 2, this.Top), new Point(this.Width / 2, this.Height));
            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(this.Left, this.Height / 2), new Point(this.Width, this.Height / 2));
            if (Points.Count >= 1)
            {
                foreach (var item in Points)
                {
                    Rectangle currentRect = new Rectangle(new Point(item.X - 2, item.Y - 2), new Size(4, 4));
                    e.Graphics.DrawEllipse(new Pen(Color.Black, 2), currentRect);
                }
                if (Points.Count >= 2)
                    e.Graphics.DrawLines(new Pen(Color.Black, 2), Points.ToArray());
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Points.Add(e.Location);
            this.Invalidate();
        }

    }
    public class ClockHandPointsEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Form form = new Form();
            form.ClientSize = new Size(500, 500);
            form.ShowInTaskbar = false;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ClientSize = new Size(500, 500);
            form.Text = context.PropertyDescriptor.DisplayName;

            IWindowsFormsEditorService editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            ClockHandPointsSelector selector = new ClockHandPointsSelector(form);
            form.Controls.Add(selector);
            editorService.ShowDialog(form);
            return selector.Points;
        }
    }
}

