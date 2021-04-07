using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;

public class Pathfind2D : MonoBehaviour
{

    static float waitTime = 2f;
    public Coordinate2D[][] lastCoordinateGrid;

    void Update()
    {
        if (Input.GetMouseButton(0) && lastCoordinateGrid != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int gridY = -(int)mousePos.y + lastCoordinateGrid.Length;
            int gridX = (int)mousePos.x + lastCoordinateGrid[0].Length;
            if (lastCoordinateGrid[gridY][gridX] != null) { print(lastCoordinateGrid[gridY][gridX].distance); }
        }
    }

    public int[][] AStar(int[][] grid, List<Coordinate2D> nodes)
    {
        // Check there are atleast 2 nodes left
        if (nodes.Count < 1) { return grid; }

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Coordinate2D start = nodes[i];
            Coordinate2D end = nodes[i + 1];

            // Initialize a coordinate grid
            Coordinate2D[][] coordinateGrid = new Coordinate2D[grid.Length][];
            for (int j = 0; j < coordinateGrid.Length; j++)
            {
                coordinateGrid[j] = new Coordinate2D[grid[0].Length];
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

        foreach (Coordinate2D adjacentNode in node.Adjacent(grid, coordinateGrid))
        {
            adjacentNode.SetDistance(node.distance + ManhattanDistance(node, adjacentNode));
            adjacentNode.SetHeuristic(ManhattanDistance(adjacentNode, end));
            if (adjacentNode.heuristic < minHeuristic)
            {
                _node = adjacentNode;
                minHeuristic = _node.heuristic;
            }
        }

        grid[_node.i][_node.j] = 15;

        if (_node == end || minHeuristic == 1 || numItterations > grid.Length * grid[0].Length)
        {
            lastCoordinateGrid = coordinateGrid;
            yield return null;
        }
        else
        {
            Coordinate2D nextNode = GetNextNode(_node, coordinateGrid);
            StartCoroutine(IEAstar(waitTime, grid, nextNode, end, coordinateGrid, numItterations+1));
        }

        yield return null;
    }

    public Coordinate2D GetNextNode(Coordinate2D node, Coordinate2D[][] coordinateGrid)
    {
        for (int i = 0; i < coordinateGrid.Length; i++)
        {
            for (int j = 0; j < coordinateGrid[0].Length; j++)
            {
                if (coordinateGrid[i][j] != null && coordinateGrid[i][j].distance + coordinateGrid[i][j].heuristic < node.distance + node.heuristic)
                {
                    node = coordinateGrid[i][j];
                }
            }
        }
        Debug.Log(String.Format("Coordinate {0}, {1} has distance {2}, and heuristic {3}", node.i, node.j, node.distance, node.heuristic));
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
