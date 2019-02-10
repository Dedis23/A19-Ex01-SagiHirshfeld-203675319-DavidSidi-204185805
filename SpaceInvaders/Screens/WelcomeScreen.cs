using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;

namespace SpaceInvaders
{
    public class WelcomeScreen : ScreenForkingToPlayScreenAndMenuScreen
    {
        private Sprite m_WelcomeMessage;
        private Sprite m_PressEnterMsg;

        protected override Keys ExitKey => Keys.Escape;

        protected override Keys TransitionToPlayScreenKey => Keys.Enter;

        protected override Keys TransitionToMenuScreenKey => Keys.T;

        public WelcomeScreen(Game i_Game)
            : base(i_Game)
        {
            m_WelcomeMessage = new Sprite(@"Sprites\Messages\WelcomeMsg", this.Game);
            this.Add(m_WelcomeMessage);

            m_PressEnterMsg = new Sprite(@"Sprites\Messages\WelcomeOptionsMsg", this.Game);
            this.Add(m_PressEnterMsg);
        }

        public override void Initialize()
        {
            base.Initialize();

            m_WelcomeMessage.Animations.Add(new PulseAnimator("Pulse", TimeSpan.Zero, 1.15f, 0.7f));
            m_WelcomeMessage.Animations.Enabled = true;
            m_WelcomeMessage.PositionOrigin = m_WelcomeMessage.SourceRectangleCenter;
            m_WelcomeMessage.Position = CenterOfViewPort - new Vector2(0, m_WelcomeMessage.Height / 4);

            m_PressEnterMsg.PositionOrigin = m_PressEnterMsg.SourceRectangleCenter;
            m_PressEnterMsg.Position =
                new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y + m_WelcomeMessage.Height);
        }
    }
}
