using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Bee: Character
    {
        Bitmap bmp;
        Bitmap bmpMirror;

        private float timeToNextFrame;
        private int curFrame;

        private float maxDirectionTime; // this make Bee fly with a direction in a time then change direction
        private float curDirectionTime; // show how long Bee have fly at current direction

        private float timeToNextAtk;

        public Bee(Point location, int width, int height, float health, int direction, int kind = 0)
            :
            base(location, width, height, health, kind)
        {
            this.direction = direction;

            bmp = new Bitmap("Bee.png");
            bmp.MakeTransparent();

            bmpMirror = new Bitmap("Bee.png");
            bmpMirror.RotateFlip(RotateFlipType.RotateNoneFlipX);
            bmpMirror.MakeTransparent();

            maxDirectionTime = 2;
            curDirectionTime = 0;

            if(direction == 0)
            {
                this.velocity = new PointF(-100, 0);
            }
            else
            {
                this.velocity = new PointF(100, 0);
            }
        }

        public void Update(double dt, World world)
        {
            // update frame
            timeToNextFrame -= (float)dt;
            if (timeToNextFrame < 0)
            {
                curFrame++;
                curFrame %= 9;
                timeToNextFrame = 0.2f;
            }

            // update velocity
            curDirectionTime += (float)dt;
            if (curDirectionTime > maxDirectionTime)
            {
                curDirectionTime = 0;
                this.velocity = new PointF(-this.velocity.X, this.velocity.Y);
            }

            #region check collision
            // check collision
            float newthisPosX = this.position.X + this.velocity.X * (float)dt;
            float newthisPosY = this.position.Y + this.velocity.Y * (float)dt;

            // reset this hanging wall status
            this.isHangingWall = false;

            if (this.velocity.X < 0)
            {
                // loop throught all brick in the left near the this from top to bottom of this
                for (int y = (int)newthisPosY / world.tileHeight; y <= (int)(newthisPosY + this.height - 1) / world.tileHeight; y++)
                {
                    if (world.GetChar((int)newthisPosX / world.tileWidth, y) == '#')
                    {
                        if (Function.checkCollision(new RectangleF(newthisPosX, this.position.Y, this.width, this.height),
                                          new RectangleF(world.GetPosOfTile((int)newthisPosX / world.tileWidth, y), new Size(world.tileWidth, world.tileHeight))))
                        {
                            newthisPosX = world.GetPosOfTile((int)newthisPosX / world.tileWidth, y).X + world.tileWidth;
                            this.isHangingWall = true;
                        }
                    }
                }
            }
            else if (this.velocity.X > 0)
            {
                // loop throught all brick in the right near the this from top to bottom of this
                for (int y = (int)newthisPosY / world.tileHeight; y <= (int)(newthisPosY + this.height - 1) / world.tileHeight; y++)
                {
                    if (world.GetChar((int)((newthisPosX + this.width) / world.tileWidth), y) == '#')
                    {
                        if (Function.checkCollision(new RectangleF(newthisPosX, this.position.Y, this.width, this.height),
                                           new RectangleF(world.GetPosOfTile((int)((newthisPosX + this.width) / world.tileWidth), y), new Size(world.tileWidth, world.tileHeight))))
                        {
                            newthisPosX = world.GetPosOfTile((int)((newthisPosX + this.width) / world.tileWidth), y).X - this.width;
                            this.isHangingWall = true;
                        }
                    }
                }
            }
            #endregion

            this.position = new PointF(newthisPosX, newthisPosY);
        }

        public override void Update(double dt, World world, Character character)
        {
            Update(dt, world);

            timeToNextAtk -= (float)dt;
            if(timeToNextAtk < 0)
            {
                timeToNextAtk = 2.5f;

                world.bullets.Add(new BeeBullet(new PointF(position.X + width / 2, position.Y + height / 2),
                                                Function.VectorMultiply(Function.GetNormalization(new PointF(character.position.X - this.position.X, character.position.Y - this.position.Y)), 150),
                                                20, this.kind));
            }
        }

        public override void Draw(Graphics gfx, int xCam, int yCam)
        {
            if (this.velocity.X < 0)
            {
                gfx.DrawImage(bmp, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 48, 46), 48 * curFrame, 0, 48, 46, GraphicsUnit.Pixel);
            }
            else
            {
                gfx.DrawImage(bmpMirror, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 48, 46), 48 * (8 - curFrame), 0, 48, 46, GraphicsUnit.Pixel);
            }
            gfx.DrawString(curDirectionTime.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 150));
        }
    }
}
