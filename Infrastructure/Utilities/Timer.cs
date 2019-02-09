using System;
using Microsoft.Xna.Framework;

using Infrastructure.ObjectModel;

namespace Infrastructure.Utilities
{
    public class Timer : GameComponent
    {
        private float m_RemainingDelay;

        public float IntervalInSeconds { get; set; }

        public event Action Notify;

        public Timer(Game i_Game) : base(i_Game)
        {
            this.Enabled = false;
            m_RemainingDelay = 0.0f;
        }

        public void Activate()
        {
            this.Enabled = true;
        }

        public void Deactivate()
        {
            this.Enabled = false;
        }

        public bool IsActive()
        {
            return this.Enabled;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (Enabled)
            {
                m_RemainingDelay += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                if (m_RemainingDelay >= IntervalInSeconds)
                {
                    Notify?.Invoke();
                    m_RemainingDelay -= IntervalInSeconds;
                }

                base.Update(i_GameTime);
            }
        }
    }
}
