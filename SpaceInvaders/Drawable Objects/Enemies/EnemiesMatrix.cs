using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class EnemiesMatrix : GameComponent
    {
        private const int k_NumOfRowsWithPinkEnemies = 1, k_NumOfRowsWithLightBlueEnemies = 2, k_NumOfRowsWithLightYellowEnemies = 2, k_NumOfEnemiesInARow = 9;
        private const float k_DistanceBetweenEachEnemy = 0.6f;
        private const float k_DefaultStartingPositionX = 0, k_DefaultStartingPositionY = 96;
        private const float k_JumpDistanceModifier = 0.5f;
        private const int k_DefaultEnemyWidth = 32, k_DefaultEnemyHeight = 32;
        private const float k_DefaultDelayBetweenJumpsInSeconds = 0.5f;
        private const float k_EnemiesReachedEdgeAccelerator = 0.92f;
        private const float k_FourEnemiesDefeatedAccelerator = 0.96f;
        private float m_TimerToNextJump;
        private float m_CurrentDelayBetweenJumps;
        private float m_JumpDirection;
        private float m_JumpDistance;
        private int m_NumOfDefeatedEnemies;
        private bool m_JumpedDownwards;
        private readonly List<List<Enemy>> r_EnemiesMatrix;
        public event Action enemiesMatrixReachedBottomScreen;

        public EnemiesMatrix(Game i_Game) : base(i_Game)
        {
            r_EnemiesMatrix = new List<List<Enemy>>();
            m_CurrentDelayBetweenJumps = k_DefaultDelayBetweenJumpsInSeconds;
            m_TimerToNextJump = 0.0f;
            m_JumpDirection = 1;
            m_NumOfDefeatedEnemies = 0;
            m_JumpedDownwards = false;
            m_JumpDistance = k_JumpDistanceModifier * k_DefaultEnemyWidth;
            initializeMatrix();
        }

        private void initializeMatrix()
        {
            Vector2 currentRowPosition = new Vector2(k_DefaultStartingPositionX, k_DefaultStartingPositionY);
            for (int i = 0; i < k_NumOfRowsWithPinkEnemies; i++)
            {
                r_EnemiesMatrix.Add(createARowOfEnemies(currentRowPosition, DrawableObjectsFactory.eSpriteType.EnemyPink));
                currentRowPosition.Y = currentRowPosition.Y + k_DefaultEnemyHeight + (k_DefaultEnemyHeight * k_DistanceBetweenEachEnemy);
            }
            for (int i = 0; i < k_NumOfRowsWithLightBlueEnemies; i++)
            {
                r_EnemiesMatrix.Add(createARowOfEnemies(currentRowPosition, DrawableObjectsFactory.eSpriteType.EnemyLightBlue));
                currentRowPosition.Y = currentRowPosition.Y + k_DefaultEnemyHeight + (k_DefaultEnemyHeight * k_DistanceBetweenEachEnemy);
            }
            for (int i = 0; i < k_NumOfRowsWithLightYellowEnemies; i++)
            {
                r_EnemiesMatrix.Add(createARowOfEnemies(currentRowPosition, DrawableObjectsFactory.eSpriteType.EnemyLightYellow));
                currentRowPosition.Y = currentRowPosition.Y + k_DefaultEnemyHeight + (k_DefaultEnemyHeight * k_DistanceBetweenEachEnemy);
            }
        }

        private List<Enemy> createARowOfEnemies(Vector2 i_PositionOfFirstEnemyInTheRow, DrawableObjectsFactory.eSpriteType i_EnemySpriteType)
        {
            List<Enemy> rowOfEnemies = new List<Enemy>();
            Vector2 currentEnemyPosition = i_PositionOfFirstEnemyInTheRow;
            Enemy currentEnemy = null;
            for (int i = 0; i < k_NumOfEnemiesInARow; i++)
            {
                try
                {
                    currentEnemy = DrawableObjectsFactory.Create(Game, i_EnemySpriteType) as Enemy;
                }
                catch (Exception)
                {
                    throw new ArgumentException("Incorrect Enemy Sprite Type");
                }
                currentEnemy.Position = currentEnemyPosition;
                currentEnemyPosition.X += k_DefaultEnemyWidth + (k_DefaultEnemyWidth * k_DistanceBetweenEachEnemy);
                Game.Components.Add(currentEnemy);
                rowOfEnemies.Add(currentEnemy);
            }
            return rowOfEnemies;
        }

        private void killEnemy(int i_Row, int i_Col)
        {
            Game.Components.Remove(r_EnemiesMatrix[i_Row][i_Col]);
            r_EnemiesMatrix[i_Row].Remove(r_EnemiesMatrix[i_Row][i_Col]);
            m_NumOfDefeatedEnemies++;
            // Check if 4 enemies were defeated
            if (m_NumOfDefeatedEnemies % 4 == 0)
            {
                decreaseDelayBetweenJumps(k_FourEnemiesDefeatedAccelerator);
            }
        }

        // DEBUG
        public float Debug { get; set; }
        public int Debug2 { get; set; }
        public override void Update(GameTime i_GameTime)
        {
            Debug = m_CurrentDelayBetweenJumps;
            Debug2++;
            if (Debug2 == 599)
            {
                killEnemy(0, 8);
                killEnemy(1, 8);
                killEnemy(2, 8);
                killEnemy(3, 8);
            }
            if (Debug2 == 2399)
            {
                killEnemy(0, 0);
                killEnemy(1, 0);
                killEnemy(2, 0);
                killEnemy(3, 0);
                killEnemy(4, 0);
            }
            if (Debug2 == 3599)
            {
                killEnemy(4, 0);
                killEnemy(4, 0);
                killEnemy(4, 0);
                killEnemy(4, 0);
                killEnemy(4, 0);
                killEnemy(4, 0);
                killEnemy(4, 0);
                killEnemy(4, 0);
            }
            handleEnemiesMatrixJumps(i_GameTime);
            base.Update(i_GameTime);
        }

        private void handleEnemiesMatrixJumps(GameTime i_GameTime)
        {
            m_TimerToNextJump += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimerToNextJump >= m_CurrentDelayBetweenJumps)
            {
                if (enemiesOnEdge() == true)
                {
                    makeAJumpDownwards();
                    m_JumpedDownwards = true;
                    checkIfEnemiesMatrixReachedBottomScreen();
                    decreaseDelayBetweenJumps(k_EnemiesReachedEdgeAccelerator);
                    m_JumpDirection *= -1;
                }
                else
                {
                    if (m_JumpedDownwards == false)
                    {
                        float amountToJump = calculateJumpAmount();
                        jumpSideways(amountToJump);
                    }
                }
                m_TimerToNextJump = 0.0f;
            }
            m_JumpedDownwards = false;
        }

        private float calculateJumpAmount()
        {
            float furthestEnemyXPosition = getFurthestEnemyXPosition();
            float amountToJump = 0.0f;
            if (m_JumpDirection == 1.0f)
            {
                amountToJump = Math.Min(m_JumpDistance, Game.GraphicsDevice.Viewport.Width - furthestEnemyXPosition - k_DefaultEnemyWidth);
            }
            else
            {
                amountToJump = Math.Min(m_JumpDistance, furthestEnemyXPosition);
            }
            return amountToJump;
        }

        private bool enemiesOnEdge()
        {
            bool enemiesOnEdge = false;
            float furthestEnemyXPosition = getFurthestEnemyXPosition();
            if (m_JumpDirection == 1.0f)
            {
                if (furthestEnemyXPosition + k_DefaultEnemyWidth == Game.GraphicsDevice.Viewport.Width)
                {
                    enemiesOnEdge = true;
                }
            }
            else
            {
                if (furthestEnemyXPosition == 0)
                {
                    enemiesOnEdge = true;
                }
            }
            return enemiesOnEdge;
        }

        private void jumpSideways(float i_JumpAmount)
        {
            foreach (List<Enemy> rowOfEnemies in r_EnemiesMatrix)
            {
                foreach (Enemy enemy in rowOfEnemies)
                {
                    enemy.Position.X += i_JumpAmount * m_JumpDirection;
                }
            }
        }

        private float getFurthestEnemyXPosition()
        {
            float furthestEnemyXPosition = 0.0f;
            Enemy enemyInTheEdge = null;
            if (m_JumpDirection == 1)
            {
                foreach (List<Enemy> rowOfEnemies in r_EnemiesMatrix)
                {
                    if (rowOfEnemies.Count > 0)
                    {
                        enemyInTheEdge = rowOfEnemies[rowOfEnemies.Count - 1];
                        furthestEnemyXPosition = Math.Max(furthestEnemyXPosition, enemyInTheEdge.Position.X);
                    }
                }
            }
            else
            {
                foreach (List<Enemy> rowOfEnemies in r_EnemiesMatrix)
                {
                    if (rowOfEnemies.Count > 0)
                    {
                        enemyInTheEdge = rowOfEnemies[0];
                        furthestEnemyXPosition = Math.Max(furthestEnemyXPosition, enemyInTheEdge.Position.X);
                    }
                }
            }
            return furthestEnemyXPosition;
        }

        private void makeAJumpDownwards()
        {
            foreach (List<Enemy> rowOfEnemies in r_EnemiesMatrix)
            {
                foreach (Enemy enemy in rowOfEnemies)
                {
                    enemy.Position.Y += m_JumpDistance;
                }
            }
        }

        private void checkIfEnemiesMatrixReachedBottomScreen()
        {
            bool matrixReachedBottomScreen = false;
            foreach (List<Enemy> rowOfEnemies in r_EnemiesMatrix)
            {
                foreach (Enemy enemy in rowOfEnemies)
                {
                    if (enemy.Position.Y + k_DefaultEnemyHeight >= Game.GraphicsDevice.Viewport.Height)
                    {
                        matrixReachedBottomScreen = true;
                    }
                }
            }
            if (matrixReachedBottomScreen == true)
            {
                enemiesMatrixReachedBottomScreen?.Invoke();
            }
        }

        private void decreaseDelayBetweenJumps(float i_AcceleratorModifier)
        {
            m_CurrentDelayBetweenJumps *= i_AcceleratorModifier;
        }
    }
}