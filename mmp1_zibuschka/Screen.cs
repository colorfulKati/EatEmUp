/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mmp1_zibuschka{
    abstract class Screen : GameObject{

        #region members and properties

        protected Button _focusedButton;

        protected LinkedList<Button> _buttons;

        internal override Point Position{
            get { return _position; }
            set { }
        }

        private readonly Point _position = new Point(0, 0);

        internal override Rectangle Rectangle { get { return _rectangle; } }

        private Rectangle _rectangle;

        internal override int Width { get { return Rectangle.Width; } }
        internal override int Height { get { return Rectangle.Height; } }

        internal Button FocusedButton{
            get { return _focusedButton; }
            set
            {
                // if a new button should be focused, 
                // the previous focused button gets informed to lose it's focus
                // and the new focused button gets informed to gain the focus.
                if(_focusedButton != null)
                    _focusedButton.LoseFocus();
                _focusedButton = value;
                _focusedButton.GetFocus();
            }
        }

        #endregion

        internal Screen(GraphicsDevice graphics, LinkedList<Button> buttons){
            _rectangle = new Rectangle(Position, graphics.Viewport.Bounds.Size);
            _buttons = buttons;
        }

        #region methods
        
        protected abstract void InitializeTextures(ContentManager content);

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Rectangle, Color.White);

            foreach (var button in _buttons) {
                button.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// This method sets the button focus to the first button in the ButtonList
        /// It's called, when the GameState changes and therefore the screen is "initially" shown
        /// </summary>
        internal void SetInitialButtonFocus() {
            FocusedButton = _buttons.First.Value;
        }

        /// <summary>
        /// This method returns the focused button and updates the focused button if up or down were pressed.
        /// </summary>
        /// <param name="pressedKey"> The pressed key. </param>
        /// <returns> The currently focused button of the active screen. </returns>
        internal Button GetFocusedButton(Keys pressedKey = Keys.None){
            if (pressedKey == Keys.None)
                return _focusedButton;

            var current = _buttons.First;
            
            while (current.Value != _focusedButton) {
                current = current.Next;
            }
            
            // update the focused button, depending on the pressed key
            switch (pressedKey) {
                case Keys.Up:
                        current = current.Previous ?? _buttons.Last;
                    break;
                case Keys.Down:
                        current = current.Next ?? _buttons.First;
                    break;
            }
            
            FocusedButton = current.Value;
            return _focusedButton;
        }

        #endregion
    }
}
