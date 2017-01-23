/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka{
    class PauseScreen : Screen, IShowControls{

        #region members and properties

        internal override Texture2D Texture { get; set; }

        private bool _isGazelleLeft;
        private Texture2D _gazelleLeft;
        private Texture2D _gazelleRight;

        private Texture2D _pauseText;
        private Rectangle _pauseTextRectangle;

        #endregion

        internal PauseScreen(GraphicsDevice graphics, ContentManager content, LinkedList<Button> buttons) : base(graphics, buttons) {
            InitializeTextures(content);
            SetInitialButtonFocus();
        }

        #region methods

        public override void Draw(SpriteBatch spriteBatch){
            base.Draw(spriteBatch);
            spriteBatch.Draw(_pauseText, _pauseTextRectangle, Color.White);
        }

        /// <summary>
        /// If the user wants to swap the roles, the Menu-Texture has to change
        /// </summary>
        public void SwapRoles(){
            _isGazelleLeft = !_isGazelleLeft;
            Texture = _isGazelleLeft ? _gazelleLeft : _gazelleRight;
        }

        protected override void InitializeTextures(ContentManager content){
            _gazelleLeft = content.Load<Texture2D>(StaticStrings.ImageMainMenuGazelleLeft);
            _gazelleRight = content.Load<Texture2D>(StaticStrings.ImageMainMenuGazelleRight);

            Texture = _isGazelleLeft ? _gazelleLeft : _gazelleRight;

            _pauseText = content.Load<Texture2D>(StaticStrings.ImagePause);

            var size = new Rectangle(0, 0, (int) (_pauseText.Width*1.5f), (int) (_pauseText.Height*1.5f));
            var position = new Vector2(Rectangle.Width/2 - size.Width/2, 230);
            _pauseTextRectangle = new Rectangle((int) position.X, (int) position.Y, size.Width, size.Height);
        }

        #endregion
    }
}