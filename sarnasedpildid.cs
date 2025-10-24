using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KolmRakendust
{
    public class MatchingGame
    {
        private TableLayoutPanel tableLayoutPanel;
        private List<string> icons;
        private Label firstClicked = null;
        private Label secondClicked = null;
        private Random random = new Random();
        private Timer timer;
        private Control parentControl;
        private int gridSize;

        public MatchingGame(Control parent, int gridSize)
        {
            parentControl = parent;
            this.gridSize = gridSize;

            // Loome mänguvälja
            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.RowCount = gridSize;
            tableLayoutPanel.ColumnCount = gridSize;
            tableLayoutPanel.Size = new Size(250, 250);
            tableLayoutPanel.BackColor = Color.CornflowerBlue;
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            parentControl.Controls.Add(tableLayoutPanel);
            tableLayoutPanel.BringToFront();

            // Loo ikoonid
            GenerateIcons();

            // Ajastaja paaride ajutiseks näitamiseks
            timer = new Timer();
            timer.Interval = 750;
            timer.Tick += Timer_Tick;

            // Ridade ja veergude proportsioonid
            for (int i = 0; i < gridSize; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / gridSize));
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / gridSize));
            }

            AddIconsToSquares();
        }

        private void GenerateIcons()
        {
            icons = new List<string>();
            string possibleIcons = "!@#$%^&*()NZvbklopqwerty";

            int neededPairs = (gridSize * gridSize) / 2;
            for (int i = 0; i < neededPairs; i++)
            {
                string symbol = possibleIcons[random.Next(possibleIcons.Length)].ToString();
                icons.Add(symbol);
                icons.Add(symbol);
            }
        }

        private void AddIconsToSquares()
        {
            int fontSize = Math.Max(12, 64 / gridSize * 3);

            foreach (int row in Enumerable.Range(0, gridSize))
            {
                foreach (int col in Enumerable.Range(0, gridSize))
                {
                    Label label = new Label();
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Font = new Font("Webdings", fontSize, FontStyle.Bold);
                    label.ForeColor = tableLayoutPanel.BackColor;
                    label.Click += Label_Click;
                    tableLayoutPanel.Controls.Add(label, col, row);
                }
            }

            foreach (Control control in tableLayoutPanel.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (timer.Enabled) return;

            Label clickedLabel = sender as Label;
            if (clickedLabel == null) return;

            if (clickedLabel.ForeColor == Color.Black)
                return;

            if (firstClicked == null)
            {
                firstClicked = clickedLabel;
                firstClicked.ForeColor = Color.Black;
                return;
            }

            secondClicked = clickedLabel;
            secondClicked.ForeColor = Color.Black;

            CheckForWinner();

            if (firstClicked.Text == secondClicked.Text)
            {
                firstClicked = null;
                secondClicked = null;
            }
            else
            {
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            firstClicked.ForeColor = tableLayoutPanel.BackColor;
            secondClicked.ForeColor = tableLayoutPanel.BackColor;
            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null && iconLabel.ForeColor == tableLayoutPanel.BackColor)
                    return;
            }

            MessageBox.Show("Tubli! Leidsid kõik paarid!", "Mäng läbi");
        }

        public void Remove()
        {
            parentControl.Controls.Remove(tableLayoutPanel);
        }

        public void SetLocation(int x, int y)
        {
            tableLayoutPanel.Location = new Point(x + 20, y);
        }
    }
}
