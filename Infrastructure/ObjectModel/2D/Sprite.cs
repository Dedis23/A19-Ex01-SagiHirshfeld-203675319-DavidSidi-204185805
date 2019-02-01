using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Animators;

namespace Infrastructure.ObjectModel
{
    public class Sprite : LoadableDrawableComponent
    {
        // Markup attribute to assist the CopyPropertiesFrom method
        [AttributeUsage(AttributeTargets.Property)]
        private class CopyingNotAllowed : System.Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Class)]
        protected class DontPremultiplyAlpha : System.Attribute
        {
        }

        private Texture2D m_Texture;
        private Color[] m_TextureData;
        private ICollisionHandler m_CollisionHandler;
        private bool m_UseSharedBatch = false;
        private BlendState m_PrivateBlendState = BlendState.AlphaBlend;

        protected SpriteBatch m_SpriteBatch;
        protected CompositeAnimator m_Animations;

        protected Vector2 m_Position;
        protected Vector2 m_DefaultPosition;
        protected Vector2 m_PositionOrigin;
        protected Vector2 m_RotationOrigin;
        protected Vector2 m_Scales = Vector2.One;
        protected Vector2 m_Velocity;
        protected float m_WidthBeforeScale;
        protected float m_HeightBeforeScale;
        protected float m_Rotation;
        protected float m_LayerDepth;
        protected Rectangle m_SourceRectangle;
        protected Color m_TintColor = Color.White;
        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;
        protected bool m_Vulnerable = true;
        private bool m_UseNonPremultipliedSpriteBatch;

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

        [CopyingNotAllowed]
        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        [CopyingNotAllowed]
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        [CopyingNotAllowed]
        public Color[] TextureData
        {
            get
            {
                if (m_TextureData == null)
                {
                    m_TextureData = new Color[Texture.Width * Texture.Height];
                    Texture.GetData(m_TextureData);
                }

                return m_TextureData;
            }

            set
            {
                m_TextureData = value;
                m_Texture.SetData(m_TextureData);
            }
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

        public Vector2 DefaultPosition
        {
            get { return m_DefaultPosition; }
            set { m_DefaultPosition = value; }
        }

        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

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

        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public Vector2 DirectionVector
        {
            get
            {
                return Vector2.Normalize(Velocity);
            }
        }

        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

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

        public void CopyPropertiesFrom(Sprite i_Source)
        {
            PropertyInfo[] targetSpriteProperties = this.GetType().GetProperties();
            PropertyInfo[] firstSpriteProperties = i_Source.GetType().GetProperties();
            foreach (PropertyInfo newProperty in targetSpriteProperties)
            {
                foreach (PropertyInfo firstProperty in firstSpriteProperties)
                {
                    bool propertyMarkedNotToCopy = firstProperty.GetCustomAttribute(typeof(CopyingNotAllowed)) != null;
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

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

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

            m_UseNonPremultipliedSpriteBatch = GetType().GetCustomAttribute(typeof(DontPremultiplyAlpha)) != null;

            if (m_SpriteBatch == null)
            {
                takeSpriteBatchFromGameServices();

                if (m_SpriteBatch == null)
                {
                    CreateAndUsePrivateSpriteBatch();
                }
            }

            base.LoadContent();
        }

        private void takeSpriteBatchFromGameServices()
        {
            if (m_UseNonPremultipliedSpriteBatch)
            {
                m_SpriteBatch = Game.Services.GetService(typeof(NonPremultipliedSpriteBatch)) as SpriteBatch;
            }
            else
            {
                m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            }

            if (m_SpriteBatch != null)
            {
                m_UseSharedBatch = true;
            }
        }

        protected void CreateAndUsePrivateSpriteBatch()
        {
            m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            m_UseSharedBatch = false;
            if (m_UseNonPremultipliedSpriteBatch)
            {
                m_PrivateBlendState = BlendState.NonPremultiplied;
            }
        }

        // This turned into injection point in case derived class want to make a specific type of Load
        protected virtual void LoadTexture()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
        }

        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        public override void Update(GameTime gameTime)
        {
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Position += this.Velocity * totalSeconds;

            this.Animations.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin(SpriteSortMode.Deferred, m_PrivateBlendState);
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
            m_SpriteBatch.Draw(
                m_Texture,
                this.DrawingPosition,
                this.SourceRectangle,
                this.TintColor,
                this.Rotation,
                this.RotationOrigin,
                this.Scales,
                this.SpriteEffects,
                this.LayerDepth);
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

        private bool checkPixelCollision(ICollidable2D i_Source)
        {
            const bool v_StopAfterFirstDetection = true;
            return LookForCollidingPixels(i_Source, v_StopAfterFirstDetection);
        }

        protected bool LookForCollidingPixels(ICollidable2D i_Source, bool i_StopAfterFirstDetection)
        {
            Func<Color, Color> nulledModificationFunc = null;
            return LookForCollidingPixels(i_Source, i_StopAfterFirstDetection, nulledModificationFunc);
        }

        protected bool LookForCollidingPixels(ICollidable2D i_Source, bool i_StopAfterFirstDetection, Func<Color, Color> i_ModifyCollidedPixelFunc)
        {
            bool collisionDetected = false;
            bool done = false;

            Rectangle intersection = Rectangle.Intersect(this.Bounds, i_Source.Bounds);

            // Scan the pixels of both sprites within their intersection
            // and look for a pixel in which both textures are not transparent
            for (int y = intersection.Top; y < intersection.Bottom && !done; y++)
            {
                for (int x = intersection.Left; x < intersection.Right && !done; x++)
                {
                    int pixelIndexA = ((y - this.Bounds.Top) * this.Bounds.Width) + (x - this.Bounds.Left);
                    int pixelIndexB = ((y - i_Source.Bounds.Top) * i_Source.Bounds.Width) + (x - i_Source.Bounds.Left);

                    // Color.A is the color's alpha component which determines opacity
                    // when a pixel's alpha == 0 that pixel is completely transparent 
                    if (this.TextureData[pixelIndexA].A != 0 && i_Source.TextureData[pixelIndexB].A != 0)
                    {
                        collisionDetected = true;
                        if (i_ModifyCollidedPixelFunc != null)
                        {
                            TextureData[pixelIndexA] = i_ModifyCollidedPixelFunc(TextureData[pixelIndexA]);
                        }

                        done = i_StopAfterFirstDetection;
                    }
                }
            }

            // If the data was modified: set it to the texture
            if (i_ModifyCollidedPixelFunc != null)
            {
                this.Texture.SetData(this.m_TextureData);
            }

            return collisionDetected;
        }

        public override void Initialize()
        {
            base.Initialize();
            if (this is ICollidable2D)
            {
                m_CollisionHandler = this.Game.Services.GetService(typeof(ICollisionHandler)) as ICollisionHandler;
            }
        }

        public virtual void Collided(ICollidable i_Collidable)
        {
            if (this is ICollidable && m_CollisionHandler != null)
            {
                m_CollisionHandler.HandleCollision(this as ICollidable, i_Collidable);
            }
        }

        public event Action<object> Dying;
        public event Action<object> Died;

        public SpriteAnimator DeathAnimation
        {
            set
            {
                SpriteAnimator animation = value;
                if (animation.Name == "DeathAnimation")
                {
                    Animations.Remove("DeathAnimation");
                    Animations.Add(animation);
                    animation.Pause();
                }
            }

            get
            {
                return Animations["DeathAnimation"];
            }
        }

        public void Kill()
        {
            Dying?.Invoke(this);
            OnDying();

            if (DeathAnimation != null)
            {
                DeathAnimation.Finished += onDeathAnimationFinished;
                DeathAnimation.Resume();
            }
            else
            {
                die();
            }
        }

        private void onDeathAnimationFinished(object sender, EventArgs e)
        {
            DeathAnimation.Finished -= onDeathAnimationFinished;
            die();
        }

        protected virtual void OnDying()
        {
        }

        private void die()
        {
            Died?.Invoke(this);
            OnDeath();
        }

        protected virtual void OnDeath()
        {
            Dispose();
        }

        //public virtual void Kill()
        //{
        //    SpriteKilled?.Invoke(this);
        //    KilledInjectionPoint();
        //}

        //protected virtual void KilledInjectionPoint()
        //{
        //    Dispose();
        //}
    }
}