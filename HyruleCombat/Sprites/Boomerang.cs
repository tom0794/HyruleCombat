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
    /// Projectile that will return to the Player after flying a limited distance
    /// or colliding with an Enemy or the edge of the stage. Is capable of stunning
    /// enemies.
    /// </summary>
    public class Boomerang : Projectile
    {
        private Vector2 destination;
        public bool returning = false;
        private Player player;
        private float maxDistance = 250f;
        private float soundBufferCounter = 0;
        private float soundBuffer = 15;

        public Boomerang(Game game, 
            Texture2D texture, 
            Vector2 position, 
            Vector2 destination, 
            float moveSpeed, 
            Player player,
            SoundEffect soundEffect,
            float drawOrder) : base(game, texture, position, destination, moveSpeed, soundEffect, drawOrder)
        {
            this.player = player;
            Scale = 2;
            rotation = 0;
            Damage = 0.01f;
            float x = maxDistance * (float)Math.Cos(rotation) + position.X;
            float y = maxDistance * (float)Math.Sin(rotation) + position.Y;
            this.destination = new Vector2(x, y);
            this.destination = VectorTools.GetVelocity(VectorTools.GetAngle(position, destination), maxDistance);
        }

        /// <summary>
        /// When thrown, the boomerang will travel until it reaches its destination
        /// or collides with an enemy, at which point it will be returning. When 
        /// returning, the boomerang chases and eventually reaches the player.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            rotation += 0.5f;
            if (!returning)
            {
                position += velocity;
                if (Math.Abs(position.X - destination.X) < 10 && Math.Abs(position.Y - destination.Y) < 10 ||
                    position.X <= 0 || position.X >= Shared.stage.X ||
                    position.Y <= 0 || position.Y >= Shared.stage.Y)
                {
                    returning = true;
                }
            }
            if (returning)
            {
                float angle = VectorTools.GetAngle(position, player.position);
                velocity = VectorTools.GetVelocity(angle, moveSpeed);
                position += velocity;
                if (Math.Abs(player.position.X - position.X) < 10 &&
                    Math.Abs(player.position.Y - position.Y) < 10)
                {
                    Disable();
                }
            }
            soundBufferCounter++;
            if (soundBufferCounter >= soundBuffer)
            {
                soundEffect.Play();
                soundBufferCounter = 0;
            }
            base.Update(gameTime);
        }

        public void Disable()
        {
            Enabled = false;
            Visible = false;
            position = new Vector2(-90, -90);
            rotation = 0;
            velocity = new Vector2(0, 0);
        }

        public void Activate()
        {
            Enabled = true;
            Visible = true;
            returning = false;
        }

        /// <summary>
        /// Using the player's position, the location of the crosshairs, and the maximum
        /// distance the boomerang can travel, determines the destination of the boomerang
        /// and the velocity.
        /// </summary>
        /// <param name="point">Crosshair location.</param>
        public void SetDestination(Vector2 point)
        {
            this.position = player.position;
            float angle = VectorTools.GetAngle(position, point);
            Vector2 offset = VectorTools.GetVelocity(angle, maxDistance);
            this.destination = position + offset;
            this.velocity = VectorTools.GetVelocity(angle, moveSpeed);
        }
    }
}
