using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class FaderAnimator : SpriteAnimator
    {
        private float m_FadeVelocity;

        public FaderAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_FadeVelocity = 1.0f / (float)i_AnimationLength.TotalSeconds;
        }

        public FaderAnimator(TimeSpan i_AnimationLength)
            : this("FaderAnimator", i_AnimationLength)
        {}

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.Opacity -= m_FadeVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (BoundSprite.Opacity == 0)
            {
                this.IsFinished = true;
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Opacity = this.m_OriginalSpriteInfo.Opacity;
        }
    }
}