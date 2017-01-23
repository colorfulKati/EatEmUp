/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka {
    class Button : GameObject {

        #region properties

        internal override Texture2D Texture { get; set; }
        internal override Point Position { get; set; }
        internal override Rectangle Rectangle { get { return new Rectangle(Position.X, Position.Y, Width, Height); } }
        internal override int Width { get { return Texture.Width; } }
        internal override int Height { get { return Texture.Height; } }

        /// <summary>
        /// If the button is not focused, it just gets drawn in its normal color.
        /// But if the button is the current focused button, it's drawn a bit darker than the other buttons
        /// </summary>
        private Color _color;
        
        /// <summary>
        /// True, if the button should be displayed 
        /// </summary>
        internal bool IsShown;

        #endregion

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Rectangle, _color);
        }

        #region internal methods

        internal void Initialize(Texture2D texture, Point position){
            Texture = texture;
            Position = position;
            _color = Color.White;;
        }
        
        internal void GetFocus(){
            _color = Color.Chocolate;
        }

        internal void LoseFocus() {
            _color = Color.White;
        }

        #endregion
    }
}
