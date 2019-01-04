//*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class BlinkAnimator : SpriteAnimator
    {
        private readonly TimeSpan r_BlinkTime;
        private TimeSpan m_TimeLeftForNextBlink;

        public BlinkAnimator(string i_Name, TimeSpan i_BlinkTime, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.r_BlinkTime = i_BlinkTime;
            this.m_TimeLeftForNextBlink = i_BlinkTime;
        }

        public BlinkAnimator(TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
            : this("BlinkAnimator", i_BlinkLength, i_AnimationLength)
        {}

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForNextBlink += i_GameTime.ElapsedGameTime;
            if (m_TimeLeftForNextBlink.TotalSeconds >= r_BlinkTime.TotalSeconds)
            {
                // we have elapsed, so blink
                this.BoundSprite.Visible = !this.BoundSprite.Visible;
                m_TimeLeftForNextBlink -= r_BlinkTime;
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
