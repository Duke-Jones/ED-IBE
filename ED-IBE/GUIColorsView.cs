using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IBE
{
    public partial class GUIColorsView : IBE.Enums_and_Utility_Classes.RNBaseForm
    {

        private Boolean             m_changedData { get; set; }
        private List<Panel>         m_Panels = new List<Panel>();      

        public GUIColorsView()
        {
            InitializeComponent();
        }

        

        private void GUIColorsView_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareGUIObjects();

                cbActivated.Checked = Program.Colors.UseColors;

                if(Program.Colors.UsePreset == 1)
                    rbPresetDefault.Checked = true;
                else
                    rbPresetElite.Checked   = true;


                this.cbActivated.CheckedChanged += cbActivated_CheckedChanged;
                this.rbPresetElite.CheckedChanged += rbPreset_CheckedChanged;

                m_changedData = false;

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in GUIColorsView_Load");
            }
            
        }

        private void PrepareGUIObjects()
        {
            Int32 counter = 0;

            try
            {
                if(m_Panels.Count > 0)
                    do
                    {
                        var currentPanel = m_Panels[0];
                        m_Panels.Remove(currentPanel);
                        currentPanel.Dispose();
                    } while (m_Panels.Count > 0); 

                foreach (GUIColors.ColorNames colorName in Enum.GetValues(typeof(GUIColors.ColorNames)))
                {

                    Panel new_paColor;
                    Label new_lblColorName;
                    PictureBox new_pbColor;

                    if (counter > 0)
                    {
                        new_lblColorName = new Label();

                        new_pbColor = new PictureBox();
                        new_pbColor.Width = pbColor_0.Width;
                        new_pbColor.Height = pbColor_0.Height;
                        new_pbColor.BorderStyle = pbColor_0.BorderStyle;
                        

                        new_paColor = new Panel();
                        new_paColor.Width = paColor_0.Width;
                        new_paColor.Height = paColor_0.Height;

                        new_paColor.Controls.Add(new_lblColorName);
                        new_paColor.Controls.Add(new_pbColor);

                        new_lblColorName.AutoSize = lblColorName_0.AutoSize;
                        new_lblColorName.Anchor = lblColorName_0.Anchor;
                        new_lblColorName.TextAlign = lblColorName_0.TextAlign;
                        new_lblColorName.Size = lblColorName_0.Size;
                        new_lblColorName.Font = lblColorName_0.Font;
                        

                        new_lblColorName.Location = lblColorName_0.Location;
                        new_pbColor.Location = pbColor_0.Location;

                        flowLayoutPanel1.Controls.Add(new_paColor);

                        new_pbColor.Click += pbColor_0_Click;

                        m_Panels.Add(new_paColor);
                    }
                    else
                    {
                        new_paColor = paColor_0;
                        new_lblColorName = lblColorName_0;
                        new_pbColor = pbColor_0;
                    }

                    new_lblColorName.Text = colorName.ToString();
                    new_pbColor.Tag = colorName;
                    ShowColor(new_pbColor, Program.Colors.GetColor(colorName));

                    counter++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating gui objects", ex);
            }
        }
        private void ShowColor(PictureBox pBox, Color guiColor)
        {
            try
            {
                if (pBox.Image != null) 
                    pBox.Image.Dispose();

                Bitmap b = new Bitmap(pBox.Width, pBox.Height);
                using (var g = Graphics.FromImage(b))
                {
                    g.Clear(guiColor);
                }
                pBox.Image = b;         
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ShowColor", ex);
            }
        }

        private void pbColor_0_Click(object sender, EventArgs e)
        {
            try
            {
                PictureBox currentPB = (PictureBox)sender;

                ColorDialog c = new ColorDialog();
                if (c.ShowDialog() == DialogResult.OK)
                {
                    Program.Colors.SetColor((GUIColors.ColorNames)currentPB.Tag, c.Color);
                    ShowColor(currentPB, Program.Colors.GetColor((GUIColors.ColorNames)currentPB.Tag));
                    m_changedData = true;
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in pbColor_0_Click");
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GUIColorsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(m_changedData)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void cmdResetColors_Click(object sender, EventArgs e)
        {
            try
            {
                Program.Colors.ResetColors();
                PrepareGUIObjects();
                m_changedData = true;

                if ((!Program.Colors.UseColors) && (MessageBox.Show("Activate re-theming ?", "Preset loaded..",  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
                {
                    cbActivated.Checked = true;
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdActivateColors_Click");
            }
        }

        private void cbActivated_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Program.Colors.UseColors = cbActivated.Checked;
                m_changedData = true;

                if(!Program.Colors.UseColors)
                    MessageBox.Show("Restart required !", "Colors deactivated",  MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cbActivated_CheckedChanged");
            }
        }

        private void rbPreset_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rb = (RadioButton)sender;

                if(rb.Checked)
                    Program.Colors.UsePreset = Int32.Parse((String)rb.Tag);

                m_changedData = true;

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cbActivated_CheckedChanged");
            }
        }

    }
}
