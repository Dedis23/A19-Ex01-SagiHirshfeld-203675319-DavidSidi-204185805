using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using System;

namespace SpaceInvaders
{
    public class SpaceInvadersGame : Game
    {
        private readonly GraphicsDeviceManager r_Graphics;
        private SpriteBatch m_SpriteBatch;
        private InputManager m_InputManager;
        private CollisionDetector m_CollisionDetector;
        private CollisionHandler m_CollisionHandler;
        private Spaceship m_Spaceship;
        private MothershipSpawner m_MothershipSpawner;
        private InvadersMatrix m_InvadersMatrix;
        private Sprite m_Background;

        public SpaceInvadersGame()
        {
            r_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = "Space Invaders";
            this.IsMouseVisible = true;

            m_InputManager = new InputManager(this);
            Components.Add(m_InputManager);

            m_CollisionHandler = new CollisionHandler(this);
            m_CollisionHandler.EnemyCollidedWithSpaceship += onEnemyCollidedWithSpaceship;
            Components.Add(m_CollisionHandler);

            m_CollisionDetector = new CollisionDetector(this);
            m_CollisionDetector.CollisionDetected += m_CollisionHandler.HandleCollision;
            Components.Add(m_CollisionDetector);

            m_Background = new SpaceBG(this);

            m_Spaceship = new Spaceship(this);
            m_Spaceship.Killed += onSpaceshipKilled;

            m_MothershipSpawner = new MothershipSpawner(this);

            m_InvadersMatrix = new InvadersMatrix(this);
            m_InvadersMatrix.invadersMatrixReachedBottomScreen += onInvadersMatrixReachedBottomScreen;
            m_InvadersMatrix.allInvadersWereDefeated += onAllInvadersWereDefeated;

            setupInputBindings();
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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            m_SpriteBatch.Begin();
            base.Draw(gameTime);
            m_SpriteBatch.End();
        }

        private void setupInputBindings()
        {
            // Keyboard
            m_InputManager.RegisterKeyboardKeyDownBinding(m_Spaceship.MoveLeft, Keys.Left);
            m_InputManager.RegisterKeyboardKeyDownBinding(m_Spaceship.MoveRight, Keys.Right);
            m_InputManager.RegisterKeyboardSinglePressBinding(m_Spaceship.Shoot, Keys.Enter);
            m_InputManager.RegisterKeyboardKeyDownBinding(Exit, Keys.Escape);

            // Mouse
            m_InputManager.MouseMoved += m_Spaceship.MoveAccordingToMousePositionDelta;
            m_InputManager.MouseLeftButtonPressedOnce += m_Spaceship.Shoot;
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