using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

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
            backgroundMusic.Play();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.M))
            {
                SoundManager.IsAllSoundMuted = !SoundManager.IsAllSoundMuted;
            }
        }
    }
}