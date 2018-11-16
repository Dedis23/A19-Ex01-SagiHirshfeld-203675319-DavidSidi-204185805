using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Spawner
    {
        private readonly Random r_RandomGenerator;
        private int m_ChanceToSpawn;
        private Timer r_Timer;

        public event Action Spawned;

        public Spawner(Game i_Game, int i_Chance, float i_TimeBetweenRollsInSeconds)
        {
            // To make the rolling based on time and not make it tied to the framerate,
            // we use Timer which has m_RemainingDelay and m_DelayBetweenTicksInSeconds
            // this way, we make sure we roll for objects spawns at a fixed delay time, no matter what the frame rate is
            r_RandomGenerator = new Random((int)DateTime.Now.Ticks);
            m_ChanceToSpawn = i_Chance;
            r_Timer = new Timer(i_Game);
            r_Timer.Interval = i_TimeBetweenRollsInSeconds;
            r_Timer.Notify += rollForSpawn;
        }

        public void Activate()
        {
            r_Timer.Activate();
        }

        public void DeActivate()
        {
            r_Timer.DeActivate();
        }

        private void rollForSpawn()
        {
            if (r_RandomGenerator.Next(1, 100) <= m_ChanceToSpawn)
            {
                Spawned?.Invoke();
            }
        }
    }
}