using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1 {
    /// <summary>
    /// A simple input handler. Add this as a component to your game to let it automatically update.
    /// </summary>
    public class Input : GameComponent {
        #region Action Map

        /// <summary>
        /// A list of gamepad buttons and keyboard keys.
        /// </summary>
        public class List {

            /// <summary>
            /// Add the given button to the input list.
            /// </summary>
            public void Add(Buttons button) {
                GamePadButtons.Add(button);
            }

            /// <summary>
            /// Add the given key to the input list.
            /// </summary>
            public void Add(Keys key) {
                KeyboardKeys.Add(key);
            }

            /// <summary>
            /// List of GamePad buttons.
            /// </summary>
            internal readonly List<Buttons> GamePadButtons = new List<Buttons>();

            /// <summary>
            /// List of Keyboard keys.
            /// </summary>
            internal readonly List<Keys> KeyboardKeys = new List<Keys>();
        }

        /// <summary>
        /// Check if any of the inputs registered for the given action is pressed.
        /// </summary>
        public static bool IsPressed(object action) {
            return IsActionPressed(ActionMaps[action]);
        }

        /// <summary>
        /// Check if any of the inputs registered for the given action is down.
        /// </summary>
        public static bool IsDown(object action) {
            return IsActionDown(ActionMaps[action]);
        }

        /// <summary>
        /// Check if any of the inputs registered for the given action is released.
        /// </summary>
        public static bool IsReleased(object action) {
            return IsActionReleased(ActionMaps[action]);
        }

        private static bool IsActionPressed(List map) {
            return map.KeyboardKeys.Any(IsPressed) ||
                   map.GamePadButtons.Any(IsPressed);
        }

        private static bool IsActionDown(List map) {
            return map.KeyboardKeys.Any(IsDown) ||
                   map.GamePadButtons.Any(IsDown);
        }

        private static bool IsActionReleased(List map) {
            return map.KeyboardKeys.Any(IsReleased) ||
                   map.GamePadButtons.Any(IsReleased);
        }

        private static Dictionary<object, List> ActionMaps { get; set; }

        /// <summary>
        /// Add or update the inputlist of the given action.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="list"></param>
        public static void SetAction(object action, List list) {
            ActionMaps[action] = list;
        }

        /// <summary>
        /// Remove the given action.
        /// </summary>
        /// <param name="action"></param>
        public static void RemoveAction(object action) {
            ActionMaps.Remove(action);
        }

        /// <summary>
        /// Clear all actions.
        /// </summary>
        public static void ClearActions() {
            ActionMaps.Clear();
        }

        #endregion

        #region Constructor

        /// <summary>Create a new Input object. Should be done only once in a game.</summary>
        /// <remarks>
        /// Recommended use in main Game class: <c>Components.Add(new Input(this));</c>
        /// Alternatively keep a reference to this instance and update it manually every frame.
        /// </remarks>
        public Input(Game game) : base(game) {
            _keyboardState = Keyboard.GetState();
            ActionMaps = new Dictionary<object, List>();
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates keyboard and gamepad states.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _lastGamePadState = _gamePadState;
            _gamePadState = GamePad.GetState(PlayerIndex.One);

            base.Update(gameTime);
        }

        #endregion

        #region Keyboard

        private static KeyboardState _keyboardState;
        private static KeyboardState _lastKeyboardState;

        /// <summary>
        /// Check if the given key is released this frame.
        /// </summary>
        public static bool IsReleased(Keys key) {
            return _keyboardState.IsKeyUp(key) && _lastKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Check if the given key is pressed this frame.
        /// </summary>
        public static bool IsPressed(Keys key) {
            return _keyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Check if the given key is down.
        /// </summary>
        public static bool IsDown(Keys key) {
            return _keyboardState.IsKeyDown(key);
        }

        #endregion

        #region GamePad

        private static GamePadState _gamePadState;
        private static GamePadState _lastGamePadState;

        /// <summary>
        /// Check if the given button is released this frame.
        /// </summary>
        public static bool IsReleased(Buttons button) {
            return _gamePadState.IsButtonUp(button) && _lastGamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Check if the given button is pressed this frame.
        /// </summary>
        public static bool IsPressed(Buttons button) {
            return _gamePadState.IsButtonDown(button) && _lastGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Check if the given button is down.
        /// </summary>
        public static bool IsDown(Buttons button) {
            return _gamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Get the position of the left thumbstick.
        /// </summary>
        public static Vector2 LeftThumbStick() {
            return _gamePadState.ThumbSticks.Left;
        }

        /// <summary>
        /// Get the position of the Right thumbstick.
        /// </summary>
        public static Vector2 RightThumbStick() {
            return _gamePadState.ThumbSticks.Right;
        }

        #endregion
    }
}