/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mmp1_zibuschka {
    
    public class EatEmUp : Game {

        #region members and properties

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        internal static ScreenManager ScreenManager;
        internal static CharacterManager CharacterManager;
        internal static GameObjectManager GameObjectManager;
        
        internal static KeyboardState CurrentKeyboardState;
        internal static KeyboardState PreviousKeyboardState;

        private static GameState _currentGameState;

        internal static GameState CurrentGameState{
            get { return _currentGameState; }
            set
            {
                PreviousGameState = _currentGameState;
                _currentGameState = value;
                ScreenManager.SetButtonFocus(); // if the gameState changes, the focused button has to be updated.
            }
        }
    
        internal static GameState PreviousGameState;

        internal static Color BackgroundColor = Color.White;

        /// <summary>
        /// To determine whether the users want to swap their roles for the next game or not.
        /// </summary>
        private bool _swappedRoles;

        #endregion

        public EatEmUp() {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 600;
            Window.Title = "Eat 'Em Up";
            
            Content.RootDirectory = "Content";
        }

        #region overridden methods
        
        protected override void Initialize(){
            ScreenManager = new ScreenManager();
            CharacterManager = new CharacterManager();
            GameObjectManager = new GameObjectManager();
            
            base.Initialize();
            
            CurrentGameState = GameState.MainMenu;
        }
        
        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager.Initialize(GraphicsDevice, Content);
            CharacterManager.Initialize(GraphicsDevice, Content);
            GameObjectManager.Initialize(GraphicsDevice, Content);
        }
        
        protected override void Update(GameTime gameTime){
            UpdateKeyboardState();
            UpdateGameState();

            ScreenManager.Update();

            if (CurrentGameState == GameState.Playing){
                CharacterManager.Update(GraphicsDevice, gameTime);
                GameObjectManager.Update();
            }
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(BackgroundColor);
            
            _spriteBatch.Begin();

            switch (CurrentGameState){
                case GameState.Playing: 
                    // Draw platforms, leaves, bushes, shoes and the leaf counter
                    GameObjectManager.Draw(_spriteBatch);

                    // Draw characters
                    CharacterManager.Draw(_spriteBatch);
                    break;
                default:
                    // Draw the menues, buttons,...
                    ScreenManager.Draw(_spriteBatch);
                    break;
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region private methods

        private void UpdateKeyboardState() {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }
        
        private void UpdateGameState(){
            Button focusedButton;
            // getting the current focused button, depending on any pressed keys
            if (KeyHelper.IsKeyPressed(Keys.Up) || KeyHelper.IsKeyPressed(Keys.W))
                focusedButton = ScreenManager.GetFocusedButton(Keys.Up);
            else if (KeyHelper.IsKeyPressed(Keys.Down) || KeyHelper.IsKeyPressed(Keys.S))
                focusedButton = ScreenManager.GetFocusedButton(Keys.Down);
            else
                focusedButton = ScreenManager.GetFocusedButton();

            switch (CurrentGameState){
                case GameState.Playing:
                    // pause
                    if (KeyHelper.IsKeyPressed(Keys.P) || KeyHelper.IsKeyPressed(Keys.Escape))
                        CurrentGameState = GameState.Pause;
                    // show winner
                    else if (CharacterManager.SomeoneHasWon())
                        CurrentGameState = GameState.End;
                    break;
                case GameState.Pause:
                    // end game
                    if (KeyHelper.IsKeyPressed(Keys.Escape))
                        Exit();
                    // continue playing
                    else if (KeyHelper.IsKeyPressed(Keys.P))
                        CurrentGameState = GameState.Playing;
                    else if (KeyHelper.IsKeyPressed(Keys.Enter) || KeyHelper.IsKeyPressed(Keys.Space)){
                        // continue playing
                        if (focusedButton == ScreenManager.ButtonResume)
                            CurrentGameState = GameState.Playing;
                        // show credits
                        else if (focusedButton == ScreenManager.ButtonCredits)
                            CurrentGameState = GameState.Credits;
                    }
                    break;
                case GameState.MainMenu:
                    // end game
                    if (CurrentKeyboardState.IsKeyDown(Keys.Escape))
                        Exit();
                    if (KeyHelper.IsKeyPressed(Keys.Enter) || KeyHelper.IsKeyPressed(Keys.Space)){
                        // start game
                        if (focusedButton == ScreenManager.ButtonStartGame){
                            CurrentGameState = GameState.Playing;
                        }
                        // swap roles
                        else if (focusedButton == ScreenManager.ButtonSwapRoles){
                            ScreenManager.SwapRoles();
                            _swappedRoles = !_swappedRoles;
                        }
                        // show credits
                        else if (focusedButton == ScreenManager.ButtonCredits)
                            CurrentGameState = GameState.Credits;
                    }
                    break;
                case GameState.End:
                    // end game
                    if (KeyHelper.IsKeyPressed(Keys.Escape))
                        Exit();
                    if (KeyHelper.IsKeyPressed(Keys.Enter) || KeyHelper.IsKeyPressed(Keys.Space))
                        CurrentGameState = GameState.MainMenu;
                    break;
                case GameState.Credits:
                    // end game
                    if (KeyHelper.IsKeyPressed(Keys.Escape))
                        Exit();
                    if (KeyHelper.IsKeyPressed(Keys.Enter) || KeyHelper.IsKeyPressed(Keys.Space))
                        CurrentGameState = PreviousGameState; // the previous game state could be pause or initial menu
                    break;
            }

            // If the game has to begin now -> reset all character and object settings
            if (CurrentGameState == GameState.Playing && (PreviousGameState == GameState.MainMenu || PreviousGameState == GameState.End)){
                GameObjectManager.Reset();
                CharacterManager.Reset(_swappedRoles);
                _swappedRoles = false;
                
                PreviousGameState = CurrentGameState; // make sure that the PreviousGameState also is Playing, so the Reset just happens once
            }
        }

        #endregion
    }
}
