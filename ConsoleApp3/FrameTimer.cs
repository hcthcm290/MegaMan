using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class FrameTimer
    {
        DateTime time;

        public FrameTimer()
        {
            time = DateTime.Now;
        }

        public double GetDt()
        {
            double dt = (DateTime.Now - time).TotalSeconds;

            time = DateTime.Now;

            return dt;
        }
    }
}
