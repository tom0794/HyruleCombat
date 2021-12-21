using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Sprites
{
    /// <summary>
    /// A DrawableGameComponent with common properties. Most DrawableGameComponents will
    /// inherit from this class.
    /// </summary>
    public class Sprite : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected Texture2D texture;
        public Vector2 position;
        protected Rectangle drawRect;
        protected Vector2 origin;
        public float rotation;
        private float scale = 1;
        protected float moveSpeed;
        private float drawOrder;

        public float Scale { get => scale; set => scale = value; }

        public Sprite(Game game,
            Texture2D texture,
            Vector2 position,
            float drawOrder) : base(game)
        {
            Game1 g = (Game1)game;
            spriteBatch = g._spriteBatch;
            this.texture = texture;
            this.position = position;
            this.drawRect = new Rectangle(0, 0, texture.Width, texture.Height);
            this.drawOrder = drawOrder;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, drawRect, Color.White, rotation, origin, scale, SpriteEffects.None, drawOrder);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public virtual void Remove()
        {
            Enabled = false;
            Visible = false;
            position = new Vector2(-200, -200);
        }
    }
}
