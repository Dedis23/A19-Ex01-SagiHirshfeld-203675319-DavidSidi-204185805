using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class PlayerLivesRow : SpriteRow
    {
        private IPlayer m_Player;
        private int m_VisibleSpritesCount;
        private LinkedListNode<Sprite> m_LastVisibleSpriteNode;

        public PlayerLivesRow(IPlayer i_Player)
            : base(i_Player.Game, i_Player.Lives, i_Player.AssetName)
        {
            BlendState = BlendState.NonPremultiplied;
            this.Opacity /= 2;
            this.Scales /= 2;
            InsertionOrder = Order.RightToLeft;
            RemovalOrder = Order.LeftToRight;

            m_Player = i_Player;
            m_Player.LivesCountChanged += onPlayerLivesCountChanged;
            m_Player.Disposed += onPlayerDispose;

            m_VisibleSpritesCount = m_Player.Lives;
            m_LastVisibleSpriteNode = r_SpritesLinkedList.Last;
        }

        private void onPlayerLivesCountChanged(object sender, EventArgs e)
        {
            if (m_Player.Lives < m_VisibleSpritesCount)
            {
                for (int i = 0; i < m_VisibleSpritesCount - m_Player.Lives; i++)
                {
                    m_LastVisibleSpriteNode.Value.Visible = false;
                    m_LastVisibleSpriteNode = m_LastVisibleSpriteNode.Previous;
                }
            }

            else if (m_Player.Lives > m_VisibleSpritesCount)
            {
                for (int i = 0; i < m_Player.Lives - m_VisibleSpritesCount; i++)
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

            m_VisibleSpritesCount = m_Player.Lives;
        }

        private void onPlayerDispose(object sender, EventArgs e)
        {
            m_Player.LivesCountChanged -= onPlayerLivesCountChanged;
            m_Player.Disposed -= onPlayerDispose;
        }
    }
}
