﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class EnemiesMatrix : GameComponent
    {
        private const int k_NumOfRowsWithPinkEnemies = 1, k_NumOfRowsWithLightBlueEnemies = 2, k_NumOfRowsWithLightYellowEnemies = 2, k_NumOfEnemiesInARow = 9;
        private const float k_DistanceBetweenEachEnemy = 0.6f;
        private const float k_DefaultStartingPositionX = 0, k_DefaultStartingPositionY = 96;
        private const int k_DefaultEnemyWidth = 32, k_DefaultEnemyHeight = 32;
        private const float k_DefaultDelayBetweenJumpsInSeconds = 0.5f;
        private const float k_JumpDistanceModifier = 0.5f;
        private const float k_EnemiesReachedEdgeAccelerator = 0.92f;
        private const float k_FourEnemiesDefeatedAccelerator = 0.96f;
        private const float k_ChanceForASingleEnemyToShoot = 5;
        private const float k_TimeBetweenRollingForShootsInSeconds = 2;
        private float m_JumpDirection;
        private float m_JumpDistance;
        private int m_NumOfDefeatedEnemies;
        private bool m_LastJumpWasDownwards;
        private Timer m_TimerForJumps;
        private Timer m_TimerForEnemyShooting;
        private Random m_RandomGenerator;
        private readonly List<List<Enemy>> r_EnemiesMatrix;
        public event Action enemiesMatrixReachedBottomScreen;

        public EnemiesMatrix(Game i_Game) : base(i_Game)
        {
            r_EnemiesMatrix = new List<List<Enemy>>();
            m_JumpDirection = 1;
            m_NumOfDefeatedEnemies = 0;
            m_LastJumpWasDownwards = false;
            m_JumpDistance = k_JumpDistanceModifier * k_DefaultEnemyWidth;
            m_RandomGenerator = new Random();
            initializeTimers();
            initializeMatrix();
        }

        private void initializeTimers()
        {
            m_TimerForJumps = new Timer(this.Game);
            m_TimerForJumps.Interval = k_DefaultDelayBetweenJumpsInSeconds;
            m_TimerForJumps.Notify += handleEnemiesMatrixJumps;
            m_TimerForJumps.Activate();
            m_TimerForEnemyShooting = new Timer(this.Game);
            m_TimerForEnemyShooting.Interval = k_TimeBetweenRollingForShootsInSeconds;
            m_TimerForEnemyShooting.Notify += rollsForShoot;
            m_TimerForEnemyShooting.Activate();
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
                currentEnemy = DrawableObjectsFactory.Create(Game, i_EnemySpriteType) as Enemy;
                currentEnemy.m_Position = currentEnemyPosition;
                currentEnemyPosition.X += k_DefaultEnemyWidth + (k_DefaultEnemyWidth * k_DistanceBetweenEachEnemy);
                currentEnemy.Killed += removeEnemy;
                rowOfEnemies.Add(currentEnemy);
                Game.Components.Add(currentEnemy);
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

        private void handleEnemiesMatrixJumps()
        {
            if (enemiesOnEdge() == true)
            {
                makeAJumpDownwards();
                m_LastJumpWasDownwards = true;
                checkIfEnemiesMatrixReachedBottomScreen();
                decreaseDelayBetweenJumps(k_EnemiesReachedEdgeAccelerator);
                m_JumpDirection *= -1;
            }
            else
            {
                if (m_LastJumpWasDownwards == false)
                {
                    float amountToJump = calculateJumpAmount();
                    jumpSideways(amountToJump);
                }
            }
            m_LastJumpWasDownwards = false;
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
                enemiesOnEdge = furthestEnemyXPosition + k_DefaultEnemyWidth == Game.GraphicsDevice.Viewport.Width ?
                    enemiesOnEdge = true : enemiesOnEdge = false;
            }
            else
            {
                enemiesOnEdge = furthestEnemyXPosition == 0 ? enemiesOnEdge = true : enemiesOnEdge = false;
            }
            return enemiesOnEdge;
        }

        private void jumpSideways(float i_JumpAmount)
        {
            foreach (List<Enemy> rowOfEnemies in r_EnemiesMatrix)
            {
                foreach (Enemy enemy in rowOfEnemies)
                {
                    enemy.m_Position.X += i_JumpAmount * m_JumpDirection;
                }
            }
        }

        private float getFurthestEnemyXPosition()
        {
            float furthestEnemyXPosition = 0.0f;
            if (m_JumpDirection == 1)
            {
                foreach (List<Enemy> rowOfEnemies in r_EnemiesMatrix)
                {
                    foreach (Enemy enemy in rowOfEnemies)
                    {
                        if (rowOfEnemies.Count > 0)
                        {
                            furthestEnemyXPosition = Math.Max(furthestEnemyXPosition, enemy.m_Position.X);
                        }
                    }
                }
            }
            else
            {
                furthestEnemyXPosition = (float)this.Game.GraphicsDevice.Viewport.Width;
                foreach (List<Enemy> rowOfEnemies in r_EnemiesMatrix)
                {
                    foreach (Enemy enemy in rowOfEnemies)
                    {
                        if (rowOfEnemies.Count > 0)
                        {
                            furthestEnemyXPosition = Math.Min(furthestEnemyXPosition, enemy.m_Position.X);
                        }
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
                    enemy.m_Position.Y += m_JumpDistance;
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
                    if (enemy.m_Position.Y + k_DefaultEnemyHeight >= Game.GraphicsDevice.Viewport.Height)
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
            m_TimerForJumps.Interval *= i_AcceleratorModifier;
        }

        private void rollsForShoot()
        {
            foreach (List<Enemy> rowOfEnemies in r_EnemiesMatrix)
            {
                foreach (Enemy enemy in rowOfEnemies)
                {
                    if (m_RandomGenerator.Next(1, 100) <= k_ChanceForASingleEnemyToShoot)
                    {
                        enemy.Shoot();
                    }
                }
            }
        }

        // Sagi: maybe temp
        private void removeEnemy(object i_Enemy)
        {
            Enemy enemy = i_Enemy as Enemy;
            foreach(List<Enemy> enemyList in r_EnemiesMatrix)
            {
                enemyList.Remove(enemy);
            }
        }
    }
}