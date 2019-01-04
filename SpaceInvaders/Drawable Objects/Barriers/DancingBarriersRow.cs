using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    public class DancingBarriersRow : DrawableGameComponent
    {
        private const int k_DancingSpeed = 45;
        private const int k_DefaultBarrierNum = 4;
        private List<Barrier> m_BarriersList;

        public DancingBarriersRow(Game i_Game, int i_BarrierNum) : base(i_Game)
        {
            m_BarriersList = new List<Barrier>();
            if (i_BarrierNum <= 0)
            {
                i_BarrierNum = 1;
            }

            for (int i = 0; i < i_BarrierNum; i++)
            {
                m_BarriersList.Add(new Barrier(i_Game));
            }

            i_Game.Components.Add(this);
        }

        public DancingBarriersRow(Game i_Game) : this(i_Game, k_DefaultBarrierNum)
        { }
            

        protected override void LoadContent()
        {
            base.LoadContent();
            this.Position = Vector2.Zero;
        }

        public Vector2 Position
        {
            get
            {
                return m_BarriersList[0].Position;
            }

            set
            {
                m_BarriersList[0].Position = value;
                for (int i = 1; i < m_BarriersList.Count; i++)
                {
                    m_BarriersList[i].Position = new Vector2(m_BarriersList[i - 1].Bounds.Right + m_BarriersList[i - 1].Width, m_BarriersList[i - 1].Position.Y);
                }

                dance();
            }
        }

        private void dance()
        {            
            bool v_Loop = true;
            foreach (Barrier barrier in m_BarriersList)
            {                
                SpriteAnimator danceAnimation = new WaypointsAnymator(
                        k_DancingSpeed,
                        TimeSpan.Zero,
                        v_Loop,
                        barrier.Position + new Vector2(barrier.Width, 0),
                        barrier.Position - new Vector2(barrier.Width, 0));
                
                barrier.Animations.Remove(danceAnimation.Name);
                barrier.Animations.Add(danceAnimation);
                barrier.Animations.Resume();
            }
        }

        public float Width
        {
            get
            {
                return m_BarriersList[m_BarriersList.Count - 1].Bounds.Right - m_BarriersList[0].Bounds.Left;
            }
        }

        public float Height
        {
            get
            {
                return m_BarriersList[0].Height;
            }
        }
    }
}
