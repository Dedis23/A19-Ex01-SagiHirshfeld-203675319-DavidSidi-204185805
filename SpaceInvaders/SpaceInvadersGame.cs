using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public class SpaceInvadersGame : Game2D
    {
        private const string k_ScoreFontAsset = @"Fonts\ComicSansMS";
        private const float k_SpaceshipPositionYModifier = 1.5f;
        private const float k_LivesGapModifier = 0.7f;
        private const float k_GapBetweenRowsModifier = 1.2f;
        private const int k_LivesDistanceFromHorizontalScreenBound = 15;

        private CollisionHandler m_CollisionHandler;
        private Sprite m_Background;
        private MothershipSpawner m_MothershipSpawner;
        private List<Spaceship> m_SpaceshipList;
        private List<SpriteRow> m_RowsOfLives;
        private InvadersMatrix m_InvadersMatrix;
        private DancingBarriersRow m_DancingBarriersRow;

        private List<TextSprite> m_ScoreSprites;

        public SpaceInvadersGame()
        {
            Content.RootDirectory = "Content";
            this.Window.Title = "Space Invaders";
            this.IsMouseVisible = true;

            m_CollisionHandler = new CollisionHandler(this);
            m_CollisionHandler.EnemyCollidedWithSpaceship += gameOver;

            m_Background = new SpaceBG(this);
            m_MothershipSpawner = new MothershipSpawner(this);
            loadSpaceships();
            loadLives();
            loadScoreSprites();
            loadInvadersMatrix();
            m_DancingBarriersRow = new DancingBarriersRow(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            fitViewportToBackground();
            setSpaceshipsPositions();
            setLivesPositions();
            setScoreSpritesPositions();
            setBarriersPosition();
        }

        private void loadSpaceships()
        {
            Spaceship newSpaceship;
            m_SpaceshipList = new List<Spaceship>();

            newSpaceship = new Player1Spaceship(this);
            newSpaceship.SpriteKilled += onSpaceshipKilled;
            m_SpaceshipList.Add(newSpaceship);

            newSpaceship = new Player2Spaceship(this);
            newSpaceship.SpriteKilled += onSpaceshipKilled;
            m_SpaceshipList.Add(newSpaceship);
        }

        private void setSpaceshipsPositions()
        {
            foreach (Spaceship spaceship in m_SpaceshipList)
            {
                spaceship.DefaultPosition = new Vector2(0, GraphicsDevice.Viewport.Height - (spaceship.Height * k_SpaceshipPositionYModifier));
                spaceship.Position = spaceship.DefaultPosition;
            }
        }

        private void loadLives()
        {
            m_RowsOfLives = new List<SpriteRow>();
            foreach (Spaceship spaceship in m_SpaceshipList)
            {
                SpriteRow spriteRow = new SpriteRow(this, spaceship.Lives, Game => new LifeIconSprite(spaceship.AssetName, this));
                spriteRow.InsertionOrder = SpriteRow.Order.RightToLeft;
                spriteRow.Opacity /= 2;
                spriteRow.Scales /= 2;
                spaceship.LifeLost += () => spriteRow.RemoveSprite();
                m_RowsOfLives.Add(spriteRow);
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
                TextSprite newScoreSprite = new TextSprite(k_ScoreFontAsset, this);
                newScoreSprite.TintColor = spaceship.ScoreColor;
                newScoreSprite.Text = string.Format("{0} Score: {1}", spaceship.Name, spaceship.Score);
                spaceship.ScoreChanged +=
                    () => newScoreSprite.Text = string.Format("{0} Score: {1}", spaceship.Name, spaceship.Score);

                m_ScoreSprites.Add(newScoreSprite);
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

        private void setBarriersPosition()
        {
            float distanceFromScreenHorizondalBounds = (GraphicsDevice.Viewport.Width - m_DancingBarriersRow.Width) / 2;
            m_DancingBarriersRow.Position = new Vector2(
                distanceFromScreenHorizondalBounds,
                m_SpaceshipList[0].DefaultPosition.Y - (m_DancingBarriersRow.Height * 2));
        }

        private void loadInvadersMatrix()
        {
            m_InvadersMatrix = new InvadersMatrix(this);
            m_InvadersMatrix.invadersMatrixReachedBottomScreen += gameOver;
            m_InvadersMatrix.allInvadersWereDefeated += gameOver;
        }

        private void fitViewportToBackground()
        {
            r_Graphics.PreferredBackBufferWidth = (int)m_Background.Width;
            r_Graphics.PreferredBackBufferHeight = (int)m_Background.Height;
            r_Graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (m_InputManager.KeyPressed(Keys.Escape))
            {
                Exit();
            }
        }

        private int m_SpaceshipsDestroyed = 0;

        private void onSpaceshipKilled(object i_Object)
        {
            m_SpaceshipsDestroyed++;
            if(m_SpaceshipsDestroyed == m_SpaceshipList.Count)
            {
                gameOver();
            }
        }

        private bool m_FirstGameOver = true;

        private void gameOver()
        {
            if (m_FirstGameOver)
            {
                m_FirstGameOver = false;
                showGameOverWindow();
                Exit();
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