using Microsoft.Xna.Framework;
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
            MenuItem[] toggleSoundMenuItem = new MenuItem[]
            {
               new MenuItem(toggleAllSound, Keys.PageUp, new TextSprite(this.Game, k_MenuItemFontAsset)
               { Text = "On",
                   Position = new Vector2(m_NextRowPosition.X + 575, m_NextRowPosition.Y) }),
               new MenuItem(toggleAllSound, Keys.PageDown, new TextSprite(this.Game, k_MenuItemFontAsset)
               { Text = "Off", Position = new Vector2(m_NextRowPosition.X + 725, m_NextRowPosition.Y) })
            };
            int allSoundStateItemToMark = m_ISoundManager.IsAllSoundMuted ? 1 : 0;
            AddNextRow(new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Toggle Sound:" },
                Color.White, Color.Orange, allSoundStateItemToMark,
                toggleSoundMenuItem));

            // Background Music Volume
            AddNextRow(new BarMenuItem(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Background Music Volume:" },
                Color.LightYellow, Color.White, Color.Orange,
                new Rectangle((int)m_NextRowPosition.X + 575, (int)m_NextRowPosition.Y + 25, 250, 25),
                0.0f, 100.0f, 10.0f, m_ISoundManager.MediaVolume * 100,
                new MenuItem(increaseBackgroundMusicVolume, Keys.PageUp),
                new MenuItem(decreaseBackgroundMusicVolume, Keys.PageDown)));

            // Sounds Effect Volume
            AddNextRow(new BarMenuItem(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset) { Text = "Sounds Effects Volume:" },
                Color.LightYellow, Color.White, Color.Orange,
                new Rectangle((int)m_NextRowPosition.X + 575, (int)m_NextRowPosition.Y + 25, 250, 25),
                0.0f, 100.0f, 10.0f, m_ISoundManager.SoundEffectsVolume * 100,
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

        private void toggleAllSound()
        {
            m_ISoundManager.IsAllSoundMuted = !m_ISoundManager.IsAllSoundMuted;
        }

        private void increaseBackgroundMusicVolume()
        {
            m_ISoundManager.MediaVolume += 0.1f;
        }

        private void decreaseBackgroundMusicVolume()
        {
            m_ISoundManager.MediaVolume -= 0.1f;
        }

        private void increaseSoundsEffectsVolume()
        {
            m_ISoundManager.SoundEffectsVolume += 0.1f;
        }

        private void decreaseSoundsEffectsVolume()
        {
            m_ISoundManager.SoundEffectsVolume -= 0.1f;
        }

        private void doneOperation()
        {
            ExitScreen();
        }
    }
}
