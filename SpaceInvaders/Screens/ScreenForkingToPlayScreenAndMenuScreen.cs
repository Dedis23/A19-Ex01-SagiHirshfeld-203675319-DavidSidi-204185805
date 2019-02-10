using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public abstract class ScreenForkingToPlayScreenAndMenuScreen : GameScreen
    {
        private bool m_PrevScreenIsMainMenu;

        protected abstract Keys ExitKey { get; }

        protected abstract Keys TransitionToPlayScreenKey { get; }

        protected abstract Keys TransitionToMenuScreenKey { get; }

        public ScreenForkingToPlayScreenAndMenuScreen(Game i_Game) : base(i_Game)
        {
        }

        protected override sealed void OnActivated()
        {
            base.OnActivated();

            if (m_PrevScreenIsMainMenu)
            {
                transitionToPlayScreen();
            }
            else
            {
                OnFocused();
            }
        }

        protected virtual void OnFocused()
        {
        }

        public override sealed void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(ExitKey))
            {
                Game.Exit();
            }
            else if (InputManager.KeyPressed(TransitionToPlayScreenKey))
            {
                transitionToPlayScreen();
            }
            else if (InputManager.KeyPressed(TransitionToMenuScreenKey))
            {
                transitionToMenuScreen();
            }
        }

        private void transitionToMenuScreen()
        {
            ScreensManager.SetCurrentScreen(new MainMenu(Game));
            m_PrevScreenIsMainMenu = true;
        }

        private void transitionToPlayScreen()
        {
            ExitScreen();
            ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
        }
    }
}
