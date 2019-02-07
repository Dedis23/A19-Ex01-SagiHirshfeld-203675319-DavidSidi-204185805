using Microsoft.Xna.Framework;
using Infrastructure.Menus;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public abstract class SpaceInvadersMenuScreen : MenuScreen
    {
        protected const string k_MenuItemFontAsset = @"Fonts\OptionsFont";
        private const float k_FirstMenuItemX = 100.0f;
        private const float k_FirstMenuItemY = 150.0f;
        private const float k_DistanceBetweenMenuItemsY = 75.0f;
        private const float k_MenuTitleY = 25.0f;
        protected Vector2 m_NextRowPosition;
        protected TextSprite m_TitleText;

        private Vector2 m_ScreenSize;
        bool m_ShowTitle;

        public SpaceInvadersMenuScreen(Game i_Game, string i_MenuTitle) : base(i_Game, Color.White, Color.Orange)
        {
            m_ScreenSize = new Vector2(i_Game.GraphicsDevice.Viewport.Width, i_Game.GraphicsDevice.Viewport.Height);
            loadTitle(i_MenuTitle);
            m_ShowTitle = false;
            m_NextRowPosition.X = k_FirstMenuItemX;
            m_NextRowPosition.Y = k_FirstMenuItemY;
            BuildMenuItems();
        }

        public Vector2 NextRowPosition
        {
            get { return m_NextRowPosition; }
            set { m_NextRowPosition = value; }
        }

        protected void AddNextRow(MenuItemsRow i_MenuRow)
        {
            i_MenuRow.RowPosition = NextRowPosition;
            NextRowPosition = new Vector2(NextRowPosition.X, NextRowPosition.Y + k_DistanceBetweenMenuItemsY);
            AddMenuItem(i_MenuRow);
        }

        private void loadTitle(string i_MenuTitle)
        {
            m_TitleText = new TextSprite(this.Game, k_MenuItemFontAsset);
            m_TitleText.Text = i_MenuTitle;
            m_TitleText.TintColor = Color.Orange;
            m_TitleText.Visible = false;
            Add(m_TitleText);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                this.Game.Exit();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (!m_ShowTitle)
            {
                m_TitleText.Position = new Vector2((m_ScreenSize.X / 2) - (m_TitleText.GetTextSize().X / 2), k_MenuTitleY);
                m_TitleText.Visible = true;
                m_ShowTitle = true;
            }
        }

        protected abstract override void BuildMenuItems();
    }
}