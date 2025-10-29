using System;
using System.Drawing;
using System.Windows.Forms;

namespace KolmRakendust
{
    public class pildiVaatamise
    {
        private Button showButton, clearButton, backgroundButton, closeButton, rotateButton;
        private CheckBox stretchCheckBox;
        private CheckBox grayCheckBox, negativeCheckBox, brightCheckBox, darkCheckBox;
        private PictureBox pictureBox;
        private OpenFileDialog openFileDialog;
        private ColorDialog colorDialog;
        private Control parentControl;
        private Bitmap originalImage; // alati originaal
        private int rotationAngle = 0; // pööramise aste (0, 90, 180, 270)

        public pildiVaatamise(Control parent)
        {
            parentControl = parent;

            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All files|*.*";
            colorDialog = new ColorDialog();

            // Põhinupud
            showButton = new Button { Text = "Vali pilt...", Location = new Point(150, 10), Size = new Size(100, 30) };
            showButton.Click += ShowButton_Click;
            SetButtonStyle(showButton);

            clearButton = new Button { Text = "Tühjenda pilt", Location = new Point(260, 10), Size = new Size(100, 30) };
            clearButton.Click += ClearButton_Click;
            SetButtonStyle(clearButton);

            backgroundButton = new Button { Text = "Taustavärv", Location = new Point(370, 10), Size = new Size(100, 30) };
            backgroundButton.Click += BackgroundButton_Click;
            SetButtonStyle(backgroundButton);

            closeButton = new Button { Text = "Sulge", Location = new Point(480, 10), Size = new Size(100, 30) };
            closeButton.Click += CloseButton_Click;
            SetButtonStyle(closeButton);

            rotateButton = new Button { Text = "Pööra pilt", Location = new Point(590, 10), Size = new Size(100, 30) };
            rotateButton.Click += RotateButton_Click;
            SetButtonStyle(rotateButton);

            // --- Filtri CheckBoxid ---
            grayCheckBox = new CheckBox { Text = "Halltoon", Location = new Point(150, 510), Size = new Size(100, 20) };
            negativeCheckBox = new CheckBox { Text = "Negatiiv", Location = new Point(260, 510), Size = new Size(100, 20) };
            brightCheckBox = new CheckBox { Text = "Heledam", Location = new Point(370, 510), Size = new Size(100, 20) };
            darkCheckBox = new CheckBox { Text = "Tumedam", Location = new Point(480, 510), Size = new Size(100, 20) };
            stretchCheckBox = new CheckBox { Text = "Rasta pilt", Location = new Point(590, 510), Size = new Size(100, 20) };
            stretchCheckBox.CheckedChanged += StretchCheckBox_CheckedChanged;

            grayCheckBox.CheckedChanged += FilterChanged;
            negativeCheckBox.CheckedChanged += FilterChanged;
            brightCheckBox.CheckedChanged += FilterChanged;
            darkCheckBox.CheckedChanged += FilterChanged;


            // PictureBox
            pictureBox = new PictureBox
            {
                Location = new Point(150, 50),
                Size = new Size(540, 450),
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.Zoom
            };


            // Lisa kõik komponendid parentile
            parent.Controls.Add(showButton);
            parent.Controls.Add(clearButton);
            parent.Controls.Add(backgroundButton);
            parent.Controls.Add(closeButton);
            parent.Controls.Add(stretchCheckBox);
            parent.Controls.Add(pictureBox);
            parent.Controls.Add(rotateButton);

            parent.Controls.Add(grayCheckBox);
            parent.Controls.Add(negativeCheckBox);
            parent.Controls.Add(brightCheckBox);
            parent.Controls.Add(darkCheckBox);
        }


        // --- Funktsioonid ---
        private void ShowButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                originalImage = new Bitmap(openFileDialog.FileName);
                rotationAngle = 0;
                UpdateImage();
            }
        }
   

        private void ClearButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = null;
            originalImage = null;
            rotationAngle = 0;
        }

        private void BackgroundButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
                pictureBox.BackColor = colorDialog.Color;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (parentControl is Form f) f.Close();
        }

        private void StretchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox.SizeMode = stretchCheckBox.Checked ?
                PictureBoxSizeMode.StretchImage :
                PictureBoxSizeMode.Zoom;
        }

        // --- Pööra (ainult visuaalselt, mitte päriselt) ---
        private void RotateButton_Click(object sender, EventArgs e)
        {
            if (originalImage == null) return;
            rotationAngle = (rotationAngle + 90) % 360;
            UpdateImage();
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

        // --- Rakendab filtrid ja pööramise ---
        private void UpdateImage()
        {
            if (originalImage == null) return;
            Bitmap bmp = new Bitmap(originalImage);

            // Rakenda filtrid
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = originalImage.GetPixel(x, y);
                    int r = c.R, g = c.G, b = c.B;

                    if (grayCheckBox.Checked)
                    {
                        int gray = (r + g + b) / 3;
                        r = g = b = gray;
                    }
                    if (negativeCheckBox.Checked)
                    {
                        r = 255 - r;
                        g = 255 - g;
                        b = 255 - b;
                    }
                    if (brightCheckBox.Checked)
                    {
                        r = Math.Min(r + 30, 255);
                        g = Math.Min(g + 30, 255);
                        b = Math.Min(b + 30, 255);
                    }
                    if (darkCheckBox.Checked)
                    {
                        r = Math.Max(r - 30, 0);
                        g = Math.Max(g - 30, 0);
                        b = Math.Max(b - 30, 0);
                    }

                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            // Kui pööre on valitud, tee sellest koopia
            if (rotationAngle != 0)
            {
                switch (rotationAngle)
                {
                    case 90: bmp.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                    case 180: bmp.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                    case 270: bmp.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
                }
            }

            pictureBox.Image = bmp;
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        // --- Show ja Hide ---
        public void Show()
        {
            showButton.Visible = true;
            clearButton.Visible = true;
            backgroundButton.Visible = true;
            closeButton.Visible = true;
            stretchCheckBox.Visible = true;
            pictureBox.Visible = true;
            rotateButton.Visible = true;

            grayCheckBox.Visible = true;
            negativeCheckBox.Visible = true;
            brightCheckBox.Visible = true;
            darkCheckBox.Visible = true;
        }

        public void Hide()
        {
            showButton.Visible = false;
            clearButton.Visible = false;
            backgroundButton.Visible = false;
            closeButton.Visible = false;
            stretchCheckBox.Visible = false;
            pictureBox.Visible = false;
            rotateButton.Visible = false;

            grayCheckBox.Visible = false;
            negativeCheckBox.Visible = false;
            brightCheckBox.Visible = false;
            darkCheckBox.Visible = false;
        }
    }
}
