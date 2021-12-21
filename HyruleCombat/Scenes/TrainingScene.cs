using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.Sprites;
using HyruleCombat.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HyruleCombat.InfoDisplay;
using HyruleCombat.Logic;

namespace HyruleCombat.Scenes
{
    /// <summary>
    /// Game mode designed for practicing game controls and mechanics. Targets
    /// are spawned that do not damage the Player. The player can end this game
    /// mode via the pause menu.
    /// </summary>
    public class TrainingScene : PlayScene
    {
        private TargetSpawner targetSpawnerTop;
        private TargetSpawner targetSpawnerRight;
        private TargetSpawner targetSpawnerLeft;

        public TrainingScene(Game game, bool showClouds) : base(game, showClouds)
        {
            // Load components unique to training scene such as enemy spawner and target
            Game1 g = (Game1)game;
            background = g.Content.Load<Texture2D>("images/trainingBackgroun");

            // Load target spawners
            Texture2D targetTex = g.Content.Load<Texture2D>("images/Target");

            targetSpawnerTop = new TargetSpawner(g, Shared.spawnerZoneTop, 3, 60, this, 1, new Vector2(3, 0), targetTex);
            this.Components.Add(targetSpawnerTop);

            targetSpawnerRight = new TargetSpawner(g, Shared.spawnerZoneRight, 3, 60, this, 1, new Vector2(0, 3), targetTex);
            this.Components.Add(targetSpawnerRight);

            Rectangle spawnerZoneLeft = new Rectangle(10, 90, 300, 325);
            targetSpawnerLeft = new TargetSpawner(g, spawnerZoneLeft, 6, 60, this, 0, new Vector2(0, 0), targetTex);
            this.Components.Add(targetSpawnerLeft);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw background for scene. 
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
