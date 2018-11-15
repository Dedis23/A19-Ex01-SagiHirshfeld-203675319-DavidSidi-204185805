using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Spawner
    {
        private int m_ChanceToSpawn;
        private Timer m_Timer;
        private Random m_RandomGenerator;
        public event Action Spawned;

        public Spawner(Game i_Game, int i_Chance, float i_TimeBetweenRollsInSeconds)
        {
            // To make the rolling based on time and not make it tied to the framerate,
            // we use Timer which has m_RemainingDelay and m_DelayBetweenTicksInSeconds
            // this way, we make sure we roll for objects spawns at a fixed delay time, no matter what the frame rate is
            m_RandomGenerator = new Random();
            m_ChanceToSpawn = i_Chance;
            m_Timer = new Timer(i_Game);
            m_Timer.Interval = i_TimeBetweenRollsInSeconds;
            m_Timer.Notify += rollForSpawn;
        }

        public void Activate()
        {
            m_Timer.Activate();
        }

        public void DeActivate()
        {
            m_Timer.DeActivate();
        }

        private void rollForSpawn()
        {
            if (m_RandomGenerator.Next(1, 100) <= m_ChanceToSpawn)
            {
                Spawned?.Invoke();
            }
        }
    }
}