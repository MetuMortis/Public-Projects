using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleShips
{
    class Cell
    {
        private CellType _CellType;
        public CellType CellType
        {
            get { return _CellType; }
            set
            {
                this.Color = SetColor(value);
                _CellType = value;
            }
        }

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsClicked { get; set; }
        public Color Color { get; set; }
        private Color SetColor(CellType type)
        {
            if (type == CellType.DestroyedShip)
                return Color.Red;
            else
                return Color.LightCyan;

        }
    }
}

