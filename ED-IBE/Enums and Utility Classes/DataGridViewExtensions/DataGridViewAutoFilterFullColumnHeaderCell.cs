using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Collections;
using System.Reflection;
using IBE.Enums_and_Utility_Classes;
using IBE;

namespace DataGridViewAutoFilter
{
    /// <summary>
    /// Provides a drop-down filter list in a DataGridViewColumnHeaderCell.
    /// </summary>
    public class DataGridViewAutoFilterFullColumnHeaderCell : DataGridViewAutoFilterHeaderCell
    {
        /// <summary>
        /// The filterwindow used for setting filter values
        /// </summary>
        private FullTextHeader filterWindow;

        /// <summary>
        /// A list of filters available for the owning column stored as 
        /// formatted and unformatted string values. 
        /// </summary>
        private System.Collections.Specialized.OrderedDictionary filters =
            new System.Collections.Specialized.OrderedDictionary();

        internal enum ConstraintValues
        {
            cv_filter_off = 0,
            cv_equals = 1,
            cv_contains = 2,
            cv_starts_with = 3,
            cv_ends_with = 4
        }

        String m_SelectedFilterText                 = "";
        ConstraintValues m_SelectedConstraintIndex  = ConstraintValues.cv_filter_off;

        /// <summary>
        /// Initializes a new instance of the DataGridViewColumnHeaderCell 
        /// class and sets its property values to the property values of the 
        /// specified DataGridViewColumnHeaderCell.
        /// </summary>
        /// <param name="oldHeaderCell">The DataGridViewColumnHeaderCell to copy property values from.</param>
        public DataGridViewAutoFilterFullColumnHeaderCell(DataGridViewColumnHeaderCell oldHeaderCell) : base(oldHeaderCell)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DataGridViewColumnHeaderCell 
        /// class. 
        /// </summary>
        public DataGridViewAutoFilterFullColumnHeaderCell() : base()
        {
        }

        /// <summary>
        /// Creates an exact copy of this cell.
        /// </summary>
        /// <returns>An object that represents the cloned DataGridViewAutoFilterFullColumnHeaderCell.</returns>
        public override object Clone()
        {
            return new DataGridViewAutoFilterFullColumnHeaderCell(this);
        }

        /// <summary>
        /// Resets the cached filter values if the filter has been removed.
        /// </summary>
        override public void ResetFilter()
        {
            if (this.DataGridView == null) return;
            BindingSource source = this.DataGridView.DataSource as BindingSource;
            if (source == null || String.IsNullOrEmpty(source.Filter))
            {
                filtered = false;

                m_SelectedConstraintIndex = ConstraintValues.cv_filter_off; 
                m_SelectedFilterText      = "";

                UpdateFilter(true);
            }
        }

        public override string ColumnFilterString
        {
            get
            {
                return String.Format("{0};{1}", (UInt32)m_SelectedConstraintIndex, m_SelectedFilterText);
            }
            set
            {
                string[] parts;
                Int32 intValue;
                ConstraintValues cnstrValue = ConstraintValues.cv_filter_off;
                String filterValue = "";

                parts = value.Split(new char[] { ';' },2);

                if(parts.GetUpperBound(0) == 1)
                {
                    filterValue = parts[1];

                    if(filterValue != "")
                    {
                        if (Int32.TryParse(parts[0], out intValue))
                        {
                            if(Enum.IsDefined(typeof(ConstraintValues), intValue))
                            {
                                cnstrValue = (ConstraintValues)intValue;
                            }
                        }
                    }
                }

                m_SelectedConstraintIndex = cnstrValue; 
                m_SelectedFilterText      = filterValue;

                UpdateFilter(true);
            }
        }

        #region drop-down list: Show/HideFilterControlBox, SetDropDownListBoxBounds, DropDownListBoxMaxHeightInternal


