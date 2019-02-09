﻿using System;
using Infrastructure;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.Managers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public abstract class Spaceship : Sprite, ICollidable2D, IShooter, IPlayer
    {
        private const string k_ShootSoundEffectAssetName = @"Audio\SSGunShot";
        private const string k_OnBulletHitSoundEffectAssetName = @"Audio\LifeDie";
        private const int k_ScorePenaltyForBulletHit = 1100;
        private const int k_VelocityScalar = 145;
        private const int k_MaxBulletsInScreen = 3;
        private const int k_StartingLivesCount = 3;
        private const float k_LoseLifeAnimationLength = 2.5f;
        private const float k_NumOfBlinksInSecondInLoseLifeAnimation = 6.0f;
        private const float k_DeathAnimationLength = 2.5f;
        private const float k_NumOfCyclesPerSecondsInDeathAnimation = 4.0f;

        private readonly Gun r_Gun;
        private readonly Vector2 r_ShootingDirectionVector = new Vector2(0, -1);

        public event EventHandler<EventArgs> LivesCountChanged;
        public event EventHandler<EventArgs> ScoreChanged;

        private int m_Score;
        private int m_Lives;
        private SoundEffectInstance m_OnBulletHitSoundEffectInstance;

        protected IInputManager InputManager { get; private set; }

        protected abstract Keys MoveLeftKey { get; }

        protected abstract Keys MoveRightKey { get; }

        protected abstract Keys ShootKey { get; }

        public SoundEffectInstance ShootingSoundEffectInstance { get; private set; }

        public Color BulletsColor { get; } = Color.Red;

        public abstract Color ScoreColor { get; }

        public abstract string Name { get; set; }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value > 0 ? value : 0;
                ScoreChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int Lives
        {
            get { return m_Lives; }

            set
            {
                if (m_Lives != value)
                {
                    m_Lives = value;
                    LivesCountChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool IsAlive
        {
            get
            {
                return Lives != 0;
            }
        }


        public Spaceship(string k_AssetName, Game i_Game) : base(k_AssetName, i_Game)
        {
            Lives = k_StartingLivesCount;
            r_Gun = new Gun(this, k_MaxBulletsInScreen);
        }

        public override void Initialize()
        {
            base.Initialize();
            InputManager = Game.Services.GetService<IInputManager>();
            initializeAnimations();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            ShootingSoundEffectInstance = Game.Content.Load<SoundEffect>(k_ShootSoundEffectAssetName).CreateInstance();
            m_OnBulletHitSoundEffectInstance = Game.Content.Load<SoundEffect>(k_OnBulletHitSoundEffectAssetName).CreateInstance();
        }

        private void initializeAnimations()
        {
            BlinkAnimator loseLifeAnimation = new BlinkAnimator(
                "LoseLifeAnimation",
                k_NumOfBlinksInSecondInLoseLifeAnimation,
                TimeSpan.FromSeconds(k_LoseLifeAnimationLength));
            loseLifeAnimation.Finished += onFinishedLoseLifeAnimation;
            Animations.Add(loseLifeAnimation);
            loseLifeAnimation.Pause();

            RotateAnimator rotateAnimator = new RotateAnimator(
                k_NumOfCyclesPerSecondsInDeathAnimation,
                TimeSpan.FromSeconds(k_DeathAnimationLength));
            FaderAnimator faderAnimator = new FaderAnimator(TimeSpan.FromSeconds(k_DeathAnimationLength));

            this.DeathAnimation = new CompositeAnimator(
                "DeathAnimation",
                TimeSpan.FromSeconds(k_DeathAnimationLength),
                this,
                rotateAnimator,
                faderAnimator);

            Animations.Resume();
        }

        private void onFinishedLoseLifeAnimation(object sender, EventArgs e)
        {
            Animations["LoseLifeAnimation"].Reset();
            Animations["LoseLifeAnimation"].Pause();
            this.Vulnerable = true;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (IsAlive)
            {
                TakeInput();
            }            
        }

        protected virtual void TakeInput()
        {
            if (InputManager.KeyboardState.IsKeyDown(MoveLeftKey))
            {
                m_Velocity.X = k_VelocityScalar * -1;
            }

            else if (InputManager.KeyboardState.IsKeyDown(MoveRightKey))
            {
                m_Velocity.X = k_VelocityScalar;
            }

            else
            {
                m_Velocity.X = 0;
            }

            if (InputManager.KeyPressed(ShootKey))
            {
                Shoot();
            }
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();

            // Clamp the position between screen boundries:
            float x = MathHelper.Clamp(Position.X, 0, this.GameScreenBounds.Width - this.Width);
            float y = MathHelper.Clamp(Position.Y, 0, this.GameScreenBounds.Height - this.Height);
            Position = new Vector2(x, y);
        }

        public void Shoot()
        {
            r_Gun.Shoot(r_ShootingDirectionVector);
        }

        public void TakeBulletHit()
        {
            this.Vulnerable = false;
            Lives--;
            Score -= k_ScorePenaltyForBulletHit;

            if (Lives == 0)
            {
                this.Kill();
            }

            else
            {
                Animations["LoseLifeAnimation"].Resume();
                this.Position = DefaultPosition;
            }

            m_OnBulletHitSoundEffectInstance.PauseAndThenPlay();
        }

        protected override void OnDying()
        {
            this.Velocity = Vector2.Zero;
            base.OnDying();
        }

        protected override void OnDeath()
        {
            Visible = false;
        }

        public void PrepareForNewLevel()
        {
            r_Gun.Reset();
            Animations.Reset();
            Animations.PauseSubAnimations();

            Position = DefaultPosition;
            this.Visible = IsAlive;
            this.Vulnerable = IsAlive;
        }

        public void ResetScoreAndLives()
        {
            Lives = k_StartingLivesCount;
            Score = 0;
        }

        protected override void OnDisposed(object sender, EventArgs args)
        {
            base.OnDisposed(sender, args);
            Animations["LoseLifeAnimation"].Finished -= onFinishedLoseLifeAnimation;
        }
    }
}