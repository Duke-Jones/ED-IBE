using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegulatedNoise
{
    class OcrCalibrator
    {
        private readonly Point[] _calibrationTemplate;
        private readonly Point _resolutionTemplate;

        public OcrCalibrator()
        {
            _resolutionTemplate.X = 1920;
            _resolutionTemplate.Y = 1080;
            
            _calibrationTemplate = new Point[12];
            _calibrationTemplate[0].X = 80;
            _calibrationTemplate[0].Y = 67;
            _calibrationTemplate[1].X = 401;
            _calibrationTemplate[1].Y = 91;
            _calibrationTemplate[2].X = 80;
            _calibrationTemplate[2].Y = 249;
            _calibrationTemplate[3].X = 439;
            _calibrationTemplate[3].Y = 249;
            _calibrationTemplate[4].X = 528;
            _calibrationTemplate[4].Y = 249;
            _calibrationTemplate[5].X = 616;
            _calibrationTemplate[5].Y = 249;
            _calibrationTemplate[6].X = 708;
            _calibrationTemplate[6].Y = 249;
            _calibrationTemplate[7].X = 822;
            _calibrationTemplate[7].Y = 249;
            _calibrationTemplate[8].X = 892;
            _calibrationTemplate[8].Y = 249;
            _calibrationTemplate[9].X = 1007;
            _calibrationTemplate[9].Y = 249;
            _calibrationTemplate[10].X = 1093;
            _calibrationTemplate[10].Y = 249;
            _calibrationTemplate[11].X = 80;
            _calibrationTemplate[11].Y = 974;
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
