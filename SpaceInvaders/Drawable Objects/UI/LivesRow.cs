using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class LivesRow : SpriteRow
    {
        private int m_VisibleSpritesCount;
        private LinkedListNode<Sprite> m_LastVisibleSpriteNode;

        public LivesRow(Game i_Game, int i_LivesNum, string i_IconAssetName )
            : base(i_Game, i_LivesNum, i_IconAssetName)
        {
            BlendState = BlendState.NonPremultiplied;
            this.Opacity /= 2;
            this.Scales /= 2;
            InsertionOrder = Order.RightToLeft;
            RemovalOrder = Order.LeftToRight;

            m_VisibleSpritesCount = i_LivesNum;
            m_LastVisibleSpriteNode = r_SpritesLinkedList.Last;
        }

        public void UpdateLivesCount(int i_NewLivesCount)
        {
            if (i_NewLivesCount < m_VisibleSpritesCount)
            {
                for (int i = 0; i < m_VisibleSpritesCount - i_NewLivesCount; i++)
                {
                    m_LastVisibleSpriteNode.Value.Visible = false;
                    m_LastVisibleSpriteNode = m_LastVisibleSpriteNode.Previous;
                }
            }
            else if (i_NewLivesCount > m_VisibleSpritesCount)
            {
                for (int i = 0; i < i_NewLivesCount - m_VisibleSpritesCount; i++)
                {
                    if (m_LastVisibleSpriteNode == null)
                    {
                        m_LastVisibleSpriteNode = r_SpritesLinkedList.First;
                    }
                    else
                    {
                        m_LastVisibleSpriteNode = m_LastVisibleSpriteNode.Next;
                    }

                    m_LastVisibleSpriteNode.Value.Visible = true;
                }
            }

            m_VisibleSpritesCount = i_NewLivesCount;
        }
    }
}
