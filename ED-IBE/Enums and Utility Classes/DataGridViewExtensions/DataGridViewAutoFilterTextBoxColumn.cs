using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DataGridViewAutoFilter
{
    public class DataGridViewAutoFilterSingleTextBoxColumn : DataGridViewAutoFilterTextBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the DataGridViewAutoFilterSingleTextBoxColumn class.
        /// </summary>
        public DataGridViewAutoFilterSingleTextBoxColumn() : base(ColumnFilterTypes.SingleSelect)
        {

        }
    }

    public class DataGridViewAutoFilterMultiTextBoxColumn : DataGridViewAutoFilterTextBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the DataGridViewAutoFilterSingleTextBoxColumn class.
        /// </summary>
        public DataGridViewAutoFilterMultiTextBoxColumn() : base(ColumnFilterTypes.MultiSelect)
        {

        }
    }

    public class DataGridViewAutoFilterFullTextBoxColumn : DataGridViewAutoFilterTextBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the DataGridViewAutoFilterSingleTextBoxColumn class.
        /// </summary>
        public DataGridViewAutoFilterFullTextBoxColumn() : base(ColumnFilterTypes.FullText)
        {

        }
    }

    /// <summary>
    /// Represents a DataGridViewTextBoxColumn with a drop-down filter list accessible from the header cell.  
    /// </summary>
    public class DataGridViewAutoFilterTextBoxColumn : DataGridViewTextBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the DataGridViewAutoFilterTextBoxColumn class.
        /// </summary>
        public DataGridViewAutoFilterTextBoxColumn(ColumnFilterTypes filterType) : base()
        {
            m_ColumnFilterType = filterType;

            switch (ColumnFilterType)
            {
                case ColumnFilterTypes.SingleSelect:
                    base.DefaultHeaderCellType = typeof(DataGridViewAutoFilterSingleColumnHeaderCell);        
                    break;
                case ColumnFilterTypes.MultiSelect:
                    base.DefaultHeaderCellType = typeof(DataGridViewAutoFilterMultiColumnHeaderCell);
                    break;
                case ColumnFilterTypes.FullText:
                    base.DefaultHeaderCellType = typeof(DataGridViewAutoFilterFullColumnHeaderCell);
                    break;
            }
            
            base.SortMode = DataGridViewColumnSortMode.Programmatic;
        }

        #region public properties that hide inherited, non-virtual properties: DefaultHeaderCellType and SortMode

        /// <summary>
        /// Returns the AutoFilter header cell type. This property hides the 
        /// non-virtual DefaultHeaderCellType property inherited from the 
        /// DataGridViewBand class. The inherited property is set in the 
        /// DataGridViewAutoFilterTextBoxColumn constructor. 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Type DefaultHeaderCellType
        {
            get
            {
                return typeof(DataGridViewAutoFilterHeaderCell);
            }
        }

        /// <summary>
        /// Gets or sets the sort mode for the column and prevents it from being 
        /// set to Automatic, which would interfere with the proper functioning 
        /// of the drop-down button. This property hides the non-virtual 
        /// DataGridViewColumn.SortMode property from the designer. The inherited 
        /// property is set in the DataGridViewAutoFilterTextBoxColumn constructor.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        [DefaultValue(DataGridViewColumnSortMode.Programmatic)]
        public new DataGridViewColumnSortMode SortMode
        {
            get
            {
                return base.SortMode;
            }
            set
            {
                if (value == DataGridViewColumnSortMode.Automatic)
                {
                    throw new InvalidOperationException(
                        "A SortMode value of Automatic is incompatible with " +
                        "the DataGridViewAutoFilterColumnHeaderCell type. " +
                        "Use the AutomaticSortingEnabled property instead.");
                }
                else
                {
                    base.SortMode = value;
                }
            }
        }

        #endregion

        #region public properties: FilteringEnabled, AutomaticSortingEnabled, DropDownListBoxMaxLines

        /// <summary>
        /// Gets or sets a value indicating whether filtering is enabled for this column. 
        /// </summary>
        [DefaultValue(true)]
        public Boolean FilteringEnabled
        {
            get
            {
                // Return the header-cell value.
                return ((DataGridViewAutoFilterHeaderCell)HeaderCell)
                    .FilteringEnabled;
            }
            set
            {
                // Set the header-cell property. 
                ((DataGridViewAutoFilterHeaderCell)HeaderCell)
                    .FilteringEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether automatic sorting is enabled for this column. 
        /// </summary>
        [DefaultValue(true)]
        public Boolean AutomaticSortingEnabled
        {
            get
            {
                // Return the header-cell value.
                return ((DataGridViewAutoFilterHeaderCell)HeaderCell)
                    .AutomaticSortingEnabled;
            }
            set
            {
                // Set the header-cell property.
                ((DataGridViewAutoFilterHeaderCell)HeaderCell)
                    .AutomaticSortingEnabled = value;
            }
        }

        public enum ColumnFilterTypes
        {
            SingleSelect,
            MultiSelect,
            FullText
        }

        ColumnFilterTypes m_ColumnFilterType;

        public ColumnFilterTypes ColumnFilterType
        {
            get
            {
                return m_ColumnFilterType;
            }
        }

        /// <summary>
        /// Gets or sets the maximum height of the drop-down filter list for this column. 
        /// </summary>
        [DefaultValue(20)]
        public Int32 DropDownListBoxMaxLines
        {
            get
            {
                // Return the header-cell value.
                return ((DataGridViewAutoFilterHeaderCell)HeaderCell)
                    .DropDownListBoxMaxLines;
            }
            set
            {
                // Set the header-cell property.
                ((DataGridViewAutoFilterHeaderCell)HeaderCell)
                    .DropDownListBoxMaxLines = value;
            }
        }

        #endregion public properties

        #region public, static, convenience methods: RemoveFilter and GetFilterStatus

        ///// <summary>
        ///// Removes the filter from the BindingSource bound to the specified DataGridView. 
        ///// </summary>
        ///// <param name="dataGridView">The DataGridView bound to the BindingSource to unfilter.</param>
        //public static void RemoveFilter(DataGridView dataGridView)
        //{
        //    DataGridViewAutoFilterHeaderCell.RemoveFilter(dataGridView);
        //}

        ///// <summary>
        ///// Gets a status string for the specified DataGridView indicating the 
        ///// number of visible rows in the bound, filtered BindingSource, or 
        ///// String.Empty if all rows are currently visible. 
        ///// </summary>
        ///// <param name="dataGridView">The DataGridView bound to the 
        ///// BindingSource to return the filter status for.</param>
        ///// <returns>A string in the format "x of y records found" where x is 
        ///// the number of rows currently displayed and y is the number of rows 
        ///// available, or String.Empty if all rows are currently displayed.</returns>
        //public static String GetFilterStatus(DataGridView dataGridView)
        //{
        //    return DataGridViewAutoFilterHeaderCell.GetFilterStatus(dataGridView);
        //}

        #endregion
    }

}
