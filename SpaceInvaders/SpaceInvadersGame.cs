using Infrastructure.Managers;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public class SpaceInvadersGame : Game2D
    {
        public SpaceInvadersGame()
        {
            this.Window.Title = "Space Invaders";
            this.IsMouseVisible = true;
            this.Background = new SpaceBG(this);
            ScreensMananger screensMananger = new ScreensMananger(this);
            screensMananger.SetCurrentScreen(new WelcomeScreen(this));
        }
    }
}