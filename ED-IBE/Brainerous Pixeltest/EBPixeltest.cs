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
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise
{
    public partial class EBPixeltest : RNBaseForm
    {
        public delegate void delStartModal(RegulatedNoise.Form1 parent);
        private Form m_parent = null;

        public override string thisObjectName { get { return "EBPixeltest"; } }

        public EBPixeltest()
        {
            InitializeComponent();
        }

        /// <summary>
        /// adds a new test picture
        /// </summary>
        /// <param name="newPicture"></param>
        /// <param name="Pixelcount"></param>
        public void addPicture(Bitmap newPicture, int Pixelcount)
        {
            PictureData newData         = new PictureData();

            newData.pbPicture.Image     = newPicture;
            newData.lblPixelcount.Text  = String.Format("dark pixel found : {0}", Pixelcount);

            flowLayoutPanel1.Controls.Add(newData);

        }

        /// <summary>
        /// OK_clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            this.Close();
        }
         
        /// <summary>
        /// starts the form modal with it's parent
        /// </summary>
        /// <param name="parent"></param>
        public void StartModal(RegulatedNoise.Form1 parent)
        {
            if (parent.InvokeRequired)
            {
                parent.Invoke(new delStartModal(StartModal), parent);
            }
            else
            {
                m_parent = parent;
                this.ShowDialog(parent);
            }
        }

        /// <summary>
        /// Shown-event: window will be centered over it's parent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EBPixeltest_Shown(object sender, EventArgs e)
        {
            this.Left = m_parent.Left + (m_parent.Width - this.Width) / 2;
            this.Top = m_parent.Top + (m_parent.Height - this.Height) / 2;
        }

        
    }
}
