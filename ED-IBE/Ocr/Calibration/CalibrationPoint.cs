using System.Collections.Generic;
using System.Drawing;

namespace RegulatedNoise
{
    public class CalibrationPoint
    {
        public CalibrationPoint(int i, Point position)
        {
            Hitbox = new Rectangle(position.X - Offset.X, position.Y - Offset.Y, HitboxSize.X, HitboxSize.Y);
            Id = i;
            Description = CalibrationDescriptions[i];
            Example = new Bitmap("Calibration Examples\\" + (i+1) + ".png");
        }

        public CalibrationPoint()
        {
            
        }

        public Point Position
        {
            get
            {
                return new Point(Hitbox.Location.X + Offset.X, Hitbox.Location.Y + Offset.Y );
            }
        }

        public Rectangle Hitbox { get; set; }

        private static Point HitboxSize
        {
            get{ return new Point(20,20);}
        }

        public Point Offset
        {
            get { return new Point(HitboxSize.X / 2, HitboxSize.Y/2); }
        }

        public void SetPosition(Point pos)
        {
            Hitbox = new Rectangle(pos.X, pos.Y, Hitbox.Width, Hitbox.Height);
        }

        public void SetX(int x)
        {
            Hitbox = new Rectangle(x, Hitbox.Y, Hitbox.Width, Hitbox.Height);
        }
        public void SetY(int y)
        {
            Hitbox = new Rectangle(Hitbox.X, y, Hitbox.Width, Hitbox.Height);
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public Bitmap Example { get; set; }

        public readonly List<string> CalibrationDescriptions = new List<string>{
            "Select just to the top-left of the Station Name.",
            "Select to the bottom-right of the Station Name.  Don't worry if it's a short station name, we'll compensate for that.",
            "Select just to the bottom-left of the dividing line between the column headers and the commodities",
            "Select just to the left of the line dividing Goods and Sell.  Don't worry about the vertical position, just get the horizontal position right.",
            "Select just to the left of the line dividing Sell and Buy.  Don't worry about the vertical position, just get the horizontal position right.",
            "Select just to the left of the line dividing Buy and Cargo.  Don't worry about the vertical position, just get the horizontal position right.",
            "Select just to the left of the line dividing Cargo and Demand.  Don't worry about the vertical position, just get the horizontal position right.",
            "Select in between the Demand, and the Demand Level (LOW/MED/HIGH).  Don't worry about the vertical position, just get the horizontal position right.",
            "Select just to the left of the line dividing Demand and Supply.  Don't worry about the vertical position, just get the horizontal position right.",
            "Select in between the Supply, and the Supply Level (LOW/MED/HIGH).  Don't worry about the vertical position, just get the horizontal position right.",
            "Select just to the left of the line dividing Supply and Galactic Average.  Don't worry about the vertical position, just get the horizontal position right.",
            "Select at the bottom-left of the commodities list, just above the orange bar that separates it from the Exit button.",
        };
    }
}