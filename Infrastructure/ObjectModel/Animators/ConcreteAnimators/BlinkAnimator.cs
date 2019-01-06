////*** Guy Ronen © 2008-2011 ***////
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class BlinkAnimator : SpriteAnimator
    {
        private readonly float r_BlinkTime;
        private float m_TimeLeftForNextBlink;

        public BlinkAnimator(string i_Name, float i_NumOfBlinksInSecond, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            // each blink is to go from Visable = true, to false to true again
            this.r_BlinkTime = 1.0f / i_NumOfBlinksInSecond / 2.0f;
            m_TimeLeftForNextBlink = r_BlinkTime;
        }

        public BlinkAnimator(float i_NumOfBlinksInSecond, TimeSpan i_AnimationLength)
            : this("BlinkAnimator", i_NumOfBlinksInSecond, i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForNextBlink += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimeLeftForNextBlink >= r_BlinkTime)
            {
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
