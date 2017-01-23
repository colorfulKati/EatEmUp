/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka {
    public abstract class GameObject {
        
        internal abstract Texture2D Texture { get; set; }
        internal abstract Point Position { get; set; }
        internal abstract Rectangle Rectangle { get; }
        internal abstract int Width { get; }
        internal abstract int Height { get; }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
