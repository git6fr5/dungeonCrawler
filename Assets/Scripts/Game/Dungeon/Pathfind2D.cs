using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;

public class Pathfind2D : MonoBehaviour
{

    static float waitTime = 0.02f;
    public Coordinate2D[][] lastCoordinateGrid;

    void Update()
    {
        if (Input.GetMouseButton(0) && lastCoordinateGrid)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int gridY = -(int)mousePos.y + lastCoordinateGrid.Length;
            int gridX = (int)mousePos.x + lastCoordinateGrid[0].Length;
            if (lastCoordinateGrid[gridY][gridX]) { print(lastCoordinateGrid[gridY][gridX].distance); }
        }
    }

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

            // Initialize a coordinate grid
            Coordinate2D[][] coordinateGrid = new Coordinate2D[grid.Length][];
            for (int i = 0; i < coordinateGrid.Length; i++)
            {
                coordinateGrid[i] = new int[grid[0].Length];
            }

            StartCoroutine(IEAstar(waitTime, grid, start, end, coordinateGrid, 0));
        }


        return grid;
    }

    public IEnumerator IEAstar(float elapsedTime, int[][] grid, Coordinate2D node, Coordinate2D end, Coordinate2D[][] coordinateGrid, int numItterations)
    {
        yield return new WaitForSeconds(elapsedTime);

        float minHeuristic = grid.Length * grid[0].Length;
        Coordinate2D _node = node;

        foreach (Coordinate2D adjacentNode in node.Adjacent())
        {
            if (adjacentNode.IsValid(grid) && adjacentNode.IsEmpty(grid))
            {
                adjacentNode.SetDistance(node.distance + ManhattanDistance(node, adjacentNode));
                float heuristic = ManhattanDistance(adjacentNode, end);
                if (heuristic < minHeuristic)
                {
                    _node = adjacentNode;
                    minHeuristic = heuristic;
                }
            }
        }

        _node.MakePathTo(grid, start, distance, end, minHeuristic, coordinateGrid);

        if (_node == end || minHeuristic == 1 || numItterations > grid.Length * grid[0].Length)
        {
            lastCoordinateGrid = coordinateGrid;
            yield return null;
        }
        else
        {
            Coordinate2D nextNode = GetNextNode(_node, minHeuristic, coordinateGrid);
            StartCoroutine(IEAstar(waitTime, grid, nextNode, end, coordinateGrid, numItterations+1));
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

    public float ManhattanDistance(Coordinate2D node1, Coordinate2D node2)
    {
        return (float)(Mathf.Abs((node1.i - node2.i)) + Mathf.Abs((node1.j - node2.j)));
    }
}
