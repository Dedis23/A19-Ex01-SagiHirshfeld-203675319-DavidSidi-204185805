using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using System;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Animators;

namespace SpaceInvaders
{
    public class Invader : Sprite, ICollidable2D, IShooter, IEnemy
    {
        private const string k_InvadersSpriteSheet = @"Sprites\Enemies";
        public const int k_DefaultInvaderWidth = 32;
        public const int k_DefaultInvaderHeight = 32;
        private const int k_MaxBulletsInScreen = 1;
        public const int k_NumOfCells = 2;
        public const float k_DefaultDelayBetweenJumpsInSeconds = 0.5f;
        private const float k_DeathAnimationTime = 1.2f;
        private const float k_NumOfCyclesPerSecondsInDeathAnimation = 6.0f;
        private readonly Gun r_Gun;
        private readonly Vector2 r_ShootingDirectionVector = new Vector2(0, 1);
        public Color BulletsColor { get; } = Color.Blue;
        public int PointsValue { get; set; }
        public float DelayBetweenJumpsInSeconds = k_DefaultDelayBetweenJumpsInSeconds;
        private int m_ColIndexInSpriteSheet;
        private int m_RowIndexInSpriteSheet;

        public Invader(Game i_Game, Color i_Tint, int i_PointsValue,
            int i_ColIndexInSpriteSheet,
            int i_RowIndexInSpriteSheet) 
            : base(k_InvadersSpriteSheet, i_Game)
        {
            TintColor = i_Tint;
            PointsValue = i_PointsValue;
            m_ColIndexInSpriteSheet = i_ColIndexInSpriteSheet;
            m_RowIndexInSpriteSheet = i_RowIndexInSpriteSheet;
            r_Gun = new Gun(this, k_MaxBulletsInScreen);
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
                (int)(0 + m_ColIndexInSpriteSheet * k_DefaultInvaderWidth),
                (int)(0 + m_RowIndexInSpriteSheet * k_DefaultInvaderHeight),
                k_DefaultInvaderWidth,
                k_DefaultInvaderHeight);
        }

        private void initializeAnimations()
        {
            CellAnimator cellAnimator = new CellAnimator(TimeSpan.FromSeconds(k_DefaultDelayBetweenJumpsInSeconds),
                k_NumOfCells, TimeSpan.Zero, m_ColIndexInSpriteSheet);
            cellAnimator.FinishedCellAnimationCycle += onFinishedCellAnimationCycle;
            Animations.Add(cellAnimator);

            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(k_DeathAnimationTime));
            RotateAnimator rotateAnimator = new RotateAnimator(
                k_NumOfCyclesPerSecondsInDeathAnimation,
                TimeSpan.FromSeconds(k_DeathAnimationTime));

            CompositeAnimator deathAnimation = new CompositeAnimator
                ("DeathAnimation",
                TimeSpan.FromSeconds(k_DeathAnimationTime),
                this,
                shrinkAnimator, rotateAnimator);
            deathAnimation.Finished += onFinishedDeathAnimation;
            Animations.Add(deathAnimation);
            deathAnimation.Pause();

            Animations.Resume();
        }

        private void onFinishedCellAnimationCycle()
        {
            // increase cell animation speed based on jumping speed
            if (DelayBetweenJumpsInSeconds < (Animations["CellAnimator"] as CellAnimator).CellTime.TotalSeconds)
            {
                (Animations["CellAnimator"] as CellAnimator).CellTime = TimeSpan.FromSeconds(DelayBetweenJumpsInSeconds);
            }
        }

        protected override void KilledInjectionPoint()
        {
            Vulnerable = false;
            Animations["DeathAnimation"].Resume();
        }

        private void onFinishedDeathAnimation(object sender, EventArgs e)
        {
            this.RemoveAndDestory();
        }

        public void Shoot()
        {
            r_Gun.Shoot(r_ShootingDirectionVector);
        }

        protected override void InitRotationOrigin()
        {
            RotationOrigin = new Vector2(k_DefaultInvaderWidth / 2, k_DefaultInvaderHeight / 2);
        }
    }
}