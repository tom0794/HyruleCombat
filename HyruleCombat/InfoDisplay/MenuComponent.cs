using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.InfoDisplay
{
    /// <summary>
    /// Used to a display a menu with options that can be cycled through 
    /// with W and S. Activating a menu option is handled by Game1.
    /// </summary>
    public class MenuComponent : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected SpriteFont regularFont;
        protected SpriteFont highlightFont;
        private SpriteFont titleFont;
        private string title;
        protected string[] menuItems;

        public int SelectedIndex { get; set; }

        protected Vector2 position;
        protected Color regularColour = Color.Black;
        protected Color highlightColour = Color.Yellow;

        private KeyboardState oldState;

        public MenuComponent(Game game,
            SpriteBatch spriteBatch,
            SpriteFont regularFont,
            SpriteFont highlightFont,
            SpriteFont titleFont,
            string title,
            string[] menuItems) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.regularFont = regularFont;
            this.highlightFont = highlightFont;
            this.titleFont = titleFont;
            this.title = title;
            this.menuItems = menuItems;
            position = new Vector2(50, 50);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 tempPos = position;

            spriteBatch.Begin();
            spriteBatch.DrawString(titleFont, title, position, Color.White);
            tempPos.Y += titleFont.LineSpacing + 20;
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (SelectedIndex == i)
                {
                    spriteBatch.DrawString(highlightFont, menuItems[i], tempPos, highlightColour);
                    tempPos.Y += highlightFont.LineSpacing;
                }
                else
                {
                    spriteBatch.DrawString(regularFont, menuItems[i], tempPos, regularColour);
                    tempPos.Y += regularFont.LineSpacing;
                }
            }
            tempPos.Y += regularFont.LineSpacing;
            spriteBatch.DrawString(regularFont, "Press SPACE to select", tempPos, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.S) && oldState.IsKeyUp(Keys.S))
            {
                SelectedIndex++;
                if (SelectedIndex > menuItems.Length - 1)
                {
                    SelectedIndex = 0;
                }
            }
            if (ks.IsKeyDown(Keys.W) && oldState.IsKeyUp(Keys.W))
            {
                SelectedIndex--;
                if (SelectedIndex < 0)
                {
                    SelectedIndex = menuItems.Length - 1;
                }
            }

            oldState = ks;
            base.Update(gameTime);
        }
    }
}
