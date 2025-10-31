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
        private Label lblQuestion1, lblQuestion2, lblQuestion3, lblQuestion4, lblResult;
        private Label lblProgressText;
        private NumericUpDown numAnswer1, numAnswer2, numAnswer3, numAnswer4;
        private Timer timer;
        private int timeLeft;
        private const int maxTime = 30;
        private ProgressBar timeProgressBar;
        private Panel progressPanel;
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

            // --- Nupud ---
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

            // --- Küsimused ja vastused ---
            lblQuestion1 = new Label() { Text = "", Location = new Point(150, 60), AutoSize = true, Font = new Font("Arial", 12) };
            lblQuestion2 = new Label() { Text = "", Location = new Point(150, 100), AutoSize = true, Font = new Font("Arial", 12) };
            lblQuestion3 = new Label() { Text = "", Location = new Point(150, 140), AutoSize = true, Font = new Font("Arial", 12) };
            lblQuestion4 = new Label() { Text = "", Location = new Point(150, 180), AutoSize = true, Font = new Font("Arial", 12) };

            numAnswer1 = new NumericUpDown() { Location = new Point(250, 60), Width = 60 };
            numAnswer2 = new NumericUpDown() { Location = new Point(250, 100), Width = 60 };
            numAnswer3 = new NumericUpDown() { Location = new Point(250, 140), Width = 60 };
            numAnswer4 = new NumericUpDown() { Location = new Point(250, 180), Width = 60 };

            // --- Keela vastuseväljad alguses ---
            DisableAnswers();

            lblResult = new Label() { Text = "", Location = new Point(370, 90), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };

            // --- ProgressBar ja tekst samas paneelis ---
            progressPanel = new Panel()
            {
                Location = new Point(150, 230),
                Size = new Size(380, 30)
            };

            timeProgressBar = new ProgressBar()
            {
                Dock = DockStyle.Fill,
                Maximum = maxTime,
                Minimum = 0,
                Value = maxTime
            };

            lblProgressText = new Label()
            {
                Text = $"Aeg: {maxTime} sek",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            progressPanel.Controls.Add(timeProgressBar);
            progressPanel.Controls.Add(lblProgressText);

            // --- Timer ---
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            // --- Lisa kõik komponendid ---
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
            parentControl.Controls.Add(lblResult);
            parentControl.Controls.Add(progressPanel);
        }

        private void SetButtonStyle(Button button)
        {
            button.BackColor = Color.FromArgb(41, 128, 185);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Arial", 12, FontStyle.Bold);

            button.MouseEnter += (sender, e) => { button.BackColor = Color.FromArgb(34, 98, 145); };
            button.MouseLeave += (sender, e) => { button.BackColor = Color.FromArgb(41, 128, 185); };
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            timeLeft = maxTime;
            lblProgressText.Text = $"Aeg: {timeLeft} sek";
            lblProgressText.ForeColor = Color.White;
            timeProgressBar.Value = maxTime;
            timer.Start();
            startButton.Enabled = false;
            submitButton.Enabled = true;

            int[] questionIndexes = GetRandomQuestions();

            lblQuestion1.Text = questions[questionIndexes[0]];
            lblQuestion2.Text = questions[questionIndexes[1]];
            lblQuestion3.Text = questions[questionIndexes[2]];
            lblQuestion4.Text = questions[questionIndexes[3]];

            numAnswer1.Tag = answers[questionIndexes[0]];
            numAnswer2.Tag = answers[questionIndexes[1]];
            numAnswer3.Tag = answers[questionIndexes[2]];
            numAnswer4.Tag = answers[questionIndexes[3]];

            // ✅ Luba kirjutamine vastuseväljadesse
            EnableAnswers();
        }

        private int[] GetRandomQuestions()
        {
            int[] questionIndexes = { 0, 1, 2, 3, 4, 5, 6, 7 };
            for (int i = 0; i < questionIndexes.Length; i++)
            {
                int j = rand.Next(i, questionIndexes.Length);
                (questionIndexes[i], questionIndexes[j]) = (questionIndexes[j], questionIndexes[i]);
            }
            return new int[] { questionIndexes[0], questionIndexes[1], questionIndexes[2], questionIndexes[3] };
        }

        // --- ✅ Uus versioon: pildiga tulemuste aken ---
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            int correctAnswers = 0;
            string correctQuestions = "";

            if ((int)numAnswer1.Tag == (int)numAnswer1.Value) { correctAnswers++; correctQuestions += lblQuestion1.Text + "\n"; }
            if ((int)numAnswer2.Tag == (int)numAnswer2.Value) { correctAnswers++; correctQuestions += lblQuestion2.Text + "\n"; }
            if ((int)numAnswer3.Tag == (int)numAnswer3.Value) { correctAnswers++; correctQuestions += lblQuestion3.Text + "\n"; }
            if ((int)numAnswer4.Tag == (int)numAnswer4.Value) { correctAnswers++; correctQuestions += lblQuestion4.Text + "\n"; }

            string message = $"Õigeid vastuseid: {correctAnswers}/4\n\nÕige vastusega küsimused:\n{correctQuestions}";

            ShowResultWithImage(message, correctAnswers);

            submitButton.Enabled = false;
            endQuizButton.Enabled = true;

            // Pärast vastuste esitamist lukusta väljad
            DisableAnswers();
        }

        // --- Kohandatud vorm pildiga ---
        private void ShowResultWithImage(string message, int correctAnswers)
        {
            Form resultForm = new Form();
            resultForm.Text = "Viktoriini tulemused";
            resultForm.Size = new Size(420, 320);
            resultForm.StartPosition = FormStartPosition.CenterScreen;
            resultForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            resultForm.MaximizeBox = false;
            resultForm.MinimizeBox = false;

            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(100, 100);
            pictureBox.Location = new Point(20, 20);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            try
            {
                if (correctAnswers >= 3)
                    pictureBox.Image = Image.FromFile("trophy.png");
                else
                    pictureBox.Image = Image.FromFile("sad.png");
            }
            catch
            {
                Bitmap fallback = new Bitmap(100, 100);
                using (Graphics g = Graphics.FromImage(fallback))
                {
                    g.Clear(correctAnswers >= 3 ? Color.Gold : Color.LightGray);
                    g.DrawString("No\nImage", new Font("Arial", 8), Brushes.Black, new PointF(10, 35));
                }
                pictureBox.Image = fallback;
            }

            Label lblMessage = new Label();
            lblMessage.Text = message;
            lblMessage.Font = new Font("Arial", 11, FontStyle.Regular);
            lblMessage.AutoSize = false;
            lblMessage.Location = new Point(140, 20);
            lblMessage.Size = new Size(250, 200);

            Button closeButton = new Button();
            closeButton.Text = "OK";
            closeButton.Font = new Font("Arial", 11, FontStyle.Bold);
            closeButton.BackColor = Color.FromArgb(41, 128, 185);
            closeButton.ForeColor = Color.White;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Size = new Size(100, 35);
            closeButton.Location = new Point(160, 230);
            closeButton.Click += (s, e) => resultForm.Close();

            resultForm.Controls.Add(pictureBox);
            resultForm.Controls.Add(lblMessage);
            resultForm.Controls.Add(closeButton);

            resultForm.ShowDialog();
        }

        private void EndQuizButton_Click(object sender, EventArgs e)
        {
            numAnswer1.Value = 0;
            numAnswer2.Value = 0;
            numAnswer3.Value = 0;
            numAnswer4.Value = 0;

            timeLeft = maxTime;
            lblProgressText.Text = $"Aeg: {timeLeft} sek";
            lblProgressText.ForeColor = Color.White;
            timeProgressBar.Value = maxTime;

            startButton.Enabled = true;
            submitButton.Enabled = false;
            endQuizButton.Enabled = false;

            // ✅ Lukusta väljad uuesti
            DisableAnswers();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                lblProgressText.Text = $"Aeg: {timeLeft} sek";
                timeProgressBar.Value = timeLeft;

                if (timeLeft < 10)
                    lblProgressText.ForeColor = Color.Red;
                else if (timeLeft < 20)
                    lblProgressText.ForeColor = Color.Orange;
            }
            else
            {
                timer.Stop();
                SubmitButton_Click(sender, e);
            }
        }

        // --- Abifunktsioonid ---
        private void DisableAnswers()
        {
            numAnswer1.Enabled = false;
            numAnswer2.Enabled = false;
            numAnswer3.Enabled = false;
            numAnswer4.Enabled = false;

            numAnswer1.BackColor = Color.LightGray;
            numAnswer2.BackColor = Color.LightGray;
            numAnswer3.BackColor = Color.LightGray;
            numAnswer4.BackColor = Color.LightGray;
        }

        private void EnableAnswers()
        {
            numAnswer1.Enabled = true;
            numAnswer2.Enabled = true;
            numAnswer3.Enabled = true;
            numAnswer4.Enabled = true;

            numAnswer1.BackColor = Color.White;
            numAnswer2.BackColor = Color.White;
            numAnswer3.BackColor = Color.White;
            numAnswer4.BackColor = Color.White;
        }

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
            lblResult.Visible = true;
            progressPanel.Visible = true;
        }

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
            lblResult.Visible = false;
            progressPanel.Visible = false;
        }
    }
}