        /// <summary>
        /// Displays the drop-down filter list. 
        /// </summary>
        override public void ShowColumnFilter()
        {
            try
            {
                if(filterControlShowing)
                {
                    HideFilterControl();
                    return;
                }

                // Cast the data source to a BindingSource. 
                BindingSource data = this.DataGridView.DataSource as BindingSource;

                Debug.Assert((data != null && data.SupportsFiltering && OwningColumn != null) || (Retriever != null),
                    "DataSource is not a BindingSource, or does not support filtering, or OwningColumn is null");

                if (data != null)
                {
                    throw new NotImplementedException();
                }

                Debug.Assert(this.DataGridView != null, "DataGridView is null");

                // Ensure that the current row is not the row for new records.
                // This prevents the new row from affecting the filter list and also
                // prevents the new row from being added when the filter changes.
                if (this.DataGridView.CurrentRow != null &&
                    this.DataGridView.CurrentRow.IsNewRow)
                {
                    this.DataGridView.CurrentCell = null;
                }

                if (filterWindow != null)
                {
                    this.DataGridView.Controls.Remove(filterWindow);
                    filterWindow.Dispose();
                }

                filterWindow = new FullTextHeader();

                filterWindow.txtFilterText.Text             = m_SelectedFilterText;
                filterWindow.cmbConstraint.SelectedIndex    = (Int32)m_SelectedConstraintIndex;

                // Add handlers to dropDownListBox.FilterListBox events. 
                HandleDropDownListBoxEvents();

                //textSelectBox.Visible = true;
                filterControlShowing = true;
                filterWindow.Show(this.DataGridView);

                // Set the size and location of dropDownListBox, then display it. 
                SetDropDownListBoxBounds();

                // Invalidate the cell so that the drop-down button will repaint
                // in the pressed state. 
                this.DataGridView.InvalidateCell(this);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while showing the column filter setting (" +  this.OwningColumn.Name + ")");
            }
        }

        /// <summary>
        /// Hides the drop-down filter list. 
        /// </summary>
        override protected void HideFilterControl()
        {
            if(filterControlShowing)
            {
                filterControlShowing = false;

                Debug.Assert(this.DataGridView != null, "DataGridView is null");

                // Hide dropDownListBox.FilterListBox, remove handlers from its events, and remove 
                // it from the DataGridView control. 
                UnhandleDropDownListBoxEvents();

                filterWindow.Visible = false;
                filterWindow.Dispose();
                filterWindow = null;

                //this.DataGridView.Controls.Remove(multiSelectBox);

                // Invalidate the cell so that the drop-down button will repaint
                // in the unpressed state. 
                this.DataGridView.InvalidateCell(this);
                this.DataGridView.Focus();
            }
        }

        /// <summary>
        /// Sets the dropDownListBox.FilterListBox size and position based on the formatted 
        /// values in the filters dictionary and the position of the drop-down 
        /// button. Called only by ShowDropDownListBox.  
        /// </summary>
        private void SetDropDownListBoxBounds()
        {
            //Debug.Assert(filters.Count > 0, "filters.Count <= 0");

            // Declare variables that will be used in the calculation, 
            // initializing dropDownListBoxHeight to account for the 
            // ListBox borders.
            Int32 dropDownListBoxHeight = filterWindow.Height;
            Int32 dropDownListBoxWidth = filterWindow.Width;
            Int32 dropDownListBoxLeft = 0;

            // Calculate the ideal location of the left edge of dropDownListBox.FilterListBox 
            // based on the location of the drop-down button and taking the 
            // RightToLeft property value into consideration. 
            if (this.DataGridView.RightToLeft == RightToLeft.No)
            {
                dropDownListBoxLeft = DropDownButtonBounds.Right - 
                    dropDownListBoxWidth + 1;
            }
            else
            {
                dropDownListBoxLeft = DropDownButtonBounds.Left - 1;
            }

            // Determine the left and right edges of the available horizontal
            // width of the DataGridView control. 
            Int32 clientLeft = 1;
            Int32 clientRight = this.DataGridView.ClientRectangle.Right;

            // Adjust the dropDownListBox.FilterListBox location and/or width if it would
            // otherwise overlap the left or right edge of the DataGridView.
            if (dropDownListBoxLeft < clientLeft)
            {
                dropDownListBoxLeft = clientLeft;
            }
            Int32 dropDownListBoxRight = 
                dropDownListBoxLeft + dropDownListBoxWidth + 1;
            if (dropDownListBoxRight > clientRight)
            {
                if (dropDownListBoxLeft == clientLeft)
                {
                    dropDownListBoxWidth -=
                        dropDownListBoxRight - clientRight;
                }
                else
                {
                    dropDownListBoxLeft -=
                        dropDownListBoxRight - clientRight;
                    if (dropDownListBoxLeft < clientLeft)
                    {
                        dropDownListBoxWidth -= clientLeft - dropDownListBoxLeft;
                        dropDownListBoxLeft = clientLeft;
                    }
                }
            }

            // Set the ListBox.Bounds property using the calculated values. 
            
            filterWindow.Bounds = this.DataGridView.RectangleToScreen(
                        new Rectangle(dropDownListBoxLeft,
                                      DropDownButtonBounds.Bottom, 
                                      dropDownListBoxWidth, 
                                      dropDownListBoxHeight));
        }

