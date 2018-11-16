using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Timer : GameComponent
    {
        private float m_RemainingDelay;
        private bool m_Active;

        public float Interval { get; set; }

        public event Action Notify;

        public Timer(Game i_Game) : base(i_Game)
        {
            m_Active = false;
            m_RemainingDelay = 0.0f;
        }

        public void Activate()
        {
            m_Active = true;
            Game.Components.Add(this);
        }

        public void DeActivate()
        {
            m_Active = false;
            Game.Components.Remove(this);
        }

        public bool IsActive()
        {
            return m_Active;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_RemainingDelay += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_RemainingDelay >= Interval)
            {
                Notify?.Invoke();
                m_RemainingDelay = 0.0f;
            }

            base.Update(i_GameTime);
        }
    }
}
