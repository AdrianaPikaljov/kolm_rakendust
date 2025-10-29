using System;
using System.Drawing;
using System.Windows.Forms;

namespace KolmRakendust
{
    public class MathQuiz
    {
        private Button startButton;
        private Button submitButton;
        private Button endQuizButton;
        private Label lblQuestion1, lblQuestion2, lblQuestion3, lblQuestion4, lblTimeLeft, lblResult;
        private NumericUpDown numAnswer1, numAnswer2, numAnswer3, numAnswer4;
        private Timer timer;
        private int timeLeft;
        private Control parentControl;

        private string[] questions = {
            "26 + 34 =", "47 - 26 =", "3 × 3 =", "64 ÷ 8 =", "12 + 18 =", "50 - 19 =", "8 × 7 =", "72 ÷ 9 ="
        };

        private int[] answers = { 26 + 34, 47 - 26, 3 * 3, 64 / 8, 12 + 18, 50 - 19, 8 * 7, 72 / 9 };

        private Random rand;

        public MathQuiz(Control parent)
        {
            parentControl = parent;
            rand = new Random();

            // Alusta ajaloenduri, nuppude ja küsimuste loomist
            startButton = new Button();
            startButton.Text = "Alusta viktoriini";
            startButton.Location = new Point(150, 10);
            startButton.Size = new Size(120, 30);
            startButton.Click += StartButton_Click;
            SetButtonStyle(startButton);

            submitButton = new Button();
            submitButton.Text = "Esita vastused";
            submitButton.Location = new Point(280, 10);
            submitButton.Size = new Size(120, 30);
            submitButton.Click += SubmitButton_Click;
            submitButton.Enabled = false;
            SetButtonStyle(submitButton);

            endQuizButton = new Button();
            endQuizButton.Text = "Lõpeta test";
            endQuizButton.Location = new Point(410, 10);
            endQuizButton.Size = new Size(120, 30);
            endQuizButton.Click += EndQuizButton_Click;
            SetButtonStyle(endQuizButton);

            // Küsimused
            lblQuestion1 = new Label() { Text = "", Location = new Point(150, 50), AutoSize = true, Font = new Font("Arial", 12) };
            lblQuestion2 = new Label() { Text = "", Location = new Point(150, 90), AutoSize = true, Font = new Font("Arial", 12) };
            lblQuestion3 = new Label() { Text = "", Location = new Point(150, 130), AutoSize = true, Font = new Font("Arial", 12) };
            lblQuestion4 = new Label() { Text = "", Location = new Point(150, 170), AutoSize = true, Font = new Font("Arial", 12) };

            // Vastused
            numAnswer1 = new NumericUpDown() { Location = new Point(250, 50), Width = 60 };
            numAnswer2 = new NumericUpDown() { Location = new Point(250, 90), Width = 60 };
            numAnswer3 = new NumericUpDown() { Location = new Point(250, 130), Width = 60 };
            numAnswer4 = new NumericUpDown() { Location = new Point(250, 170), Width = 60 };

            lblTimeLeft = new Label() { Text = "Aeg: 30 sek.", Location = new Point(330, 50), AutoSize = true, Font = new Font("Arial", 14, FontStyle.Bold), ForeColor = Color.Green };
            lblResult = new Label() { Text = "", Location = new Point(370, 90), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };

            // Timer
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            // Pane kõik komponendid kokku
            parentControl.Controls.Add(startButton);
            parentControl.Controls.Add(submitButton);
            parentControl.Controls.Add(endQuizButton);
            parentControl.Controls.Add(lblQuestion1);
            parentControl.Controls.Add(lblQuestion2);
            parentControl.Controls.Add(lblQuestion3);
            parentControl.Controls.Add(lblQuestion4);
            parentControl.Controls.Add(numAnswer1);
            parentControl.Controls.Add(numAnswer2);
            parentControl.Controls.Add(numAnswer3);
            parentControl.Controls.Add(numAnswer4);
            parentControl.Controls.Add(lblTimeLeft);
            parentControl.Controls.Add(lblResult);
        }

        // Ühtse stiili määramine nuppudele
        private void SetButtonStyle(Button button)
        {
            button.BackColor = Color.FromArgb(41, 128, 185); // Sinine värv
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Arial", 12, FontStyle.Bold);

            button.MouseEnter += (sender, e) => { button.BackColor = Color.FromArgb(34, 98, 145); }; // Hover efekt
            button.MouseLeave += (sender, e) => { button.BackColor = Color.FromArgb(41, 128, 185); }; // Algvärv
        }

