using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Sprites
{
    /// <summary>
    /// Cosmetic component displayed briefly when the player fires an arrow.
    /// </summary>
    public class Bow : Sprite
    {
        private Player player;
        private int originXOffset = -8;
        private int displayTime = 30;
        private int displayCounter = 0;

        public Bow(Game game, 
            Texture2D texture, 
            Vector2 position, 
            float drawOrder,
            Player player) : base(game, texture, position, drawOrder)
        {
            this.player = player;
            origin = new Vector2(originXOffset, texture.Height / 2);
            Scale = 2;
        }

        public override void Draw(GameTime gameTime)
        {
            if (displayCounter < displayTime)
            {
                base.Draw(gameTime);
            }
            else
            {
                displayCounter = 0;
                Remove();
            }
        }

        public override void Update(GameTime gameTime)
        {
            displayCounter++;
            rotation = player.rotation - player.rotationFactor;
            position = player.position;
            base.Update(gameTime);
        }

        public void Display()
        {
            Enabled = true;
            Visible = true;
            position = player.position;
        }
    }
}
