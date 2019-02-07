﻿using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceInvaders
{
    public class GameOverScreen : GameScreen
    {
        private BackgroundSprite m_Background;
        private Sprite m_GameOverMsg;
        private Sprite m_InstructionsMsg;
        private TextSprite m_GameOverTextSprite;
        private bool m_PrevScreenIsMainMenu;

        public GameOverScreen(Game i_Game) : base(i_Game)
        {
            m_Background = new SpaceBG(i_Game);
            m_Background.TintColor = Color.PaleVioletRed;
            this.Add(m_Background);

            m_GameOverMsg = new Sprite(@"Sprites\Messages\GameOverMsg", i_Game);
            this.Add(m_GameOverMsg);

            m_GameOverTextSprite = new TextSprite(i_Game, @"Fonts\GameOverScoreFont");
            m_GameOverTextSprite.TintColor = Color.White;
            this.Add(m_GameOverTextSprite);

            m_InstructionsMsg = new Sprite(@"Sprites\Messages\GameOverOptionsMsg", i_Game);
            this.Add(m_InstructionsMsg);
        }

        public string Text
        {
            set
            {
                m_GameOverTextSprite.Text = value;
            }
        }


        public override void Initialize()
        {
            base.Initialize();

            m_GameOverTextSprite.Position = new Vector2(
                CenterOfViewPort.X - m_GameOverTextSprite.Width / 2,
                CenterOfViewPort.Y - m_GameOverTextSprite.Height / 2);

            m_GameOverMsg.Animations.Add(new PulseAnimator("Pulse", TimeSpan.Zero, 1.15f, 0.7f));
            m_GameOverMsg.Animations.Enabled = true;
            m_GameOverMsg.PositionOrigin = m_GameOverMsg.SourceRectangleCenter;
            m_GameOverMsg.Position = new Vector2(CenterOfViewPort.X, m_GameOverTextSprite.Position.Y - m_GameOverTextSprite.Height / 2);

            m_InstructionsMsg.PositionOrigin = m_InstructionsMsg.SourceRectangleCenter;
            m_InstructionsMsg.Position = new Vector2(CenterOfViewPort.X, m_GameOverTextSprite.Position.Y + m_GameOverTextSprite.Height * 1.5f);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (m_PrevScreenIsMainMenu)
            {
                goBackToPlayScreen();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            else if (InputManager.KeyPressed(Keys.Home))
            {
                goBackToPlayScreen();
            }

            else if (InputManager.KeyPressed(Keys.T))
            {
                ScreensManager.SetCurrentScreen(new MainMenu(Game));
                m_PrevScreenIsMainMenu = true;
            }
        }
        
        private void goBackToPlayScreen()
        {
            ExitScreen();
            m_PrevScreenIsMainMenu = false;
        }
    }
}
