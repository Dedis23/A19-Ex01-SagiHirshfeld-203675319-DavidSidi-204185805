using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.ObjectModel
{
    public class Game2D : Game
    {
        GraphicsDeviceManager r_GraphicsDeviceManager;

        public Game2D()
        {
            this.Content.RootDirectory = "Content";
            r_GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Services.AddService(typeof(GraphicsDeviceManager), r_GraphicsDeviceManager);
            new CollisionsManager(this);
            new InputManager(this);
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
            if (r_GraphicsDeviceManager != null)
            {
                r_GraphicsDeviceManager.PreferredBackBufferWidth = (int)m_Background.Width;
                r_GraphicsDeviceManager.PreferredBackBufferHeight = (int)m_Background.Height;
                r_GraphicsDeviceManager.ApplyChanges();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            base.Draw(gameTime);
        }
    }
}
