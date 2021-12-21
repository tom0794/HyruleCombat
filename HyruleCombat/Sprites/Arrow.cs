using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Sprites
{
    /// <summary>
    /// A type of projectile that moves along a set straight path.
    /// </summary>
    public class Arrow : Projectile
    {
        public Arrow(Game game, 
            Texture2D texture,
            Vector2 position, 
            Vector2 destination, 
            float moveSpeed,
            SoundEffect soundEffect,
            float drawOrder
            ) : base(game, texture, position, destination, moveSpeed, soundEffect, drawOrder)
        {
            Damage = 1;
            Scale = 2;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            if (position.X >= Shared.stage.X + 10 || position.X <= 0 || position.Y >= Shared.stage.Y + 10 || position.Y <= 0)
            {
                Remove();
            }
            base.Update(gameTime);
        }

        public override Rectangle GetHitbox()
        {
            // The arrow hitbox is a 1 x 1 rectangle at its tip
            float arrowTipX = position.X + (texture.Width * (float)Math.Cos(rotation));
            float arrowTipY = position.Y + (texture.Width * (float)Math.Sin(rotation));
            return new Rectangle((int)arrowTipX, (int)arrowTipY, 1, 1);
        }
    }
}
