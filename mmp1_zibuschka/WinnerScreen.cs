/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka {
    class WinnerScreen : Screen {

        #region members and properties

        internal override Texture2D Texture { get; set; }
        
        private Texture2D _winnerLion;
        private Texture2D _winnerGazelle;

        #endregion

        internal WinnerScreen(GraphicsDevice graphics, ContentManager content, LinkedList<Button> buttons) : base(graphics, buttons) {
            InitializeTextures(content);
            SetInitialButtonFocus();
        }

        #region methods
        
        internal void SetWinner(Winner winner){
            if (winner == Winner.Gazelle)
                Texture = _winnerGazelle;

            if (winner == Winner.Lion)
                Texture = _winnerLion;
        }

        protected override void InitializeTextures(ContentManager content) {
            _winnerGazelle = content.Load<Texture2D>(StaticStrings.ImageWinnerGazelle);
            _winnerLion = content.Load<Texture2D>(StaticStrings.ImageWinnerLion);
        }

        #endregion
    }
}
