using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;

namespace SpaceInvaders
{
    public abstract class Spaceship : Sprite, ICollidable2D, IShooter, IPlayer
    {        
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

        public event Action LifeLost;

        public event Action ScoreChanged;

        private int m_Score;

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value > 0 ? value : 0;
                ScoreChanged?.Invoke();
            }
        }

        public Color BulletsColor { get; } = Color.Red;

        protected IInputManager InputManager { get; private set; }

        public int Lives { get; set; }

        public abstract Color ScoreColor { get; }

        public abstract string Name { get; set; }

        public Spaceship(string k_AssetName, Game i_Game) : base(k_AssetName, i_Game)
        {
            Lives = k_StartingLivesCount;
            r_Gun = new Gun(this, k_MaxBulletsInScreen);
        }

        public override void Initialize()
        {
            base.Initialize();
            InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            initializeAnimations();
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

            CompositeAnimator deathAnimation = new CompositeAnimator(
                "DeathAnimation",
                TimeSpan.FromSeconds(k_DeathAnimationLength),
                this,
                rotateAnimator,
                faderAnimator);
            deathAnimation.Finished += onFinishedDeathAnimation;
            Animations.Add(deathAnimation);
            deathAnimation.Pause();

            Animations.Resume();
        }

        private void onFinishedLoseLifeAnimation(object sender, EventArgs e)
        {
            Animations["LoseLifeAnimation"].Reset();
            Animations["LoseLifeAnimation"].Pause();
            this.Vulnerable = true;
        }

        private void onFinishedDeathAnimation(object sender, EventArgs e)
        {
            this.Kill();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            TakeInput();

            // Clamp the position between screen boundries:
            float x = MathHelper.Clamp(Position.X, 0, this.GameScreenBounds.Width - this.Width);
            Position = new Vector2(x, Position.Y);
        }

        protected virtual void TakeInput()
        {
            if (MoveLeftDetected())
            {
                m_Velocity.X = k_VelocityScalar * -1;
            }
            else if (MoveRightDetected())
            {
                m_Velocity.X = k_VelocityScalar;
            }
            else
            {
                m_Velocity.X = 0;
            }

            if (ShootDetected())
            {
                Shoot();
            }
        }

        // Injection points that are meant to be overriden by inheritors
        protected virtual bool MoveLeftDetected()
        {
            return false;
        }

        protected virtual bool MoveRightDetected()
        {
            return false;
        }

        protected virtual bool ShootDetected()
        {
            return false;
        }

        private void Shoot()
        {
            r_Gun.Shoot(r_ShootingDirectionVector);
        }

        public void TakeBulletHit()
        {
            this.Vulnerable = false;
            Lives--;
            LifeLost?.Invoke();

            Score -= k_ScorePenaltyForBulletHit;

            if (Lives == 0)
            {
                this.r_Gun.Enabled = false;
                Animations["DeathAnimation"].Resume();
            }
            else
            {
                Animations["LoseLifeAnimation"].Resume();
                this.Position = DefaultPosition;
            }
        }
    }
}