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
        private readonly Random r_RandomGenerator;
        private readonly List<List<Invader>> r_InvadersMatrix;
        private Timer m_TimerForJumps;
        private Timer m_TimerForInvaderShooting;
        private float m_JumpDirection;
        private float m_JumpDistance;
        private int m_NumOfDefeatedInvaders;
        private bool m_LastJumpWasDownwards;

        public event Action invadersMatrixReachedBottomScreen;

        public event Action allInvadersWereDefeated;

        public InvadersMatrix(Game i_Game) : base(i_Game)
        {
            r_InvadersMatrix = new List<List<Invader>>();
            m_JumpDirection = 1;
            m_NumOfDefeatedInvaders = 0;
            m_LastJumpWasDownwards = false;
            m_JumpDistance = k_JumpDistanceModifier * k_DefaultInvaderWidth;
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
                currentRowPosition.Y += k_DefaultInvaderHeight + (k_DefaultInvaderHeight * k_DistanceBetweenEachInvader);
            }

            for (int i = 0; i < k_NumOfRowsWithLightBlueInvaders; i++)
            {
                r_InvadersMatrix.Add(createARowOfInvaders(currentRowPosition, DrawableObjectsFactory.eSpriteType.InvaderLightBlue));
                currentRowPosition.Y += k_DefaultInvaderHeight + (k_DefaultInvaderHeight * k_DistanceBetweenEachInvader);
            }

            for (int i = 0; i < k_NumOfRowsWithLightYellowInvaders; i++)
            {
                r_InvadersMatrix.Add(createARowOfInvaders(currentRowPosition, DrawableObjectsFactory.eSpriteType.InvaderLightYellow));
                currentRowPosition.Y += k_DefaultInvaderHeight + (k_DefaultInvaderHeight * k_DistanceBetweenEachInvader);
            }
        }

        private List<Invader> createARowOfInvaders(Vector2 i_PositionOfFirstInvaderInTheRow, DrawableObjectsFactory.eSpriteType i_InvaderSpriteType)
        {
            List<Invader> rowOfInvaders = new List<Invader>();
            Vector2 nextInvaderPosition = i_PositionOfFirstInvaderInTheRow;
            Invader currentInvader = null;
            for (int i = 0; i < k_NumOfInvadersInARow; i++)
            {
                currentInvader = DrawableObjectsFactory.Create(Game, i_InvaderSpriteType) as Invader;
                currentInvader.PositionX = nextInvaderPosition.X;
                currentInvader.PositionY = nextInvaderPosition.Y;
                nextInvaderPosition.X += k_DefaultInvaderWidth + (k_DefaultInvaderWidth * k_DistanceBetweenEachInvader);
                currentInvader.Killed += removeInvader;
                rowOfInvaders.Add(currentInvader);
                Game.Components.Add(currentInvader);
            }

            return rowOfInvaders;
        }

        private void handleInvadersMatrixJumps()
        {
            if (invadersOnEdge() == true)
            {
                makeAJumpDownwards();
                m_LastJumpWasDownwards = true;
                checkIfInvadersMatrixReachedBottomScreen();
                decreaseDelayBetweenJumps(k_InvadersReachedEdgeAccelerator);
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
            float furthestInvaderXPosition = getFurthestInvaderXPosition();
            float amountToJump = 0.0f;
            if (m_JumpDirection == 1.0f)
            {
                amountToJump = Math.Min(m_JumpDistance, Game.GraphicsDevice.Viewport.Width - furthestInvaderXPosition - k_DefaultInvaderWidth);
            }
            else
            {
                amountToJump = Math.Min(m_JumpDistance, furthestInvaderXPosition);
            }

            return amountToJump;
        }

        private bool invadersOnEdge()
        {
            bool invadersOnEdge = false;
            float furthestInvaderXPosition = getFurthestInvaderXPosition();
            if (m_JumpDirection == 1.0f)
            {
                invadersOnEdge = furthestInvaderXPosition + k_DefaultInvaderWidth == Game.GraphicsDevice.Viewport.Width ?
                    invadersOnEdge = true : invadersOnEdge = false;
            }
            else
            {
                invadersOnEdge = furthestInvaderXPosition == 0 ? invadersOnEdge = true : invadersOnEdge = false;
            }

            return invadersOnEdge;
        }

        private void jumpSideways(float i_JumpAmount)
        {
            foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
            {
                foreach (Invader invader in rowOfInvaders)
                {
                    invader.PositionX += i_JumpAmount * m_JumpDirection;
                }
            }
        }

        private float getFurthestInvaderXPosition()
        {
            float furthestInvaderXPosition = 0.0f;
            if (m_JumpDirection == 1)
            {
                foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
                {
                    foreach (Invader invader in rowOfInvaders)
                    {
                        if (rowOfInvaders.Count > 0)
                        {
                            furthestInvaderXPosition = Math.Max(furthestInvaderXPosition, invader.PositionX);
                        }
                    }
                }
            }
            else
            {
                furthestInvaderXPosition = (float)this.Game.GraphicsDevice.Viewport.Width;
                foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
                {
                    foreach (Invader invader in rowOfInvaders)
                    {
                        if (rowOfInvaders.Count > 0)
                        {
                            furthestInvaderXPosition = Math.Min(furthestInvaderXPosition, invader.PositionX);
                        }
                    }
                }
            }

            return furthestInvaderXPosition;
        }

        private void makeAJumpDownwards()
        {
            foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
            {
                foreach (Invader invader in rowOfInvaders)
                {
                    invader.PositionY += m_JumpDistance;
                }
            }
        }
        
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