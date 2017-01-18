using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WS = Layered2D.Interop.UnsafeNativeMethods.WindowStyles;

namespace Layered2D.Windows
{
    public class LayeredWindow : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.ExStyle |= (int)WS.EX_LAYERED;
                cp.ExStyle |= (int)WS.EX_TRANSPARENT;

                return cp;
            }
        }
    }
}
