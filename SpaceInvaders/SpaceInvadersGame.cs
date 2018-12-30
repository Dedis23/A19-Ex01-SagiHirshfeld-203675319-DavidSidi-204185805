using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;

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

            loadBackground();
            loadSpaceship();
            loadMothershipSpawner();
            //loadInvaders();  

            setupInputBindings();
            base.Initialize();
        }

        protected override void LoadContent()
        {           
            // Sagi: Maybe something inits have to move here to make shit work???
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
        }

        private void loadBackground()
        {
            m_Background = DrawableObjectsFactory.Create(this, DrawableObjectsFactory.eSpriteType.SpaceBG);
            // Create a new SpriteBatch, which can be used to draw textures.
            // Adjust the ViewPort to match the size of the background
            //r_Graphics.PreferredBackBufferWidth = m_Background.Width;
            //r_Graphics.PreferredBackBufferHeight = m_Background.Height;
            //r_Graphics.ApplyChanges();
        }

        private void loadMothershipSpawner()
        {
            m_MothershipSpawner = new MothershipSpawner(this);
        }

        private void loadInvaders()
        {
            m_InvadersMatrix = new InvadersMatrix(this);
            m_InvadersMatrix.invadersMatrixReachedBottomScreen += onInvadersMatrixReachedBottomScreen;
            m_InvadersMatrix.allInvadersWereDefeated += onAllInvadersWereDefeated;
            Components.Add(m_InvadersMatrix);
        }
        
        private void loadSpaceship()
        {
            m_Spaceship = DrawableObjectsFactory.Create(this, DrawableObjectsFactory.eSpriteType.Spaceship) as Spaceship;
            m_Spaceship.Killed += onSpaceshipKilled;
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