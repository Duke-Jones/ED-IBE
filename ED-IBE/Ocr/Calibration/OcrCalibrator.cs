using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace IBE
{
    public class OcrCalibrator
    {

        private static Point[] _calibrationTemplate
        {
            get
            {
               var p = new Point[12];
                p[0].X = 102;
                p[0].Y = 90;
                p[1].X = 823;
                p[1].Y = 130;
                p[2].X = 106;
                p[2].Y = 326;
                p[3].X = 590;
                p[3].Y = 326;
                p[4].X = 706;
                p[4].Y = 326;
                p[5].X = 823;
                p[5].Y = 326;
                p[6].X = 948;
                p[6].Y = 326;
                p[7].X = 1098;
                p[7].Y = 326;
                p[8].X = 1191;
                p[8].Y = 326;
                p[9].X = 1346;
                p[9].Y = 326;
                p[10].X = 1460;
                p[10].Y = 326;
                p[11].X = 106;
                p[11].Y = 1300;

                return p;
            }
        }

        private static Point _resolutionTemplate = new Point
        {
            X = 2560,
            Y = 1440
        };

        public static List<CalibrationPoint> CalibrationBoxes;

        public static void SaveCalibration()
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
        public static void LoadCalibration()
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
            

          //TODO is this ever called?

             LoadCalibration();
        }
        public static Point[] GetCalculatedCalibrationPoints(Point resolution)
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
