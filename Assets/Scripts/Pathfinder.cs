//Path is my implementation of the A* Pathfinding Algorithm. A good explanation of how it works can be found here: https://www.geeksforgeeks.org/a-search-algorithm/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;
public static class PathFinder
{
    /*   public static Vector2[] Path(Vector2 startPos, Vector2 endPos)
       {
           List<PathFinderNode> openList = new List<PathFinderNode>();
           List<PathFinderNode> closedList = new List<PathFinderNode>();

           foreach (Vector2 pos in TilePatterns.FindArea(startPos, 1, 1))
           {
               if (TileManager.IsTileOpen(pos))
               {
                   openList.Add(new PathFinderNode(pos, startPos, endPos, startPos));
               }
           }

           while (openList.Count > 0)
           {

               //Chose the node with the lowest F 
               PathFinderNode q = openList[0];
               int lowestF = openList[0].f;
               foreach (PathFinderNode option in openList)
               {
                   if (option.f < lowestF)
                   {

                       lowestF = option.f;
                       q = option;
                   }
               }

               if (lowestF > Functions.GridDistance(startPos, endPos))
               {
                   break;
                   //continue;
               }

               openList.Remove(q);
               foreach (Vector2 pos in TilePatterns.FindArea(q.pos, 1, 1))
               {
                   PathFinderNode potentialNode = new PathFinderNode(pos, startPos, endPos, q.pos);
                   bool bestPath = TileManager.IsTileOpen(pos);
                   if (bestPath)
                   {
                       if (pos == endPos)
                       {
                           List<Vector2> path = new List<Vector2>();
                           Vector2 newAddition = pos;
                           if (newAddition != startPos)
                           {
                               path.Insert(0, newAddition);
                           }
                           return path.ToArray();
                       }
                       foreach (PathFinderNode option in openList)
                       {
                           if (option.f <= potentialNode.f)
                           {
                               bestPath = false;
                               break;
                           }
                       }
                       if (bestPath)
                       {
                           foreach (PathFinderNode option in closedList)
                           {
                               if (option.f <= potentialNode.f)
                               {
                                   bestPath = false;
                                   break;
                               }
                           }
                           if (bestPath)
                           {
                               openList.Add(potentialNode);
                           }
                       }
                   }
               }
               closedList.Add(q);

           }
           return new Vector2[0];
           //return false;
       }
   */

    public static Vector2[] Path(Vector2 startPos, Vector2 endPos, MoveType condition)
    {
        if(startPos == endPos)
        {
            return new Vector2[0];
        }

        List<PathFinderNode> openList = new List<PathFinderNode>();
        List<PathFinderNode> closedList = new List<PathFinderNode>();

        //Put start location on the open List
        openList.Add(new PathFinderNode(startPos, startPos, endPos, null));

        int time = DateTime.Now.Minute * 60 + DateTime.Now.Second;

        //Loop until the open List is empty
        while (openList.Count > 0)
        {
            if(DateTime.Now.Minute * 60 + DateTime.Now.Second > 5 + time)
            {
                Debug.LogError("Uh oh! You took to long to pathfind. The Time police are stopping this operation");
                Debug.Log("I see you called for a report sir, here are the details");
                Debug.Log("StartPos: " + startPos);
                Debug.Log("EndPos: " + endPos);
                Debug.Log("Condition " + condition.ToString());
                return null;
            }
            //Find the node with the smallest f on the open list
            PathFinderNode q = openList[0];

            foreach (PathFinderNode node in openList)
            {
                if (node.f < q.f)
                {
                    q = node;
                }
                else if(node.f == q.f && node.h < q.h)
                {
                    q = node;
                }
            }
            //Remove that node from the open list
            openList.Remove(q);

            //Generate 4 cardinal children
            foreach (Vector2 newPos in TilePatterns.Range(q.pos, 1, 1))
            {
                //Only move in a direction if it is possible to move that way
                if (TileManager.TileAt(newPos).CanPassThrough(condition))
                {
                    PathFinderNode possibleNode = new PathFinderNode(newPos, startPos, endPos, q);
                    //If this is the goal
                    if (newPos == endPos)
                    {
                        //End the search and find the path
                        return possibleNode.ReturnPath();
                    }

                    //Don't add this node if it exists on the open or closed list with a smaller f
                    bool bestPath = true;
                    foreach (PathFinderNode node in openList)
                    {
                        //This maybe should be <= for optimization but I am not sure at the moment
                        if (node.pos == possibleNode.pos && node.f < possibleNode.f)
                        {

                            bestPath = false;
                            break;
                        }
                    }
                    if (bestPath)
                    {
                        foreach (PathFinderNode node in closedList)
                        {
                            //This maybe should be <= for optimization but I am not sure at the moment
                            if (node.pos == possibleNode.pos && node.f < possibleNode.f)
                            {
                                bestPath = false;
                                break;
                            }
                        }
                    }

                    if (bestPath)
                    {
                        openList.Add(possibleNode);
                    }
                }
            }

            closedList.Add(q);
        }
        return new Vector2[0];
    }
    public static bool Possible(Vector2 startPos, Vector2 endPos, MoveType moveType, int maxMoves = -1)
    {
        if (startPos == endPos)
        {
            return true;
        }

        else if(!TileManager.TileAt(endPos).CanStopHere(moveType))
        {
            return false;
        }

        Vector2[] moves = Path(startPos, endPos, moveType);
        int movesTaken = moves.Length;

        if (maxMoves < 0)
        {
            return movesTaken != 0;
        }
        else
        {
            return movesTaken != 0 && movesTaken <= maxMoves + 1;
        }
    }

    
   /* public static Vector2[] Area(Vector2 startPos, int range, MoveType condition)
    {
        List<Vector2> validSpaces = new List<Vector2>();
        AreaNode(startPos, 0);
        return validSpaces.ToArray();

        void AreaNode(Vector2 pos, int distance)
        {
            validSpaces.Add(pos);
            foreach (Vector2 newPos in TilePatterns.Range(pos, 1, 1))
            {
                //If the position isn't in valid spaces && distance traveled isn't too high
                if(distance < range )
                {
                    if(!validSpaces.Contains(newPos))
                    AreaNode(newPos, distance + 1);
                }
            }
        }

    }*/
}

public class PathFinderNode
{
    public Vector2 pos;
    public PathFinderNode parent;
    public int f, g, h;

    //f = g + h
    //g = The movement cost from the starting position to this point
    //h = Guess of the movement cost from here to the end position

    public PathFinderNode(Vector2 pos, Vector2 startPos, Vector2 endPos, PathFinderNode parent)
    {
        this.pos = pos;
        this.parent = parent;
        g = ReturnPath().Length;
        h = Functions.GridDistance(pos, endPos);
        f = g + h;
    }

    //I think this works but I have not tested it at all yet
    public Vector2[] ReturnPath()
    {
        if (parent == null)
        {
            return new Vector2[] { pos };
        }
        else
        {
            Vector2[] temp = parent.ReturnPath();
            Vector2[] path = new Vector2[temp.Length + 1];
            Array.Copy(temp, path, temp.Length);
            path[temp.Length] = pos;
            return path;
        }
    }
}
/*
public class AreaNode
{
    public Vector2 pos;
    public int distance;
    public AreaNode(Vector2 pos, int distance)
    {
        this.pos = pos;
        this.distance = distance;
        //add to validSpaces
        foreach (Vector2 newPos in TilePatterns.Range(pos, 1, 1))
        {
            //If the position isn't in valid spaces && distance traveled isn't too high
            new AreaNode(newPos, distance + 1);
        }

    }
}*/