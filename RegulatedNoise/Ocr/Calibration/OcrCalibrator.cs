using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegulatedNoise
{
    public class OcrCalibrator
    {
        private readonly Point[] _calibrationTemplate;
        private readonly Point _resolutionTemplate;

        public List<CalibrationPoint> calibrationBoxes;

        public void SaveCalibration()
        {
            if (File.Exists("Calibration.txt"))
                File.Delete("Calibration.txt");

            using (var writer = new StreamWriter(File.OpenWrite("Calibration.txt")))
            {
                foreach (var calibrationPoint in calibrationBoxes)
                {
                    writer.Write(calibrationPoint.Position.X + ";" + calibrationPoint.Position.Y + ";");
                }

            }
        }
        public void LoadCalibration()
        {
            if (!File.Exists("Calibration.txt")) return;

            using (var reader = new StreamReader(File.OpenRead("Calibration.txt")))
            {
                var readLine = reader.ReadLine();
                if (readLine == null) return;
                var coords = readLine.Split(';');

                if (calibrationBoxes == null)
                    calibrationBoxes = new List<CalibrationPoint>();

                calibrationBoxes.Clear();
                if (coords.GetLength(0) > 23) // new calibration
                {
                    for (var i = 0; i < 12; i++)
                    {
                        var r = new CalibrationPoint(i, new Point(int.Parse(coords[i * 2]), int.Parse(coords[i * 2 + 1])));
                        calibrationBoxes.Add(r);

                    }
                }
            }
        }

        public OcrCalibrator()
        {
            _resolutionTemplate.X = 2560;
            _resolutionTemplate.Y = 1440;

            _calibrationTemplate = new Point[12];
            _calibrationTemplate[0].X = 108;
            _calibrationTemplate[0].Y = 90;
            _calibrationTemplate[1].X = 600;
            _calibrationTemplate[1].Y = 126;
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
            _calibrationTemplate[11].X = 109;
            _calibrationTemplate[11].Y = 1300;
        }
        public Point[] getCalculatedCalibrationPoints(Point resolution)
        {
            if (resolution == _resolutionTemplate)
                return _calibrationTemplate;

            var calibration = new List<Point>();
            foreach (var point in _calibrationTemplate)
            {
                var p = new Point();
                //Get percentage increase/decrease and update point
                var incr = ((((float)(resolution.X - _resolutionTemplate.X)) / _resolutionTemplate.X) * 100) + 100;
                p.X = (int)(point.X * incr) / 100;

                incr = ((((float)(resolution.Y - _resolutionTemplate.Y)) / _resolutionTemplate.Y) * 100) + 100;
                p.Y = (int)(point.Y * incr) / 100;
                calibration.Add(p);
            }

            return calibration.ToArray();
        }
    }
}
