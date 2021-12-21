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
    /// EnemySpawner responsible for spawning Mothula enemies.
    /// </summary>
    public class MothulaSpawner : EnemySpawner
    {
        private Game1 g;
        private float moveSpeed;
        private Vector2 mothPosition;
        private Vector2 velocity;
        private Texture2D texture;
        private SoundEffect deathSound;

        public MothulaSpawner(Game game, 
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
                mothPosition = new Vector2(0, 122);
                Rectangle mothZone = new Rectangle(0, 90, Shared.stageRect.Width, 342);
                Mothula newMoth = new Mothula(g, texture, mothPosition, velocity, mothZone, moveSpeed, this, 0, currentScene, deathSound);
                currentScene.Components.Add(newMoth);
                currentScene.EnemyList.Add(newMoth);
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
