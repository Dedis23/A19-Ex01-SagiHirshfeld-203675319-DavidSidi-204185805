using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.Utilities;

namespace SpaceInvaders
{
    [DontPremultiplyAlpha]
    public class Mothership : Sprite, ICollidable2D, IEnemy
    {
        private const string k_AssetName = @"Sprites\MotherShip_32x120";
        private const int k_ChanceToSpawn = 100;
        private const float k_TimeBetweenRolls = 1;
        private const int k_MotherShipVelocity = 110;
        private const int k_MotherShipPointsValue = 850;
        private const float k_DeathAnimationLength = 2.2f;
        private const float k_NumOfBlinksInASecondInDeathAnimation = 4.0f;
        private readonly RandomRoller r_RandomSpawnRoller;

        public int PointsValue { get; set; }

        public Mothership(Game i_Game) : base(k_AssetName, i_Game)
        {
            this.TintColor = Color.Red;
            this.Velocity = Vector2.Zero;
            this.Visible = false;
            PointsValue = k_MotherShipPointsValue;

            r_RandomSpawnRoller = new RandomRoller(i_Game, k_ChanceToSpawn, k_TimeBetweenRolls);
            r_RandomSpawnRoller.RollSucceeded += SpawnAndFly;
            r_RandomSpawnRoller.Activate();
        }

        public override void Initialize()
        {
            base.Initialize();
            initializeAnimations();
        }

        private void initializeAnimations()
        {
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(k_DeathAnimationLength));
            FaderAnimator faderAnimator = new FaderAnimator(TimeSpan.FromSeconds(k_DeathAnimationLength));
            BlinkAnimator blinkAnimator = new BlinkAnimator(
                k_NumOfBlinksInASecondInDeathAnimation,
                TimeSpan.FromSeconds(k_DeathAnimationLength));

            CompositeAnimator deathAnimation = new CompositeAnimator(
                "DeathAnimation",
                TimeSpan.FromSeconds(k_DeathAnimationLength),
                this,
                shrinkAnimator,
                faderAnimator,
                blinkAnimator);
            /// Animations.Add(deathAnimation);
            /// deathAnimation.Pause();

            DeathAnimation = deathAnimation;

            Animations.Resume();
        }

        public void SpawnAndFly()
        {
            r_RandomSpawnRoller.Deactivate();
            this.Position = DefaultPosition;
            this.Vulnerable = true;
            this.Visible = true;
            this.Velocity = new Vector2(k_MotherShipVelocity, 0);
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (this.Position.X >= this.GraphicsDevice.Viewport.Width)
            {
                hideAndWaitForNextSpawn();
            }
        }

        private void hideAndWaitForNextSpawn()
        {
            this.Visible = false;
            this.Velocity = Vector2.Zero;
            r_RandomSpawnRoller.Activate();
        }

        protected override void OnDying()
        {
            Vulnerable = false;
            Animations["DeathAnimation"].Resume();
            Velocity = Vector2.Zero;
        }

        protected override void OnDeath()
        {
            DeathAnimation.Reset();
            DeathAnimation.Pause();
            hideAndWaitForNextSpawn();
        }

        protected override void OnDisposed(object sender, EventArgs args)
        {
            base.OnDisposed(sender, args);
            r_RandomSpawnRoller.RollSucceeded -= SpawnAndFly;
        }
    }
}