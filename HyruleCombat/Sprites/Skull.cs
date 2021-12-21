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
    /// A simple Enemy that bounces within a set area.
    /// </summary>
    public class Skull : Enemy
    {
        private List<Rectangle> frames;
        private int frameIndex;
        private int frameBuffer = 5;
        private int frameBufferCounter = 0;

        public Skull(Game game, 
            Texture2D texture, 
            Vector2 position, 
            Vector2 velocity, 
            Rectangle zone, 
            float moveSpeed,
            EnemySpawner spawner, 
            float drawOrder, 
            SoundEffect deathSound) : base(game, texture, position, velocity, zone, spawner, drawOrder, deathSound)
        {
            Health = 2;
            Damage = 0.5f;
            ScoreValue = 100;
            Scale = 2;
            this.moveSpeed = moveSpeed;
            this.velocity = new Vector2(moveSpeed * velocity.X, moveSpeed * velocity.Y);
            Random r = new Random();
            if (r.Next(0,1) == 0)
            {
                velocity.X *= -1;
                if (r.Next(0,1) == 0)
                {
                    velocity.Y *= -1;
                }
            }

            this.frames = Shared.GetFramesOneRow(new Vector2(texture.Width, texture.Height), 5, 0);
            frameIndex = 0;
            this.drawRect = frames[frameIndex];

            origin = new Vector2(drawRect.Width / 2, drawRect.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Stunned)
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
                if (position.Y + drawRect.Height * Scale / 2 >= zone.Y + zone.Height)
                {
                    position.Y = zone.Y + zone.Height - drawRect.Height * Scale / 2;
                    velocity.Y *= -1;
                }
                if (position.Y - drawRect.Height * Scale / 2 <= zone.Y)
                {
                    position.Y = zone.Y + drawRect.Height * Scale / 2;
                    velocity.Y *= -1;
                }
                frameBufferCounter++;
                if (frameBufferCounter >= frameBuffer)
                {
                    frameBufferCounter = 0;
                    frameIndex = (frameIndex + 1) % frames.Count;
                    drawRect = frames[frameIndex];
                }
            }
            base.Update(gameTime);
        }
    }
}
