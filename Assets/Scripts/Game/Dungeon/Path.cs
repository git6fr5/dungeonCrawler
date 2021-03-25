using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Path : Dungeon
{
    /* --- Debug --- */
    [HideInInspector] public bool DEBUG_path = false;

    /* --- Components --- */

    /* --- Internal Variables --- */

    // Direction
    public enum Direction
    {
        left, top, right, bottom
    }

    // Construction
    [HideInInspector] public int length = 20;
    [HideInInspector] public int maxWidth = 10;
    [HideInInspector] public int direction = 1;
    [HideInInspector] public int variance = 3;
    [HideInInspector] public int center;
    [HideInInspector] public int[] centerLine;
    [HideInInspector] public int[][] wallLines = new int[2][];

    /* --- Unity Methods --- */
    public override void Update()
    {
        if (!DEBUG_path) { return; }
    }

    /* --- Methods --- */
    public void ConnectBlocks(int[][] grid, int[][] nodes0, int[][] nodes1)
    {
        int minDist = grid.Length * grid.Length + grid[0].Length * grid[0].Length;
        int[] minNodes = new int[2];

        for (int i = 0; i < nodes0.Length; i++)
        {
            for (int j = 0; j < nodes1.Length; j++)
            {
                int dist = ConnectNodes(grid, nodes0[i], nodes1[j]);
                if (dist < minDist)
                {
                    minDist = dist;
                    minNodes = new int[2] { i, j };
                }
            }
        }

        Direction node0Direction = (Direction)minNodes[0];
        Direction node1Direction = (Direction)minNodes[1];

        int[][] newNodes = ExtendPath(grid, nodes0[minNodes[0]], nodes1[minNodes[1]], node0Direction, node1Direction, 1);
        int[] connectDist = LinearDistances(newNodes[0], newNodes[1]);
        ManhattanPath(grid, newNodes[0], newNodes[1], connectDist[0], connectDist[1]);
    }

    int ConnectNodes(int[][] grid, int[] node0, int[] node1)
    {
        int maxDist = ManhattanDistance(node0, node1);
        return maxDist;
    }

    int ManhattanDistance(int[] node0, int[] node1)
    {
        int dist1 = Mathf.Abs(node0[0] - node1[0]);
        int dist2 = Mathf.Abs(node0[1] - node1[1]);
        return dist1 + dist2;
    }

    int[] LinearDistances(int[] node0, int[] node1)
    {
        print("Calculting distance between " + node0[0].ToString() + ", " + node0[1].ToString() + " and " + node1[0].ToString() + ", " + node1[1].ToString());
        int dist1 = node0[0] - node1[0];
        int dist2 = node0[1] - node1[1];
        return new int[2] { dist1, dist2 };
    }

    int[][] ExtendPath(int[][] grid, int[] node0, int[] node1, Direction node0Direction, Direction node1Direction, int extensionLength)
    {
        int[] move0 = Move(grid, node0[0], node0[1], node0Direction);
        int[] move1 = Move(grid, node1[0], node1[1], node1Direction);

        int[] newNode0 = new int[] { node0[0] + move0[0], node0[1] + move0[1] };
        int[] newNode1 = new int[] { node1[0] + move1[0], node1[1] + move1[1] };

        grid[newNode0[0]][newNode0[1]] = 15;
        grid[newNode1[0]][newNode1[1]] = 15;

        int[][] newNodes = new int[2][] { newNode0, newNode1 };

        if (extensionLength > 0) { newNodes = ExtendPath(grid, newNode0, newNode1, node0Direction, node1Direction, extensionLength - 1); }

        return newNodes;
    }

    void ManhattanPath(int[][] grid, int[] startNode, int[] endNode, int slopeRise, int slopeRun)
    {
        int[] node = startNode;

        Direction directionRise;
        Direction directionRun;

        if (Mathf.Sign(slopeRise) == -1f) { directionRise = Direction.bottom; }
        else { directionRise = Direction.top; }

        if (Mathf.Sign(slopeRun) == 1f) { directionRun = Direction.left; }
        else { directionRun = Direction.right; }

        int valueRise = (int)Mathf.Abs(slopeRise);
        int valueRun = (int)Mathf.Abs(slopeRun);

        print("ValueRise: " + valueRise.ToString());
        print("ValueRun: " + valueRun.ToString());

        print(node[0].ToString() + ", " + node[1].ToString());
        bool isInEmptySpace = true;

        for (int j = 0; j < valueRise; j++)
        {
            int[] move = Move(grid, node[0], node[1], directionRise);
            node = new int[] { node[0] + move[0], node[1] + move[1] };
            isInEmptySpace = SetPath(grid, node, isInEmptySpace);
            if (node == endNode) { return; }

        }

        for (int j = 0; j < valueRun; j++)
        {
            int[] move = Move(grid, node[0], node[1], directionRun);
            node = new int[] { node[0] + move[0], node[1] + move[1] };
            isInEmptySpace = SetPath(grid, node, isInEmptySpace);
            if (node == endNode) { return; }
        }
    }

    int[] Move(int[][] grid, int i, int j, Direction direction)
    {
        if (j == 0 && direction == Direction.left) { return new int[] { 0, 0}; }
        if (j == grid[0].Length && direction == Direction.right) { return new int[] { 0, 0 }; }
        if (i == 0 && direction == Direction.top) { return new int[] { 0, 0 }; }
        if (i == grid.Length && direction == Direction.bottom) { return new int[] { 0, 0 }; }

        if (direction == Direction.left) { return new int[] { 0, -1 }; }
        if (direction == Direction.right) { return new int[] { 0, 1 }; }
        if (direction == Direction.top) { return new int[] { -1, 0 }; }
        if (direction == Direction.bottom) { return new int[] { 1, 0 }; }

        return new int[] { 0, 0 };
    }

    bool SetPath(int[][]grid, int[] node, bool isInEmptySpace)
    {
        if (grid[node[0]][node[1]] == 0)
        {
            grid[node[0]][node[1]] = 15;
            isInEmptySpace = true;
        }
        else if (grid[node[0]][node[1]] < 14)
        {
            if (isInEmptySpace) { grid[node[0]][node[1]] = 14; }
            isInEmptySpace = false;
        }
        return isInEmptySpace;
    }

    Direction GetOppositeDirection(Direction direction)
    {
        int _direction = (int)direction;
        _direction = (_direction + 2) % 4;
        return (Direction)_direction;
    }

    int GetDirectionIndex(Direction direction)
    {
        int _direction = (int)direction;
        int index = (_direction + 1) % 2;
        return index;
    }

}
