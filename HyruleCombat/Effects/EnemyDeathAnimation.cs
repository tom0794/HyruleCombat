using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Scenes;
using HyruleCombat.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Effects
{
    /// <summary>
    /// Animation effect to be played when an Enemy dies.
    /// </summary>
    public class EnemyDeathAnimation : Sprite
    {
        private Vector2 dimensions;
        private List<Rectangle> frames;
        private int frameIndex = -1;
        private int delay;
        private int delayCounter;
        private int rows;
        private int cols;
        private PlayScene currentScene;

        public EnemyDeathAnimation(Game game, 
            Texture2D texture, 
            Vector2 position, 
            float drawOrder,
            int delay,
            int rows,
            int cols,
            PlayScene currentScene) : base(game, texture, position, drawOrder)
        {
            this.delay = delay;

            this.rows = rows;
            this.cols = cols;
            this.currentScene = currentScene;

            Scale = 2;
            origin = new Vector2((texture.Width / cols) / 2, (texture.Height / rows) / 2);
            dimensions = new Vector2(texture.Width / cols, texture.Height / rows);
            Hide();
            CreateFrames();
        }

        public void Hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public void Restart()
        {
            frameIndex = -1;
            delayCounter = 0;
            this.Enabled = true;
            this.Visible = true;
        }

        /// <summary>
        /// Using the dimensions of the texture a list of draw rectangles representing
        /// each frame can be obtained.
        /// </summary>
        private void CreateFrames()
        {
            frames = new List<Rectangle>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int x = j * (int)dimensions.X;
                    int y = i * (int)dimensions.Y;
                    Rectangle rect = new Rectangle(x, y, (int)dimensions.X, (int)dimensions.Y);
                    frames.Add(rect);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (frameIndex >= 0)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, position, frames[frameIndex], Color.White, 0, origin, Scale, SpriteEffects.None, 1);
                spriteBatch.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            delayCounter++;
            if (delayCounter > delay)
            {
                frameIndex++;
                if (frameIndex == frames.Count)
                {
                    frameIndex = -1;
                    Remove();
                }
                delayCounter = 0;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Effect is removed from the scene's component list after completing.
        /// </summary>
        public override void Remove()
        {
            Enabled = false;
            Visible = false;
            currentScene.Components.Remove(this);
        }
    }
}
