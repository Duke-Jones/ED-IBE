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

#region event handler

        [System.ComponentModel.Browsable(true)]
        public event EventHandler<SortedEventArgs> ColumnSorted;

        protected virtual void OnDataChanged(SortedEventArgs e)
        {
            EventHandler<SortedEventArgs> myEvent = ColumnSorted;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class SortedEventArgs : EventArgs
        {
            public SortedEventArgs()
            {
                SortColumn = null;
                SortOrder  = System.Windows.Forms.SortOrder.None;
            }

            public DataGridViewColumn   SortColumn { get; set; }
            public SortOrder            SortOrder  { get; set; }
        }

#endregion


        public DataGridViewExt()
        {
            InitializeComponent();

            this.Sorted += DataGridViewExt_Sorted;
        }

        public DataGridViewExt(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            this.Sorted += DataGridViewExt_Sorted;
        }

        public Boolean DoubleBuffer {
            get { return this.DoubleBuffered; }
            set { this.DoubleBuffered = value; }
        }

        /// <summary>
        /// Event raises after the grid is sorted by a column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataGridViewExt_Sorted(object sender, EventArgs e)
        {
            var EA = new SortedEventArgs() { SortColumn = this.SortedColumn, SortOrder = this.SortOrder};
            ColumnSorted.Raise(this, EA);
        }

    }
}
