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
    /// Enemy type that moves sporadically by changing direction every 180 frames.
    /// </summary>
    public class Bat : Enemy
    {
        private List<Rectangle> frames;
        private int frameIndex;
        private int frameBuffer = 7;
        private int frameBufferCounter = 0;

        private int directionChangeTime = 180;
        private int directionChangeCounter = 0;

        public Bat(Game game,
            Texture2D texture,
            Vector2 position,
            Vector2 velocity,
            Rectangle zone,
            float moveSpeed,
            EnemySpawner spawner,
            float drawOrder,
            SoundEffect deathSound) : base(game, texture, position, velocity, zone, spawner, drawOrder, deathSound)
        {
            Health = 0.5f;
            Damage = 0.5f;
            ScoreValue = 150;
            Scale = 2;
            this.moveSpeed = moveSpeed;

            Random r = new Random();
            this.velocity = VectorTools.GetVelocity(r.Next(0, (int)Math.PI * 2), moveSpeed);

            this.frames = Shared.GetFramesOneRow(new Vector2(texture.Width, texture.Height), 2, 0);
            frameIndex = 0;
            this.drawRect = frames[frameIndex];

            origin = new Vector2(drawRect.Width / 2, drawRect.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Stunned)
            {
                directionChangeCounter++;
                if (directionChangeCounter > directionChangeTime)
                {
                    Random r = new Random();
                    velocity = VectorTools.GetVelocity(r.Next(0, (int)Math.PI * 2), moveSpeed);
                    directionChangeCounter = 0;
                }
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
