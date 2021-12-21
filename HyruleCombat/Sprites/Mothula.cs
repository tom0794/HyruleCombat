using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Logic;
using HyruleCombat.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Sprites
{
    /// <summary>
    /// A powerful enemy featuring different attack patterns. Is capable of creating/firing 
    /// projectiles at the player. Note that unlike other Enemies, Mothula cannot be stunned.
    /// </summary>
    public class Mothula : Enemy
    {
        private Game1 g;
        private List<Rectangle> frames;
        private int frameIndex;
        private int frameBuffer = 5;
        private int frameBufferCounter = 0;
        private PlayScene currentScene;

        private int chargeTimerCounter = 0;
        private int chargeTimeInterval = 300;
        private bool charging = false;

        private int projectileTimerCounter = 0;
        private int projectileTimeInterval = 120;
        private bool firing = false;

        private int projectileBufferCounter = 0;
        private int projectileBuffer = 15;
        private int numberOfProjectiles = 0;
        private int maxProjectiles = 5;
        private int projectileSpeed = 7;
        private Texture2D projectileTex;
        private SoundEffect fireSound;

        public Mothula(Game game,
            Texture2D texture,
            Vector2 position,
            Vector2 velocity,
            Rectangle zone,
            float moveSpeed,
            EnemySpawner spawner,
            float drawOrder,
            PlayScene currentScene,
            SoundEffect deathSound) : base(game, texture, position, velocity, zone, spawner, drawOrder, deathSound)
        {
            Health = 13f;
            Damage = 2.5f;
            ScoreValue = 5000;
            Scale = 2;
            this.moveSpeed = moveSpeed;
            this.currentScene = currentScene;
            this.velocity = new Vector2(moveSpeed, 0);

            // Mothula has different width frames arranged in the sprite sheet in 
            // three one column rows.
            frames = new List<Rectangle>();
            frames.Add(Shared.GetFramesOneRow(new Vector2(texture.Width, texture.Height / 3), 1, 0)[0]);
            frames.Add(Shared.GetFramesOneRow(new Vector2(44, texture.Height / 3), 1, 1)[0]);
            frames.Add(Shared.GetFramesOneRow(new Vector2(22, texture.Height / 3), 1, 2)[0]);
            frameIndex = 0;
            this.drawRect = frames[frameIndex];

            g = (Game1)game;
            projectileTex = g.Content.Load<Texture2D>("images/fireball");
            fireSound = g.Content.Load<SoundEffect>("sounds/fireball");

            origin = new Vector2(drawRect.Width / 2, drawRect.Height / 2);
        }

        /// <summary>
        /// Two movement patterns:
        /// Move left at and right at the top/bottom of the screen firing projectiles
        /// Charge at the player from one side of the screen to the other
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            position += velocity;
            if (position.X + drawRect.Width * Scale / 2 >= zone.X + zone.Width)
            {
                position.X = zone.X + zone.Width - drawRect.Width * Scale / 2;
                velocity.X *= -1;
            }
            if (position.X - drawRect.Width * Scale / 2 <= zone.X)
            {
                position.X = zone.X + drawRect.Width * Scale / 2;
                velocity.X *= -1;
            }
            // Charging: An attack pattern where Mothula moves quickly from one side of
            // the screen to the opposite in the direction of the player.
            if (charging)
            {
                if (position.Y + drawRect.Height * Scale / 2 >= zone.Y + zone.Height)
                {
                    position.Y = zone.Y + zone.Height - drawRect.Height * Scale / 2;
                    charging = false;
                    if (velocity.X < 0)
                    {
                        velocity = new Vector2(-moveSpeed, 0);
                    }
                    else
                    {
                        velocity = new Vector2(moveSpeed, 0);
                    }
                }
                if (position.Y - drawRect.Height * Scale / 2 <= zone.Y)
                {
                    position.Y = zone.Y + drawRect.Height * Scale / 2;
                    charging = false;
                    if (velocity.X < 0)
                    {
                        velocity = new Vector2(-moveSpeed, 0);
                    }
                    else
                    {
                        velocity = new Vector2(moveSpeed, 0);
                    }
                }
            }
            else if (!charging)
            {
                // Mothula will fire projectiles within a set interval. When firing, it will fire
                // up to 5 buffered projectiles.
                if (firing)
                {
                    projectileBufferCounter++;
                    if (projectileBufferCounter >= projectileBuffer)
                    {
                        projectileBufferCounter = 0;
                        numberOfProjectiles++;
                        FireProjectile();
                    }
                    if (numberOfProjectiles == maxProjectiles)
                    {
                        firing = false;
                        numberOfProjectiles = 0;
                    }
                }
                projectileTimerCounter++;
                if (projectileTimerCounter >= projectileTimeInterval)
                {
                    projectileTimerCounter = 0;
                    firing = true;
                }

                chargeTimerCounter++;
                if (chargeTimerCounter >= chargeTimeInterval && !firing)
                {
                    chargeTimerCounter = 0;
                    Random r = new Random();
                    if (r.Next(1, 2) == 1)
                    {
                        charging = true;
                        float angleTowardsPlayer = VectorTools.GetAngle(position, currentScene.player.position);
                        velocity = VectorTools.GetVelocity(angleTowardsPlayer, moveSpeed * 1.8f);
                    }
                }
            }

            frameBufferCounter++;
            if (frameBufferCounter >= frameBuffer)
            {
                frameBufferCounter = 0;
                frameIndex = (frameIndex + 1) % frames.Count;
                drawRect = frames[frameIndex];
                origin = new Vector2(drawRect.Width / 2, drawRect.Height / 2);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Creates a fireball projectile aimed at the player.
        /// </summary>
        private void FireProjectile()
        {
            Fireball newFireball = new Fireball(g, projectileTex, position, currentScene.player.position, projectileSpeed, fireSound, 1);
            fireSound.Play();
            currentScene.Components.Add(newFireball);
            currentScene.EnemyProjectiles.Add(newFireball);
        }
    }
}
