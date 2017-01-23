/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka {
    class CreditsScreen : Screen{

        #region properties
        internal override Texture2D Texture { get; set; }

        #endregion

        internal CreditsScreen(GraphicsDevice graphics, ContentManager content, LinkedList<Button> buttons) : base(graphics, buttons) {
            InitializeTextures(content);
            SetInitialButtonFocus();
        }

        #region methods
        
        protected override void InitializeTextures(ContentManager content){
            Texture = content.Load<Texture2D>(StaticStrings.ImageCredits);
        }

        #endregion
    }
}
