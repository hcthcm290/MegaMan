using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp3
{
    class Program
    {
        [STAThread]        
        static void Main(string[] args)
        {
            MainForm mainForm = new MainForm();
            Application.Run(mainForm);
        }
    }
}
