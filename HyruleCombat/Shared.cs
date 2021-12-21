using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat
{
    /// <summary>
    /// Properties and utilities used in various classes.
    /// </summary>
    public abstract class Shared
    {
        public static Vector2 stage;
        public static Rectangle stageRect;

        public static Rectangle spawnerZoneTop = new Rectangle(185, 20, 335, 30);
        public static Rectangle spawnerZoneBottom = new Rectangle(185, 435, 335, 30);
        public static Rectangle spawnerZoneLeft = new Rectangle(20, 88, 30, 325);
        public static Rectangle spawnerZoneRight = new Rectangle(740, 88, 30, 325);

        public static string[] characters = { "A", "B", "C", "D", "E", "F",
                                              "G", "H", "I", "J", "K", "L",
                                              "M", "N", "O", "P", "Q", "R",
                                              "S", "T", "U", "V", "W", "X",
                                              "Y", "Z", "!", "?", "-",};

        /// <summary>
        /// Returns a list of rectangles representing the animation frames from a
        /// desired row ofw a texture.
        /// </summary>
        /// <param name="columns">Number of columns in the row.</param>
        /// <param name="rowDimensions">Height and Width of the row.</param>
        /// <param name="rowNumber">The row number to obtain frames from (starting at 0,
        /// counting from the top row down).</param>
        /// <returns>A list of rectangles.</returns>
        public static List<Rectangle> GetFramesOneRow(Vector2 rowDimensions, int columns, int rowNumber)
        {
            List<Rectangle> frames = new List<Rectangle>();

            Vector2 frameDimensions = new Vector2(rowDimensions.X / columns, rowDimensions.Y);

            int y = (int)frameDimensions.Y * rowNumber;
            for (int i = 0; i < columns; i++)
            {
                int x = (int)frameDimensions.X * i;
                Rectangle newFrame = new Rectangle(x, y, (int)frameDimensions.X, (int)frameDimensions.Y);
                frames.Add(newFrame);
            }

            return frames;
        }
    }
}
