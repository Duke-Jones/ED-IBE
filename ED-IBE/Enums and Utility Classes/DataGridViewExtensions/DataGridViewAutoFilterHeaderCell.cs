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

namespace DataGridViewAutoFilter
{
    /// <summary>
    /// Provides a drop-down filter list in a DataGridViewColumnHeaderCell.
    /// </summary>
    public class DataGridViewAutoFilterHeaderCell : DataGridViewColumnHeaderCell
    {

        /// <summary>
        /// Indicates whether the DataGridView is currently filtered by the owning column.  
        /// </summary>
        protected Boolean filtered;

        /// <summary>
        /// Initializes a new instance of the DataGridViewColumnHeaderCell 
        /// class and sets its property values to the property values of the 
        /// specified DataGridViewColumnHeaderCell.
        /// </summary>
        /// <param name="oldHeaderCell">The DataGridViewColumnHeaderCell to copy property values from.</param>
        public DataGridViewAutoFilterHeaderCell(DataGridViewColumnHeaderCell oldHeaderCell)
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
            DataGridViewAutoFilterHeaderCell filterCell =
                oldHeaderCell as DataGridViewAutoFilterHeaderCell;
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
        public DataGridViewAutoFilterHeaderCell()
        {
        }

        /// <summary>
        /// Creates an exact copy of this cell.
        /// </summary>
        /// <returns>An object that represents the cloned DataGridViewAutoFilterColumnHeaderCell.</returns>
        public override object Clone()
        {
            return new DataGridViewAutoFilterHeaderCell(this);
        }

        /// <summary>
        /// Called when the value of the DataGridView property changes
        /// in order to perform initialization that requires access to the 
        /// owning control and column. 
        /// </summary>
        protected override void OnDataGridViewChanged()
        {
            // Continue only if there is a DataGridView. 
            if (this.DataGridView == null)
            {
                return;
            }

            // Disable sorting and filtering for columns that can't make
            // effective use of them. 
            if (OwningColumn != null)
            {
                if (OwningColumn is DataGridViewImageColumn ||
                (OwningColumn is DataGridViewButtonColumn && 
                ((DataGridViewButtonColumn)OwningColumn).UseColumnTextForButtonValue) ||
                (OwningColumn is DataGridViewLinkColumn && 
                ((DataGridViewLinkColumn)OwningColumn).UseColumnTextForLinkValue))
                {
                    AutomaticSortingEnabled = false;
                    FilteringEnabled = false;
                }

                // Ensure that the column SortMode property value is not Automatic.
                // This prevents sorting when the user clicks the drop-down button.
                if (OwningColumn.SortMode == DataGridViewColumnSortMode.Automatic)
                {
                    OwningColumn.SortMode = DataGridViewColumnSortMode.Programmatic;
                }
            }

            // Confirm that the data source meets requirements. 
            VerifyDataSource();

            // Add handlers to DataGridView events. 
            HandleDataGridViewEvents();

            // Initialize the drop-down button bounds so that any initial
            // column autosizing will accommodate the button width. 
            SetDropDownButtonBounds();

            // Call the OnDataGridViewChanged method on the base class to 
            // raise the DataGridViewChanged event.
            base.OnDataGridViewChanged();
        }

        /// <summary>
        /// Confirms that the data source, if it has been set, is a BindingSource.
        /// </summary>
        protected void VerifyDataSource()
        {
            // Continue only if there is a DataGridView and its DataSource has been set.
            if (this.DataGridView == null || this.DataGridView.DataSource == null)
            {
                return;
            }

            // Throw an exception if the data source is not a BindingSource. 
            BindingSource data = this.DataGridView.DataSource as BindingSource;
            if (data == null)
            {
                throw new NotSupportedException(
                    "The DataSource property of the containing DataGridView control " +
                    "must be set to a BindingSource.");
            }
        }

        #region DataGridView events: HandleDataGridViewEvents, DataGridView event handlers, ResetDropDown, ResetFilter

