/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka {
    class Shoes : GameObject {

        #region members and properties

        internal bool IsVisible { get { return _isVisible; } }
        private bool _isVisible;

        /// <summary>
        /// A point, where the shoes are located, when they aren't shown.
        /// </summary>
        private Point _hidingSpot;

        internal override Texture2D Texture { get; set; }
        internal override Point Position { get; set; }

        private Rectangle _rectangle;

        internal override Rectangle Rectangle{ get { return _rectangle; } }

        internal override int Width { get { return Texture.Width; } }
        internal override int Height { get { return Texture.Height; } }

        #endregion

        public override void Draw(SpriteBatch spriteBatch){
            if(_isVisible)
                spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

        #region internal methods

        internal void Initialize(Texture2D texture){
            Texture = texture;
            _hidingSpot = new Point(-500, -500);
            ResetPosition();
        }

        /// <summary>
        /// The shoes appear at the passed position
        /// </summary>
        /// <param name="position"> The point, where the shoes should be shown. </param>
        internal void Show(Point position){
            Position = position;
            _rectangle.Location = position;
            _isVisible = true;
        }

        internal void Hide() {
            ResetPosition();
            _isVisible = false;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Resets the position and the rectangle of the shoes to the hiding spot.
        /// </summary>
        private void ResetPosition(){
            Position = _hidingSpot;
            _rectangle = new Rectangle(_hidingSpot.X, -_hidingSpot.Y, Width, Height);
        }

        #endregion
    }
}
