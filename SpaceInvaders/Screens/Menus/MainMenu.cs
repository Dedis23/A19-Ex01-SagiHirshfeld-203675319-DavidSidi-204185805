using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using Infrastructure.Menus;

namespace SpaceInvaders
{
    public class MainMenu : SpaceInvadersMenuScreen
    {
        public MainMenu(Game i_Game) : base(i_Game, "Main Menu")
        {
        }

        protected override void BuildMenuItems()
        {
            // Players
            MenuItem[] multiplayerMenuItem = new MenuItem[]
            {
               new MenuItem(activateOnePlayerMode, Keys.PageUp, new TextSprite(this.Game, k_MenuItemFontAsset)
               { Text = "One", Position = new Vector2(m_NextRowPosition.X + 550, m_NextRowPosition.Y) }),
               new MenuItem(activateTwoPlayerMode, Keys.PageDown, new TextSprite(this.Game, k_MenuItemFontAsset)
               { Text = "Two", Position = new Vector2(m_NextRowPosition.X + 700, m_NextRowPosition.Y) })
            };
            AddNextRow(new MenuItemsRow(
                this, 
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Players:" },
                Color.White, Color.Orange, 0,
                multiplayerMenuItem));

            // Screen Settings
            MenuItem screenSettingsMenuItem = new MenuItem(screenSettingsOperation, Keys.Enter);
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Screen Settings" },
                Color.White, Color.Orange,
                screenSettingsMenuItem));

            // Sound Settings
            MenuItem soundSettingsMenuItem = new MenuItem(soundSettingsOperation, Keys.Enter);
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Sound Settings" },
                Color.White, Color.Orange,
                soundSettingsMenuItem));

            // Play
            MenuItem playMenuItem = new MenuItem(playOperation, Keys.Enter);
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Play" },
                Color.White, Color.Orange,
                playMenuItem));

            // Exit
            MenuItem exitMenuItem = new MenuItem(exitOperation, Keys.Enter);
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Exit" },
                Color.White, Color.Orange,
                exitMenuItem));
        }

        private void activateOnePlayerMode()
        {
            // TODO
            // DEBUG
            Game.Window.Title = "in: one";
            // DEBUG
        }

        private void activateTwoPlayerMode()
        {
            // TODO
            // DEBUG
            Game.Window.Title = "in: two";
            // DEBUG
        }

        private void screenSettingsOperation()
        {
            ScreensManager.SetCurrentScreen(new ScreenSettings(this.Game));
        }

        private void soundSettingsOperation()
        {
            ScreensManager.SetCurrentScreen(new SoundSettings(this.Game));
        }

        private void playOperation()
        {
            ScreensManager.SetCurrentScreen(new PlayScreen(this.Game));
        }

        private void exitOperation()
        {
            this.Game.Exit();
        }
    }
}