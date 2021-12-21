using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Sprites
{
    /// <summary>
    /// A reticle displayed at the cursor location to allow the player to aim.
    /// </summary>
    public class Crosshairs : Sprite
    {
        private const float MAX_SCALE = 2.5f;
        private const float MIN_SCALE = 1.5f;

        public Crosshairs(Game game, 
            Texture2D texture, 
            Vector2 position,
            float drawOrder) : base(game, texture, position, drawOrder)
        {
            this.Scale = 1.5f;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        /// <summary>
        /// Crosshair will enlarge while the right mouse button is pressed.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            MouseState mousePosition = Mouse.GetState(Game.Window);
            position = new Vector2(mousePosition.X, mousePosition.Y);
            if (mousePosition.RightButton == ButtonState.Pressed)
            {
                Scale += 0.5f;
                if (Scale > MAX_SCALE)
                {
                    Scale = MAX_SCALE;
                }
            }
            if (Scale > MIN_SCALE)
            {
                Scale -= 0.2f;
                if (Scale < MIN_SCALE)
                {
                    Scale = MIN_SCALE;
                }
            }
            base.Update(gameTime);
        }
    }
}
