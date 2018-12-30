using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvadersMatrix : GameComponent
    {
        private const int k_NumOfRowsWithPinkInvaders = 1, k_NumOfRowsWithLightBlueInvaders = 2, k_NumOfRowsWithLightYellowInvaders = 2, k_NumOfInvadersInARow = 9;
        private const float k_DistanceBetweenEachInvader = 0.6f;
        private const float k_DefaultStartingPositionX = 0, k_DefaultStartingPositionY = 96;
        private const int k_DefaultInvaderWidth = 32, k_DefaultInvaderHeight = 32;
        private const float k_DefaultDelayBetweenJumpsInSeconds = 0.5f;
        private const float k_JumpDistanceModifier = 0.5f;
        private const float k_InvadersReachedEdgeAccelerator = 0.92f;
        private const float k_FourInvadersDefeatedAccelerator = 0.96f;
        private const float k_ChanceForASingleInvaderToShoot = 5;
        private const float k_TimeBetweenRollingForShootsInSeconds = 2;
        private const float k_XGapBetweenInvaders = k_DefaultInvaderWidth + (k_DefaultInvaderWidth * k_DistanceBetweenEachInvader);
        private const float k_YGapBetweenInvaders = k_DefaultInvaderHeight + (k_DefaultInvaderHeight * k_DistanceBetweenEachInvader);
        private const float k_DefaultJumpDistance = k_JumpDistanceModifier * k_DefaultInvaderWidth;
        private readonly Random r_RandomGenerator;
        private readonly List<List<Invader>> r_InvadersMatrix;
        private Invader m_CurrentfurthestInvaderInXPosition;
        private Timer m_TimerForJumps;
        private Timer m_TimerForInvaderShooting;
        private float m_JumpDirection;
        private int m_NumOfDefeatedInvaders;

        public event Action invadersMatrixReachedBottomScreen;

        public event Action allInvadersWereDefeated;

        public InvadersMatrix(Game i_Game) : base(i_Game)
        {
            r_InvadersMatrix = new List<List<Invader>>();
            m_JumpDirection = 1.0f;
            m_NumOfDefeatedInvaders = 0;
            r_RandomGenerator = new Random((int)DateTime.Now.Ticks);
            initializeTimers();
            initializeMatrix();
        }

        private void initializeTimers()
        {
            m_TimerForJumps = new Timer(this.Game);
            m_TimerForJumps.Interval = k_DefaultDelayBetweenJumpsInSeconds;
            m_TimerForJumps.Notify += handleInvadersMatrixJumps;
            m_TimerForJumps.Activate();
            m_TimerForInvaderShooting = new Timer(this.Game);
            m_TimerForInvaderShooting.Interval = k_TimeBetweenRollingForShootsInSeconds;
            m_TimerForInvaderShooting.Notify += rollsForShoot;
            m_TimerForInvaderShooting.Activate();
        }

        private void initializeMatrix()
        {
            Vector2 currentRowPosition = new Vector2(k_DefaultStartingPositionX, k_DefaultStartingPositionY);
            for (int i = 0; i < k_NumOfRowsWithPinkInvaders; i++)
            {
                r_InvadersMatrix.Add(createARowOfInvaders(currentRowPosition, DrawableObjectsFactory.eSpriteType.InvaderPink));
                currentRowPosition.Y += k_YGapBetweenInvaders;
            }

            for (int i = 0; i < k_NumOfRowsWithLightBlueInvaders; i++)
            {
                r_InvadersMatrix.Add(createARowOfInvaders(currentRowPosition, DrawableObjectsFactory.eSpriteType.InvaderLightBlue));
                currentRowPosition.Y += k_YGapBetweenInvaders;
            }

            for (int i = 0; i < k_NumOfRowsWithLightYellowInvaders; i++)
            {
                r_InvadersMatrix.Add(createARowOfInvaders(currentRowPosition, DrawableObjectsFactory.eSpriteType.InvaderLightYellow));
                currentRowPosition.Y += k_YGapBetweenInvaders;
            }
        }

        private List<Invader> createARowOfInvaders(Vector2 i_PositionOfFirstInvaderInTheRow, DrawableObjectsFactory.eSpriteType i_InvaderSpriteType)
        {
            List<Invader> rowOfInvaders = new List<Invader>();
            Vector2 nextInvaderPosition = i_PositionOfFirstInvaderInTheRow;
            Invader currentInvader;
            for (int i = 0; i < k_NumOfInvadersInARow; i++)
            {
                currentInvader = DrawableObjectsFactory.Create(Game, i_InvaderSpriteType) as Invader;
                currentInvader.PositionX = nextInvaderPosition.X;
                currentInvader.PositionY = nextInvaderPosition.Y;
                nextInvaderPosition.X += k_XGapBetweenInvaders;
                currentInvader.Killed += removeInvader;
                rowOfInvaders.Add(currentInvader);
                Game.Components.Add(currentInvader);
            }

            return rowOfInvaders;
        }

        private void handleInvadersMatrixJumps()
        {
            if (invadersOnEdge() == false)
            {
                float amountToJump = calculateJumpDistance();
                doAJump(true, amountToJump);
            }
            else
            {
                doAJump(false, k_DefaultJumpDistance);
                checkIfInvadersMatrixReachedBottomScreen();
                decreaseDelayBetweenJumps(k_InvadersReachedEdgeAccelerator);
                flipCurrentSideJumpDirection();
            }
        }

        private void flipCurrentSideJumpDirection()
        {
            m_JumpDirection *= -1.0f;
            m_CurrentfurthestInvaderInXPosition = null;
        }

        private float calculateJumpDistance()
        {
            float furthestInvaderXPosition = 0.0f;
            if (m_CurrentfurthestInvaderInXPosition == null)
            {
                // check only when needed
                m_CurrentfurthestInvaderInXPosition = getFurthestInvaderXPosition();
            }
            furthestInvaderXPosition = m_CurrentfurthestInvaderInXPosition.PositionX;
            float amountToJump = 0.0f;
            switch (m_JumpDirection)
            {
                case 1.0f:
                    amountToJump = Math.Min(k_DefaultJumpDistance, Game.GraphicsDevice.Viewport.Width - furthestInvaderXPosition - k_DefaultInvaderWidth);
                    break;

                case -1.0f:
                    amountToJump = Math.Min(k_DefaultJumpDistance, furthestInvaderXPosition);
                    break;
            }

            return amountToJump;
        }

        private bool invadersOnEdge()
        {
            bool invadersOnEdge = false;
            float furthestInvaderXPosition = 0.0f;
            if (m_CurrentfurthestInvaderInXPosition == null)
            {
                // check only when needed
                m_CurrentfurthestInvaderInXPosition = getFurthestInvaderXPosition();
            }
            furthestInvaderXPosition = m_CurrentfurthestInvaderInXPosition.PositionX;
            switch (m_JumpDirection)
            {
                case 1.0f:
                    invadersOnEdge = furthestInvaderXPosition + k_DefaultInvaderWidth == Game.GraphicsDevice.Viewport.Width;
                    break;

                case -1.0f:
                    invadersOnEdge = furthestInvaderXPosition == 0;
                    break;
            }

            return invadersOnEdge;
        }

        private void doAJump(bool i_JumpSideways, float i_JumpAmount)
        {
            float xDelta = 0.0f;
            float yDelta = 0.0f;

            if (i_JumpSideways == true)
            {
                xDelta = i_JumpAmount * m_JumpDirection;
            }
            else
            {
                yDelta = i_JumpAmount;
            }

            foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
            {
                foreach (Invader invader in rowOfInvaders)
                {
                    invader.PositionX += xDelta;
                    invader.PositionY += yDelta;
                }
            }
        }

        private Invader getFurthestInvaderXPosition()
        {
            Invader furthestInvaderXPositionToReturn = null;
            switch (m_JumpDirection)
            {
                case 1.0f:
                    float furthestInvaderXPosition = 0.0f;
                    foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
                    {
                        foreach (Invader invader in rowOfInvaders)
                        {
                            if (rowOfInvaders.Count > 0)
                            {
                                if (furthestInvaderXPosition < invader.PositionX)
                                {
                                    furthestInvaderXPosition = invader.PositionX;
                                    furthestInvaderXPositionToReturn = invader;
                                }
                            }
                        }
                    }

                    break;

                case -1.0f:
                    furthestInvaderXPosition = (float)this.Game.GraphicsDevice.Viewport.Width;
                    foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
                    {
                        foreach (Invader invader in rowOfInvaders)
                        {
                            if (rowOfInvaders.Count > 0)
                            {
                                if (furthestInvaderXPosition >= invader.PositionX)
                                {
                                    furthestInvaderXPosition = invader.PositionX;
                                    furthestInvaderXPositionToReturn = invader;
                                }
                            }
                        }
                    }

                    break;
            }

            return furthestInvaderXPositionToReturn;
        }

        // DEBUG
        public string DEBUG()
        {
            return m_CurrentfurthestInvaderInXPosition?.PositionX.ToString();
        }
        // DEBUG
        
        private void checkIfInvadersMatrixReachedBottomScreen()
        {
            bool matrixReachedBottomScreen = false;
            foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
            {
                foreach (Invader invader in rowOfInvaders)
                {
                    if (invader.PositionY + k_DefaultInvaderHeight >= Game.GraphicsDevice.Viewport.Height)
                    {
                        matrixReachedBottomScreen = true;
                    }
                }
            }

            if (matrixReachedBottomScreen == true)
            {
                invadersMatrixReachedBottomScreen?.Invoke();
            }
        }

        private void decreaseDelayBetweenJumps(float i_AcceleratorModifier)
        {
            m_TimerForJumps.Interval *= i_AcceleratorModifier;
        }

        private void rollsForShoot()
        {
            foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
            {
                foreach (Invader invader in rowOfInvaders)
                {
                    if (r_RandomGenerator.Next(1, 100) <= k_ChanceForASingleInvaderToShoot)
                    {
                        invader.Shoot();
                    }
                }
            }
        }

        private void removeInvader(object i_Invader)
        {
            Invader invader = i_Invader as Invader;
            if (invader == m_CurrentfurthestInvaderInXPosition)
            {
                m_CurrentfurthestInvaderInXPosition = null;
            }
            foreach (List<Invader> invaderList in r_InvadersMatrix)
            {
                invaderList.Remove(invader);
            }

            m_NumOfDefeatedInvaders++;

            // Check if 4 invaders were defeated
            if (m_NumOfDefeatedInvaders % 4 == 0)
            {
                decreaseDelayBetweenJumps(k_FourInvadersDefeatedAccelerator);
            }

            if (m_NumOfDefeatedInvaders == (k_NumOfRowsWithPinkInvaders +
                                            k_NumOfRowsWithLightBlueInvaders +
                                            k_NumOfRowsWithLightYellowInvaders) * k_NumOfInvadersInARow)
            {
                allInvadersWereDefeated?.Invoke();
            }
        }
    }
}