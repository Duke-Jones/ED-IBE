using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegulatedNoise.Enums_and_Utility_Classes;

public partial class FilterTest : Form
{
    private Bitmap _TestBitmap;
    private Rectangle _MagnifierPosition;
    private bool _MousebuttonIsDown = false;
    
    public FilterTest()
    {
        InitializeComponent();
    }

    public int CutoffLevel { get; set; }

    public Bitmap TestBitmap
    {
        get
        {
            return _TestBitmap;
        }
        set
        {
            _TestBitmap = value;
        }
    }

    private void FilterTest_Load(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        this.DialogResult = System.Windows.Forms.DialogResult.None;
        
        nudCutoffValue.Value = CutoffLevel;

        pbPicture.Image = RNGraphics.PreprocessScreenshot(_TestBitmap, 1, (int)(nudCutoffValue.Value));

        pbPicture.Size = pbPicture.Image.Size;
        Cursor = Cursors.Default;
    }

    private void cmdCloseOnly_Click(object sender, EventArgs e)
    {
        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.Close();
    }

    private void cmdSaveClose_Click(object sender, EventArgs e)
    {
        this.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.Close();
    }

    private void FilterTest_Shown(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        _MagnifierPosition = new Rectangle((paPicturePanel.Width / 2) - 25, (paPicturePanel.Height / 2) - 17, 50, 31); 
        
        setMagnifier();
        Cursor = Cursors.Default;
    }

    private void setMagnifier()
    {
        if (pb_calibratorMagnifier.Image != null)
            pb_calibratorMagnifier.Image.Dispose();

        if (_MagnifierPosition.Width>0 && _MagnifierPosition.Height > 0)
            pb_calibratorMagnifier.Image = RNGraphics.Crop((Bitmap)(pbPicture.Image), _MagnifierPosition);
    }

    private void FilterTest_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
    {
        Cursor = Cursors.WaitCursor;

        if (this.DialogResult == System.Windows.Forms.DialogResult.None)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        CutoffLevel = (int)(nudCutoffValue.Value);
        Cursor = Cursors.Default;
    }

    private void nudCutoffValue_ValueChanged(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        pbPicture.Image = RNGraphics.PreprocessScreenshot(_TestBitmap, 1, (int)(nudCutoffValue.Value));    
        setMagnifier();
        Cursor = Cursors.Default;
    }

    private void pbPicture_Click(object sender, MouseEventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        _MagnifierPosition = new Rectangle(e.X - 25, e.Y - 17, 50, 31);
        setMagnifier();
        Cursor = Cursors.Default;
    }

    private void pbSampleTooHigh_Click(object sender, EventArgs e)
    {
        //pb_calibratorMagnifier.Image.Save(@"C:\temp\SampleTooHigh.png");
    }

    private void pbSampleTooLow_Click(object sender, EventArgs e)
    {
        //pb_calibratorMagnifier.Image.Save(@"C:\temp\SampleTooLow.png");
    }

    private void pbPicture_MouseMove(object sender, MouseEventArgs e)
    {
        if (_MousebuttonIsDown)
        {
            _MagnifierPosition = new Rectangle(e.X - 25, e.Y - 17, 50, 31);
            setMagnifier();
        }

    }

    private void pbPicture_MouseUp(object sender, MouseEventArgs e)
    {
        _MousebuttonIsDown = false;
    }

    private void pbPicture_MouseDown(object sender, MouseEventArgs e)
    {
        _MousebuttonIsDown = true;
    }

   
}
