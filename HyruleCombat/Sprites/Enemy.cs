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
    /// Object that has set behavior and properties, can inflict damage on the
    /// player, and receives damage from player attacks. Can also be stunned by
    /// the boomerang, during which time it cannot move. Is briefly invulernable
    /// after taking damage.
    /// </summary>
    public class Enemy : Sprite
    {
        private float health;
        private float damage;
        private int scoreValue;
        private bool invulnerable = false;
        protected float invulnTime = 0;
        protected float maxInvulnTime = 90;
        protected Vector2 velocity;
        // Zone is a rectangle within which the enemy will not leave
        protected Rectangle zone;
        protected EnemySpawner spawner;
        private SoundEffect deathSound;

        private bool stunned = false;
        private int stunCounter = 0;
        private int stunMaxValue = 100;

        public bool Invulnerable { get => invulnerable; set => invulnerable = value; }
        public float Health { get => health; set => health = value; }
        public float Damage { get => damage; set => damage = value; }
        public SoundEffect DeathSound { get => deathSound; set => deathSound = value; }
        public int ScoreValue { get => scoreValue; set => scoreValue = value; }
        public bool Stunned { get => stunned; set => stunned = value; }
        public int StunCounter { get => stunCounter; set => stunCounter = value; }
        public int StunMaxValue { get => stunMaxValue; set => stunMaxValue = value; }

        public Enemy(Game game,
            Texture2D texture,
            Vector2 position,
            Vector2 velocity,
            Rectangle zone,
            EnemySpawner spawner,
            float drawOrder,
            SoundEffect deathSound) : base(game, texture, position, drawOrder)
        {
            origin = new Vector2(drawRect.Width / 2, drawRect.Height / 2);
            this.zone = zone;
            this.velocity = velocity;
            this.spawner = spawner;
            this.deathSound = deathSound;
        }

        public override void Draw(GameTime gameTime)
        {
            // If the enemy is invulnerable, skip drawing the object on certain frames
            // to display the enemy as flashing
            if (invulnerable)
            {
                if (invulnTime % 5 != 0)
                {
                    base.Draw(gameTime);
                }
            }
            else
            {
                base.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (invulnerable)
            {
                invulnTime++;
                if (invulnTime >= maxInvulnTime)
                {
                    invulnerable = false;
                    invulnTime = 0;
                }
            }
            if (stunned)
            {
                stunCounter++;
                if (stunCounter >= StunMaxValue)
                {
                    stunCounter = 0;
                    stunned = false;
                }
            }
            base.Update(gameTime);
        }

        public override void Remove()
        {
            spawner.ActiveEnemies--;
            base.Remove();
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