        // Alusta viktoriini
        private void StartButton_Click(object sender, EventArgs e)
        {
            timeLeft = 30;
            lblTimeLeft.Text = $"Aeg: {timeLeft} sek.";
            timer.Start();
            startButton.Enabled = false;
            submitButton.Enabled = true;

            // Juhuslikult valitud küsimused
            int[] questionIndexes = GetRandomQuestions();

            // Kuvame küsimused
            lblQuestion1.Text = questions[questionIndexes[0]];
            lblQuestion2.Text = questions[questionIndexes[1]];
            lblQuestion3.Text = questions[questionIndexes[2]];
            lblQuestion4.Text = questions[questionIndexes[3]];

            // Salvestame vastused
            numAnswer1.Tag = answers[questionIndexes[0]];
            numAnswer2.Tag = answers[questionIndexes[1]];
            numAnswer3.Tag = answers[questionIndexes[2]];
            numAnswer4.Tag = answers[questionIndexes[3]];
        }

        // Juhuslikult küsimused
        private int[] GetRandomQuestions()
        {
            int[] questionIndexes = { 0, 1, 2, 3, 4, 5, 6, 7 };
            for (int i = 0; i < questionIndexes.Length; i++)
            {
                int j = rand.Next(i, questionIndexes.Length);
                int temp = questionIndexes[i];
                questionIndexes[i] = questionIndexes[j];
                questionIndexes[j] = temp;
            }

            return new int[] { questionIndexes[0], questionIndexes[1], questionIndexes[2], questionIndexes[3] };
        }

        // Esita vastused ja kontrolli neid
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            int correctAnswers = 0;

            if ((int)numAnswer1.Tag == (int)numAnswer1.Value) correctAnswers++;
            if ((int)numAnswer2.Tag == (int)numAnswer2.Value) correctAnswers++;
            if ((int)numAnswer3.Tag == (int)numAnswer3.Value) correctAnswers++;
            if ((int)numAnswer4.Tag == (int)numAnswer4.Value) correctAnswers++;

            lblResult.Text = $"Õigeid vastuseid: {correctAnswers}/4";

            submitButton.Enabled = false;
            endQuizButton.Enabled = true;
        }

        // Lõpeta viktoriin, tühjenda vastused ja uuenda aega
        private void EndQuizButton_Click(object sender, EventArgs e)
        {
            // Tühjendame kõik vastused
            numAnswer1.Value = 0;
            numAnswer2.Value = 0;
            numAnswer3.Value = 0;
            numAnswer4.Value = 0;

            // Taastame aja algväärtuse
            timeLeft = 30;
            lblTimeLeft.Text = $"Aeg: {timeLeft} sek.";
            lblResult.Text = " ";

            // Taastame nuppude olekud (start nupp aktiivseks, submit nupp mitteaktiivseks)
            startButton.Enabled = true;
            submitButton.Enabled = false;
            endQuizButton.Enabled = false;
        }

        // Aja loenduri täiendamine
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                lblTimeLeft.Text = $"Aeg: {timeLeft} sek.";
            }
            else
            {
                timer.Stop();
                SubmitButton_Click(sender, e); // Kui aeg otsa saab, esita vastused automaatselt
            }
        }

        // Kuvab viktoriini
        public void Show()
        {
            startButton.Visible = true;
            submitButton.Visible = true;
            endQuizButton.Visible = true;
            lblQuestion1.Visible = true;
            lblQuestion2.Visible = true;
            lblQuestion3.Visible = true;
            lblQuestion4.Visible = true;
            numAnswer1.Visible = true;
            numAnswer2.Visible = true;
            numAnswer3.Visible = true;
            numAnswer4.Visible = true;
            lblTimeLeft.Visible = true;
            lblResult.Visible = true;
        }

        // Peidab viktoriini
        public void Hide()
        {
            startButton.Visible = false;
            submitButton.Visible = false;
            endQuizButton.Visible = false;
            lblQuestion1.Visible = false;
            lblQuestion2.Visible = false;
            lblQuestion3.Visible = false;
            lblQuestion4.Visible = false;
            numAnswer1.Visible = false;
            numAnswer2.Visible = false;
            numAnswer3.Visible = false;
            numAnswer4.Visible = false;
            lblTimeLeft.Visible = false;
            lblResult.Visible = false;
        }
    }
}

