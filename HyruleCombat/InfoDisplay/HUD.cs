using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.InfoDisplay
{
    /// <summary>
    /// HUD (Heads Up Display) shows the Player's equipped item, score and 
    /// number of enemies killed.
    /// </summary>
    public class HUD : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        private string labelScore = "Score";
        private Texture2D backgroundScore;
        private HUDValue scoreValue;

        private string labelKills = "Kills";
        private Texture2D backgroundKills;
        private HUDValue killCount;
        private Game1 g;

        private HUDEquippableItem boomerangIcon;
        private HUDEquippableItem bowIcon;

        private PlayScene currentScene;

        public HUDValue KillCount { get => killCount; set => killCount = value; }
        public HUDValue ScoreValue { get => scoreValue; set => scoreValue = value; }
        public HUDEquippableItem BoomerangIcon { get => boomerangIcon; set => boomerangIcon = value; }
        public HUDEquippableItem BowIcon { get => bowIcon; set => bowIcon = value; }

        public HUD(Game game,
            SpriteBatch spriteBatch,
            SpriteFont font,
            Texture2D backgroundScore,
            Texture2D backgroundKills,
            PlayScene currentScene,
            int boomerangValue,
            int bowValue) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.backgroundScore = backgroundScore;
            this.backgroundKills = backgroundKills;
            this.currentScene = currentScene;
            Game1 g = (Game1)game;
            this.g = g;
            string numberFormatScore = "D9";
            string numberFormatKills = "D5";
            scoreValue = new HUDValue(g, spriteBatch, font, 0, numberFormatScore, new Vector2(5, 436 + font.LineSpacing));
            killCount = new HUDValue(g, spriteBatch, font, 0, numberFormatKills, new Vector2(714, 436 + font.LineSpacing));

            Texture2D boomerangIconTex = g.Content.Load<Texture2D>("images/hudBoomerang");
            Texture2D bowIconTex = g.Content.Load<Texture2D>("images/hudBow");
            boomerangIcon = new HUDEquippableItem(g, boomerangIconTex, new Vector2(5, 5), currentScene, boomerangValue, 1);
            bowIcon = new HUDEquippableItem(g, bowIconTex, new Vector2(50, 5), currentScene, bowValue, 1);
            boomerangIcon.Scale = 2;
            BowIcon.Scale = 2;
        }

        /// <summary>
        /// The equippable items are objects that draw/update themselves. The HUD draws
        /// the score and kill counters.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            // Draw score
            spriteBatch.Draw(backgroundScore, new Vector2(0, 432), Color.White);
            spriteBatch.DrawString(font, labelScore, new Vector2(5, 436), Color.Yellow);
            // Draw kill counter
            spriteBatch.Draw(backgroundKills, new Vector2(709, 432), Color.White);
            spriteBatch.DrawString(font, labelKills, new Vector2(714, 436), Color.Yellow);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Using the current scene, the score and kill values are updated.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            scoreValue.value = currentScene.Score;
            killCount.value = currentScene.KillCounter;
            base.Update(gameTime);
        }
    }
}
