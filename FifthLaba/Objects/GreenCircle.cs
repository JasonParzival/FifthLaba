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
        private float current = 50f; // Начальный размер
        private const float min = 1f; // Минимальный размер
        private const float speed = 0.2f; // Скорость уменьшения
        public GreenCircle(float x, float y, float angle) : base(x, y, angle) 
        {

        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Lime), 
                -current / 2, -current / 2,
                current, current
                );
            g.DrawEllipse(new Pen(Color.Green, 2),
                     -current / 2, -current / 2,
                     current, current
                     );
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-current / 2, -current / 2, current, current);
            return path;
        }

        public void Update()
        {
            current = Math.Max(min, current - speed);
        }

        public bool disappear
        {
            get { return current <= min; }
        }
    }
}
