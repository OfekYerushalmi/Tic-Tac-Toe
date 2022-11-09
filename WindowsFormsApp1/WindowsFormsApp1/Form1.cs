using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Board board;
        Button[] myButtons;
        public Form1() 
        {    
            InitializeComponent();
            board = new Board();
            
            myButtons = new Button[16];
            myButtons[0] = button0;
            myButtons[1] = button1;
            myButtons[2] = button2;
            myButtons[3] = button3;
            myButtons[4] = button4;
            myButtons[5] = button5;
            myButtons[6] = button6;
            myButtons[7] = button7;
            myButtons[8] = button8;
            myButtons[9] = button9;
            myButtons[10] = button10;
            myButtons[11] = button11;
            myButtons[12] = button12;
            myButtons[13] = button13;
            myButtons[14] = button14;
            myButtons[15] = button15;

            UpdateButtons();
        }

        //מעדכנת כפתורים
        private void UpdateButtons()
        {
            for (int i=0; i<16; i++)
            {
                if (board.cells[i] == CellStatus.Empty)
                {
                    myButtons[i].Text = "";
                    myButtons[i].Image = null;
                    myButtons[i].Enabled = true;
                }
                else if (board.cells[i] == CellStatus.X)
                {
                    //myButtons[i].Text = "X";
                    myButtons[i].Image = WindowsFormsApp1.Properties.Resources.GREY_X;
                    myButtons[i].Enabled = false;
                }
                else if (board.cells[i] == CellStatus.O)
                {
                    //myButtons[i].Text = "O";
                    myButtons[i].Image = WindowsFormsApp1.Properties.Resources.GREY_O;
                    myButtons[i].Enabled = false;
                }
            }
            int[] winningIndexes;
            CellStatus winner = board.CheckWinner(out winningIndexes); //מחזיר מי ינצח והיכן המיקומים של ניצחון
            if (winner!= CellStatus.Empty)
            {
                Image image;
                if (winner== CellStatus.O)
                {
                    image = WindowsFormsApp1.Properties.Resources.BLACK_O;
                }
                else
                {
                    image = WindowsFormsApp1.Properties.Resources.BLACK_X;
                }
                foreach (int i in winningIndexes)
                {
                    myButtons[i].Image = image;
                }
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void GameOver()
        {
            foreach (Button button in myButtons)
            {
                button.Enabled = false;
            }
        }

        private bool CheckGameOver()
        {
            int[] winningIndexes;
            if (board.CheckWinner(out winningIndexes) == CellStatus.X)
            {
                label3.Text = "Congratulation! you have won !!!!";
                GameOver();
                return true;
            }
            else if (board.CheckWinner(out winningIndexes) == CellStatus.O)
            {
                this.label3.Text = "Maybe next time :(";
                GameOver();
                return true;
            }
            else if (board.CheckTie())
            {
                label3.Text = "Tie";
                GameOver();
                return true;
            } else
            {
                return false;
            }
        }

        private void buttonClicked(int i)
        {
            board.cells[i] = CellStatus.X;            
            UpdateButtons();
            if (!CheckGameOver())
            {
                board.playOneTurn(CellStatus.O, CellStatus.X);                
                UpdateButtons();
                if (!CheckGameOver())
                {
                    this.label3.Text = "Your turn...";
                }
            }
        }

        private void button0_Click(object sender, EventArgs e)
        {
            buttonClicked(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonClicked(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            buttonClicked(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            buttonClicked(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            buttonClicked(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            buttonClicked(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            buttonClicked(6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            buttonClicked(7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            buttonClicked(8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            buttonClicked(9);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            buttonClicked(10);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            buttonClicked(11);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            buttonClicked(12);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            buttonClicked(13);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            buttonClicked(14);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            buttonClicked(15);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            board = new Board();
            UpdateButtons();
            label3.Text = "Started a new game. Your turn.";
            foreach (Button button in myButtons)
            {
                button.Enabled = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
