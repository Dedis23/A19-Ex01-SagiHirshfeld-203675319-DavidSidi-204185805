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

        public PlayerLivesRow(IPlayer i_Player)
            : base(i_Player.Game, i_Player.Lives, i_Player.AssetName)
        {
            BlendState = BlendState.NonPremultiplied;
            this.Opacity /= 2;
            this.Scales /= 2;
            InsertionOrder = Order.RightToLeft;
            RemovalOrder = Order.LeftToRight;

            m_Player = i_Player;
            m_Player.LifeLost += onPlayerLifeLost;
            m_Player.Disposed += onPlayerDispose;
        }

        private void onPlayerLifeLost(object sender, EventArgs e)
        {
            RemoveSprite();
        }

        private void onPlayerDispose(object sender, EventArgs e)
        {
            m_Player.LifeLost -= onPlayerLifeLost;
            m_Player.Disposed -= onPlayerDispose;
        }
    }
}
