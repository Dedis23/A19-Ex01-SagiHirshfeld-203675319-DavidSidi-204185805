using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class Game2D : Game
    {
        protected readonly GraphicsDeviceManager r_Graphics;
        protected SpriteBatch m_PremultipliedSpriteBatch;
        protected NonPremultipliedSpriteBatch m_NonPremultipliedSpriteBatch;
        protected IInputManager m_InputManager;
        protected ICollisionsManager m_CollisionsManager;

        public Game2D()
        {
            r_Graphics = new GraphicsDeviceManager(this);
            m_InputManager = new InputManager(this);
            m_CollisionsManager = new CollisionsManager(this);
        }

        protected override void Initialize()
        {
            m_PremultipliedSpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(m_PremultipliedSpriteBatch.GetType(), m_PremultipliedSpriteBatch);

            m_NonPremultipliedSpriteBatch = new NonPremultipliedSpriteBatch(GraphicsDevice);
            this.Services.AddService(m_NonPremultipliedSpriteBatch.GetType(), m_NonPremultipliedSpriteBatch);

            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_NonPremultipliedSpriteBatch.Begin();
            m_PremultipliedSpriteBatch.Begin();
            base.Draw(gameTime);
            m_PremultipliedSpriteBatch.End();
            m_NonPremultipliedSpriteBatch.End();
        }
    }
}
