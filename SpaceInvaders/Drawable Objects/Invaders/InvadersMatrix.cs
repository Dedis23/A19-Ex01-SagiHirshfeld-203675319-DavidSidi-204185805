using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public class InvadersMatrix : RegisteredComponent
    {
        private const int k_NumOfRowsWithPinkInvaders = 1, k_NumOfRowsWithLightBlueInvaders = 2, k_NumOfRowsWithLightYellowInvaders = 2, k_NumOfInvadersInARow = 9;
        private const float k_DistanceBetweenEachInvader = 0.6f;
        private const float k_DefaultStartingPositionX = 0, k_DefaultStartingPositionY = 96;
        private const float k_JumpDistanceModifier = 0.5f;
        private const float k_InvadersReachedEdgeAccelerator = 0.92f;
        private const float k_FourInvadersDefeatedAccelerator = 0.96f;
        private const float k_ChanceForASingleInvaderToShoot = 5;
        private const float k_TimeBetweenRollingForShootsInSeconds = 2;
        private const float k_XGapBetweenInvaders = Invader.k_DefaultInvaderWidth + (Invader.k_DefaultInvaderWidth * k_DistanceBetweenEachInvader);
        private const float k_YGapBetweenInvaders = Invader.k_DefaultInvaderHeight + (Invader.k_DefaultInvaderHeight * k_DistanceBetweenEachInvader);
        private const float k_DefaultJumpDistance = k_JumpDistanceModifier * Invader.k_DefaultInvaderWidth;
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
            m_TimerForJumps.Interval = Invader.k_DefaultDelayBetweenJumpsInSeconds;
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
                r_InvadersMatrix.Add(createARowOfInvaders(currentRowPosition, eInvaderPresets.InvaderPink));
                currentRowPosition.Y += k_YGapBetweenInvaders;
            }

            for (int i = 0; i < k_NumOfRowsWithLightBlueInvaders; i++)
            {
                r_InvadersMatrix.Add(createARowOfInvaders(currentRowPosition, eInvaderPresets.InvaderLightBlue));
                currentRowPosition.Y += k_YGapBetweenInvaders;
            }

            for (int i = 0; i < k_NumOfRowsWithLightYellowInvaders; i++)
            {
                r_InvadersMatrix.Add(createARowOfInvaders(currentRowPosition, eInvaderPresets.InvaderLightYellow));
                currentRowPosition.Y += k_YGapBetweenInvaders;
            }
        }

        private enum eInvaderPresets
        {
            InvaderPink,
            InvaderLightBlue,
            InvaderLightYellow
        }

        private List<Invader> createARowOfInvaders(Vector2 i_PositionOfFirstInvaderInTheRow, eInvaderPresets i_InvaderPreset)
        {
            List<Invader> rowOfInvaders = new List<Invader>();
            Vector2 nextInvaderPosition = i_PositionOfFirstInvaderInTheRow;
            for (int i = 0; i < k_NumOfInvadersInARow; i++)
            {
                Invader currentInvader = createInvaderBasedOnPreset(i_InvaderPreset);
                currentInvader.Position = new Vector2(nextInvaderPosition.X, nextInvaderPosition.Y);
                nextInvaderPosition.X += k_XGapBetweenInvaders;
                currentInvader.SpriteKilled += removeInvader;
                rowOfInvaders.Add(currentInvader);
            }

            return rowOfInvaders;
        }

        private Invader createInvaderBasedOnPreset(eInvaderPresets i_InvaderPreset)
        {
            Invader newInvader = null;
            switch (i_InvaderPreset)
            {
                case eInvaderPresets.InvaderPink:
                    newInvader = new InvaderPink(this.Game, 0) as Invader;
                    break;
                case eInvaderPresets.InvaderLightBlue:
                    newInvader = new InvaderLightBlue(this.Game, 2) as Invader;
                    break;
                case eInvaderPresets.InvaderLightYellow:
                    newInvader = new InvaderLightYellow(this.Game, 4) as Invader;
                    break;
            }
            return newInvader;
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
            furthestInvaderXPosition = m_CurrentfurthestInvaderInXPosition.Position.X;
            float amountToJump = 0.0f;
            switch (m_JumpDirection)
            {
                case 1.0f:
                    amountToJump = Math.Min(k_DefaultJumpDistance, Game.GraphicsDevice.Viewport.Width - furthestInvaderXPosition - Invader.k_DefaultInvaderWidth);
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
            furthestInvaderXPosition = m_CurrentfurthestInvaderInXPosition.Position.X;
            switch (m_JumpDirection)
            {
                case 1.0f:
                    invadersOnEdge = furthestInvaderXPosition + Invader.k_DefaultInvaderWidth == Game.GraphicsDevice.Viewport.Width;
                    break;

                case -1.0f:
                    invadersOnEdge = furthestInvaderXPosition == 0;
                    break;
            }

            return invadersOnEdge;
        }

        private void doAJump(bool i_JumpSideways, float i_JumpAmount)
        {
            Vector2 delta = Vector2.Zero;

            if (i_JumpSideways == true)
            {
                delta.X = i_JumpAmount * m_JumpDirection;
            }
            else
            {
                delta.Y = i_JumpAmount;
            }

            foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
            {
                foreach (Invader invader in rowOfInvaders)
                {
                    invader.Position += delta;
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
                                if (furthestInvaderXPosition <= invader.Position.X)
                                {
                                    furthestInvaderXPosition = invader.Position.X;
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
                                if (furthestInvaderXPosition >= invader.Position.X)
                                {
                                    furthestInvaderXPosition = invader.Position.X;
                                    furthestInvaderXPositionToReturn = invader;
                                }
                            }
                        }
                    }

                    break;
            }

            return furthestInvaderXPositionToReturn;
        }

        private void checkIfInvadersMatrixReachedBottomScreen()
        {
            bool matrixReachedBottomScreen = false;
            foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
            {
                foreach (Invader invader in rowOfInvaders)
                {
                    if (invader.Position.Y + Invader.k_DefaultInvaderHeight >= Game.GraphicsDevice.Viewport.Height)
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
            foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
            {
                foreach (Invader invader in rowOfInvaders)
                {
                    invader.DelayBetweenJumpsInSeconds *= i_AcceleratorModifier;
                }
            }
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

        private List<Invader> m_KilledInvadersList = new List<Invader>();
        private void addToKilledInvadersList(object i_Invader)
        {
            m_KilledInvadersList.Add(i_Invader as Invader);
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