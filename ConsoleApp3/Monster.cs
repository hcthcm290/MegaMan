using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Monster: Character
    {
        float timeToNextAtk;

        public Monster(Point location, int width, int height, float health, int direction, int kind = 0)
            :
            base(location, width, height, health, kind)
        {
            this.direction = direction;
            timeToNextAtk = 0;
        }

        public override void Update(double dt, World world)
        {
            timeToNextAtk -= (float)dt;
            if(timeToNextAtk < 0)
            {
                if (this.direction == 0)
                {
                    world.bullets.Add(new Bullet(this.position, new PointF(-500, 0), 30, 1));
                }
                if (this.direction == 1)
                {
                    world.bullets.Add(new Bullet(new PointF(this.position.X + this.width, this.position.Y), new PointF(500, 0), 30, 1));
                }
                timeToNextAtk = 0.5f;
            }
        }
    }
}
