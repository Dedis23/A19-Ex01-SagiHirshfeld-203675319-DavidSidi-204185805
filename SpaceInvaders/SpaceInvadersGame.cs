using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework.Audio;

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
            screensMananger.Push(new PlayScreen(this));
            screensMananger.SetCurrentScreen(new WelcomeScreen(this));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            SoundEffectInstance backgroundMusic = Content.Load<SoundEffect>(@"Audio\BGMusic").CreateInstance();
            backgroundMusic.IsLooped = true;
            backgroundMusic.Volume *= 0.5f;
            backgroundMusic.Play();
        }
    }
}