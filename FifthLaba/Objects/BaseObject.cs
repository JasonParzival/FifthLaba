using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace FifthLaba.Objects
{
    internal class BaseObject
    {
        public float X;
        public float Y;
        public float Angle;

        public BaseObject(float x, float y, float angle)
        {
            X = x;
            Y = y;
            Angle = angle;
        }

        public Matrix GetTransform()
        {
            var matrix = new Matrix();
            matrix.Translate(X, Y);
            matrix.Rotate(Angle);
            return matrix;
        }

        // добавил виртуальный метод для отрисовки
        public virtual void Render(Graphics g)
        {
            // тут пусто
        }
    }
}