        #endregion drop-down list

        #region ListBox events: HandleDropDownListBoxEvents, UnhandleDropDownListBoxEvents, ListBox event handlers

        /// <summary>
        /// Adds handlers to ListBox events for handling mouse
        /// and keyboard input.
        /// </summary>
        private void HandleDropDownListBoxEvents()
        {
            //dropDownListBox.FilterListBox.MouseClick += new MouseEventHandler(DropDownListBox_MouseClick);
            filterWindow.LostFocus += new EventHandler(DropDownListBox_LostFocus);
            filterWindow.KeyDown += new KeyEventHandler(DropDownListBox_KeyDown);
            filterWindow.Deactivate += FilterWindow_Deactivate;
            filterWindow.cmdOk.Click += CmdOk_Click;

        }


        /// <summary>
        /// Removes the ListBox event handlers. 
        /// </summary>
        private void UnhandleDropDownListBoxEvents()
        {
            //dropDownListBox.FilterListBox.MouseClick -= new MouseEventHandler(DropDownListBox_MouseClick);
            filterWindow.LostFocus -= new EventHandler(DropDownListBox_LostFocus);
            filterWindow.KeyDown -= new KeyEventHandler(DropDownListBox_KeyDown);
            filterWindow.Deactivate -= FilterWindow_Deactivate;
            filterWindow.cmdOk.Click -= CmdOk_Click;
        }

        private void CmdOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (filterWindow.cmbConstraint.SelectedIndex == (Int32)ConstraintValues.cv_filter_off)
                {
                    filterWindow.txtFilterText.Text = "";
                }

                UpdateFilter();
                HideFilterControl();

