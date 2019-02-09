using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders
{
    public class LevelTransitionScreen : GameScreen
    {
        private const int k_CountdownDurationInSeconds = 3;
        private GameState m_GameState;
        private TextSprite m_LevelAnnouncementTextSprite;
        private TextSprite m_CountDownTextSprite;
        private Timer m_Timer;
        private SoundEffectInstance m_LevelClearedSoundEffectInstance;

        private int m_SecondsLeftInCountdown = k_CountdownDurationInSeconds;

        public LevelTransitionScreen(Game i_Game) : base(i_Game)
        {
            m_GameState = Game.Services.GetService<GameState>();

            m_LevelAnnouncementTextSprite = new TextSprite(i_Game, @"Fonts\LevelTransitionFont");
            m_LevelAnnouncementTextSprite.Text = string.Format("Level: {0}", m_GameState.LevelNumber + 1);
            this.Add(m_LevelAnnouncementTextSprite);

            m_CountDownTextSprite = new TextSprite(i_Game, @"Fonts\LevelTransitionFont");
            m_CountDownTextSprite.Text = m_SecondsLeftInCountdown.ToString();
            m_CountDownTextSprite.TintColor = Color.White;
            this.Add(m_CountDownTextSprite);

            m_Timer = new Timer(i_Game);

            /// m_Timer.IntervalInSeconds = 1;
            ///         
            /// This is here instead of the above code to speed up level transitions while developing
            m_Timer.IntervalInSeconds = 0.33f;

            m_Timer.Notify += onTimerNotification;
            m_Timer.Activate();
            this.Add(m_Timer);

        }

        public override void Initialize()
        {
            base.Initialize();
            m_LevelAnnouncementTextSprite.Position = CenterOfViewPort - new Vector2(m_LevelAnnouncementTextSprite.Width / 2, 0);
            m_CountDownTextSprite.Position = CenterOfViewPort + new Vector2(-m_CountDownTextSprite.Width / 2, m_LevelAnnouncementTextSprite.Height);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_LevelClearedSoundEffectInstance = Game.Content.Load<SoundEffect>(@"Audio\LevelWin").CreateInstance();
        }

        private void onTimerNotification()
        {
            m_SecondsLeftInCountdown--;
            m_CountDownTextSprite.Text = m_SecondsLeftInCountdown.ToString();

            if (m_SecondsLeftInCountdown == 0)
            {
                ExitScreen();
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (m_GameState.LevelNumber > 0)
            {
                m_LevelClearedSoundEffectInstance.Play();
            }
        }
    }
}
