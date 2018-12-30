using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    //public abstract class Sprite : DrawableGameComponent, IKillable
    //{
    //    private readonly string r_SourceFileURL;
    //    private Vector2 m_Position;
    //    public Color Color { get; set; } = Color.White;
    //    public Texture2D Texture { get; set; }
    //    public Vector2 Velocity { get; set; }
    //    public event Action<object> Killed;

    //    public float PositionX
    //    {
    //        get
    //        {
    //            return m_Position.X;
    //        }

    //        set
    //        {
    //            m_Position.X = value;
    //        }
    //    }

    //    public float PositionY
    //    {
    //        get
    //        {
    //            return m_Position.Y;
    //        }

    //        set
    //        {
    //            m_Position.Y = value;
    //        }
    //    }

    //    public Vector2 Position
    //    {
    //        get
    //        {
    //            return m_Position;
    //        }
    //    }

    //    public int Width
    //    {
    //        get
    //        {
    //            return Texture != null ? Texture.Width : 0;
    //        }
    //    }

    //    public int Height
    //    {
    //        get
    //        {
    //            return Texture != null ? Texture.Height : 0;
    //        }
    //    }

    //    public int Top
    //    {
    //        get
    //        {
    //            return (int)Math.Floor(m_Position.Y);
    //        }
    //    }

    //    public int Bottom
    //    {
    //        get
    //        {
    //            return (int)Math.Floor(m_Position.Y) + this.Height;
    //        }
    //    }

    //    public int Left
    //    {
    //        get
    //        {
    //            return (int)Math.Floor(m_Position.X);
    //        }
    //    }

    //    public int Right
    //    {
    //        get
    //        {
    //            return (int)Math.Floor(m_Position.X) + this.Width;
    //        }
    //    }

    //    public bool IsInScreen
    //    {
    //        get
    //        {
    //            Rectangle spriteRectangle = new Rectangle(this.Left, this.Top, this.Width, this.Height);
    //            Rectangle screenRectangle = new Rectangle(0, 0, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
    //            return spriteRectangle.Intersects(screenRectangle);
    //        }
    //    }

    //    public Sprite(Game i_Game, string i_SourceFileURL) : base(i_Game)
    //    {
    //        r_SourceFileURL = i_SourceFileURL;
    //        Texture = Game.Content.Load<Texture2D>(r_SourceFileURL);
    //    }

    //    public override void Update(GameTime i_GameTime)
    //    {
    //        m_Position += (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
    //        base.Update(i_GameTime);
    //    }

    //    public virtual void Kill()
    //    {
    //        this.Game.Components.Remove(this);

    //        if (Killed != null)
    //        {
    //            Killed.Invoke(this);
    //            foreach (Delegate d in Killed.GetInvocationList())
    //            {
    //                Killed -= (Action<object>)d;
    //            }
    //        }

    //        this.Dispose();
    //    }
    //}
}
