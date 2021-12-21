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
    /// An Enemy that will chase the player. Has four sets of animation frames, 
    /// one for each direction it can face.
    /// </summary>
    public class Mimic : Enemy
    {
        private List<Rectangle>[] frameSets = new List<Rectangle>[4];
        private const int FACING_DOWN = 0;
        private const int FACING_UP = 1;
        private const int FACING_RIGHT = 2;
        private const int FACING_LEFT = 3;

        private int frameSetIndex;
        private int frameIndex;
        private int frameBuffer = 7;
        private int frameBufferCounter = 0;
        private Player player;

        public Mimic(Game game, 
            Texture2D texture, 
            Vector2 position, 
            Vector2 velocity, 
            Rectangle zone,
            float moveSpeed,
            EnemySpawner spawner, 
            float drawOrder, 
            SoundEffect deathSound,
            Player player) : base(game, texture, position, velocity, zone, spawner, drawOrder, deathSound)
        {
            Health = 3f;
            Damage = 1.5f;
            ScoreValue = 500;
            Scale = 2;
            this.moveSpeed = moveSpeed;
            this.player = player;

            // Load a set of frames for each direction the enemy can face
            frameSets[FACING_DOWN] = Shared.GetFramesOneRow(new Vector2(texture.Width, texture.Height / 4), 3, 0);
            frameSets[FACING_UP] = Shared.GetFramesOneRow(new Vector2(texture.Width, texture.Height / 4), 3, 1);
            frameSets[FACING_RIGHT] = Shared.GetFramesOneRow(new Vector2(36, texture.Height / 4), 2, 2);
            frameSets[FACING_LEFT] = Shared.GetFramesOneRow(new Vector2(36, texture.Height / 4), 2, 3);

            frameIndex = 0;
            frameSetIndex = 0;
            this.drawRect = frameSets[frameSetIndex][frameIndex];
            origin = new Vector2(drawRect.Width / 2, drawRect.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Stunned)
            {
                float angleTowardsPlayer = VectorTools.GetAngle(position, player.position);
                velocity = VectorTools.GetVelocity(angleTowardsPlayer, moveSpeed);
                position += velocity;
                if (VectorTools.CheckFacingDown(angleTowardsPlayer))
                {
                    frameSetIndex = FACING_DOWN;
                }
                else if (VectorTools.CheckFacingUp(angleTowardsPlayer))
                {
                    frameSetIndex = FACING_UP;
                }
                else if (VectorTools.CheckFacingLeft(angleTowardsPlayer))
                {
                    frameSetIndex = FACING_LEFT;
                }
                else if (VectorTools.CheckFacingRight(angleTowardsPlayer))
                {
                    frameSetIndex = FACING_RIGHT;
                }

                frameBufferCounter++;
                if (frameBufferCounter >= frameBuffer)
                {
                    frameBufferCounter = 0;
                    frameIndex = (frameIndex + 1) % frameSets[frameSetIndex].Count;
                    drawRect = frameSets[frameSetIndex][frameIndex];
                }
            }
            base.Update(gameTime);
        }
    }
}
