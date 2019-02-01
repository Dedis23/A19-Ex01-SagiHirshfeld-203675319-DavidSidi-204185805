using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class Game2D : Game
    {
        protected IInputManager m_InputManager;
        protected ICollisionsManager m_CollisionManager;

        public Game2D()
        {
            this.Content.RootDirectory = "Content";            
            Services.AddService(typeof(GraphicsDeviceManager), new GraphicsDeviceManager(this));
            new InputManager(this);
            new CollisionsManager(this);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
