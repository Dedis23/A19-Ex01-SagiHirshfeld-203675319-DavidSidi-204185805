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
        public Vector2 m_Position;
        public Color Tint { get; set; } = Color.White;
        public Texture2D Texture { get; set; }
        public int Velocity { get; set; }
        private SpriteBatch m_SpriteBatch;

        public Vector2 Position
        {
            get
            {
                return m_Position;
            }

            set
            {
                m_Position = value;
            }
        }

        public int Width
        {
            get
            {
                return Texture != null ? Texture.Width : 0;
            }
        }

        public int Height
        {
            get
            {
                return Texture != null ? Texture.Height : 0;
            }
        }

        public int Top
        {
            get
            {
                return (int)Math.Floor(m_Position.Y);
            }
        }

        public int Bottom
        {
            get
            {
                return (int)Math.Floor(m_Position.Y) + this.Height;
            }
        }

        public int Left
        {
            get
            {
                return (int)Math.Floor(m_Position.X);
            }
        }

        public int Right
        {
            get
            {
                return (int)Math.Floor(m_Position.X) + this.Width;
            }
        }

        public bool IsInScreen
        {
            get
            {
                int screenTop, screenBottom, screenLeft, screenRight;
                screenTop = 0;
                screenBottom = this.GraphicsDevice.Viewport.Height;
                screenLeft = 0;
                screenRight = this.GraphicsDevice.Viewport.Width;

                return !(this.Right < screenLeft || screenRight < screenLeft || this.Bottom < screenTop || screenBottom < this.Top);
            }
        }

        public Drawable2DGameComponent(Game i_Game, String i_SourceFileURL) : base(i_Game)
        {
            r_SourceFileURL = i_SourceFileURL;
            m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Texture = Game.Content.Load<Texture2D>(r_SourceFileURL);
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(Texture, m_Position, Tint);
            m_SpriteBatch.End();
            base.Draw(gameTime);
        }        
    }
}
