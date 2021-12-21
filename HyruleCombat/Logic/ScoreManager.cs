using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HyruleCombat.Logic
{
    /// <summary>
    /// Load, update, and access saved high scores.
    /// </summary>
    public abstract class ScoreManager
    {
        // Scores are string arrays with three values, in order:
        // player name, number of kills, score
        private static List<string[]> highScores = new List<string[]>();
        private static int numberOfScores = 10;
        private static int lowestHighScore;
        private static string filename = "highscores.txt";
        private static char delimiter = ',';
        private static string[] newestScore;

        public static List<string[]> HighScores { get => highScores; set => highScores = value; }
        public static int NumberOfScores { get => numberOfScores; set => numberOfScores = value; }
        public static int LowestHighScore { get => lowestHighScore; set => lowestHighScore = value; }
        public static string[] NewestScore { get => newestScore; set => newestScore = value; }

        /// <summary>
        /// Access the high score save file and load the scores. If the file does not
        /// exist, create it using the default high scores. Use the default high scores
        /// if there is an error reading the file.
        /// </summary>
        public static void LoadHighScores()
        {
            if (File.Exists(filename))
            {
                StreamReader reader = new StreamReader(filename);
                try
                {
                    for (int i = 0; i < numberOfScores; i++)
                    {
                        string[] score = reader.ReadLine().Split(delimiter);
                        highScores.Add(score);
                    }
                }
                catch (Exception)
                {
                    highScores = GetDefaultScores();
                }
                SortHighScores();
                reader.Close();
            }
            else
            {
                highScores = GetDefaultScores();
                SaveHighScores();
            }
            lowestHighScore = int.Parse(highScores[numberOfScores - 1][2]);
        }

        /// <summary>
        /// Write the current high scores to the save file.
        /// </summary>
        public static void SaveHighScores()
        {
            StreamWriter writer = new StreamWriter(filename);
            try
            {
                foreach (string[] score in highScores)
                {
                    writer.WriteLine($"{score[0]}{delimiter}{score[1]}{delimiter}{score[2]}");
                }
            }
            catch
            {
                return;
            }
            writer.Close();
        }

        /// <summary>
        /// Replace the lowest high score with a newly achieved high score.
        /// </summary>
        /// <param name="name">Name of new high score.</param>
        /// <param name="killCount">Kill count of new high score.</param>
        /// <param name="score">Score value of new high score.</param>
        public static void AddHighScore(string name, int killCount, int score)
        {
            string[] newScore = {name, killCount.ToString(), score.ToString() };
            highScores[numberOfScores - 1] = newScore;
            SortHighScores();
            SaveHighScores();
            newestScore = newScore;
            // Forgot this line :(
            lowestHighScore = int.Parse(highScores[numberOfScores - 1][2]);
        }

        /// <summary>
        /// Create a default list of high scores 
        /// </summary>
        /// <returns></returns>
        public static List<string[]> GetDefaultScores()
        {
            List<string[]> defaultScores = new List<string[]>();
            defaultScores.Add(new string[] { "ABC", "100", "10000" });
            defaultScores.Add(new string[] { "DEF", "90", "9000" });
            defaultScores.Add(new string[] { "GHI", "80", "8000" });
            defaultScores.Add(new string[] { "JKL", "70", "7000" });
            defaultScores.Add(new string[] { "MNO", "60", "6000" });
            defaultScores.Add(new string[] { "PQR", "50", "5000" });
            defaultScores.Add(new string[] { "STU", "40", "4000" });
            defaultScores.Add(new string[] { "VWX", "30", "3000" });
            defaultScores.Add(new string[] { "YZ!", "20", "2000" });
            defaultScores.Add(new string[] { "123", "10", "1000" });
            return defaultScores;
        }

        /// <summary>
        /// Sorts the high scores by score in descending order.
        /// </summary>
        public static void SortHighScores()
        {
            highScores = highScores.OrderByDescending(s => int.Parse(s[2])).ToList();
        }
    }
}
