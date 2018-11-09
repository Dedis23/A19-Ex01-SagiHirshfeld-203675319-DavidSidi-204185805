using System;

namespace SpaceInvaders
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();

            // Hello Dedi, this is Sagi's GIT test!
            // Hi Sagi, this is Boaz. do you even calculus bro?
        }
    }
#endif
}
