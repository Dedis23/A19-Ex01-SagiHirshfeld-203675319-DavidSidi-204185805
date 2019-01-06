using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ServiceInterfaces;
using System;
using Infrastructure.ObjectModel.Animators;
using SpaceInvaders;
using System.Reflection;

namespace Infrastructure.ObjectModel
{
    public class Sprite : LoadableDrawableComponent
    {    
        // Markup attribute to assist the DeepCopyFrom method
        [AttributeUsage(AttributeTargets.Property)]
        private class DeepCopyNotAllowedFromThisProperty : System.Attribute
        {
        }

        public Sprite(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
            m_Animations = new CompositeAnimator(this);
        }

        public Sprite(string i_AssetName, Game i_Game, int i_CallsOrder)
            : this(i_AssetName, i_Game, i_CallsOrder, i_CallsOrder)
        {
        }

        public Sprite(string i_AssetName, Game i_Game)
            : this(i_AssetName, i_Game, int.MaxValue)
        {
        }

        protected CompositeAnimator m_Animations;

        [DeepCopyNotAllowedFromThisProperty]
        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        private Texture2D m_Texture;

        [DeepCopyNotAllowedFromThisProperty]
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)m_Position.X,
                    (int)m_Position.Y,
                    (int)Width,
                    (int)Height);
            }
        }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
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

        protected Vector2 m_DefaultPosition;
        public Vector2 DefaultPosition
        {
            get { return m_DefaultPosition; }
            set { m_DefaultPosition = value; }
        }


        protected float m_WidthBeforeScale;
        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        protected float m_HeightBeforeScale;
        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

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

        protected float m_LayerDepth;
        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;
        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

        protected bool m_Vulnerable = true;
        public bool Vulnerable
        {
            get { return m_Vulnerable; }
            set
            {
                if (m_Vulnerable != value)
                {
                    m_Vulnerable = value;
                    OnVulnerableChanged();
                }
            }
        }

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
        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }


        public Sprite ShallowClone()
        {
            return this.MemberwiseClone() as Sprite;
        }

        public void DeepCopyFrom(Sprite i_Source)
        {
            PropertyInfo[] targetSpriteProperties = this.GetType().GetProperties();
            PropertyInfo[] firstSpriteProperties = i_Source.GetType().GetProperties();
            foreach (PropertyInfo newProperty in targetSpriteProperties)
            {
                foreach (PropertyInfo firstProperty in firstSpriteProperties)
                {
                    bool propertyMarkedNotToCopy = firstProperty.GetCustomAttribute(typeof(DeepCopyNotAllowedFromThisProperty)) != null;
                    bool propertyInaccessible = newProperty.GetSetMethod() == null || newProperty.GetGetMethod() == null;
                    if (propertyMarkedNotToCopy || propertyInaccessible)
                    {
                        break;
                    }

                    else if (newProperty.Name == firstProperty.Name)
                    {
                        newProperty.SetValue(this, firstProperty.GetValue(i_Source));
                        break;
                    }
                }
            }
        }

        protected Rectangle m_SourceRectangle;
        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        protected float m_Rotation = 0;
        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        public Vector2 m_PositionOrigin;
        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        protected override void InitBounds()
        {
            // default initialization of bounds
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
            PositionOrigin = Vector2.Zero;
            InitSourceRectangle();
            InitRotationOrigin();
        }

        /// <summary>
        /// Default initialization of bounds
        /// </summary>
        /// <remarks>
        /// Derived classes are welcome to override this to implement their specific boundns initialization
        /// </remarks>
        ///
        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        // Default rotation origin is the texture cetner, this turned into injection point in case using sprite sheet
        protected virtual void InitRotationOrigin()
        {
            RotationOrigin = TextureCenter;
        }

        public Vector2 DrawingPosition
        {
            get { return Position - PositionOrigin + RotationOrigin; }
        }

        protected Vector2 m_Scales = Vector2.One;
        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    OnPositionChanged();
                }
            }
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

        // This turned into injection point in case derived class want to make a specific type of Load
        protected virtual void LoadTexture()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
        }

        public Vector2 m_RotationOrigin = Vector2.Zero;
        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
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
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Position += this.Velocity * totalSeconds;

            this.Animations.Update(gameTime);

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

            SpecificSpriteBatchDraw();

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        protected virtual void SpecificSpriteBatchDraw()
        {
            m_SpriteBatch.Draw(m_Texture, this.DrawingPosition,
            this.SourceRectangle, this.TintColor,
            this.Rotation, this.RotationOrigin, this.Scales,
            SpriteEffects.None, this.LayerDepth);
        }

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

            Rectangle intersection = Rectangle.Intersect(this.Bounds, i_Source.Bounds);

            // Scan the pixels of both sprites within their intersection
            // and look for a pixel in which both textures are not transparent
            for (int y = intersection.Top; y < intersection.Bottom && !collisionDetected; y++)
            {
                for (int x = intersection.Left; x < intersection.Right && !collisionDetected; x++)
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
            RemoveAndDestory();
        }

        protected virtual void RemoveAndDestory()
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