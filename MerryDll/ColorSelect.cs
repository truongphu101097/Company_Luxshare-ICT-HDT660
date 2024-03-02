using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MerryDllFramework
{
    public partial class ColorSelect : Form
    {
        public ColorSelect()
        {
            InitializeComponent();
            btnSelect.Focus();
        }
        private string i = "00";
        private void cboxBlack_CheckedChanged(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
            Color1(Color.White);
            cboxWhite.Checked = false;
            cboxBlue.Checked = false;
            i = "00";
        }
        private void Color1(Color a)
        {
            cboxBlack.ForeColor = a;
            cboxBlue.ForeColor = a;
            cboxWhite.ForeColor = a;
            //btnSelect.ForeColor = a;
           // label1.ForeColor = a;
            label1.BackColor = Color.Gainsboro;
        }


        private void cboxWhite_CheckedChanged(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            Color1(Color.Black);
            cboxBlack.Checked = false;
            cboxBlue.Checked = false;
            i = "01";
        }

        private void cboxBlue_CheckedChanged(object sender, EventArgs e)
        {
            this.BackColor = Color.Blue;
            Color1(Color.Red);
            cboxBlack.Checked = false;
            cboxWhite.Checked = false;
            i = "02";
        }
        public string ColorReturn()
        {
            return i;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            ColorReturn();
            this.Close();
        }

        private void ColorSelect_Load(object sender, EventArgs e)
        {
            btnSelect.Focus();
        }

        private void btnSelect_KeyDown(object sender, KeyEventArgs e)
        {
            btnSelect_Click(null, null);
        }

        private void cboxBlack_KeyDown(object sender, KeyEventArgs e)
        {
            btnSelect_Click(null, null);
        }

        private void cboxWhite_KeyDown(object sender, KeyEventArgs e)
        {
            btnSelect_Click(null, null);
        }

        private void cboxBlue_KeyDown(object sender, KeyEventArgs e)
        {
            btnSelect_Click(null, null);
        }

        private void Lara_CheckedChanged(object sender, EventArgs e)
        {
            this.BackColor = Color.Lavender;
            Color1(Color.Red);
            cboxBlack.Checked = false;
            cboxWhite.Checked = false;
            i = "03";
        }
    }
}
