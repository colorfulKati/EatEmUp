/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace mmp1_zibuschka{

    /// <summary>
    /// The controls, a character has.
    /// </summary>
    public enum Controls{
        Left,
        Right,
        Up,
        Down,
        Action
    }

    /// <summary>
    /// The possible states of the game.
    /// </summary>
    public enum GameState{
        MainMenu,
        Playing,
        Pause,
        End,
        Credits
    }

    /// <summary>
    /// The Winner (gazelle or lion)
    /// </summary>
    public enum Winner{
        Lion,
        Gazelle,
        Nobody
    }

    public static class GameStateExtensions{
        public static string ToString(this GameState gameState){
            switch (gameState){
                case GameState.MainMenu:
                    return StaticStrings.GameStateMainMenu;
                case GameState.Pause:
                    return StaticStrings.GameStatePause;
                case GameState.End:
                    return StaticStrings.GameStateEnd;
                case GameState.Credits:
                    return StaticStrings.GameStateCredits;
                default:
                    return StaticStrings.GameStatePlaying;
            }
        }
    }

    /// <summary>
    /// Some strings that are needed for loading the images and fonts .
    /// </summary>
    public static class StaticStrings{
        private static string ImageFolder = "images\\";
        // Characters
        public static string ImageGazelle = ImageFolder + "gazelle";
        public static string ImageLion = ImageFolder + "lion";
        public static string ImageLionWithShoes = ImageFolder + "lionShoes";
        // GameObjects
        public static string ImageLeaf = ImageFolder + "leaf";
        public static string ImageShoes = ImageFolder + "shoes";
        public static string ImageBush = ImageFolder + "bush";
        // Platforms
        public static string ImagePlatformShort = ImageFolder + "platformShort";
        public static string ImagePlatformMedium = ImageFolder + "platformMedium";
        public static string ImagePlatformLong1 = ImageFolder + "platformLong";
        public static string ImagePlatformLong2 = ImageFolder + "platformReallyLong";
        public static string ImagePlatformLong3 = ImageFolder + "platformReallyReallyLong";
        // Buttons
        public static string ImageButtonStartGame = ImageFolder + "buttonStartGame";
        public static string ImageButtonResume = ImageFolder + "buttonResume";
        public static string ImageButtonSwapRoles = ImageFolder + "buttonSwapRoles";
        public static string ImageButtonCredits = ImageFolder + "buttonCredits";
        public static string ImageButtonMainMenu = ImageFolder + "buttonMainMenu";
        // Screens
        public static string ImageCredits = ImageFolder + "Credits";
        public static string ImageMainMenuGazelleLeft = ImageFolder + "MainMenuGazelleLeft";
        public static string ImageMainMenuGazelleRight = ImageFolder + "MainMenuGazelleRight";
        public static string ImageWinnerGazelle = ImageFolder + "WinnerGazelle";
        public static string ImageWinnerLion = ImageFolder + "WinnerLion";
        // TextImages
        public static string ImagePause = ImageFolder + "Pause";
        // Fonts
        private static string FontFolder = "fonts\\";
        public static string TextFont = FontFolder + "TextFont";
        // Sounds
        private static string SoundFolder = "sounds\\";
        public static string SoundGazelle = SoundFolder + "GazelleMunch";
        public static string SoundLion = SoundFolder + "LionRoar";
        // GameStates
        public static string GameStateMainMenu = "MainMenu";
        public static string GameStatePause = "Pause";
        public static string GameStateEnd = "End";
        public static string GameStateCredits = "Credits";
        public static string GameStatePlaying = "Playing";
    }

    /// <summary>
    /// The KeyHelper is to determine if a key was just pressed or released.
    /// It's needed because the XNA keyboardstate class has no methods to check this cases.
    /// </summary>
    public static class KeyHelper{

        internal static bool IsKeyPressed(Keys key) {
            return EatEmUp.CurrentKeyboardState.IsKeyDown(key) && EatEmUp.PreviousKeyboardState.IsKeyUp(key);
        }

        internal static bool IsKeyReleased(Keys key) {
            return EatEmUp.PreviousKeyboardState.IsKeyDown(key) && EatEmUp.CurrentKeyboardState.IsKeyUp(key);
        }
    }

    /// <summary>
    /// The RectangleHelper is needed for some types of collision detection.
    /// </summary>
    public static class RectangleHelper{

        /// <summary>
        /// This method determines whether the first rectangle is on top of the other rectangle, given a small scope above the second rectangle.
        /// </summary>
        /// <param name="r1"> The rectangle, which should be checked whether it's above the second or not. </param>
        /// <param name="r2"> The second rectangle. </param>
        /// <returns> True, if the first rectangle is located directly above the second rectangle. </returns>
        public static bool IsOnTopOf(this Rectangle r1, Rectangle r2){
            const int penetrationMargin = 15;

            return r1.Bottom >= r2.Top - penetrationMargin &&
                   r1.Bottom <= r2.Top &&
                   r1.Right >= r2.Left + penetrationMargin &&
                   r1.Left <= r2.Right - penetrationMargin;
        }
        
        /// <summary>
        /// This method determines whether the rectangle intersects any of the passed GameObjects
        /// </summary>
        /// <param name="bounds"> The rectangle, which should be checked. </param>
        /// <param name="gameObjects"> The relevant GameObjects. </param>
        /// <returns> True, if the rectangle intersects any of the GameObjects. </returns>
        public static bool IntersectsAnyObject(this Rectangle bounds, IEnumerable<GameObject> gameObjects){
            return gameObjects.Any(gameObject => gameObject.Rectangle.Intersects(bounds));
        }

        /// <summary>
        /// This method determines, whether the first rectangle not only intersects the second but also is completely inside of the second.
        /// </summary>
        /// <param name="r1"> The rectangle, which should be checked whether it's inside the second or not. </param>
        /// <param name="r2"> The second rectangle. </param>
        /// <returns> True, if the first rectangle is completely inside the second rectangle. </returns>
        public static bool IsInsideOf(this Rectangle r1, Rectangle r2){
            return r1.Left >= r2.Left && r1.Right <= r2.Right && r1.Bottom <= r2.Bottom && r1.Top >= r2.Top;
        }
    }
}
