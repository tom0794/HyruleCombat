using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Scenes
{
    /// <summary>
    /// Utilizes ScoreManager to display the list of saved high scores.
    /// The most recently achieved score is displayed in yellow.
    /// </summary>
    public class ScoreScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private List<string[]> scores = new List<string[]>();
        private Vector2 scorePosition;
        private Vector2 titlePosition;
        private Texture2D background;
        private SpriteFont regularFont;
        private SpriteFont titleFont;

        public ScoreScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            spriteBatch = g._spriteBatch;
            scores = ScoreManager.HighScores;

            titlePosition = new Vector2(73, 15);
            scorePosition = new Vector2(75, 96);

            regularFont = g.Content.Load<SpriteFont>("fonts/regularFont");
            titleFont = g.Content.Load<SpriteFont>("fonts/titleFont");
            background = g.Content.Load<Texture2D>("images/titlescreen");
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(titleFont, "High Scores", titlePosition, Color.Black);
            string header = $"Name      Kills       Score";
            spriteBatch.DrawString(regularFont, header, new Vector2(scorePosition.X, scorePosition.Y - regularFont.LineSpacing), Color.White);
            Vector2 tempPos = scorePosition;
            foreach (string[] score in scores)
            {
                string line = $"{score[0]} ----- {int.Parse(score[1]).ToString("D5")} ----- {int.Parse(score[2]).ToString("D9")}";
                Color lineColor = Color.Black;
                if (score.Equals(ScoreManager.NewestScore))
                {
                    lineColor = Color.Yellow;
                }
                spriteBatch.DrawString(regularFont, line, tempPos, lineColor);
                tempPos.Y += regularFont.LineSpacing;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
