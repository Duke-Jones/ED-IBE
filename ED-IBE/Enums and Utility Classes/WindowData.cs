using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.Enums_and_Utility_Classes
{
    public class WindowData
    {
        public Rectangle        Position;
        public FormWindowState  State { get; set; }

        public WindowData()
        {
            Position.X          = -1;
            Position.Y          = -1;
            Position.Width      = -1;
            Position.Height     = -1;

            State = FormWindowState.Normal;
        }

        public FormWindowState State1
        {
            get
            {
                return State;
            }
            set
            {
                State = value;
            }
        }
        public String LocationString
        {
            get
            {
                String retValue;

                retValue = Position.Left.ToString()   + "|" +
                           Position.Top.ToString()    + "|" +
                           Position.Width.ToString() + "|" +
                           Position.Height.ToString();

                return retValue;
            }

            set
            {
                List<String> LocationData = value.Split(new char[] {'|'}).ToList();

                if (LocationData.Count == 4)
                {
                    Position.X              = Int32.Parse(LocationData[0]);
                    Position.Y              = Int32.Parse(LocationData[1]);
                    Position.Width          = Int32.Parse(LocationData[2]);
                    Position.Height         = Int32.Parse(LocationData[3]);
                }
            }
        }

        public Rectangle Location
        {
            get
            {
                return Position;
            }

            set
            {
                Position.X      = value.X;
                Position.Y      = value.Y;
                Position.Width  = value.Width;
                Position.Height = value.Height;
            }
        }

        public String StateString
        {
            get
            {
                String retValue;
                retValue = State.ToString();
                return retValue;
            }

            set
            {
                State    = (FormWindowState)Enum.Parse(typeof(FormWindowState), value, true);
            }
        }

        internal void SetValuesToForm(Form form)
        {
            form.Left           = Position.X;
            form.Top            = Position.Y;
            form.Width          = Position.Width;
            form.Height         = Position.Height;

            form.WindowState    = State;
        }

        internal void GetValuesFromForm(Form form)
        {
            Position.X      = form.Left;           
            Position.Y      = form.Top;            
            Position.Width  = form.Width;          
            Position.Height = form.Height;         
                            
            State           = form.WindowState;    
        }
    }
}
