using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegulatedNoise.EDDB_Data
{
    public partial class EDStationView : Form
    {
        private EDStation m_Station;

        internal EDStationView()
        {
            InitializeComponent();

        }

        internal EDStationView(EDStation Station)
        {

            InitializeComponent();

            m_Station = Station;

            showData();

            this.Show();
        }

        internal void showData()
        {
            for (int i = 0; i < m_Station.GetType().GetProperties().Count(); i++)
            {
                String Name = m_Station.GetType().GetProperties()[i].Name;
                String PropType = m_Station.GetType().GetProperties()[i].PropertyType.UnderlyingSystemType.Name;
                
            }

        }
    }
}
