﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using Infrastructure.Menus;

namespace SpaceInvaders
{
    public class SoundSettings : SpaceInvadersMenuScreen
    {
        public SoundSettings(Game i_Game) : base(i_Game, "Sound Settings")
        {
        }

        protected override void BuildMenuItems()
        {
            // Toggle Sound
            MenuItem[] mouseVisabilityMenuItem = new MenuItem[]
            {
               new MenuItem(setSoundOn, Keys.PageUp, new TextSprite(this.Game, k_MenuItemFontAsset)
               { Text = "On", Position = new Vector2(m_NextRowPosition.X + 575, m_NextRowPosition.Y) }),
               new MenuItem(setSoundOff, Keys.PageDown, new TextSprite(this.Game, k_MenuItemFontAsset)
               { Text = "Off", Position = new Vector2(m_NextRowPosition.X + 725, m_NextRowPosition.Y) })
            };
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Toggle Sound:" },
                Color.White, Color.Orange, 0,
                mouseVisabilityMenuItem));

            // Background Music Volume
            AddNextRow(new BarMenuItem(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Background Music Volume:" },
                Color.LightYellow, Color.White, Color.Orange,
                new Rectangle(680, (int)m_NextRowPosition.Y + 25, 250, 25),
                0.0f, 100.0f, 10.0f, 50.0f,
                new MenuItem(increaseBackgroundMusicVolume, Keys.PageUp),
                new MenuItem(decreaseBackgroundMusicVolume, Keys.PageDown)));

            // Sounds Effect Volume
            AddNextRow(new BarMenuItem(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Sounds Effects Volume:" },
                Color.LightYellow, Color.White, Color.Orange,
                new Rectangle(680, (int)m_NextRowPosition.Y + 25, 250, 25),
                0.0f, 100.0f, 10.0f, 50.0f,
                new MenuItem(increaseSoundsEffectsVolume, Keys.PageUp),
                new MenuItem(decreaseSoundsEffectsVolume, Keys.PageDown)));

            // Done
            MenuItem doneMenuItem = new MenuItem(doneOperation, Keys.Enter);
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Done" },
                Color.White, Color.Orange,
                doneMenuItem));
        }

        private void setSoundOn()
        {
            // TODO
        }

        private void setSoundOff()
        {
            // TODO
        }
        // DEBUG
        int test = 50;
        // DEBUG
        private void increaseBackgroundMusicVolume()
        {
            // TODO
            // DEBUG
            test += 10;
            Game.Window.Title = "in: increase background music volume, num is " + test;
            // DEBUG
        }

        private void decreaseBackgroundMusicVolume()
        {
            // TODO
            // DEBUG
            test -= 10;
            Game.Window.Title = "in: decrease background music volume, num is " + test;
            // DEBUG
        }

        private void increaseSoundsEffectsVolume()
        {
            // TODO
            // DEBUG
            Game.Window.Title = "in: increase sounds effects volume";
            // DEBUG
        }

        private void decreaseSoundsEffectsVolume()
        {
            // TODO
            // DEBUG
            Game.Window.Title = "in: decrease sounds effects volume";
            // DEBUG
        }

        private void doneOperation()
        {
            ExitScreen();
        }
    }
}
