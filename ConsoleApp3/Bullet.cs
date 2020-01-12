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
        public float radius { get; set; } // the radius of the bullet
        public int kind { get; set; } // this show what kind of bullet, 
                                      // if the bullet and character is the same kind, the bullet won't hurt
        public float damage { get; set; }

        protected int curFrame;
        protected float timeToNextFrame;

        protected Bitmap bmp;
        protected Bitmap bmpMirror;

        public Bullet(PointF position, PointF velocity, float damage, int kind = 0, float radius = 15)
        {
            this.position = position;
            this.velocity = velocity;
            this.radius = radius;
            this.kind = kind;
            this.damage = damage;
        }

        virtual public void Update(double dt)
        {
            // update position 
            this.position = new PointF((float)(this.position.X + this.velocity.X * dt), (float)(this.position.Y + this.velocity.Y * dt));

        }

        virtual public void Draw(Graphics gfx, int xCam, int yCam)
        {
            
        }
    }
}
