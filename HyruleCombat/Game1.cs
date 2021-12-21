/* Game1.cs
 * PROG 2370 Final Project. Tom Chiasson (7406242) Sec 3
 * Revision History
 *      Tom Chiasson, 2021.11.21: Created
 */

using HyruleCombat.Logic;
using HyruleCombat.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HyruleCombat
{
    /// <summary>
    /// Manages the various GameScenes. Allows the player to navigate between
    /// menus and game modes.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        private AboutScene aboutScene;
        private HelpScene helpScene;
        private MenuScene mainMenuScene;
        private MenuScene pauseMenuScene;
        private ScoreScene scoreScene;
        private TrainingScene trainingScene;
        private SurvivalScene survivalScene;
        private GameOverScene gameOverScene;
        private GameScene currentScene;

        private string[] mainMenuItems = { "Survival", "Training", "High Scores", "Help", "About", "Quit" };
        private string mainMenuTitle = "Hyrule Combat";
        private string[] pauseMenuItems = { "Resume", "Help", "Quit to Main Menu", "Quit Game" };
        private string pauseMenuTitle = "Paused";

        private const int MAIN_MENU_SURVIVAL = 0;
        private const int MAIN_MENU_TRAINING = 1;
        private const int MAIN_MENU_HIGHSCORES = 2;
        private const int MAIN_MENU_HELP = 3;
        private const int MAIN_MENU_ABOUT = 4;
        private const int MAIN_MENU_QUIT = 5;

        private const int PAUSE_MENU_RESUME = 0;
        private const int PAUSE_MENU_HELP = 1;
        private const int PAUSE_MENU_MAINMENU = 2;
        private const int PAUSE_MENU_QUITGAME = 3;

        private bool paused = false;
        private KeyboardState previousInput;
        private Keys confirmKey = Keys.Space;

        private Song survivalMusic;
        private Song menuMusic;
        private Song trainingMusic;
        private Song gameOverMusic;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            Shared.stage = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Shared.stageRect = new Rectangle(0, 0, (int)Shared.stage.X, (int)Shared.stage.Y);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load High Scores ===============================================
            ScoreManager.LoadHighScores();
            // ================================================================

            // Load Music =====================================================
            survivalMusic = this.Content.Load<Song>("sounds/survival");
            trainingMusic = this.Content.Load<Song>("sounds/training");
            menuMusic = this.Content.Load<Song>("sounds/menu");
            gameOverMusic = this.Content.Load<Song>("sounds/gameover");
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(menuMusic);
            // ================================================================

            // Load scenes ====================================================
            aboutScene = new AboutScene(this);
            this.Components.Add(aboutScene);

            helpScene = new HelpScene(this);
            this.Components.Add(helpScene);

            mainMenuScene = new MenuScene(this, mainMenuItems, mainMenuTitle);
            this.Components.Add(mainMenuScene);

            pauseMenuScene = new MenuScene(this, pauseMenuItems, pauseMenuTitle);
            this.Components.Add(pauseMenuScene);

            scoreScene = new ScoreScene(this);
            this.Components.Add(scoreScene);

            trainingScene = new TrainingScene(this, true);
            this.Components.Add(trainingScene);

            survivalScene = new SurvivalScene(this, false);
            this.Components.Add(survivalScene);

            gameOverScene = new GameOverScene(this, survivalScene);
            // ================================================================

            mainMenuScene.Show();
        }

        private void HideAllScenes()
        {
            foreach (GameScene scene in this.Components)
            {
                scene.Hide();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            int selectedIndex = 0;
            KeyboardState ks = Keyboard.GetState();
            if (mainMenuScene.Enabled)
            {
                selectedIndex = mainMenuScene.Menu.SelectedIndex;
                if (selectedIndex == MAIN_MENU_SURVIVAL && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                {
                    mainMenuScene.Hide();
                    this.Components.Remove(survivalScene);
                    survivalScene = new SurvivalScene(this, false);
                    this.Components.Add(survivalScene);
                    survivalScene.Show();
                    MediaPlayer.Play(survivalMusic);
                }
                else if (selectedIndex == MAIN_MENU_TRAINING && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                {
                    mainMenuScene.Hide();
                    // Re instantiate scene to reset it
                    this.Components.Remove(trainingScene);
                    trainingScene = new TrainingScene(this, true);
                    this.Components.Add(trainingScene);
                    trainingScene.Show();
                    MediaPlayer.Play(trainingMusic);
                }
                else if (selectedIndex == MAIN_MENU_HIGHSCORES && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                {
                    mainMenuScene.Hide();
                    scoreScene.Show();
                }
                else if (selectedIndex == MAIN_MENU_HELP && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                {
                    mainMenuScene.Hide();
                    helpScene.Show();
                }
                else if (selectedIndex == MAIN_MENU_ABOUT && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                {
                    mainMenuScene.Hide();
                    aboutScene.Show();
                }
                else if (selectedIndex == MAIN_MENU_QUIT && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                {
                    Exit();
                }
            }
            else if (pauseMenuScene.Enabled)
            {
                paused = true;
                selectedIndex = pauseMenuScene.Menu.SelectedIndex;
                if (selectedIndex == PAUSE_MENU_RESUME && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                {
                    currentScene.Show();
                    pauseMenuScene.Hide();
                    paused = false;
                }
                else if (selectedIndex == PAUSE_MENU_HELP && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                {
                    pauseMenuScene.Hide();
                    paused = true;
                    helpScene.Show();
                }
                else if (selectedIndex == PAUSE_MENU_MAINMENU && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                {
                    HideAllScenes();
                    paused = false;
                    mainMenuScene.Menu.SelectedIndex = 0;
                    mainMenuScene.Show();
                    MediaPlayer.Play(menuMusic);
                }
                else if (selectedIndex == PAUSE_MENU_QUITGAME && ks.IsKeyDown(confirmKey) && previousInput.IsKeyUp(confirmKey))
                { 
                    Exit();
                }
            }
            

            if (aboutScene.Enabled || helpScene.Enabled && !paused || scoreScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    HideAllScenes();
                    mainMenuScene.Menu.SelectedIndex = 0;
                    mainMenuScene.Show();
                }
            }
            else if (trainingScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    trainingScene.Hide();
                    currentScene = trainingScene;
                    pauseMenuScene.Menu.SelectedIndex = 0;
                    pauseMenuScene.Show();
                }
            }
            else if (survivalScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    survivalScene.Hide();
                    currentScene = survivalScene;
                    pauseMenuScene.Menu.SelectedIndex = 0;
                    pauseMenuScene.Show();
                }
                // Check for game over
                if (survivalScene.player.health <= 0)
                {
                    survivalScene.Hide();
                    this.Components.Remove(gameOverScene);
                    gameOverScene = new GameOverScene(this, survivalScene);
                    this.Components.Add(gameOverScene);
                    gameOverScene.Show();
                    MediaPlayer.Play(gameOverMusic);
                }
            }
            else if (helpScene.Enabled && paused)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    helpScene.Hide();
                    pauseMenuScene.Menu.SelectedIndex = 0;
                    pauseMenuScene.Show();
                }
            }
            else if (gameOverScene.Enabled)
            {
                if (gameOverScene.newHighScoreAchieved && gameOverScene.selectedOption == GameOverScene.DONE
                    && ks.IsKeyDown(confirmKey))
                {
                    gameOverScene.Hide();
                    ScoreManager.AddHighScore(gameOverScene.name, gameOverScene.kills, gameOverScene.score);
                    this.Components.Remove(scoreScene);
                    scoreScene = new ScoreScene(this);
                    this.Components.Add(scoreScene);
                    scoreScene.Show();
                    MediaPlayer.Play(menuMusic);
                }
                else if (!gameOverScene.newHighScoreAchieved && ks.IsKeyDown(confirmKey))
                {
                    gameOverScene.Hide();
                    mainMenuScene.Show();
                    MediaPlayer.Play(menuMusic);
                }
            }

            previousInput = ks;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
