using System;
using System.Drawing;
using OpenTK;

namespace GameEngine
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            Console.WriteLine("Hello World");
            GameWindow window = new GameWindow(640, 480);
            window.Run();
        }
    }
}
