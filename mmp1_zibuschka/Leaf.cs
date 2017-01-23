/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka {
    class Leaf : GameObject{

        #region members and properties

        private Rectangle _rectangle;
        private Point _position;

        internal override Texture2D Texture { get; set; }

        internal override Point Position{
            get { return _position; }
            set {
                _position.X = value.X;
                _position.Y = value.Y;
                _rectangle = new Rectangle(_position, new Point(Width, Height));
            }
        }

        internal override Rectangle Rectangle { get {return _rectangle;} }
        internal override int Width { get { return Texture.Width; } }
        internal override int Height { get { return Texture.Height; } }

        #endregion

        #region methods

        internal void Initialize(Texture2D texture, Point position){
            Texture = texture;
            Position = position;
        }
        
        public override void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

        #endregion
    }
}
