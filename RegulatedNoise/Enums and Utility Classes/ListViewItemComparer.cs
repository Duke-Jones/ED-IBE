using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System;

/// <summary>
/// This class is an implementation of the 'IComparer' interface.
/// </summary>
public class ListViewColumnSorter : IComparer
{
    /// <summary>
    /// Specifies the column to be sorted
    /// </summary>
    private int ColumnToSort;
    /// <summary>
    /// Specifies the order in which to sort (i.e. 'Ascending').
    /// </summary>
    private SortOrder OrderOfSort;
    /// <summary>
    /// Case insensitive comparer object
    /// </summary>
    private CaseInsensitiveComparer ObjectCompare;

    private int ListNumber;
    /// <summary>
    /// Class constructor.  Initializes various elements
    /// </summary>
    public ListViewColumnSorter(int listNumber)
    {
        ListNumber = listNumber;

        // Initialize the column to '0'
        ColumnToSort = 0;

        // Initialize the sort order to 'none'
        OrderOfSort = SortOrder.None;

        // Initialize the CaseInsensitiveComparer object
        ObjectCompare = new CaseInsensitiveComparer();
    }

    /// <summary>
    /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
    /// </summary>
    /// <param name="x">First object to be compared</param>
    /// <param name="y">Second object to be compared</param>
    /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
    public int Compare(object x, object y)
    {
        int compareResult = 0;
        ListViewItem listviewX, listviewY;

        // Cast the objects to be compared to ListViewItem objects
        listviewX = (ListViewItem)x;
        listviewY = (ListViewItem)y;

        if(ListNumber == 0)
            switch(ColumnToSort)
            {
                case 1:
                case 2:
                case 3:
                case 5:
                case 9:
                    decimal a = -1,b = -1;
                    decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out a);
                    decimal.TryParse(listviewY.SubItems[ColumnToSort].Text, out b);
                    compareResult = ObjectCompare.Compare(a,b);
                    break;
                case 10:
                    DateTime  Date_a = DateTime.MinValue, Date_b = DateTime.MinValue;
                    DateTime.TryParse(listviewX.SubItems[ColumnToSort].Text, out Date_a);
                    DateTime.TryParse(listviewY.SubItems[ColumnToSort].Text, out Date_b);
                    compareResult = ObjectCompare.Compare(Date_a, Date_b);
                    break;
                default:
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                    break;
            }

        if (ListNumber == 1)
            switch (ColumnToSort)
            {
                case 1:
                case 3:
                case 5:
                    decimal a = -1, b = -1;
                    decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out a);
                    decimal.TryParse(listviewY.SubItems[ColumnToSort].Text, out b);
                    compareResult = ObjectCompare.Compare(a, b);
                    break;
                case 7:
                    DateTime  Date_a = DateTime.MinValue, Date_b = DateTime.MinValue;
                    DateTime.TryParse(listviewX.SubItems[ColumnToSort].Text, out Date_a);
                    DateTime.TryParse(listviewY.SubItems[ColumnToSort].Text, out Date_b);
                    compareResult = ObjectCompare.Compare(Date_a, Date_b);
                    break;
                default:
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                    break;
            }

        if (ListNumber == 2)
            switch (ColumnToSort)
            {
                case 1:
                case 3:
                case 4:
                case 6:
                case 7:
                    decimal a = -1, b = -1;
                    decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out a);
                    decimal.TryParse(listviewY.SubItems[ColumnToSort].Text, out b);
                    compareResult = ObjectCompare.Compare(a, b);
                    break;
                default:
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                    break;
            }

        /*                        commodity.Key,
                        buyPrice.ToString(),
                        supply.ToString(),
                        supplyLevel,
                         sellPrice.ToString(),
                         demand.ToString(),
                        demandLevel,
                         difference.ToString()*/
        if (ListNumber == 3)
            switch (ColumnToSort)
            {
                case 1:
                case 2:
                case 4:
                case 5:
                case 7:
                    decimal a = -1, b = -1;
                    decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out a);
                    decimal.TryParse(listviewY.SubItems[ColumnToSort].Text, out b);
                    compareResult = ObjectCompare.Compare(a, b);
                    break;
                default:
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                    break;
            }

        // Commander's Log
        if (ListNumber == 4)
            switch (ColumnToSort)
            {
                case 0:
                    DateTime  Date_a = DateTime.MinValue, Date_b = DateTime.MinValue;
                    DateTime.TryParse(listviewX.SubItems[ColumnToSort].Text, out Date_a);
                    DateTime.TryParse(listviewY.SubItems[ColumnToSort].Text, out Date_b);
                    compareResult = ObjectCompare.Compare(Date_a, Date_b);
                    break;
                case 1:
                    
                    break;
                case 2:
                case 4:
                case 5:
                case 7:
                    decimal a = -1, b = -1;
                    decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out a);
                    decimal.TryParse(listviewY.SubItems[ColumnToSort].Text, out b);
                    compareResult = ObjectCompare.Compare(a, b);
                    break;
                default:
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                    break;
            }
        // Calculate correct return value based on object comparison
        if (OrderOfSort == SortOrder.Ascending)
        {
            // Ascending sort is selected, return normal result of compare operation
            return compareResult;
        }
        else if (OrderOfSort == SortOrder.Descending)
        {
            // Descending sort is selected, return negative result of compare operation
            return (-compareResult);
        }
        else
        {
            // Return '0' to indicate they are equal
            return 0;
        }
    }

    /// <summary>
    /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
    /// </summary>
    public int SortColumn
    {
        set
        {
            ColumnToSort = value;
        }
        get
        {
            return ColumnToSort;
        }
    }

    /// <summary>
    /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
    /// </summary>
    public SortOrder Order
    {
        set
        {
            OrderOfSort = value;
        }
        get
        {
            return OrderOfSort;
        }
    }

}