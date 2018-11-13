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
        Spaceship m_Spaceship;
        MotherShipSpawner m_MotherShipSpawner;
        EnemiesMatrix m_EnemiesMatrix;

        public SpaceInvadersGame()
        {
            graphics = new GraphicsDeviceManager(this);

            // need to remove this from here and make this not hard-coded
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            m_InputManager = new InputManager(this);
            Components.Add(m_InputManager);

            m_MotherShipSpawner = new MotherShipSpawner(this);
            m_MotherShipSpawner.MotherShipSpawned += OnMotherShipSpawned;
            m_MotherShipSpawner.MotherShipDeSpawned += OnMotherShipDeSpawned;
            Components.Add(m_MotherShipSpawner);

            Components.Add(DrawableObjectsFactory.Create(this, DrawableObjectsFactory.eSpriteType.SpaceBG));

            m_EnemiesMatrix = new EnemiesMatrix(this);
            m_EnemiesMatrix.enemiesMatrixReachedBottomScreen += OnEnemiesMatrixReachedBottomScreen;
            Components.Add(m_EnemiesMatrix);

            m_Spaceship = DrawableObjectsFactory.Create(this, DrawableObjectsFactory.eSpriteType.SpaceShip) as Spaceship;
            Components.Add(m_Spaceship);
        }

        protected override void Initialize()
        {
            base.Initialize();
            setupInputBindings();
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
        }

        // Overriden to enable keybind registration
        public void Exit(GameTime i_GameTime)
        {
            Exit();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.Window.Title = m_EnemiesMatrix.Debug.ToString();
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
        
        private void OnEnemiesMatrixReachedBottomScreen()
        {
            gameOver();
        }

        private void gameOver()
        {
            // TO DO
            System.Windows.Forms.MessageBox.Show("It's GG", "Game Over!", System.Windows.Forms.MessageBoxButtons.OK);
            Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}