using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Boss: Character
    {
        Bitmap bmp;
        Bitmap bmpMirror;

        public bool activated;
        
        bool isThrusting; // this show if the boss is doing thrusting attack or not
        bool isShooting;  // this show if the boss is doing shooting attack or not

        float timeToNextAtk; // Boss can only attack if timeToNExtAtk < 0
        float timeToNextFrame; // Only update frame if timeToNextFrame < 0
        float shootingTime; // time for shooting frame

        int curFrame;

        public Boss(Point location, int width, int height, float health, int kind = 0)
            :
            base(location, width, height, health, kind)
        {
            bmp = new Bitmap("Boss.png");
            bmp.MakeTransparent();
            bmpMirror = new Bitmap("Boss.png");
            bmpMirror.RotateFlip(RotateFlipType.RotateNoneFlipX);
            bmpMirror.MakeTransparent();

            this.direction = 1;
        }

        public override void Update(double dt, World world, Character character)
        {
            if(!activated)
            {
                return;
            }
            if (!isShooting && !isThrusting)
            {
                timeToNextAtk -= (float)dt;
            }

            // update atk
            #region update atk
            if (timeToNextAtk < 0)
            {
                Random rd = new Random();
                int rdNumber = rd.Next(0, 5);
                if(rdNumber < 3)
                {
                    isThrusting = true;
                    isShooting = false;

                    if(this.direction == 0)
                    {
                        this.velocity = new PointF(-400, 0);
                    }
                    else
                    {
                        this.velocity = new PointF(400, 0);
                    }

                    timeToNextAtk = 99f;
                    curFrame = 0;
                }
                else
                {
                    timeToNextAtk = 3;
                    isShooting = true;
                    shootingTime = 1;
                    if(direction == 0)
                    {
                        world.bullets.Add(new BossBullet(new PointF(position.X, position.Y),
                                                         new PointF(-200, 0), 30, 0, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X, position.Y),
                                                         new PointF(-200, -200), 30, 0, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X, position.Y),
                                                         new PointF(-200, 0), 30, 1.3f, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X, position.Y),
                                                         new PointF(-200, -200), 30, 1.3f, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X, position.Y),
                                                         new PointF(-200, 0), 30, 2.6f, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X, position.Y),
                                                         new PointF(-200, -200), 30, 2.6f, (float)rd.Next(300, 1000) / 1000, kind, 26));
                    }
                    else
                    {
                        world.bullets.Add(new BossBullet(new PointF(position.X + width, position.Y),
                                                         new PointF(200, 0), 30, 0, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X + width, position.Y),
                                                         new PointF(200, -200), 30, 0, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X + width, position.Y),
                                                         new PointF(200, 0), 30, 1.3f, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X + width, position.Y),
                                                         new PointF(200, -200), 30, 1.3f, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X + width, position.Y),
                                                         new PointF(200, 0), 30, 2.6f, (float)rd.Next(300, 1000) / 1000, kind, 26));
                        world.bullets.Add(new BossBullet(new PointF(position.X + width, position.Y),
                                                         new PointF(200, -200), 30, 2.6f, (float)rd.Next(300, 1000) / 1000, kind, 26));
                    }
                }
            }

            #endregion
            // update frame
            #region update frame
            timeToNextFrame -= (float)dt;
            if (shootingTime > 0)
            {
                shootingTime -= (float)dt;
            }
            else
            {
                isShooting = false;
            }

            if (timeToNextFrame < 0)
            {
                if (isThrusting)
                {
                    curFrame++;
                    if (curFrame > 4 && velocity.X != 0)
                    {
                        curFrame = 4;
                    }
                    else
                    {
                        curFrame %= 7;
                    }
                    if(curFrame == 0)
                    {
                        isThrusting = false;
                    }
                }
                else if(isShooting)
                {

                }
                else
                {
                    curFrame = 0;
                }
                timeToNextFrame = 0.2f;
            }
            #endregion

            #region update position
            if (!isShooting && !isThrusting)
            {
                if(character.position.X < this.position.X)
                {
                    direction = 0;
                }
                else
                {
                    direction = 1;
                }
            }

            this.velocity = new PointF(this.velocity.X, this.velocity.Y + world.gravity * (float)dt);

            // update collision
            float newthisPosX = this.position.X + this.velocity.X * (float)dt;
            float newthisPosY = this.position.Y + this.velocity.Y * (float)dt;


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
                            this.velocity = new PointF(0, this.velocity.Y);
                            timeToNextAtk = 1.2f;
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
                            timeToNextAtk = 1.2f;
                            this.velocity = new PointF(0, this.velocity.Y);
                        }
                    }
                }
            }
            if (this.velocity.Y < 0)
            {
                // loop throught all brick in the top near the this from left to right of this
                for (int x = (int)newthisPosX / world.tileWidth; x <= (int)(newthisPosX + this.width - 1) / world.tileWidth; x++)
                {
                    if (world.GetChar(x, (int)newthisPosY / world.tileHeight) == '#')
                    {
                        if (Function.checkCollision(new RectangleF(newthisPosX, newthisPosY, this.width, this.height),
                                          new RectangleF(world.GetPosOfTile(x, (int)newthisPosY / world.tileHeight), new Size(world.tileWidth, world.tileHeight))))
                        {
                            newthisPosY = world.GetPosOfTile(x, (int)newthisPosY / world.tileHeight).Y + world.tileHeight;
                            this.velocity = new PointF(this.velocity.X, 0);
                        }
                    }
                }
            }
            else if (this.velocity.Y > 0)
            {
                this.isOnTheGround = false;
                // loop throught all brick in the bottom near the this from left to right of this
                for (int x = (int)newthisPosX / world.tileWidth; x <= (int)(newthisPosX + this.width - 1) / world.tileWidth; x++)
                {
                    if (world.GetChar(x, (int)((newthisPosY + this.height) / world.tileHeight)) == '#')
                    {
                        if (Function.checkCollision(new RectangleF(newthisPosX, newthisPosY, this.width, this.height),
                                          new RectangleF(world.GetPosOfTile(x, (int)((newthisPosY + this.height) / world.tileHeight)), new Size(world.tileWidth, world.tileHeight))))
                        {
                            newthisPosY = (int)(world.GetPosOfTile(x, (int)((newthisPosY + this.height) / world.tileHeight)).Y - this.height);
                            this.isOnTheGround = true;
                            this.velocity = new PointF(this.velocity.X, 0);
                        }
                    }
                }
            }

            this.position = new PointF(newthisPosX, newthisPosY);
            #endregion

            if (Function.checkCollision(new RectangleF(this.position, new Size(this.width, this.height)),
                                       new RectangleF(character.position, new Size(character.width, character.height))))
            {
                character.ApplyDmg(20);
            }

            #region update taking damage 

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

            #endregion
        }

        public new void Draw(Graphics gfx, int xCam, int yCam)
        {
            int yFrame;

            if(activated)
            {
                gfx.DrawRectangle(new Pen(new SolidBrush(Color.Blue), 3), new Rectangle(700, 29, 206, 20));
                gfx.FillRectangle(new SolidBrush(Color.Green), new Rectangle(704, 32, (int)(curHealth * 200 / maxHealth), 15));
                gfx.DrawString("Gwyn, Lord of Cinder", new Font("Times New Roman", 15), new SolidBrush(Color.Black), new PointF(704, 50));
            }

            if (isShooting)
            {
                yFrame = 1;
            }
            else
            {
                yFrame = 0;
            }

            if(direction == 1)
            {
                gfx.DrawImage(bmp, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 88, 64), 88 * curFrame, 64*yFrame, 88, 64, GraphicsUnit.Pixel);
            }
            else
            {
                gfx.DrawImage(bmpMirror, new Rectangle((int)position.X - xCam, (int)position.Y - yCam, 88, 64), 88 *(8 - curFrame), 64*yFrame, 88, 64, GraphicsUnit.Pixel);
            }
            gfx.DrawString(activated.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 50));
            //gfx.DrawString(isShooting.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 100));
            //gfx.DrawString(isThrusting.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 150));
            //gfx.DrawString(direction.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 200));

        }
    }
}
