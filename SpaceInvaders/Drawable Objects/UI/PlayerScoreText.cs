using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    public class PlayerScoreText : TextSprite
    {
        private IPlayer m_Player;

        public PlayerScoreText(IPlayer i_Player, string i_AssetName) : base(i_AssetName, i_Player.Game)
        {
            m_Player = i_Player;
            this.TintColor = m_Player.ScoreColor;
            this.Text = string.Format("{0} Score: {1}", m_Player.Name, m_Player.Score);
            m_Player.ScoreChanged += onPlayerScoreChange;
            m_Player.Disposed += onPlayerDispose;
        }

        private void onPlayerScoreChange(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} Score: {1}", m_Player.Name, m_Player.Score);
        }

        private void onPlayerDispose(object sender, EventArgs e)
        {
            m_Player.ScoreChanged -= onPlayerScoreChange;
            m_Player.Disposed -= onPlayerDispose;
        }
    }
}
