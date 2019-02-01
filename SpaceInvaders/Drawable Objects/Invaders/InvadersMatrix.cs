using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.Utilities;

namespace SpaceInvaders
{
    public class InvadersMatrix : CompositeDrawableComponent<GameComponent>
    {
        private const int k_NumOfRowsWithPinkInvaders = 1, k_NumOfRowsWithLightBlueInvaders = 2, k_NumOfRowsWithLightYellowInvaders = 2, k_NumOfInvadersInARow = 9;
        private const int k_NumOfRows = k_NumOfRowsWithPinkInvaders + k_NumOfRowsWithLightBlueInvaders + k_NumOfRowsWithLightYellowInvaders;
        private const int k_StartingInvadersCount = k_NumOfRows * k_NumOfInvadersInARow;
        private const float k_DistanceBetweenEachInvader = 0.6f;
        private const float k_DefaultStartingPositionX = 0, k_DefaultStartingPositionY = 96;
        private const float k_JumpDistanceModifier = 0.5f;
        private const float k_InvadersReachedEdgeAccelerator = 0.92f;
        private const float k_FourInvadersDefeatedAccelerator = 0.96f;
        private const float k_ChanceToShootIncrementModifierOnInvaderDeath = 1.05f;
        private const float k_XGapBetweenInvaders = Invader.k_DefaultInvaderWidth + (Invader.k_DefaultInvaderWidth * k_DistanceBetweenEachInvader);
        private const float k_YGapBetweenInvaders = Invader.k_DefaultInvaderHeight + (Invader.k_DefaultInvaderHeight * k_DistanceBetweenEachInvader);
        private const float k_DefaultJumpDistance = k_JumpDistanceModifier * Invader.k_DefaultInvaderWidth;
        private const float k_DefaultDelayBetweenJumpsInSeconds = 0.5f;
        private readonly List<List<Invader>> r_InvadersMatrix;
        private Invader m_CurrentfurthestInvaderInXPosition;
        private Timer m_TimerForJumps;
        private float m_JumpDirection;
        private int m_NumOfDefeatedInvaders;

        public event Action invadersMatrixReachedBottomScreen;

        public event Action AllInvadersWereDefeated;

        public InvadersMatrix(Game i_Game) : base(i_Game)
        {
            r_InvadersMatrix = new List<List<Invader>>();
            m_JumpDirection = 1.0f;
            m_NumOfDefeatedInvaders = 0;
            initializeTimers();
            initializeMatrix();
        }

        private void initializeTimers()
        {
            m_TimerForJumps = new Timer(this.Game);
            m_TimerForJumps.Interval = k_DefaultDelayBetweenJumpsInSeconds;
            m_TimerForJumps.Notify += handleInvadersMatrixJumps;
            m_TimerForJumps.Activate();
        }

        private void initializeMatrix()
        {
            Vector2 currentRowPosition = new Vector2(k_DefaultStartingPositionX, k_DefaultStartingPositionY);
            for (int i = 0; i < k_NumOfRowsWithPinkInvaders; i++)
            {
                r_InvadersMatrix.Add(
                    createARowOfInvaders(
                        currentRowPosition,
                    eInvaderPresets.InvaderPink,
                    i % Invader.k_NumOfCells));
                currentRowPosition.Y += k_YGapBetweenInvaders;
            }

            for (int i = 0; i < k_NumOfRowsWithLightBlueInvaders; i++)
            {
                r_InvadersMatrix.Add
                    (createARowOfInvaders(
                        currentRowPosition,
                    eInvaderPresets.InvaderLightBlue,
                    i % Invader.k_NumOfCells));
                currentRowPosition.Y += k_YGapBetweenInvaders;
            }

            for (int i = 0; i < k_NumOfRowsWithLightYellowInvaders; i++)
            {
                r_InvadersMatrix.Add(createARowOfInvaders(
                    currentRowPosition,
                    eInvaderPresets.InvaderLightYellow,
                    i % Invader.k_NumOfCells));
                currentRowPosition.Y += k_YGapBetweenInvaders;
            }
        }

        private enum eInvaderPresets
        {
            InvaderPink,
            InvaderLightBlue,
            InvaderLightYellow
        }

        private List<Invader> createARowOfInvaders(Vector2 i_PositionOfFirstInvaderInTheRow, eInvaderPresets i_InvaderPreset, int i_StartingCell)
        {
            List<Invader> rowOfInvaders = new List<Invader>();
            Vector2 nextInvaderPosition = i_PositionOfFirstInvaderInTheRow;
            for (int i = 0; i < k_NumOfInvadersInARow; i++)
            {
                Invader currentInvader = createInvaderBasedOnPreset(i_InvaderPreset, i_StartingCell);
                currentInvader.Position = new Vector2(nextInvaderPosition.X, nextInvaderPosition.Y);
                nextInvaderPosition.X += k_XGapBetweenInvaders;
                currentInvader.Dying += OnInvaderDying;
                currentInvader.Died += OnInvaderKilled;
                rowOfInvaders.Add(currentInvader);
            }

            return rowOfInvaders;
        }

