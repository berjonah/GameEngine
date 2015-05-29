using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace GameEngine
{
    class Program
    {

        [STAThread]
        public static void Main()
        {
            using (DisplayWindow window = new DisplayWindow())
            {
                window.Run(30,30);
            }
        }
    }
}