                RaiseDataChangedEvent();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while confirming the column filter setting (" + this.OwningColumn.Name + ")");
            }
        }

        #endregion ListBox events

        #region filtering: PopulateFilters, FilterWithoutCurrentColumn, UpdateFilter, RemoveFilter, AvoidNewRowWhenFiltering, GetFilterStatus

        ///// <summary>
        ///// Returns a copy of the specified filter string after removing the part that filters the current column, if present. 
        ///// </summary>
        ///// <param name="filter">The filter string to parse.</param>
        ///// <returns>A copy of the specified filter string without the current column's filter.</returns>
        //private String FilterWithoutCurrentColumn(String filter)
        //{
        //    // If there is no filter in effect, return String.Empty. 
        //    if (String.IsNullOrEmpty(filter))
        //    {
        //        return String.Empty;
        //    }

        //    // If the column is not filtered, return the filter string unchanged. 
        //    if (!filtered)
        //    {
        //        return filter;
        //    }

        //    if (filter.IndexOf(currentColumnFilter) > 0)
        //    {
        //        // If the current column filter is not the first filter, return
        //        // the specified filter value without the current column filter 
        //        // and without the preceding " AND ". 
        //        return filter.Replace(
        //            " AND " + currentColumnFilter, String.Empty);
        //    }
        //    else
        //    {
        //        if (filter.Length > currentColumnFilter.Length)
        //        {
        //            // If the current column filter is the first of multiple 
        //            // filters, return the specified filter value without the 
        //            // current column filter and without the subsequent " AND ". 
        //            return filter.Replace(
        //                currentColumnFilter + " AND ", String.Empty);
        //        }
        //        else
        //        {
        //            // If the current column filter is the only filter, 
        //            // return the empty string.
        //            return String.Empty;
        //        }
        //    }
        //}

        /// <summary>
        /// Updates the BindingSource.Filter value based on a user selection
        /// from the drop-down filter list. 
        /// </summary>
        override protected void UpdateFilter(Boolean onlyRefresh = false)
        {
            if(!onlyRefresh)
            {
                // Continue only if the selection has changed.
                if ((filterWindow.cmbConstraint.SelectedIndex == (Int32)m_SelectedConstraintIndex) &&
                   (filterWindow.txtFilterText.Text.Equals(m_SelectedFilterText, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return;
                }

                // Store the new filter value
                m_SelectedConstraintIndex = (ConstraintValues)filterWindow.cmbConstraint.SelectedIndex;
                m_SelectedFilterText = filterWindow.txtFilterText.Text;
            }

            // Cast the data source to an IBindingListView.
            IBindingListView data =
                this.DataGridView.DataSource as IBindingListView;

            if((data == null) && (Retriever == null))
                return;

            Debug.Assert((data != null && data.SupportsFiltering) || (Retriever != null),
                "DataSource is not an IBindingListView or does not support filtering");

            if (data != null)
            {
                throw new NotImplementedException();
                // If the user selection is (All), remove any filter currently 
                // in effect for the column. 
                //if(selectedFilterValue.Count == filters.Count)
                //{
                //    //data.Filter = FilterWithoutCurrentColumn(data.Filter);
                //    filtered = false;
                //    //currentColumnFilter.Clear();
                //    return;
                //}

                // Declare a variable to store the filter string for this column.
                //List<String> newColumnFilter = new List<string>();

                // Store the column name in a form acceptable to the Filter property, 
                // using a backslash to escape any closing square brackets. 
                String columnProperty =
                    OwningColumn.DataPropertyName.Replace("]", @"\]");

                // Determine the column filter string based on the user selection.
                // For (Blanks) and (NonBlanks), the filter string determines whether
                // the column value is null or an empty string. Otherwise, the filter
                // string determines whether the column value is the selected value. 
                //switch (selectedFilterValue)
                //{
                //    case "(Blanks)":
                //        newColumnFilter = String.Format(
                //            "LEN(ISNULL(CONVERT([{0}],'System.String'),''))=0",
                //            columnProperty);
                //        break;
                //    case "(NonBlanks)":
                //        newColumnFilter = String.Format(
                //            "LEN(ISNULL(CONVERT([{0}],'System.String'),''))>0",
                //            columnProperty);
                //        break;
                //    default:
                //newColumnFilter = String.Format("[{0}]='{1}'",
                //    columnProperty,
                //    ((String)filters[selectedFilterValue])
                //    .Replace("'", "''"));  
                //        break;
                //}

                // Determine the new filter string by removing the previous column 
                // filter string from the BindingSource.Filter value, then appending 
                // the new column filter string, using " AND " as appropriate. 
                //String newFilter = FilterWithoutCurrentColumn(data.Filter);
                //if (String.IsNullOrEmpty(newFilter))
                //{
                //    newFilter += newColumnFilter;
                //}
                //else
                //{
                //    newFilter += " AND " + newColumnFilter;
                //}


                // Set the filter to the new value.
                try
                {
                    //data.Filter = newFilter;
                }
                catch (InvalidExpressionException ex)
                {
                    //throw new NotSupportedException(
                    //"Invalid expression: " + newFilter, ex);
                }

                // Indicate that the column is currently filtered
                // and store the new column filter for use by subsequent
                // calls to the FilterWithoutCurrentColumn method. 
                filtered = true;
                //currentColumnFilter = newColumnFilter;
            }
            else
            {

                StringBuilder filterString = new StringBuilder();

                switch (m_SelectedConstraintIndex)
                {
                    case ConstraintValues.cv_filter_off:
                        // leave the string empty
                        filtered = false;
                        break;

                    case ConstraintValues.cv_equals:
                        filterString.AppendFormat("({0} = {1})", this.OwningColumn.DataPropertyName, IBE.SQL.DBConnector.SQLAEscape(m_SelectedFilterText));
                        filtered = true;

                        break;
                    case ConstraintValues.cv_contains:
                        filterString.AppendFormat("({0} like '%{1}%')", this.OwningColumn.DataPropertyName, IBE.SQL.DBConnector.SQLEscape(m_SelectedFilterText));
                        filtered = true;

                        break;
                    case ConstraintValues.cv_starts_with:
                        filterString.AppendFormat("({0} like '{1}%')", this.OwningColumn.DataPropertyName, IBE.SQL.DBConnector.SQLEscape(m_SelectedFilterText));
                        filtered = true;

                        break;
                    case ConstraintValues.cv_ends_with:
                        filterString.AppendFormat("({0} like '%{1}')", this.OwningColumn.DataPropertyName, IBE.SQL.DBConnector.SQLEscape(m_SelectedFilterText));
                        filtered = true;

                        break;
                }

                Retriever.SetFilter(this.OwningColumn.Name, filterString.ToString());
            }
        }

        /// <summary>
        /// Removes the filter from the BindingSource bound to the specified DataGridView. 
        /// </summary>
        /// <param name="dataGridView">The DataGridView bound to the BindingSource to unfilter.</param>
        public static void RemoveFilter(DataGridView dataGridView)
        {
            if (dataGridView == null)
            {
                throw new ArgumentNullException("dataGridView");
            }

            // Cast the data source to a BindingSource.
            BindingSource data = dataGridView.DataSource as BindingSource;

            // Confirm that the data source is a BindingSource that 
            // supports filtering.
            if (data == null ||
                data.DataSource == null ||
                !data.SupportsFiltering)
            {
                throw new ArgumentException("The DataSource property of the " +
                    "specified DataGridView is not set to a BindingSource " +
                    "with a SupportsFiltering property value of true.");
            }

            // Ensure that the current row is not the row for new records.
            // This prevents the new row from being added when the filter changes.
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.IsNewRow)
            {
                dataGridView.CurrentCell = null;
            }

            // Remove the filter. 
            data.Filter = null;
        }

        /// <summary>
        /// Gets a status string for the specified DataGridView indicating the 
        /// number of visible rows in the bound, filtered BindingSource, or 
        /// String.Empty if all rows are currently visible. 
        /// </summary>
        /// <param name="dataGridView">The DataGridView bound to the 
        /// BindingSource to return the filter status for.</param>
        /// <returns>A string in the format "x of y records found" where x is 
        /// the number of rows currently displayed and y is the number of rows 
        /// available, or String.Empty if all rows are currently displayed.</returns>
        public static String GetFilterStatus(DataGridView dataGridView)
        {
            // Continue only if the specified value is valid. 
            if (dataGridView == null)
            {
                throw new ArgumentNullException("dataGridView");
            }

            // Cast the data source to a BindingSource.
            BindingSource data = dataGridView.DataSource as BindingSource;

            // Return String.Empty if there is no appropriate data source or
            // there is no filter in effect. 
            if (String.IsNullOrEmpty(data.Filter) ||
                data == null ||
                data.DataSource == null ||
                !data.SupportsFiltering)
            {
                return String.Empty;
            }

            // Retrieve the filtered row count. 
            Int32 currentRowCount = data.Count;

            // Retrieve the unfiltered row count by 
            // temporarily unfiltering the data.
            data.RaiseListChangedEvents = false;
            String oldFilter = data.Filter;
            data.Filter = null;
            Int32 unfilteredRowCount = data.Count;
            data.Filter = oldFilter;
            data.RaiseListChangedEvents = true;

            Debug.Assert(currentRowCount <= unfilteredRowCount,
                "current count is greater than unfiltered count");

            // Return String.Empty if the filtered and unfiltered counts
            // are the same, otherwise, return the status string. 
            if (currentRowCount == unfilteredRowCount)
            {
                return String.Empty;
            }
            return String.Format("{0} of {1} records found",
                currentRowCount, unfilteredRowCount);
        }

        #endregion filtering

    }

}

