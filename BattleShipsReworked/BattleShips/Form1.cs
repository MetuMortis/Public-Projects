using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShips
{
    partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            SetUpFields();
            this.Resize += Form1_Resize;
            this.Invalidate();
        }

        //fields
        private BSField _Enemy;
        private BSField _Player;
        private int _CellSize;

        //properties
        public BSField Enemy
        {
            get { return _Enemy; }
            set { _Enemy = value; }
        }
        public BSField Player
        {
            get { return _Player; }
            set { _Player = value; }
        }
        public int CellSize
        {
            get { return _CellSize; }
            set { _CellSize = value; }
        }

        //methods
        private void SetUpFields()
        {
            CellSize = this.ClientSize.Height / 10;
            Enemy = new BSField
            {
                Location = new Point(0, 0),
                CellSize = this.CellSize,
                Size = new Size(CellSize * 10 + 1, CellSize * 10 + 1),
                FieldType = FieldType.AIOpponent,
            };

            Player = new BSField
            {

                CellSize = Enemy.CellSize,
                Location = new Point(CellSize * 15, 0),
                Size = Enemy.Size,
                FieldType = FieldType.Player
            };

            Enemy.Opponent = Player;
            Player.Opponent = Enemy;
            this.Controls.Add(Enemy);
            this.Controls.Add(Player);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            CellSize = this.ClientSize.Height / 10;
            this.MinimumSize = new Size(CellSize * 25 + 15, 150);
            this.MaximumSize = new Size(CellSize * 25 + 15, 800);

            Enemy.Size = new Size(CellSize * 10 + 1, CellSize * 10 + 1);
            Enemy.CellSize = CellSize;
            Enemy.HighlightedRectangle = new Rectangle();

            Player.Size = Enemy.Size;
            Player.Location = new Point(CellSize * 15, 0);
            Player.CellSize = CellSize;

            Enemy.Invalidate();
            Player.Invalidate();
        }

    }
}
