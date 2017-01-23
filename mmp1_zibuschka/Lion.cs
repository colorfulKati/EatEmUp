/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mmp1_zibuschka {
    class Lion : Character{

        #region members and properties

        private bool _hasCaughtGazelle;

        /// <summary>
        /// The texture of the lion if it's wearing shoes.
        /// </summary>
        private Texture2D _withShoes;

        /// <summary>
        /// The standard texture of the lion.
        /// </summary>
        private Texture2D _withoutShoes;
        
        internal override bool HasWon {
            get { return _hasCaughtGazelle; }
        }

        private SoundEffect _sound;

        #endregion

        #region internal methods

        internal void Initialize(ContentManager content, Point position, List<KeyValuePair<Controls, Keys>> controls){
            _canHide = false;
            _sound = content.Load<SoundEffect>(StaticStrings.SoundLion);
            _withShoes = content.Load<Texture2D>(StaticStrings.ImageLionWithShoes);
            _withoutShoes = content.Load<Texture2D>(StaticStrings.ImageLion);
            Initialize(_withoutShoes, position, controls);
        }

        internal override void Reset() {
            _hasCaughtGazelle = false;
            _hasShoes = false;
            _isUsingShoes = false;
            _runningSpeed = _initialRunningSpeed;
            Texture = _withoutShoes;
            Position = _initialPosition;
        }

        internal override void Win(){
            _sound.Play(1.0f, -0.2f, 0);
            _hasCaughtGazelle = true;
        }
        
        internal void CollectShoes(){
            _hasShoes = true;
            Texture = _withShoes;
        }

        internal void TakeOffShoes(){
            _hasShoes = false;
            Texture = _withoutShoes;
        }

        #endregion
    }
}
