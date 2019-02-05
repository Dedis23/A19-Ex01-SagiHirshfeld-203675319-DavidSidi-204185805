using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.Utilities;

namespace SpaceInvaders
{
    public class InvadersMatrix : CompositeDrawableComponent<Invader>
    {
        private const int k_NumOfRowsWithPinkInvaders = 1, k_NumOfRowsWithLightBlueInvaders = 2, k_NumOfRowsWithLightYellowInvaders = 2;
        private const int k_BaseColumnCount = 9;
        private const int k_NumOfRows = k_NumOfRowsWithPinkInvaders + k_NumOfRowsWithLightBlueInvaders + k_NumOfRowsWithLightYellowInvaders;
        private const float k_DistanceBetweenEachInvader = 0.6f;
        private const float k_JumpDistanceModifier = 0.5f;
        private const float k_InvadersReachedEdgeAccelerator = 0.92f;
        private const float k_FourInvadersDefeatedAccelerator = 0.96f;
        private const float k_BaseChanceToShootPerInvader = 5;
        private const float k_ChanceToShootMultiplierOnInvaderDeath = 1.05f;
        private const float k_ChanceToShootLevelMultiplier = 2;
        private const int k_ScoreBonusPerDifficultyLevel = 120;
        private const float k_XGapBetweenInvaders = Invader.k_DefaultInvaderWidth + (Invader.k_DefaultInvaderWidth * k_DistanceBetweenEachInvader);
        private const float k_YGapBetweenInvaders = Invader.k_DefaultInvaderHeight + (Invader.k_DefaultInvaderHeight * k_DistanceBetweenEachInvader);
        private const float k_DefaultJumpDistance = k_JumpDistanceModifier * Invader.k_DefaultInvaderWidth;
        private const float k_DefaultDelayBetweenJumpsInSeconds = 0.5f;

        private Invader m_CurrentfurthestInvaderInXPosition;
        private Timer m_TimerForJumps;
        private float m_JumpDirection;
        private int m_NumOfDefeatedInvaders;

        public event Action invadersMatrixReachedBottomScreen;

        public event Action AllInvadersWereDefeated;

        public Vector2 DefaultStartingPosition { get; set; }

        public InvadersMatrix(Game i_Game) : base(i_Game)
        {
            m_JumpDirection = 1.0f;
            initializeJumpsTimer();
        }

        private void initializeJumpsTimer()
        {
            m_TimerForJumps = new Timer(this.Game);
            m_TimerForJumps.IntervalInSeconds = k_DefaultDelayBetweenJumpsInSeconds;
            m_TimerForJumps.Notify += handleInvadersMatrixJumps;
            m_TimerForJumps.Activate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /// Look for a better way
            m_TimerForJumps.Update(gameTime);
        }

        public void PopulateMatrix(int i_DifficultyLevel)
        {
            Vector2 nextInvaderPosition = DefaultStartingPosition;
            Invader currentInvader;

            int columnCount = k_BaseColumnCount + i_DifficultyLevel;

            for (int row = 0; row < k_NumOfRows; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    currentInvader = createAndAddNewInvader(row);
                    currentInvader.ChanceToShoot = k_BaseChanceToShootPerInvader + i_DifficultyLevel * k_ChanceToShootLevelMultiplier;
                    currentInvader.PointsValue += i_DifficultyLevel * k_ScoreBonusPerDifficultyLevel;
                    currentInvader.Position = nextInvaderPosition;
                    nextInvaderPosition.X += k_XGapBetweenInvaders;
                }

                nextInvaderPosition.X = DefaultStartingPosition.X;
                nextInvaderPosition.Y += k_YGapBetweenInvaders;
            }
        }

        private Invader createAndAddNewInvader(int i_RowInMatrix)
        {
            Invader newInvader = null;
            int startingCell = i_RowInMatrix % Invader.k_NumOfCells;
            if (i_RowInMatrix < k_NumOfRowsWithPinkInvaders)
            {
                newInvader = new InvaderPink(this.Game, startingCell);
            }

            else if (i_RowInMatrix < k_NumOfRowsWithPinkInvaders + k_NumOfRowsWithLightBlueInvaders)
            {
                newInvader = new InvaderLightBlue(this.Game, startingCell);
            }

            else
            {
                newInvader = new InvaderLightYellow(this.Game, startingCell);
            }

            newInvader.Dying += OnInvaderDying;
            newInvader.Died += OnInvaderDied;
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
                m_TimerForJumps.IntervalInSeconds *= k_InvadersReachedEdgeAccelerator;
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

            foreach(Invader invader in this)
            {
                if (invader.Vulnerable)
                {
                    invader.Position += delta;
                    invader.GoNextFrame();
                }
            }
        }

        private Invader getFurthestInvaderXPosition()
        {
            Invader furthestInvader = null;
            float furthestX;
            Predicate<Invader> invaderIsFurthestPredicate;

            if (m_JumpDirection == -1.0f)
            {
                furthestX = Game.GraphicsDevice.Viewport.Width;
                invaderIsFurthestPredicate = (invader) => furthestX >= invader.Position.X;
            }
            else
            {
                furthestX = 0;
                invaderIsFurthestPredicate = (invader) => furthestX <= invader.Position.X;
            }

            foreach (Invader invader in this)
            {
                if (invader.Vulnerable && invaderIsFurthestPredicate(invader))
                {
                    furthestX = invader.Position.X;
                    furthestInvader = invader;
                }
            }

            return furthestInvader;
        }

        private void checkIfInvadersMatrixReachedBottomScreen()
        {
            bool matrixReachedBottomScreen = false;

            foreach (Invader invader in this)
            {
                if (invader.Position.Y + Invader.k_DefaultInvaderHeight >= Game.GraphicsDevice.Viewport.Height)
                {
                    matrixReachedBottomScreen = true;
                }
            }

            if (matrixReachedBottomScreen == true)
            {
                invadersMatrixReachedBottomScreen?.Invoke();
            }
        }

        private void OnInvaderDying(object i_Invader)
        {
            if (i_Invader as Invader == m_CurrentfurthestInvaderInXPosition)
            {
                m_CurrentfurthestInvaderInXPosition = null;
            }

            foreach (Invader invader in this)
            {
                invader.ChanceToShoot *= k_ChanceToShootMultiplierOnInvaderDeath;
            }

            m_NumOfDefeatedInvaders++;

            // Check if 4 invaders were defeated
            if (m_NumOfDefeatedInvaders % 4 == 0)
            {
                m_TimerForJumps.IntervalInSeconds *= k_FourInvadersDefeatedAccelerator;
            }
        }

        private void OnInvaderDied(object i_Invader)
        {
            this.Remove(i_Invader as Invader);
        }

        // Applies To dead invaders AND on all invaders after this.Clear()
        protected override void OnComponentRemoved(GameComponentEventArgs<Invader> e)
        {
            base.OnComponentRemoved(e);
            Invader invader = e.GameComponent;
            invader.Dying -= OnInvaderDying;
            invader.Died -= OnInvaderDied;
            invader.Animations.Pause();
            invader.Dispose();

            if (this.Count == 0)
            {
                AllInvadersWereDefeated?.Invoke();
            }
        }

        public override void Clear()
        {
            m_TimerForJumps.IntervalInSeconds = k_DefaultDelayBetweenJumpsInSeconds;
            m_NumOfDefeatedInvaders = 0;
            m_JumpDirection = 1.0f;
            m_CurrentfurthestInvaderInXPosition = null;

            base.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            m_TimerForJumps.Notify -= handleInvadersMatrixJumps;
        }
    }
}