        private Invader createInvaderBasedOnPreset(eInvaderPresets i_InvaderPreset, int i_StartingCell)
        {
            Invader newInvader = null;
            switch (i_InvaderPreset)
            {
                case eInvaderPresets.InvaderPink:
                    newInvader = new InvaderPink(this.Game, i_StartingCell) as Invader;
                    break;
                case eInvaderPresets.InvaderLightBlue:
                    newInvader = new InvaderLightBlue(this.Game, i_StartingCell) as Invader;
                    break;
                case eInvaderPresets.InvaderLightYellow:
                    newInvader = new InvaderLightYellow(this.Game, i_StartingCell) as Invader;
                    break;
            }

            this.Add(newInvader);
            return newInvader;
        }

        private void handleInvadersMatrixJumps()
        {
            const bool v_JumpSideways = true;

            if (invadersOnEdge() == false)
            {
                float amountToJump = calculateJumpDistance();
                doAJump(v_JumpSideways, amountToJump);
            }
            else
            {
                doAJump(!v_JumpSideways, k_DefaultJumpDistance);
                checkIfInvadersMatrixReachedBottomScreen();
                m_TimerForJumps.Interval *= k_InvadersReachedEdgeAccelerator;
                flipCurrentSideJumpDirection();
            }

            foreach (List<Invader> invadersList in r_InvadersMatrix)
            {
                foreach (Invader invader in invadersList)
                {
                    invader.GoNextFrame();
                }
            }
        }

        private void flipCurrentSideJumpDirection()
        {
            m_JumpDirection *= -1.0f;
            m_CurrentfurthestInvaderInXPosition = null;
        }

        private float calculateJumpDistance()
        {
            float amountToJump = 0.0f;

            if (m_CurrentfurthestInvaderInXPosition == null)
            {
                // check only when needed
                m_CurrentfurthestInvaderInXPosition = getFurthestInvaderXPosition();
            }

            if (m_CurrentfurthestInvaderInXPosition != null)
            {
                float furthestInvaderXPosition = m_CurrentfurthestInvaderInXPosition.Position.X;
                switch (m_JumpDirection)
                {
                    case 1.0f:
                        amountToJump = Math.Min(k_DefaultJumpDistance, Game.GraphicsDevice.Viewport.Width - furthestInvaderXPosition - Invader.k_DefaultInvaderWidth);
                        break;

                    case -1.0f:
                        amountToJump = Math.Min(k_DefaultJumpDistance, furthestInvaderXPosition);
                        break;
                }
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

            if (m_CurrentfurthestInvaderInXPosition != null)
            {
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
            }

            return invadersOnEdge;
        }

        private void doAJump(bool i_JumpSideways, float i_JumpAmount)
        {
            Vector2 delta = Vector2.Zero;

            if (i_JumpSideways)
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
            float furthestInvaderXPosition = 0;
            Predicate<Invader> invaderIsFurthest = (invader) => furthestInvaderXPosition <= invader.Position.X;
            if (m_JumpDirection == -1.0f)
            {
                furthestInvaderXPosition = Game.GraphicsDevice.Viewport.Width;
                invaderIsFurthest = (invader) => furthestInvaderXPosition >= invader.Position.X;
            }

            foreach (List<Invader> rowOfInvaders in r_InvadersMatrix)
            {
                foreach (Invader invader in rowOfInvaders)
                {
                    if (rowOfInvaders.Count > 0)
                    {
                        if (invaderIsFurthest(invader))
                        {
                            furthestInvaderXPosition = invader.Position.X;
                            furthestInvaderXPositionToReturn = invader;
                        }
                    }
                }
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

        private void OnInvaderDying(object i_Invader)
        {
            Invader invaderToRemove = i_Invader as Invader;
            if (invaderToRemove == m_CurrentfurthestInvaderInXPosition)
            {
                m_CurrentfurthestInvaderInXPosition = null;
            }

            foreach (List<Invader> invaderList in r_InvadersMatrix)
            {
                invaderList.Remove(invaderToRemove);

                // While we're here - increase the shooting chance of the enemies to make things more interesting
                foreach (Invader invader in invaderList)
                {
                    invader.ChanceToShoot *= k_ChanceToShootIncrementModifierOnInvaderDeath;
                }
            }

            m_NumOfDefeatedInvaders++;

            // Check if 4 invaders were defeated
            if (m_NumOfDefeatedInvaders % 4 == 0)
            {
                m_TimerForJumps.Interval *= k_FourInvadersDefeatedAccelerator;
            }

            if (m_NumOfDefeatedInvaders == k_StartingInvadersCount)
            {
                AllInvadersWereDefeated?.Invoke();
            }
        }

        private void OnInvaderKilled(object i_Invader)
        {
            Invader invader = i_Invader as Invader;
            invader.Dying -= OnInvaderDying;
            invader.Died -= OnInvaderKilled;
            this.Remove(invader);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            m_TimerForJumps.Notify -= handleInvadersMatrixJumps;
        }
    }
}