using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BattleShips
{
    class Ship
    {
        public Ship(Cell[,] field, int fieldLength, int shipLength, Random random)
        {
            Random = random;
            _ShipCells = new List<Cell>();
            _Field = field;
            _FieldLength = fieldLength;
            CreateShip(shipLength);
        }

        private Random _Random;
        private bool _IsDestroyed;
        private List<Cell> _ShipCells;
        private Cell[,] _Field;
        private int _FieldLength;

        public bool IsDestroyed
        {
            get { return _IsDestroyed; }
            set { _IsDestroyed = value; }
        }
        public List<Cell> ShipCells
        {
            get { return _ShipCells; }
            set
            {
                IsDestroyed = CheckIfEveryShipCellIsDestroyed();
                _ShipCells = value;
            }
        }
        public Random Random
        {
            get { return _Random; }
            set { _Random = value; }
        }

        private void CreateShip(int shipLength)
        {
            while (true)
            {
                Cell initialCell = _Field[Random.Next(_FieldLength), Random.Next(_FieldLength)];
                if (initialCell.CellType != CellType.Ship && initialCell.CellType != CellType.EmptyCellNearShip)
                {
                    if (ChooseDirectionAndAddShip(initialCell, shipLength) == true)
                        break;
                }
            }
        }

        private bool ChooseDirectionAndAddShip(Cell initialCell, int lengthOfShip)
        {
            bool alreadyFindSpot = false;
            if (initialCell.Y + lengthOfShip < _FieldLength)
                alreadyFindSpot = ShipCheckVertical(initialCell, initialCell.Y + lengthOfShip, 1);
            if (initialCell.X - lengthOfShip >= 0 && !alreadyFindSpot)
                alreadyFindSpot = ShipCheckHorizontal(initialCell, initialCell.X - lengthOfShip, -1);
            if (initialCell.Y - lengthOfShip >= 0 && !alreadyFindSpot)
                alreadyFindSpot = ShipCheckVertical(initialCell, initialCell.Y - lengthOfShip, -1);
            if (initialCell.X + lengthOfShip < _FieldLength && !alreadyFindSpot)
                alreadyFindSpot = ShipCheckHorizontal(initialCell, initialCell.X + lengthOfShip, 1);
            if (alreadyFindSpot)
            {
                AddShipToField();
            }
            return alreadyFindSpot;
        }

        private bool ShipCheckVertical(Cell initialCell, int lengthOfShip, int step)
        {
            for (int i = initialCell.Y + step; i != lengthOfShip; i += step)
            {
                if (_Field[i, initialCell.X].CellType == CellType.Ship || _Field[i, initialCell.X].CellType == CellType.EmptyCellNearShip)
                {
                    ShipCells.Clear();
                    return false;
                }
                ShipCells.Add(_Field[i, initialCell.X]);
            }
            ShipCells.Add(initialCell);
            return true;
        }
        private bool ShipCheckHorizontal(Cell initialCell, int lengthOfShip, int step)
        {
            for (int i = initialCell.X + step; i != lengthOfShip; i += step)
            {
                if (_Field[initialCell.Y, i].CellType == CellType.Ship || _Field[initialCell.Y, i].CellType == CellType.EmptyCellNearShip)
                {
                    ShipCells.Clear();
                    return false;
                }
                ShipCells.Add(_Field[initialCell.Y, i]);
            }
            ShipCells.Add(initialCell);
            return true;
        }
        private void AddShipToField()
        {
            foreach (var item in ShipCells)
            {
                _Field[item.Y, item.X].CellType = CellType.Ship;
                MarkEveryCellAround(item.Y, item.X);
            }
        }
        private void MarkEveryCellAround(int y, int x)
        {
            if (y > 0)
            {
                if (x > 0 && _Field[y - 1, x - 1].CellType != CellType.Ship)
                    _Field[y - 1, x - 1].CellType = CellType.EmptyCellNearShip;
                if (x < _FieldLength - 1 && _Field[y - 1, x + 1].CellType != CellType.Ship)
                    _Field[y - 1, x + 1].CellType = CellType.EmptyCellNearShip;
                if (_Field[y - 1, x].CellType != CellType.Ship)
                    _Field[y - 1, x].CellType = CellType.EmptyCellNearShip;
            }

            if (y < _FieldLength - 1)
            {
                if (x > 0 && _Field[y + 1, x - 1].CellType != CellType.Ship)
                    _Field[y + 1, x - 1].CellType = CellType.EmptyCellNearShip;
                if (x < _FieldLength - 1 && _Field[y + 1, x + 1].CellType != CellType.Ship)
                    _Field[y + 1, x + 1].CellType = CellType.EmptyCellNearShip;
                if (_Field[y + 1, x].CellType != CellType.Ship)
                    _Field[y + 1, x].CellType = CellType.EmptyCellNearShip;
            }

            if (x > 0 && _Field[y, x - 1].CellType != CellType.Ship)
                _Field[y, x - 1].CellType = CellType.EmptyCellNearShip;
            if (x < _FieldLength - 1 && _Field[y, x + 1].CellType != CellType.Ship)
                _Field[y, x + 1].CellType = CellType.EmptyCellNearShip;
        }

        public bool CheckIfEveryShipCellIsDestroyed()
        {
            foreach (var item in _ShipCells)
            {
                if (item.CellType != CellType.DestroyedShip)
                    return false;
            }
            return true;
        }       
        private void OpenEveryCellAroundDestroyedCell(ref int unclickedCells, int x, int y)
        {
            if (y > 0)
            {
                if (x > 0 && _Field[y - 1, x - 1].CellType != CellType.DestroyedShip && !_Field[y - 1, x - 1].IsClicked)
                {
                    unclickedCells--;
                    _Field[y - 1, x - 1].IsClicked = true;
                    _Field[y - 1, x - 1].CellType = CellType.EmptyAttackedCell;
                }
                if (x < _FieldLength - 1 && _Field[y - 1, x + 1].CellType != CellType.DestroyedShip && !_Field[y - 1, x + 1].IsClicked)
                {
                    unclickedCells--;
                    _Field[y - 1, x + 1].IsClicked = true;
                    _Field[y - 1, x + 1].CellType = CellType.EmptyAttackedCell;
                }
                if (_Field[y - 1, x].CellType != CellType.DestroyedShip && !_Field[y - 1, x].IsClicked)
                {
                    unclickedCells--;
                    _Field[y - 1, x].IsClicked = true;
                    _Field[y - 1, x].CellType = CellType.EmptyAttackedCell;
                }
            }
            if (y < _FieldLength - 1)
            {
                if (x > 0 && _Field[y + 1, x - 1].CellType != CellType.DestroyedShip && !_Field[y + 1, x - 1].IsClicked)
                {
                    unclickedCells--;
                    _Field[y + 1, x - 1].IsClicked = true;
                    _Field[y + 1, x - 1].CellType = CellType.EmptyAttackedCell;
                }
                if (x < _FieldLength - 1 && _Field[y + 1, x + 1].CellType != CellType.DestroyedShip && !_Field[y + 1, x + 1].IsClicked)
                {
                    unclickedCells--;
                    _Field[y + 1, x + 1].IsClicked = true;
                    _Field[y + 1, x + 1].CellType = CellType.EmptyAttackedCell;
                }
                if (_Field[y + 1, x].CellType != CellType.DestroyedShip && !_Field[y + 1, x].IsClicked)
                {
                    unclickedCells--;
                    _Field[y + 1, x].IsClicked = true;
                    _Field[y + 1, x].CellType = CellType.EmptyAttackedCell;
                }
            }
            if (x > 0 && _Field[y, x - 1].CellType != CellType.DestroyedShip && !_Field[y, x - 1].IsClicked)
            {
                unclickedCells--;
                _Field[y, x - 1].IsClicked = true;
                _Field[y, x - 1].CellType = CellType.EmptyAttackedCell;
            }
            if (x < _FieldLength - 1 && _Field[y, x + 1].CellType != CellType.DestroyedShip && !_Field[y, x + 1].IsClicked)
            {
                unclickedCells--;
                _Field[y, x + 1].IsClicked = true;
                _Field[y, x + 1].CellType = CellType.EmptyAttackedCell;
            }
        }
        public void OpenEveryCellAroundDestroyedShip(ref int unclickedCells)
        {
            foreach (var cell in ShipCells)
            {
                OpenEveryCellAroundDestroyedCell(ref unclickedCells, cell.X, cell.Y);
            }
        }
    }
}
