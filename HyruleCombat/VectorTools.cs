using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat
{
    /// <summary>
    /// Tools used to calculate angles and vectors.
    /// </summary>
    public abstract class VectorTools
    {
        /// <summary>
        /// Calculates the angle formed by joining two given points.
        /// </summary>
        /// <param name="origin">The first point.</param>
        /// <param name="destination">The second point.</param>
        /// <returns>Angle between origin and destination.</returns>
        public static float GetAngle(Vector2 origin, Vector2 destination)
        {
            float angle;

            float xDifference = destination.X - origin.X;
            float yDifference = destination.Y - origin.Y;
            float deviation = 0;
            if (xDifference < 0)
            {
                deviation = (float)Math.PI;
            }
            angle = (float)Math.Atan(yDifference / xDifference) + deviation;
            // Convert any negative angle to an equivalent positive value by adding 2pi
            // This ensures all angles will be between 0 and 2pi, which is considered 
            // in calculations such as the direction the player is to face.
            if (angle < 0)
            {
                angle += 2 * (float)Math.PI;
            }

            return angle;
        }

        /// <summary>
        /// Given an angle and a magnitude (speed), calculate a vector representing 
        /// the change that must be applied to an original point's x and y values 
        /// to move it to a destination point.
        /// </summary>
        /// <param name="angle">The angle formed between points.</param>
        /// <param name="speed">The distance between points (magnitude).</param>
        /// <returns>A Vector2 that can be added to a Vector2 to get a Vector2 that is
        /// at a distance from the original point equal to the angle and magnitude.</returns>
        public static Vector2 GetVelocity(float angle, float speed)
        {
            float xChange = speed * (float)Math.Cos(angle);
            float yChange = speed * (float)Math.Sin(angle);
            return new Vector2(xChange, yChange);
        }

        // Diagram of Cones
        //  *       |	    *
        //    *     |     *
        //      *   | 4 *
        //      3 * | *  
        //----------+----------
        //        * | * 1 
        //      * 2 |   *
        //    *     |     *
        //  *       |       *

        /// <summary>
        /// Checks to see if angle falls within Cone 1 (Q1 bottom and Q4 top).
        /// </summary>
        /// <param name="angle">Angle being checked.</param>
        /// <returns>True if angle falls within range.</returns>
        public static bool CheckFacingRight(float angle)
        {
            if (angle >= 7 * Math.PI / 4 || (angle >= 0 && angle < Math.PI / 4))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if angle falls within Cone 2 (Q1 right and Q2 left).
        /// </summary>
        /// <param name="angle">Angle being checked.</param>
        /// <returns>True if angle falls within range.</returns>
        public static bool CheckFacingDown(float angle)
        {
            if (angle >= Math.PI / 4 && angle < 3 * Math.PI / 4)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if angle falls within Cone 3 (Q2 bottom and Q3 top).
        /// </summary>
        /// <param name="angle">Angle being checked.</param>
        /// <returns>True if angle falls within range.</returns>
        public static bool CheckFacingLeft(float angle)
        {
            if (angle >= 3 * Math.PI / 4 && angle < 5 * Math.PI / 4)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if angle falls within Cone 4 (Q3 left and Q4 right).
        /// </summary>
        /// <param name="angle">Angle being checked.</param>
        /// <returns>True if angle falls within range.</returns>
        public static bool CheckFacingUp(float angle)
        {
            if (angle >= 5 * Math.PI / 4 && angle < 7 * Math.PI / 4)
            {
                return true;
            }
            return false;
        }
    }
}
