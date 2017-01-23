/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka {
    class MainMenuScreen : Screen, IShowControls {

        #region members and properties
        internal override Texture2D Texture { get; set; }
        
        private bool _isGazelleLeft;
        private Texture2D _gazelleLeft;
        private Texture2D _gazelleRight;

        #endregion

        internal MainMenuScreen(GraphicsDevice graphics, ContentManager content, LinkedList<Button> buttons) : base(graphics, buttons) {
            InitializeTextures(content);
            SetInitialButtonFocus();
        }

        #region methods

        /// <summary>
        /// If the user wants to swap the roles, the Menu-Texture has to change
        /// </summary>
        public void SwapRoles() {
            _isGazelleLeft = !_isGazelleLeft;
            SetInitialButtonFocus();

            Texture = _isGazelleLeft ? _gazelleLeft : _gazelleRight;
        }

        protected override void InitializeTextures(ContentManager content) {
            _gazelleLeft = content.Load<Texture2D>(StaticStrings.ImageMainMenuGazelleLeft);
            _gazelleRight = content.Load<Texture2D>(StaticStrings.ImageMainMenuGazelleRight);

            Texture = _isGazelleLeft ? _gazelleLeft : _gazelleRight;
        }

        #endregion
    }
}
