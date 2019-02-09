using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.Managers;
using Infrastructure.ServiceInterfaces;

namespace Infrastructure.ObjectModel
{
    public class Game2D : Game
    {
        protected GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        protected IInputManager InputManager { get;  set; }

        protected ISoundManager SoundManager { get;  set; }

        protected ICollisionsManager CollisionsManager { get;  set; }

        public Game2D()
        {
            this.Content.RootDirectory = "Content";
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Services.AddService(typeof(GraphicsDeviceManager), GraphicsDeviceManager);
            CollisionsManager = new CollisionsManager(this);
            SoundManager = new SoundManager(this);
            InputManager = new InputManager(this);
        }

        private BackgroundSprite m_Background;

        public BackgroundSprite Background
        {
            set
            {
                if (Components.Contains(m_Background))
                {
                    Components.Remove(m_Background);
                }

                m_Background = value;
                Components.Add(m_Background);
            }
        }

        protected override void Initialize()
        {
            if (m_Background != null)
            {
                m_Background.Initialize();
                fitViewportToBackground();
            }

            base.Initialize();
        }

        private void fitViewportToBackground()
        {
            if (GraphicsDeviceManager != null)
            {
                GraphicsDeviceManager.PreferredBackBufferWidth = (int)m_Background.Width;
                GraphicsDeviceManager.PreferredBackBufferHeight = (int)m_Background.Height;
                GraphicsDeviceManager.ApplyChanges();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
