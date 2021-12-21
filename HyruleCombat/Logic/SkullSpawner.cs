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
    /// EnemySpawner responsible for spawning Skull enemies.
    /// </summary>
    public class SkullSpawner : EnemySpawner
    {
        private Game1 g;
        private float moveSpeed;
        private Vector2 skullPosition;
        private Vector2 velocity;
        private Texture2D texture;
        private SoundEffect deathSound;

        public SkullSpawner(Game game, 
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
            deathSound = g.Content.Load<SoundEffect>("sounds/enemyDeath");
        }

        public override void Update(GameTime gameTime)
        {
            spawnTimerCounter++;
            if (spawnNewEnemy && spawnTimerCounter >= spawnTimerMax)
            {
                Random r = new Random();
                skullPosition = new Vector2(r.Next(spawnZone.X, spawnZone.X + spawnZone.Width),
                    r.Next(spawnZone.Y, spawnZone.Y + spawnZone.Height));
                Skull skull = new Skull(g, texture, skullPosition, velocity, Shared.stageRect, moveSpeed, this, 0, deathSound);
                currentScene.Components.Add(skull);
                currentScene.EnemyList.Add(skull);
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
