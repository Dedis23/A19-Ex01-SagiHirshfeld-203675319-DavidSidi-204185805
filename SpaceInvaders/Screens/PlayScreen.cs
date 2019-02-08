using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using System.Text;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Audio;
using Infrastructure;

namespace SpaceInvaders
{
    class PlayScreen : GameScreen
    {
        private const string k_ScoreFontAsset = @"Fonts\ComicSansMS";
        private const string k_LevelClearedSoundEffectAssetName = @"Audio\LevelWin";
        private const string k_GameOverSoundEffectAssetName = @"Audio\GameOver";

        private const float k_SpaceshipPositionYModifier = 1.5f;
        private const float k_LivesGapModifier = 0.7f;
        private const float k_GapBetweenRowsModifier = 1.2f;
        private const int k_LivesDistanceFromHorizontalScreenBound = 15;
        private const int k_UniqueLevelsCount = 6;
        private readonly Vector2 r_LeftDirectionVector = new Vector2(-1, 0);
        private readonly Vector2 r_RightDirectionVector = new Vector2(1, 0);

        private CollisionHandler m_CollisionHandler;
        private PauseScreen m_PauseScreen;
        private GameOverScreen m_GameOverScreen;
        private LevelTransitionScreen m_LevelTransitionScreen;

        private Spaceship m_Player1Spaceship;
        private Spaceship m_Player2Spaceship;
        private PlayerLivesRow m_Player1Lives;
        private PlayerLivesRow m_Player2Lives;
        private PlayerScoreText m_Player1ScoreText;
        private PlayerScoreText m_Player2ScoreText;
        private Mothership m_Mothership;
        private InvadersMatrix m_InvadersMatrix;
        private DancingBarriersRow m_DancingBarriersRow;

        private SoundEffectInstance m_LevelClearedSoundEffectInstance;
        private SoundEffectInstance m_GameOverSoundEffectInstance;

        private bool m_FirstLevelHasBeenTransitionedTo = false;
        private bool m_GameOver = false;
        private bool m_LevelCleared = false;
        private int m_CurrentLevel;

        private int CurrentLevel
        {
            get { return m_CurrentLevel; }
            set
            {
                m_CurrentLevel = value;
                m_LevelTransitionScreen.Text = string.Format("Level: {0}", m_CurrentLevel + 1);
            }
        }

        private int DifficultyLevel
        {
            get
            {
                return CurrentLevel % k_UniqueLevelsCount;
            }
        }

        public PlayScreen(Game i_Game) : base(i_Game)
        {            
            this.BlendState = BlendState.NonPremultiplied;

            m_CollisionHandler = new CollisionHandler(i_Game);
            m_CollisionHandler.EnemyCollidedWithSpaceship += () => m_GameOver = true;

            m_PauseScreen = new PauseScreen(i_Game);
            m_GameOverScreen = new GameOverScreen(i_Game);
            m_LevelTransitionScreen = new LevelTransitionScreen(i_Game);
            CurrentLevel = 0;

            loadSprites();
        }

        
        protected override void  OnActivated()
        {
            base.OnActivated();
            if (!m_FirstLevelHasBeenTransitionedTo)
            {
                ScreensManager.SetCurrentScreen(m_LevelTransitionScreen);
                m_FirstLevelHasBeenTransitionedTo = true;
            }
        }

