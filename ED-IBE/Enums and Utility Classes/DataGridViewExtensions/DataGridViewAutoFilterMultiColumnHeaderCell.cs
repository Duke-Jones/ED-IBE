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
    public class DataGridViewAutoFilterMultiColumnHeaderCell : DataGridViewAutoFilterHeaderCell
    {
        /// <summary>
        /// The ListBox used for all drop-down lists. 
        /// </summary>
        private static MultiSelectHeaderList filterWindow;

        /// <summary>
        /// A list of filters available for the owning column stored as 
        /// formatted and unformatted string values. 
        /// </summary>
        private System.Collections.Specialized.OrderedDictionary filters =
            new System.Collections.Specialized.OrderedDictionary();

        /// <summary>
        /// The drop-down list filter value currently in effect for the owning column. 
        /// </summary>
        private List<string> selectedFilterValue = new List<string>();

        /// <summary>
        /// The complete filter string currently in effect for the owning column. 
        /// </summary>
        private List<String> currentColumnFilter = new List<String>();

        List<String> m_FilterValues = new List<string>();

         /// <summary>
         /// Initializes a new instance of the DataGridViewColumnHeaderCell 
         /// class and sets its property values to the property values of the 
         /// specified DataGridViewColumnHeaderCell.
         /// </summary>
         /// <param name="oldHeaderCell">The DataGridViewColumnHeaderCell to copy property values from.</param>
         public DataGridViewAutoFilterMultiColumnHeaderCell(DataGridViewColumnHeaderCell oldHeaderCell) : base(oldHeaderCell)
         {
             this.ContextMenuStrip = oldHeaderCell.ContextMenuStrip;
             this.ErrorText = oldHeaderCell.ErrorText;
             this.Tag = oldHeaderCell.Tag;
             this.ToolTipText = oldHeaderCell.ToolTipText;
             this.Value = oldHeaderCell.Value;
             this.ValueType = oldHeaderCell.ValueType;

             // Use HasStyle to avoid creating a new style object
             // when the Style property has not previously been set. 
             if (oldHeaderCell.HasStyle)
             {
                 this.Style = oldHeaderCell.Style;
             }

             // Copy this type's properties if the old cell is an auto-filter cell. 
             // This enables the Clone method to reuse this constructor. 
             DataGridViewAutoFilterMultiColumnHeaderCell filterCell =
                 oldHeaderCell as DataGridViewAutoFilterMultiColumnHeaderCell;
             if (filterCell != null)
             {
                 this.FilteringEnabled = filterCell.FilteringEnabled;
                 this.AutomaticSortingEnabled = filterCell.AutomaticSortingEnabled;
                 this.DropDownListBoxMaxLines = filterCell.DropDownListBoxMaxLines;
                 this.currentDropDownButtonPaddingOffset = 
                     filterCell.currentDropDownButtonPaddingOffset;
             }
         }

         /// <summary>
         /// Initializes a new instance of the DataGridViewColumnHeaderCell 
         /// class. 
         /// </summary>
         public DataGridViewAutoFilterMultiColumnHeaderCell() : base()
         {
         }

        /// <summary>
        /// Creates an exact copy of this cell.
        /// </summary>
        /// <returns>An object that represents the cloned DataGridViewAutoFilterMultiColumnHeaderCell.</returns>
        public override object Clone()
        {
            return new DataGridViewAutoFilterMultiColumnHeaderCell(this);
        }

        /// <summary>
        /// Resets the cached filter values if the filter has been removed.
        /// </summary>
        override protected void ResetFilter()
        {
            if (this.DataGridView == null) return;
            BindingSource source = this.DataGridView.DataSource as BindingSource;
            if (source == null || String.IsNullOrEmpty(source.Filter))
            {
                filtered = false;
                selectedFilterValue.Clear();
                currentColumnFilter.Clear();
            }
        }

        #region drop-down list: Show/HideDropDownListBox, SetDropDownListBoxBounds, DropDownListBoxMaxHeightInternal

        /// <summary>
        /// Displays the drop-down filter list. 
        /// </summary>
        override public void ShowDropDownList()
        {
            try
            {
                if(filterControlShowing)
                {
                    HideFilterControl();
                    return;
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

                // Populate the filters dictionary, then copy the filter values 
                // from the filters.Keys collection into the ListBox.Items collection, 
                // selecting the current filter if there is one in effect. 
                PopulateFilters();

                String[] filterArray = new String[filters.Count];
                filters.Keys.CopyTo(filterArray, 0);

                if(selectedFilterValue.Count == 0)
                    selectedFilterValue.AddRange(filterArray);

                filterWindow = new MultiSelectHeaderList();
                filterWindow.FilterListBox.Items.Clear();
                filterWindow.FilterListBox.Items.AddRange(filterArray);
                filterWindow.SelectedValues = selectedFilterValue;

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
            }
        }

        /// <summary>
        /// Sets the dropDownListBox.FilterListBox size and position based on the formatted 
        /// values in the filters dictionary and the position of the drop-down 
        /// button. Called only by ShowDropDownListBox.  
        /// </summary>
        private void SetDropDownListBoxBounds()
        {
            Debug.Assert(filters.Count > 0, "filters.Count <= 0");

            // Declare variables that will be used in the calculation, 
            // initializing dropDownListBoxHeight to account for the 
            // ListBox borders.
            Int32 dropDownListBoxHeight = 2;
            Int32 currentWidth = 0;
            Int32 dropDownListBoxWidth = 0;
            Int32 dropDownListBoxLeft = 0;

            // For each formatted value in the filters dictionary Keys collection,
            // add its height to dropDownListBoxHeight and, if it is wider than 
            // all previous values, set dropDownListBoxWidth to its width.
            using (Graphics graphics = filterWindow.FilterListBox.CreateGraphics())
            {
                foreach (String filter in filters.Keys)
                {
                    SizeF stringSizeF = graphics.MeasureString(
                        filter, filterWindow.FilterListBox.Font);
                    dropDownListBoxHeight += (Int32)stringSizeF.Height;
                    currentWidth = (Int32)stringSizeF.Width;
                    if (dropDownListBoxWidth < currentWidth)
                    {
                        dropDownListBoxWidth = currentWidth;
                    }
                }
                dropDownListBoxWidth += (Int32)(CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.CheckedNormal).Width * 2.0);
            }

            // Increase the width to allow for horizontal margins and borders. 
            dropDownListBoxWidth += 6;

            // Increase the width and height to take care of the width/height difference between CheckedListBox and UserControl 
            dropDownListBoxWidth += 6;
            dropDownListBoxHeight += 56;

            // Constrain the dropDownListBox.FilterListBox height to the 
            // DropDownListBoxMaxHeightInternal value, which is based on 
            // the DropDownListBoxMaxLines property value but constrained by
            // the maximum height available in the DataGridView control.
            if (dropDownListBoxHeight > DropDownListBoxMaxHeightInternal)
            {
                dropDownListBoxHeight = DropDownListBoxMaxHeightInternal;

                // If the preferred height is greater than the available height,
                // adjust the width to accommodate the vertical scroll bar. 
                dropDownListBoxWidth += SystemInformation.VerticalScrollBarWidth;
            }

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
            if (this.DataGridView.DisplayedRowCount(false) < 
                this.DataGridView.RowCount)
            {
                if (this.DataGridView.RightToLeft == RightToLeft.Yes)
                {
                    clientLeft += SystemInformation.VerticalScrollBarWidth;
                }
                else
                {
                    clientRight -= SystemInformation.VerticalScrollBarWidth;
                }
            }

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

        /// <summary>
        /// Gets the actual maximum height of the drop-down list, in pixels.
        /// The maximum height is calculated from the DropDownListBoxMaxLines 
        /// property value, but is limited to the available height of the 
        /// DataGridView control. 
        /// </summary>
        protected Int32 DropDownListBoxMaxHeightInternal
        {
            get
            {
                // Calculate the height of the available client area
                // in the DataGridView control, taking the horizontal
                // scroll bar into consideration and leaving room
                // for the ListBox bottom border. 
                Int32 dataGridViewMaxHeight = this.DataGridView.Height -
                    this.DataGridView.ColumnHeadersHeight - 1;
                if (this.DataGridView.DisplayedColumnCount(false) <
                    this.DataGridView.ColumnCount)
                {
                    dataGridViewMaxHeight -=
                        SystemInformation.HorizontalScrollBarHeight;
                }

                // Calculate the height of the list box, using the combined 
                // height of all items plus 2 for the top and bottom border. 
                Int32 listMaxHeight = dropDownListBoxMaxLinesValue * filterWindow.FilterListBox.ItemHeight + 2;

                // Return the smaller of the two values. 
                if (listMaxHeight < dataGridViewMaxHeight)
                {
                    return listMaxHeight;
                }
                else
                {
                    return dataGridViewMaxHeight;
                }
            }
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
            filterWindow.Deactivate += FilterWindow_Deactivate;
            filterWindow.KeyDown += new KeyEventHandler(DropDownListBox_KeyDown);
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
                UpdateFilter();
                HideFilterControl();

                RefreshDGV(0);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while confirming the column filter setting (" + this.OwningColumn.Name + ")");
            }
        }

        #endregion ListBox events

        #region filtering: PopulateFilters, FilterWithoutCurrentColumn, UpdateFilter, RemoveFilter, AvoidNewRowWhenFiltering, GetFilterStatus

        /// <summary>
        /// Populates the filters dictionary with formatted and unformatted string
        /// representations of each unique value in the column, accounting for all 
        /// filters except the current column's. Also adds special filter options. 
        /// </summary>
        private void PopulateFilters()
        {
            ArrayList list = null;
            Boolean containsBlanks;
            Boolean containsNonBlanks;
            String oldFilter = "";

            // Continue only if there is a DataGridView.
            if (this.DataGridView == null)
            {
                return;
            }

            // Cast the data source to a BindingSource. 
            BindingSource data = this.DataGridView.DataSource as BindingSource;

            Debug.Assert((data != null && data.SupportsFiltering && OwningColumn != null) || (Retriever != null),
                "DataSource is not a BindingSource, or does not support filtering, or OwningColumn is null");

            containsBlanks = false;
            containsNonBlanks = false;

            // Reset the filters dictionary and initialize some flags
            // to track whether special filter options are needed. 
            filters.Clear();

            if (data != null)
            {
                // Prevent the data source from notifying the DataGridView of changes. 
                data.RaiseListChangedEvents = false;

                // Cache the current BindingSource.Filter value and then change 
                // the Filter property to temporarily remove any filter for the 
                // current column. 

                oldFilter = data.Filter;
                //data.Filter = FilterWithoutCurrentColumn(oldFilter);

                // Initialize an ArrayList to store the values in their original
                // types. This enables the values to be sorted appropriately.  
                list = new ArrayList(data.Count);

                // Retrieve each value and add it to the ArrayList if it isn't
                // already present. 
                foreach (Object item in data)
                {
                    Object value = null;

                    // Use the ICustomTypeDescriptor interface to retrieve properties
                    // if it is available; otherwise, use reflection. The 
                    // ICustomTypeDescriptor interface is useful to customize 
                    // which values are exposed as properties. For example, the 
                    // DataRowView class implements ICustomTypeDescriptor to expose 
                    // cell values as property values.                
                    // 
                    // Iterate through the property names to find a case-insensitive
                    // match with the DataGridViewColumn.DataPropertyName value.
                    // This is necessary because DataPropertyName is case-
                    // insensitive, but the GetProperties and GetProperty methods
                    // used below are case-sensitive.
                    ICustomTypeDescriptor ictd = item as ICustomTypeDescriptor;
                    if (ictd != null)
                    {
                        PropertyDescriptorCollection properties = ictd.GetProperties();
                        foreach (PropertyDescriptor property in properties)
                        {
                            if (String.Compare(this.OwningColumn.DataPropertyName,
                                property.Name, true /*case insensitive*/,
                                System.Globalization.CultureInfo.InvariantCulture) == 0)
                            {
                                value = property.GetValue(item);
                                break;
                            }
                        }
                    }
                    else
                    {
                        PropertyInfo[] properties = item.GetType().GetProperties(
                            BindingFlags.Public | BindingFlags.Instance);
                        foreach (PropertyInfo property in properties)
                        {
                            if (String.Compare(this.OwningColumn.DataPropertyName,
                                property.Name, true /*case insensitive*/,
                                System.Globalization.CultureInfo.InvariantCulture) == 0)
                            {
                                value = property.GetValue(item, null /*property index*/);
                                break;
                            }
                        }
                    }

                    // Skip empty values, but note that they are present. 
                    if (value == null || value == DBNull.Value)
                    {
                        containsBlanks = true;
                        continue;
                    }

                    // Add values to the ArrayList if they are not already there.
                    if (!list.Contains(value))
                    {
                        list.Add(value);
                    }
                }
            }
            else
            {
                // Initialize an ArrayList to store the values in their original
                // types. This enables the values to be sorted appropriately.  

                DataTable dataTable = new DataTable();

                IBE.Program.DBCon.Execute(String.Format("{0} {1}", RetrieverSQLSelect, Retriever.BaseStatement), dataTable);

                list = new ArrayList(dataTable.Rows.Count);

                foreach (DataRow selectableItem in dataTable.Rows)
                    list.Add(selectableItem[0]);
            }

            // Sort the ArrayList. The default Sort method uses the IComparable 
            // implementation of the stored values so that string, numeric, and 
            // date values will all be sorted correctly. 
            list.Sort();

            // Convert each value in the ArrayList to its formatted representation
            // and store both the formatted and unformatted string representations
            // in the filters dictionary. 
            foreach (Object value in list)
            {
                // Use the cell's GetFormattedValue method with the column's
                // InheritedStyle property so that the dropDownListBox.FilterListBox format
                // will match the display format used for the column's cells. 
                String formattedValue = null;
                DataGridViewCellStyle style = OwningColumn.InheritedStyle;
                formattedValue = (String)GetFormattedValue(value, -1, ref style,
                    null, null, DataGridViewDataErrorContexts.Formatting);

                if (String.IsNullOrEmpty(formattedValue))
                {
                    // Skip empty values, but note that they are present.
                    containsBlanks = true;
                }
                else if (!filters.Contains(formattedValue))
                {
                    // Note whether non-empty values are present. 
                    containsNonBlanks = true;

                    // For all non-empty values, add the formatted and 
                    // unformatted string representations to the filters 
                    // dictionary.
                    filters.Add(formattedValue, value.ToString());
                }
            }

            if (data != null)
            {
                // Restore the filter to the cached filter string and 
                // re-enable data source change notifications. 
                if (oldFilter != null) data.Filter = oldFilter;
                data.RaiseListChangedEvents = true;
            }

            // Add special filter options to the filters dictionary
            // along with null values, since unformatted representations
            // are not needed. 
            if (containsBlanks && containsNonBlanks)
            {
                filters.Add("(Blanks)", null);
                filters.Add("(NonBlanks)", null);
            }
        }

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
        override protected void UpdateFilter()
        {
            // Continue only if the selection has changed.
            if (filterWindow.SelectedValues.ContentEquals(selectedFilterValue))
            {
                return;
            }

            // Store the new selection value. 
            selectedFilterValue = filterWindow.SelectedValues;

            // Cast the data source to an IBindingListView.
            IBindingListView data =
                this.DataGridView.DataSource as IBindingListView;

            Debug.Assert((data != null && data.SupportsFiltering) || (Retriever != null),
                "DataSource is not an IBindingListView or does not support filtering");

            if (data != null)
            {
                // If the user selection is (All), remove any filter currently 
                // in effect for the column. 
                if (selectedFilterValue.Count == filters.Count)
                {
                    //data.Filter = FilterWithoutCurrentColumn(data.Filter);
                    filtered = false;
                    //currentColumnFilter.Clear();
                    return;
                }

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

                if ((selectedFilterValue.Count == filters.Count) || (selectedFilterValue.Count == 0))
                {
                    Retriever.SetFilter(this.OwningColumn.Name, "");
                    selectedFilterValue.Clear();
                    filtered = false;
                    return;
                }

                StringBuilder filterString = new StringBuilder();

                foreach (String filterValue in selectedFilterValue)
                {
                    if (filterString.Length != 0)
                        filterString.Append(" or ");

                    filterString.AppendFormat("({0} = {1})", this.OwningColumn.DataPropertyName, IBE.SQL.DBConnector.SQLAEscape(filterValue));
                }

                Retriever.SetFilter(this.OwningColumn.Name, filterString.ToString());

                filtered = true;
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

