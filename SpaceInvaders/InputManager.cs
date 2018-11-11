using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class InputManager : GameComponent
    {
        private static readonly IEnumerable<Keys> sr_EnumerableKeyboardKeys = Enum.GetValues(typeof(Keys)).Cast<Keys>();
        private Dictionary<Action, Keys> m_ActionToKeyboardKeyDictionary;  
        private MouseState? m_PrevMouseState;
        public event Action<Vector2> MouseMoved;
        public event Action MouseLeftButtonPressed;
        public event Action MouseRightButtonPressed;

        public InputManager(Game game) : base(game)
        {
            m_ActionToKeyboardKeyDictionary = new Dictionary<Action, Keys>();
        }

        public void RegisterKeyboardKeyBinding(Action i_Action, Keys i_KeyboardKey)
        {
            if(m_ActionToKeyboardKeyDictionary.ContainsKey(i_Action))
            {
                m_ActionToKeyboardKeyDictionary[i_Action] = i_KeyboardKey;
            }

            else
            {
                m_ActionToKeyboardKeyDictionary.Add(i_Action, i_KeyboardKey);
            }
        }

        public void RemoveKeyboardKeyBinding(Action i_Action)
        {
            m_ActionToKeyboardKeyDictionary.Remove(i_Action);
        }

        public override void Update(GameTime gameTime)
        {   
            checkAndNotifyForKeyboardInput();
            checkAndNotifyForMouseInput();

            base.Update(gameTime);
        }

        private void checkAndNotifyForKeyboardInput()
        {
            // Check any notify if any keyboard key of a registered action is down
            foreach (Action action in m_ActionToKeyboardKeyDictionary.Keys)
            {
                if (Keyboard.GetState().IsKeyDown(m_ActionToKeyboardKeyDictionary[action]))
                {
                    action.Invoke();
                }
            }
        }

        private void checkAndNotifyForMouseInput()
        {
            Vector2 mousePositionDelta = getMousePositionDelta();
            if (mousePositionDelta != Vector2.Zero)
            {
                MouseMoved?.Invoke(mousePositionDelta);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                MouseLeftButtonPressed?.Invoke();
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                MouseRightButtonPressed?.Invoke();
            }
        }

        private Vector2 getMousePositionDelta()
        {
            Vector2 retVal = Vector2.Zero;
            MouseState currState = Mouse.GetState();

            if (m_PrevMouseState != null)
            {
                retVal.X = (currState.X - m_PrevMouseState.Value.X);
                retVal.Y = (currState.Y - m_PrevMouseState.Value.Y);
            }

            m_PrevMouseState = currState;

            return retVal;
        }
    }
}
