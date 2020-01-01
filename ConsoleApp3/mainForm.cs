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
                         "...........................##...##....######......" +
                         ".................................................." +
                         "..............................#########..........." +
                         ".................................................." +
                         ".................................#################" +
                         ".................................................." +
                         "##############################..............####.." +
                         "###.##...##...#########.....################......" +
                         ".................................................." ;
            world = new World(map, wWidth, wHeight, 600);
            player = new Character(new Point(0, 0), 32, 32);
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
            if( a.X >= (b.X - a.Width + 0.1) && a.X <= (b.X + b.Width - 0.1) && 
                a.Y >= (b.Y - a.Height + 0.1) && a.Y <= (b.Y + b.Height - 0.1))
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
            if(Keyboard.KeyPress(Keys.Left))
            {
                player.velocity = new PointF(-100, player.velocity.Y);
            } 
            else if (Keyboard.KeyPress(Keys.Right))
            {
                player.velocity = new PointF(100, player.velocity.Y);
            }
            else
            {
                player.velocity = new PointF(0, player.velocity.Y);
            }

            // check if player can jump or not
            if(player.isOnTheGround && !Keyboard.KeyPress(Keys.Up))
            {
                player.canJump = true;
            }

            // if player can jump and key up is press, then jump
            if (Keyboard.KeyPress(Keys.Up) && player.canJump)
            {
                player.velocity = new PointF(player.velocity.X, -450);
                player.isOnTheGround = false;
                player.isJumping = true;
                player.canJump = false;
            }

            // apply gravity 
            if (player.velocity.Y > 0 && player.isJumping)
            {
                player.velocity = new PointF(player.velocity.X, player.velocity.Y + world.gravity * (float)dt * 2.7f);
            }
            else if (player.isJumping && Keyboard.KeyPress(Keys.Up) == false)
            {
                player.velocity = new PointF(player.velocity.X, player.velocity.Y + world.gravity * (float)dt * 2.0f);
            }
            else
            {
                player.velocity = new PointF(player.velocity.X, player.velocity.Y + world.gravity * (float)dt * 1.0f);
            }

            // collision 
            float newPlayerPosX = player.position.X + player.velocity.X * (float)dt;
            float newPlayerPosY = player.position.Y + player.velocity.Y * (float)dt;

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
                            newPlayerPosX = world.GetPosOfTile((int)newPlayerPosX / world.tileWidth + 1, y).X - world.tileWidth;
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
                // loop throught all brick in the bottom near the player from left to right of player
                for (int x = (int)newPlayerPosX / world.tileWidth; x <= (int)(newPlayerPosX + player.width - 1) / world.tileWidth; x++)
                {
                    if (world.GetChar(x, (int)newPlayerPosY / world.tileHeight + 1) == '#')
                    {
                        if (checkCollision(new RectangleF(newPlayerPosX, newPlayerPosY, player.width, player.height),
                                          new RectangleF(world.GetPosOfTile(x, (int)newPlayerPosY / world.tileHeight + 1), new Size(world.tileWidth, world.tileHeight))))
                        {
                            newPlayerPosY = world.GetPosOfTile(x, (int)newPlayerPosY / world.tileHeight + 1).Y - world.tileHeight;
                            player.isOnTheGround = true;
                            player.velocity = new PointF(player.velocity.X, 0);
                            player.isJumping = false;
                        }
                    }
                }
            }

            player.position = new PointF(newPlayerPosX, newPlayerPosY);

            // update camera position
            //camera = new Point((int)(player.position.X - this.Width / 2), (int)(player.position.Y - this.Height / 2));
            //if(camera.X < 0)
            //{
            //    camera = new Point(0, camera.Y);
            //}
            //if (camera.Y < 0)
            //{
            //    //camera = new Point(camera.X, 0);
            //}
            //if(camera.X > world.tileWidth*world.width - this.Width)
            //{
            //    camera = new Point(world.tileWidth * world.width - this.Width, camera.Y);
            //}
            //if (camera.Y > world.tileHeight * world.height - this.Height / 2)
            //{
            //    //camera = new Point(camera.X, world.tileHeight * world.height - this.Height / 2);
            //}

            this.Refresh(); // paint after update
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            world.Draw(gfx, camera.X, camera.Y);
            player.Draw(gfx, camera.X, camera.Y);
            gfx.DrawString(player.position.X.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 50));
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
