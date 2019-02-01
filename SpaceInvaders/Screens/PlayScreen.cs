using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using System.Text;

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

        private BackgroundSprite m_Background;
        private Mothership m_Mothership;
        private List<Spaceship> m_SpaceshipList;
        private List<SpriteRow> m_RowsOfLives;
        private BulletsFactory m_BulletsFactory;
        private InvadersMatrix m_InvadersMatrix;
        private DancingBarriersRow m_DancingBarriersRow;
        private List<TextSprite> m_ScoreSprites;

        private bool m_GameOver = false;

        public PlayScreen(Game i_Game) : base(i_Game)
        {            
            this.BlendState = BlendState.NonPremultiplied;
            m_CollisionHandler = new CollisionHandler(i_Game);
            m_CollisionHandler.EnemyCollidedWithSpaceship += () => m_GameOver = true;

            m_BulletsFactory = new BulletsFactory(this);

            m_Background = new SpaceBG(i_Game);
            this.Add(m_Background);

            m_Mothership = new Mothership(i_Game);
            this.Add(m_Mothership);

            loadSpaceships();
            loadLives();
            loadScoreSprites();
            loadInvadersMatrix();
            m_DancingBarriersRow = new DancingBarriersRow(i_Game);
            this.Add(m_DancingBarriersRow);
        }

        public override void Initialize()
        {
            base.Initialize();
            fitViewportToBackground();
            setMothershipPosition();
            setSpaceshipsPositionsAndInputManager();            
            setLivesPositions();
            setScoreSpritesPositions();
            setBarriersPosition();
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

        private void loadSpaceships()
        {
            Spaceship newSpaceship;
            m_SpaceshipList = new List<Spaceship>();

            newSpaceship = new Player1Spaceship(Game);
            newSpaceship.Died += onSpaceshipKilled;
            m_SpaceshipList.Add(newSpaceship);
            this.Add(newSpaceship);

            newSpaceship = new Player2Spaceship(Game);
            newSpaceship.Died += onSpaceshipKilled;
            m_SpaceshipList.Add(newSpaceship);
            this.Add(newSpaceship);
        }

        private void setSpaceshipsPositionsAndInputManager()
        {
            foreach (Spaceship spaceship in m_SpaceshipList)
            {
                spaceship.Position = spaceship.DefaultPosition =
                    new Vector2(0, GraphicsDevice.Viewport.Height - (spaceship.Height * k_SpaceshipPositionYModifier));
                spaceship.InputManager = this.InputManager;
            }
        }

        private void loadLives()
        {
            m_RowsOfLives = new List<SpriteRow>();
            foreach (Spaceship spaceship in m_SpaceshipList)
            {
                SpriteRow spriteRow = new SpriteRow(Game, spaceship.Lives, Game => new LifeIcon(spaceship.AssetName, Game));
                spriteRow.InsertionOrder = SpriteRow.Order.RightToLeft;
                spriteRow.RemovalOrder = SpriteRow.Order.LeftToRight;
                spriteRow.BlendState = BlendState.NonPremultiplied;
                spaceship.LifeLost += () => spriteRow.RemoveSprite();
                m_RowsOfLives.Add(spriteRow);
                this.Add(spriteRow);
            }
        }

        private void setLivesPositions()
        {
            m_RowsOfLives[0].Gap = m_RowsOfLives[0].First.Width * k_GapBetweenRowsModifier;
            m_RowsOfLives[0].Position = new Vector2(
                GraphicsDevice.Viewport.Width - m_RowsOfLives[0].First.Width - k_LivesDistanceFromHorizontalScreenBound, 0);

            for (int i = 1; i < m_RowsOfLives.Count; i++)
            {
                m_RowsOfLives[i].Gap = m_RowsOfLives[i - 1].Gap;
                m_RowsOfLives[i].Position = m_RowsOfLives[i - 1].Position + new Vector2(0, m_RowsOfLives[0].First.Height * k_GapBetweenRowsModifier);
            }
        }

        private void loadScoreSprites()
        {
            m_ScoreSprites = new List<TextSprite>();
            foreach (Spaceship spaceship in m_SpaceshipList)
            {
                TextSprite newScoreSprite = new TextSprite(k_ScoreFontAsset, Game);
                newScoreSprite.TintColor = spaceship.ScoreColor;
                newScoreSprite.Text = string.Format("{0} Score: {1}", spaceship.Name, spaceship.Score);
                spaceship.ScoreChanged +=
                    () => newScoreSprite.Text = string.Format("{0} Score: {1}", spaceship.Name, spaceship.Score);

                m_ScoreSprites.Add(newScoreSprite);
                this.Add(newScoreSprite);
            }
        }

        private void setScoreSpritesPositions()
        {
            for (int i = 0; i < m_ScoreSprites.Count; i++)
            {
                float height = m_ScoreSprites[i].Height;
                m_ScoreSprites[i].Position = new Vector2(0, i * m_ScoreSprites[i].Height);
            }
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
                m_SpaceshipList[0].DefaultPosition.Y - (m_DancingBarriersRow.Height * 2));
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
            Spaceship spaceship = i_Spaceship as Spaceship;
            this.Remove(spaceship);
            m_SpaceshipList.Remove(spaceship);
            if (m_SpaceshipList.Count == 0)
            {
                m_GameOver = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                ExitScreen();
            }

            if (m_GameOver)
            {
                showGameOverWindow();
                ExitScreen();
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

            foreach (IPlayer player in m_SpaceshipList)
            {
                messageBuilder.Append(string.Format("{0} Score: {1}", player.Name, player.Score));
                messageBuilder.Append(Environment.NewLine);
            }

            return messageBuilder.ToString();
        }

        private string getTheNameOfTheWinner()
        {
            int maxScore = 0;
            string winnerName = string.Empty;

            foreach (IPlayer player in m_SpaceshipList)
            {
                if (player.Score >= maxScore)
                {
                    maxScore = player.Score;
                    winnerName = player.Name;
                }
            }

            return winnerName;
        }
    }
}
