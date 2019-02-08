using System;

namespace SpaceInvaders
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new SpaceInvadersGame())
            {
                game.Run();
            }
        }
    }
#endif
}

// THIS IS A GIT TEST //
