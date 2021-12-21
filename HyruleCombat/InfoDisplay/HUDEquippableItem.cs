using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Scenes;
using HyruleCombat.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.InfoDisplay
{
    /// <summary>
    /// One of the Player's usable weapons, displayed on the HUD. Highlighted if
    /// active, darkened if not selected.
    /// </summary>
    public class HUDEquippableItem : Sprite
    {
        private bool selected;
        private int itemValue;
        private PlayScene currentScene;

        public HUDEquippableItem(Game game, 
            Texture2D texture, 
            Vector2 position,
            PlayScene currentScene,
            int itemValue,
            float drawOrder) : base(game, texture, position, drawOrder)
        {
            this.currentScene = currentScene;
            this.itemValue = itemValue;
        }

        public override void Draw(GameTime gameTime)
        {
            Color drawColour = Color.White;
            if (!selected)
            {
                drawColour = Color.DarkGreen;
            }
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, drawRect, drawColour, rotation, origin, Scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (currentScene.SelectedItem == itemValue)
            {
                selected = true;
            }
            else
            {
                selected = false;
            }
            base.Update(gameTime);
        }
    }
}
