using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Visualization
{
    public partial class Form1 : Form
    {
        Deck d = new Deck();

        public Form1()
        {
            InitializeComponent();
        }

        private void Testbtn_Click(object sender, EventArgs e)
        {
            d.cards = Deck.GenerateDeck();
        }

        public void PreFlop()
        {
            PlayerCardOne.Image = d.Draw().Image;
            PlayerCardTwo.Image = d.Draw().Image;
        }

        public void Flop()
        {
            CommunityOne.Image = d.Draw().Image;
            CommunityTwo.Image = d.Draw().Image;
            CommunityThree.Image = d.Draw().Image;
        }

        public void Turn()
        {
            CommunityFour.Image = d.Draw().Image;
        }

        public void River()
        {
            CommunityFive.Image = d.Draw().Image;
        }
    }
}
