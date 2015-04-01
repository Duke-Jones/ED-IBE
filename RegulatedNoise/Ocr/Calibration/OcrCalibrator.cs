using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RegulatedNoise
{
    public class OcrCalibrator
    {
        private readonly Point[] _calibrationTemplate;
        private readonly Point _resolutionTemplate;

        public List<CalibrationPoint> CalibrationBoxes;

        public void SaveCalibration()
        {
            if (File.Exists("Calibration.txt"))
                File.Delete("Calibration.txt");

            using (var writer = new StreamWriter(File.OpenWrite("Calibration.txt")))
            {
                foreach (var calibrationPoint in CalibrationBoxes)
                {
                    writer.Write(calibrationPoint.Position.X + ";" + calibrationPoint.Position.Y + ";");
                }

            }

            // otherwise the previous calculated (and now maybe wrong) digit width can result in bad OCR
            if (File.Exists(@"Brainerous\settings.ini"))
                File.Delete(@"Brainerous\settings.ini");

        }
        public void LoadCalibration()
        {
            if (!File.Exists("Calibration.txt")) return;

            using (var reader = new StreamReader(File.OpenRead("Calibration.txt")))
            {
                var readLine = reader.ReadLine();
                if (readLine == null) return;
                var coords = readLine.Split(';');

                if (CalibrationBoxes == null)
                    CalibrationBoxes = new List<CalibrationPoint>();

                CalibrationBoxes.Clear();
                if (coords.GetLength(0) > 23) // new calibration
                {
                    for (var i = 0; i < 12; i++)
                    {
                        var r = new CalibrationPoint(i, new Point(int.Parse(coords[i * 2]), int.Parse(coords[i * 2 + 1])));
                        CalibrationBoxes.Add(r);

                    }
                }
            }
        }

        public OcrCalibrator()
        {
            _resolutionTemplate.X = 2560;
            _resolutionTemplate.Y = 1440;

            _calibrationTemplate = new Point[12];
            _calibrationTemplate[0].X = 102;
            _calibrationTemplate[0].Y = 90;
            _calibrationTemplate[1].X = 823;
            _calibrationTemplate[1].Y = 130;
            _calibrationTemplate[2].X = 106;
            _calibrationTemplate[2].Y = 326;
            _calibrationTemplate[3].X = 590;
            _calibrationTemplate[3].Y = 326;
            _calibrationTemplate[4].X = 706;
            _calibrationTemplate[4].Y = 326;
            _calibrationTemplate[5].X = 823;
            _calibrationTemplate[5].Y = 326;
            _calibrationTemplate[6].X = 948;
            _calibrationTemplate[6].Y = 326;
            _calibrationTemplate[7].X = 1098;
            _calibrationTemplate[7].Y = 326;
            _calibrationTemplate[8].X = 1191;
            _calibrationTemplate[8].Y = 326;
            _calibrationTemplate[9].X = 1346;
            _calibrationTemplate[9].Y = 326;
            _calibrationTemplate[10].X = 1460;
            _calibrationTemplate[10].Y = 326;
            _calibrationTemplate[11].X = 106;
            _calibrationTemplate[11].Y = 1300;
        }
        public Point[] GetCalculatedCalibrationPoints(Point resolution)
        {
            var returnVal = _calibrationTemplate;

            if (resolution != _resolutionTemplate)
            {
                var calibration = new List<Point>();
                foreach (var point in _calibrationTemplate)
                {
                    var p = new Point();
                    //Get percentage increase/decrease and update point
                    var incr = ((((float) (resolution.X - _resolutionTemplate.X))/_resolutionTemplate.X)*100) + 100;
                    p.X = (int) (point.X*incr)/100;

                    incr = ((((float) (resolution.Y - _resolutionTemplate.Y))/_resolutionTemplate.Y)*100) + 100;
                    p.Y = (int) (point.Y*incr)/100;
                    calibration.Add(p);
                }
                returnVal = calibration.ToArray();
            }
            for (int i = 3; i < 11; i++)
                returnVal[i] = new Point(returnVal[i].X, returnVal[2].Y);

            returnVal[11] = new Point(returnVal[2].X, returnVal[11].Y);

            returnVal[1] = new Point(returnVal[5].X, returnVal[1].Y);

            return returnVal;
        }
    }
}
