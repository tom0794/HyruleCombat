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
    /// The Player's Sword. Can be swung in an arching fashion in the direction
    /// the player is facing.
    /// </summary>
    public class Sword : Sprite
    {
        public float originalRotation;
        private Player player;
        private int originOffset = -15;
        public SoundEffect soundEffect;
        private float slashDistance = (float)Math.PI;
        public bool cooldown = false;
        public float cooldownTime = 0;
        public float maxCooldownTime = 45;
        private float damage = 2;

        public float Damage { get => damage; set => damage = value; }

        public Sword(Game game, 
            Texture2D texture, 
            Vector2 position,
            float rotation,
            Player player,
            SoundEffect soundEffect,
            float drawOrder) : base(game, texture, position, drawOrder)
        {
            this.moveSpeed = 0.20f;
            this.rotation = rotation;
            originalRotation = rotation;
            origin = new Vector2(originOffset, texture.Height / 2);
            this.player = player;
            this.Enabled = false;
            this.Visible = false;
            this.soundEffect = soundEffect;
        }

        public override void Update(GameTime gameTime)
        {
            // While cooldown is true, the player will not be able to swing the sword.
            // This is to limit the speed at which the player can repeatedly swing.
            if (cooldown)
            {
                cooldownTime++;
                if (cooldownTime >= maxCooldownTime)
                {
                    cooldown = false;
                    cooldownTime = 0;
                    this.Enabled = false;
                }
            }

            rotation -= moveSpeed;
            position = player.position;
            if (rotation <= originalRotation - slashDistance)
            {
                this.Visible = false;
                this.position = new Vector2(-200, -200);
                rotation = originalRotation - slashDistance;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// For collision the Sword's hitbox consists of multiple rectangles arranged
        /// along the length of the Sword.
        /// </summary>
        /// <returns>An array of Rectangles located along the Sword.</returns>
        public Rectangle[] GetHitbox()
        {
            int numberOfRectangles = 10;
            Rectangle[] hitBoxes = new Rectangle[numberOfRectangles];

            // Locate the base of the sword relative to the origin. This is where the hitbox
            // rentangles must begin
            Vector2 startingPoint = VectorTools.GetVelocity(rotation, -originOffset);
            startingPoint.X += position.X;
            startingPoint.Y += position.Y;
            // The increment spaces out the rectangles evenly
            Vector2 increment = VectorTools.GetVelocity(rotation, (texture.Width / numberOfRectangles));

            for (int i = 0; i < numberOfRectangles; i++)
            {
                Rectangle rect = new Rectangle((int)(startingPoint.X + ((i + 1) * increment.X)), (int)(startingPoint.Y + ((i + 1) * increment.Y)),
                    texture.Width / numberOfRectangles, texture.Height);
                // change the rectangle's position such that the original point is centered
                rect.X -= rect.Width / 2;
                rect.Y -= rect.Height / 2;
                hitBoxes[i] = rect;
            }

            return hitBoxes;
        }
    }
}
