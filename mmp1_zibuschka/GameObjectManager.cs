/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace mmp1_zibuschka {
    class GameObjectManager {

        #region members

        /// <summary>
        /// A list of all GameObjects but the characters (platforms, bushes, leaves, shoes)
        /// </summary>
        internal List<GameObject> GameObjects;

        private Rectangle _viewBounds;

        private SpriteFont _textFont;
        private Texture2D _leafCounterLeafTexture;
        private Rectangle _leafCounterLeafRectangle;
        private string _leafCounterText;
        private Vector2 _leafCounterTextPosition;
        private Vector2 _leafCounterTextOrigin;

        #endregion

        #region internal methods

        internal void Initialize(GraphicsDevice graphicsDevice, ContentManager content) {
            _viewBounds = graphicsDevice.Viewport.Bounds;
            GameObjects = new List<GameObject>();
            
            LoadBushes(content);
            LoadPlatforms(content);
            LoadLeaves(content);
            LoadShoes(content);
            LoadLeafCounter(content);
        }

        internal void Update(){
            UpdateLeafCounter();
        }
        
        internal void Draw(SpriteBatch spriteBatch){
            // render all objects
            foreach (var gameObject in GameObjects){
                gameObject.Draw(spriteBatch);
            }

            DrawLeafCounter(spriteBatch);
        }

        internal void Reset(){
            RepositionLeaves();
            HideShoes();
        }

        /// <summary>
        /// This method finds a new position for every leaf.
        /// </summary>
        internal void RepositionLeaves() {
            var leaves = GameObjects.OfType<Leaf>();

            var temporaryPosition = new Point(-50, -50);
            foreach (var leaf in leaves) {
                leaf.Position = temporaryPosition;
            }

            foreach (var leaf in leaves) {
                leaf.Position = GenerateLeafPosition(leaf.Texture.Bounds);
            }
        }
        
        /// <summary>
        /// This method hides the shoes (should be called after the lion has collected them).
        /// </summary>
        internal void HideShoes() {
            var shoes = (Shoes)GameObjects.First(item => item.GetType() == typeof(Shoes));
            if (!shoes.IsVisible)
                return;

            shoes.Hide();
        }

        /// <summary>
        /// This method generates new shoes.
        /// </summary>
        internal void GenerateShoes(){
            var shoes = (Shoes) GameObjects.First(item => item.GetType() == typeof (Shoes));
            if (shoes.IsVisible)
                return;

            // In fact the shoes aren't really generated. They just show up at a random location.
            shoes.Show(GenerateShoesPosition(shoes));
        }

        #endregion

        #region private methods

        private void LoadLeafCounter(ContentManager content) {
            _textFont = content.Load<SpriteFont>(StaticStrings.TextFont);
            _leafCounterLeafTexture = content.Load<Texture2D>(StaticStrings.ImageLeaf);
            _leafCounterTextPosition = new Vector2(_viewBounds.Width / 2, 15);
            _leafCounterText =  "0/10";
            _leafCounterTextOrigin = _textFont.MeasureString(_leafCounterText) / 2;
            _leafCounterLeafRectangle = new Rectangle((int)_leafCounterTextPosition.X + (int)_leafCounterTextOrigin.X * 4 / 3,
                     (int)_leafCounterTextPosition.Y - (int)_leafCounterTextOrigin.Y, _leafCounterLeafTexture.Width / 2, _leafCounterLeafTexture.Height / 2);
        }

        private void UpdateLeafCounter(){
            _leafCounterText = EatEmUp.CharacterManager.GetCollectedLeaves() + "/10";
        }

        /// <summary>
        /// Renders the leaf counter (shows how many leaves the gazelle already has collected).
        /// </summary>
        private void DrawLeafCounter(SpriteBatch spriteBatch){
            spriteBatch.DrawString(_textFont, _leafCounterText, _leafCounterTextPosition, Color.Black, 0,
                _leafCounterTextOrigin, 1.1f, SpriteEffects.None, 1.0f);
            spriteBatch.Draw(_leafCounterLeafTexture, _leafCounterLeafRectangle, null, Color.White);
        }

        private void LoadPlatforms(ContentManager content) {

            var platforms = new List<Platform>();

            for (int i = 0; i < 11; i++) {
                platforms.Add(new Platform());
            }

            platforms[0].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformMedium), new Point(-Platform.CollisionOffsetWidth * 2, 100));
            platforms[1].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformLong1), new Point(130, 210));
            platforms[2].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformLong3), new Point(400, 120));
            platforms[3].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformLong3), new Point(-Platform.CollisionOffsetWidth * 2, 320));
            platforms[4].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformMedium), new Point(500, 400));
            platforms[5].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformShort), new Point(620, 310));
            platforms[6].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformShort), new Point(700, 220));
            platforms[7].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformMedium), new Point(610, 530));
            platforms[8].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformLong2), new Point(170, 570));
            platforms[9].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformShort), new Point(-Platform.CollisionOffsetWidth * 2, 520));
            platforms[10].Initialize(content.Load<Texture2D>(StaticStrings.ImagePlatformMedium), new Point(230, 450));

            foreach (var platform in platforms) {
                GameObjects.Add(platform);
            }
        }

        private void LoadBushes(ContentManager content){
            var bushes = new List<Bush>();

            for (int i = 0; i < 4; i++){
                bushes.Add(new Bush());
            }

            var texture = content.Load<Texture2D>(StaticStrings.ImageBush);
            bushes[0].Initialize(texture, new Point(10, 100 - texture.Height + Platform.CollisionOffsetHeight));
            bushes[1].Initialize(texture, new Point(600, 120 - texture.Height + Platform.CollisionOffsetHeight));
            bushes[2].Initialize(texture, new Point(627, 310 - texture.Height + Platform.CollisionOffsetHeight));
            bushes[3].Initialize(texture, new Point(200, 570 - texture.Height + Platform.CollisionOffsetHeight));

            foreach (var bush in bushes){
                GameObjects.Add(bush);
            }
        }
        
        private void LoadLeaves(ContentManager content) {

            var texture = content.Load<Texture2D>(StaticStrings.ImageLeaf);
            var leaves = new List<Leaf>();

            for (int i = 0; i < 4; i++) {
                leaves.Add(new Leaf());

                // Initialize every leaf an place it at a random position
                leaves[i].Initialize(texture, GenerateLeafPosition(texture.Bounds));

                GameObjects.Add(leaves[i]);
            }
        }

        /// <summary>
        /// This method generates a random position for a leaf.
        /// </summary>
        /// <param name="leafSize"> The size of a leaf. </param>
        /// <returns> The new coordinates for the leaf. </returns>
        private Point GenerateLeafPosition(Rectangle leafSize) {
            var r = new Random();
            var leafBounds = leafSize;

            // add a small offset to the bounds of a leaf, so they aren't too close to any other objects
            leafBounds.Width += 15;
            leafBounds.Height += 15;

            bool isValidPosition = false;

            do{
                leafBounds.X = r.Next(0, _viewBounds.Width - leafSize.Width);
                leafBounds.Y = r.Next(0, _viewBounds.Height - leafSize.Height);

                // the position of a leaf must not intersect platforms, bushes, shoes, characters or other leaves 
                if (!leafBounds.IntersectsAnyObject(GameObjects) &&
                    !leafBounds.IntersectsAnyObject(CharacterManager.Characters.Select(character => character as GameObject))){
                    isValidPosition = true;
                }
            } while (!isValidPosition);

            return leafBounds.Location;
        }
        
        private void LoadShoes(ContentManager content) {

            var shoes = new Shoes();
            shoes.Initialize(content.Load<Texture2D>(StaticStrings.ImageShoes));

            GameObjects.Add(shoes);
        }

        /// <summary>
        /// This method generates a random position for the shoes.
        /// </summary>
        /// <param name="shoes"> The shoes. </param>
        /// <returns> The new position for the shoes. </returns>
        private Point GenerateShoesPosition(Shoes shoes){
            Random r = new Random();

            // first a random position is generated.
            var shoesRectangle = new Rectangle(r.Next(0, _viewBounds.Width - shoes.Width),
                r.Next(0, _viewBounds.Height - shoes.Height), shoes.Width, shoes.Height);

            // get a list of all GameObject Rectangles
            var platformRectangles = GameObjects.OfType<Platform>().Select(item => item.Rectangle).ToList();

            var foundPosition = false;
            var fallingCounter = 0;

            // then let the shoes "fall" down until they stand on a platform
            do {
                // if the shoes have fallen through the whole screen once generate a random position for the shoes
                if (fallingCounter >= _viewBounds.Height) {
                    shoesRectangle.X = r.Next(0, _viewBounds.Width - shoes.Width);
                    shoesRectangle.Y = r.Next(0, _viewBounds.Height - shoes.Height);
                    fallingCounter = 0;
                }

                shoesRectangle.Y += 10;
                fallingCounter += 10;

                // if the shoes fell outside of the window bounds, they should come in at the top again. (like the characters)
                if (shoesRectangle.Y >= _viewBounds.Height)
                    shoesRectangle.Y = 0;

                foreach (var platformRectangle in platformRectangles){
                    // if the shoes stand on any platform, a valid position has been found
                    if (shoesRectangle.IsOnTopOf(platformRectangle)){
                        shoesRectangle.Y = platformRectangle.Y - shoesRectangle.Height;
                        foundPosition = true;
                        break;
                    }
                }

            } while (!foundPosition);

            return shoesRectangle.Location;
        }

        #endregion
    }
}
