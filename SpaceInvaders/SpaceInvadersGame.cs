using Infrastructure.Managers;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class SpaceInvadersGame : Game2D
    {
        public SpaceInvadersGame()
        {
            this.Window.Title = "Space Invaders";
            this.IsMouseVisible = true;
            ScreensMananger screensMananger = new ScreensMananger(this);
            screensMananger.SetCurrentScreen(new PlayScreen(this));
        }
    }
}