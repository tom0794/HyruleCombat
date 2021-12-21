using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.InfoDisplay
{
    /// <summary>
    /// A visual representation of the Player's current health. Hearts can be drawn
    /// as full hearts, half hearts, or empty hearts, depending on the amount of
    /// health the Player has.
    /// </summary>
    public class Heart : Sprite
    {
        private float minDisplay;
        private float halfDisplay;
        private Player player;

        public Heart(Game game, 
            Texture2D texture, 
            Vector2 position, 
            Player player,
            float minDisplay,
            float halfDisplay,
            float drawOrder) : base(game, texture, position, drawOrder)
        {
            this.player = player;
            this.minDisplay = minDisplay;
            this.halfDisplay = halfDisplay;
            Scale = 3f;
            drawRect = new Rectangle(0, 0, 7, 7);
        }

        /// <summary>
        /// Depending on the amound of health the player has, the heart will be displayed
        /// as full, half, or empty. For example, Heart number 10 will be full if the player
        /// has at least 10 health, half if they have exactly 9.5 health, or empty if they 
        /// have less than 9.5 health.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (minDisplay > player.health)
            {
                drawRect.X = 14;
            }
            if (minDisplay < player.health)
            {
                drawRect.X = 0;
            }
            if (player.health == halfDisplay)
            {
                drawRect.X = 7;
            }
            base.Update(gameTime);
        }
    }
}
