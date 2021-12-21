using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.InfoDisplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Scenes
{
    /// <summary>
    /// A scene that displays a list of options selectable by the Player.
    /// Two instances of this classed are used in Game1 (main menu and
    /// pause menu).
    /// </summary>
    public class MenuScene : GameScene
    {
        private MenuComponent menu;
        private SpriteBatch spriteBatch;
        private string[] menuItems;
        private Texture2D background;

        public MenuComponent Menu { get => menu; set => menu = value; }

        public MenuScene(Game game, string[] menuItems, string title) : base(game)
        {
            Game1 g = (Game1)game;
            this.spriteBatch = g._spriteBatch;
            this.menuItems = menuItems;
            
            SpriteFont regularFont = g.Content.Load<SpriteFont>("fonts/regularFont");
            SpriteFont highlightFont = g.Content.Load<SpriteFont>("fonts/highlightFont");
            SpriteFont titleFont = g.Content.Load<SpriteFont>("fonts/titleFont");
            background = g.Content.Load<Texture2D>("images/titlescreen");

            menu = new MenuComponent(g, spriteBatch, regularFont, highlightFont, titleFont, title, menuItems);
            this.Components.Add(menu);

        }

        public override void Draw(GameTime gameTime)
        {
            // Load menu background
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
