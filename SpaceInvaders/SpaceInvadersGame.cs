using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceInvadersGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private InputManager m_InputManager;
        private CollisionDetector m_CollisionDetector;
        private CollisionHandler m_CollisionHandler;
        private Spaceship m_Spaceship;
        private MotherShipSpawner m_MotherShipSpawner;
        private EnemiesMatrix m_EnemiesMatrix;

        public SpaceInvadersGame()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            loadBackground();
            loadMothershipSpawner();
            loadEnemies();
            loadSpaceship();

            setupInputBindings();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            IEnumerable<Drawable2DGameComponent> drawableGameComponents = from gameComponent in this.Components
                                                                          where gameComponent is Drawable2DGameComponent
                                                                          select gameComponent as Drawable2DGameComponent;

            foreach (Drawable2DGameComponent drawableGameComponent in drawableGameComponents)
            {
                spriteBatch.Draw(drawableGameComponent.Texture, drawableGameComponent.Position, drawableGameComponent.Tint);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void loadBackground()
        {
            Drawable2DGameComponent background = DrawableObjectsFactory.Create(this, DrawableObjectsFactory.eSpriteType.SpaceBG);
            Components.Add(background);

            // Adjust the ViewPort to match the size of the background
            graphics.PreferredBackBufferWidth = (int)background.Width;
            graphics.PreferredBackBufferHeight = (int)background.Height;
            graphics.ApplyChanges();
        }

        private void loadMothershipSpawner()
        {
            m_MotherShipSpawner = new MotherShipSpawner(this);
            m_MotherShipSpawner.MotherShipSpawned += OnMotherShipSpawned;
            m_MotherShipSpawner.MotherShipDeSpawned += OnMotherShipDeSpawned;
            Components.Add(m_MotherShipSpawner);
        }

        private void OnMotherShipSpawned(MotherShip i_SpawnedMotherShip)
        {
            Components.Add(i_SpawnedMotherShip);
            i_SpawnedMotherShip.setDefaultPosition();
        }
        private void OnMotherShipDeSpawned(MotherShip i_DeSpawnedMotherShip)
        {
            Components.Remove(i_DeSpawnedMotherShip);
        }

        private void loadEnemies()
        {
            m_EnemiesMatrix = new EnemiesMatrix(this);
            m_EnemiesMatrix.enemiesMatrixReachedBottomScreen += OnEnemiesMatrixReachedBottomScreen;
            Components.Add(m_EnemiesMatrix);
        }
        
        private void loadSpaceship()
        {
            m_Spaceship = DrawableObjectsFactory.Create(this, DrawableObjectsFactory.eSpriteType.SpaceShip) as Spaceship;
            m_Spaceship.Killed += onSpaceshipKilled;
            Components.Add(m_Spaceship);
        }

        private void onSpaceshipKilled(object obj)
        {
            gameOver();
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

        private void OnEnemiesMatrixReachedBottomScreen()
        {
            gameOver();
        }

        private void onEnemyCollidedWithSpaceship()
        {
            gameOver();
        }

        private void gameOver()
        {
            // TODO:
            System.Windows.Forms.MessageBox.Show("GG! Your score is " + m_Spaceship.Score.ToString(), "Game Over!", System.Windows.Forms.MessageBoxButtons.OK);
            Exit();
        }

        // Overriden to enable keybind registration
        public void Exit(GameTime i_GameTime)
        {
            Exit();
        }
    }
}