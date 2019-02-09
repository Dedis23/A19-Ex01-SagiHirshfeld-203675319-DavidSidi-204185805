using System;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Menus
{
    public class MenuItem
    {
        protected Action m_Operation;
        protected Keys m_Key;
        protected Sprite m_Sprite;

        public Action Operation
        {
            get { return m_Operation; }
            set { m_Operation = value; }
        }

        public Keys Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        public Sprite Sprite
        {
            get { return m_Sprite; }
            set { m_Sprite = value; }
        }

        public MenuItem(Action i_Operation, Keys i_Key, Sprite i_Sprite)
        {
            m_Key = i_Key;
            m_Operation = i_Operation;
            m_Sprite = i_Sprite;
        }

        public MenuItem(Action i_Operation, Keys i_Key)
        {
            m_Key = i_Key;
            m_Operation = i_Operation;
            m_Sprite = null;
        }
    }
}
