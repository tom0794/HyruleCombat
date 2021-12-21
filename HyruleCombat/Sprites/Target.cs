using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Sprites
{
    /// <summary>
    /// Basic enemy that is either stationary or moves within a set rectangle.
    /// </summary>
    public class Target : Enemy
    {
        public Target(Game game, 
            Texture2D texture, 
            Vector2 position,
            Vector2 velocity,
            Rectangle zone,
            float moveSpeed,
            EnemySpawner spawner,
            float drawOrder,
            SoundEffect deathSound) : base(game, texture, position, velocity, zone, spawner, drawOrder, deathSound)
        {
            Health = 0.01f;
            Damage = 0;
            ScoreValue = 150;
            this.moveSpeed = moveSpeed;
            this.velocity = new Vector2(moveSpeed * velocity.X, moveSpeed * velocity.Y);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Stunned)
            {
                position += velocity;
                if (position.X >= zone.X + zone.Width)
                {
                    position.X = zone.X + zone.Width;
                    velocity.X *= -1;
                }
                if (position.X <= zone.X)
                {
                    position.X = zone.X;
                    velocity.X *= -1;
                }
                if (position.Y >= zone.Y + zone.Height - drawRect.Height)
                {
                    position.Y = zone.Y + zone.Height - drawRect.Height;
                    velocity.Y *= -1;
                }
                if (position.Y <= zone.Y)
                {
                    position.Y = zone.Y;
                    velocity.Y *= -1;
                } 
            }
            base.Update(gameTime);
        }

    }
}
