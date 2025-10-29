using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KolmRakendust
{
    public class MatchingGame
    {
        private Label[] labels;
        private Label firstClicked = null;
        private Label secondClicked = null;
        private Random random = new Random();

        private Timer flipTimer; // kaartide pööramise taimer
        private Timer gameTimer; // mängu aja taimer

        private int matchedPairs = 0;
        private int points = 0;
        private int timeLeftSeconds;
        private bool gameActive = false;

        private Form form;
        private Label timeLabel;
        private Button level1Btn;
        private Button level2Btn;
        private Button level3Btn;
        private Button btnBack; // Back button

        private string[] currentIcons; // aktiivse mängu ikoonid

        // Algne sümbolite komplekt
        private readonly List<string> baseIcons = new List<string>()
        {
            "!", "N", ",", "k", "b", "v", "w", "z", "p", "d", "h", "s", "a", "l", "r"
        };

        public MatchingGame(Form form)
        {
            this.form = form;
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Add "Back" button
            btnBack = new Button();
            btnBack.Text = "Tagasi";
            btnBack.Location = new Point(150, 500);  // Set position of the "Back" button
            btnBack.Click += BtnBack_Click;
            form.Controls.Add(btnBack);
            SetButtonStyle(btnBack);


            // Aja näitaja
            timeLabel = new Label
            {
                Width = 200,
                Height = 30,
                Text = "Aeg: 00:00",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(150, 20)
            };
            form.Controls.Add(timeLabel);

            // Tase 1
            level1Btn = new Button
            {
                Text = "Tase 1",
                Location = new Point(370, 20),
                Width = 80
            };
            level1Btn.Click += (s, e) => StartLevel(1);
            form.Controls.Add(level1Btn);
            SetButtonStyle(level1Btn);

            // Tase 2
            level2Btn = new Button
            {
                Text = "Tase 2",
                Location = new Point(460, 20),
                Width = 80
            };
            level2Btn.Click += (s, e) => StartLevel(2);
            form.Controls.Add(level2Btn);
            SetButtonStyle(level2Btn);

            // Tase 3
            level3Btn = new Button
            {
                Text = "Tase 3",
                Location = new Point(550, 20),
                Width = 80
            };
            level3Btn.Click += (s, e) => StartLevel(3);
            form.Controls.Add(level3Btn);
            SetButtonStyle(level3Btn);

            // Taimerid
            flipTimer = new Timer { Interval = 750 };
            flipTimer.Tick += FlipTimer_Tick;

            gameTimer = new Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;

            // Käivitame esimese taseme
            StartLevel(1);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            // Hide current form (MatchingGame) and show main UI elements
            this.Hide();
            ((Form1)form).ShowUIElements();  // Show main UI elements (buttons and label)
        }

        private void StartLevel(int level)
        {
            // Eemaldame vanad kaardid
            if (labels != null)
            {
                foreach (var l in labels)
                {
                    if (l != null && form.Controls.Contains(l))
                        form.Controls.Remove(l);
                }
            }

            firstClicked = null;
            secondClicked = null;
            matchedPairs = 0;
            points = 0;
            gameActive = true;

            int pairs;
            switch (level)
            {
                case 1:
                    pairs = 9; // 18 kaarti
                    timeLeftSeconds = 30;
                    break;
                case 2:
                    pairs = 12; // 24 kaarti
                    timeLeftSeconds = 45;
                    break;
                case 3:
                default:
                    pairs = 15; // 30 kaarti
                    timeLeftSeconds = 60;
                    break;
            }

            // Loome aktiivse ikoonide komplekti
            var selected = new List<string>();
            for (int i = 0; i < pairs; i++)
            {
                string symbol = baseIcons[i % baseIcons.Count];
                selected.Add(symbol);
                selected.Add(symbol);
            }
            currentIcons = selected.ToArray();
            ShuffleIcons();

            int totalCards = pairs * 2;
            labels = new Label[totalCards];

            // Dünaamiline paigutus
            int columns = Math.Min(6, pairs); // max 6 veergu
            int spacing = 110;
            int startX = 150;
            int startY = 70;
            int x = startX, y = startY;

            for (int i = 0; i < totalCards; i++)
            {
                labels[i] = new Label
                {
                    Width = 100,
                    Height = 100,
                    Text = "",
                    Font = new Font("Webdings", 28, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(41, 128, 185), // Kaart alguses sinine
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(x, y)
                };

                labels[i].Click += Label_Click;
                form.Controls.Add(labels[i]);

                x += spacing;
                if ((i + 1) % columns == 0)
                {
                    x = startX;
                    y += spacing;
                }
            }

            UpdateTimeLabel();
        }

        private void ShuffleIcons()
        {
            for (int i = 0; i < currentIcons.Length; i++)
            {
                int j = random.Next(currentIcons.Length);
                (currentIcons[i], currentIcons[j]) = (currentIcons[j], currentIcons[i]);
            }
        }

        private void SetButtonStyle(Button button)
        {
            button.BackColor = Color.FromArgb(41, 128, 185); // Sinine värv
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Arial", 10, FontStyle.Bold);

            button.MouseEnter += (sender, e) => { button.BackColor = Color.FromArgb(34, 98, 145); }; // Hover efekt
            button.MouseLeave += (sender, e) => { button.BackColor = Color.FromArgb(41, 128, 185); }; // Algvärv
        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (!gameActive || flipTimer.Enabled) return;

            var clickedLabel = sender as Label;
            if (clickedLabel == null || clickedLabel.Text != "") return;

            // Käivitame taimeri esimesel klikkimisel
            if (!gameTimer.Enabled) gameTimer.Start();

            int index = Array.IndexOf(labels, clickedLabel);
            clickedLabel.Text = currentIcons[index];
            clickedLabel.BackColor = Color.Yellow; // Kui kaart on valitud, muudame selle kollaseks

            if (firstClicked == null)
            {
                firstClicked = clickedLabel;
                return;
            }

            secondClicked = clickedLabel;

            if (firstClicked.Text == secondClicked.Text)
            {
                matchedPairs++;
                points += 10;
                firstClicked.BackColor = Color.LightGreen; // Õige paar - roheliseks
                secondClicked.BackColor = Color.LightGreen; // Õige paar - roheliseks
                ResetClickedLabels();

                if (matchedPairs == currentIcons.Length / 2)
                {
                    gameTimer.Stop();
                    gameActive = false;
                    MessageBox.Show($"🎉 Võit! Punktid: {points}");
                }
            }
            else
            {
                flipTimer.Start();
            }
        }

        private void FlipTimer_Tick(object sender, EventArgs e)
        {
            flipTimer.Stop();

            if (firstClicked != null)
            {
                firstClicked.Text = "";
                firstClicked.BackColor = Color.FromArgb(41, 128, 185);  // Kui kaarti ei ole õige paar, värvime siniseks
            }
            if (secondClicked != null)
            {
                secondClicked.Text = "";
                secondClicked.BackColor = Color.FromArgb(41, 128, 185);  // Kui kaarti ei ole õige paar, värvime siniseks
            }
            ResetClickedLabels();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeftSeconds--;
            UpdateTimeLabel();

            if (timeLeftSeconds <= 0)
            {
                gameTimer.Stop();
                gameActive = false;
                MessageBox.Show("⏰ Aeg on otsas!");
            }
        }

        private void UpdateTimeLabel()
        {
            int minutes = timeLeftSeconds / 60;
            int seconds = timeLeftSeconds % 60;
            timeLabel.Text = $"Aeg: {minutes:00}:{seconds:00}";
        }

        private void ResetClickedLabels()
        {
            firstClicked = null;
            secondClicked = null;
        }


        public void Show()
        {
            level1Btn.Visible = true;
            level2Btn.Visible = true;
            level3Btn.Visible = true;
            timeLabel.Visible = true;
            foreach (var label in labels)
                label.Visible = true;
        }

        public void Hide()
        {
            level1Btn.Visible = false;
            level2Btn.Visible = false;
            level3Btn.Visible = false;
            timeLabel.Visible = false;
            foreach (var label in labels)
                label.Visible = false;
        }
    }
}
