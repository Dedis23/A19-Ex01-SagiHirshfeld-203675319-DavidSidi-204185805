using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class InputManager : GameComponent
    {
        private const int k_FramesToWaitBeforeInputTaking = 3;
        private static readonly IEnumerable<Keys> sr_EnumerableKeyboardKeys = Enum.GetValues(typeof(Keys)).Cast<Keys>();
        private readonly Dictionary<Action, Keys> r_ActionToKeyboardDownDictionary;
        private readonly Dictionary<Action, Keys> r_ActionToKeyboardSinglePressDictionary;
        private int m_WaitedFramesBeforeInputTaking = 0;
        private KeyboardState m_CurrentKeyboardState;
        private MouseState m_CurrentMouseState;
        private KeyboardState? m_PrevKeyboardState;
        private MouseState? m_PrevMouseState;

        public event Action<Vector2> MouseMoved;

        public event Action MouseLeftButtonPressed;

        public event Action MouseRightButtonPressed;

        public event Action MouseLeftButtonPressedOnce;

        public event Action MouseRightButtonPressedOnce;

        public InputManager(Game i_Game) : base(i_Game)
        {
            r_ActionToKeyboardDownDictionary = new Dictionary<Action, Keys>();
            r_ActionToKeyboardSinglePressDictionary = new Dictionary<Action, Keys>(); 
        }

        public void RegisterKeyboardKeyDownBinding(Action i_Action, Keys i_KeyboardKey)
        {
            if(r_ActionToKeyboardDownDictionary.ContainsKey(i_Action))
            {
                r_ActionToKeyboardDownDictionary[i_Action] = i_KeyboardKey;
            }
            else
            {
                r_ActionToKeyboardDownDictionary.Add(i_Action, i_KeyboardKey);
            }
        }

        public void RegisterKeyboardSinglePressBinding(Action i_Action, Keys i_KeyboardKey)
        {
            if (r_ActionToKeyboardSinglePressDictionary.ContainsKey(i_Action))
            {   
                r_ActionToKeyboardSinglePressDictionary[i_Action] = i_KeyboardKey;
            }
            else
            {
                r_ActionToKeyboardSinglePressDictionary.Add(i_Action, i_KeyboardKey);
            }
        }

        public void RemoveKeyboardDownBinding(Action i_Action)
        {
            r_ActionToKeyboardDownDictionary.Remove(i_Action);
        }

        public void RemoveKeyboardSinglePressBinding(Action i_Action)
        {
            r_ActionToKeyboardSinglePressDictionary.Remove(i_Action);
        }

        public override void Update(GameTime i_GameTime)
        {
            m_CurrentKeyboardState = Keyboard.GetState();
            m_CurrentMouseState = Mouse.GetState();

            // We've noticed that accurate input data for the mouse only registers
            // after the second frame (= after the second Update)
            if(m_WaitedFramesBeforeInputTaking >= k_FramesToWaitBeforeInputTaking)
            {
                checkAndNotifyForKeyboardInput();
                checkAndNotifyForMouseInput();
            }
            else
            {
                m_WaitedFramesBeforeInputTaking++;
            }

            m_PrevKeyboardState = m_CurrentKeyboardState;
            m_PrevMouseState = m_CurrentMouseState;

            base.Update(i_GameTime);
        }

        private void checkAndNotifyForKeyboardInput()
        {
            // Check and notify if any keyboard key of a registered action is down
            foreach (Action action in r_ActionToKeyboardDownDictionary.Keys)
            {
                if (m_CurrentKeyboardState.IsKeyDown(r_ActionToKeyboardDownDictionary[action]))
                {
                    action.Invoke();
                }
            }

            // Check and notify if a keyboard key was pressed only once
            foreach (Action action in r_ActionToKeyboardSinglePressDictionary.Keys)
            {
                Keys keyBindedToAction = r_ActionToKeyboardSinglePressDictionary[action];
                if (m_CurrentKeyboardState.IsKeyDown(keyBindedToAction) && (m_PrevKeyboardState == null || !m_PrevKeyboardState.Value.IsKeyDown(keyBindedToAction)))
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

            if (m_CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                MouseLeftButtonPressed?.Invoke();
            }

            if (m_CurrentMouseState.RightButton == ButtonState.Pressed)
            {
                MouseRightButtonPressed?.Invoke();
            }

            if (m_CurrentMouseState.LeftButton == ButtonState.Pressed && 
                (m_PrevMouseState == null || m_PrevMouseState.Value.LeftButton == ButtonState.Released))
            {
                MouseLeftButtonPressedOnce?.Invoke();
            }

            if (m_CurrentMouseState.RightButton == ButtonState.Pressed &&
                (m_PrevMouseState == null || m_PrevMouseState.Value.RightButton == ButtonState.Released))
            {
                MouseRightButtonPressedOnce?.Invoke();
            }
        }

        private Vector2 getMousePositionDelta()
        {
            Vector2 retVal = Vector2.Zero;

            if (m_PrevMouseState != null)
            {
                retVal.X = m_CurrentMouseState.X - m_PrevMouseState.Value.X;
                retVal.Y = m_CurrentMouseState.Y - m_PrevMouseState.Value.Y;
            }

            return retVal;
        }
    }
}
