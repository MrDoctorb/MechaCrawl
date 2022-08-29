using UnityEngine;
using System.Collections.Generic;
namespace Zanespace
{
    public delegate void Alert();
    public static class References
    {
        public const int ACTIONTHRESHOLD = 10;
        public static float timeBetweenMove = .25f;
        public static UnitManager uManager;
        public static TileManager tManager;
        public static ActionSelectMenu actionSelectMenu;
    }

    public static class Functions
    {
        public static Vector2 DirectionToVector(Direction direction)
        {
            int dir = (int)direction;
            switch (dir)
            {
                case 0:
                    return Vector2.up;
                case 1:
                    return Vector2.right;
                case 2:
                    return Vector2.down;
                case 3:
                    return Vector2.left;
                default:
                    throw new System.Exception("I don't know how, but your input was an invalid direction");
            }
        }

        public static int GridDistance(Vector2 pos1, Vector2 pos2)
        {
            int x = (int)Mathf.Abs(pos1.x - pos2.x);
            int y = (int)Mathf.Abs(pos1.y - pos2.y);
            return x + y;
        }

        public static Vector2 RotatePointAroundPoint(Vector2 pivot, Vector2 rotatee, Direction endDirection, Direction startDirection = Direction.Up)
        {
            rotatee -= pivot;

            //Big ol Conversion to take our direction and make them opposite radians
            float angle = -((((int) startDirection * 90) + (endDirection - startDirection) * 90) * Mathf.Deg2Rad);

            Vector2 newRotation = new Vector2(rotatee.x * Mathf.Cos(angle) - rotatee.y * Mathf.Sin(angle),
                                              rotatee.x * Mathf.Sin(angle) + rotatee.y * Mathf.Cos(angle)) + pivot;
            return newRotation;
        }

        /* Is this even where this should be? Or should this be on its own script for fog of war (fow)
        public static bool IsOpenLine(Vector2 origin, Vector2 destination)
        {
            foreach(Vector2 tile in TilePatterns.Line(origin, destination))
            {
                if(TileManager.TileAt(tile).CanSeeThrough))
            }
            return true;
        }*/

    }

    public static class TilePatterns
    {

        /// <summary>
        /// Draws a line from origin to destination inclusively along the grid and returns all squares in the line
        /// </summary>
        /// <returns>All squares with the line starting with origin</returns>
        public static Vector2[] Line(Vector2 origin, Vector2 destination)
        {
            List<Vector2> squares = new List<Vector2>();
            squares.Add(origin);

            float dy =(destination.y - origin.y);
            float dx = (destination.x - origin.x);
            int length = Mathf.Abs(dx) > Mathf.Abs(dy) ? (int)dx : (int)dy;
            float slope = dy/dx;
            Vector2 nextPoint = new Vector2(origin.x, origin.y);

            if(Mathf.Abs(slope) >= 1)
            {
                for (int i = 0; i <= length; i++)
                {
                    float temp = origin.x + ((1/slope) * i);
                    int newx = Mathf.RoundToInt(temp);
                    
                    Debug.Log("value of " + temp + "rounded to " +  newx);

                    int newy = (int)origin.y + i;

                    squares.Add(new Vector2(newx, newy));
                }
            }



            return squares.ToArray();
        }

        public static Vector2[] Range(Vector2 origin, int outerRadius, int innerRadius = 0)
        {
            List<Vector2> squares = new List<Vector2>();
            if (innerRadius == 0)
            {
                squares.Add(origin);
            }

            for (int dir = -1; dir <= 1; dir += 2)
            {
                for (int i = 1; i <= outerRadius; i++)
                {
                    if (i >= innerRadius)
                    {
                        squares.Add(origin + new Vector2(0, i) * dir);
                        squares.Add(origin + new Vector2(i, 0) * dir);

                    }
                    for (int j = 1; j <= outerRadius - i; j++)
                    {
                        if (j + i >= innerRadius)
                        {
                            squares.Add(origin + new Vector2(j, i) * dir);
                            squares.Add(origin + new Vector2(i, -j) * dir);
                        }
                    }
                }
            }
            return squares.ToArray();
        }
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public enum MoveType
    {
        Walk,
        Jump,
        Fly,
        Teleport
        //Dig?
    }

    public enum TileType
    {
        BlockNone,
        BlockWalk,
        BlockFly,
        BlockAll
    }
}