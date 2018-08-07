using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace BattleShips
{
    class PaintHelper
    {
        public void DrawField(PaintEventArgs e, Cell[,] field, Size size, FieldType type, int fieldLength, int cellSize)
        {
            DrawCells(e, field, type, cellSize);
            DrawLines(e, size, cellSize, fieldLength);

        }

        public void HighlightCell(PaintEventArgs e, Rectangle rectangle)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.MediumVioletRed), rectangle);
        }
        private void DrawCells(PaintEventArgs e, Cell[,] field, FieldType type, int cellSize)
        {
            if (type == FieldType.AIOpponent)
            {
                foreach (var item in field)
                {
                    if (!item.IsClicked)
                        e.Graphics.FillRectangle(new SolidBrush(Color.LightGray), item.X * cellSize, item.Y * cellSize, cellSize, cellSize);
                    else
                    {
                        Brush brush = new SolidBrush(item.Color);
                        e.Graphics.FillRectangle(brush, item.X * cellSize, item.Y * cellSize, cellSize, cellSize);
                    }
                }
            }
            else
            {
                foreach (var item in field)
                {
                    if (item.CellType == CellType.Ship)
                        e.Graphics.FillRectangle(new SolidBrush(Color.LightCoral), item.X * cellSize, item.Y * cellSize, cellSize, cellSize);
                    else if (!item.IsClicked)
                        e.Graphics.FillRectangle(new SolidBrush(Color.LightGray), item.X * cellSize, item.Y * cellSize, cellSize, cellSize);
                    else
                    {
                        Brush brush = new SolidBrush(item.Color);
                        e.Graphics.FillRectangle(brush, item.X * cellSize, item.Y * cellSize, cellSize, cellSize);
                    }
                }
            }
        }
        private void DrawLines(PaintEventArgs e, Size size, int cellSize, int fieldLength)
        {
            for (int i = 0; i <= fieldLength; i++)
            {
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(0, i * cellSize), new Point(size.Height, i * cellSize));
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(i * cellSize, 0), new Point(i * cellSize, size.Height));
            }
        }
    }
}

