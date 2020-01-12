using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ConsoleApp3
{
    class Zero: Character
    {
        float timeToNextAtk; // Zero can only shoot when timeToNextAtk = 0
        public Bitmap bmp;
        public Bitmap bmpMirror;

        private int curFrame;
        private float timeToNextFrame; // only update frame when timeToNextFrame = 0

        bool shooting;

        public Zero(Point location, int width, int height, float health, int kind = 0)
            :
            base(location, width, height, health, kind)
        {
            bmp = new Bitmap("megaman.png");
            bmp.MakeTransparent();
            bmpMirror = new Bitmap("megaman.png");
            bmpMirror.RotateFlip(RotateFlipType.RotateNoneFlipX);
            bmpMirror.MakeTransparent();
        }

        public override void Update(double dt, World world)
        {
            #region update position
            // update this position
            if (this.wallJumpTime > 0)
            {
                // if in wall jump time, dont change velocity
            }
            else if (Keyboard.KeyPress(Keys.Left))
            {
                this.velocity = new PointF(-140, this.velocity.Y);
                this.direction = 0;
            }
            else if (Keyboard.KeyPress(Keys.Right))
            {
                this.velocity = new PointF(140, this.velocity.Y);
                this.direction = 1;
            }
            else
            {
                this.velocity = new PointF(0, this.velocity.Y);
            }

            // update wall jump time
            this.wallJumpTime -= (float)dt;

            // check if this can jump or not
            if (!this.isOnTheGround && !this.isHangingWall)
            {
                this.canJump = false;
            }
            if (this.isOnTheGround && !Keyboard.KeyPress(Keys.Space))
            {
                this.canJump = true;
            }
            if (this.isHangingWall && !Keyboard.KeyPress(Keys.Space))
            {
                this.canJump = true;
            }

            // if this can jump and key up is press, then jump
            if (Keyboard.KeyPress(Keys.Space) && this.canJump)
            {
                if (!this.isHangingWall)
                {
                    this.velocity = new PointF(this.velocity.X, -600);
                }
                else
                {
                    this.velocity = new PointF(-this.velocity.X, -630);
                    this.wallJumpTime = 0.18f;
                }
                this.isOnTheGround = false;
                this.isJumping = true;
                this.canJump = false;
            }

            // apply gravity 

            if (this.velocity.Y < 0 && this.isJumping && Keyboard.KeyPress(Keys.Space) == true)
            {
                this.velocity = new PointF(this.velocity.X, this.velocity.Y + world.gravity * (float)dt * 1.5f);
            }
            else if (this.velocity.Y < 0 && Keyboard.KeyPress(Keys.Space) == false)
            {
                this.velocity = new PointF(this.velocity.X, this.velocity.Y + world.gravity * (float)dt * 3.5f);
            }
            else
            {
                this.velocity = new PointF(this.velocity.X, this.velocity.Y + world.gravity * (float)dt * 1.5f);
            }

            // collision 
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
                            this.isJumping = false;
                        }
                    }
                }
            }

            // after check collision, we know if this is hanging on the edge of the wall or not
            if (this.isHangingWall && !this.isOnTheGround)
            {
                // when this is hanging on the wall, make him slide down slowly
                this.velocity = new PointF(0, 80);
            }

            this.position = new PointF(newthisPosX, newthisPosY);

            #endregion

            #region update shooting

            timeToNextAtk -= (float)dt;
            if(Keyboard.KeyPress(Keys.C) && timeToNextAtk < 0)
            {
                if(this.direction == 0)
                {
                    world.bullets.Add(new ZeroBullet(new PointF(this.position.X, this.position.Y + 30), new PointF(-600, 0), 30));
                }
                if (this.direction == 1)
                {
                    world.bullets.Add(new ZeroBullet(new PointF(this.position.X + this.width, this.position.Y + 30), new PointF(600, 0), 30));
                }
                timeToNextAtk = 0.15f;
            }

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

            #region update Frame

            timeToNextFrame -= (float)dt;
            if(timeToNextFrame < 0)
            {
                curFrame++;
                timeToNextFrame = 0.05f;
            }

            if(velocity.X > 0 || velocity.X < 0)
            {
                curFrame %= 10;
                if(curFrame == 0)
                {
                    curFrame = 2;
                }
            }

            if(Keyboard.KeyPress(Keys.C))
            {
                shooting = true;
            }
            else
            {
                shooting = false;
            }

            #endregion
        }

        public new void Draw(Graphics gfx, int xCam, int yCam)
        {
            int yFrame;
            if(shooting)
            {
                yFrame = 1;
            }
            else
            {
                yFrame = 0;
            }
            if (velocity.X > 0 && velocity.Y == 0) // running right
            {
                gfx.DrawImage(bmp, new Rectangle((int)position.X - xCam - 17, (int)position.Y - yCam - 11, 66, 82), 66 * curFrame, yFrame * 82, 66, 82, GraphicsUnit.Pixel);
            }
            else if(velocity.X < 0 && velocity.Y == 0) // running left
            {
                gfx.DrawImage(bmpMirror, new Rectangle((int)position.X - xCam - 17, (int)position.Y - yCam - 11, 66, 82), 66 * (9 - curFrame), yFrame * 82, 66, 82, GraphicsUnit.Pixel);
            }
            else if(direction == 1 && velocity.Y != 0) // jump or fall with face right
            {
                gfx.DrawImage(bmp, new Rectangle((int)position.X - xCam - 17, (int)position.Y - yCam - 11, 66, 82), 66, yFrame * 82, 66, 82, GraphicsUnit.Pixel);
            }
            else if(direction == 0 && velocity.Y != 0) // jump or fall with face left
            {
                gfx.DrawImage(bmpMirror, new Rectangle((int)position.X - xCam - 17, (int)position.Y - yCam - 11, 66, 82), 66*(9-1), yFrame * 82, 66, 82, GraphicsUnit.Pixel);
            }
            else if(direction == 1) // standing with face right
            {
                gfx.DrawImage(bmp, new Rectangle((int)position.X - xCam - 17, (int)position.Y - yCam - 11, 66, 82), 66*0, yFrame * 82, 66, 82, GraphicsUnit.Pixel);
            }
            else if(direction == 0) // standing with face left
            {
                gfx.DrawImage(bmpMirror, new Rectangle((int)position.X - xCam - 17, (int)position.Y - yCam - 11, 66, 82), 66*(9-0), yFrame * 82, 66, 82, GraphicsUnit.Pixel);
            }
        }
    }
}
