using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MineSweeper
{
    class StartScreen
    {
        const int distanceAndSize = 20;
        public Label UpperBar { get; set; }
        public RadioButton Easy { get; set; }
        public RadioButton Medium { get; set; }
        public RadioButton Hard { get; set; }
        public RadioButton Custom { get; set; }
        public Button NewGameButton { get; set; }
        public Form1 Window { get; set; }
        public int NumberOfMines { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public NumericUpDown NumericRows { get; set; }
        public NumericUpDown NumericColumns { get; set; }
        public NumericUpDown NumericMines { get; set; }
        public GameField Start { get; set; }

        public StartScreen(Form1 form)
        {
            Window = form;
            AdjustForm();
            InitializeControls();
        }

        private void AdjustForm()
        {
            Window.Size = new Size(250, 215);
            Window.Text = "Minesweeper";
            Window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Window.FormClosing += Window_FormClosing;
        }
        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Start != null)
            {
                e.Cancel = true;
                Window.Hide();
            }
        }

        private void InitializeControls()
        {
            UpperBar = new Label
            {
                Size = new Size(Window.ClientSize.Width - 79, distanceAndSize),
                Location = new Point(79, 5),
                Text = "Rows   Columns   Mines",
                ForeColor = Color.Black
            };
            Window.Controls.Add(UpperBar);

            Easy = new RadioButton
            {
                Size = new Size(Window.ClientSize.Width - 5, distanceAndSize),
                Location = new Point(5, distanceAndSize),
                Text = "Easy:           9          9              10",
                Checked = true
            };
            Window.Controls.Add(Easy);

            Medium = new RadioButton
            {
                Size = Easy.Size,
                Location = new Point(5, Easy.Top + distanceAndSize),
                Text = "Medium:      16        16            40"
            };
            Window.Controls.Add(Medium);

            Hard = new RadioButton
            {
                Size = Easy.Size,
                Location = new Point(5, Medium.Top + distanceAndSize),
                Text = "Hard:           16        30            99"
            };
            Window.Controls.Add(Hard);

            Custom = new RadioButton
            {
                Size = new Size(distanceAndSize * 3, distanceAndSize),
                Location = new Point(5, Hard.Top + distanceAndSize),
                Text = "Custom:"
            };
            Window.Controls.Add(Custom);




            NumericRows = new NumericUpDown
            {
                Location = new Point(Custom.Left + Custom.Size.Width + distanceAndSize - 2, Custom.Top),
                Size = new Size((int)(distanceAndSize * 1.6), distanceAndSize),
                Minimum = 2,
                Maximum = 30
            };
            NumericRows.ValueChanged += Numerics_ValueChanged;
            Window.Controls.Add(NumericRows);

            NumericColumns = new NumericUpDown
            {
                Location = new Point(NumericRows.Left + NumericRows.Width + 5, NumericRows.Top),
                Size = NumericRows.Size,
                Minimum = 8,
                Maximum = 60
            };
            NumericColumns.ValueChanged += Numerics_ValueChanged;
            Window.Controls.Add(NumericColumns);

            NumericMines = new NumericUpDown
            {
                Location = new Point(NumericColumns.Left + NumericColumns.Width + 14, NumericColumns.Top),
                Size = new Size((int)(distanceAndSize * 2.2), distanceAndSize),
                Minimum = 5,
                Maximum = NumericRows.Value * NumericColumns.Value - 1
            };

            NumericMines.ValueChanged += Numerics_ValueChanged;
            Window.Controls.Add(NumericMines);

            NewGameButton = new Button
            {
                Size = new Size(distanceAndSize * 4, distanceAndSize * 2),
                Location = new Point(Window.ClientSize.Width / 2 - distanceAndSize * 2, Window.ClientSize.Height - distanceAndSize * 3),
                Text = "New Game"
            };
            NewGameButton.Click += NewGameButton_Click;
            Window.Controls.Add(NewGameButton);
        }
        private void Numerics_ValueChanged(object sender, EventArgs e)
        {
            if (sender != NumericMines)
                NumericMines.Maximum = NumericRows.Value * NumericColumns.Value - 1;
            Custom.Checked = true;
        }      
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            if (Start != null)
                Start.Dispose();
            if (Easy.Checked)
            {
                Rows = 9;
                Columns = 9;
                NumberOfMines = 10;
            }
            else if (Medium.Checked)
            {
                Rows = 16;
                Columns = 16;
                NumberOfMines = 40;
            }
            else if (Hard.Checked)
            {
                Rows = 16;
                Columns = 30;
                NumberOfMines = 99;
            }
            else if (Custom.Checked)
            {
                Rows = (int)NumericRows.Value;
                Columns = (int)NumericColumns.Value;
                NumberOfMines = (int)NumericMines.Value;
            }
            Start = new GameField(Rows, Columns, NumberOfMines, Window);
            Window.Hide();
        }
    }
}
