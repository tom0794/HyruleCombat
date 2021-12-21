using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Sprites
{
    /// <summary>
    /// Character controlled by the game Player. Possesses properties such as health 
    /// and invulnerability. Has four sets of frames that allow for walk animations
    /// in four directions.
    /// </summary>
    public class Player : Sprite
    {
        public float rotationFactor = 0;
        public float health = 20;
        public bool invulnerable = false;
        private float invulnTime = 0;
        private float maxInvulnTime = 90;

        private List<Rectangle>[] frameSets = new List<Rectangle>[4];
        private const int FACING_DOWN = 0;
        private const int FACING_UP = 1;
        private const int FACING_RIGHT = 2;
        private const int FACING_LEFT = 3;

        private bool isMoving = false;

        private int frameSetIndex;
        private int frameIndex;
        private int frameBuffer = 4;
        private int frameBufferCounter = 0;

        private SoundEffect hurtSound;
        public SoundEffect HurtSound { get => hurtSound; set => hurtSound = value; }

        public Player(Game game, 
            Texture2D texture, 
            Vector2 position,
            float drawOrder,
            SoundEffect hurtSound) : base(game, texture, position, drawOrder)
        {
            // Load a set of frames for each direction the enemy can face
            frameSets[FACING_RIGHT] = Shared.GetFramesOneRow(new Vector2(128, texture.Height / 4), 8, 0);
            frameSets[FACING_DOWN] = Shared.GetFramesOneRow(new Vector2(texture.Width, texture.Height / 4), 9, 1);
            frameSets[FACING_LEFT] = Shared.GetFramesOneRow(new Vector2(128, texture.Height / 4), 8, 2);
            frameSets[FACING_UP] = Shared.GetFramesOneRow(new Vector2(texture.Width, texture.Height / 4), 9, 3);

            frameIndex = 0;
            frameSetIndex = 0;
            this.drawRect = frameSets[frameSetIndex][frameIndex];

            this.origin = new Vector2(drawRect.Width / 2, drawRect.Height / 2);
            this.Scale = 2;
            this.moveSpeed = 2.2f;

            this.hurtSound = hurtSound;
        }

        public override void Draw(GameTime gameTime)
        {
            // If the player is invulnerable, skip drawing the object on certain frames
            // to display the player as flashing
            if (invulnerable)
            {
                if (invulnTime % 5 != 0)
                {
                    base.Draw(gameTime);
                }
            }
            else
            {
                base.Draw(gameTime);
            }
        }

        /// <summary>
        /// The Player's movement and rotation is updated.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            MouseState ms = Mouse.GetState();
            float angle = VectorTools.GetAngle(new Vector2(position.X, position.Y), new Vector2(ms.X, ms.Y));
            rotationFactor = 0;

            // Change player picture direction
            // rotationFactor compensates for the direction the texture is facing

            if (VectorTools.CheckFacingRight(angle))
            {
                frameSetIndex = FACING_RIGHT;
            }
            if (VectorTools.CheckFacingDown(angle))
            {
                frameSetIndex = FACING_DOWN;
                rotationFactor = -1 * (float)Math.PI / 2;
            }
            if (VectorTools.CheckFacingLeft(angle))
            {
                frameSetIndex = FACING_LEFT;
                rotationFactor = (float)Math.PI;
            }
            if (VectorTools.CheckFacingUp(angle))
            {
                frameSetIndex = FACING_UP;
                rotationFactor = (float)Math.PI / 2;
            }

            // Update rotation if the mouse position is not equal to player position
            if (ms.X != position.X && ms.Y != position.Y)
            {
                rotation = angle + rotationFactor;
            }

            // Check if the player is moving. Update the player's position, and update
            // the information controlling animation frames.
            isMoving = false;
            bool backwards = false;
            if (ks.IsKeyDown(Keys.W))
            {
                isMoving = true;
                position.Y -= moveSpeed;
                if (position.Y - drawRect.Height * Scale / 2 <= 0)
                {
                    position.Y = drawRect.Height * Scale / 2;
                }
            }
            if (ks.IsKeyDown(Keys.S))
            {
                isMoving = true;
                position.Y += moveSpeed;
                if (position.Y + drawRect.Height * Scale / 2 >= Shared.stage.Y)
                {
                    position.Y = Shared.stage.Y - drawRect.Height * Scale / 2;
                }
            }
            if (ks.IsKeyDown(Keys.A))
            {
                isMoving = true;
                position.X -= moveSpeed;
                if (position.X - drawRect.Width * Scale / 2 <= 0)
                {
                    position.X = drawRect.Width * Scale / 2;
                }
                if (VectorTools.CheckFacingRight(angle))
                {
                    backwards = true;
                }
            }
            if (ks.IsKeyDown(Keys.D))
            {
                isMoving = true;
                position.X += moveSpeed;
                if (position.X + drawRect.Width * Scale / 2 >= Shared.stage.X)
                {
                    position.X = Shared.stage.X - drawRect.Width * Scale / 2;
                }
                if (VectorTools.CheckFacingLeft(angle))
                {
                    backwards = true;
                }
            }

            // Show the next frame of animation if the player is moving.
            frameBufferCounter++;
            if (frameBufferCounter >= frameBuffer)
            {
                frameBufferCounter = 0;
                if (isMoving)
                {
                    // If the player is moving forwards increment the frame index positively
                    if (!backwards)
                    {
                        frameIndex = (frameIndex + 1) % frameSets[frameSetIndex].Count;
                    }
                    // If moving backwards subtract from the frame index to reverse the animation
                    else
                    {
                        frameIndex--;
                        if (frameIndex == -1)
                        {
                            frameIndex = frameSets[frameSetIndex].Count - 1;
                        }
                    }
                }
                else
                {
                    frameIndex = 0;
                }
                drawRect = frameSets[frameSetIndex][frameIndex];
            }

            // If the player is invulernable, add to the invulnTime counter until it reaches its
            // max value, at which point the player is no longer invulnerable
            if (invulnerable)
            {
                invulnTime++;
                if (invulnTime >= maxInvulnTime)
                {
                    invulnerable = false;
                    invulnTime = 0;
                }
            }

            base.Update(gameTime);
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle((int)position.X - drawRect.Width / 2, (int)position.Y - drawRect.Height / 2, 
                drawRect.Width, drawRect.Height);
        }
    }
}
