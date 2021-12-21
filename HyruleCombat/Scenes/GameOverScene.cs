using System;
using System.Collections.Generic;
using System.Text;
using HyruleCombat.InfoDisplay;
using HyruleCombat.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Scenes
{
    /// <summary>
    /// When a player's help drops to 0 in a game mode, the game over scene is displayed.
    /// If the player achieved a high score, they are prompted to enter their name
    /// before being taken to the high score scene. Otherwise, the are returned to the
    /// main menu when they press space.
    /// </summary>
    public class GameOverScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private SpriteFont regularFont;
        private SpriteFont highlightFont;
        private SpriteFont titleFont;
        private PlayScene previousScene;
        private Texture2D background;
        public bool newHighScoreAchieved = false;

        private Vector2 titlePosition = new Vector2(209, 15);
        private Vector2 scorePosition = new Vector2(223, 125);
        private Vector2 newHighPosition = new Vector2(250, 260);
        private Vector2 namePosition = new Vector2(33, 355);

        private CharacterEntry character1;
        private CharacterEntry character2;
        private CharacterEntry character3;

        private Vector2 char1Position = new Vector2(473, 353);
        private Vector2 char2Position = new Vector2(502, 353);
        private Vector2 char3Position = new Vector2(531, 353);
        private Vector2 donePosition = new Vector2(570, 353);

        public int selectedOption = 0;
        private int numOptions = 4;

        private const int FIRST = 0;
        private const int SECOND = 1;
        private const int THIRD = 2;
        public const int DONE = 3;

        public string name;
        public int kills;
        public int score;

        private KeyboardState oldState;

        public GameOverScene(Game game,
            PlayScene playScene) : base(game)
        {
            Game1 g = (Game1)game;
            this.spriteBatch = g._spriteBatch;

            regularFont = g.Content.Load<SpriteFont>("fonts/regularFont");
            highlightFont = g.Content.Load<SpriteFont>("fonts/highlightFont");
            titleFont = g.Content.Load<SpriteFont>("fonts/titleFont");
            background = g.Content.Load<Texture2D>("images/titlescreen");
            this.previousScene = playScene;

            kills = previousScene.KillCounter;
            score = previousScene.Score;

            if (previousScene.Score > ScoreManager.LowestHighScore)
            {
                newHighScoreAchieved = true;
                character1 = new CharacterEntry(g, spriteBatch, regularFont, highlightFont, titleFont, "", Shared.characters, char1Position);
                character2 = new CharacterEntry(g, spriteBatch, regularFont, highlightFont, titleFont, "", Shared.characters, char2Position);
                character3 = new CharacterEntry(g, spriteBatch, regularFont, highlightFont, titleFont, "", Shared.characters, char3Position);
                this.Components.Add(character1);
                this.Components.Add(character2);
                this.Components.Add(character3);
            }
        }

        /// <summary>
        /// A summary of the previous game is displayed, and if the player achieved
        /// a new high score, name entry options are shown.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(titleFont, "Game Over", titlePosition, Color.Black);
            string scoreInfo = $"Kills   --Score--\n{kills.ToString("D5")}   {score.ToString("D9")}";
            spriteBatch.DrawString(regularFont, scoreInfo, scorePosition, Color.Black);
            if (newHighScoreAchieved)
            {
                spriteBatch.DrawString(regularFont, "New high score!", newHighPosition, Color.Yellow);
                spriteBatch.DrawString(regularFont, "Enter your initials:", namePosition, Color.Yellow);
                if (selectedOption == DONE)
                {
                    spriteBatch.DrawString(highlightFont, "Done", donePosition, Color.Yellow);
                }
                else
                {
                    spriteBatch.DrawString(highlightFont, "Done", donePosition, Color.Black);
                }
            }
            else
            {
                spriteBatch.DrawString(regularFont, "Press space to return\nto the main menu", namePosition, Color.Yellow);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// The Player toggles between the three CharacterEntry menus and the "Done"
        /// option using A and D. 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (newHighScoreAchieved)
            {
                KeyboardState ks = Keyboard.GetState();
                if (ks.IsKeyDown(Keys.D) && oldState.IsKeyUp(Keys.D))
                {
                    selectedOption++;
                    if (selectedOption >= numOptions)
                    {
                        selectedOption = 0;
                    }
                }
                if (ks.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                {
                    selectedOption--;
                    if (selectedOption < 0)
                    {
                        selectedOption = numOptions - 1;
                    }
                }
                character1.ComponentSelected = false;
                character2.ComponentSelected = false;
                character3.ComponentSelected = false;
                if (selectedOption == FIRST)
                {
                    character1.ComponentSelected = true;
                }
                else if (selectedOption == SECOND)
                {
                    character2.ComponentSelected = true;
                }
                else if (selectedOption == THIRD)
                {
                    character3.ComponentSelected = true;
                }
                oldState = ks;
                name = character1.selectedValue + character2.selectedValue + character3.selectedValue;
            }
            base.Update(gameTime);
        }
    }
}
