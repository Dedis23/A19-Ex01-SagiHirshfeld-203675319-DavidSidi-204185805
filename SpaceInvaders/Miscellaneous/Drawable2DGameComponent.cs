﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public abstract class Drawable2DGameComponent : DrawableGameComponent, IKillable
    {
        readonly private String r_SourceFileURL;
        public Vector2 m_Position;
        public Color Tint { get; set; } = Color.White;
        public Texture2D Texture { get; set; }
        public int Velocity { get; set; }
        private SpriteBatch m_SpriteBatch;
        public event Action<object> Killed;

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
                Rectangle spriteRectangle = new Rectangle(this.Left, this.Top, this.Width, this.Height);
                Rectangle screenRectangle = new Rectangle(0, 0, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
                return spriteRectangle.Intersects(screenRectangle);
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

        public virtual void Kill()
        {
            this.Game.Components.Remove(this);
            Killed?.Invoke(this);
            this.Dispose();
        }
    }
}