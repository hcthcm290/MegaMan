using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApp3
{
    class Bullet
    {
        public PointF velocity { get; set; }
        public PointF position { get; set; }
        public int kind { get; set; } // this show what kind of bullet, 
                                      // if the bullet and character is the same kind, the bullet won't hurt
        public void Update()
        {

        }
    }
}
