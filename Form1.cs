using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KolmRakendust
{
    public partial class Form1 : Form
    {
        TreeView tree;
        Button btn;
        Label lbl;
        private pildiVaatamise pildiVaataja;
        private MathQuiz mathQuiz;
        private MatchingGame matchingGame;

        public Form1()
        {
            InitializeComponent();
            this.Height = 800;
            this.Width = 1000;
            this.Text = "Kolm rakendus ";
            tree = new TreeView();
            tree.Dock = DockStyle.Left;
            tree.AfterSelect += Tree_AfterSelect;
            TreeNode tn = new TreeNode("Elemendid");
            tn.Nodes.Add(new TreeNode("Pildi vaatamise programm"));
            tn.Nodes.Add(new TreeNode("Matemaatiline äraarvamismäng"));
            tn.Nodes.Add(new TreeNode("Sarnaste piltide leidmise mäng"));
            tn.Nodes.Add(new TreeNode("Välja"));


            btn = new Button();
            btn.Text = "Vajuta siia";
            btn.Location = new Point(150, 30);
            btn.Height = 30;
            btn.Width = 100;
            //pealkiri
            lbl = new Label();
            lbl.Text = "Elementide loomine c# abil";
            lbl.Font = new Font("Arial", 24);
            lbl.Size = new Size(400, 30);
            lbl.Location = new Point(150, 0);
            lbl.MouseHover += Lbl_MouseHover;
            lbl.MouseLeave += Lbl_MouseLeave;
            tree.Nodes.Add(tn);
            this.Controls.Add(tree);
            pildiVaataja = new pildiVaatamise(this);
            pildiVaataja.Hide();
            mathQuiz = new MathQuiz(this);
            mathQuiz.Hide();
            matchingGame = new MatchingGame(this);
            matchingGame.Hide();


        }


        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "Pildi vaatamise programm")
            {
                pildiVaataja.Show();

            }
            else if (e.Node.Text == "Matemaatiline test")
            {
                mathQuiz.Show();
            }
            else if (e.Node.Text == "Sarnaste piltide leidmise mäng")
            {
                matchingGame.Show();
            }
        }

        private void Lbl_MouseLeave(object sender, EventArgs e)
        {
            lbl.BackColor = Color.Transparent;
            Form1 Form = new Form1();
            Form.Show();
            this.Hide();


        }

        private void Lbl_MouseHover(object sender, EventArgs e)
        {
            lbl.BackColor = Color.FromArgb(200, 10, 20);
        }

    }
}
