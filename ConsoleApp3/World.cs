using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class World
    {
        public string map { get; }
        public int width { get; }
        public int height { get; }

        public int tileWidth { get; }
        public int tileHeight { get; }

        public float gravity { get; set; }

        // this contains all bullets are in the world
        public List<Bullet> bullets;

        public char GetChar(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                if(y == 1)
                {
                    return map[y * width + x];
                }
                return map[y * width + x];
            }
            return ' ';
        }
        public Point GetPosOfTile(int x, int y)
        {
            return new Point(x * tileWidth, y * tileHeight);
        }

        public World(string map, int width, int height, float gravity = 10)
        {
            this.map = map;
            this.width = width;
            this.height = height;
            tileWidth = tileHeight = 32;
            this.gravity = gravity;
            this.bullets = new List<Bullet>();
        }

        public void Update(double dt)
        {
            // make bullet disapear when hit the wall
            for (int i = 0; i < bullets.Count;)
            {
                bool collision = false;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (map[y * width + x] == '#' && 
                            Function.checkCollision(bullets[i].position.X + bullets[i].radius/2, bullets[i].position.Y + bullets[i].radius/2, bullets[i].radius,
                                                    new Rectangle(GetPosOfTile(x, y), new Size(tileWidth, tileHeight))))
                        {
                            collision = true;
                        }
                    }
                }
                if(collision)
                {
                    bullets.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            foreach (var bullet in bullets)
            {
                bullet.Update(dt);
            }
        }

        public void Draw(Graphics gfx, int xCam, int yCam)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    char c = GetChar(x, y);
                    switch (c)
                    {
                        case '#':
                            gfx.FillRectangle(new SolidBrush(Color.Red), new Rectangle(x * tileWidth - xCam, y * tileHeight - yCam, tileWidth, tileHeight));
                            break;
                        case '.':
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (var bullet in bullets)
            {
                bullet.Draw(gfx, xCam, yCam);
            }
        }
    }
}
