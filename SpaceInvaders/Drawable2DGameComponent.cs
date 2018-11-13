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
    public abstract class Drawable2DGameComponent : DrawableGameComponent
    {
        readonly private String r_SourceFileURL;
        public Vector2 Position;
        public Color Tint { get; set; } = Color.White;
        public Texture2D Texture { get; set; }
        public int Velocity { get; set; }

        private SpriteBatch m_SpriteBatch;

        public Drawable2DGameComponent(Game i_Game, String i_SourceFileURL) : base(i_Game)
        {
            r_SourceFileURL = i_SourceFileURL;
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(Texture, Position, Tint);
            m_SpriteBatch.End();
            base.Draw(gameTime);
        }

        // This function is called when Game.LoadContent() is invoked - but before it is
        // accessed. This means that any Texture/Position/Color assignments within 
        // Game.LoadContent() will "overpower" the following assignments            
        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Texture = Game.Content.Load<Texture2D>(r_SourceFileURL);

            base.LoadContent();
        }
    }
}
