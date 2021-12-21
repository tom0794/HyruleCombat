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
    /// A moving object that has velocity and can inflict damage.
    /// </summary>
    public class Projectile : Sprite
    {
        private float damage;
        protected Vector2 velocity;
        protected SoundEffect soundEffect;

        public float Damage { get => damage; set => damage = value; }

        public Projectile(Game game, 
            Texture2D texture, 
            Vector2 position,
            Vector2 destination,
            float moveSpeed,
            SoundEffect soundEffect,
            float drawOrder) : base(game, texture, position, drawOrder)
        {
            this.soundEffect = soundEffect;
            this.moveSpeed = moveSpeed;

            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            rotation = VectorTools.GetAngle(this.position, destination);
            velocity = VectorTools.GetVelocity(rotation, this.moveSpeed);
        }

        public virtual Rectangle GetHitbox()
        {
            return new Rectangle((int)position.X - drawRect.Width * (int)Scale / 2,
                (int)position.Y - drawRect.Height * (int)Scale / 2,
                drawRect.Width * (int)Scale,
                drawRect.Height * (int)Scale);
        }
    }
}
