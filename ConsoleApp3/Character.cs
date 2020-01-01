using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApp3
{
    class Character
    {
        public PointF position { get; set; }
        public PointF velocity { get; set; }
        public int width { get; }
        public int height { get; }
        public bool isOnTheGround { get; set; }
        public bool isJumping { get; set; }
        public bool canJump { get; set; }

        public Character(Point location, int width, int height)
        {
            this.position = location;
            this.width = width;
            this.height = height;
            isOnTheGround = false;
        }

        public void Draw(Graphics gfx, int xCam, int yCam)
        {
            gfx.FillRectangle(new SolidBrush(Color.Blue), new Rectangle((int)position.X - xCam, (int)position.Y - yCam, width, height));
        }
    }
}
