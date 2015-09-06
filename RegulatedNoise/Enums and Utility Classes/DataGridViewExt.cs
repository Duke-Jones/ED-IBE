using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    public partial class DataGridViewExt : DataGridView
    {
        public DataGridViewExt()
        {
            InitializeComponent();
        }

        public DataGridViewExt(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public Boolean DoubleBuffer {
            get { return this.DoubleBuffered; }
            set { this.DoubleBuffered = value; }
        }
    }
}
