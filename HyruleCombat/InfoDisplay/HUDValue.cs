using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.InfoDisplay
{
    /// <summary>
    /// An updating HUD element (score or kill count)
    /// </summary>
    public class HUDValue : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        public int value;
        private string numberFormat;
        private Vector2 position;
        private Color colour = Color.Yellow;

        public HUDValue(Game game, 
            SpriteBatch spriteBatch, 
            SpriteFont font, 
            int value,
            string numberFormat,
            Vector2 position) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.value = value;
            this.position = position;
            this.numberFormat = numberFormat;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, value.ToString(numberFormat), position, colour);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
