/**************************
    Katrin-Anna Zibuschka
    Eat 'Em Up - Multimediaprojekt 1 
    MMT - FH Salzburg
**************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mmp1_zibuschka
{
    class Character : GameObject
    {
        #region members and properties

        internal override Texture2D Texture { get; set; }

        #region position

        internal override Point Position{
            get { return _position; }
            set { _position = value; }
        }

        private Point _position;
        
        /// <summary>
        /// The position, the character takes at every beginning of the game.
        /// </summary>
        protected Point _initialPosition;

        #endregion

        #region dimensions

        internal override Rectangle Rectangle{
            get { return new Rectangle(Position.X, Position.Y, _characterWidth, _characterHeight); }
        }

        private const int _catchingMargin = 5;

        /// <summary>
        /// This rectangle is a bit smaller than the normal character rectangle. 
        /// It's needed because the characters should overlap a little bit before the lion has won.
        /// </summary>
        internal Rectangle CatchingRectangle{
            get
            {
                return new Rectangle(_position.X + _catchingMargin, _position.Y + _catchingMargin,
                    _characterWidth - _catchingMargin, _characterHeight - _catchingMargin);
            }
        }
        
        internal override int Width{
            get { return _characterWidth; }
        }
        
        internal override int Height{
            get { return _characterHeight; }
        }

        private int _characterWidth;
        private int _characterHeight;

        #endregion

        private Vector2 _velocity;

        #region animation

        /// <summary>
        /// The time since the animated picture has last changed.
        /// </summary>
        private float _elapsedTimeAnimation;

        /// <summary>
        /// After this time the next image of the animation is shown
        /// </summary>
        private const float _animationDelay = 50;

        /// <summary>
        /// The current shown frame of the animation
        /// </summary>
        private int _frame;

        #endregion

        #region controls

        private Keys _left;
        private Keys _right;
        private Keys _up;
        private Keys _down;
        private Keys _action;

        #endregion

        #region running

        protected const float _initialRunningSpeed = 0.3f;
        protected float _runningSpeed = 0.3f;

        #endregion

        #region jumping

        /// <summary>
        /// When the character jumps, he shoots up a few pixels -> the jumpingOffset
        /// </summary>
        private const int _jumpingOffset = -25;

        /// <summary>
        /// A negative value for jumping up along the y-axis.
        /// </summary>
        private const int _jumpingSpeed = -14;

        /// <summary>
        /// The character never moves faster than the speed limit
        /// </summary>
        private const int _speedLimit = 15;
        private bool _isJumping;

        #endregion

        #region hiding

        /// <summary>
        /// Determins whether the character hides behind a bush or not.
        /// </summary>
        internal bool IsHiding { get { return _isHiding; } }
        protected bool _isHiding;
        protected bool _canHide;

        #endregion

        #region fast running

        /// <summary>
        /// Determins wheter the character is wearing shoes or not.
        /// </summary>
        internal bool HasShoes { get { return _hasShoes; } }
        protected bool _hasShoes;
        protected bool _isUsingShoes;

        /// <summary>
        /// The factor of which the speed gets increased, if the character uses shoes.
        /// </summary>
        private const float _speedIncrease = 1.6f;

        /// <summary>
        /// The time for how long the character has already been running fast.
        /// </summary>
        private float _elapsedTimeRunningFast;
        /// <summary>
        /// 
        /// Timespan, how long the character is able to run fast
        /// </summary>
        private const float _endurance = 3200;

        #endregion
        
        /// <summary>
        /// This property is overridden by lion and gazelle. Shows if the character has won the game.
        /// </summary>
        internal virtual bool HasWon { get; }

        #endregion

        #region public methods

        public void Initialize(Texture2D texture, Point position, List<KeyValuePair<Controls, Keys>> controls){
            _initialPosition = position;
            Position = position;
            Texture = texture;

            _characterWidth = Texture.Width/16; // because there are 16 different walking states on one texture
            _characterHeight = Texture.Height;

            InitializeControls(controls);
        }

        public void Update(GraphicsDevice graphics, GameTime gameTime){
            var bushes = EatEmUp.GameObjectManager.GameObjects.OfType<Bush>().ToList();

            //determine whether the character is hiding behind a bush...
            if (_canHide && KeyHelper.IsKeyPressed(_action)){
                foreach (var bush in bushes){
                    if (!Rectangle.IsInsideOf(bush.Rectangle))
                        continue;

                    // if the character stands in front of a bush or hides behind a bush and the player presses the action button
                    // the character should hide behind or show up in front of the bush
                    _isHiding = !_isHiding;

                    if(_isHiding)
                        EatEmUp.CharacterManager.OnCharacterUsedBush();

                    break;
                }
            } // ...  or using shoes
            else if (_hasShoes && !_isUsingShoes && KeyHelper.IsKeyPressed(_action) ){
                _isUsingShoes = true;
                _runningSpeed *= _speedIncrease;
            }

            if (!_isHiding){
                UpdatePosition(gameTime);
                UpdateAnimation(gameTime);
                AvoidPositionOutOfBounds(graphics);
            } else{
                UpdatePosition(bushes);
            }

            if (_isUsingShoes){
                UpdateFastRunning(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch){
            if (_isHiding)
                return;

            var destinationRectangle = new Rectangle(Position.X, Position.Y, _characterWidth, _characterHeight);
            var sourceRectangle = new Rectangle(_characterWidth * _frame, 0, _characterWidth, _characterHeight);
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }

        #endregion

        #region internal methods

        internal virtual void Win(){}

        /// <summary>
        /// Resets the character to its inital state
        /// </summary>
        internal virtual void Reset(){}

        /// <summary>
        /// Change the characters controls
        /// </summary>
        /// <param name="controls"> The new controls. </param>
        internal void ResetControls(List<KeyValuePair<Controls, Keys>> controls){
            InitializeControls(controls);
        }

        #endregion

        #region private methods

        private void InitializeControls(List<KeyValuePair<Controls, Keys>> controls)
        {
            foreach (var control in controls) {
                switch (control.Key) {
                    case Controls.Left:
                        _left = control.Value;
                        break;
                    case Controls.Right:
                        _right = control.Value;
                        break;
                    case Controls.Up:
                        _up = control.Value;
                        break;
                    case Controls.Down:
                        _down = control.Value;
                        break;
                    case Controls.Action:
                        _action = control.Value;
                        break;
                    default:
                        throw new Exception("Wrong initialization of controls!");
                }
            }
        }

        /// <summary> 
        /// This method updates the position of the character while hiding behind the bushes
        /// </summary>
        /// <param name="bushes"> A list of all bushes. </param>
        private void UpdatePosition(List<Bush> bushes){
            // Determins whether the character jumps to the next bush clockwise or counter clockwise
            int jumpToBush = 0;

            // if the user presses the left button he changes to the next bush counter clockwise
            // if the user presses the right button he changes to the next bush clockwise
            if (KeyHelper.IsKeyPressed(_left))
                jumpToBush = -1;
            else if (KeyHelper.IsKeyPressed(_right))
                jumpToBush = 1;
            
            // if the user doesn't press left or right, he doesn't want to change the bush -> do nothing
            if (jumpToBush == 0)
                return;

            int bushIndex = -1;
            // determine behind which bush the character is hiding
            for (int i = 0; i < bushes.Count; i++){
                if (Rectangle.IsInsideOf(bushes[i].Rectangle)){
                    bushIndex = i;
                    break;
                }
            }

            bushIndex += jumpToBush;

            // avoid indexOutOfRange
            if (bushIndex < 0)
                bushIndex = bushes.Count - 1;
            else if (bushIndex >= bushes.Count)
                bushIndex = 0;

            var bushRectangle = bushes[bushIndex].Rectangle;

            // set the position to the position of the new bush, the user wants to use for hiding
            Position = new Point(bushRectangle.X + bushRectangle.Width/2 - Width/2, bushRectangle.Y);
        }

        /// <summary>
        /// This method updates the position of the character while he's not hiding behind bushes but running and jumping
        /// </summary>
        /// <param name="gameTime"> The gameTime is needed for the running movement. </param>
        private void UpdatePosition(GameTime gameTime)
        {
            HandleXMovement(gameTime);
            HandleYMovement();

            // Adapt position to current velocity
            _position.X += (int)_velocity.X;
            _position.Y += (int)_velocity.Y;
        }

        /// <summary>
        /// Handle running (movement along the X axis)
        /// </summary>
        private void HandleXMovement(GameTime gameTime){
            if (EatEmUp.CurrentKeyboardState.IsKeyDown(_left)){
                _velocity.X = -_runningSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if (EatEmUp.CurrentKeyboardState.IsKeyDown(_right)) {
                _velocity.X = _runningSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else {
                _velocity.X = 0;
            }
        }

        /// <summary>
        /// Handle falling and jumping of the character (movement along the Y axis)
        /// </summary>
        private void HandleYMovement(){
            // a list of all platforms
            var platforms = EatEmUp.GameObjectManager.GameObjects.OfType<Platform>().ToList();

            if (_velocity.Y < _speedLimit)
                _velocity.Y++; // gravitation (character falls faster and faster but not faster than the maximum speed) 

            // determine whether the character stands on a platform or is falling
            foreach (var platform in platforms) {
                if (!Rectangle.IsOnTopOf(platform.Rectangle))
                    continue;

                // if the character hits a platform, he's definitely not jumping anymore
                _isJumping = false;

                if (KeyHelper.IsKeyPressed(_down)){
                    _position.Y += 10; // fall from platform
                } else{
                    _velocity.Y = 0; // if the character stands on a platform, gravitation is deactivated so he doesn't fall anymore
                    _position.Y = platform.Position.Y - Height;
                    break; // The character can only stand on one platform so there's no need to check if he's standing on any other platform
                }
            }

            // The character shouldn't be able to jump continuously, so he has to press the up button again to jump once more (see previousKeyboardState)
            if (_isJumping || !KeyHelper.IsKeyPressed(_up))
                return;

            // if the character pressed the jump button...
            _isJumping = true;
            _position.Y += _jumpingOffset; // ... shoot him up a few pixels
            _velocity.Y = _jumpingSpeed; // ... and let him fall
        }
        
        /// <summary>
        /// Updates the shown picture, depending on the moving direction an the time since the last animation upate.
        /// </summary>
        private void UpdateAnimation(GameTime gameTime){
            _elapsedTimeAnimation += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            // if the character looks to the left but the right button is pressed, the actual frame is set to the first right animation picture
            if (EatEmUp.CurrentKeyboardState.IsKeyDown(_right) && _frame < 8){
                _frame = 8;
            }
            // if the character looks to the right but the left button is pressed, the actual frame is set to the first left animation picture
            else if (EatEmUp.CurrentKeyboardState.IsKeyDown(_left) && _frame >= 8){
                _frame = 0;
            }
            // if it's the time to change the animation frame and the user presses any control for running left or right, the frame number gets increased
            else if (_elapsedTimeAnimation >= _animationDelay && (EatEmUp.CurrentKeyboardState.IsKeyDown(_right) || EatEmUp.CurrentKeyboardState.IsKeyDown(_left))) {
                if (_frame == 15) {
                    _frame = 8; // animation loop
                }
                else if (_frame == 7) {
                    _frame = 0; // animation loop
                }
                else {
                    _frame++; // next animation frame
                }
                // reset time
                _elapsedTimeAnimation = 0;
            }
        }

        /// <summary>
        /// This method ensures that the character does not go out of bounds, but loops back in at the other side
        /// </summary>
        private void AvoidPositionOutOfBounds(GraphicsDevice graphics) {
            var viewWidth = graphics.Viewport.Width;
            var viewHeight = graphics.Viewport.Height;

            if (Position.X <= -Width)
            {
                _position.X += viewWidth + Width;
            }
            else if (Position.X >= viewWidth)
            {
                _position.X -= viewWidth + Width;
            }
            if (Position.Y <= -Height)
            {
                _position.Y += viewHeight;
            }
            else if (Position.Y >= viewHeight)
            {
                _position.Y -= viewHeight + Height;
            }
        }

        /// <summary>
        /// This method terminates the fast running if the character has run fast for long enough
        /// </summary>
        private void UpdateFastRunning(GameTime gameTime) {
            _elapsedTimeRunningFast += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsedTimeRunningFast < _endurance)
                return;

            EatEmUp.CharacterManager.OnCharacterUsedShoes(); // tell the CharacterManager, that the shoes are worn out and the lion has to take them off
            _isUsingShoes = false;
            _runningSpeed /= _speedIncrease; // reset the running speed
            _elapsedTimeRunningFast = 0;
        }

        #endregion
    }
}
