using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using System;

namespace SpaceInvaders
{
    public class GameState : GameService
    {
        private const int k_UniqueLevelsCount = 6;
        private int m_Player1Score = 0;
        private int m_Player2Score = 0;

        public event Action<int> Player1ScoreChanged;
        public event Action<int> Player2ScoreChanged;

        public GameState(Game i_Game) : base(i_Game)
        {
        }

        public bool IsMultiplayer { get; set; } = false;

        public int LevelNumber { get; set; } = 0;

        public int DifficultyLevel
        {
            get
            {
                return LevelNumber % k_UniqueLevelsCount;
            }
        }

        public string Player1Name { get; set; } = "P1";

        public string Player2Name { get; set; } = "P2";

        public int Player1Score
        {
            get
            {
                return m_Player1Score;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (m_Player1Score != value)
                {
                    m_Player1Score = value;
                    Player1ScoreChanged?.Invoke(value);
                }
            }
        }

        public int Player2Score
        {
            get
            {
                return m_Player2Score;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (m_Player2Score != value)
                {
                    m_Player2Score = value;
                    Player2ScoreChanged?.Invoke(value);
                }
            }
        }

        public void ResetLevelAndScore()
        {
            LevelNumber = Player1Score = Player2Score = 0;
        }
    }
}
