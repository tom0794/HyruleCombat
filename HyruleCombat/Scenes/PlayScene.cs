using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HyruleCombat.Effects;
using HyruleCombat.InfoDisplay;
using HyruleCombat.Logic;
using HyruleCombat.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Scenes
{
    /// <summary>
    /// Gameplay scene. Includes elements common to both game modes: HUD, player, 
    /// and logic for controlling player. Also possesses properties for tracking
    /// enemies, player health, and score values.
    /// </summary>
    public abstract class PlayScene : GameScene
    {
        // Public player, sword, etc.
        // Hud components
        private Game1 g;
        protected SpriteBatch spriteBatch;
        protected Texture2D background;
        public Player player;
        protected Crosshairs crosshairs;
        private Sword sword;
        private Boomerang boomerang;
        private Bow bow;

        protected HealthDisplay healthDisplay;
        protected HUD hud;

        private CollisionManager collisionManager;
        private List<Enemy> enemyList;
        private List<Projectile> projectileList;
        private List<Projectile> enemyProjectiles = new List<Projectile>();
        public List<Enemy> EnemyList { get => enemyList; set => enemyList = value; }
        public List<Projectile> ProjectileList { get => projectileList; set => projectileList = value; }
        public List<Projectile> EnemyProjectiles { get => enemyProjectiles; set => enemyProjectiles = value; }

        private int score = 0;
        private int killCounter = 0;
        private int selectedItem = 0;

        public Texture2D enemyDeathTexture;

        private SoundEffect swordSounds;
        private SoundEffect arrowSound;
        private SoundEffect breakSound;
        private SoundEffect boomerangSound;
        public SoundEffect enemyHitSound;

        private KeyboardState oldKeyState;

        public const int BOOMERANG = 0;
        public const int BOW = 1;
        private const float ARROW_SPEED = 8f;
        private const float BOOMERANG_SPEED = 7;

        private int arrowDelayCounter = 0;
        private int arrowDelay = 30;

        public int SelectedItem { get => selectedItem; set => selectedItem = value; }
        public int Score { get => score; set => score = value; }
        public int KillCounter { get => killCounter; set => killCounter = value; }

        public PlayScene(Game game, bool showClouds) : base(game)
        {
            g = (Game1)game;
            spriteBatch = g._spriteBatch;

            // Load scene assets that are common to all play scenes

            // Sounds ----------------------------------------------------------------------
            swordSounds = g.Content.Load<SoundEffect>("sounds/master sword");
            arrowSound = g.Content.Load<SoundEffect>("sounds/arrow 1");
            breakSound = g.Content.Load<SoundEffect>("sounds/break");
            boomerangSound = g.Content.Load<SoundEffect>("sounds/boomerang");
            enemyHitSound = g.Content.Load<SoundEffect>("sounds/enemyHit");
            // -----------------------------------------------------------------------------

            // Load player
            Texture2D playerTex = g.Content.Load<Texture2D>("images/link1");
            SoundEffect hurtSound = g.Content.Load<SoundEffect>("sounds/hurt");
            player = new Player(g, playerTex, new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2), 1.0f, hurtSound);
            this.Components.Add(player);

            // Load sword
            Texture2D swordTex = g.Content.Load<Texture2D>("images/sword");
            SoundEffect swordSound = g.Content.Load<SoundEffect>("sounds/master sword");
            sword = new Sword(g, swordTex, player.position, 0, player, swordSound, 0.1f);
            sword.Remove();
            this.Components.Add(sword);

            // Load boomerang
            Texture2D boomerangTex = g.Content.Load<Texture2D>("images/boomerang");
            boomerang = new Boomerang(g, boomerangTex, Vector2.Zero, Vector2.Zero, BOOMERANG_SPEED, player, boomerangSound, 0.1f);
            boomerang.Disable();
            this.Components.Add(boomerang);

            // Load bow
            Texture2D bowTex = g.Content.Load<Texture2D>("images/bow");
            bow = new Bow(g, bowTex, player.position, 1, player);
            bow.Remove();
            this.Components.Add(bow);

            // Load cloud effect
            if (showClouds)
            {
                Texture2D cloudTex = g.Content.Load<Texture2D>("images/clouds");
                Rectangle cloudRect = new Rectangle(0, 0, cloudTex.Width, cloudTex.Height);
                Vector2 cloudSpeed = new Vector2(1, 0);
                CloudsEffect clouds = new CloudsEffect(g, cloudTex, cloudRect, Vector2.Zero, cloudSpeed, 0.1f);
                this.Components.Add(clouds);
            }

            // Load collision manager
            EnemyList = new List<Enemy>();
            ProjectileList = new List<Projectile>();
            collisionManager = new CollisionManager(g, EnemyList, ProjectileList, player, sword, boomerang, this);
            this.Components.Add(collisionManager);

            // Load Health display
            SpriteFont hudFont = g.Content.Load<SpriteFont>("fonts/healthFont");
            Texture2D heartTex = g.Content.Load<Texture2D>("images/heart");
            healthDisplay = new HealthDisplay(g, spriteBatch, hudFont, heartTex, player);
            this.Components.Add(healthDisplay);
            foreach (Heart heart in healthDisplay.hearts)
            {
                this.Components.Add(heart);
            }

            // Load Heads Up Display
            Texture2D hudElementTex = g.Content.Load<Texture2D>("images/hudElement");
            Texture2D hudElementSmallTex = g.Content.Load<Texture2D>("images/hudElementSmall");
            hud = new HUD(g, spriteBatch, hudFont, hudElementTex, hudElementSmallTex, this, BOOMERANG, BOW);
            this.Components.Add(hud);
            this.Components.Add(hud.ScoreValue);
            this.Components.Add(hud.KillCount);
            this.Components.Add(hud.BoomerangIcon);
            this.Components.Add(hud.BowIcon);

            // Load crosshairs
            Texture2D crosshairsTex = g.Content.Load<Texture2D>("images/crosshairs");
            crosshairs = new Crosshairs(g, crosshairsTex, new Vector2(0, 0), 0);
            this.Components.Add(crosshairs);

            // Load enemy death animation texture
            enemyDeathTexture = g.Content.Load<Texture2D>("images/enemyDeathAnim");
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();

            // Left click: Swing sword
            if (mouse.LeftButton == ButtonState.Pressed && mouse.RightButton == ButtonState.Released
                && !sword.Enabled && !sword.cooldown)
            {
                sword.rotation = player.rotation - player.rotationFactor + (float)Math.PI / 2;
                if (sword.rotation > 2 * Math.PI)
                {
                    sword.rotation -= 2 * (float)Math.PI;
                }
                sword.originalRotation = sword.rotation;
                sword.position = player.position;
                sword.Enabled = true;
                sword.Visible = true;
                sword.cooldown = true;
                sword.soundEffect.Play();
            }

            // Equip item
            if (ks.IsKeyDown(Keys.Q) && oldKeyState.IsKeyUp(Keys.Q))
            {
                if (selectedItem == BOW)
                {
                    selectedItem = BOOMERANG;
                }
                else
                {
                    selectedItem = BOW;
                }
            }

            // Fire projectile
            arrowDelayCounter++;
            if (mouse.RightButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released)
            {
                if (selectedItem == BOW && arrowDelayCounter >= arrowDelay)
                {
                    Texture2D arrowImg = g.Content.Load<Texture2D>("images/arrow");
                    Arrow arrow = new Arrow(g, arrowImg, player.position, crosshairs.position, ARROW_SPEED, arrowSound, 0.1f);
                    this.Components.Add(arrow);
                    ProjectileList.Add(arrow);
                    arrowSound.Play();
                    bow.Display();
                    arrowDelayCounter = 0;
                }
                else if (selectedItem == BOOMERANG && !boomerang.Enabled)
                {
                    boomerang.SetDestination(new Vector2(mouse.X, mouse.Y));
                    boomerang.Activate();
                }
            }
            oldKeyState = Keyboard.GetState();
            // Remove disabled arrows
            for (int i = 0; i < this.Components.Count; i++)
            {
                if (this.Components[i] is Arrow && !this.Components[i].Enabled)
                {
                    this.Components.Remove(this.Components[i]);
                    i--;
                }
            }
            foreach (Projectile p in enemyProjectiles.ToList())
            {
                if (!p.Enabled)
                {
                    this.Components.Remove(p);
                }
            }
            base.Update(gameTime);
        }
    }
}
