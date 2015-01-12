using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegulatedNoise
{
    public partial class OcrCalibratorTab : UserControl
    {
        public OcrCalibratorTab()
        {
            InitializeComponent();
        }

        private CalibrationPoint selCalibrationPoint;
        private bool isMouseDown;
        private bool drawPoints = false;

        private void OcrCalibratorTab_Load(object sender, EventArgs e)
        {
            setResolutionValues();

            fillRawData();

            tb_uicolor.Text = Form1.RegulatedNoiseSettings.UiColour;
        }

        private void setResolutionValues()
        {
            if (Form1.GameSettings.Display == null)
            {
                tb_resolution.Text = "ERROR";
            }
            else
            {
                tb_resolution.Text = Form1.GameSettings.Display.Resolution.X + "x" + Form1.GameSettings.Display.Resolution.Y;
            }
        }
        private void doCalibration()
        {
            if (Form1.GameSettings.Display != null)
            {
                var calibrations = Form1.OcrCalibrator.getCalculatedCalibrationPoints(Form1.GameSettings.Display.Resolution);
                drawCalibrationPoints(calibrations);
                return;
            }

            MessageBox.Show("Unable to calibrate automatically. Please calibration manually.");
        }
        private void manualCalibrate()
        {
            Form1.OcrCalibrator.calibrationBoxes = new List<CalibrationPoint>();
            var p = new Point(30, 30);
            tb_description.Text = "Point 1: " + new CalibrationPoint()._calibrationDescriptions[0];
            for (var i = 0; i < 12; i++)
            {
                var cb = new CalibrationPoint(i, new Point(p.X+(50*(i+1)), p.Y));
                Form1.OcrCalibrator.calibrationBoxes.Add(cb);
            }
            fillRawData();
            drawPoints = true;
            pb_calibratorBox.Refresh();
        }

        private void fillRawData()
        {
            if (Form1.OcrCalibrator.calibrationBoxes == null || Form1.OcrCalibrator.calibrationBoxes.Count < 1)
                return;
            tb_rawdata.Text = string.Join(Environment.NewLine, Form1.OcrCalibrator.calibrationBoxes.Select(p => p.Position.X + ";" + p.Position.Y));
        }

        private Bitmap getReferenceScreenshot(bool necessarily)
        {
            var openFile = new OpenFileDialog
            {
                DefaultExt = "bmp",
                Multiselect = true,
                Filter = "BMP (*.bmp)|*.bmp",
                InitialDirectory =
                    Environment.GetFolderPath((Environment.SpecialFolder.MyPictures)) +
                    @"\Frontier Developments\Elite Dangerous"
            };
            openFile.Title = necessarily ? "Open a screenshot for calibration verification. (This step is not necessarily)" : "Open a screenshot for calibration";
            
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var bmp = new Bitmap(openFile.FileName);

                if (bmp.Height == Form1.GameSettings.Display.Resolution.Y &&
                    bmp.Width == Form1.GameSettings.Display.Resolution.X) return bmp;
                var wrongres = MessageBox.Show("The selected image has a different resoluton than what we can read from you game configuration, do you want to pick another image?", "Ooops", MessageBoxButtons.YesNo);
                return wrongres == DialogResult.Yes ? getReferenceScreenshot(necessarily) : bmp;
            }
            else
            {
                return getReferenceScreenshot(necessarily);
            }
            return null;
        }

        private void btn_calibrate_Click(object sender, EventArgs e)
        {
            drawPoints = false;
            doCalibration();

            if (Form1.OcrCalibrator.calibrationBoxes == null)
            {
                //Manual calibrate here 
                var refbmp = getReferenceScreenshot(true);
                pb_calibratorBox.Image = refbmp;

                MessageBox.Show("To calibrate drag the crosshairs to the position described to the right");
                manualCalibrate();
            }
            else
            {
                fillRawData();
                var refbmp = getReferenceScreenshot(false);
                if (refbmp != null)
                {
                    pb_calibratorBox.Image = refbmp;
                    drawPoints = true;
                }
                Form1.OcrCalibrator.SaveCalibration();
            }

        }
        private void updateDescriptions(CalibrationPoint cb)
        {
            pb_example.Image = cb.Example;
            tb_description.Text = cb.Description;
        }
        private void drawCalibrationPoints(IEnumerable<Point> calibration)
        {
            Form1.OcrCalibrator.calibrationBoxes = new List<CalibrationPoint>();
            var i = 0;
            foreach (var c in calibration)
            {
                var r = new CalibrationPoint(i, c);
                Form1.OcrCalibrator.calibrationBoxes.Add(r);
                i++;
            }
        }
        private void pb_calibratorBox_Paint(object sender, PaintEventArgs e)
        {
            if (Form1.OcrCalibrator.calibrationBoxes == null || Form1.OcrCalibrator.calibrationBoxes.Count < 1 || !drawPoints)
                return;

            var i = 0;
            foreach (var calibrationBox in Form1.OcrCalibrator.calibrationBoxes)
            {
                //e.Graphics.FillRectangle(new SolidBrush(Color.RoyalBlue), calibrationBox.Hitbox); //Uncomment to show hitbox
                e.Graphics.DrawString("⊗", new Font("Arial", 30), new SolidBrush(Color.Yellow), calibrationBox.Hitbox.X-8, calibrationBox.Hitbox.Y-11);
                e.Graphics.DrawString((i+1).ToString(), new Font("Arial", 10), new SolidBrush(Color.Yellow), calibrationBox.Hitbox.X - 8, calibrationBox.Hitbox.Y - 11);
                i++;
            }

        }
        private void pb_calibratorBox_MouseDown(object sender, MouseEventArgs e)
        {
            //Detect if mouse if over a calibrationpoint
            foreach (var cb in Form1.OcrCalibrator.calibrationBoxes)
            {
                if (cb.Hitbox.Contains(e.Location))
                {
                    selCalibrationPoint = cb;

                    //update textbox and refimage
                    updateDescriptions(cb);

                    isMouseDown = true;
                    break;
                }
            }
        }
        private void pb_calibratorBox_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;

            //Update position
            if (selCalibrationPoint == null)
                return;

            Form1.OcrCalibrator.calibrationBoxes[selCalibrationPoint.ID] = selCalibrationPoint;
            fillRawData();

            Form1.OcrCalibrator.SaveCalibration();
        }
        private void pb_calibratorBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                selCalibrationPoint.setPosition(new Point(e.X - selCalibrationPoint.Offset.X, e.Y - selCalibrationPoint.Offset.X));
                

                if (selCalibrationPoint.Hitbox.Right > pb_calibratorBox.Width)
                {
                    selCalibrationPoint.setX(pb_calibratorBox.Width - selCalibrationPoint.Hitbox.Width);
                }
                if (selCalibrationPoint.Hitbox.Top < 0)
                {
                    selCalibrationPoint.setY(0);
                }
                if (selCalibrationPoint.Hitbox.Left < 0)
                {
                    selCalibrationPoint.setX(0);
                }
                if (selCalibrationPoint.Hitbox.Bottom > pb_calibratorBox.Height)
                {
                    selCalibrationPoint.setY(pb_calibratorBox.Height - selCalibrationPoint.Hitbox.Height);
                }
                pb_calibratorBox.Refresh();
            }
        }

        private void tb_uicolor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var red = int.Parse(tb_uicolor.Text.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                var green = int.Parse(tb_uicolor.Text.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                var blue = int.Parse(tb_uicolor.Text.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

                pb_uicolor.BackColor = Color.FromArgb(red, green, blue);

                Form1.RegulatedNoiseSettings.UiColour = tb_uicolor.Text;
            }
            catch (Exception ex)
            {
            }
            
            
        }
       
    }
}
