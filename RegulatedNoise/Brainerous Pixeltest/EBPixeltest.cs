using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegulatedNoise.Brainerous_Pixeltest;

namespace RegulatedNoise
{
    public partial class EBPixeltest : Form
    {
        public EBPixeltest()
        {
            InitializeComponent();
        }

        public void addPicture(Bitmap newPicture, int Pixelcount)
        {
            PictureData newData         = new PictureData();

            newData.pbPicture.Image     = newPicture;
            newData.lblPixelcount.Text  = String.Format("dark pixel found : {0}", Pixelcount);

            flowLayoutPanel1.Controls.Add(newData);

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            this.Close();
        }

    }
}
