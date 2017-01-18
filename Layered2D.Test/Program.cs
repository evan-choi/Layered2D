using System;
using System.Windows.Forms;
using Layered2D.Windows;

namespace Layered2D.Test
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var lw = new LayeredForm();

            RenderLoop.Run(
                lw,
                new RenderLoop.RenderCallback(() =>
                {
                    lw.Render();
                }));
        }
    }
}
