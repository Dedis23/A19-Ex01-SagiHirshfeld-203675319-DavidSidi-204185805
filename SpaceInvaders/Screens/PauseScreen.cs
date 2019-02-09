using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class PauseScreen : GameScreen
    {
        private Sprite m_PausedMessage;

        public PauseScreen(Game i_Game) : base(i_Game)
        {
            this.IsModal = true;
            this.IsOverlayed = true;
            this.UseGradientBackground = true;
            this.BlackTintAlpha = 0.4f;
            this.UseFadeTransition = true;
            this.ActivationLength = this.DeactivationLength = TimeSpan.FromSeconds(0.5f);

            m_PausedMessage = new Sprite(@"Sprites\Messages\PausedMsg", this.Game);
            this.Add(m_PausedMessage);
        }

        public override void Initialize()
        {
            base.Initialize();
            m_PausedMessage.PositionOrigin = m_PausedMessage.SourceRectangleCenter;
            m_PausedMessage.Position = CenterOfViewPort;
            m_PausedMessage.Scales = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.R))
            {
                ExitScreen();
            }

            m_PausedMessage.Scales = new Vector2(TransitionPosition);
        }
    }
}
