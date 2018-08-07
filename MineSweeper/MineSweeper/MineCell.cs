using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MineSweeper
{
    class MineCell
    {
        Size sizes = new Size(30, 30);
        public bool IsMined { get; set; }
        public bool IsMarked { get; set; }
        public int MinesNearby { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        public bool IsClicked { get; set; }
        Image pic;
        public Image Picture { get { return pic; } set { pic = resizeImage(value, sizes); } }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
    }
}
