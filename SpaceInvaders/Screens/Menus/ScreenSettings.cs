using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using Infrastructure.Menus;

namespace SpaceInvaders
{
    public class ScreenSettings : SpaceInvadersMenuScreen
    {
        public ScreenSettings(Game i_Game) : base(i_Game, "Screen Settings")
        {
        }

        protected override void BuildMenuItems()
        {
            // Mouse Visability
            MenuItem[] mouseVisabilityMenuItem = new MenuItem[]
            {
               new MenuItem(
                   toggleMouseVisability,
                   Keys.PageUp,
                   new TextSprite(this.Game, k_MenuItemFontAsset)
                   {
                        Text = "Visible", Position = new Vector2(m_NextRowPosition.X + 525, m_NextRowPosition.Y)
                   }),
               new MenuItem(
                   toggleMouseVisability,
                   Keys.PageDown,
                   new TextSprite(this.Game, k_MenuItemFontAsset)
                   {
                        Text = "Invisible", Position = new Vector2(m_NextRowPosition.X + 675, m_NextRowPosition.Y)
                   })
            };
            int mouseVisabilityItemToMark = Game.IsMouseVisible ? 0 : 1;
            AddNextRow(
                new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset)
                {
                    Text = "Mouse Visability:"
                },
                Color.White,
                Color.Orange,
                mouseVisabilityItemToMark,
                mouseVisabilityMenuItem));

            // Allow Window Resizing
            MenuItem[] allowWindowResizingMenuItem = new MenuItem[]
            {
               new MenuItem(
                   toggleWindowResizing,
                   Keys.PageUp,
                   new TextSprite(this.Game, k_MenuItemFontAsset)
                   {
                       Text = "On", Position = new Vector2(m_NextRowPosition.X + 525, m_NextRowPosition.Y)
                   }),
               new MenuItem(
                   toggleWindowResizing,
                   Keys.PageDown,
                   new TextSprite(this.Game, k_MenuItemFontAsset)
                   {
                       Text = "Off", Position = new Vector2(m_NextRowPosition.X + 675, m_NextRowPosition.Y)
                   })
            };
            int allowWindowResizingItemToMark = this.Game.Window.AllowUserResizing ? 0 : 1;
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Allow Window Resizing:" },
                Color.White,
                Color.Orange,
                allowWindowResizingItemToMark,
                allowWindowResizingMenuItem));

            // Full Screen Mode
            MenuItem[] fullScreenModeMenuItem = new MenuItem[]
            {
               new MenuItem(
                   toggleFullScreenMode,
                   Keys.PageUp,
                   new TextSprite(this.Game, k_MenuItemFontAsset)
                   {
                       Text = "On", Position = new Vector2(m_NextRowPosition.X + 525, m_NextRowPosition.Y)
                   }),
               new MenuItem(
                   toggleFullScreenMode,
                   Keys.PageDown,
                   new TextSprite(this.Game, k_MenuItemFontAsset)
                   {
                       Text = "Off", Position = new Vector2(m_NextRowPosition.X + 675, m_NextRowPosition.Y)
                   })
            };
            int fullScreenModeToMark = r_GraphicsDevice.IsFullScreen ? 0 : 1;
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Full Screen Mode:" },
                Color.White,
                Color.Orange,
                fullScreenModeToMark,
                fullScreenModeMenuItem));

            // Done
            MenuItem doneMenuItem = new MenuItem(doneOperation, Keys.Enter);
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Done" },
                Color.White,
                Color.Orange,
                doneMenuItem));
        }

        private void toggleMouseVisability()
        {
            this.Game.IsMouseVisible = !this.Game.IsMouseVisible;
        }

        private void toggleWindowResizing()
        {
            this.Game.Window.AllowUserResizing = !this.Game.Window.AllowUserResizing;
        }

        private void toggleFullScreenMode()
        {
            r_GraphicsDevice.ToggleFullScreen();
        }

        private void doneOperation()
        {
            ExitScreen();
        }
    }
}
