using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Menus
{
    public class MenuItemsRow : DrawableGameComponent
    {
        private IInputManager m_InputManager;
        private int m_LastItem;
        private int m_CurrentItem;
        private GameScreen m_GameScreen;
        private readonly List<MenuItem> r_Items;
        protected AnimatedTextSprite m_MenuTextSprite;
        private Color m_NonSelectedColor;
        private Color m_SelectedColor;
        protected Vector2 m_RowPosition;
        protected Vector2 m_NextPositionInTheRow;
        protected Keys m_RightKey;
        protected Keys m_LeftKey;
        private bool m_IsActive;
        private bool m_IsLoopedItems;

        public MenuItemsRow(GameScreen i_GameScreen, AnimatedTextSprite i_RowText,
            Color i_NonSelectedColor,
            Color i_SelectedColor,
            int i_DefaultItem = 0,
            Keys i_RightKey = Keys.PageDown,
            Keys i_LeftKey = Keys.PageUp,
            params MenuItem[] i_Items)
            : base(i_GameScreen.Game)
        {
            r_Items = new List<MenuItem>(i_Items);
            m_GameScreen = i_GameScreen;
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            m_LastItem = m_CurrentItem = i_DefaultItem;
            m_NonSelectedColor = i_NonSelectedColor;
            m_SelectedColor = i_SelectedColor;
            m_MenuTextSprite = i_RowText;
            m_RightKey = i_RightKey;
            m_LeftKey = i_LeftKey;
            m_IsActive = false;
            m_IsLoopedItems = r_Items.Count > 1;
            loadMenuSpritesToGameScreen();
        }

        public MenuItemsRow(GameScreen i_GameScreen, AnimatedTextSprite i_RowText,
            Color i_NonSelectedColor,
            Color i_SelectedColor,
            MenuItem i_Items)
            : this(i_GameScreen, i_RowText, i_NonSelectedColor, i_SelectedColor, 0, Keys.PageDown, Keys.PageUp, i_Items)
        {
        }

        public MenuItemsRow(GameScreen i_GameScreen, AnimatedTextSprite i_RowText,
            Color i_NonSelectedColor,
            Color i_SelectedColor,
            int i_DefaultItem,
            MenuItem[] i_Items)
            : this(i_GameScreen, i_RowText, i_NonSelectedColor, i_SelectedColor, i_DefaultItem, Keys.PageDown, Keys.PageUp, i_Items)
        {
        }

        public MenuItemsRow(GameScreen i_GameScreen, AnimatedTextSprite i_RowText, MenuItem[] i_Items)
            : this(i_GameScreen, i_RowText, Color.White, Color.White, 0, Keys.PageDown, Keys.PageUp, i_Items)
        {
        }

        public bool Active
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        public bool IsLoopedItems
        {
            get { return m_IsLoopedItems; }
            set { m_IsLoopedItems = value; }
        }

        public List<MenuItem> Items
        {
            get { return r_Items; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (r_Items.Count != 0 && Active)
            {
                if (m_InputManager.KeyPressed(m_LeftKey))
                {
                    m_LastItem = m_CurrentItem--;
                    if (m_CurrentItem < 0)
                    {
                        m_CurrentItem = r_Items.Count - 1;
                    }
                }

                if (m_InputManager.KeyPressed(m_RightKey))
                {
                    m_LastItem = m_CurrentItem++;
                    if (m_CurrentItem >= r_Items.Count)
                    {
                        m_CurrentItem = 0;
                    }
                }
                if (m_LastItem != m_CurrentItem)
                {
                    updateSelectedColor();
                }
            }
        }

        private void updateSelectedColor()
        {
            if (r_Items[m_LastItem].Sprite != null)
            {
                r_Items[m_LastItem].Sprite.TintColor = m_NonSelectedColor;
            }
            if (r_Items[m_CurrentItem].Sprite != null)
            {
                r_Items[m_CurrentItem].Sprite.TintColor = m_SelectedColor;
            }
        }

        public AnimatedTextSprite MenuText
        {
            get { return m_MenuTextSprite; }
            set { m_MenuTextSprite = value; }
        }

        public Vector2 RowPosition
        {
            get { return m_RowPosition; }
            set
            {
                m_RowPosition = value;
                m_MenuTextSprite.Position = value;
            }
        }

        private void loadMenuSpritesToGameScreen()
        {
            foreach (MenuItem item in r_Items)
            {
                m_GameScreen.Add(item.Sprite);
            }
            m_GameScreen.Add(m_MenuTextSprite);
            m_GameScreen.Add(this);
            updateSelectedColor();
        }

        public Keys GetSelectedKey()
        {
            return r_Items[m_CurrentItem].Key;
        }

        public void InvokeCurrentSelected()
        {
            r_Items[m_CurrentItem].Operation?.Invoke();
        }

        public bool IsEmpty
        {
            get { return r_Items.Count == 0; }
        }

        public void StartTitleAnimation()
        {
            m_MenuTextSprite.StartAnimation();
        }

        public void StopTitleAnimation()
        {
            m_MenuTextSprite.StopAnimation();
        }
    }
}