﻿using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.Utilities;

namespace SpaceInvaders
{
    public class Invader : Sprite, ICollidable2D, IShooter, IEnemy
    {
        private const string k_InvadersSpriteSheet = @"Sprites\Enemies";
        private const int k_MaxBulletsInScreen = 1;
        private const int k_MinTimeBetweenShootRolls = 1;
        private const int k_MaxTimeBetweenShootRolls = 3;
        private const float k_DeathAnimationLength = 1.2f;
        private const float k_NumOfCyclesPerSecondsInDeathAnimation = 6.0f;
        public const int k_NumOfCells = 2;
        public const int k_DefaultInvaderWidth = 32;
        public const int k_DefaultInvaderHeight = 32;
        private readonly Gun r_Gun;
        private readonly RandomRoller r_RandomShootRoller;
        private readonly float r_TimeBetweenRollingForShootInSeconds;
        private readonly Vector2 r_ShootingDirectionVector = new Vector2(0, 1);

        //public float DelayBetweenJumpsInSeconds = k_DefaultDelayBetweenJumpsInSeconds;
        private int m_ColIndexInSpriteSheet;
        private int m_RowIndexInSpriteSheet;
        private float m_ChanceToShoot = 5;

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

            this.SourceRectangle = new Rectangle(
                m_ColIndexInSpriteSheet * this.SourceRectangle.Width,
                this.SourceRectangle.Top,
                this.SourceRectangle.Width,
                this.SourceRectangle.Height);
        }

        public void GoNextFrame()
        {
            m_ColIndexInSpriteSheet = (m_ColIndexInSpriteSheet + 1) % k_NumOfCells;
        }

        private void initializeAnimations()
        {
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(k_DeathAnimationLength));
            RotateAnimator rotateAnimator = new RotateAnimator(
                k_NumOfCyclesPerSecondsInDeathAnimation,
                TimeSpan.FromSeconds(k_DeathAnimationLength));

            CompositeAnimator deathAnimation = new CompositeAnimator(
                "DeathAnimation",
                TimeSpan.FromSeconds(k_DeathAnimationLength),
                this,
                shrinkAnimator,
                rotateAnimator);
            deathAnimation.Finished += onFinishedDeathAnimation;
            /// Animations.Add(deathAnimation);
            /// deathAnimation.Pause();
            Animations.Resume();

            this.DeathAnimation = deathAnimation;
        }

        protected override void OnDying()
        {
            Vulnerable = false;
            m_ChanceToShoot++;
            r_RandomShootRoller.RollSucceeded -= Shoot;
            /// Animations["DeathAnimation"].Resume();
        }

        private void onFinishedDeathAnimation(object sender, EventArgs e)
        {
        }

        //public override void Kill()
        //{
        //    Vulnerable = false;
        //    m_ChanceToShoot++;
        //    r_RandomShootRoller.RollSucceeded -= Shoot;
        //    SpriteAnimator deathAnimation = Animations["DeathAnimation"];
        //    deathAnimation.Finished += onFinishedDeathAnimation;
        //    Animations["DeathAnimation"].Resume();
        //}

        //private void onFinishedDeathAnimation(object sender, EventArgs e)
        //{
        //    base.Kill();
        //}

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
            r_RandomShootRoller.RollSucceeded -= Shoot;
            Animations["DeathAnimation"].Finished -= onFinishedDeathAnimation;
        }
    }
}