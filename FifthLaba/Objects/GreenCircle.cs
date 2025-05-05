using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifthLaba.Objects
{
    internal class GreenCircle : BaseObject
    {
        public GreenCircle(float x, float y, float angle) : base(x, y, angle) 
        {

        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Lime), -10, -10, 40, 40);
            g.DrawEllipse(new Pen(Color.Green, 2), -10, -10, 40, 40);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-10, -10, 40, 40);
            return path;
        }
    }
}
