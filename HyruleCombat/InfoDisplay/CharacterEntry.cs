using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.InfoDisplay
{
    /// <summary>
    /// A special type of menu used for entering characters for high score 
    /// names. Displays only the selected character. W and S are used to
    /// cycle through characters.
    /// </summary>
    public class CharacterEntry : MenuComponent
    {
        public bool ComponentSelected { get; set; }
        public string selectedValue;

        public CharacterEntry(Game game, 
            SpriteBatch spriteBatch, 
            SpriteFont regularFont, 
            SpriteFont highlightFont, 
            SpriteFont titleFont, 
            string title, 
            string[] menuItems,
            Vector2 position) : base(game, spriteBatch, regularFont, highlightFont, titleFont, title, menuItems)
        {
            this.position = position;
            ComponentSelected = false;
            selectedValue = menuItems[0];
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (ComponentSelected)
            {
                spriteBatch.DrawString(highlightFont, menuItems[SelectedIndex], position, highlightColour);
            }
            else
            {
                spriteBatch.DrawString(highlightFont, menuItems[SelectedIndex], position, regularColour);
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (ComponentSelected)
            {
                base.Update(gameTime);
                selectedValue = menuItems[SelectedIndex];
            }
        }
    }
}
