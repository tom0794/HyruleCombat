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
    /// A HUD element consisting of a label and 20 Hearts representing and 
    /// displaying the player's current life.
    /// </summary>
    public class HealthDisplay : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private string label = "- LIFE -";
        private Texture2D heartTex;
        private Player player;
        public List<Heart> hearts = new List<Heart>();
        private int numHearts = 20;

        public HealthDisplay(Game game,
            SpriteBatch spriteBatch,
            SpriteFont font,
            Texture2D heartTex,
            Player player) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.heartTex = heartTex;
            this.player = player;

            Vector2 heartPosition = new Vector2(555, 30);
            
            for (int i = 0; i < numHearts; i++)
            {
                float minDisplay = i + 1;
                float halfDisplay = i + 0.5f;
                int spacing = 22;
                if (i == numHearts / 2)
                {
                    heartPosition.X = 577;
                    heartPosition.Y = 54;
                }
                else
                {
                    heartPosition.X += spacing;
                }
                Heart heart = new Heart(game, heartTex, heartPosition, player, minDisplay, halfDisplay, 0);
                hearts.Add(heart);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, label, new Vector2(622, 5), Color.Yellow);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
