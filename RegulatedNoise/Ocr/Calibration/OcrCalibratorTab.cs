using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace RegulatedNoise
{
    public partial class OcrCalibratorTab : UserControl
    {
        public OcrCalibratorTab()
        {
            InitializeComponent();
        }

        private CalibrationPoint _selCalibrationPoint;
        private bool _isMouseDown;
        private bool _drawPoints;

        private void OcrCalibratorTab_Load(object sender, EventArgs e)
        {
            SetResolutionValues();

            FillRawData();

            tb_uicolor.Text = Form1.RegulatedNoiseSettings.UiColour;
        }

        private void SetResolutionValues()
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
        private void DoCalibration()
        {
            if (Form1.GameSettings.Display != null && (Form1.OcrCalibrator.CalibrationBoxes == null || Form1.OcrCalibrator.CalibrationBoxes.Count < 1))
            {
                var calibrations = Form1.OcrCalibrator.GetCalculatedCalibrationPoints(Form1.GameSettings.Display.Resolution);
                DrawCalibrationPoints(calibrations);
                return;
            }

            if (Form1.OcrCalibrator.CalibrationBoxes != null || Form1.OcrCalibrator.CalibrationBoxes.Count >= 1)
                return;

            MessageBox.Show("Unable to calibrate automatically, please calibrate manually...");
        }
        private void ManualCalibrate()
        {
            Form1.OcrCalibrator.CalibrationBoxes = new List<CalibrationPoint>();
            var p = new Point(30, 30);
            tb_description.Text = "Point 1: " + new CalibrationPoint().CalibrationDescriptions[0];
            for (var i = 0; i < 12; i++)
            {
                var cb = new CalibrationPoint(i, new Point(p.X+(50*(i+1)), p.Y));
                Form1.OcrCalibrator.CalibrationBoxes.Add(cb);
            }
            FillRawData();
            _drawPoints = true;
            pb_calibratorBox.Refresh();
        }

        private void FillRawData()
        {
            if (Form1.OcrCalibrator.CalibrationBoxes == null || Form1.OcrCalibrator.CalibrationBoxes.Count < 1)
                return;
            tb_rawdata.Text = string.Join(Environment.NewLine, Form1.OcrCalibrator.CalibrationBoxes.Select(p => p.Position.X + ";" + p.Position.Y));
        }

        private Bitmap getReferenceScreenshot()
        {
            var openFile = new OpenFileDialog
            {
                DefaultExt = "bmp",
                Multiselect = true,
                Filter = "BMP (*.bmp)|*.bmp",
                InitialDirectory =
                    Environment.GetFolderPath((Environment.SpecialFolder.MyPictures)) +
                    @"\Frontier Developments\Elite Dangerous",
                Title = "Open a screenshot for calibration"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var bmp = new Bitmap(openFile.FileName);

                if (bmp.Height == Form1.GameSettings.Display.Resolution.Y &&
                    bmp.Width == Form1.GameSettings.Display.Resolution.X) return bmp;
                var wrongres = MessageBox.Show("The selected image has a different resolution from your current game settings. Do you want to pick another image?", "Ooops...", MessageBoxButtons.YesNo);
                if (wrongres == DialogResult.Yes)
                {
                    return getReferenceScreenshot();
                }
                
                // Force resolution from input bmp
                Form1.GameSettings.Display.ScreenHeight = bmp.Height;
                Form1.GameSettings.Display.ScreenWidth = bmp.Width;
                SetResolutionValues();
                var calibrations = Form1.OcrCalibrator.GetCalculatedCalibrationPoints(Form1.GameSettings.Display.Resolution);
                DrawCalibrationPoints(calibrations);

                return bmp;
            }
            return null;
        }

        private Bitmap _refbmp;

        private void btn_calibrate_Click(object sender, EventArgs e)
        {

            _drawPoints = false;
            DoCalibration();

            if (_refbmp != null)
                _refbmp.Dispose();

            _refbmp = getReferenceScreenshot();

            if (_refbmp == null)
            {
                return;
            }

            btn_calibration_reset.Enabled = true;
            pb_calibratorBox.Image = _refbmp;

            if (Form1.OcrCalibrator.CalibrationBoxes == null)
            {
                //Manual calibrate here 
                MessageBox.Show("To calibrate, drag the crosshairs to the position shown and described on the right.");
                ManualCalibrate();
            }
            else
            {
                FillRawData();
                _drawPoints = true;
                Form1.OcrCalibrator.SaveCalibration();
            }

        }
        private void UpdateDescriptions(CalibrationPoint cb)
        {
            pb_example.Image = cb.Example;
            tb_description.Text = cb.Description;
        }

        protected void DrawCalibrationPoints(IEnumerable<Point> calibration)
        {
            Form1.OcrCalibrator.CalibrationBoxes = new List<CalibrationPoint>();
            var i = 0;
            foreach (var c in calibration)
            {
                var r = new CalibrationPoint(i, c);
                Form1.OcrCalibrator.CalibrationBoxes.Add(r);
                i++;
            }
        }

        private void Pb_calibratorBox_Paint(object sender, PaintEventArgs e)
        {
            if (Form1.OcrCalibrator.CalibrationBoxes == null || Form1.OcrCalibrator.CalibrationBoxes.Count < 1 || !_drawPoints)
                return;

            var i = 0;
            foreach (var calibrationBox in Form1.OcrCalibrator.CalibrationBoxes)
            {
                //e.Graphics.FillRectangle(new SolidBrush(Color.RoyalBlue), calibrationBox.Hitbox); //Uncomment to show hitbox
                e.Graphics.DrawString("⊗", new Font("Arial", 30), new SolidBrush(Color.Yellow), calibrationBox.Hitbox.X-8, calibrationBox.Hitbox.Y-11);
                e.Graphics.DrawString((i+1).ToString(CultureInfo.InvariantCulture), new Font("Arial", 10), new SolidBrush(Color.Yellow), calibrationBox.Hitbox.X - 8, calibrationBox.Hitbox.Y - 11);
                i++;
            }
        }

        private void Pb_calibratorBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (Form1.OcrCalibrator.CalibrationBoxes == null)
                return;

            //Detect if mouse if over a calibration point
            foreach (var cb in Form1.OcrCalibrator.CalibrationBoxes)
            {
                if (cb.Hitbox.Contains(e.Location))
                {
                    _selCalibrationPoint = cb;

                    //update textbox and refimage
                    UpdateDescriptions(cb);

                    _isMouseDown = true;
                    break;
                }
            }
        }
        private void Pb_calibratorBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (Form1.OcrCalibrator.CalibrationBoxes == null)
                return;

            _isMouseDown = false;

            //Update position
            if (_selCalibrationPoint == null)
                return;

            Form1.OcrCalibrator.CalibrationBoxes[_selCalibrationPoint.Id] = _selCalibrationPoint;

            AlignCalibrationBoxesWhereRelevant(Form1.OcrCalibrator.CalibrationBoxes);

            FillRawData();

            pb_calibratorBox.Refresh();

            Form1.OcrCalibrator.SaveCalibration();
        }

        private static void AlignCalibrationBoxesWhereRelevant(List<CalibrationPoint> calibrationBoxes )
        {
            for (int i = 3; i < 11; i++)
                calibrationBoxes[i] = new CalibrationPoint
                    (i,new Point(calibrationBoxes[i].Position.X,
                            calibrationBoxes[2].Position.Y));

            calibrationBoxes[11] = new CalibrationPoint
                (11,new Point(calibrationBoxes[2].Position.X,
                    calibrationBoxes[11].Position.Y));

            calibrationBoxes[1] = new CalibrationPoint
                (1,new Point(calibrationBoxes[5].Position.X,
                        calibrationBoxes[1].Position.Y));
        }

        private void Pb_calibratorBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (Form1.OcrCalibrator.CalibrationBoxes == null)
                return;

            if (_isMouseDown)
            {
                _selCalibrationPoint.SetPosition(new Point(e.X - _selCalibrationPoint.Offset.X, e.Y - _selCalibrationPoint.Offset.X));
                

                if (_selCalibrationPoint.Hitbox.Right > pb_calibratorBox.Width)
                {
                    _selCalibrationPoint.SetX(pb_calibratorBox.Width - _selCalibrationPoint.Hitbox.Width);
                }
                if (_selCalibrationPoint.Hitbox.Top < 0)
                {
                    _selCalibrationPoint.SetY(0);
                }
                if (_selCalibrationPoint.Hitbox.Left < 0)
                {
                    _selCalibrationPoint.SetX(0);
                }
                if (_selCalibrationPoint.Hitbox.Bottom > pb_calibratorBox.Height)
                {
                    _selCalibrationPoint.SetY(pb_calibratorBox.Height - _selCalibrationPoint.Hitbox.Height);
                }
                pb_calibratorBox.Refresh();
            }
            //255,162
            if (pb_calibratorMagnifier.Image != null)
                pb_calibratorMagnifier.Image.Dispose();

            if (pb_calibratorBox.Image != null && _refbmp != null)
                pb_calibratorMagnifier.Image = Crop(_refbmp, new Rectangle(e.X - 25, e.Y - 17, 50, 31));
        }

        // call me lazy if you will - this is copied from OCR.cs
        public Bitmap Crop(Bitmap b, Rectangle r)
        {
            // From http://stackoverflow.com/questions/734930/how-to-crop-an-image-using-c

            var nb = new Bitmap(r.Width, r.Height);
            var g = Graphics.FromImage(nb);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(b, -r.X, -r.Y);
            g.ScaleTransform(4f,4f);
            return nb;
        }

        private void Pb_calibratorMagnifier_Paint(object sender, PaintEventArgs e)
        {
            if (pb_calibratorMagnifier.Image != null)
            {
                e.Graphics.DrawLine(new Pen(Color.LightGray), pb_calibratorMagnifier.Width/2, 0,
                    pb_calibratorMagnifier.Width/2, pb_calibratorMagnifier.Height);
                e.Graphics.DrawLine(new Pen(Color.LightGray), 0, pb_calibratorMagnifier.Height / 2,
                    pb_calibratorMagnifier.Width, pb_calibratorMagnifier.Height/2);
            
            }
        }

        private void Tb_uicolor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var red = int.Parse(tb_uicolor.Text.Substring(1, 2),NumberStyles.HexNumber);
                var green = int.Parse(tb_uicolor.Text.Substring(3, 2), NumberStyles.HexNumber);
                var blue = int.Parse(tb_uicolor.Text.Substring(5, 2), NumberStyles.HexNumber);

                pb_uicolor.BackColor = Color.FromArgb(red, green, blue);

                Form1.RegulatedNoiseSettings.UiColour = tb_uicolor.Text;
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
            
            
        }

        private void Btn_calibration_reset_Click(object sender, EventArgs e)
        {
            if (Form1.GameSettings.Display != null)
            {
                var calibrations = Form1.OcrCalibrator.GetCalculatedCalibrationPoints(Form1.GameSettings.Display.Resolution);
                DrawCalibrationPoints(calibrations);
                FillRawData();
                pb_calibratorBox.Refresh();
                Form1.OcrCalibrator.SaveCalibration();
                return;
            }

            MessageBox.Show("Unable to calibrate automatically, please calibrate manually...");
        }
       
    }
}
