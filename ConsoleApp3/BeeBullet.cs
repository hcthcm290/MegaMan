using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class BeeBullet: Bullet
    {
        public BeeBullet(PointF position, PointF velocity, float damage, int kind = 0, float radius = 15)
            :
            base(position, velocity, damage, kind, radius)
        {
            bmp = new Bitmap("BeeBullet.png");
            bmp.MakeTransparent();
            bmpMirror = new Bitmap("BeeBullet.png");
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
                curFrame %= 3;
                timeToNextFrame = 0.12f;
            }
        }

        public override void Draw(Graphics gfx, int xCam, int yCam)
        {
            if (velocity.X < 0)
            {
                gfx.DrawImage(bmp, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 17, 17), 17 * curFrame, 0, 17, 17, GraphicsUnit.Pixel);
            }
            else
            {
                gfx.DrawImage(bmpMirror, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 17, 17), 17 *(2 - curFrame), 0, 17, 17, GraphicsUnit.Pixel);
            }
        }
    }
}
