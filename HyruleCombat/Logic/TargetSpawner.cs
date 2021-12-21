using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Scenes;
using HyruleCombat.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Logic
{
    /// <summary>
    /// EnemySpawner responsible for spawning Target enemies.
    /// </summary>
    public class TargetSpawner : EnemySpawner
    {
        private Game1 g;
        private float moveSpeed;
        private Vector2 targetPosition;
        private Vector2 velocity;
        private Texture2D texture;
        private SoundEffect breakSound;

        public TargetSpawner(Game game, 
            Rectangle spawnZone, 
            int maxEnemies, 
            int spawnTimerMax, 
            PlayScene currentScene,
            float moveSpeed,
            Vector2 velocity,
            Texture2D texture) : base(game, spawnZone, maxEnemies, spawnTimerMax, currentScene)
        {
            this.g = (Game1)game;
            this.moveSpeed = moveSpeed;
            this.velocity = velocity;
            this.texture = texture;
            breakSound = g.Content.Load<SoundEffect>("sounds/break");
        }

        public override void Update(GameTime gameTime)
        {
            spawnTimerCounter++;
            if (spawnNewEnemy && spawnTimerCounter >= spawnTimerMax)
            {
                Random r = new Random();
                targetPosition = new Vector2(r.Next(spawnZone.X, spawnZone.X + spawnZone.Width - texture.Width), 
                    r.Next(spawnZone.Y, spawnZone.Y + spawnZone.Height - texture.Height));
                Target newTarget = new Target(g, texture, targetPosition, velocity, spawnZone, moveSpeed, this, 0, breakSound);
                currentScene.Components.Add(newTarget);
                currentScene.EnemyList.Add(newTarget);
                ActiveEnemies++;
                spawnTimerCounter = 0;
                if (ActiveEnemies == MaxEnemies)
                {
                    spawnNewEnemy = false;
                }
            }
            if (!spawnNewEnemy && ActiveEnemies < MaxEnemies)
            {
                spawnNewEnemy = true;
            }
            if (!spawnNewEnemy)
            {
                spawnTimerCounter = 0;
            }
            base.Update(gameTime);
        }
    }
}