        private void loadSprites()
        {
            m_Player1Spaceship = new Player1Spaceship(Game);
            m_Player1Spaceship.Died += onSpaceshipKilled;
            this.Add(m_Player1Spaceship);

            m_Player2Spaceship = new Player2Spaceship(Game);
            m_Player2Spaceship.Died += onSpaceshipKilled;
            this.Add(m_Player2Spaceship);

            m_Player1Lives = new PlayerLivesRow(m_Player1Spaceship);
            this.Add(m_Player1Lives);

            m_Player2Lives = new PlayerLivesRow(m_Player2Spaceship);
            this.Add(m_Player2Lives);

            m_Player1ScoreText = new PlayerScoreText(m_Player1Spaceship, k_ScoreFontAsset);
            this.Add(m_Player1ScoreText);

            m_Player2ScoreText = new PlayerScoreText(m_Player2Spaceship, k_ScoreFontAsset);
            this.Add(m_Player2ScoreText);

            m_Mothership = new Mothership(Game);
            this.Add(m_Mothership);

            m_InvadersMatrix = new InvadersMatrix(Game);
            m_InvadersMatrix.invadersMatrixReachedBottomScreen += () => m_GameOver = true;
            m_InvadersMatrix.AllInvadersWereDefeated += () => m_LevelCleared = true;

            this.Add(m_InvadersMatrix);

            m_DancingBarriersRow = new DancingBarriersRow(Game);
            this.Add(m_DancingBarriersRow);
        }

        public override void Initialize()
        {
            base.Initialize();
            initializeDrawablesPositions();
            m_InvadersMatrix.PopulateMatrix(DifficultyLevel);
            m_DancingBarriersRow.Dance(DifficultyLevel);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_LevelClearedSoundEffectInstance = Game.Content.Load<SoundEffect>(k_LevelClearedSoundEffectAssetName).CreateInstance();
            m_GameOverSoundEffectInstance = Game.Content.Load<SoundEffect>(k_GameOverSoundEffectAssetName).CreateInstance();
        }

        private void initializeDrawablesPositions()
        {
            // Spaceships
            Vector2 spaceshipsPos = new Vector2(0, GraphicsDevice.Viewport.Height - (m_Player1Spaceship.Height * k_SpaceshipPositionYModifier));
            m_Player1Spaceship.Position = m_Player1Spaceship.DefaultPosition = spaceshipsPos;
            m_Player2Spaceship.Position = m_Player2Spaceship.DefaultPosition = spaceshipsPos;

            // Lives
            Sprite lifeIcon = m_Player1Lives.First;
            m_Player1Lives.GapBetweenSprites = lifeIcon.Width * k_GapBetweenRowsModifier;
            m_Player2Lives.GapBetweenSprites = m_Player1Lives.GapBetweenSprites;
            m_Player1Lives.Position = new Vector2(GraphicsDevice.Viewport.Width - lifeIcon.Width - k_LivesDistanceFromHorizontalScreenBound, 0);
            m_Player2Lives.Position = m_Player1Lives.Position + new Vector2(0, lifeIcon.Height * k_GapBetweenRowsModifier);

            // ScoreTexts
            m_Player1ScoreText.Position = Vector2.Zero;
            m_Player2ScoreText.Position = new Vector2(0, m_Player1ScoreText.Height);

            // Mothership
            m_Mothership.Position = m_Mothership.DefaultPosition = new Vector2(-m_Mothership.Width, m_Mothership.Height);

            // InvadersMatrix
            m_InvadersMatrix.DefaultStartingPosition = new Vector2(0, Invader.k_DefaultInvaderHeight * 3);

            // Barriers
            Vector2 barriersPos = new Vector2(
                (GraphicsDevice.Viewport.Width - m_DancingBarriersRow.Width) / 2,
                m_Player1Spaceship.DefaultPosition.Y - (m_DancingBarriersRow.Height * 2));
            m_DancingBarriersRow.DefaultPosition = m_DancingBarriersRow.Position = barriersPos;
        }

