using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Scenes;
using HyruleCombat.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Logic
{
    /// <summary>
    /// Dynamically spawn Enemies within a set location, at a set frequency, 
    /// up to a set maximum number of Enemies at a time. Attempts to add a 
    /// new Enemy every interval as long as the max number of Enemies aren't
    /// currently active.
    /// </summary>
    public class EnemySpawner : GameComponent
    {
        private int activeEnemies;
        private int maxEnemies;
        // SpawnZone is the location wherein the spawner is to spawn enemies
        protected Rectangle spawnZone;
        public bool spawnNewEnemy = true;
        protected int spawnTimerCounter = 0;
        protected int spawnTimerMax;
        protected PlayScene currentScene;

        public int ActiveEnemies { get => activeEnemies; set => activeEnemies = value; }
        public int MaxEnemies { get => maxEnemies; set => maxEnemies = value; }

        public EnemySpawner(Game game,
            Rectangle spawnZone,
            int maxEnemies,
            int spawnTimerMax,
            PlayScene currentScene) : base(game)
        {
            this.spawnZone = spawnZone;
            this.maxEnemies = maxEnemies;
            this.spawnTimerMax = spawnTimerMax;
            this.currentScene = currentScene;
        }
    }
}
