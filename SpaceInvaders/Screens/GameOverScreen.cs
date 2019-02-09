using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;

namespace SpaceInvaders
{
    public class GameOverScreen : GameScreen
    {
        private BackgroundSprite m_Background;
        private Sprite m_GameOverMsg;
        private Sprite m_InstructionsMsg;
        private TextSprite m_GameOverTextSprite;
        private SoundEffectInstance m_GameOverSoundEffectInstance;
        private bool m_PrevScreenIsMainMenu;
        private GameState m_GameState;

        public GameOverScreen(Game i_Game) : base(i_Game)
        {
            m_GameState = Game.Services.GetService<GameState>();

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

        public override void Initialize()
        {
            base.Initialize();

            m_GameOverTextSprite.Text = buildGameOverMessage();

            m_GameOverTextSprite.Position = new Vector2(
                CenterOfViewPort.X - (m_GameOverTextSprite.Width / 2),
                CenterOfViewPort.Y - (m_GameOverTextSprite.Height / 2));

            m_GameOverMsg.Animations.Add(new PulseAnimator("Pulse", TimeSpan.Zero, 1.15f, 0.7f));
            m_GameOverMsg.Animations.Enabled = true;
            m_GameOverMsg.PositionOrigin = m_GameOverMsg.SourceRectangleCenter;
            m_GameOverMsg.Position = new Vector2(CenterOfViewPort.X, m_GameOverMsg.Height / 2);

            m_InstructionsMsg.PositionOrigin = m_InstructionsMsg.SourceRectangleCenter;            
            m_InstructionsMsg.Position = new Vector2(CenterOfViewPort.X, this.GraphicsDevice.Viewport.Height - (m_InstructionsMsg.Height / 2));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_GameOverSoundEffectInstance = Game.Content.Load<SoundEffect>(@"Audio\GameOver").CreateInstance();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (m_PrevScreenIsMainMenu)
            {
                goBackToPlayScreen();
            }
            else
            {
                m_GameOverSoundEffectInstance.Play();
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
        }

        private string buildGameOverMessage()
        {
            string gameOverMessage;

            if (m_GameState.IsMultiplayer)
            {
                gameOverMessage = buildMultiplayerGameOverMessage();
            }
            else
            {
                gameOverMessage = buildSoloGameOverMessage();
            }

            return gameOverMessage;
        }

        private string buildMultiplayerGameOverMessage()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string nameOfTheWinner = m_GameState.Player1Score >= m_GameState.Player2Score ? m_GameState.Player1Name : m_GameState.Player2Name;
            stringBuilder.Append(string.Format("The winner is {0}!", nameOfTheWinner));
            stringBuilder.Append(Environment.NewLine);

            stringBuilder.Append(string.Format("{0} Score: {1}", m_GameState.Player1Name, m_GameState.Player1Score));
            stringBuilder.Append(Environment.NewLine);

            stringBuilder.Append(string.Format("{0} Score: {1}", m_GameState.Player2Name, m_GameState.Player2Score));
            stringBuilder.Append(Environment.NewLine);

            return stringBuilder.ToString();
            throw new NotImplementedException();
        }

        private string buildSoloGameOverMessage()
        {
            return string.Format("Your score is {0}", m_GameState.Player1Score);
        }
    }
}
