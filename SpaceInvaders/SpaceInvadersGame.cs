using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Managers;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel;
using System;

namespace SpaceInvaders
{
    public class SpaceInvadersGame : Game
    {
        private readonly GraphicsDeviceManager r_Graphics;
        private SpriteBatch m_SpriteBatch;        
        private List<Spaceship> m_SpaceshipList;
        private MothershipSpawner m_MothershipSpawner;
        private InvadersMatrix m_InvadersMatrix;
        private Sprite m_Background;

        private IInputManager m_InputManager;
        private ICollisionsManager m_CollisionsManager;
        private CollisionHandler m_CollisionHandler;
        private AnimationManager m_AnimationManager;

        // Test
        //SpriteFont m_Arial;

        public SpaceInvadersGame()
        {
            r_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = "Space Invaders";
            this.IsMouseVisible = true;

            m_InputManager = new InputManager(this);

            m_CollisionsManager = new CollisionsManager(this);

            m_CollisionHandler = new CollisionHandler(this);
            m_CollisionHandler.EnemyCollidedWithSpaceship += onEnemyCollidedWithSpaceship;

            m_AnimationManager = new AnimationManager(this);

            m_Background = new SpaceBG(this);

            loadSpaceships();

            m_MothershipSpawner = new MothershipSpawner(this);

            loadInvadersMatrix();
        }

        protected override void Initialize()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(m_SpriteBatch.GetType(), m_SpriteBatch);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            fitViewportToBackground();
            setSpaceshipsAtDefaultPosition();

            // Test
            //m_Arial = Content.Load<SpriteFont>(@"Fonts\ComicSansMS");
           
            base.LoadContent();
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

        private void setSpaceshipsAtDefaultPosition()
        {
            foreach (Spaceship spaceship in m_SpaceshipList)
            {
                spaceship.SetDefaultPosition();
            }
        }

        private void loadInvadersMatrix()
        {
            m_InvadersMatrix = new InvadersMatrix(this);
            m_InvadersMatrix.invadersMatrixReachedBottomScreen += onInvadersMatrixReachedBottomScreen;
            m_InvadersMatrix.allInvadersWereDefeated += onAllInvadersWereDefeated;
        }

        private void fitViewportToBackground()
        {
            r_Graphics.PreferredBackBufferWidth = m_Background.Width;
            r_Graphics.PreferredBackBufferHeight = m_Background.Height;
            r_Graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            if( m_InputManager.KeyPressed(Keys.Escape))
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            m_SpriteBatch.Begin();
            base.Draw(gameTime);
            //m_SpriteBatch.DrawString(m_Arial, "TEXT ON SCREEN", Vector2.Zero, Color.Black);
            m_SpriteBatch.End();
        }

        private void onSpaceshipKilled(object i_Object)
        {
            (i_Object as Spaceship).Enabled = false;

            bool allSpaceshipsDisabled = true;
            foreach (Spaceship spaceship in m_SpaceshipList)
            {
                if (spaceship.Enabled)
                {
                    allSpaceshipsDisabled = false;
                    break;
                }
            }

            if (allSpaceshipsDisabled)
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

        private void gameOver()
        {
            System.Windows.Forms.MessageBox.Show("GG! Your score is " + m_SpaceshipList[0].Score.ToString(), "Game Over!", System.Windows.Forms.MessageBoxButtons.OK);
            Exit();
        }
    }
}