//Path is my implementation of the A* Pathfinding Algorithm. A good explanation of how it works can be found here: https://www.geeksforgeeks.org/a-search-algorithm/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;
public static class PathFinder
{
    public static Vector2[] Path(Vector2 startPos, Vector2 endPos, MoveType movementType, int maxMoves = -1)
    {
        if (startPos == endPos)
        {
            return new Vector2[0];
        }

        List<PathFinderNode> closedList = new List<PathFinderNode>();
        //Add Start to Open list
        NodeHeap openList = new NodeHeap(new PathFinderNode(startPos, startPos, endPos, null));
        //Loop until we find the end square or the open list is empty (no path)
        while (openList.Count() > 0)
        {
            //Find the lowest F square (node) on the open list
            PathFinderNode node = openList.Pop();

            //Move that square to the closed list
            closedList.Add(node);

            //For each of the 4 directions (D)
            for (int i = 0; i < 4; i++)
            {
                //Make a new Node in the given Direction
                PathFinderNode directionNode = new PathFinderNode(node.pos + Functions.DirectionToVector((Direction)i), startPos, endPos, node);

                //if directionNode is the end state, end the loop
                if (directionNode.pos == endPos)
                {
                    return directionNode.ReturnPath();
                }

                //If it that directional square (D) isn't in the closed list and we're allowed to move there (Still need to add that)
                //The g is currently hardcoded @ 10, but now the pathfinder won't try to pathfind so far it breaks
                if (!closedList.Contains(directionNode) && directionNode.g < 10
                    && TileManager.TileAt(directionNode.pos).CanPassThrough(movementType))
                {
                    //If it is on the open list
                    if (openList.Contains(directionNode))
                    {
                        //Calculate if this new path is better

                    }
                    //If it isn't on the open list
                    else
                    {
                        //Add it
                        openList.Push(directionNode);
                    }
                }
            }
        }
        //if Open List is empty, We can't find a path
        return null;
    }
    public static bool Possible(Vector2 startPos, Vector2 endPos, MoveType moveType, int maxMoves = -1)
    {
        if (startPos == endPos)
        {
            return true;
        }

        else if (!TileManager.TileAt(endPos).CanStopHere(moveType))
        {
            return false;
        }

        Vector2[] moves = Path(startPos, endPos, moveType);
        if (moves == null)
        {
            return false;
        }
        int movesTaken = moves.Length;

        return movesTaken <= maxMoves + 1;

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

public class NodeHeap
{
    List<PathFinderNode> nodes = new List<PathFinderNode>();
    List<Vector2> storedPositions = new List<Vector2>();

    public NodeHeap(PathFinderNode root)
    {
        nodes.Add(root);
        storedPositions.Add(root.pos);
    }

    public int Count()
    {
        return nodes.Count;
    }

    public bool Contains(PathFinderNode node)
    {
        return storedPositions.Contains(node.pos);
    }

    void MinHeapify(int index)
    {
        int leftIndex = index * 2 + 1;
        int rightIndex = index * 2 + 2;
        int smallestValueIndex = index;

        if (leftIndex < nodes.Count && nodes[leftIndex].f < nodes[smallestValueIndex].f)
        {
            smallestValueIndex = leftIndex;
        }
        if (rightIndex < nodes.Count && nodes[rightIndex].f < nodes[smallestValueIndex].f)
        {
            smallestValueIndex = rightIndex;
        }

        if (index != smallestValueIndex)
        {
            PathFinderNode temp = nodes[index];
            nodes[index] = nodes[smallestValueIndex];
            nodes[smallestValueIndex] = temp;
            MinHeapify(smallestValueIndex);
        }

    }

    public int Parent(int index)
    {
        if (index == 0)
        {
            return 0;
        }
        return (index - 1) / 2;
    }

    public void Push(PathFinderNode node)
    {
        nodes.Add(node);
        storedPositions.Add(node.pos);
        while (nodes[Parent(nodes.IndexOf(node))].f > node.f)
        {
            int parentIndex = Parent(nodes.IndexOf(node));
            PathFinderNode temp = nodes[parentIndex];
            nodes[parentIndex] = node;
            nodes[nodes.Count - 1] = temp;
        }
    }

    public PathFinderNode Pop()
    {
        PathFinderNode output = nodes[0];
        storedPositions.Remove(output.pos);
        nodes[0] = nodes[nodes.Count - 1];
        MinHeapify(0);
        nodes.RemoveAt(nodes.Count - 1);
        /*string debug = $"Removed the root {output.f} from the list";
        foreach (PathFinderNode node in nodes)
        {
            debug += ", " + node.f;
        }
        Debug.Log(debug);*/
        return output;
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

//Old implementations
//Storing them here for now just in case

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

/*public static Vector2[] Path(Vector2 startPos, Vector2 endPos, MoveType condition)
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

            References.uManager.EndTurn();
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
*/
