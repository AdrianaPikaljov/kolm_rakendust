using System;
using System.Drawing;
using System.Windows.Forms;

namespace KolmRakendust
{
    public partial class Form1 : Form
    {
        Button btnMathQuiz;
        Button btnPildiVaatamine;
        Button btnMatchingGame;
        Label lbl;

        private pildiVaatamise pildiVaataja;
        private MathQuiz mathQuiz;
        private MatchingGame matchingGame;

        public Form1()
        {
            InitializeComponent();

            this.Height = 800;
            this.Width = 1000;
            this.Text = "Kolm rakendus";

            // Initialize buttons
            btnPildiVaatamine = new Button();
            btnPildiVaatamine.Text = "Pildi vaatamise programm";
            btnPildiVaatamine.Location = new Point(150, 100);
            btnPildiVaatamine.Height = 50;
            btnPildiVaatamine.Width = 300;
            btnPildiVaatamine.Click += BtnPildiVaatamine_Click;

            btnMathQuiz = new Button();
            btnMathQuiz.Text = "Matemaatiline test";
            btnMathQuiz.Location = new Point(150, 200);
            btnMathQuiz.Height = 50;
            btnMathQuiz.Width = 300;
            btnMathQuiz.Click += BtnMathQuiz_Click;

            btnMatchingGame = new Button();
            btnMatchingGame.Text = "Sarnaste piltide leidmise mäng";
            btnMatchingGame.Location = new Point(150, 300);
            btnMatchingGame.Height = 50;
            btnMatchingGame.Width = 300;
            btnMatchingGame.Click += BtnMatchingGame_Click;

            // Initialize label
            lbl = new Label();
            lbl.Text = "KOLM RAKENDUST";
            lbl.Font = new Font("Arial", 24);
            lbl.Size = new Size(400, 30);
            lbl.Location = new Point(150, 0);
            lbl.MouseHover += Lbl_MouseHover;
            lbl.MouseLeave += Lbl_MouseLeave;

            // Add controls to the form
            this.Controls.Add(btnPildiVaatamine);
            this.Controls.Add(btnMathQuiz);
            this.Controls.Add(btnMatchingGame);
            this.Controls.Add(lbl);

            // Initialize the applications
            pildiVaataja = new pildiVaatamise(this);
            pildiVaataja.Hide();
            mathQuiz = new MathQuiz(this);
            mathQuiz.Hide();
            matchingGame = new MatchingGame(this);
            matchingGame.Hide();
        }

        private void BtnPildiVaatamine_Click(object sender, EventArgs e)
        {
            HideUIElements(); // Hide the buttons and label
            pildiVaataja.Show();
            mathQuiz.Hide();
            matchingGame.Hide();
        }

        private void BtnMathQuiz_Click(object sender, EventArgs e)
        {
            HideUIElements(); // Hide the buttons and label
            mathQuiz.Show();
            matchingGame.Hide();
            pildiVaataja.Hide();
        }

        private void BtnMatchingGame_Click(object sender, EventArgs e)
        {
            HideUIElements(); // Hide the buttons and label
            matchingGame.Show();
            pildiVaataja.Hide();
            mathQuiz.Hide();
        }

        // Method to hide buttons and label
        private void HideUIElements()
        {
            btnPildiVaatamine.Hide();
            btnMathQuiz.Hide();
            btnMatchingGame.Hide();
            lbl.Hide();
        }

        // Method to show buttons and label again
        public void ShowUIElements()
        {
            btnPildiVaatamine.Show();
            btnMathQuiz.Show();
            btnMatchingGame.Show();
            lbl.Show();
            pildiVaataja.Hide();
            mathQuiz.Hide();
            matchingGame.Hide();
        }

        private void Lbl_MouseHover(object sender, EventArgs e)
        {
            lbl.BackColor = Color.FromArgb(200, 10, 20);
        }

        private void Lbl_MouseLeave(object sender, EventArgs e)
        {
            lbl.BackColor = Color.Transparent;
        }
    }
}
