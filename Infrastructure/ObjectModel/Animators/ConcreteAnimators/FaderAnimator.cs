using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class FaderAnimator : SpriteAnimator
    {
        private float m_FadeVelocity;
        private float m_CurrentOpacity;

        public FaderAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
        }

        public FaderAnimator(TimeSpan i_AnimationLength)
            : this("FaderAnimator", i_AnimationLength)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_CurrentOpacity = BoundSprite.Opacity;
            m_FadeVelocity = m_CurrentOpacity / (float)AnimationLength.TotalSeconds;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_CurrentOpacity -= m_FadeVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            BoundSprite.Opacity = m_CurrentOpacity;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Opacity = this.m_OriginalSpriteInfo.Opacity;
            m_CurrentOpacity = this.m_OriginalSpriteInfo.Opacity;
        }
    }
}