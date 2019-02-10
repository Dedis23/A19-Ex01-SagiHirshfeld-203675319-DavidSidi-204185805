using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class PlayScreen : GameScreen
    {
        private const string k_ScoreFontAsset = @"Fonts\ComicSansMS";
        private const float k_SpaceshipPositionYModifier = 1.5f;
        private const float k_LivesGapModifier = 0.7f;
        private const float k_GapBetweenRowsModifier = 1.2f;
        private const int k_LivesDistanceFromHorizontalScreenBound = 15;
        private GameState m_GameState;
        private CollisionHandler m_CollisionHandler;
        private Spaceship m_Player1Spaceship;
        private Spaceship m_Player2Spaceship;
        private LivesRow m_Player1Lives;
        private LivesRow m_Player2Lives;
        private ScoreText m_Player1ScoreText;
        private ScoreText m_Player2ScoreText;
        private Mothership m_Mothership;
        private InvadersMatrix m_InvadersMatrix;
        private DancingBarriersRow m_DancingBarriersRow;
        private bool m_GameOver = false;
        private bool m_LevelCleared = false;

        public PlayScreen(Game i_Game) : base(i_Game)
        {            
            this.BlendState = BlendState.NonPremultiplied;
            m_GameState = Game.Services.GetService<GameState>();
            m_CollisionHandler = new CollisionHandler(i_Game);
            m_CollisionHandler.EnemyCollidedWithSpaceship += () => m_GameOver = true;
            loadDrawables();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            m_Player2Lives.Visible = m_Player2ScoreText.Visible = m_GameState.IsMultiplayer;
            m_Player2Spaceship.Enabled &= m_GameState.IsMultiplayer;
            m_Player2Spaceship.Visible &= m_GameState.IsMultiplayer;
        }

        private void loadDrawables()
        {
            m_Player1Spaceship = new Player1Spaceship(Game);
            m_Player1Spaceship.Died += onSpaceshipKilled;
            this.Add(m_Player1Spaceship);

            m_Player2Spaceship = new Player2Spaceship(Game);
            m_Player2Spaceship.Died += onSpaceshipKilled;
            this.Add(m_Player2Spaceship);

            m_Player1Lives = new LivesRow(Game, m_Player1Spaceship.Lives, m_Player1Spaceship.AssetName);
            m_Player1Spaceship.LivesCountChanged += m_Player1Lives.UpdateLivesCount;
            this.Add(m_Player1Lives);

            m_Player2Lives = new LivesRow(Game, m_Player2Spaceship.Lives, m_Player2Spaceship.AssetName);
            m_Player2Spaceship.LivesCountChanged += m_Player2Lives.UpdateLivesCount;
            this.Add(m_Player2Lives);

            m_Player1ScoreText = new ScoreText(m_GameState.Player1Name, Color.Blue, Game, k_ScoreFontAsset);
            m_GameState.Player1ScoreChanged += m_Player1ScoreText.UpdateNewScore;
            this.Add(m_Player1ScoreText);

            m_Player2ScoreText = new ScoreText(m_GameState.Player2Name, Color.Green, Game, k_ScoreFontAsset);
            m_GameState.Player2ScoreChanged += m_Player2ScoreText.UpdateNewScore;
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
            m_InvadersMatrix.PopulateMatrix(m_GameState.DifficultyLevel);
            m_DancingBarriersRow.Dance(m_GameState.DifficultyLevel);
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
            if (m_GameState.IsMultiplayer)
            {
                m_GameOver = !m_Player1Spaceship.IsAlive && !m_Player2Spaceship.IsAlive;
            }
            else
            {
                m_GameOver = !m_Player1Spaceship.IsAlive;
            } 
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            takeInput();

            if (m_LevelCleared)
            {
                m_GameState.LevelNumber++;
                ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
            }
            else if (m_GameOver)
            {
                this.ScreensManager.SetCurrentScreen(new GameOverScreen(Game));
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            if (m_GameOver)
            {
                m_GameState.ResetLevelAndScore();
                m_Player1Spaceship.ResetLives();
                m_Player2Spaceship.ResetLives();
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
            m_InvadersMatrix.PopulateMatrix(m_GameState.DifficultyLevel);
            m_DancingBarriersRow.Reset();
            m_DancingBarriersRow.Dance(m_GameState.DifficultyLevel);
            m_Mothership.HideAndWaitForNextSpawn();
            m_Player1Spaceship.ResetEverythingButLives();
            m_Player2Spaceship.ResetEverythingButLives();
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

        private void takeInput()
        {
            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }
            else if (InputManager.KeyPressed(Keys.P))
            {
                this.ScreensManager.SetCurrentScreen(new PauseScreen(Game));
            }
        }
    }
}