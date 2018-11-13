using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class RandomSpawner
    {
        private int m_ChanceToSpawn;
        private float m_TimeBetweenRollsInSeconds;
        private float m_RemainingDelay;
        private Random m_RandomGenerator;
        public event Action ObjectSpawned;

        public RandomSpawner(Game i_Game, int i_ChanceToSpawn, float i_TimeBetweenRollsInSeconds)
        {
            m_RandomGenerator = new Random();
            m_ChanceToSpawn = i_ChanceToSpawn;
            m_TimeBetweenRollsInSeconds = i_TimeBetweenRollsInSeconds;
        }

        public void RollForSpawn(GameTime gameTime)
        {
            // To make the rolling based on fixed time and not make it tied to the framerate,
            // we use m_RemainingDelay and m_DelayBetweenRollsInSeconds
            // this way, we make sure we try to spawn the objects at a fixed delay time and no matter what the frame rate is
            m_RemainingDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (m_RemainingDelay >= m_TimeBetweenRollsInSeconds)
            {
                if ((m_RandomGenerator.Next(100) + 1) <= m_ChanceToSpawn)
                {
                    ObjectSpawned?.Invoke();
                }

                m_RemainingDelay = 0;
            }
        }
    }
}