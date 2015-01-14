using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdClasses.ClassDefinitions;
using EdClasses.ClassDefinitions.Commodities;

namespace RegulatedNoise.MRmPTestTab
{
    public partial class MRmPTestTab : UserControl
    {
        public MRmPTestTab()
        {
            InitializeComponent();
        }
        private List<EdSystem> systms = new List<EdSystem>();
        private void button1_Click(object sender, EventArgs e)
        {
            
            systms.Add(new EdClasses.Class1().Test());

            comboBox1.DataSource = systms;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (ComboBox)sender;
            comboBox2.DataSource = ((EdSystem)s.SelectedItem).Stations;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Id";
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (ComboBox)sender;
            comboBox3.DataSource = ((EdStation)s.SelectedItem).Commodities;
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "Id";
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (ComboBox)sender;
            var c = ((EdCommodity)s.SelectedItem);
            label2.Text = c.Name;
            label3.Text = c.Sell.ToString();
            label4.Text = c.Buy.ToString();
            label5.Text = c.Demand.ToString();
            label6.Text = c.DemandRate.ToString();
            label7.Text = c.Supply.ToString();
            label8.Text = c.SupplyRate.ToString();
            label1.Text = c.CommodityType.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var c = new EdCommodity();
            c.Name = textBox1.Text;
            
        }
    }
}