        private void onSpaceshipKilled(object i_Spaceship)
        {
            if (!m_Player1Spaceship.IsAlive && !m_Player2Spaceship.IsAlive)
            {
                m_GameOver = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            takeInput();

            if (m_LevelCleared)
            {
                transitionToTheNextLevel();
            }

            else if (m_GameOver)
            {
                transitionToGameOverScreen();
            }
        }

        private void transitionToTheNextLevel()
        {
            m_LevelClearedSoundEffectInstance.PauseAndThenPlay();
            CurrentLevel++;
            ScreensManager.SetCurrentScreen(m_LevelTransitionScreen);
        }

        private void transitionToGameOverScreen()
        {
            m_GameOverSoundEffectInstance.PauseAndThenPlay();
            m_GameOverScreen.Text = buildGameOverMessage();
            this.ScreensManager.SetCurrentScreen(m_GameOverScreen);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            if (m_GameOver)
            {
                CurrentLevel = 0;
                m_FirstLevelHasBeenTransitionedTo = false;
                m_Player1Spaceship.ResetScoreAndLives();
                m_Player2Spaceship.ResetScoreAndLives();
            }

            if (m_GameOver || m_LevelCleared)
            {
                reset();
            }
        }

        private void reset()
        {
            m_GameOver = false;
            m_LevelCleared = false;
            clearAllBullets();
            m_InvadersMatrix.Clear();
            m_InvadersMatrix.PopulateMatrix(DifficultyLevel);
            m_DancingBarriersRow.Reset();
            m_DancingBarriersRow.Dance(DifficultyLevel);
            m_Mothership.HideAndWaitForNextSpawn();
            m_Player1Spaceship.PrepareForNewLevel();
            m_Player2Spaceship.PrepareForNewLevel();
        }
        
        private void clearAllBullets()
        {
            List<Bullet> bullets = new List<Bullet>();
            foreach (Sprite sprite in m_Sprites)
            {
                if (sprite is Bullet)
                {
                    bullets.Add(sprite as Bullet);
                }
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.Kill();
            }
        }

        private string buildGameOverMessage()
        {
            StringBuilder messageBuilder = new StringBuilder();

            string nameOfTheWinner = m_Player1Spaceship.Score >= m_Player2Spaceship.Score ? m_Player1Spaceship.Name : m_Player2Spaceship.Name;        
            messageBuilder.Append(string.Format("The winner is {0}!", nameOfTheWinner));
            messageBuilder.Append(Environment.NewLine);

            messageBuilder.Append(string.Format("{0} Score: {1}", m_Player1Spaceship.Name, m_Player1Spaceship.Score));
            messageBuilder.Append(Environment.NewLine);

            messageBuilder.Append(string.Format("{0} Score: {1}", m_Player2Spaceship.Name, m_Player2Spaceship.Score));
            messageBuilder.Append(Environment.NewLine);

            return messageBuilder.ToString();
        }

        private void takeInput()
        {
            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            /// This is here just to enable skipping to the game over screen - remove before submition
            else if (InputManager.KeyPressed(Keys.Delete))
            {
                m_GameOver = true;
            }

            /// Similarly, this is here to test level transitions
            else if (InputManager.KeyPressed(Keys.Insert))
            {
                m_LevelCleared = true;
            }

            else if (InputManager.KeyPressed(Keys.P))
            {
                this.ScreensManager.SetCurrentScreen(m_PauseScreen);
            }

            else
            {
                takePlayer1Input();
                takePlayer2Input();
            }
        }

        private void takePlayer1Input()
        {
            if (InputManager.KeyboardState.IsKeyDown(Keys.H))
            {
                m_Player1Spaceship.Move(r_LeftDirectionVector);
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.K))
            {
                m_Player1Spaceship.Move(r_RightDirectionVector);
            }

            if (InputManager.KeyPressed(Keys.U) || InputManager.ButtonPressed(eInputButtons.Left))
            {
                m_Player1Spaceship.Shoot();
            }

            m_Player1Spaceship.MoveAccordingToMousePositionDelta(InputManager.MousePositionDelta);
        }

        private void takePlayer2Input()
        {
            if (InputManager.KeyboardState.IsKeyDown(Keys.A))
            {
                m_Player2Spaceship.Move(r_LeftDirectionVector);
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.D))
            {
                m_Player2Spaceship.Move(r_RightDirectionVector);
            }

            if (InputManager.KeyPressed(Keys.W))
            {
                m_Player2Spaceship.Shoot();
            }
        }
    }
}