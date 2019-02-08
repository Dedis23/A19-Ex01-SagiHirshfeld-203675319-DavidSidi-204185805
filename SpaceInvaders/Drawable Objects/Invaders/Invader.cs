using System;
using Microsoft.Xna.Framework;
using Infrastructure;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.Utilities;
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders
{
    public class Invader : Sprite, ICollidable2D, IShooter, IEnemy
    {
        public const int k_NumOfCells = 2;
        public const int k_DefaultInvaderWidth = 32;
        public const int k_DefaultInvaderHeight = 32;

        private const string k_ShootingSoundEffectAssetName = @"Audio\EnemyGunShot";
        private const string k_DyingSoundEffectAssetName = @"Audio\EnemyKill";
        private const string k_InvadersSpriteSheet = @"Sprites\Enemies";
        private const int k_MaxBulletsInScreen = 1;
        private const int k_MinTimeBetweenShootRolls = 1;
        private const int k_MaxTimeBetweenShootRolls = 3;
        private const float k_DeathAnimationLength = 1.2f;
        private const float k_NumOfCyclesPerSecondsInDeathAnimation = 6.0f;
        private readonly Gun r_Gun;
        private readonly RandomRoller r_RandomShootRoller;
        private readonly float r_TimeBetweenRollingForShootInSeconds;
        private readonly Vector2 r_ShootingDirectionVector = new Vector2(0, 1);

        private SoundEffectInstance m_DyingSoundEffectInstance;
        private int m_ColIndexInSpriteSheet;
        private int m_RowIndexInSpriteSheet;
        private float m_ChanceToShoot;

        public int PointsValue { get; set; }

        public Color BulletsColor { get; } = Color.Blue;

        public float ChanceToShoot
        {
            get { return m_ChanceToShoot; }
            set
            {
                value = MathHelper.Clamp(value, 0, 100);
                m_ChanceToShoot = value;
                if (r_RandomShootRoller != null)
                {
                    r_RandomShootRoller.ChanceToRoll = m_ChanceToShoot;
                }
            }
        }

        public SoundEffectInstance ShootingSoundEffectInstance { get; private set; }

        public Invader(
            Game i_Game,
            Color i_Tint,
            int i_PointsValue,
            int i_ColIndexInSpriteSheet,
            int i_RowIndexInSpriteSheet)
            : base(k_InvadersSpriteSheet, i_Game)
        {
            TintColor = i_Tint;
            PointsValue = i_PointsValue;
            m_ColIndexInSpriteSheet = i_ColIndexInSpriteSheet;
            m_RowIndexInSpriteSheet = i_RowIndexInSpriteSheet;
            r_Gun = new Gun(this, k_MaxBulletsInScreen);

            r_TimeBetweenRollingForShootInSeconds = RandomGenerator.Instance.NextFloat(k_MinTimeBetweenShootRolls, k_MaxTimeBetweenShootRolls);
            r_RandomShootRoller = new RandomRoller(i_Game, m_ChanceToShoot, r_TimeBetweenRollingForShootInSeconds);
            r_RandomShootRoller.RollSucceeded += Shoot;
            r_RandomShootRoller.Activate();
        }

        public override void Initialize()
        {
            base.Initialize();
            initializeAnimations();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            ShootingSoundEffectInstance = Game.Content.Load<SoundEffect>(k_ShootingSoundEffectAssetName).CreateInstance();
            m_DyingSoundEffectInstance = Game.Content.Load<SoundEffect>(k_DyingSoundEffectAssetName).CreateInstance();
        }

        protected override void InitSourceRectangle()
        {
            m_WidthBeforeScale = k_DefaultInvaderWidth;
            m_HeightBeforeScale = k_DefaultInvaderHeight;

            this.SourceRectangle = new Rectangle(
                m_ColIndexInSpriteSheet * k_DefaultInvaderWidth,
                m_RowIndexInSpriteSheet * k_DefaultInvaderHeight,
                k_DefaultInvaderWidth,
                k_DefaultInvaderHeight);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /// Look for a better way
            r_RandomShootRoller.Update(gameTime);
        }

        public void GoNextFrame()
        {
            m_ColIndexInSpriteSheet = (m_ColIndexInSpriteSheet + 1) % k_NumOfCells;

            this.SourceRectangle = new Rectangle(
                m_ColIndexInSpriteSheet * this.SourceRectangle.Width,
                this.SourceRectangle.Top,
                this.SourceRectangle.Width,
                this.SourceRectangle.Height);
        }

        private void initializeAnimations()
        {
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(k_DeathAnimationLength));
            RotateAnimator rotateAnimator = new RotateAnimator(
                k_NumOfCyclesPerSecondsInDeathAnimation,
                TimeSpan.FromSeconds(k_DeathAnimationLength));

            this.DeathAnimation = new CompositeAnimator(
                "DeathAnimation",
                TimeSpan.FromSeconds(k_DeathAnimationLength),
                this,
                shrinkAnimator,
                rotateAnimator);

            Animations.Resume();
        }

        protected override void OnDying()
        {
            Vulnerable = false;
            m_DyingSoundEffectInstance.PauseAndThenPlay();
            r_RandomShootRoller.RollSucceeded -= Shoot;
        }

        public void Shoot()
        {
            r_Gun.Shoot(r_ShootingDirectionVector);
        }

        protected override void InitRotationOrigin()
        {
            RotationOrigin = new Vector2(k_DefaultInvaderWidth / 2, k_DefaultInvaderHeight / 2);
        }

        protected override void OnDisposed(object sender, EventArgs args)
        {
            base.OnDisposed(sender, args);
            r_Gun.Reset();
            r_RandomShootRoller.RollSucceeded -= Shoot;
        }
    }
}