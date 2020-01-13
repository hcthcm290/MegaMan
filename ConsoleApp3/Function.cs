using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApp3
{
    // this class contain mathematic, operator, collision, ... functions that needed by program
    static class Function
    {
        static public bool checkCollision(RectangleF a, RectangleF b)
        {
            if (a.X >= (b.X - a.Width + 0.01) && a.X <= (b.X + b.Width - 0.01) &&
                a.Y >= (b.Y - a.Height + 0.01) && a.Y <= (b.Y + b.Height - 0.01))
            {
                return true;
            }
            return false;
        }

        static public bool checkCollision(double cx, double cy, double radius, Rectangle rec)
        {

            // temporary variables to set edges for testing
            double testX = cx;
            double testY = cy;

            // which edge is closest?
            if (cx < rec.X) testX = rec.X;      // test left edge
            else if (cx > rec.X + rec.Width) testX = rec.X + rec.Width;   // right edge
            if (cy < rec.Y) testY = rec.Y;      // top edge
            else if (cy > rec.Y + rec.Height) testY = rec.Y + rec.Height;   // bottom edge

            // get distance from closest edges
            double distX = cx - testX;
            double distY = cy - testY;
            double distance = Math.Sqrt((distX * distX) + (distY * distY));

            // if the distance is less than the radius, collision!
            if (distance <= radius)
            {
                return true;
            }
            return false;
        }

        static public PointF GetNormalization(PointF vector)
        {
            return new PointF(vector.X / (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y),
                              vector.Y / (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y));
        }

        static public PointF VectorMultiply(PointF vector, float multipler)
        {
            return new PointF(vector.X * multipler, vector.Y * multipler);
        }

        static public float Distance(PointF A, PointF B)
        {
            return (float)Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y));
        }
    }
}
