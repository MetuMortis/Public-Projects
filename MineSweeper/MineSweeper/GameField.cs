using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace MineSweeper
{
    class GameField : Form
    {
        const string path = @"icons/";
        const int upperBarHeight = 50;
        const int elementLength = 30;

        public int Rows { get; set; }
        public int Columns { get; set; }
        public int NumberOfMines { get; set; }

        public MineCell[,] MineCells { get; set; }
        public Point[,] Points { get; set; }
        public Size MineCellSize { get; set; }
        public int FlagsRemaining { get; set; }
        public Label FlagsRemainingLabel { get; set; }
        public Button SmileGuy { get; set; }
        public Button Settings { get; set; }
        public Dictionary<int, Image> ImageLibrary { get; set; }
        public bool FirstClick { get; set; }
        public PictureBox PlayArea { get; set; }
        public int SafeMineCellsClicked { get; set; }
        public Form1 Window { get; set; }

        public GameField(int rows, int columns, int numberOfMines, Form1 window)
        {
            Window = window;
            CenterToScreen();
            SetVariables(rows, columns, numberOfMines);
            SetImageLibrary();
            AdjustForm();
            SetUpperBarControls();
            SetMineCells();
            Show();
        }

        private void SetVariables(int rows, int columns, int numberOfMines)
        {
            FirstClick = true;
            Rows = rows;
            Columns = columns;
            NumberOfMines = numberOfMines;
            FlagsRemaining = NumberOfMines;
            SafeMineCellsClicked = 0;
            MineCellSize = new Size(elementLength, elementLength);
        }

        private void SetImageLibrary()
        {
            ImageLibrary = new Dictionary<int, Image>();
            ImageLibrary.Add(0, Properties.Resources.button_pressed);
            ImageLibrary.Add(1, Properties.Resources.button_1);
            ImageLibrary.Add(2, Properties.Resources.button_2);
            ImageLibrary.Add(3, Properties.Resources.button_3);
            ImageLibrary.Add(4, Properties.Resources.button_4);
            ImageLibrary.Add(5, Properties.Resources.button_5);
            ImageLibrary.Add(6, Properties.Resources.button_6);
            ImageLibrary.Add(7, Properties.Resources.button_7);
            ImageLibrary.Add(8, Properties.Resources.button_8);
            ImageLibrary.Add(-1, Properties.Resources.button_unpressed);
            ImageLibrary.Add(11, Properties.Resources.button_marked);
            ImageLibrary.Add(13, Properties.Resources.button_mined_pressed);
            ImageLibrary.Add(12, Properties.Resources.button_mined);
            ImageLibrary.Add(14, Properties.Resources.button_mined_crossed);
            ImageLibrary.Add(100, Properties.Resources.smileguy_smile);
            ImageLibrary.Add(101, Properties.Resources.smileguy_surprised);
            ImageLibrary.Add(102, Properties.Resources.smileguy_sunglasses);
            ImageLibrary.Add(103, Properties.Resources.smileguy_dead);
            ImageLibrary.Add(104, Properties.Resources.smileguy_pressed);


        }

        private void AdjustForm()
        {
            Text = "MineSweeper";
            ClientSize = new Size(Columns * elementLength, Rows * elementLength + upperBarHeight);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackColor = Color.LightGray;
            DoubleBuffered = true;
            FormClosed += GameField_FormClosed;
        }
        private void GameField_FormClosed(object sender, FormClosedEventArgs e)
        {
            Window.Dispose();
        }

        private void SetUpperBarControls()
        {
            SmileGuy = new Button
            {
                Size = new Size(40, 40),
                Location = new Point(ClientSize.Width / 2 - 20, 5),
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                BackgroundImage = ImageLibrary[100]
            };
            SmileGuy.FlatAppearance.BorderSize = 0;
            SmileGuy.MouseUp += SmileGuy_MouseUp;
            SmileGuy.MouseDown += SmileGuy_MouseDown;
            Controls.Add(SmileGuy);

            FlagsRemainingLabel = new Label()
            {
                Size = new Size(100, 40),
                Location = new Point(ClientSize.Width - 105, 5),
                BackColor = Color.Black,
                ForeColor = Color.Red,
                Font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold),
                Text = FlagsRemaining.ToString(),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(FlagsRemainingLabel);

            Settings = new Button
            {
                Location = new Point(5, upperBarHeight / 2 - 15),
                Size = new Size(FlagsRemainingLabel.Size.Width, 30),
                Text = "Settings"
            };
            Settings.Click += Settings_Click;
            Controls.Add(Settings);


        }
        private void SmileGuy_MouseDown(object sender, MouseEventArgs e)
        {
            SmileGuy.BackgroundImage = ImageLibrary[104];
        }
        private void SmileGuy_MouseUp(object sender, EventArgs e)
        {
            SmileGuy.BackgroundImage = ImageLibrary[100];
            SetVariables(Rows, Columns, NumberOfMines);
            PlayArea.Dispose();
            SetMineCells();
            FlagsRemainingLabel.Text = FlagsRemaining.ToString();
            PlayArea.Invalidate();
        }
        private void Settings_Click(object sender, EventArgs e)
        {
            if (!Window.Visible)
                Window.Show();
            else
                Window.Focus();
        }

        private void SetMineCells()
        {
            SetMineCellsSizeAndLocation();
            AddButtonsAndEvents();
        }
        private void SetMineCellsSizeAndLocation()
        {
            MineCells = new MineCell[Rows, Columns];
            Points = new Point[Rows, Columns];
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    MineCells[y, x] = new MineCell();
                    MineCells[y, x].Picture = ImageLibrary[-1];
                    MineCells[y, x].Y = y;
                    MineCells[y, x].X = x;
                    Points[y, x] = new Point();
                    Points[y, x].Y = y * elementLength;
                    Points[y, x].X = x * elementLength;
                }
            }
        }

        private void AddButtonsAndEvents()
        {
            PlayArea = new PictureBox
            {
                BackColor = Color.Red,
                Size = new Size(ClientSize.Width, ClientSize.Height - upperBarHeight),
                Location = new Point(0, upperBarHeight)
            };
            Controls.Add(PlayArea);
            PlayArea.MouseUp += MineCells_MouseUp;
            PlayArea.MouseDown += MineCells_MouseDown;
            PlayArea.Paint += PlayArea_Paint;
        }

        private void PlayArea_Paint(object sender, PaintEventArgs e)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    e.Graphics.DrawImage(MineCells[y, x].Picture, Points[y, x]);
                }
            }
        }

        private void MineCells_MouseDown(object sender, MouseEventArgs e)
        {
            int y = e.Y / elementLength;
            int x = e.X / elementLength;

            if (!MineCells[y, x].IsClicked && e.Button == MouseButtons.Left && !MineCells[y, x].IsMarked)
            {
                MineCells[y, x].Picture = ImageLibrary[0];
                SmileGuy.BackgroundImage = ImageLibrary[101];
            }
            PlayArea.Invalidate();
        }

        private void MineCells_MouseUp(object sender, MouseEventArgs e)
        {
            int y = e.Y / elementLength;
            int x = e.X / elementLength;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    OnLeftMouseClick(y, x);
                    break;
                case MouseButtons.Right:
                    OnRightMouseClick(y, x);
                    break;
                default:
                    break;
            };
        }
        private void OnLeftMouseClick(int y, int x)
        {
            if (FirstClick)
            {
                SetMinesToMineCells(y, x);
                SetNumbersToAllMineCells();
                FirstClick = false;
            }
            if (!MineCells[y, x].IsClicked && !MineCells[y, x].IsMarked)
            {
                SmileGuy.BackgroundImage = ImageLibrary[100];
                if (MineCells[y, x].IsMined)
                {
                    EndGame(false);
                    MineCells[y, x].Picture = ImageLibrary[13];
                }
                else
                {
                    MineCells[y, x].IsClicked = true;
                    MineCells[y, x].Picture = ImageLibrary[MineCells[y, x].MinesNearby];
                    if (MineCells[y, x].IsMarked)
                    {
                        MineCells[y, x].IsMarked = false;
                        FlagsRemaining++;
                        FlagsRemainingLabel.Text = FlagsRemaining.ToString();
                    }
                    if (MineCells[y, x].MinesNearby == 0)
                        ClickEveryMineCellAround(y, x);
                    SafeMineCellsClicked++;
                    if (SafeMineCellsClicked == Columns * Rows - NumberOfMines)
                        EndGame(true);
                }
            }
            PlayArea.Invalidate();
        }
        private void SetMinesToMineCells(int firstCellY, int firstCellX)
        {
            int minesPlaced = 0;
            Random rnd = new Random();
            int counter = Columns * Rows / NumberOfMines * 2;
            while (minesPlaced < NumberOfMines)
            {
                for (int y = 0; y < Rows; y++)
                {
                    for (int x = 0; x < Columns; x++)
                    {
                        if (!MineCells[y, x].IsMined && minesPlaced < NumberOfMines && MineCells[y, x] != MineCells[firstCellY, firstCellX])
                        {
                            if (rnd.Next(counter) == 0)
                            {
                                MineCells[y, x].IsMined = true;
                                minesPlaced++;
                                if (minesPlaced == NumberOfMines)
                                    return;
                                counter = Columns * Rows / NumberOfMines * 2;
                            }
                            else
                                counter = (counter-- == 0) ? counter : counter--;
                        }
                    }
                }
            }
        }

        private void SetNumbersToAllMineCells()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    MineCells[y, x].MinesNearby = SetNumberToMineCell(y, x);
                }
            }
        }
        private int SetNumberToMineCell(int y, int x)
        {
            if (MineCells[y, x].IsMined)
                return -1;
            int numberOfMinesNearby = 0;
            if (y > 0)
            {
                if (x > 0 && MineCells[y - 1, x - 1].IsMined)
                    numberOfMinesNearby++;
                if (MineCells[y - 1, x].IsMined)
                    numberOfMinesNearby++;
                if (x < Columns - 1 && MineCells[y - 1, x + 1].IsMined)
                    numberOfMinesNearby++;
            }
            if (y < Rows - 1)
            {
                if (x > 0 && MineCells[y + 1, x - 1].IsMined)
                    numberOfMinesNearby++;
                if (MineCells[y + 1, x].IsMined)
                    numberOfMinesNearby++;
                if (x < Columns - 1 && MineCells[y + 1, x + 1].IsMined)
                    numberOfMinesNearby++;
            }
            if (x > 0 && MineCells[y, x - 1].IsMined)
                numberOfMinesNearby++;
            if (x < Columns - 1 && MineCells[y, x + 1].IsMined)
                numberOfMinesNearby++;
            return numberOfMinesNearby;
        }

        private void ClickEveryMineCellAround(int y, int x)
        {
            if (y > 0)
            {
                if (x > 0 && !MineCells[y - 1, x - 1].IsMarked)
                    OnLeftMouseClick(y - 1, x - 1);
                if (x < Columns - 1 && !MineCells[y - 1, x + 1].IsMarked)
                    OnLeftMouseClick(y - 1, x + 1);
                if (!MineCells[y - 1, x].IsMarked)
                    OnLeftMouseClick(y - 1, x);
            }

            if (y < Rows - 1)
            {
                if (x > 0 && !MineCells[y + 1, x - 1].IsMarked)
                    OnLeftMouseClick(y + 1, x - 1);
                if (x < Columns - 1 && !MineCells[y + 1, x + 1].IsMarked)
                    OnLeftMouseClick(y + 1, x + 1);
                if (!MineCells[y + 1, x].IsMarked)
                    OnLeftMouseClick(y + 1, x);
            }

            if (x > 0 && !MineCells[y, x - 1].IsMarked)
                OnLeftMouseClick(y, x - 1);
            if (x < Columns - 1 && !MineCells[y, x + 1].IsMarked)
                OnLeftMouseClick(y, x + 1);
        }

        private void OnRightMouseClick(int y, int x)
        {
            if (MineCells[y, x].IsMarked)
            {
                MineCells[y, x].IsMarked = false;
                MineCells[y, x].Picture = ImageLibrary[-1];
                FlagsRemaining++;
                FlagsRemainingLabel.Text = FlagsRemaining.ToString();
            }
            else if (!MineCells[y, x].IsClicked)
            {
                MineCells[y, x].IsMarked = true;
                MineCells[y, x].Picture = ImageLibrary[11];
                FlagsRemaining--;
                FlagsRemainingLabel.Text = FlagsRemaining.ToString();
            }
            PlayArea.Invalidate(new Rectangle(Points[y, x], MineCellSize));
        }

        private void EndGame(bool isWon)
        {
            ShowAllMines(isWon);
            SmileGuy.BackgroundImage = (isWon) ? ImageLibrary[102] : ImageLibrary[103];
            if (isWon)
            {
                FlagsRemaining = 0;
                FlagsRemainingLabel.Text = FlagsRemaining.ToString();
            }
            PlayArea.MouseUp -= MineCells_MouseUp;
            PlayArea.MouseDown -= MineCells_MouseDown;
            PlayArea.Invalidate();
        }
        private void ShowAllMines(bool isWon)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    if (MineCells[y, x].IsMined && !MineCells[y, x].IsMarked)
                        MineCells[y, x].Picture = (isWon) ? ImageLibrary[11] : ImageLibrary[12];
                    if (!MineCells[y, x].IsMined && MineCells[y, x].IsMarked)
                        MineCells[y, x].Picture = ImageLibrary[14];
                }
            }

        }
    }
}

