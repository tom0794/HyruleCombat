using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Sprites
{
    /// <summary>
    /// An enemy projectile. Travels along a path initially set using two locations.
    /// </summary>
    public class Fireball : Projectile
    {
        public Fireball(Game game, 
            Texture2D texture, 
            Vector2 position, 
            Vector2 destination, 
            float moveSpeed, 
            SoundEffect soundEffect, 
            float drawOrder) : base(game, texture, position, destination, moveSpeed, soundEffect, drawOrder)
        {
            Damage = 1;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            rotation += 0.1f;
            if (position.X >= Shared.stage.X + 10 || position.X <= 0 || position.Y >= Shared.stage.Y + 10 || position.Y <= 0)
            {
                Remove();
            }
            base.Update(gameTime);
        }
    }
}
