using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Managers;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel;
using System;
using System.Text;

namespace SpaceInvaders
{
    public class Game2D : Game
    {
        protected readonly GraphicsDeviceManager r_Graphics;
        protected SpriteBatch m_SpriteBatch;
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
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(m_SpriteBatch.GetType(), m_SpriteBatch);

            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            base.Draw(gameTime);
            DrawInjectionPoint();
            m_SpriteBatch.End();
        }

        protected virtual void DrawInjectionPoint()
        {
        }
    }
}

