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
        private Spaceship m_Spaceship;
        private MothershipSpawner m_MothershipSpawner;
        private InvadersMatrix m_InvadersMatrix;
        private Sprite m_Background;

        private IInputManager m_InputManager;
        private ICollisionsManager m_CollisionsManager;
        private CollisionHandler m_CollisionHandler;
        private AnimationManager m_AnimationManager;

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

            m_Spaceship = new Spaceship(this);
            m_Spaceship.SpriteKilled += onSpaceshipKilled;

            m_MothershipSpawner = new MothershipSpawner(this);

            m_InvadersMatrix = new InvadersMatrix(this);
            m_InvadersMatrix.invadersMatrixReachedBottomScreen += onInvadersMatrixReachedBottomScreen;
            m_InvadersMatrix.allInvadersWereDefeated += onAllInvadersWereDefeated;
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
            setSpaceshipDefaultPosition(m_Spaceship);
           
            base.LoadContent();
        }

        private void setSpaceshipDefaultPosition(Spaceship m_Spaceship)
        {
            m_Spaceship.SetDefaultPosition();
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
            m_SpriteBatch.End();
        }

        private void onSpaceshipKilled(object i_Object)
        {
            gameOver();
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
            System.Windows.Forms.MessageBox.Show("GG! Your score is " + m_Spaceship.Score.ToString(), "Game Over!", System.Windows.Forms.MessageBoxButtons.OK);
            Exit();
        }
    }
}