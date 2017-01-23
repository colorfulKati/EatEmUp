/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka{
    class Platform : GameObject{

        #region members and properties

        /// <summary>
        /// The rectangle in which the texture is drawn.
        /// </summary>
        private Rectangle _textureRectangle;

        /// <summary>
        /// The rectangle which is used for all collision checks.
        /// The collision rectangle is a little bit smaller than the rectangle in which the platform is drawn.
        /// This is because of the grass on top of the platform and the rounded ends.
        /// </summary>
        private Rectangle _collisionRectangle;

        internal override Texture2D Texture { get; set; }
        internal override Point Position { get; set; }

        internal override Rectangle Rectangle{
            get { return _collisionRectangle; }
        }

        internal override int Width{
            get { return Rectangle.Width; }
        }

        internal override int Height{
            get { return Rectangle.Height; }
        }

        /// <summary>
        /// The width of the rounded ends of the platform.
        /// </summary>
        internal static readonly int CollisionOffsetWidth = 12;

        /// <summary>
        /// The height of the grass on top of the platform
        /// </summary>
        internal static readonly int CollisionOffsetHeight = 6;

        #endregion

        #region methods

        internal void Initialize(Texture2D texture, Point position){
            Position = new Point(position.X + CollisionOffsetWidth, position.Y + CollisionOffsetHeight); 
            Texture = texture;

            _textureRectangle = new Rectangle(position.X, position.Y, Texture.Width, Texture.Height);
            _collisionRectangle = new Rectangle(Position.X, Position.Y, Texture.Width - 2*CollisionOffsetWidth,
                Texture.Height - 2*CollisionOffsetHeight);
        }
        
        public override void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(Texture, _textureRectangle, Color.White);
        }

        #endregion
    }
}