using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public class ScorePrinter : RegisteredComponent
    {
        private const string k_FontAssetName = @"Fonts\ComicSansMS";
        private SpriteFont m_Font;
        private readonly IEnumerable<IPlayer> r_Players;

        public ScorePrinter(Game i_Game, IEnumerable<IPlayer> i_Players) : base(i_Game)
        {
            r_Players = i_Players;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Font = Game.Content.Load<SpriteFont>(k_FontAssetName);
        }

        internal void DrawScore(SpriteBatch i_SpriteBatch)
        {
            if(i_SpriteBatch != null && m_Font != null)
            {
                Vector2 drawPosition = Vector2.Zero;
                foreach(IPlayer player in r_Players)
                {
                    i_SpriteBatch.DrawString
                        (m_Font,
                        String.Format("{0} Score: {1}", player.Name, player.Score),
                        drawPosition,
                        player.ScoreColor);

                    drawPosition.Y += m_Font.LineSpacing;
                }
            }
        }

        internal void ShowGameOverWindow()
        {
            String gameOverMessage = buildGameOverMessage();
            System.Windows.Forms.MessageBox.Show(gameOverMessage, "Game Over!", System.Windows.Forms.MessageBoxButtons.OK);
        }

        private string buildGameOverMessage()
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(String.Format("GG! The winner is {0}!", getTheNameOfTheWinner()));
            messageBuilder.Append(Environment.NewLine);

            foreach(IPlayer player in r_Players)
            {
                messageBuilder.Append(String.Format("{0} Score: {1}", player.Name, player.Score));
                messageBuilder.Append(Environment.NewLine);
            }

            return messageBuilder.ToString();
        }

        private String getTheNameOfTheWinner()
        {
            int maxScore = 0;
            String winnerName = "";

            foreach(IPlayer player in r_Players)
            {
                if(player.Score >= maxScore)
                {
                    maxScore = player.Score;
                    winnerName = player.Name;
                }
            }

            return winnerName;
        }
    }
}
