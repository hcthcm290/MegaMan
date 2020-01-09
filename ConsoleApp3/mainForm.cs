using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ConsoleApp3
{
    class MainForm : Form
    {
        Timer timer;
        World world;
        Character player;
        Point camera;
        FrameTimer ft;

        public MainForm()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += Update;
            timer.Start();
            int wWidth = 50;
            int wHeight = 10;
            string map = ".................................................." +
                         "##.........................###.....#..######......" +
                         "##.........................................##....." +
                         "##................................................" +
                         "##.......................................#########" +
                         "##...............................#################" +
                         "##................................................" +
                         "##..........................................####.." +
                         "###.##...##...#########.....################......" +
                         ".................................................." ;
            world = new World(map, wWidth, wHeight, 600);
            player = new Character(new Point(100, 0), 32, 64);
            camera = new Point(0, 0);
            ft = new FrameTimer();
            this.DoubleBuffered = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(500, 320);
            this.Name = "MainForm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.ResumeLayout(false);
        }

        private bool checkCollision(RectangleF a, RectangleF b)
        {
            if( a.X >= (b.X - a.Width + 0.01) && a.X <= (b.X + b.Width - 0.01) && 
                a.Y >= (b.Y - a.Height + 0.01) && a.Y <= (b.Y + b.Height - 0.01))
            {
                return true;
            }
            return false;
        }

        private void Update(object sender, EventArgs e)
        {
            double dt = ft.GetDt();
            if(dt > 1)
            {
                dt = 1;
            }

            // update player position
            if(player.wallJumpTime > 0)
            {
                // if in wall jump time, dont change velocity
            }
            else if(Keyboard.KeyPress(Keys.Left))
            {
                player.velocity = new PointF(-140, player.velocity.Y);
            } 
            else if (Keyboard.KeyPress(Keys.Right))
            {
                player.velocity = new PointF(140, player.velocity.Y);
            }
            else
            {
                player.velocity = new PointF(0, player.velocity.Y);
            }

            // update wall jump time
            player.wallJumpTime -= (float)dt;

            // check if player can jump or not
            if(!player.isOnTheGround && !player.isHangingWall)
            {
                player.canJump = false;
            }
            if(player.isOnTheGround && !Keyboard.KeyPress(Keys.Space))
            {
                player.canJump = true;
            }
            if(player.isHangingWall && !Keyboard.KeyPress(Keys.Space))
            {
                player.canJump = true;
            }

            // if player can jump and key up is press, then jump
            if (Keyboard.KeyPress(Keys.Space) && player.canJump)
            {
                if (!player.isHangingWall)
                {
                    player.velocity = new PointF(player.velocity.X, -360);
                }
                else
                {
                    player.velocity = new PointF(-player.velocity.X, -370);
                    player.wallJumpTime = 0.23f;
                }
                player.isOnTheGround = false;
                player.isJumping = true;
                player.canJump = false;
            }

            // apply gravity 
            
            if (player.velocity.Y > 0 && player.isJumping)
            {
                player.velocity = new PointF(player.velocity.X, player.velocity.Y + world.gravity * (float)dt * 1.5f);
            }
            else if (player.isJumping && Keyboard.KeyPress(Keys.Up) == false)
            {
                player.velocity = new PointF(player.velocity.X, player.velocity.Y + world.gravity * (float)dt * 1.1f);
            }
            else
            {
                player.velocity = new PointF(player.velocity.X, player.velocity.Y + world.gravity * (float)dt * 1.0f);
            }

            // collision 
            float newPlayerPosX = player.position.X + player.velocity.X * (float)dt;
            float newPlayerPosY = player.position.Y + player.velocity.Y * (float)dt;

            // reset player hanging wall status
            player.isHangingWall = false;

            if (player.velocity.X < 0)
            {
                // loop throught all brick in the left near the player from top to bottom of player
                for (int y = (int)newPlayerPosY / world.tileHeight; y <= (int)(newPlayerPosY + player.height - 1) / world.tileHeight; y++) 
                {
                    if (world.GetChar((int)newPlayerPosX / world.tileWidth, y) == '#') 
                    {
                        if(checkCollision(new RectangleF(newPlayerPosX, player.position.Y, player.width, player.height), 
                                          new RectangleF(world.GetPosOfTile((int)newPlayerPosX / world.tileWidth, y), new Size(world.tileWidth, world.tileHeight))))
                        {
                            newPlayerPosX = world.GetPosOfTile((int)newPlayerPosX / world.tileWidth, y).X + world.tileWidth;
                            player.isHangingWall = true;
                        }
                    }
                }
            }
            else if(player.velocity.X > 0)
            {
                // loop throught all brick in the right near the player from top to bottom of player
                for (int y = (int)newPlayerPosY / world.tileHeight; y <= (int)(newPlayerPosY + player.height - 1) / world.tileHeight; y++)
                {
                    if (world.GetChar((int)newPlayerPosX / world.tileWidth + 1, y) == '#')
                    {
                        if (checkCollision(new RectangleF(newPlayerPosX, player.position.Y, player.width, player.height),
                                           new RectangleF(world.GetPosOfTile((int)newPlayerPosX / world.tileWidth + 1, y), new Size(world.tileWidth, world.tileHeight))))
                        {
                            newPlayerPosX = world.GetPosOfTile((int)newPlayerPosX / world.tileWidth + player.width / world.tileWidth, y).X - world.tileWidth;
                            player.isHangingWall = true;
                        }
                    }
                }
            }
            
            if(player.velocity.Y < 0)
            {
                // loop throught all brick in the top near the player from left to right of player
                for (int x = (int)newPlayerPosX / world.tileWidth; x <= (int)(newPlayerPosX + player.width - 1) / world.tileWidth; x++)
                {
                    if (world.GetChar(x, (int)newPlayerPosY / world.tileHeight) == '#')
                    {
                        if (checkCollision(new RectangleF(newPlayerPosX, newPlayerPosY, player.width, player.height),
                                          new RectangleF(world.GetPosOfTile(x, (int)newPlayerPosY / world.tileHeight), new Size(world.tileWidth, world.tileHeight))))
                        {
                            newPlayerPosY = world.GetPosOfTile(x, (int)newPlayerPosY / world.tileHeight).Y + world.tileHeight;
                            player.velocity = new PointF(player.velocity.X, 0);
                        }
                    }
                }
            }
            else if(player.velocity.Y > 0)
            {
                player.isOnTheGround = false;
                // loop throught all brick in the bottom near the player from left to right of player
                for (int x = (int)newPlayerPosX / world.tileWidth; x <= (int)(newPlayerPosX + player.width - 1) / world.tileWidth; x++)
                {
                    if (world.GetChar(x, (int)((newPlayerPosY + player.height) / world.tileHeight)) == '#')
                    {
                        if (checkCollision(new RectangleF(newPlayerPosX, newPlayerPosY, player.width, player.height),
                                          new RectangleF(world.GetPosOfTile(x, (int)((newPlayerPosY + player.height) / world.tileHeight)), new Size(world.tileWidth, world.tileHeight))))
                        {
                            newPlayerPosY = (int)(world.GetPosOfTile(x, (int)((newPlayerPosY + player.height) / world.tileHeight)).Y - player.height);
                            player.isOnTheGround = true;
                            player.velocity = new PointF(player.velocity.X, 0);
                            player.isJumping = false;
                        }
                    }
                }
            }

            // after check collision, we know if player is hanging on the edge of the wall or not
            if (player.isHangingWall)
            {
                // when player is hanging on the wall, make him slide down slowly
                player.velocity = new PointF(0, 40);
            }

            player.position = new PointF(newPlayerPosX, newPlayerPosY);

            //update camera position
            camera = new Point((int)(player.position.X - this.Width / 2), (int)(player.position.Y - this.Height / 2));
            if(camera.X < 0)
            {
                camera = new Point(0, camera.Y);
            }
            if (camera.Y < 0)
            {
                //camera = new Point(camera.X, 0);
            }
            if(camera.X > world.tileWidth*world.width - this.Width)
            {
                camera = new Point(world.tileWidth * world.width - this.Width, camera.Y);
            }
            if (camera.Y > world.tileHeight * world.height - this.Height / 2)
            {
                //camera = new Point(camera.X, world.tileHeight * world.height - this.Height / 2);
            }

            this.Refresh(); // paint after update
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            world.Draw(gfx, camera.X, camera.Y);
            player.Draw(gfx, camera.X, camera.Y);
            //gfx.DrawString(player.position.X.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 50));
            gfx.DrawString(((int)(player.position.Y / world.tileHeight) + (player.height / world.tileHeight)).ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 100));
            gfx.DrawString(player.isOnTheGround.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 150));
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            Keyboard.AddKey(e.KeyCode);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            Keyboard.DelKey(e.KeyCode);
        }
    }
}
