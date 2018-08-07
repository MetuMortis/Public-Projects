using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BattleShips
{
    class BSField : Control
    {
        public BSField()
        {
            this.DoubleBuffered = true;
            Random = new Random();
            _Field = CreateField();
            _Ships = new List<Ship>();
            _AttackedShip = new List<Cell>();
            _PaintHelper = new PaintHelper();
            _UnclickedCells = FieldLength * FieldLength;
            AddShips();
            this.MouseClick += BSField_MouseClick;
        }

        const int fieldLength = 10;
        const int maxLength = 4;
        List<Cell> _AttackedShip;
        int _CellSize;
        FieldType _FieldType;
        Cell[,] _Field;
        BSField _Opponent;
        List<Ship> _Ships;
        PaintHelper _PaintHelper;
        int _UnclickedCells;
        Random _Random;

        Rectangle _LastHighlightedRectangle;
        Rectangle _HighLightedRectangle;
        public Random Random
        {
            get { return _Random; }
            set { _Random = value; }
        }
        public Rectangle HighlightedRectangle
        {
            get { return _HighLightedRectangle; }
            set
            {
                _LastHighlightedRectangle = _HighLightedRectangle;
                _HighLightedRectangle = value;
                Invalidate(_HighLightedRectangle);
                Invalidate(_LastHighlightedRectangle);
            }
        }
        public Cell[,] Field
        {
            get { return _Field; }
            set { _Field = value; }
        }
        public BSField Opponent
        {
            get { return _Opponent; }
            set { _Opponent = value; }
        }
        public FieldType FieldType
        {
            get { return _FieldType; }
            set { _FieldType = value; }
        }
        public List<Ship> Ships
        {
            get { return _Ships; }
            set { _Ships = value; }
        }
        public int CellSize
        {
            get { return _CellSize; }
            set { _CellSize = value; }
        }
        public List<Cell> AttackedShip
        {
            get { return _AttackedShip; }
            set { _AttackedShip = value; }
        }
        public int UnclickedCells
        {
            get { return _UnclickedCells; }
            set { _UnclickedCells = value; }
        }
        public int FieldLength
        {
            get { return fieldLength; }
        }
        public PaintHelper PaintHelper
        {
            get { return _PaintHelper; }
        }

        private Cell[,] CreateField()
        {

            Cell[,] cells = new Cell[fieldLength, fieldLength];
            for (int y = 0; y < fieldLength; y++)
            {
                for (int x = 0; x < fieldLength; x++)
                {
                    cells[y, x] = new Cell
                    {
                        Y = y,
                        X = x,
                        CellType = CellType.EmptyCell
                    };
                }
            }
            return cells;

        }

        private void AddShips()
        {
            int quantity = 0;
            for (int i = maxLength; i > 0; i--)
            {
                quantity++;
                AddShipCategory(i, quantity);
            }
        }
        private void AddShipCategory(int lengthOfShip, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Ships.Add(new Ship(Field, FieldLength, lengthOfShip, Random));
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintHelper.DrawField(e, this.Field, this.Size, this.FieldType, this.FieldLength, this.CellSize);
            PaintHelper.HighlightCell(e, HighlightedRectangle);



        }



        private void BSField_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.FieldType == FieldType.AIOpponent)
            {
                if (Opponent.Ships.Count == 0)
                {
                    this.MouseClick -= BSField_MouseClick;
                    return;
                }
                Cell currentCell = Field[e.Location.Y / CellSize, e.Location.X / CellSize];
                if (!currentCell.IsClicked)
                {
                    Attack(currentCell);
                }
            }
        }
        private void Attack(Cell currentCell)
        {
            if (currentCell.CellType == CellType.Ship)
            {
                currentCell.CellType = CellType.DestroyedShip;
                CheckIfAnyShipIsDestroyed(Ships);
            }
            currentCell.IsClicked = true;
            UnclickedCells--;
            this.Invalidate();
            if (this.FieldType == FieldType.AIOpponent && currentCell.CellType != CellType.DestroyedShip)
                Opponent.AIAttack();


        }
        private void CheckIfAnyShipIsDestroyed(List<Ship> ships)
        {
            for (int i = Ships.Count - 1; i >= 0; i--)
            {
                ships[i].IsDestroyed = ships[i].CheckIfEveryShipCellIsDestroyed();
                if (Ships[i].IsDestroyed)
                {
                    Ships[i].OpenEveryCellAroundDestroyedShip(ref _UnclickedCells);
                    ships.Remove(ships[i]);
                }
            }
            if (Ships.Count == 0)
            {
                string message = (this.FieldType == FieldType.AIOpponent) ? "You won!" : "You lost!";
                MessageBox.Show(message);
                this.MouseClick -= BSField_MouseClick;

            }
        }

        private void AIAttack()
        {
            while (true)
            {
                int shipsBeforeAttack = Ships.Count;
                Cell currentCell;
                if (AttackedShip.Count == 0)
                    currentCell = (UnclickedCells < FieldLength) ? ReturnCellIfFewUnclickedCellsRemain() : ReturnRandomCell();
                else
                    currentCell = FindNextTarget(AttackedShip);

                if (!currentCell.IsClicked)
                {
                    Attack(currentCell);
                    if (currentCell.CellType != CellType.DestroyedShip)
                        break;
                    else
                    {
                        AttackedShip.Add(currentCell);
                    }
                }
                if (shipsBeforeAttack != Ships.Count)
                {
                    AttackedShip.Clear();
                    if (Ships.Count == 0)
                        return;
                }
            }

        }
        private Cell FindNextTarget(List<Cell> ship)
        {
            List<int> number = new List<int>();
            int max;
            int min;
            if (ship.Count == 1)
            {
                return FindNextTargetIfOnlyOneCellOfShipDamaged(ship[0]);
            }
            else if (ship[0].X == ship[1].X)
            {
                foreach (var item in AttackedShip)
                {
                    number.Add(item.Y);
                }
                max = number.Max();
                min = number.Min();
                number.Clear();
                if (min - 1 >= 0 && !Field[min - 1, ship[0].X].IsClicked)
                    return Field[min - 1, ship[0].X];
                else return Field[max + 1, ship[0].X];
            }
            else
            {
                foreach (var item in ship)
                {
                    number.Add(item.X);
                }
                max = number.Max();
                min = number.Min();
                number.Clear();
                if (min - 1 >= 0 && !Field[ship[0].Y, min - 1].IsClicked)
                    return Field[ship[0].Y, min - 1];
                else return Field[ship[0].Y, max + 1];
            }
        }
        private Cell FindNextTargetIfOnlyOneCellOfShipDamaged(Cell cell)
        {
            if (cell.X > 0 && !Field[cell.Y, cell.X - 1].IsClicked)
                return Field[cell.Y, cell.X - 1];
            else if (cell.X + 1 < fieldLength && !Field[cell.Y, cell.X + 1].IsClicked)
                return Field[cell.Y, cell.X + 1];
            else if (cell.Y > 0 && !Field[cell.Y - 1, cell.X].IsClicked)
                return Field[cell.Y - 1, cell.X];
            else
                return Field[cell.Y + 1, cell.X];
        }
        
        private Cell ReturnCellIfFewUnclickedCellsRemain()
        {
            foreach (var item in Field)
            {
                if (!item.IsClicked)
                    return item;
            }
            return null;
        }
        private Cell ReturnRandomCell()
        {
            Random rnd = new Random();
            return Field[rnd.Next(fieldLength), rnd.Next(fieldLength)];
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.FieldType == FieldType.AIOpponent)
            {
                Rectangle newRectangle = new Rectangle(new Point(e.X - e.X % CellSize + 1, e.Y - e.Y % CellSize + 1), new Size(CellSize - 1, CellSize - 1));
                if (HighlightedRectangle != newRectangle)
                    HighlightedRectangle = newRectangle;
            }
        }




    }
}

