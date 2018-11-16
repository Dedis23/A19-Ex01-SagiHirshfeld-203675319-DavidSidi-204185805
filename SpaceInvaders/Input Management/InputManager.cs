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
        private Dictionary<Action<GameTime>, Keys> m_ActionToKeyboardDownDictionary;
        private Dictionary<Action<GameTime>, Keys> m_ActionToKeyboardSinglePressDictionary;
        private KeyboardState? m_PrevKeyboardState;
        private MouseState? m_PrevMouseState;
        public event Action<GameTime, Vector2> MouseMoved;
        public event Action<GameTime> MouseLeftButtonPressed;
        public event Action<GameTime> MouseRightButtonPressed;
        public event Action<GameTime> MouseLeftButtonPressedOnce;
        public event Action<GameTime> MouseRightButtonPressedOnce;

        public InputManager(Game game) : base(game)
        {
            m_ActionToKeyboardDownDictionary = new Dictionary<Action<GameTime>, Keys>();
            m_ActionToKeyboardSinglePressDictionary = new Dictionary<Action<GameTime>, Keys>(); 
        }

        public void RegisterKeyboardKeyDownBinding(Action<GameTime> i_Action, Keys i_KeyboardKey)
        {
            if(m_ActionToKeyboardDownDictionary.ContainsKey(i_Action))
            {
                m_ActionToKeyboardDownDictionary[i_Action] = i_KeyboardKey;
            }

            else
            {
                m_ActionToKeyboardDownDictionary.Add(i_Action, i_KeyboardKey);
            }
        }

        public void RegisterKeyboardSinglePressBinding(Action<GameTime> i_Action, Keys i_KeyboardKey)
        {
            if (m_ActionToKeyboardSinglePressDictionary.ContainsKey(i_Action))
            {
                m_ActionToKeyboardSinglePressDictionary[i_Action] = i_KeyboardKey;
            }

            else
            {
                m_ActionToKeyboardSinglePressDictionary.Add(i_Action, i_KeyboardKey);
            }
        }

        public void RemoveKeyboardDownBinding(Action<GameTime> i_Action)
        {
            m_ActionToKeyboardDownDictionary.Remove(i_Action);
        }

        public void RemoveKeyboardSinglePressBinding(Action<GameTime> i_Action)
        {
            m_ActionToKeyboardSinglePressDictionary.Remove(i_Action);
        }

        public override void Update(GameTime i_GameTime)
        {   
            checkAndNotifyForKeyboardInput(i_GameTime);
            checkAndNotifyForMouseInput(i_GameTime);

            m_PrevKeyboardState = Keyboard.GetState();
            m_PrevMouseState = Mouse.GetState();

            base.Update(i_GameTime);
        }

        private void checkAndNotifyForKeyboardInput(GameTime i_GameTime)
        {
            // Check any notify if any keyboard key of a registered action is down
            foreach (Action<GameTime> action in m_ActionToKeyboardDownDictionary.Keys)
            {
                if (Keyboard.GetState().IsKeyDown(m_ActionToKeyboardDownDictionary[action]))
                {
                    action.Invoke(i_GameTime);
                }
            }

            // Check any notify if a keyboard key was pressed only once
            foreach (Action<GameTime> action in m_ActionToKeyboardSinglePressDictionary.Keys)
            {
                Keys keyBindedToAction = m_ActionToKeyboardSinglePressDictionary[action];
                if (Keyboard.GetState().IsKeyDown(keyBindedToAction) && (m_PrevKeyboardState == null || !m_PrevKeyboardState.Value.IsKeyDown(keyBindedToAction)))
                {
                    action.Invoke(i_GameTime);
                }
            }
        }

        private void checkAndNotifyForMouseInput(GameTime i_GameTime)
        {
            Vector2 mousePositionDelta = getMousePositionDelta();
            if (mousePositionDelta != Vector2.Zero)
            {
                MouseMoved?.Invoke(i_GameTime, mousePositionDelta);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                MouseLeftButtonPressed?.Invoke(i_GameTime);
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                MouseRightButtonPressed?.Invoke(i_GameTime);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && 
                (m_PrevMouseState == null || m_PrevMouseState.Value.LeftButton == ButtonState.Released))
            {
                MouseLeftButtonPressedOnce?.Invoke(i_GameTime);
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed &&
                (m_PrevMouseState == null || m_PrevMouseState.Value.RightButton == ButtonState.Released))
            {
                MouseRightButtonPressedOnce?.Invoke(i_GameTime);
            }
        }

        private Vector2 getMousePositionDelta()
        {
            Vector2 retVal = Vector2.Zero;

            if (m_PrevMouseState != null)
            {
                retVal.X = Mouse.GetState().X - m_PrevMouseState.Value.X;
                retVal.Y = Mouse.GetState().Y - m_PrevMouseState.Value.Y;
            }

            return retVal;
        }
    }
}
