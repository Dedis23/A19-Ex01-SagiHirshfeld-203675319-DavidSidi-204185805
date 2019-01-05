using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Managers;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel;
using System;
using System.Text;

namespace SpaceInvaders
{
    public class SpaceInvadersGame : Game2D
    {
        private List<Spaceship> m_SpaceshipList;
        private MothershipSpawner m_MothershipSpawner;
        private InvadersMatrix m_InvadersMatrix;
        private Sprite m_Background;
        private CollisionHandler m_CollisionHandler;
        private DancingBarriersRow m_DancingBarriersRow;

        public SpaceInvadersGame()
        {
            Content.RootDirectory = "Content";
            this.Window.Title = "Space Invaders";
            this.IsMouseVisible = true;

            m_CollisionHandler = new CollisionHandler(this);
            m_CollisionHandler.EnemyCollidedWithSpaceship += onEnemyCollidedWithSpaceship;

            m_Background = new SpaceBG(this);
            m_MothershipSpawner = new MothershipSpawner(this);
            loadSpaceships();
            loadInvadersMatrix();
            m_DancingBarriersRow = new DancingBarriersRow(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            fitViewportToBackground();
            setSpaceshipsPositions();
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
                spaceship.DefaultPosition = new Vector2(0, GraphicsDevice.Viewport.Height - spaceship.Height * 1.5f);
                spaceship.Position = spaceship.DefaultPosition;
            }
        }

        private void setBarriersPosition()
        {
            float distanceFromScreenHorizondalBounds = (GraphicsDevice.Viewport.Width - m_DancingBarriersRow.Width) / 2;
            m_DancingBarriersRow.Position = new Vector2(
                distanceFromScreenHorizondalBounds,
                m_SpaceshipList[0].DefaultPosition.Y - m_DancingBarriersRow.Height * 2);
        }

        private void loadInvadersMatrix()
        {
            m_InvadersMatrix = new InvadersMatrix(this);
            m_InvadersMatrix.invadersMatrixReachedBottomScreen += onInvadersMatrixReachedBottomScreen;
            m_InvadersMatrix.allInvadersWereDefeated += onAllInvadersWereDefeated;
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

        private void onInvadersMatrixReachedBottomScreen()
        {
            gameOver();
        }

        private void onAllInvadersWereDefeated()
        {
            gameOver();
        }

        private void onEnemyCollidedWithSpaceship()
        {
            gameOver();
        }

        bool m_FirstGameOver = true;
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
            String gameOverMessage = buildGameOverMessage();
            System.Windows.Forms.MessageBox.Show(gameOverMessage, "Game Over!", System.Windows.Forms.MessageBoxButtons.OK);
        }

        private string buildGameOverMessage()
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(String.Format("GG! The winner is {0}!", getTheNameOfTheWinner()));
            messageBuilder.Append(Environment.NewLine);

            foreach (IPlayer player in m_SpaceshipList)
            {
                messageBuilder.Append(String.Format("{0} Score: {1}", player.Name, player.Score));
                messageBuilder.Append(Environment.NewLine);
            }

            return messageBuilder.ToString();
        }

        private String getTheNameOfTheWinner()
        {
            int maxScore = 0;
            String winnerName = "";

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