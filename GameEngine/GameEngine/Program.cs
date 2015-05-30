using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace GameEngine
{
    class Program
    {

        [STAThread]
        public static void Main(string[] args)
        {

            using (DisplayWindow window = new DisplayWindow(1000,700,args[0]))
            {
                window.Run(120,120);
            }
        }
    }
}
