using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


namespace SpaceInvaders
{
    public class WelcomeScreen : GameScreen
    {
        private Sprite m_WelcomeMessage;
        private Sprite m_PressEnterMsg;
        private bool m_PrevScreenIsMainMenu;

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

        protected override void OnActivated()
        {
            base.OnActivated();
            if (m_PrevScreenIsMainMenu)
            {
                transitionToPlayScreen();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            else if (InputManager.KeyPressed(Keys.Enter))
            {
                transitionToPlayScreen();
            }

            else if (InputManager.KeyPressed(Keys.T))
            {
                ScreensManager.SetCurrentScreen(new MainMenu(Game));
                m_PrevScreenIsMainMenu = true;
            }
        }

        private void transitionToPlayScreen()
        {
            m_PrevScreenIsMainMenu = false;
            ExitScreen();
        }
    }
}
