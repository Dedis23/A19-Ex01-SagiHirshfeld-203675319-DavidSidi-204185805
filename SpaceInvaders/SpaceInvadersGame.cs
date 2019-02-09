using System;
using Infrastructure;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceInvaders
{
    public class SpaceInvadersGame : Game2D
    {
        private const float k_InitialMediaVolume = 0.5f;
        private const float k_InitialSoundEffectsVolume = 0.5f;

        public GameState GameState { get; set; }

        public SpaceInvadersGame()
        {
            this.Window.Title = "Space Invaders";
            this.IsMouseVisible = true;
            this.Background = new SpaceBG(this);
            this.GameState = new GameState(this);
            ScreensMananger screensMananger = new ScreensMananger(this);
            screensMananger.Push(new PlayScreen(this));
            screensMananger.SetCurrentScreen(new WelcomeScreen(this));
        }

        protected override void Initialize()
        {
            base.Initialize();
            SoundManager.MediaVolume = k_InitialMediaVolume;
            SoundManager.SoundEffectsVolume = k_InitialSoundEffectsVolume;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            playBackgroundMusic();
        }

        private void playBackgroundMusic()
        {
            const bool v_Looped = true;
            Song bgMusic = Content.Load<Song>(@"Audio\BGMusic");
            bgMusic.Play(v_Looped);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.M))
            {
                SoundManager.MuteAllSound = !SoundManager.MuteAllSound;
            }
        }
    }
}