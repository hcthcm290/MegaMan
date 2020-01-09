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
        }
    }
}
