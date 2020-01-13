using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApp3
{
    abstract class Character
    {
        public PointF position { get; set; }
        public PointF velocity { get; set; }
        public int width { get; }
        public int height { get; }
        public bool isOnTheGround { get; set; }
        public bool isJumping { get; set; }
        public bool canJump { get; set; }
        public bool isHangingWall { get; set; }
        public float wallJumpTime { get; set; }
        public int direction { get; set; } // direction show the character is face left or right
                                           // 0: left
                                           // 1: right
        public int kind { get; set; }// this show what kind of character, 
                                     // if the bullet and character is the same kind, the bullet won't hurt
        public float maxHealth { get; set; }

        public float curHealth { get; set; }

        public Character(Point location, int width, int height, float maxHealth, int kind = 0)
        {
            this.position = location;
            this.width = width;
            this.height = height;
            this.isOnTheGround = false;
            this.wallJumpTime = 0f;
            this.kind = kind;
            this.maxHealth = this.curHealth = maxHealth;
        }

        public virtual void Draw(Graphics gfx, int xCam, int yCam)
        {
            gfx.FillRectangle(new SolidBrush(Color.Blue), new Rectangle((int)position.X - xCam, (int)position.Y - yCam, width, height));
        }

        virtual public void Update(double dt, World world, Character character)
        {

        }

        public virtual void ApplyDmg(float dmg)
        {

        }
    }
}
