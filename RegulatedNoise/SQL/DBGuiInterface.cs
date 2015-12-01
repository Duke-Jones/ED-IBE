using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegulatedNoise.Enums_and_Utility_Classes;
using System.Reflection;
using System.Data;
using System.Diagnostics;

namespace RegulatedNoise.SQL
{
    class DBGuiInterface
    {
        String m_InitGroup;
        Object m_currentLoadingObject   = null;        
        Int32  m_inloadAllSettings      = 0;
        Int32  m_inloadSetting          = 0;

#region  TagParts

        private class TagParts
        {
            public String IDString { get; set; }
            public String DefaultValue { get; set; }
        }


#endregion

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="InitGroup"></param>
        public DBGuiInterface(String InitGroup)
        {
            try
            {
                m_InitGroup = InitGroup;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating object", ex);
            }
        }

        /// <summary>
        /// saves a setting to the database
        /// </summary>
        /// <param name="sender"></param>
        public Boolean saveSetting(object sender, Object Param1 = null)
        {
            Boolean retValue = false;

            try
            {
                if((m_inloadAllSettings == 0) && (m_currentLoadingObject != sender))
                { 
                    if(sender.GetType() == typeof(CheckBox))
                    {
                        var cbSender = (CheckBox)sender;
                        var Parts    = splitTag(cbSender.Tag);    

                        if(Parts != null)
                            retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, cbSender.Checked.ToString());
                    }
                    else if((sender.GetType() == typeof(ComboBox)) || (sender.GetType() == typeof(ComboBoxInt32)))
                    {
                        var cbSender = (ComboBox)sender;
                        var Parts    = splitTag(cbSender.Tag);    
                        Object SelectComboBoxValue = null;

                        if(Parts != null)
                        {

                            if ((cbSender.ValueMember != null) && (!String.IsNullOrEmpty(cbSender.ValueMember)))
                            {
                                // it's working with a ValueMember -> translate "Value" to "Item" and set ".SelectedItem"
                                var Props = cbSender.Items[0].GetType().GetProperties();
                                System.Reflection.PropertyInfo FoundPropertyItem = null;
                                Type FoundColumnType = null;

                                if((cbSender.DataSource == null) || (!cbSender.DataSource.GetType().Equals(typeof(System.Data.DataTable))))
                                { 
                                    foreach (var PropertyItem in Props)
                                    {
                                        if(PropertyItem.Name == cbSender.ValueMember)
                                        { 
                                            FoundPropertyItem = PropertyItem;
                                            FoundColumnType = FoundPropertyItem.GetMethod.ReturnType;
                                            break;
                                        }
                                    }

                                    SelectComboBoxValue = cbSender.SelectedItem.GetType().GetProperty(FoundPropertyItem.Name).GetValue(cbSender.SelectedItem, null);

                                }
                                else
                                {
                                    FoundColumnType = ((DataTable)cbSender.DataSource).Columns[cbSender.ValueMember].DataType;

                                    SelectComboBoxValue = ((DataRowView)cbSender.SelectedItem)[cbSender.ValueMember];
                                }

                                

                                retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, Convert.ChangeType(SelectComboBoxValue, FoundColumnType).ToString());
                            }
                            else
                                retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, cbSender.Text);

                        }
                            //if (cbSender.ValueMember != null)
                            //    retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, cbSender.SelectedValue.ToString());
                            //else
                            //    retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, cbSender.Text);
                    }
                    else if((sender.GetType() == typeof(TextBox)) || (sender.GetType() == typeof(TextBoxInt32)) || (sender.GetType() == typeof(TextBoxDouble)))
                    {
                        var cbSender = (TextBox)sender;
                        var Parts    = splitTag(cbSender.Tag);    

                        if(Parts != null)
                            retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, cbSender.Text);
                    }
                    else if(sender.GetType() == typeof(NumericUpDown))
                    {
                        var cbSender = (NumericUpDown)sender;
                        var Parts    = splitTag(cbSender.Tag);    

                        if(Parts != null)
                            retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, cbSender.Text);
                    }
                    else if(sender.GetType() == typeof(RadioButton))
                    {
                        // radio button will be set due to its parent container
                        var cbSender = (RadioButton)sender;

                        // avoid double saving (one is activated, another is deactivated)
                        if(cbSender.Checked)
                            retValue = saveSetting(cbSender.Parent);
                    }

                    else if(sender.GetType() == typeof(GroupBox))
                    {
                        // a container will handle it's radiobuttons
                        var cbSender = (GroupBox)sender;
                        var Parts    = splitTag(cbSender.Tag);    
                    
                        if(Parts != null)
                        {
                            Boolean Found = false;

                            // Search all radionbuttons in the combobox and 
                            // check this one with the <Value> tag. Or if not found
                            // set the fist found RadioButton checked

                            foreach (Control SubControl in cbSender.Controls)
                            {
                                if(SubControl.GetType() == typeof(RadioButton))
                                {
                                    RadioButton rbControl = (RadioButton)SubControl;

                                    if(rbControl.Checked)
                                    {
                                        retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, rbControl.Tag.ToString());
                                        Found = true;
                                        break;
                                    }
                                }
                            }

                            if(!Found)
                                retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, Parts.DefaultValue);

                        }
                    }
                    else if(sender.GetType() == typeof(DataGridViewExt))
                    {
                        var cbSender = (DataGridViewExt)sender;
                        var Parts    = splitTag(cbSender.Tag);    

                        if(Parts != null)
                        {
                            if((Param1 != null) && (Param1.GetType() == typeof(DataGridViewExt.SortedEventArgs)))
                            {
                                var SortEA = (DataGridViewExt.SortedEventArgs)Param1;

                                // sortorder changed
                                retValue  = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString + "_SortColumn", SortEA.SortColumn.Index.ToString());
                                retValue |= Program.DBCon.setIniValue(m_InitGroup, Parts.IDString + "_SortOrder", SortEA.SortOrder.ToString());
                            }

                            StringBuilder SaveString = new StringBuilder();
                            foreach (DataGridViewColumn currentColumn in cbSender.Columns)
                            {
                                SaveString.Append(String.Format("{0}/{1}/{2}/{3}/{4}/{5};", currentColumn.DisplayIndex.ToString(), 
                                                                                            currentColumn.Visible.ToString(), 
                                                                                            currentColumn.AutoSizeMode.ToString(), 
                                                                                            currentColumn.Width.ToString(), 
                                                                                            currentColumn.FillWeight.ToString().Replace(",","."), 
                                                                                            currentColumn.MinimumWidth.ToString()));
                            }
                            Program.DBCon.setIniValue(m_InitGroup, Parts.IDString + "_ColumnSettings", SaveString.ToString());
                        }
                    }
                    else if(sender.GetType() == typeof(SplitContainer))
                    {
                        var cbSender         = (SplitContainer)sender;
                        var Parts            = splitTag(cbSender.Tag);    
                        Int32 SplitterRatio;

                        if(Parts != null)
                        { 
                            if(cbSender.Orientation == Orientation.Vertical)
                                SplitterRatio = (Int32)Math.Round(((Single)(cbSender.SplitterDistance) * 100  / ((Single)cbSender.Width)), 0);
                            else
                                SplitterRatio = (Int32)Math.Round(((Single)(cbSender.SplitterDistance) * 100  / ((Single)cbSender.Height)), 0);
                            
                            if(SplitterRatio > 0)
                                retValue = Program.DBCon.setIniValue(m_InitGroup, Parts.IDString, SplitterRatio.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in saveSetting");
            }

            return retValue;
        }

        /// <summary>
        ///loads a setting from the database
        /// </summary>
        /// <param name="sender"></param>
        public void loadSetting(object sender, Control BaseObject = null)
        {
            try
            {
                m_inloadSetting++;

                /// set the value of the baseobject if necessary
                if(sender.GetType() == typeof(CheckBox))
                {
                    var cbSender = (CheckBox)sender;
                    var Parts    = splitTag(cbSender.Tag);    

                    if(Parts != null)
                    {
                        m_currentLoadingObject = cbSender;
                        cbSender.Checked       = Program.DBCon.getIniValue<Boolean>(m_InitGroup, Parts.IDString, Parts.DefaultValue, false, true);
                        m_currentLoadingObject = null;
                    }
                }
                else if((sender.GetType() == typeof(ComboBox)) || (sender.GetType() == typeof(ComboBoxInt32)))
                {
                    var cbSender            = (ComboBox)sender;
                    var TagParts            = splitTag(cbSender.Tag);    
                    MethodInfo GenericMethodInfo;
                    Object ItemToSet        = null;
                    Object ValueToSet       = null;

                    if(TagParts != null)
                    {
                        // this is one of ours

                        m_currentLoadingObject = cbSender;

                        if ((cbSender.ValueMember != null) && (!String.IsNullOrEmpty(cbSender.ValueMember)))
                        {
                            // it's working with a ValueMember -> translate "Value" to "Item" and set ".SelectedItem"
                            var Props = cbSender.Items[0].GetType().GetProperties();
                            System.Reflection.PropertyInfo FoundPropertyItem = null;
                            Type FoundColumnType = null;

                            if((cbSender.DataSource == null) || (!cbSender.DataSource.GetType().Equals(typeof(System.Data.DataTable))))
                            { 
                                foreach (var PropertyItem in Props)
                                {
                                    if(PropertyItem.Name == cbSender.ValueMember)
                                    { 
                                        FoundPropertyItem = PropertyItem;
                                        FoundColumnType = FoundPropertyItem.GetMethod.ReturnType;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                FoundColumnType = ((DataTable)cbSender.DataSource).Columns[cbSender.ValueMember].DataType;
                            }

                            if (FoundColumnType != null)
                            { 
                                GenericMethodInfo       = typeof(DBConnector).GetMethod("getIniValue", new Type[] {typeof(String), typeof(String), typeof(String) , typeof(Boolean) , typeof(Boolean)} );
                                GenericMethodInfo       = GenericMethodInfo.MakeGenericMethod(FoundColumnType);
                                ValueToSet              = GenericMethodInfo.Invoke(Program.DBCon, new object[] { m_InitGroup, TagParts.IDString, TagParts.DefaultValue, false, true });
                            }

                            if (FoundPropertyItem != null)
                            { 
                                foreach (var currentComboxItem in cbSender.Items)
	                            {
		                            if(Convert.ChangeType(currentComboxItem.GetType().GetProperty(FoundPropertyItem.Name).GetValue(currentComboxItem, null), FoundColumnType).Equals(Convert.ChangeType(ValueToSet, FoundColumnType)))
                                    { 
                                        ItemToSet = currentComboxItem;
                                        break;
                                    }
	                            }
                            
                                if(ItemToSet != null)
                                    cbSender.SelectedItem = ItemToSet;
                            }
                            else
                            {
                                foreach (DataRowView currentDataRow in cbSender.Items)
	                            {
		                            if(Convert.ChangeType(currentDataRow[cbSender.ValueMember], FoundColumnType).Equals(Convert.ChangeType(ValueToSet,FoundColumnType)))
                                    { 
                                        ItemToSet = currentDataRow;
                                        break;
                                    }
                                }

                                //foreach (DataRow currentDataRow in ((DataTable)cbSender.DataSource).Rows)
                                //{
                                //    if(Convert.ChangeType(currentDataRow[cbSender.ValueMember], FoundColumnType).Equals(Convert.ChangeType(ValueToSet,FoundColumnType)))
                                //    { 
                                //        ItemToSet = currentDataRow;
                                //        break;
                                //    }
                                //}
                            
                                if(ItemToSet != null)
                                    cbSender.SelectedItem = ItemToSet;
                            }
                        }
                        else
                            cbSender.Text          = Program.DBCon.getIniValue<String>(m_InitGroup, TagParts.IDString, TagParts.DefaultValue, false, true);

                        m_currentLoadingObject = null;
                    }
                }
                else if((sender.GetType() == typeof(TextBox)) || (sender.GetType() == typeof(TextBoxInt32)) || (sender.GetType() == typeof(TextBoxDouble)))
                {
                    var cbSender = (TextBox)sender;
                    var Parts    = splitTag(cbSender.Tag);    

                    if(Parts != null)
                    {
                        m_currentLoadingObject = cbSender;
                        if(Parts.DefaultValue.Equals("EMPTY"))
                            cbSender.Text          = Program.DBCon.getIniValue<String>(m_InitGroup, Parts.IDString, "");
                        else
                            cbSender.Text          = Program.DBCon.getIniValue<String>(m_InitGroup, Parts.IDString, Parts.DefaultValue, false, true);
                        m_currentLoadingObject = null;
                    }
                }
                else if(sender.GetType() == typeof(NumericUpDown))
                {
                    var cbSender = (NumericUpDown)sender;
                    var Parts    = splitTag(cbSender.Tag);    

                    if(Parts != null)
                    {
                        m_currentLoadingObject = cbSender;
                        cbSender.Text          = Program.DBCon.getIniValue<String>(m_InitGroup, Parts.IDString, Parts.DefaultValue, false, true);
                        m_currentLoadingObject = null;
                    }
                }
                else if(sender.GetType() == typeof(RadioButton))
                {
                    // radio button will be set due to its parent container
                    var cbSender = (RadioButton)sender;

                    if(cbSender.Parent != BaseObject)
                        // avoid recursion
                        loadSetting(cbSender.Parent);
                }

                else if(sender.GetType() == typeof(GroupBox))
                {
                    // a container will handle it's radiobuttons
                    var cbSender = (GroupBox)sender;
                    var Parts    = splitTag(cbSender.Tag);    
                    
                    if(Parts != null)
                    {
                        RadioButton firstRadioButton = null;
                        Boolean Found = false;
                        String Value;

                        m_currentLoadingObject  = cbSender;
                        Value                   = Program.DBCon.getIniValue<String>(m_InitGroup, Parts.IDString, Parts.DefaultValue, false, true);
                        m_currentLoadingObject  = null;

                        // Search all radionbuttons in the combobox and 
                        // check this one with the <Value> tag. Or if not found
                        // set the fist found RadioButton checked

                        foreach (Control SubControl in cbSender.Controls)
                        {
                            if(SubControl.GetType() == typeof(RadioButton))
                            {
                                RadioButton rbControl = (RadioButton)SubControl;

                                if(firstRadioButton == null)
                                    firstRadioButton = rbControl;
                                
                                if(rbControl.Tag.Equals(Value))
                                {
                                    rbControl.Checked = true;
                                    Found = true;
                                }
                                else
                                {
                                    rbControl.Checked = false;
                                }

                            }
                        }

                        if(!Found)
                            firstRadioButton.Checked = true;

                    }
                }
                else if(sender.GetType() == typeof(DataGridViewExt))
                {
                    var cbSender = (DataGridViewExt)sender;
                    var Parts    = splitTag(cbSender.Tag);    
                    System.ComponentModel.ListSortDirection Order;
                    String OrderStr = "";

                    // sortorder changed

                    if(Parts != null)
                    {
                        var Column    = cbSender.Columns[Program.DBCon.getIniValue<String>(m_InitGroup, Parts.IDString + "_SortColumn", Parts.DefaultValue, false, true)];
                        OrderStr      = Program.DBCon.getIniValue<String>(m_InitGroup, Parts.IDString + "_SortOrder", SortOrder.Ascending.ToString(), false, true);

                        if(OrderStr.Equals(SortOrder.Descending.ToString(), StringComparison.InvariantCultureIgnoreCase))
                            Order = System.ComponentModel.ListSortDirection.Descending;
                        else 
                            Order = System.ComponentModel.ListSortDirection.Ascending;
                        
                        if(Column != null)
                        {
                            m_currentLoadingObject = cbSender;
                            cbSender.Sort(Column,Order);
                            m_currentLoadingObject = null;
                        }

                            //    SaveString.Append(String.Format("{0}/{1}/{2}/{3}/{4};", currentColumn.DisplayIndex, currentColumn.Width, currentColumn.Visible, currentColumn.FillWeight, currentColumn.AutoSizeMode));
                            //}
                            //Program.DBCon.setIniValue(m_InitGroup, Parts.IDString + "_ColumnSettings", SaveString.ToString());

                        String[] VisibilityStrings = null;
                        VisibilityStrings = Program.DBCon.getIniValue(m_InitGroup, Parts.IDString + "_ColumnSettings", "").Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                        Int32 ColumnIndex = 0;

                        cbSender.SuspendLayout();

                        foreach (DataGridViewColumn currentColumn in cbSender.Columns)
                        {
                            if(VisibilityStrings.GetUpperBound(0) >= ColumnIndex)
                            { 
                                String[] ViParts = VisibilityStrings[ColumnIndex].Split(new char[] {'/'});     

                                if(ViParts.GetUpperBound(0) == 5)
                                {
                                    try
                                    {
                                        currentColumn.DisplayIndex   = Int32.Parse(ViParts[0]);
                                        currentColumn.Visible        = Boolean.Parse(ViParts[1]);
                                        currentColumn.AutoSizeMode   = (DataGridViewAutoSizeColumnMode)Enum.Parse(typeof(DataGridViewAutoSizeColumnMode), (String)ViParts[2]);
                                        switch (currentColumn.AutoSizeMode)
                                        {
                                            case DataGridViewAutoSizeColumnMode.Fill:
                                                currentColumn.FillWeight     = Single.Parse(ViParts[4],System.Globalization.NumberStyles.AllowDecimalPoint,System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                                break;
                                            default:
                                                currentColumn.Width          = Int32.Parse(ViParts[3]);
                                                break;
                                        }
                                        
                                        currentColumn.MinimumWidth   = Int32.Parse(ViParts[5]);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }

                            ColumnIndex++;

                        }

                        cbSender.ResumeLayout();

                    }
                }
                else if(sender.GetType() == typeof(SplitContainer))
                {
                    // got some problems with this setting so 
                    // i deactivate it for the moment
                    var cbSender = (SplitContainer)sender;
                    var Parts    = splitTag(cbSender.Tag);    
                    Int32 SplitterRatio;

                    if(Parts != null)
                    {
                        SplitterRatio = Program.DBCon.getIniValue<Int32>(m_InitGroup, Parts.IDString, Parts.DefaultValue, false, true);
                        if(cbSender.Orientation == Orientation.Vertical)
                            SplitterRatio = (Int32)Math.Round(((Single)(SplitterRatio * cbSender.Width)) / 100, 0);
                        else
                            SplitterRatio = (Int32)Math.Round(((Single)(SplitterRatio * cbSender.Height)) / 100, 0);

                        if(SplitterRatio > 0)
                        { 
                            m_currentLoadingObject  = cbSender;
                            cbSender.SplitterDistance = SplitterRatio;
                            m_currentLoadingObject  = null;
                        }
                    }
                }

                m_inloadSetting--;

            }
            catch (Exception ex)
            {
                m_inloadSetting--;
                m_currentLoadingObject = null;
                cErr.showError(ex, "Error in loadSetting");
            }
        }

        /// <summary>
        /// loads recursive all needed values 
        /// </summary>
        /// <param name="BaseObject"></param>
        internal void loadAllSettings(Control BaseObject, Control BaseBaseObject = null)
        {
            try
            {
                m_inloadAllSettings++;

                loadSetting(BaseObject, BaseBaseObject);

                // check if there are sub-objects who need values
                foreach (Control SubObject in BaseObject.Controls)
                    loadAllSettings(SubObject, BaseObject);

                m_inloadAllSettings--;
            }
            catch (Exception ex)
            {
                m_inloadAllSettings--;
                throw new Exception("Error while loading all setting from DB", ex);
            }
        }

        /// <summary>
        /// splits the parts of a tag-string
        /// </summary>
        /// <param name="TagString"></param>
        /// <returns></returns>
        private TagParts splitTag(Object TagString)
        {
            TagParts TParts     = null;

            try
            {
                if(TagString != null)
                { 
                    String[] Parts      = ((String)TagString).Split(';');

                    if(Parts.GetUpperBound(0) == 1)
                    { 
                        TParts              = new TagParts();

                        TParts.IDString     = Parts[0];
                        TParts.DefaultValue = Parts[1];
                    }
                }

                return TParts;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while splitting tag", ex);
            }
        }

        public static class Foo
        {
            public static void Bar<T>(string test)
            {
                MessageBox.Show(typeof(T).Name);
            }
        }

//...


    }
}
