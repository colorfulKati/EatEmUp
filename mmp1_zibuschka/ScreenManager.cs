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
    /// <summary>
    /// This Class manages the update and draw methods of the different menu Screens
    /// </summary>
    class ScreenManager {

        #region members
        
        internal Button ButtonStartGame;
        internal Button ButtonSwapRoles;
        internal Button ButtonCredits;
        internal Button ButtonMainMenu;
        internal Button ButtonResume;
        
        /// <summary>
        /// A list of all GameStates and the associated screens
        /// </summary>
        private List<KeyValuePair<GameState, Screen>> _screens;

        #endregion

        #region internal methods

        internal void Initialize(GraphicsDevice graphics, ContentManager content) {
            InitializeButtons(graphics, content);
            InitializeScreens(graphics, content);
        }
        
        /// <summary>
        /// This method updates the different screens
        /// Here is just one case in which something needs to be updated: if someone has won, the winnerscreen needs to know the winner
        /// </summary>
        internal void Update(){
            var currentGameState = EatEmUp.CurrentGameState;

            switch (currentGameState) {
                case GameState.Playing:
                    EatEmUp.BackgroundColor = Color.CornflowerBlue;
                    break;
                case GameState.End:
                    (_screens.First(screen => screen.Key == GameState.End).Value as WinnerScreen).SetWinner(
                        EatEmUp.CharacterManager.WhoHasWon());
                    break;
            }
        }

        /// <summary>
        /// This method draws the different menues, depending on the current GameState
        /// </summary>
        internal void Draw(SpriteBatch spriteBatch){
            var gameState = EatEmUp.CurrentGameState;

            if (gameState == GameState.Playing)
                return;
            
            _screens.First(screen => screen.Key == gameState).Value.Draw(spriteBatch);
        }
        
        /// <summary>
        /// This method returns the focused button of the currently shown Screen and updates the focused button if up or down were pressed.
        /// </summary>
        /// <param name="pressedKey"> The pressed key. </param>
        /// <returns> The currently focused button of the active screen. </returns>
        internal Button GetFocusedButton(Keys pressedKey = Keys.None) {
            var gameState = EatEmUp.CurrentGameState;

            if (gameState == GameState.Playing)
                return null;

            return _screens.First(screen => screen.Key == gameState).Value.GetFocusedButton(pressedKey);
        }
        
        /// <summary>
        /// If the user wants to swap the roles, the Texture of some Screens has to change
        /// </summary>
        internal void SwapRoles(){
            var screens = _screens.Where(screen => screen.Value is IShowControls);

            foreach (var screen in screens){
                (screen.Value as IShowControls).SwapRoles();
            }
        }

        /// <summary>
        /// This method updates the focused button, depending on the current GameState
        /// It's called when the GameState changes.
        /// </summary>
        internal void SetButtonFocus() {
            var gameState = EatEmUp.CurrentGameState;

            if (gameState == GameState.Playing)
                return;

            _screens.First(screen => screen.Key == gameState).Value.SetInitialButtonFocus();
        }

        #endregion

        #region private methods
        
        private void InitializeButtons(GraphicsDevice graphics, ContentManager content) {

            var texture = content.Load<Texture2D>(StaticStrings.ImageButtonStartGame);

            ButtonStartGame = new Button();
            ButtonStartGame.Initialize(texture, new Point(graphics.Viewport.Width / 2 - texture.Width / 2, 275));

            ButtonResume = new Button();
            ButtonResume.Initialize(content.Load<Texture2D>(StaticStrings.ImageButtonResume), new Point(graphics.Viewport.Width / 2 - texture.Width / 2, 350));

            ButtonSwapRoles = new Button();
            ButtonSwapRoles.Initialize(content.Load<Texture2D>(StaticStrings.ImageButtonSwapRoles), new Point(graphics.Viewport.Width / 2 - texture.Width / 2, 375));

            ButtonCredits = new Button();
            ButtonCredits.Initialize(content.Load<Texture2D>(StaticStrings.ImageButtonCredits), new Point(graphics.Viewport.Width / 2 - texture.Width / 2, 475));

            ButtonMainMenu = new Button();
            ButtonMainMenu.Initialize(content.Load<Texture2D>(StaticStrings.ImageButtonMainMenu), new Point(graphics.Viewport.Width / 2 - texture.Width / 2, 520));
        }

        private void InitializeScreens(GraphicsDevice graphics, ContentManager content) {
            _screens = new List<KeyValuePair<GameState, Screen>>();

            var buttons = new LinkedList<Button>();
            buttons.AddLast(ButtonStartGame);
            buttons.AddLast(ButtonSwapRoles);
            buttons.AddLast(ButtonCredits);
            // MainMenu Screen
            _screens.Add(new KeyValuePair<GameState, Screen>(GameState.MainMenu, new MainMenuScreen(graphics, content, buttons)));

            buttons = new LinkedList<Button>();
            buttons.AddLast(ButtonResume);
            buttons.AddLast(ButtonCredits);
            // Pause Screen
            _screens.Add(new KeyValuePair<GameState, Screen>(GameState.Pause, new PauseScreen(graphics, content, buttons)));

            buttons = new LinkedList<Button>();
            buttons.AddLast(ButtonMainMenu);
            // Winner Screen
            _screens.Add(new KeyValuePair<GameState, Screen>(GameState.End, new WinnerScreen(graphics, content, buttons)));
            // Credits Screen
            _screens.Add(new KeyValuePair<GameState, Screen>(GameState.Credits, new CreditsScreen(graphics, content, buttons)));
        }

        #endregion
    }
}
