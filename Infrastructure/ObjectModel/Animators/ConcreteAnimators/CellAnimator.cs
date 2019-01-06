////*** Guy Ronen © 2008-2011 ***////
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class CellAnimator : SpriteAnimator
    {
        private readonly int r_NumOfCells = 1;
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrCellIdx;
        private int m_StartCell;

        public Action FinishedCellAnimationCycle;

        public CellAnimator(string i_Name, TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength, int i_StartCell)
            : base(i_Name, i_AnimationLength)
        {
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            this.r_NumOfCells = i_NumOfCells;
            m_StartCell = m_CurrCellIdx = i_StartCell;

            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        public CellAnimator(TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength, int i_StartCell)
            : this("CellAnimator", i_CellTime, i_NumOfCells, i_AnimationLength, i_StartCell)
        {
        }

        public TimeSpan CellTime
        {
            get { return m_CellTime; }
            set { m_CellTime = value; }
        }

        private void goToNextFrame()
        {
            m_CurrCellIdx++;
            if (m_CurrCellIdx >= r_NumOfCells)
            {
                if (m_Loop)
                {
                    m_CurrCellIdx = 0;
                }
                else
                {
                    m_CurrCellIdx = 0;
                    this.IsFinished = true;
                }

                FinishedCellAnimationCycle?.Invoke();
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
                m_CurrCellIdx * this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Top,
                this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Height);
        }
    }
}
