using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.Utilities
{
    public class RandomRoller : GameComponent
    {
        private RandomGenerator m_RandomGenerator;
        private float m_ChanceToRoll;
        private float m_TimeBetweenRollsInSeconds;
        private Timer r_Timer;

        public event Action RollSucceeded;

        public RandomRoller(Game i_Game, float i_Chance, float i_TimeBetweenRollsInSeconds) : base(i_Game)
        {
            // To make the rolling based on time and not make it tied to the framerate,
            // we use Timer which has m_RemainingDelay and m_DelayBetweenTicksInSeconds
            // this way, we make sure we roll for objects spawns at a fixed delay time, no matter what the frame rate is
            m_ChanceToRoll = i_Chance;
            m_TimeBetweenRollsInSeconds = i_TimeBetweenRollsInSeconds;
            m_RandomGenerator = RandomGenerator.Instance;
            r_Timer = new Timer(i_Game);
            r_Timer.IntervalInSeconds = i_TimeBetweenRollsInSeconds;
            r_Timer.Notify += roll;
        }

        public float ChanceToRoll
        {
            get { return m_ChanceToRoll; }
            set { m_ChanceToRoll = MathHelper.Clamp(value, 0, 100); }
        }

        public float TimeBetweenRollsInSeconds
        {
            get { return m_TimeBetweenRollsInSeconds; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                m_TimeBetweenRollsInSeconds = value;
                r_Timer.IntervalInSeconds = m_TimeBetweenRollsInSeconds;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            r_Timer.Update(gameTime);
        }

        public void Activate()
        {
            r_Timer.Activate();
        }

        public void Deactivate()
        {
            r_Timer.Deactivate();
        }

        private void roll()
        {
            if (m_RandomGenerator.Next(1, 100) <= m_ChanceToRoll)
            {
                RollSucceeded?.Invoke();
            }
        }
    }
}
