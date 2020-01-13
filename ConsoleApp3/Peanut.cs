using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Peatnut: Character
    {
        bool canShoot;
        float timeToNextFrame;

        Bitmap bmp;
        Bitmap bmpMirror;

        int curFrame;

        public Peatnut(Point location, int width, int height, float health, int direction, int kind = 0)
            :
            base(location, width, height, health, kind)
        {
            this.direction = direction;

            bmp = new Bitmap("Peanut.png");
            bmp.MakeTransparent(Color.White);

            bmpMirror = new Bitmap("Peanut.png");
            bmpMirror.RotateFlip(RotateFlipType.RotateNoneFlipX);
            bmpMirror.MakeTransparent(Color.White);
        }

        public void Update(double dt, World world)
        {
            // update frame
            timeToNextFrame -= (float)dt;
            if(timeToNextFrame < 0)
            {
                curFrame++;
                curFrame %= 4;
                timeToNextFrame = 0.2f;
            }

            if(curFrame == 3 && canShoot)
            {
                if (this.direction == 0)
                {
                    world.bullets.Add(new PeanutBullet(new PointF(this.position.X, this.position.Y + 10), new PointF(-500, 0), 30, this.kind));
                }
                if (this.direction == 1)
                {
                    world.bullets.Add(new PeanutBullet(new PointF(this.position.X + this.width, this.position.Y + 10), new PointF(500, 0), 30, this.kind));
                }
                canShoot = false;
            }
            if(curFrame != 3)
            {
                canShoot = true;
            }
        }


        public override void Update(double dt, World world, Character character)
        {
            Update(dt, world);

            // update taking dmg
            for (int i = 0; i < world.bullets.Count;)
            {
                if (Function.checkCollision(world.bullets[i].position.X + world.bullets[i].radius / 2, world.bullets[i].position.Y + world.bullets[i].radius / 2, world.bullets[i].radius,
                   new Rectangle((int)position.X, (int)position.Y, width, height)) &&
                   world.bullets[i].kind != kind)
                {
                    curHealth -= world.bullets[i].damage;
                    world.bullets.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public override void Draw(Graphics gfx, int xCam, int yCam)
        {
            if(direction == 1)
            {
                gfx.DrawImage(bmp, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 50, 50), 50 * curFrame, 0, 50, 50, GraphicsUnit.Pixel);
            }
            else
            {
                gfx.DrawImage(bmpMirror, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 50, 50), 50 * (3 - curFrame), 0, 50, 50, GraphicsUnit.Pixel);
            }
        }
    }
}
