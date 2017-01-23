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
    class Gazelle : Character {

        #region members and properties

        internal int CollectedLeaves;

        /// <summary>
        /// the number of leaves the gazelle has to collect to win the game
        /// </summary>
        private const int _neededLeaves = 10;

        internal override bool HasWon{
            get { return CollectedLeaves >= _neededLeaves; }
        }

        private SoundEffect _sound;

        #endregion

        #region internal methods

        internal void Initialize(ContentManager content, Point position, List<KeyValuePair<Controls, Keys>> controls){
            _canHide = true;
            _sound = content.Load<SoundEffect>(StaticStrings.SoundGazelle);
            Initialize(content.Load<Texture2D>(StaticStrings.ImageGazelle), position, controls);
        }

        internal override void Reset(){
            CollectedLeaves = 0;
            _isHiding = false;
            Position = _initialPosition;
        }
        
        internal void CollectLeaf(){
            _sound.Play(0.6f, 0.3f, 0);
            CollectedLeaves++;
        }

        #endregion
    }
}
