using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using System;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;

namespace SpaceInvaders
{
    public class Invader : Sprite, ICollidable2D, IShooter, IEnemy
    {
        private const string k_InvadersSpriteSheet = @"Sprites\Enemies";
        public const int k_DefaultInvaderWidth = 32;
        public const int k_DefaultInvaderHeight = 32;
        private const int k_MaxBulletsInScreen = 1;
        private const int k_NumOfCells = 2;
        public const float k_DefaultDelayBetweenJumpsInSeconds = 0.5f;
        private readonly Gun r_Gun;
        public Color BulletsColor { get; } = Color.Blue;
        public int PointsValue { get; set; }
        public float DelayBetweenJumpsInSeconds = k_DefaultDelayBetweenJumpsInSeconds;
        private int m_StartingCellAnimationIndexInSpriteSheet;

        public Invader(Game i_Game, Color i_Tint, int i_PointsValue, int i_StartingCellAnimationIndexInSpriteSheet) 
            : base(k_InvadersSpriteSheet, i_Game)
        {
            TintColor = i_Tint;
            PointsValue = i_PointsValue;
            m_StartingCellAnimationIndexInSpriteSheet = i_StartingCellAnimationIndexInSpriteSheet;
            r_Gun = new Gun(this, k_MaxBulletsInScreen);
        }

        public override void Initialize()
        {
            base.Initialize();
            initializeAnimations();
        }

        protected override void InitSourceRectangle()
        {
            this.SourceRectangle = new Rectangle(
                (int)(0 + m_StartingCellAnimationIndexInSpriteSheet * Width),
                0,
                k_DefaultInvaderWidth,
                k_DefaultInvaderHeight);
        }

        private void initializeAnimations()
        {
            CellAnimator cellAnimator = new CellAnimator(TimeSpan.FromSeconds(k_DefaultDelayBetweenJumpsInSeconds),
                k_NumOfCells, TimeSpan.Zero);
            cellAnimator.FinishedCellAnimationCycle += onFinishedCellAnimationCycle;
            Animations.Add(cellAnimator);
            Animations.Enabled = true;
        }

        private void onFinishedCellAnimationCycle()
        {
            if (DelayBetweenJumpsInSeconds < (Animations["CelAnimation"] as CellAnimator).CellTime.TotalSeconds)
            {
                (Animations["CelAnimation"] as CellAnimator).CellTime = TimeSpan.FromSeconds(DelayBetweenJumpsInSeconds);
            }
        }

        public void Shoot()
        {
            r_Gun.Shoot();
        }

        protected override void SpecificTextureBounds()
        {
            // manually set the texture height and width of the invader because now we are using a sprite sheet
            WidthBeforeScale = k_DefaultInvaderWidth;
            HeightBeforeScale = k_DefaultInvaderHeight;
        }
    }
}