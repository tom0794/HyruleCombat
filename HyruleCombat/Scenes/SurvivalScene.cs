using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.InfoDisplay;
using HyruleCombat.Logic;
using HyruleCombat.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Scenes
{
    /// <summary>
    /// The Survival game mode. Possesses EnemySpawners that add Enemies to 
    /// the scene. Each time the player reaches a certain number of kills,
    /// regular Enemies stop spawning and the boss Enemy Mothula will spawn.
    /// </summary>
    public class SurvivalScene : PlayScene
    {
        private BatSpawner batSpawner1;
        private SkullSpawner skullSpawner1;
        private MimicSpawner mimicSpawner1;
        private MothulaSpawner mothSpawner1;

        private const float BAT_SPEED = 2;
        private const float SKULL_SPEED = 1;
        private const float MOTH_SPEED = 4;
        private const float MIMIC_SPEED = 1.8f;
        private const int KILL_BENCHMARK = 30;

        public SurvivalScene(Game game, bool showClouds) : base(game, showClouds)
        {
            // Load components unique to training scene such as enemy spawner and target
            Game1 g = (Game1)game;
            background = g.Content.Load<Texture2D>("images/survivalBackground");

            // Load Enemy spawners
            Texture2D batTex = g.Content.Load<Texture2D>("images/bat");
            batSpawner1 = new BatSpawner(g, Shared.spawnerZoneTop, 8, 60, this, BAT_SPEED, new Vector2(3, 3), batTex);
            this.Components.Add(batSpawner1);

            Texture2D skullTex = g.Content.Load<Texture2D>("images/bubble");
            skullSpawner1 = new SkullSpawner(g, Shared.spawnerZoneRight, 4, 60, this, SKULL_SPEED, new Vector2(2, 2), skullTex);
            skullSpawner1.Enabled = false;
            this.Components.Add(skullSpawner1);

            Texture2D mimicTex = g.Content.Load<Texture2D>("images/mimic");
            mimicSpawner1 = new MimicSpawner(g, Shared.spawnerZoneLeft, 1, 20, this, MIMIC_SPEED, Vector2.Zero, mimicTex);
            mimicSpawner1.Enabled = false;
            this.Components.Add(mimicSpawner1);

            Texture2D mothTex = g.Content.Load<Texture2D>("images/mothula");
            mothSpawner1 = new MothulaSpawner(g, Shared.spawnerZoneTop, 1, 100, this, MOTH_SPEED, new Vector2(1, 0), mothTex);
            mothSpawner1.Enabled = false;
            this.Components.Add(mothSpawner1);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw background for scene. 
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// The active spawners are configured based on the number of kills the
        /// player has accumulated. This logic is flexible and can be adjusted
        /// to increase or decrease the difficulty of the game.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Add game logic for scene here
            if (KillCounter == 10)
            {
                skullSpawner1.Enabled = true;
                mimicSpawner1.Enabled = true;
            }
            if ((KillCounter + EnemyList.Count) % KILL_BENCHMARK == 0 && KillCounter > 0)
            {
                batSpawner1.Enabled = false;
                mimicSpawner1.Enabled = false;
                skullSpawner1.Enabled = false;
            }
            if (KillCounter % KILL_BENCHMARK == 0 && KillCounter > 0)
            {
                mothSpawner1.Enabled = true;
            }
            if (((KillCounter - 1) % KILL_BENCHMARK) == 0 && KillCounter > 1)
            {
                mothSpawner1.Enabled = false;
                batSpawner1.Enabled = true;
                mimicSpawner1.Enabled = true;
                skullSpawner1.Enabled = true;
            }
            base.Update(gameTime);
        }
    }
}
