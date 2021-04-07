using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;

public class Pathfind2D : MonoBehaviour
{

    static float waitTime = 0.02f;

    public int[][] AStar(int[][] grid, List<Coordinate2D> nodes)
    {
        // Discard nodes that can't be pathed to
        for (int i = nodes.Count - 1; i >= 0; i--)
        {
            bool isPathable = false;
            foreach (Coordinate2D adjacentCell in nodes[i].Adjacent())
            {
                if (adjacentCell.IsValid(grid) && adjacentCell.IsEmpty(grid))
                {
                    isPathable = true;
                    break;
                }
            }
            if (!isPathable && nodes[i].IsValid(grid))
            {
                grid[nodes[i].i][nodes[i].j] = 15;
                nodes.RemoveAt(i);
            }
        }

        // Check there are atleast 2 nodes left
        if (nodes.Count < 1) { return grid; }

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Coordinate2D start = nodes[i];
            Coordinate2D end = nodes[i + 1];

            List<Coordinate2D> trail = new List<Coordinate2D>();
            StartCoroutine(IEAstar(waitTime, grid, start, end, trail, 0));
        }


        return grid;
    }

    public IEnumerator IEAstar(float elapsedTime, int[][] grid, Coordinate2D node, Coordinate2D end, List<Coordinate2D> trail, int numItterations)
    {
        yield return new WaitForSeconds(elapsedTime);

        float minHeuristic = grid.Length * grid[0].Length;
        Coordinate2D _node = node;

        foreach (Coordinate2D adjacentNode in node.Adjacent())
        {
            if (adjacentNode.IsValid(grid) && adjacentNode.IsEmpty(grid))
            {
                float heuristic = ManhattanDistance(adjacentNode, end);
                if (heuristic < minHeuristic)
                {
                    _node = adjacentNode;
                    minHeuristic = heuristic;
                }
            }
        }

        trail = _node.MakePathTo(grid, end, minHeuristic, trail);

        if (_node == end || minHeuristic == 1 || numItterations > grid.Length * grid[0].Length)
        {
            yield return null;
        }
        else
        {
            Coordinate2D nextNode = GetNextNode(_node, minHeuristic, trail);
            StartCoroutine(IEAstar(waitTime, grid, nextNode, end, trail, numItterations+1));
        }

        yield return null;
    }

    public Coordinate2D GetNextNode(Coordinate2D node, float distance, List<Coordinate2D> trail )
    {
        for (int i = 0; i < trail.Count; i++)
        {
            if (trail[i].distance < distance)
            {
                node = trail[i];
                distance = trail[i].distance;
            }

        }
        return node;
    }

    public float EuclideanDistance(Coordinate2D node1, Coordinate2D node2)
    {
        return (float)(node1.i - node2.i) * (node1.i - node2.i) + (node1.j - node2.j) * (node1.j - node2.j);
    }

    public float ManhattanDistance(Coordinate2D cell1, Coordinate2D cell2)
    {
        return (float)(Mathf.Abs((cell1.i - cell2.i)) + Mathf.Abs((cell1.j - cell2.j)));
    }
}
