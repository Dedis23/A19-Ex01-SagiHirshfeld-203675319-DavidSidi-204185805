using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using System.Text;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    class PlayScreen : GameScreen
    {
        private const string k_ScoreFontAsset = @"Fonts\ComicSansMS";

        private const float k_SpaceshipPositionYModifier = 1.5f;
        private const float k_LivesGapModifier = 0.7f;
        private const float k_GapBetweenRowsModifier = 1.2f;
        private const int k_LivesDistanceFromHorizontalScreenBound = 15;

        private CollisionHandler m_CollisionHandler;

        private Mothership m_Mothership;

        private Spaceship m_Player1Spaceship;
        private Spaceship m_Player2Spaceship;
        private PlayerLivesRow m_Player1Lives;
        private PlayerLivesRow m_Player2Lives;
        private PlayerScoreText m_Player1ScoreText;
        private PlayerScoreText m_Player2ScoreText;

        private InvadersMatrix m_InvadersMatrix;
        private DancingBarriersRow m_DancingBarriersRow;

        private PauseScreen m_PauseScreen;

        private bool m_GameOver = false;

        public PlayScreen(Game i_Game) : base(i_Game)
        {            
            this.BlendState = BlendState.NonPremultiplied;

            m_CollisionHandler = new CollisionHandler(i_Game);
            m_CollisionHandler.EnemyCollidedWithSpaceship += () => m_GameOver = true;

            m_Mothership = new Mothership(i_Game);
            this.Add(m_Mothership);

            loadSpaceships();
            loadLives();
            loadScoreSprites();
            loadInvadersMatrix();
            m_DancingBarriersRow = new DancingBarriersRow(i_Game);
            this.Add(m_DancingBarriersRow);

            m_PauseScreen = new PauseScreen(i_Game);
        }

        public override void Initialize()
        {
            base.Initialize();
            setMothershipPosition();
            setSpaceshipsPositions();            
            setLivesPositions();
            setScoreSpritesPositions();
            setBarriersPosition();
        }

        private void loadSpaceships()
        {
            m_Player1Spaceship = new Player1Spaceship(Game);
            m_Player1Spaceship.Died += onSpaceshipKilled;
            this.Add(m_Player1Spaceship);

            m_Player2Spaceship = new Player2Spaceship(Game);
            m_Player2Spaceship.Died += onSpaceshipKilled;
            this.Add(m_Player2Spaceship);
        }

        private void setSpaceshipsPositions()
        {
            Vector2 pos = new Vector2(0, GraphicsDevice.Viewport.Height - (m_Player1Spaceship.Height * k_SpaceshipPositionYModifier));

            m_Player1Spaceship.Position = m_Player1Spaceship.DefaultPosition = pos;
            m_Player2Spaceship.Position = m_Player2Spaceship.DefaultPosition = pos;

        }

        private void loadLives()
        {
            m_Player1Lives = new PlayerLivesRow(m_Player1Spaceship);
            m_Player2Lives = new PlayerLivesRow(m_Player2Spaceship);

            this.Add(m_Player1Lives);
            this.Add(m_Player2Lives);
        }

        private void setLivesPositions()
        {
            Sprite lifeIcon = m_Player1Lives.First;
            m_Player1Lives.GapBetweenSprites =  lifeIcon.Width * k_GapBetweenRowsModifier;
            m_Player2Lives.GapBetweenSprites = m_Player1Lives.GapBetweenSprites;
            m_Player1Lives.Position = new Vector2(GraphicsDevice.Viewport.Width - lifeIcon.Width - k_LivesDistanceFromHorizontalScreenBound, 0);
            m_Player2Lives.Position = m_Player1Lives.Position + new Vector2(0, lifeIcon.Height * k_GapBetweenRowsModifier);
        }

        private void loadScoreSprites()
        {
            m_Player1ScoreText = new PlayerScoreText(m_Player1Spaceship, k_ScoreFontAsset);
            m_Player2ScoreText = new PlayerScoreText(m_Player2Spaceship, k_ScoreFontAsset);

            this.Add(m_Player1ScoreText);
            this.Add(m_Player2ScoreText);
        }

        private void setScoreSpritesPositions()
        {
            m_Player1ScoreText.Position = Vector2.Zero;
            m_Player2ScoreText.Position = new Vector2(0, m_Player1ScoreText.Height);
        }

        private void setMothershipPosition()
        {
            m_Mothership.Position = m_Mothership.DefaultPosition = new Vector2(-m_Mothership.Width, m_Mothership.Height);
        }

        private void setBarriersPosition()
        {
            float distanceFromScreenHorizondalBounds = (GraphicsDevice.Viewport.Width - m_DancingBarriersRow.Width) / 2;
            m_DancingBarriersRow.Position = new Vector2(
                distanceFromScreenHorizondalBounds,
                m_Player1Spaceship.DefaultPosition.Y - (m_DancingBarriersRow.Height * 2));
        }

        private void loadInvadersMatrix()
        {
            m_InvadersMatrix = new InvadersMatrix(Game);
            this.Add(m_InvadersMatrix);

            m_InvadersMatrix.invadersMatrixReachedBottomScreen += () => m_GameOver = true;
            m_InvadersMatrix.AllInvadersWereDefeated += () => m_GameOver = true;
        }

        private void onSpaceshipKilled(object i_Spaceship)
        {
            this.Remove(i_Spaceship as Spaceship);
            if (!this.Contains(m_Player1Spaceship) && !this.Contains(m_Player2Spaceship))
            {
                m_GameOver = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            takeInput();

            if (m_GameOver)
            {
                showGameOverWindow();
                Game.Exit();
            }
        }

        private void takeInput()
        {
            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            else if (InputManager.KeyPressed(Keys.P))
            {
                this.ScreensManager.SetCurrentScreen(m_PauseScreen);
            }

            else
            {
                takePlayer1Input();
                takePlayer2Input();
            }
        }

        private void takePlayer1Input()
        {
            if (InputManager.KeyboardState.IsKeyDown(Keys.H))
            {
                m_Player1Spaceship.MoveLeft();
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.K))
            {
                m_Player1Spaceship.MoveRight();
            }

            if (InputManager.KeyPressed(Keys.U) || InputManager.ButtonPressed(eInputButtons.Left))
            {
                m_Player1Spaceship.Shoot();
            }

            m_Player1Spaceship.MoveAccordingToMousePositionDelta(InputManager.MousePositionDelta);
        }

        private void takePlayer2Input()
        {
            if (InputManager.KeyboardState.IsKeyDown(Keys.A))
            {
                m_Player2Spaceship.MoveLeft();
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.D))
            {
                m_Player2Spaceship.MoveRight();
            }

            if (InputManager.KeyPressed(Keys.W))
            {
                m_Player2Spaceship.Shoot();
            }
        }

        private void showGameOverWindow()
        {
            string gameOverMessage = buildGameOverMessage();
            System.Windows.Forms.MessageBox.Show(gameOverMessage, "Game Over!", System.Windows.Forms.MessageBoxButtons.OK);
        }

        private string buildGameOverMessage()
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(string.Format("GG! The winner is {0}!", getTheNameOfTheWinner()));
            messageBuilder.Append(Environment.NewLine);

            messageBuilder.Append(string.Format("{0} Score: {1}", m_Player1Spaceship.Name, m_Player1Spaceship.Score));
            messageBuilder.Append(Environment.NewLine);

            messageBuilder.Append(string.Format("{0} Score: {1}", m_Player2Spaceship.Name, m_Player2Spaceship.Score));
            messageBuilder.Append(Environment.NewLine);

            return messageBuilder.ToString();
        }

        private string getTheNameOfTheWinner()
        {
            return m_Player1Spaceship.Score >= m_Player2Spaceship.Score ? m_Player1Spaceship.Name : m_Player2Spaceship.Name;
        }
    }
}
