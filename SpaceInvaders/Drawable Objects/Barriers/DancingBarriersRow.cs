using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class DancingBarriersRow : SpriteRow<Barrier>
    {
        private const int k_BaseDancingSpeed = 45;
        private const float k_DancingSpeedIncreaseModifier = 0.93f;
        private const int k_DefaultBarrierNum = 4;
        private float m_DancingSpeed;

        public DancingBarriersRow(Game i_Game, int i_BarrierNum) : base(i_Game, i_BarrierNum, Game => new Barrier(i_Game))
        {
            this.InsertionOrder = Order.LeftToRight;
            this.BlendState = BlendState.NonPremultiplied;
        }

        public DancingBarriersRow(Game i_Game) : this(i_Game, k_DefaultBarrierNum)
        {
        }       

        protected override void LoadContent()
        {
            base.LoadContent();
            this.GapBetweenSprites = this.First.Width;
        }

        public Vector2 DefaultPosition { get; set; }

        public void Dance(int i_DifficultyLevel)
        {
            bool v_Loop = true;
            m_DancingSpeed = matchDancingSpeedToDifficultyLevel(i_DifficultyLevel);

            foreach (Barrier sprite in this.SpritesLinkedList)
            {
                SpriteAnimator danceAnimation = new WaypointsAnymator(
                        m_DancingSpeed,
                        TimeSpan.Zero,
                        v_Loop,
                        sprite.Position + new Vector2(sprite.Width, 0),
                        sprite.Position - new Vector2(sprite.Width, 0));

                sprite.Animations.Remove(danceAnimation.Name);
                sprite.Animations.Add(danceAnimation);
                sprite.Animations.Resume();
            }
        }

        private float matchDancingSpeedToDifficultyLevel(int i_DifficultyLevel)
        {
            float newDancingSpeed;

            switch(i_DifficultyLevel)
            {
                case 0:
                    newDancingSpeed = 0;
                    break;
                case 1:
                    newDancingSpeed = k_BaseDancingSpeed;
                    break;
                default:
                    newDancingSpeed = m_DancingSpeed * k_DancingSpeedIncreaseModifier;
                    break;
            }

            return newDancingSpeed;
        }

        public void Reset()
        {
            foreach (Barrier barrier in r_SpritesLinkedList)
            {
                barrier.Reset();
            }

            this.Position = this.DefaultPosition;
        }
    }
}
