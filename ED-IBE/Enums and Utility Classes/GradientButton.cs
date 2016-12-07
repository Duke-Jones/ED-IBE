/*
Code revised from chapter 5


GDI+ Custom Controls with Visual C# 2005
By Iulian Serban, Dragos Brezoi, Tiberiu Radu, Adam Ward 

Language English
Paperback 272 pages [191mm x 235mm]
Release date July 2006
ISBN 1904811604

Sample chapter
http://www.packtpub.com/files/1604_CustomControls_SampleChapter.pdf
or
https://www.packtpub.com/sites/default/files/1604_CustomControls_SampleChapter.pdf


For More info on GDI+ Custom Control with Microsoft Visual C# book 
visit website www.packtpub.com 


*/ 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{

    public partial class GradientButton : BorderGradientPanel
    {
        public GradientButton()
        {
            UpdateAppearance();
            InitializeComponent();
        }
        private bool clicked = false;
        private void UpdateAppearance()
        {
            if (clicked)
            {
                StartColor = SystemColors.Control;
                EndColor = SystemColors.ControlLight;
                BorderStyle = Border3DStyle.Sunken;
            }
            else
            {
                StartColor = SystemColors.ControlLight;
                EndColor = SystemColors.Control;
                BorderStyle = Border3DStyle.Raised;
            }

        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                clicked = true;
                UpdateAppearance();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                clicked = false;
                UpdateAppearance();
            }
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Brush foreBrush = new SolidBrush(ForeColor);
            SizeF size = pe.Graphics.MeasureString(Text, Font);
            PointF pt = new PointF((Width - size.Width) / 2, (Height - size.Height) / 2);
            if (clicked)
            {
                pt.X += 2;
                pt.Y += 2;
            }
            pe.Graphics.DrawString(Text, Font, foreBrush, pt);
            foreBrush.Dispose();
        }
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion
        
    }


    public partial class BorderGradientPanel : Control
    {
        private Border3DStyle borderStyle = Border3DStyle.Sunken;
        private Color startColor = SystemColors.Control;
        private Color endColor = SystemColors.Control;
        public Color EndColor
        {
            get
            {
                return endColor;
            }
            set
            {
                if (endColor != value)
                {
                    endColor = value;
                    Invalidate();
                }
            }
        }
        public Color StartColor
        {
            get
            {
                return startColor;
            }
            set
            {
                if (startColor != value)
                {
                    startColor = value;
                    Invalidate();
                }
            }
            
        }
        public Border3DStyle BorderStyle
        {
            get
            {
                return borderStyle;
            }
            set
            {
                if (borderStyle != value)
                {
                    borderStyle = value;
                    Invalidate();
                }
            }
        }


        public BorderGradientPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            LinearGradientBrush brush  = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), startColor, endColor);
            
            Blend blend1 = new Blend(9);

            // Set the values in the Factors array to be all green, 
            // go to all blue, and then go back to green.
            blend1.Factors = new float[]{0.0F, 0.5F, 0.8F, 0.9F, 1.0F, 0.9F, 0.8F, 0.5F, 0.0F};

            // Set the positions.
            blend1.Positions =  new float[]{0.0F, 0.1F, 0.2F, 0.3F, 0.4F, 0.7F,  0.8F, 0.9F, 1.0F};

                brush.Blend = blend1;

            e.Graphics.FillRectangle(brush, ClientRectangle);
            ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, borderStyle);
            brush.Dispose();
        }

        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion
    }




    public class ScrollArrowButton : ControlPart
    {
        protected override void RenderControl(Graphics g, ButtonState buttonState, CheckState checkState)
        {
            //ControlPaint.DrawScrollButton(g, ClientRectangle, ScrollButton.Up,buttonState);
            ControlPaint.DrawScrollButton(g, ClientRectangle, sButton, buttonState);
        }
        private ScrollButton sButton = ScrollButton.Up;
        public ScrollButton ButtonType
        {
            get
            {
                return sButton;
            }
            set
            {
                if (sButton != value)
                {
                    sButton = value;
                    Invalidate();
                }
            }
        }
    }

    public class CheckButton : ControlPart
    {
        protected override void RenderControl(Graphics g, ButtonState buttonState, CheckState checkState)
        {
            ButtonState bstate = buttonState;
            switch (checkState)
            {
                case CheckState.Checked: bstate = ButtonState.Checked; break;
                case CheckState.Indeterminate: bstate = ButtonState.All; break;
            }
            ControlPaint.DrawCheckBox(g, ClientRectangle, bstate);
        }

    }

    public class ControlPart : Control
    {
        private ButtonState buttonState = ButtonState.Flat;
        private CheckState checkState = CheckState.Unchecked;
        //indicates wheter the mouse is hovering over the control
        protected bool mouseOver = false;
        public ControlPart()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            buttonState = ButtonState.Normal;
            mouseOver = true;
            Invalidate(true);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            buttonState = ButtonState.Flat;
            mouseOver = false;
            Invalidate(true);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();
            if (!(e.Button == MouseButtons.Left)) return;
            buttonState = ButtonState.Pushed;
            switch (checkState)
            {
                case CheckState.Checked: checkState = CheckState.Unchecked; break;
                case CheckState.Unchecked: checkState = CheckState.Checked; break;
                case CheckState.Indeterminate: checkState = CheckState.Unchecked; break;
            }
            Invalidate(true);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!((e.Button & MouseButtons.Left) == MouseButtons.Left)) return;
            buttonState = ButtonState.Normal;
            Invalidate(true);
        }
        protected virtual void RenderControl(Graphics g, ButtonState buttonState, CheckState checkState)
        {
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RenderControl(e.Graphics, buttonState, checkState);
        }

    }
}