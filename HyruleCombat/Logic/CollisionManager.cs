using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HyruleCombat.Effects;
using HyruleCombat.Scenes;
using HyruleCombat.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Logic
{
    /// <summary>
    /// Checks for collisions between the Player and Enemies, Player and Enemy projectiles,
    /// and Enemies and Player Projectiles/Sword. Note that the Player and Enemies can possess
    /// invulnerability, meaning after taking damage there is a brief period where they are 
    /// immune to further damage. This is indicated by the Sprite blinking.
    /// </summary>
    public class CollisionManager : GameComponent
    {
        private List<Enemy> enemies;
        private List<Projectile> projectiles;
        private Player player;
        private Sword sword;
        private Boomerang boomerang;
        private PlayScene currentScene;

        public CollisionManager(Game game,
            List<Enemy> enemies,
            List<Projectile> projectiles,
            Player player,
            Sword sword,
            Boomerang boomerang,
            PlayScene currentScene) : base(game)
        {
            this.enemies = enemies;
            this.projectiles = projectiles;
            this.player = player;
            this.sword = sword;
            this.boomerang = boomerang;
            this.currentScene = currentScene;
        }

        /// <summary>
        /// Check for each type of collision. 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies.ToList())
            {
                Rectangle enemyRect = enemy.GetHitbox();
                // Check sword collision
                Rectangle[] swordRects = sword.GetHitbox();
                foreach (Rectangle hitBox in swordRects)
                {
                    if (hitBox.Intersects(enemyRect))
                    {
                        if (!enemy.Invulnerable)
                        {
                            enemy.Health -= sword.Damage;
                            if (enemy.Health <= 0)
                            {
                                PlayDeathAnimation(enemy);
                                currentScene.Score += enemy.ScoreValue;
                                currentScene.KillCounter++;
                                enemy.DeathSound.Play();
                                enemy.Remove();
                                enemies.Remove(enemy);
                                currentScene.Components.Remove(enemy);
                            }
                            else
                            {
                                currentScene.enemyHitSound.Play();
                            }
                            enemy.Invulnerable = true;
                            break; 
                        }
                    }
                }

                // Check projectile collision
                foreach (Projectile projectile in projectiles)
                {
                    if (projectile is Arrow)
                    {
                        Rectangle projRect = projectile.GetHitbox();
                        if (enemyRect.Intersects(projRect))
                        {
                            if (!enemy.Invulnerable)
                            {
                                enemy.Health -= projectile.Damage;
                                projectile.Remove();
                                projectiles.Remove(projectile);
                                if (enemy.Health <= 0)
                                {
                                    PlayDeathAnimation(enemy);
                                    currentScene.Score += enemy.ScoreValue;
                                    currentScene.KillCounter++;
                                    enemy.DeathSound.Play();
                                    enemy.Remove();
                                    enemies.Remove(enemy);
                                    currentScene.Components.Remove(enemy);
                                }
                                else
                                {
                                    currentScene.enemyHitSound.Play();
                                }
                                enemy.Invulnerable = true;
                                break;
                            }
                        }
                    }
                }

                // Check boomerang collision
                Rectangle boomerangRect = boomerang.GetHitbox();
                if (enemyRect.Intersects(boomerangRect) && !enemy.Invulnerable)
                {
                    enemy.Stunned = true;
                    boomerang.returning = true;
                }

                // Check player hit by enemy collision
                Rectangle playerRect = player.GetHitbox();
                if (enemyRect.Intersects(playerRect))
                {
                    if (!player.invulnerable)
                    {
                        player.health -= enemy.Damage;
                        player.invulnerable = true;
                        player.HurtSound.Play();
                    }
                }

                // Check player hit by enemy projectile
                foreach (Projectile enemyProj in currentScene.EnemyProjectiles)
                {
                    Rectangle projHitbox = enemyProj.GetHitbox();
                    if (projHitbox.Intersects(playerRect))
                    {
                        if (!player.invulnerable)
                        {
                            player.health -= enemyProj.Damage;
                            player.invulnerable = true;
                            player.HurtSound.Play();
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// When an Enemy's health reaches 0, the Enemy is removed and a death 
        /// animation is played on the Enemy's location.
        /// </summary>
        /// <param name="enemy"></param>
        private void PlayDeathAnimation(Enemy enemy)
        {
            // Play enemy death animation
            EnemyDeathAnimation deathEffect = new EnemyDeathAnimation(currentScene.Game,
                currentScene.enemyDeathTexture, enemy.position, 1, 2, 1, 7, currentScene);
            currentScene.Components.Add(deathEffect);
            deathEffect.Restart();
        }
    }
}
