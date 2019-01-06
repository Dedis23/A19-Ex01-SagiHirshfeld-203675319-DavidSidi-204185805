using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class ShrinkAnimator : SpriteAnimator
    {
        private float m_ShrinkVelocity;

        public ShrinkAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_ShrinkVelocity = 1.0f / (float)i_AnimationLength.TotalSeconds;
        }

        public ShrinkAnimator(TimeSpan i_AnimationLength)
            : this("ShrinkAnimator", i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.Scales -= new Vector2(m_ShrinkVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            if (BoundSprite.Scales.X < 0 && BoundSprite.Scales.Y < 0)
            {
                this.IsFinished = true;
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = this.m_OriginalSpriteInfo.Scales;
        }
    }
}
