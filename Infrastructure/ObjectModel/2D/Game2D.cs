using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class Game2D : Game
    {
        GraphicsDeviceManager r_GraphicsDeviceManager;

        public Game2D()
        {
            this.Content.RootDirectory = "Content";
            r_GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Services.AddService(typeof(GraphicsDeviceManager), r_GraphicsDeviceManager);
            new InputManager(this);
            new CollisionsManager(this);
        }

        private bool m_FitViewportToBackgroundIsNeeded = false;

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
                m_FitViewportToBackgroundIsNeeded = true;
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

        protected override void Update(GameTime gameTime)
        {
            if (m_FitViewportToBackgroundIsNeeded)
            {
                fitViewportToBackground();
                m_FitViewportToBackgroundIsNeeded = false;
            }

            base.Update(gameTime);
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
