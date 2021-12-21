using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Effects
{
    /// <summary>
    /// Parallax scrolling semi transparent clouds that can be overlaid on
    /// Play Scenes.
    /// </summary>
    public class CloudsEffect : Sprite
    {
        private Vector2 position2;
        private Vector2 speed;

        public CloudsEffect(Game game, 
            Texture2D texture, 
            Rectangle drawRect,
            Vector2 position,
            Vector2 speed,
            float drawOrder
            ) : base(game, texture, position, drawOrder)
        {
            this.drawRect = drawRect;
            this.position = position;
            this.position2 = new Vector2(position.X + drawRect.Width, position.Y);
            this.speed = speed;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, drawRect, Color.White);
            spriteBatch.Draw(texture, position2, drawRect, Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            position -= speed;
            position2 -= speed;
            if (position.X < -drawRect.Width)
            {
                position.X = position2.X + drawRect.Width;
            }
            if (position2.X < -drawRect.Width)
            {
                position2.X = position.X + drawRect.Width;
            }

            base.Update(gameTime);
        }
    }
}
