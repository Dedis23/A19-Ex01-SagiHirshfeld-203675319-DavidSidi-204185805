using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;

namespace Infrastructure.ObjectModel
{
    public class AnimatedTextSprite : TextSprite
    {
        public AnimatedTextSprite(Game i_Game, string i_AssetName, float i_TargetScale = 1.05f, float i_PulsePerSecond = 0.9f)
            : base(i_Game, i_AssetName)
        {
            initializeAnimations(i_TargetScale, i_PulsePerSecond);
        }

        private void initializeAnimations(float i_TargetScale, float i_PulsePerSecond)
        {
            Animations.Add(new PulseAnimator("PulseAnimator", TimeSpan.Zero, i_TargetScale, i_PulsePerSecond));
            Animations.Enabled = false;
        }

        public override void Initialize()
        {
            base.LoadContent();
        }

        public void StartAnimation()
        {
            Animations.Resume();
        }

        public void StopAnimation()
        {
            Animations.Reset();
            Animations.Pause();
        }
    }
}
