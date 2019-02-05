using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.Utilities;
using Microsoft.Xna.Framework;


namespace SpaceInvaders
{
    public class LevelTransitionScreen : GameScreen
    {
        private const int k_CountdownDurationInSeconds = 3;
        private TextSprite m_LevelAnnouncementTextSprite;
        private TextSprite m_CountDownTextSprite;
        private Timer m_Timer;

        /// private int m_SecondsLeftInCountdown = k_CountdownDurationInSeconds;
        /// 
        /// This is here instead of the above code to speed up level transitions while developing
        private float m_SecondsLeftInCountdown = k_CountdownDurationInSeconds / 3;

        public LevelTransitionScreen(Game i_Game) : base(i_Game)
        {
            m_LevelAnnouncementTextSprite = new TextSprite(@"Fonts\LevelTransitionFont", i_Game);
            m_LevelAnnouncementTextSprite.Text = string.Format("Level: 1");
            this.Add(m_LevelAnnouncementTextSprite);

            m_CountDownTextSprite = new TextSprite(@"Fonts\LevelTransitionFont", i_Game);
            m_CountDownTextSprite.Text = m_SecondsLeftInCountdown.ToString();
            m_CountDownTextSprite.TintColor = Color.White;
            this.Add(m_CountDownTextSprite);

            m_Timer = new Timer(i_Game);

            /// m_Timer.IntervalInSeconds = 1;
            ///         
            /// This is here instead of the above code to speed up level transitions while developing
            m_Timer.IntervalInSeconds = 0.33f;

            m_Timer.Notify += onTimerNotification;
            this.Add(m_Timer);
        }

        public override void Initialize()
        {
            base.Initialize();
            m_LevelAnnouncementTextSprite.Position = CenterOfViewPort - new Vector2(m_LevelAnnouncementTextSprite.Width / 2, 0);
            m_CountDownTextSprite.Position = CenterOfViewPort + new Vector2(-m_CountDownTextSprite.Width / 2, m_LevelAnnouncementTextSprite.Height);
        }
         
        public string Text
        {
            get
            {
                return m_LevelAnnouncementTextSprite.Text;
            }
            set
            {
                m_LevelAnnouncementTextSprite.Text = value;
            }
        }

        private void onTimerNotification()
        {
            m_SecondsLeftInCountdown--;
            m_CountDownTextSprite.Text = m_SecondsLeftInCountdown.ToString();

            if (m_SecondsLeftInCountdown == 0)
            {
                ScreensManager.SetCurrentScreen(PreviousScreen);
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            m_SecondsLeftInCountdown = k_CountdownDurationInSeconds;
            m_CountDownTextSprite.Text = m_SecondsLeftInCountdown.ToString();
            m_Timer.Activate();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            m_Timer.Deactivate();
        }
    }
}
