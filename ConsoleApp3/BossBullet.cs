using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class BossBullet: Bullet
    {
        private float delay; // delay time make bullet wait for a time to start moving
        
        private float maxDirectionTime;
        private float curDirectionTime;

        public BossBullet(PointF position, PointF velocity, float damage, int kind = 0, float radius = 15)
            :
            base(position, velocity, damage, kind, radius)
        {
            bmp = new Bitmap("BossBullet.png");
            bmp.MakeTransparent(Color.Black);
            bmpMirror = new Bitmap("BossBullet.png");
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            bmpMirror.MakeTransparent();
        }

        public BossBullet(PointF position, PointF velocity, float damage,float delay, float maxDirectionTime, int kind = 0, float radius = 15)
            :
            this(position, velocity, damage, kind, radius)
        {
            this.delay = delay;
            this.maxDirectionTime = maxDirectionTime;
        }

        public override void Update(double dt)
        {
            if(delay > 0)
            {
                delay -= (float)dt;
                return;
            }
            base.Update(dt);

            curDirectionTime += (float)dt;

            if(curDirectionTime >= maxDirectionTime)
            {
                curDirectionTime = 0;
                this.velocity = new PointF(velocity.X, -velocity.Y);
            }
        }

        public override void Draw(Graphics gfx, int xCam, int yCam)
        {
            int xFrame;
            int yFrame;

            if (velocity.X < 0)
                xFrame = 0;
            else if (velocity.X == 0)
                xFrame = 1;
            else
                xFrame = 2;

            if (velocity.Y < 0)
                yFrame = 0;
            else if (velocity.Y == 0)
                yFrame = 1;
            else
                yFrame = 2;

            gfx.DrawImage(bmp, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 26, 26), 26 * xFrame, 26 * yFrame, 26, 26, GraphicsUnit.Pixel);
        }
    }
}
