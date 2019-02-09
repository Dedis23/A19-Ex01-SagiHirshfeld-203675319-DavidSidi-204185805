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
               new MenuItem(
                   toggleAllSound,
                   Keys.PageUp,
                   new TextSprite(this.Game, k_MenuItemFontAsset)
                   {
                       Text = "On", Position = new Vector2(m_NextRowPosition.X + 575, m_NextRowPosition.Y)
                   }),
               new MenuItem(
                   toggleAllSound,
                   Keys.PageDown,
                   new TextSprite(this.Game, k_MenuItemFontAsset)
                   {
                       Text = "Off", Position = new Vector2(m_NextRowPosition.X + 725, m_NextRowPosition.Y)
                   })
            };
            int allSoundStateItemToMark = m_SoundManager.MuteAllSound ? 1 : 0;
            AddNextRow(
                new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset)
                {
                    Text = "Toggle Sound:"
                },
                Color.White,
                Color.Orange,
                allSoundStateItemToMark,
                toggleSoundMenuItem));

            // Background Music Volume
            AddNextRow(
                new BarMenuItem(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset)
                {
                    Text = "Background Music Volume:"
                },
                Color.LightYellow,
                Color.White,
                Color.Orange,
                new Rectangle((int)m_NextRowPosition.X + 575, (int)m_NextRowPosition.Y + 25, 250, 25),
                0.0f,
                100.0f,
                10.0f,
                m_SoundManager.MediaVolume * 100,
                new MenuItem(increaseBackgroundMusicVolume, Keys.PageUp),
                new MenuItem(decreaseBackgroundMusicVolume, Keys.PageDown)));

            // Sounds Effect Volume
            AddNextRow(
                new BarMenuItem(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset)
                {
                    Text = "Sounds Effects Volume:"
                },
                Color.LightYellow,
                Color.White,
                Color.Orange,
                new Rectangle((int)m_NextRowPosition.X + 575, (int)m_NextRowPosition.Y + 25, 250, 25),
                0.0f,
                100.0f,
                10.0f,
                m_SoundManager.SoundEffectsVolume * 100,
                new MenuItem(increaseSoundsEffectsVolume, Keys.PageUp),
                new MenuItem(decreaseSoundsEffectsVolume, Keys.PageDown)));

            // Done
            MenuItem doneMenuItem = new MenuItem(doneOperation, Keys.Enter);
            AddNextRow(
                new MenuItemsRow(
                this,
                new AnimatedTextSprite(this.Game, k_MenuItemFontAsset)
                {
                    Text = "Done"
                },
                Color.White,
                Color.Orange,
                doneMenuItem));
        }

        private void toggleAllSound()
        {
            m_SoundManager.MuteAllSound = !m_SoundManager.MuteAllSound;
        }

        private void increaseBackgroundMusicVolume()
        {
            m_SoundManager.MediaVolume += 0.1f;
        }

        private void decreaseBackgroundMusicVolume()
        {
            m_SoundManager.MediaVolume -= 0.1f;
        }

        private void increaseSoundsEffectsVolume()
        {
            m_SoundManager.SoundEffectsVolume += 0.1f;
        }

        private void decreaseSoundsEffectsVolume()
        {
            m_SoundManager.SoundEffectsVolume -= 0.1f;
        }

        private void doneOperation()
        {
            ExitScreen();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            const int v_ToggleSoundRow = 0;
            const int v_OffItem = 0;
            const int v_OnItem = 1;
            if (InputManager.KeyPressed(Keys.M))
            {
                if (m_SoundManager.MuteAllSound)
                {
                    MarkASpecificItemInTheRow(v_ToggleSoundRow, v_OffItem);
                }
                else
                {
                    MarkASpecificItemInTheRow(v_ToggleSoundRow, v_OnItem);
                }
            }
        }
    }
}
