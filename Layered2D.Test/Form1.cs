using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Layered2D.Animations;
using System.Threading;

namespace Layered2D.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();   
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            int top = 0;

            var acts = new List<Action>();

            var btn2 = new Button()
            {
                Text = "ALL",
                Width = panel1.Width - 20,
                Height = 30,
                Left = 0,
                Top = 0
            };

            panel1.Controls.Add(btn2);
            btn2.Click += (s, ee) =>
            {
                foreach (Action a in acts)
                    a();
            };

            foreach (EasingTypes easing in Enum.GetValues(typeof(EasingTypes)).Cast<EasingTypes>().Reverse())
            {
                var btn = new Button()
                {
                    Text = easing.ToString(),
                    Width = 100,
                    Height = 30,
                    Left = 0,
                    Top = top
                };
                
                top += 40;
                panel1.Controls.Add(btn);

                var a = new Animation()
                {
                    From = 0,
                    To = panel1.Width - btn.Width - 100,
                    Duration = TimeSpan.FromMilliseconds(1000),
                    BeginTime = TimeSpan.FromMilliseconds(0),
                    Easing = easing
                };

                a.ValueChanged += (o, v) =>
                {
                    btn.Left = (int)v;
                };

                acts.Add(() =>
                {
                    a.Start(true);
                });
            }
        }

        private void Animation_ValueChanged(double obj)
        {
            
        }
    }
}
