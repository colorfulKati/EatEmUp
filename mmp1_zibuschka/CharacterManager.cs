/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mmp1_zibuschka {
    class CharacterManager {

        #region members and properties

        private Lion _lion;
        private Gazelle _gazelle;

        private List<KeyValuePair<Controls, Keys>> _controlsGazelle;
        private List<KeyValuePair<Controls, Keys>> _controlsLion;

        /// <summary>
        /// List of all characters (so just one gazelle and one lion)
        /// </summary>
        internal static List<Character> Characters;

        #endregion

        #region internal methods

        internal void Initialize(GraphicsDevice graphicsDevice, ContentManager content) {

            _lion = new Lion();
            _gazelle = new Gazelle();
            Characters = new List<Character> { _gazelle, _lion };
            
            _controlsLion = new List<KeyValuePair<Controls, Keys>> {
                new KeyValuePair<Controls, Keys>(Controls.Up, Keys.W),
                new KeyValuePair<Controls, Keys>(Controls.Down, Keys.S),
                new KeyValuePair<Controls, Keys>(Controls.Left, Keys.A),
                new KeyValuePair<Controls, Keys>(Controls.Right, Keys.D),
                new KeyValuePair<Controls, Keys>(Controls.Action, Keys.LeftControl)
            };

            _lion.Initialize(content, new Point(0, graphicsDevice.Viewport.Height - 150), _controlsLion);
            
            _controlsGazelle = new List<KeyValuePair<Controls, Keys>> {
                new KeyValuePair<Controls, Keys>(Controls.Up, Keys.Up),
                new KeyValuePair<Controls, Keys>(Controls.Down, Keys.Down),
                new KeyValuePair<Controls, Keys>(Controls.Left, Keys.Left),
                new KeyValuePair<Controls, Keys>(Controls.Right, Keys.Right),
                new KeyValuePair<Controls, Keys>(Controls.Action, Keys.RightControl)
            };

            _gazelle.Initialize(content, new Point(graphicsDevice.Viewport.Width - _lion.Width, 40), _controlsGazelle);
        }
        
        /// <summary>
        /// Resets the character's states and swaps the controls of the characters if requested by the users
        /// </summary>
        /// <param name="swapControls"> True, if the controls of lion and gazelle should be swapped. </param>
        internal void Reset(bool swapControls = false){
            foreach (var character in Characters){
                character.Reset();
            }

            if (swapControls){
                var helper = _controlsGazelle;
                _controlsGazelle = _controlsLion;
                _controlsLion = helper;

                _lion.ResetControls(_controlsLion);
                _gazelle.ResetControls(_controlsGazelle);
            }
        }

        /// <summary>
        /// Updates the characters
        /// Determines if anyone has achieved their goal
        /// Determines if the gazelle is able to collect leaves
        /// Determines if the lion is able to collect shoes
        /// </summary>
        internal void Update(GraphicsDevice graphicsDevice, GameTime gameTime) {
            if (!_gazelle.IsHiding && _lion.CatchingRectangle.Intersects(_gazelle.CatchingRectangle))
                _lion.Win();

            if (_gazelle.CatchingRectangle.IntersectsAnyObject(EatEmUp.GameObjectManager.GameObjects.OfType<Leaf>())) {
                _gazelle.CollectLeaf();
                EatEmUp.GameObjectManager.RepositionLeaves();
            }

            if (_lion.Rectangle.IntersectsAnyObject(EatEmUp.GameObjectManager.GameObjects.OfType<Shoes>())) {
                _lion.CollectShoes();
                EatEmUp.GameObjectManager.HideShoes();
            }

            foreach (var character in Characters) {
                character.Update(graphicsDevice, gameTime);
            }
        }

        /// <summary>
        /// Draws all characters.
        /// </summary>
        internal void Draw(SpriteBatch spriteBatch) {
            foreach (var character in Characters) {
                character.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Determines if someone has won.
        /// </summary>
        /// <returns> True, if someone has won. </returns>
        internal bool SomeoneHasWon() {
            return Characters.Any(character => character.HasWon);
        }

        /// <summary>
        /// Determines who has won.
        /// </summary>
        /// <returns> The winner. </returns>
        internal Winner WhoHasWon() {
            if (_lion.HasWon)
                return Winner.Lion;

            if (_gazelle.HasWon)
                return Winner.Gazelle;

            return Winner.Nobody;
        }

        /// <summary>
        /// Determines how many leaves were already collected by the gazelle 
        /// </summary>
        /// <returns> The amount of collected leaves. </returns>
        internal int GetCollectedLeaves(){
            return _gazelle.CollectedLeaves;
        }

        /// <summary>
        /// Handles the situation after a character has worn out his shoes.
        /// </summary>
        internal void OnCharacterUsedShoes(){
            _lion.TakeOffShoes();
        }

        /// <summary>
        /// Handles the situation when a character enters a bush.
        /// Everytime the gazelle hides behind a bush and the lion has no shoes, new shoes should appear.
        /// </summary>
        internal void OnCharacterUsedBush(){
            if (_lion.HasShoes)
                return;

            EatEmUp.GameObjectManager.GenerateShoes();
        }

        #endregion
    }
}
