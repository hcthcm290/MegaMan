using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp3
{
    class Keyboard
    {
        static List<Keys> keys = new List<Keys>();

        public static void AddKey(Keys keyName)
        {
            keys.Add(keyName);
        }

        public static void DelKey(Keys keyName)
        {
            keys.RemoveAll(x => x == keyName);
        }

        public static bool KeyPress(Keys keyName)
        {
            bool x = keys.Exists(y => y == keyName);
            return keys.Exists(z => z == keyName);
        }
    }
}
