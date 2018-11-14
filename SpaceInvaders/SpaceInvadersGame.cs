using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceInvadersGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputManager m_InputManager;
        CollisionDetector m_CollisionDetector;
        Spaceship m_Spaceship;
        MotherShipSpawner m_MotherShipSpawner;
        EnemiesMatrix m_EnemiesMatrix;

        public SpaceInvadersGame()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            m_InputManager = new InputManager(this);
            Components.Add(m_InputManager);

            m_CollisionDetector = new CollisionDetector(this);
            m_CollisionDetector.CollisionDetected += onCollision;
            Components.Add(m_CollisionDetector);

            base.Initialize();
        }

        private void onCollision(ICollideable i_CollideableA, ICollideable i_CollideableB)
        {
            /// FOR TESTING ///            
            if (i_CollideableA is Spaceship && i_CollideableB is Enemy)                
            {
                flipSpaceshipColor(i_CollideableA as Spaceship);
            }

            if (i_CollideableB is Spaceship && i_CollideableA is Enemy)
            {
                flipSpaceshipColor(i_CollideableB as Spaceship);
            }
            /// FOR TESTING ///

            // TODO: pass to CollisionHandler?
        }

        /// FOR TESTING ///
        private void flipSpaceshipColor(Spaceship i_Spaceship)
        {
            i_Spaceship.Tint = i_Spaceship.Tint == Color.Black ? Color.White : Color.Black;
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            loadBackground();
            loadMothership();
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

        private void loadMothership()
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
            Components.Add(m_Spaceship);
        }

        private void setupInputBindings()
        {
            // Keyboard
            m_InputManager.RegisterKeyboardKeyBinding(m_Spaceship.MoveLeft, Keys.Left);
            m_InputManager.RegisterKeyboardKeyBinding(m_Spaceship.MoveRight, Keys.Right);
            m_InputManager.RegisterKeyboardKeyBinding(Exit, Keys.Escape);

            // Mouse
            m_InputManager.MouseMoved += m_Spaceship.MoveAccordingToMousePositionDelta;
            m_InputManager.MouseLeftButtonPressed += m_Spaceship.FireBullet;

            /// FOR TESTING ///
            m_InputManager.RegisterKeyboardKeyBinding(m_Spaceship.MoveUp, Keys.Up);
            m_InputManager.RegisterKeyboardKeyBinding(m_Spaceship.MoveDown, Keys.Down);
            /// FOR TESTING ///
        }

        private void OnEnemiesMatrixReachedBottomScreen()
        {
            gameOver();
        }

        private void gameOver()
        {
            // TODO:
            System.Windows.Forms.MessageBox.Show("It's GG", "Game Over!", System.Windows.Forms.MessageBoxButtons.OK);
            Exit();
        }

        // Overriden to enable keybind registration
        public void Exit(GameTime i_GameTime)
        {
            Exit();
        }
    }
}