        /// <summary>
        /// Add handlers to various DataGridView events, primarily to invalidate 
        /// the drop-down button bounds, hide the drop-down list, and reset 
        /// cached filter values when changes in the DataGridView require it.
        /// </summary>
        protected void HandleDataGridViewEvents()
        {
            this.DataGridView.Scroll += new ScrollEventHandler(DataGridView_Scroll);
            this.DataGridView.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(DataGridView_ColumnDisplayIndexChanged);
            this.DataGridView.ColumnWidthChanged += new DataGridViewColumnEventHandler(DataGridView_ColumnWidthChanged);
            this.DataGridView.ColumnHeadersHeightChanged += new EventHandler(DataGridView_ColumnHeadersHeightChanged);
            this.DataGridView.SizeChanged += new EventHandler(DataGridView_SizeChanged);
            this.DataGridView.DataSourceChanged += new EventHandler(DataGridView_DataSourceChanged);
            this.DataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(DataGridView_DataBindingComplete);

            // Add a handler for the ColumnSortModeChanged event to prevent the
            // column SortMode property from being inadvertently set to Automatic.
            this.DataGridView.ColumnSortModeChanged += new DataGridViewColumnEventHandler(DataGridView_ColumnSortModeChanged);
        }

        /// <summary>
        /// Invalidates the drop-down button bounds when the user scrolls horizontally.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">A ScrollEventArgs that contains the event data.</param>
        protected void DataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                ResetDropDown();
            }
        }

        /// <summary>
        /// Invalidates the drop-down button bounds when the column display index changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DataGridView_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ResetDropDown();
        }

        /// <summary>
        /// Invalidates the drop-down button bounds when a column width changes
        /// in the DataGridView control. A width change in any column of the 
        /// control has the potential to affect the drop-down button location, 
        /// depending on the current horizontal scrolling position and whether
        /// the changed column is to the left or right of the current column. 
        /// It is easier to invalidate the button in all cases. 
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">A DataGridViewColumnEventArgs that contains the event data.</param>
        protected void DataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ResetDropDown();
        }

        /// <summary>
        /// Invalidates the drop-down button bounds when the height of the column headers changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected void DataGridView_ColumnHeadersHeightChanged(object sender, EventArgs e)
        {
            ResetDropDown();
        }

        /// <summary>
        /// Invalidates the drop-down button bounds when the size of the DataGridView changes.
        /// This prevents a painting issue that occurs when the right edge of the control moves 
        /// to the right and the control contents have previously been scrolled to the right.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected void DataGridView_SizeChanged(object sender, EventArgs e)
        {
            ResetDropDown();
        }

        /// <summary>
        /// Invalidates the drop-down button bounds, hides the drop-down 
        /// filter list, if it is showing, and resets the cached filter values
        /// if the filter has been removed. 
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">A DataGridViewBindingCompleteEventArgs that contains the event data.</param>
        protected void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.Reset)
            {
                ResetDropDown();
                ResetFilter();
            }
        }

        /// <summary>
        /// Verifies that the data source meets requirements, invalidates the 
        /// drop-down button bounds, hides the drop-down filter list if it is 
        /// showing, and resets the cached filter values if the filter has been removed. 
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected void DataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            VerifyDataSource();
            ResetDropDown();
            ResetFilter();
        }

        /// <summary>
        /// Invalidates the drop-down button bounds and hides the filter
        /// list if it is showing.
        /// </summary>
        protected void ResetDropDown()
        {
            InvalidateDropDownButtonBounds();
            if (filterControlShowing)
            {
                HideFilterControl();
            }
        }

        /// <summary>
        /// Resets the cached filter values if the filter has been removed.
        /// </summary>
        protected virtual void ResetFilter()
        {
            throw new NotImplementedException("ResetFilter() missing");
        }

        /// <summary>
        /// Throws an exception when the column sort mode is changed to Automatic.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">A DataGridViewColumnEventArgs that contains the event data.</param>
        protected void DataGridView_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column == OwningColumn &&
                e.Column.SortMode == DataGridViewColumnSortMode.Automatic)
            {
                throw new InvalidOperationException(
                    "A SortMode value of Automatic is incompatible with " +
                    "the DataGridViewAutoFilterColumnHeaderCell type. " +
                    "Use the AutomaticSortingEnabled property instead.");
            }
        }

        #endregion DataGridView events

        /// <summary>
        /// Paints the column header cell, including the drop-down button. 
        /// </summary>
        /// <param name="graphics">The Graphics used to paint the DataGridViewCell.</param>
        /// <param name="clipBounds">A Rectangle that represents the area of the DataGridView that needs to be repainted.</param>
        /// <param name="cellBounds">A Rectangle that contains the bounds of the DataGridViewCell that is being painted.</param>
        /// <param name="rowIndex">The row index of the cell that is being painted.</param>
        /// <param name="cellState">A bitwise combination of DataGridViewElementStates values that specifies the state of the cell.</param>
        /// <param name="value">The data of the DataGridViewCell that is being painted.</param>
        /// <param name="formattedValue">The formatted data of the DataGridViewCell that is being painted.</param>
        /// <param name="errorText">An error message that is associated with the cell.</param>
        /// <param name="cellStyle">A DataGridViewCellStyle that contains formatting and style information about the cell.</param>
        /// <param name="advancedBorderStyle">A DataGridViewAdvancedBorderStyle that contains border styles for the cell that is being painted.</param>
        /// <param name="paintParts">A bitwise combination of the DataGridViewPaintParts values that specifies which parts of the cell need to be painted.</param>
        protected override void Paint(
            Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, 
            int rowIndex, DataGridViewElementStates cellState, 
            object value, object formattedValue, string errorText, 
            DataGridViewCellStyle cellStyle, 
            DataGridViewAdvancedBorderStyle advancedBorderStyle, 
            DataGridViewPaintParts paintParts)
        {
            // Use the base method to paint the default appearance. 
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, 
                cellState, value, formattedValue, 
                errorText, cellStyle, advancedBorderStyle, paintParts);

            // Continue only if filtering is enabled and ContentBackground is 
            // part of the paint request. 
            if (!FilteringEnabled || 
                (paintParts & DataGridViewPaintParts.ContentBackground) == 0)
            {
                return;
            }

            // Retrieve the current button bounds. 
            Rectangle buttonBounds = DropDownButtonBounds;

            // Continue only if the buttonBounds is big enough to draw.
            if (buttonBounds.Width < 1 || buttonBounds.Height < 1) return;

            // Paint the button manually or using visual styles if visual styles 
            // are enabled, using the correct state depending on whether the 
            // filter list is showing and whether there is a filter in effect 
            // for the current column. 
            if (Application.RenderWithVisualStyles)
            {
                ComboBoxState state = ComboBoxState.Normal;

                if (filterControlShowing)
                {
                    state = ComboBoxState.Pressed;
                }
                else if (filtered)
                {
                    state = ComboBoxState.Hot;
                }
                ComboBoxRenderer.DrawDropDownButton(
                    graphics, buttonBounds, state);
            }
            else
            {
                // Determine the pressed state in order to paint the button 
                // correctly and to offset the down arrow. 
                Int32 pressedOffset = 0;
                PushButtonState state = PushButtonState.Normal;
                if (filterControlShowing)
                {
                    state = PushButtonState.Pressed;
                    pressedOffset = 1;
                }
                ButtonRenderer.DrawButton(graphics, buttonBounds, state);

                // If there is a filter in effect for the column, paint the 
                // down arrow as an unfilled triangle. If there is no filter 
                // in effect, paint the down arrow as a filled triangle.
                if (filtered)
                {
                    graphics.DrawPolygon(SystemPens.ControlText, new Point[] {
                        new Point(
                            buttonBounds.Width / 2 + 
                                buttonBounds.Left - 1 + pressedOffset, 
                            buttonBounds.Height * 3 / 4 + 
                                buttonBounds.Top - 1 + pressedOffset),
                        new Point(
                            buttonBounds.Width / 4 + 
                                buttonBounds.Left + pressedOffset,
                            buttonBounds.Height / 2 + 
                                buttonBounds.Top - 1 + pressedOffset),
                        new Point(
                            buttonBounds.Width * 3 / 4 + 
                                buttonBounds.Left - 1 + pressedOffset,
                            buttonBounds.Height / 2 + 
                                buttonBounds.Top - 1 + pressedOffset)
                    });
                }
                else
                {
                    graphics.FillPolygon(SystemBrushes.ControlText, new Point[] {
                        new Point(
                            buttonBounds.Width / 2 + 
                                buttonBounds.Left - 1 + pressedOffset, 
                            buttonBounds.Height * 3 / 4 + 
                                buttonBounds.Top - 1 + pressedOffset),
                        new Point(
                            buttonBounds.Width / 4 + 
                                buttonBounds.Left + pressedOffset,
                            buttonBounds.Height / 2 + 
                                buttonBounds.Top - 1 + pressedOffset),
                        new Point(
                            buttonBounds.Width * 3 / 4 + 
                                buttonBounds.Left - 1 + pressedOffset,
                            buttonBounds.Height / 2 + 
                                buttonBounds.Top - 1 + pressedOffset)
                    });
                }
            }

        }

        /// <summary>
        /// Handles mouse clicks to the header cell, displaying the 
        /// drop-down list or sorting the owning column as appropriate. 
        /// </summary>
        /// <param name="e">A DataGridViewCellMouseEventArgs that contains the event data.</param>
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            Debug.Assert(this.DataGridView != null, "DataGridView is null");

            // Continue only if the user did not click the drop-down button 
            // while the drop-down list was displayed. This prevents the 
            // drop-down list from being redisplayed after being hidden in 
            // the LostFocus event handler. 
            if (lostFocusOnDropDownButtonClick)
            {
                lostFocusOnDropDownButtonClick = false;
                return;
            }

            // Retrieve the current size and location of the header cell,
            // excluding any portion that is scrolled off screen. 
            Rectangle cellBounds = this.DataGridView
                .GetCellDisplayRectangle(e.ColumnIndex, -1, false);

            // Continue only if the column is not manually resizable or the
            // mouse coordinates are not within the column resize zone. 
            if (this.OwningColumn.Resizable == DataGridViewTriState.True &&
                ((this.DataGridView.RightToLeft == RightToLeft.No &&
                cellBounds.Width - e.X < 6) || e.X < 6))
            {
                return;
            }

            // Unless RightToLeft is enabled, store the width of the portion
            // that is scrolled off screen. 
            Int32 scrollingOffset = 0;
            if (this.DataGridView.RightToLeft == RightToLeft.No &&
                this.DataGridView.FirstDisplayedScrollingColumnIndex ==
                this.ColumnIndex)
            {
                scrollingOffset =
                    this.DataGridView.FirstDisplayedScrollingColumnHiddenWidth;
            }

            // Show the drop-down list if filtering is enabled and the mouse click occurred
            // within the drop-down button bounds. Otherwise, if sorting is enabled and the
            // click occurred outside the drop-down button bounds, sort by the owning column. 
            // The mouse coordinates are relative to the cell bounds, so the cell location
            // and the scrolling offset are needed to determine the client coordinates.
            if (FilteringEnabled &&
                DropDownButtonBounds.Contains(
                e.X + cellBounds.Left - scrollingOffset, e.Y + cellBounds.Top))
            {
                // If the current cell is in edit mode, commit the edit. 
                if (this.DataGridView.IsCurrentCellInEditMode)
                {
                    // Commit and end the cell edit.  
                    this.DataGridView.EndEdit();

                    // Commit any change to the underlying data source. 
                    BindingSource source =
                        this.DataGridView.DataSource as BindingSource;
                    if (source != null)
                    {
                        source.EndEdit();
                    }
                }
                ShowDropDownList();
            }
            else if (AutomaticSortingEnabled &&
                this.DataGridView.SelectionMode != 
                DataGridViewSelectionMode.ColumnHeaderSelect)
            {
                SortByColumn();
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Sorts the DataGridView by the current column if AutomaticSortingEnabled is true.
        /// </summary>
        protected void SortByColumn()
        {
            Debug.Assert(this.DataGridView != null && OwningColumn != null, "DataGridView or OwningColumn is null");

            // Continue only if the data source supports sorting. 
            IBindingList sortList = this.DataGridView.DataSource as IBindingList;
            if (sortList == null ||
                !sortList.SupportsSorting ||
                !AutomaticSortingEnabled)
            {
                return;
            }

            // Determine the sort direction and sort by the owning column. 
            ListSortDirection direction = ListSortDirection.Ascending;
            if (this.DataGridView.SortedColumn == OwningColumn && 
                this.DataGridView.SortOrder == SortOrder.Ascending)
            {
                direction = ListSortDirection.Descending;
            }
            this.DataGridView.Sort(OwningColumn, direction);
        }

        #region drop-down list: Show/HideFilterControlBox, SetDropDownListBoxBounds, DropDownListBoxMaxHeightInternal

        /// <summary>
        /// Indicates whether dropDownListBox is currently displayed 
        /// for this header cell. 
        /// </summary>
        protected bool filterControlShowing;

        #endregion drop-down list

        #region ListBox events: HandleDropDownListBoxEvents, UnhandleDropDownListBoxEvents, ListBox event handlers


        /// <summary>
        /// forces refreshing this tab
        /// </summary>
        protected void RefreshDGV(int? currentRow)
        {
            if(Retriever != null)
            {
                // force refresh
                Retriever.MemoryCache.Clear();
                this.DataGridView.RowCount  = Retriever.RowCount(true);
                this.DataGridView.Invalidate();
                    

                // jump to the new row
                if ((currentRow != null) && (this.DataGridView.RowCount > currentRow))
                //try
                //{
                        this.DataGridView.CurrentCell = this.DataGridView[1, currentRow.Value];
                //}
                //catch{}
            }
        }

        /// <summary>
        /// Indicates whether the drop-down list lost focus because the
        /// user clicked the drop-down button. 
        /// </summary>
        private Boolean lostFocusOnDropDownButtonClick;

        /// <summary>
        /// Hides the drop-down list when it loses focus. 
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected void DropDownListBox_LostFocus(object sender, EventArgs e)
        {
            // If the focus was lost because the user clicked the drop-down
            // button, store a value that prevents the subsequent OnMouseDown
            // call from displaying the drop-down list again. 
            if (DropDownButtonBounds.Contains(
                this.DataGridView.PointToClient(new Point(
                Control.MousePosition.X, Control.MousePosition.Y))))
            {
                lostFocusOnDropDownButtonClick = true;
            }
            HideFilterControl();
        }

        protected void FilterWindow_Deactivate(object sender, EventArgs e)
        {
            // If the focus was lost because the user clicked the drop-down
            // button, store a value that prevents the subsequent OnMouseDown
            // call from displaying the drop-down list again. 
            if (DropDownButtonBounds.Contains(
                this.DataGridView.PointToClient(new Point(
                Control.MousePosition.X, Control.MousePosition.Y))))
            {
                lostFocusOnDropDownButtonClick = true;
            }
            HideFilterControl();
        }

        /// <summary>
        /// Handles the ENTER and ESC keys.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">A KeyEventArgs that contains the event data.</param>
        protected void DropDownListBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    UpdateFilter();
                    HideFilterControl();
                    RefreshDGV(0);
                    break;
                case Keys.Escape:
                    HideFilterControl();
                    break;
            }
        }

        /// <summary>
        /// Updates the BindingSource.Filter value based on a user selection
        /// from the drop-down filter list. 
        /// </summary>
        virtual protected void UpdateFilter()
        {
            throw new NotImplementedException("UpdateFilter() missing");
        }

        /// <summary>
        /// Displays the drop-down filter list. 
        /// </summary>
        virtual public void ShowDropDownList()
        {
            throw new NotImplementedException("ShowDropDownList() missing");
        }

        /// <summary>
        /// Hides the drop-down filter list. 
        /// </summary>
        virtual protected void HideFilterControl()
        {
            throw new NotImplementedException("HideFilterControl() missing");
        }

        #endregion ListBox events

        #region button bounds: DropDownButtonBounds, InvalidateDropDownButtonBounds, SetDropDownButtonBounds, AdjustPadding

        /// <summary>
        /// The bounds of the drop-down button, or Rectangle.Empty if filtering 
        /// is disabled or the button bounds need to be recalculated. 
        /// </summary>
        private Rectangle dropDownButtonBoundsValue = Rectangle.Empty;

        /// <summary>
        /// The bounds of the drop-down button, or Rectangle.Empty if filtering
        /// is disabled. Recalculates the button bounds if filtering is enabled
        /// and the bounds are empty.
        /// </summary>
        protected Rectangle DropDownButtonBounds
        {
            get
            {
                if (!FilteringEnabled)
                {
                    return Rectangle.Empty;
                }
                if (dropDownButtonBoundsValue == Rectangle.Empty)
                {
                    SetDropDownButtonBounds();
                }
                return dropDownButtonBoundsValue;
            }
        }

        /// <summary>
        /// Sets dropDownButtonBoundsValue to Rectangle.Empty if it isn't already empty. 
        /// This indicates that the button bounds should be recalculated. 
        /// </summary>
        private void InvalidateDropDownButtonBounds()
        {
            if (!dropDownButtonBoundsValue.IsEmpty)
            {
                dropDownButtonBoundsValue = Rectangle.Empty;
            }
        }

        /// <summary>
        /// Sets the position and size of dropDownButtonBoundsValue based on the current 
        /// cell bounds and the preferred cell height for a single line of header text. 
        /// </summary>
        private void SetDropDownButtonBounds()
        {
            // Retrieve the cell display rectangle, which is used to 
            // set the position of the drop-down button. 
            Rectangle cellBounds = 
                this.DataGridView.GetCellDisplayRectangle(
                this.ColumnIndex, -1, false);

            // Initialize a variable to store the button edge length,
            // setting its initial value based on the font height. 
            Int32 buttonEdgeLength = this.InheritedStyle.Font.Height + 5;

            // Calculate the height of the cell borders and padding.
            Rectangle borderRect = BorderWidths(
                this.DataGridView.AdjustColumnHeaderBorderStyle(
                this.DataGridView.AdvancedColumnHeadersBorderStyle,
                new DataGridViewAdvancedBorderStyle(), false, false));
            Int32 borderAndPaddingHeight = 2 +
                borderRect.Top + borderRect.Height +
                this.InheritedStyle.Padding.Vertical;
            Boolean visualStylesEnabled =
                Application.RenderWithVisualStyles &&
                this.DataGridView.EnableHeadersVisualStyles;
            if (visualStylesEnabled) 
            {
                borderAndPaddingHeight += 3;
            }

            // Constrain the button edge length to the height of the 
            // column headers minus the border and padding height. 
            if (buttonEdgeLength >
                this.DataGridView.ColumnHeadersHeight -
                borderAndPaddingHeight)
            {
                buttonEdgeLength =
                    this.DataGridView.ColumnHeadersHeight -
                    borderAndPaddingHeight;
            }

            // Constrain the button edge length to the
            // width of the cell minus three.
            if (buttonEdgeLength > cellBounds.Width - 3)
            {
                buttonEdgeLength = cellBounds.Width - 3;
            }

            // Calculate the location of the drop-down button, with adjustments
            // based on whether visual styles are enabled. 
            Int32 topOffset = visualStylesEnabled ? 4 : 1;
            Int32 top = cellBounds.Bottom - buttonEdgeLength - topOffset;
            Int32 leftOffset = visualStylesEnabled ? 3 : 1;
            Int32 left = 0;
            if (this.DataGridView.RightToLeft == RightToLeft.No)
            {
                left = cellBounds.Right - buttonEdgeLength - leftOffset;
            }
            else
            {
                left = cellBounds.Left + leftOffset;
            }

            // Set the dropDownButtonBoundsValue value using the calculated 
            // values, and adjust the cell padding accordingly.  
            dropDownButtonBoundsValue = new Rectangle(left, top, 
                buttonEdgeLength, buttonEdgeLength);
            AdjustPadding(buttonEdgeLength + leftOffset);
        }

        /// <summary>
        /// Adjusts the cell padding to widen the header by the drop-down button width.
        /// </summary>
        /// <param name="newDropDownButtonPaddingOffset">The new drop-down button width.</param>
        private void AdjustPadding(Int32 newDropDownButtonPaddingOffset)
        {
            // Determine the difference between the new and current 
            // padding adjustment.
            Int32 widthChange = newDropDownButtonPaddingOffset - 
                currentDropDownButtonPaddingOffset;

            // If the padding needs to change, store the new value and 
            // make the change.
            if (widthChange != 0)
            {
                // Store the offset for the drop-down button separately from 
                // the padding in case the client needs additional padding.
                currentDropDownButtonPaddingOffset = 
                    newDropDownButtonPaddingOffset;
                
                // Create a new Padding using the adjustment amount, then add it
                // to the cell's existing Style.Padding property value. 
                Padding dropDownPadding = new Padding(0, 0, widthChange, 0);
                this.Style.Padding = Padding.Add(
                    this.InheritedStyle.Padding, dropDownPadding);
            }
        }

        /// <summary>
        /// The current width of the drop-down button. This field is used to adjust the cell padding.  
        /// </summary>
        protected Int32 currentDropDownButtonPaddingOffset;

        #endregion button bounds

        #region public properties: FilteringEnabled, AutomaticSortingEnabled, DropDownListBoxMaxLines

        /// <summary>
        /// Indicates whether filtering is enabled for the owning column. 
        /// </summary>
        private Boolean filteringEnabledValue = true;

        /// <summary>
        /// Gets or sets a value indicating whether filtering is enabled.
        /// </summary>
        [DefaultValue(true)]
        public Boolean FilteringEnabled
        {
            get 
            { 
                // Return filteringEnabledValue if (there is no DataGridView
                // or if (its DataSource property has not been set. 
                if (this.DataGridView == null ||
                    this.DataGridView.DataSource == null)
                {
                    return filteringEnabledValue;
                }

                // if (the DataSource property has been set, return a value that combines 
                // the filteringEnabledValue and BindingSource.SupportsFiltering values.
                BindingSource data = this.DataGridView.DataSource as BindingSource;
                Debug.Assert(data != null);
                return filteringEnabledValue && data.SupportsFiltering;
            }
            set 
            {
                // If filtering is disabled, remove the padding adjustment
                // and invalidate the button bounds. 
                if (!value)
                {
                    AdjustPadding(0);
                    InvalidateDropDownButtonBounds();
                }
                
                filteringEnabledValue = value; 
            }
        }

        /// <summary>
        /// Indicates whether automatic sorting is enabled. 
        /// </summary>
        private Boolean automaticSortingEnabledValue = true;

        /// <summary>
        /// Gets or sets a value indicating whether automatic sorting is enabled for the owning column. 
        /// </summary>
        [DefaultValue(true)]
        public Boolean AutomaticSortingEnabled
        {
            get 
            { 
                return automaticSortingEnabledValue; 
            }
            set 
            { 
                automaticSortingEnabledValue = value;
                if (OwningColumn != null)
                {
                    if (value)
                    {
                        OwningColumn.SortMode = DataGridViewColumnSortMode.Programmatic;
                    }
                    else
                    {
                        OwningColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
            }
        }

        

        /// <summary>
        /// The maximum number of lines in the drop-down list. 
        /// </summary>
        protected Int32 dropDownListBoxMaxLinesValue = 20;

        public IBE.Enums_and_Utility_Classes.DataRetriever Retriever { get; set; }
        public string RetrieverSQLSelect { get; set; }


        /// <summary>
        /// Gets or sets the maximum number of lines to display in the drop-down filter list. 
        /// The actual height of the drop-down list is constrained by the DataGridView height. 
        /// </summary>
        [DefaultValue(20)]
        public Int32 DropDownListBoxMaxLines
        {
            get { return dropDownListBoxMaxLinesValue; }
            set { dropDownListBoxMaxLinesValue = value; }
        }

        #endregion public properties

    }

}

