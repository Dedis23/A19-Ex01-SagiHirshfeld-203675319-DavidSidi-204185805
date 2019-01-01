//*** Guy Ronen © 2008-2011 ***//
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ServiceInterfaces;
using System;

namespace Infrastructure.ObjectModel
{
    public class Sprite : LoadableDrawableComponent
    {
        private Texture2D m_Texture;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        // TODO 01: The Bounds property for collision detection
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)m_Position.X,
                    (int)m_Position.Y,
                    m_Width,
                    m_Height);
            }
        }
        // -- end of TODO 01

        // TODO 13: Notify about  change:
        protected int m_Width;
        public int Width
        {
            get { return m_Width; }
            set
            {
                if (m_Width != value)
                {
                    m_Width = value;
                    OnSizeChanged();
                }
            }
        }

        protected int m_Height;
        public int Height
        {
            get { return m_Height; }
            set
            {
                if (m_Height != value)
                {
                    m_Height = value;
                    OnSizeChanged();
                }
            }
        }

        protected Vector2 m_Position;
        public Vector2 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }
        // -- end of TODO 13

        protected Color m_TintColor = Color.White;
        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        protected Vector2 m_Velocity = Vector2.Zero;
        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public Sprite(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        { }

        public Sprite(string i_AssetName, Game i_Game, int i_CallsOrder)
            : base(i_AssetName, i_Game, i_CallsOrder)
        { }

        public Sprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game, int.MaxValue)
        { }

        protected Rectangle GameScreenBounds
        {
            get { return new Rectangle(0, 0, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height); }
        }

        public bool IsInScreen
        {
            get
            {
                return this.Bounds.Intersects(GameScreenBounds);
            }
        }

        /// <summary>
        /// Default initialization of bounds
        /// </summary>
        /// <remarks>
        /// Derived classes are welcome to override this to implement their specific boudns initialization
        /// </remarks>
        /// 
        protected float Scale { get; set; }
        protected float Rotation { get; set; }
        protected Vector2 PositionOrigin { get; set; }
        protected Vector2 RotationOrigin { get; set; }
        protected override void InitBounds()
        {
            // default initialization of bounds
            SpecificTextureBounds();
            Scale = 1.0f;
            PositionOrigin = Vector2.Zero;
            RotationOrigin = Vector2.Zero;
        }
        protected virtual void SpecificTextureBounds()
        {
            if (m_Texture != null)
            {
                m_Width = m_Texture.Width;
                m_Height = m_Texture.Height;
            }
        }

        public Vector2 DrawingPosition
        {
            get { return Position - PositionOrigin + RotationOrigin; }
        }

        private bool m_UseSharedBatch = true;

        protected SpriteBatch m_SpriteBatch;
        public SpriteBatch SpriteBatch
        {
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        protected override void LoadContent()
        {
            LoadTexture();

            if (m_SpriteBatch == null)
            {
                m_SpriteBatch =
                    Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }

            base.LoadContent();
        }

        // this turned into injection point in case derived class want to make a specific type of Load
        protected virtual void LoadTexture()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
        }

        /// <summary>
        /// Basic movement logic (position += velocity * totalSeconds)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <remarks>
        /// Derived classes are welcome to extend this logic.
        /// </remarks>
        public override void Update(GameTime gameTime)
        {
            this.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        /// <summary>
        /// Basic texture draw behavior, using a shared/own sprite batch
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            SpecificDraw();

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        // this turned into injection point in case derived class want to make a specific draw
        protected virtual void SpecificDraw()
        {
            m_SpriteBatch.Draw(m_Texture, DrawingPosition, m_TintColor);
        }

        // TODO 14: Implement a basic collision detection between two ICollidable2D objects:
        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            if (source != null)
            {
                if (source.Bounds.Intersects(this.Bounds) || source.Bounds.Contains(this.Bounds))
                {
                    collided = checkPixelCollision(source);
                }
            }

            return collided;
        }

        // The following method was mostly taken from this tutorial: 
        // https://www.youtube.com/watch?v=5vKF0zb0PsA - "C# Xna Made Easy Tutorial 27 - Pixel Perfect Collision"
        private bool checkPixelCollision(ICollidable2D i_Source)
        {
            bool collisionDetected = false;

            Texture2D spriteA = this.Texture;
            Texture2D spriteB = i_Source.Texture;

            // Store the pixel data
            Color[] colorDataA = new Color[spriteA.Width * spriteA.Height];
            Color[] colorDataB = new Color[spriteB.Width * spriteB.Height];
            spriteA.GetData(colorDataA);
            spriteB.GetData(colorDataB);

            // Calculate the boundaries of the rectangle which is the overlap between i_CollideableA and i_CollideableB
            // float is used instead of int for numerical capacity
            int top, bottom, left, right;
            top = Math.Max(this.Bounds.Top, i_Source.Bounds.Top);
            bottom = Math.Min(this.Bounds.Bottom, i_Source.Bounds.Bottom);
            left = Math.Max(this.Bounds.Left, i_Source.Bounds.Left);
            right = Math.Min(this.Bounds.Right, i_Source.Bounds.Right);

            // Scan the pixels of the rectangle which defines the overlap
            // and look for a pixel in which both textures are not transparent
            for (int y = top; y < bottom && !collisionDetected; y++)
            {
                for (int x = left; x < right && !collisionDetected; x++)
                {
                    int pixelIndexA = (y - this.Bounds.Top) * (this.Bounds.Width) + (x - this.Bounds.Left);
                    int pixelIndexB = (y - i_Source.Bounds.Top) * (i_Source.Bounds.Width) + (x - i_Source.Bounds.Left);

                    Color pixelOfSpriteA = colorDataA[pixelIndexA];
                    Color pixelOfSpriteB = colorDataB[pixelIndexB];

                    // Color.A is the color's alpha component which determines opacity
                    // when a pixel's alpha == 0 that pixel is completely transparent 
                    collisionDetected = pixelOfSpriteA.A != 0 && pixelOfSpriteB.A != 0;
                }
            }

            return collisionDetected;
        }

        public ICollisionHandler m_CollisionHandler;
        public override void Initialize()
        {
            base.Initialize();

            if (this is ICollidable2D)
            {
                m_CollisionHandler = this.Game.Services.GetService(typeof(ICollisionHandler)) as ICollisionHandler;
            }

            if (this is IAnimated)
            {
                IAnimationManager animationManager = this.Game.Services.GetService(typeof(IAnimationManager)) as IAnimationManager;
                animationManager.AddObjectToMonitor(this as IAnimated);
            }
        }

        // TODO 15: Implement a basic collision reaction between two ICollidable2D objects
        public virtual void Collided(ICollidable i_Collidable)
        {
            if (this is ICollidable && m_CollisionHandler != null)
            {
                m_CollisionHandler.HandleCollision(this as ICollidable, i_Collidable);
            }
        }

        public event Action<object> SpriteKilled;
        public void Kill()
        {
            SpriteKilled?.Invoke(this);
            KilledInjectionPoint();
        }

        protected virtual void KilledInjectionPoint()
        {
            this.Game.Components.Remove(this);
            if (SpriteKilled != null)
            {
                foreach (Delegate d in SpriteKilled.GetInvocationList())
                {
                    SpriteKilled -= (Action<object>)d;
                }
            }

            this.Dispose();
        }
    }
}