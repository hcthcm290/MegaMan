using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class ZeroBullet: Bullet
    {
        public ZeroBullet(PointF position, PointF velocity, float damage, int kind = 0, float radius = 15)
            :
            base(position, velocity, damage, kind, radius)
        {
            bmp = new Bitmap("megaman-bullet.png");
            bmp.MakeTransparent();
            bmpMirror = new Bitmap("megaman-bullet.png");
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            bmpMirror.MakeTransparent();
        }

        public override void Update(double dt)
        {
            base.Update(dt);

            //update frame
            timeToNextFrame -= (float)dt;
            if (timeToNextFrame < 0)
            {
                curFrame++;
                curFrame %= 4;
                timeToNextFrame = 0.12f;
            }
        }

        public override void Draw(Graphics gfx, int xCam, int yCam)
        {
            if(velocity.X < 0)
            {
                gfx.DrawImage(bmp, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 30, 20), 30 * curFrame, 0, 30, 20, GraphicsUnit.Pixel);
            }
            else
            {
                gfx.DrawImage(bmpMirror, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 30, 20), 30 * curFrame, 0, 30, 20, GraphicsUnit.Pixel);
            }
        }
    }
}
