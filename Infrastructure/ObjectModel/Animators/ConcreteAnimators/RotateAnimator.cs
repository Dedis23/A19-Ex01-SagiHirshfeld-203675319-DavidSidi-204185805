﻿using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class RotateAnimator : SpriteAnimator
    {
        private float m_RotationVelocity;

        public RotateAnimator(string i_Name, float i_NumOfCyclesPerSecond, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_RotationVelocity = i_NumOfCyclesPerSecond * MathHelper.TwoPi;
        }

        public RotateAnimator(float i_NumOfCyclesPerSecond, TimeSpan i_AnimationLength)
            : this("RotateAnimator", i_NumOfCyclesPerSecond, i_AnimationLength)
        { }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.Rotation += m_RotationVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Rotation = this.m_OriginalSpriteInfo.Rotation;
        }
    }
}