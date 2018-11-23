using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public SpaceInvadersGame()
        {
            r_Graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            this.Window.Title = "Space Invaders";
            this.IsMouseVisible = true;

            m_InputManager = new InputManager(this);
            Components.Add(m_InputManager);

            m_CollisionHandler = new CollisionHandler(this);
            m_CollisionHandler.EnemyCollidedWithSpaceship += OnEnemyCollidedWithSpaceship;
            Components.Add(m_CollisionHandler);

            m_CollisionDetector = new CollisionDetector(this);
            m_CollisionDetector.CollisionDetected += m_CollisionHandler.HandleCollision;
            Components.Add(m_CollisionDetector);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            loadBackground();
            loadMothershipSpawner();
            loadInvaders();
            loadSpaceship();
            setupInputBindings();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            m_SpriteBatch.Begin();
            IEnumerable<Drawable2DGameComponent> drawableGameComponents = from gameComponent in this.Components
                                                                          where gameComponent is Drawable2DGameComponent
                                                                          select gameComponent as Drawable2DGameComponent;

            foreach (Drawable2DGameComponent drawableGameComponent in drawableGameComponents)
            {
                m_SpriteBatch.Draw(drawableGameComponent.Texture, drawableGameComponent.Position, drawableGameComponent.Color);
            }

            m_SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void loadBackground()
        {
            Drawable2DGameComponent background = DrawableObjectsFactory.Create(this, DrawableObjectsFactory.eSpriteType.SpaceBG);
            Components.Add(background);

            // Adjust the ViewPort to match the size of the background
            r_Graphics.PreferredBackBufferWidth = (int)background.Width;
            r_Graphics.PreferredBackBufferHeight = (int)background.Height;
            r_Graphics.ApplyChanges();
        }

        private void loadMothershipSpawner()
        {
            m_MothershipSpawner = new MothershipSpawner(this);
            Components.Add(m_MothershipSpawner);
        }

        private void loadInvaders()
        {
            m_InvadersMatrix = new InvadersMatrix(this);
            m_InvadersMatrix.invadersMatrixReachedBottomScreen += OnInvadersMatrixReachedBottomScreen;
            m_InvadersMatrix.allInvadersWereDefeated += OnAllInvadersWereDefeated;
            Components.Add(m_InvadersMatrix);
        }
        
        private void loadSpaceship()
        {
            m_Spaceship = DrawableObjectsFactory.Create(this, DrawableObjectsFactory.eSpriteType.Spaceship) as Spaceship;
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

        private void OnInvadersMatrixReachedBottomScreen()
        {
            gameOver();
        }

        private void OnAllInvadersWereDefeated()
        {
            gameOver();
        }

        private void OnEnemyCollidedWithSpaceship()
        {
            gameOver();
        }

        private void gameOver()
        {
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