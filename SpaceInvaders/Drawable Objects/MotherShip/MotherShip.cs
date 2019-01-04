using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;

namespace SpaceInvaders
{
    public class Mothership : Sprite, ICollidable2D, IEnemy
    {
        private const string k_AssetName = @"Sprites\MotherShip_32x120";
        private const int k_MotherShipVelocity = 110;
        private const int k_MotherShipPointsValue = 850;
        private const float k_DeathAnimationTime = 2.2f;
        private const float k_BlinkingAnimationLength = 0.1375f;

        public int PointsValue { get; set; }

        public Mothership(Game i_Game) : base(k_AssetName, i_Game)
        {
            this.TintColor = Color.Red;
            this.Velocity = Vector2.Zero;
            this.Visible = false;
            PointsValue = k_MotherShipPointsValue;
        }

        public override void Initialize()
        {
            base.Initialize();
            initializeAnimations();
        }
        private void initializeAnimations()
        {
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(k_DeathAnimationTime));
            FaderAnimator faderAnimator = new FaderAnimator(TimeSpan.FromSeconds(k_DeathAnimationTime));
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(k_BlinkingAnimationLength),
                TimeSpan.FromSeconds(k_DeathAnimationTime));

            CompositeAnimator deathAnimation = new CompositeAnimator
                ("DeathAnimation",
                TimeSpan.FromSeconds(k_DeathAnimationTime),
                this,
                shrinkAnimator,
                faderAnimator,
                blinkAnimator);
            deathAnimation.Finished += onFinishedDeathAnimation;
            Animations.Add(deathAnimation);
            deathAnimation.Pause();

            Animations.Resume();
        }

        protected override void KilledInjectionPoint()
        {
            Vulnerable = false;
            Animations["DeathAnimation"].Resume();
            Velocity = Vector2.Zero;
        }

        private void onFinishedDeathAnimation(object sender, EventArgs e)
        {
            CompositeAnimator deathAnimation = sender as CompositeAnimator;
            deathAnimation.Reset(); // reset animation to original state
            deathAnimation.Pause(); // (enable = false)
            hideAndWaitForNextSpawn();
        }

        private void hideAndWaitForNextSpawn()
        {
            this.Visible = false;
            this.Velocity = Vector2.Zero;
        }

        public void SpawnAndFly()
        {
            setDefaultPosition();
            this.Vulnerable = true;
            this.Visible = true;
            this.Velocity = new Vector2(k_MotherShipVelocity, 0);
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (this.Vulnerable && this.Position.X >= this.GraphicsDevice.Viewport.Width)
            {
                this.Kill();
            }
        }

        protected override void InitBounds()
        {
            base.InitBounds();

            setDefaultPosition();
        }

        private void setDefaultPosition()
        {
            // Default MotherShip position (coming from the left of the screen) 
            this.Position = new Vector2(-this.Width, this.Height);
        }
    }
}