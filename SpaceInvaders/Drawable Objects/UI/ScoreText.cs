using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class ScoreText : TextSprite
    {
        private string m_PlayerName;

        public ScoreText(string i_PlayerName, Color i_ScoreColor, Game i_Game, string i_AssetName) : base(i_Game, i_AssetName)
        {
            m_PlayerName = i_PlayerName;
            this.TintColor = i_ScoreColor;
            this.Text = string.Format("{0} Score: {1}", i_PlayerName, 0);
        }

        public void UpdateNewScore(int i_NewScore)
        {
            this.Text = string.Format("{0} Score: {1}", m_PlayerName, i_NewScore);
        }
    }
}
