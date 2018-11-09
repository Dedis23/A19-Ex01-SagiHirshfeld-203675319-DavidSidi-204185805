using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Background : DrawableGameComponent
    {
        Texture2D m_Texure;
        Vector2 m_Position;
        Color m_Tint;
        SpriteBatch m_SpriteBatch;

        public Background(Game game) : base(game)
        {
            m_Position = Vector2.Zero;
            m_Tint = Color.White;   
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_Texure = Game.Content.Load<Texture2D>(@"Backgrounds\BG_Space01_1024x768");
            m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(m_Texure, m_Position, m_Tint);
            m_SpriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
