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
        MotherShipSpawner m_MotherShipSpawner;
        Spaceship m_Spaceship;

        public SpaceInvadersGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Components.Add(new Background(this));

            m_InputManager = new InputManager(this);
            Components.Add(m_InputManager);

            m_MotherShipSpawner = new MotherShipSpawner(this);
            m_MotherShipSpawner.MotherShipSpawned += OnMotherShipSpawned;
            m_MotherShipSpawner.MotherShipDeSpawned += OnMotherShipDeSpawned;
            Components.Add(m_MotherShipSpawner);

            m_Spaceship = new Spaceship(this);
            Components.Add(m_Spaceship);

            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            setupInputBindings();            

            base.Initialize();
        }

        private void setupInputBindings()
        {
            m_InputManager.RegisterKeyboardKeyBinding(m_Spaceship.MoveLeft, Keys.Left);
            m_InputManager.RegisterKeyboardKeyBinding(m_Spaceship.MoveRight, Keys.Right);
            m_InputManager.RegisterKeyboardKeyBinding(Exit, Keys.Escape);

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
            // TODO: use this.Content to load your game content here
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
        }

        public void OnMotherShipSpawned(object i_Source, MotherShipEventArgs i_MotherShipArgs)
        {
            Components.Add(i_MotherShipArgs.MotherShip);
        }
        public void OnMotherShipDeSpawned(object i_Source, MotherShipEventArgs i_MotherShipArgs)
        {
            Components.Remove(i_MotherShipArgs.MotherShip);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
