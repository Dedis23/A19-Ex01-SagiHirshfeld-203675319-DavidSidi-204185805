﻿using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


namespace SpaceInvaders
{
    public class WelcomeScreen : GameScreen
    {
        Sprite m_WelcomeMessage;
        Sprite m_PressEnterMsg;

        BackgroundSprite m_Background;
        public WelcomeScreen(Game i_Game)
            : base(i_Game)
        {
            m_Background = new SpaceBG(i_Game);
            this.Add(m_Background);

            m_WelcomeMessage = new Sprite(@"Sprites\Messages\WelcomeMsg", this.Game);
            this.Add(m_WelcomeMessage);

            m_PressEnterMsg = new Sprite(@"Sprites\Messages\WelcomeOptionsMsg", this.Game);
            this.Add(m_PressEnterMsg);
        }

        public override void Initialize()
        {
            base.Initialize();

            fitViewportToBackground();

            m_WelcomeMessage.Animations.Add(new PulseAnimator("Pulse", TimeSpan.Zero, 1.15f, 0.7f));
            m_WelcomeMessage.Animations.Enabled = true;
            m_WelcomeMessage.PositionOrigin = m_WelcomeMessage.SourceRectangleCenter;
            m_WelcomeMessage.RotationOrigin = m_WelcomeMessage.SourceRectangleCenter;
            m_WelcomeMessage.Position = CenterOfViewPort - new Vector2(0, m_WelcomeMessage.Height / 4);

            m_PressEnterMsg.PositionOrigin = m_PressEnterMsg.SourceRectangleCenter;
            m_PressEnterMsg.Position =
                new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y + m_WelcomeMessage.Height);
        }

        private void fitViewportToBackground()
        {
            GraphicsDeviceManager graphicsManager = Game.Services.GetService(typeof(GraphicsDeviceManager)) as GraphicsDeviceManager;
            if (graphicsManager != null)
            {
                graphicsManager.PreferredBackBufferWidth = (int)m_Background.Width;
                graphicsManager.PreferredBackBufferHeight = (int)m_Background.Height;
                graphicsManager.ApplyChanges();
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
                this.ScreensManager.SetCurrentScreen(new PlayScreen(Game));
            }

            else if (InputManager.KeyPressed(Keys.T))
            {
                /// this.ScreensManager.SetCurrentScreen(new OptionsMenuScreen(Game));
            }
        }
    }
}
