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
        Zero player;
        Point camera;
        FrameTimer ft;
        List<Character> monsters;

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
                         "##.................................#..######......" +
                         "##.........................................##....." +
                         "##................................................" +
                         "##.......................................#########" +
                         "##.................####..........#################" +
                         "##................................................" +
                         "##..........................................####.." +
                         "###.#######...#########.....################......" +
                         ".................................................." ;
            world = new World(map, wWidth, wHeight, 800);
            player = new Zero(new Point(100, 0), 34, 70, 100);
            monsters = new List<Character>();
            monsters.Add(new Peatnut(new Point(620, 110), 50, 50, 100, 0, 1));
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
            this.ClientSize = new System.Drawing.Size(925, 519);
            this.Name = "MainForm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.ResumeLayout(false);

        }

        private void Update(object sender, EventArgs e)
        {
            double dt = ft.GetDt();
            if(dt > 1)
            {
                dt = 1;
            }

            player.Update(dt, world);
            world.Update(dt);
            foreach(var monster in monsters)
            {
                monster.Update(dt, world);
            }

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
            foreach (var monster in monsters)
            {
                monster.Draw(gfx, camera.X, camera.Y);
            }
            //gfx.DrawString(player.position.X.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 50));
            //gfx.DrawString(((int)(player.position.Y / world.tileHeight) + (player.height / world.tileHeight)).ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 100));
            //gfx.DrawString(player.isOnTheGround.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 150));
            
            gfx.DrawRectangle(new Pen(new SolidBrush(Color.Blue), 3), new Rectangle(15, 15, 106, 20));
            gfx.FillRectangle(new SolidBrush(Color.Green), new Rectangle(18, 18, (int)player.curHealth, 15));
            gfx.DrawString(player.curHealth.ToString(), new Font("Times New Roman", 20), new SolidBrush(Color.Black), new Point(120, 150));

            //gfx.DrawImage(player, new Rectangle(0, 0, 100, 200), 0, 0, 30, 40, GraphicsUnit.Pixel);
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
