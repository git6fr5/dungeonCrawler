using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate2D
{
    [HideInInspector] public int i;
    [HideInInspector] public int j;
    [HideInInspector] public float distance;
    [HideInInspector] public float heuristic;
    [HideInInspector] public Coordinate2D endPath;
    [HideInInspector] public Coordinate2D startPath;

    public Coordinate2D(int _i, int _j)
    {
        i = _i;
        j = _j;
    }

    public Coordinate2D(int _i, int _j, Coordinate2D[][] coordinateGrid)
    {
        i = _i;
        j = _j;
        coordinateGrid[i][j] = this;
    }

    // corners
    public static Coordinate2D northEast = new Coordinate2D(1, 1);
    public static Coordinate2D northWest = new Coordinate2D(1, -1);
    public static Coordinate2D southEast = new Coordinate2D(-1, 1);
    public static Coordinate2D southWest = new Coordinate2D(-1, -1);

    // cardinals
    public static Coordinate2D north = new Coordinate2D(1, 0);
    public static Coordinate2D south = new Coordinate2D(-1, 0);
    public static Coordinate2D east = new Coordinate2D(0, 1);
    public static Coordinate2D west = new Coordinate2D(0, -1);

    public static Coordinate2D[] directions = new Coordinate2D[] { north, northEast, east, southEast, south, southWest, west, northWest };

    public Coordinate2D Move(Coordinate2D direction, int[][] grid, Coordinate2D[][] coordinateGrid)
    {
        if (!IsValidMove(i + direction.i, j + direction.j, coordinateGrid)) { return null; } // Check if its a valid coordinate
        if (!IsEmpty(i + direction.i, j + direction.j, grid)) { return null; } // Check if its an empt
        if (coordinateGrid[i + direction.i][j + direction.j] != null) 
        {
            return coordinateGrid[i + direction.i][j + direction.j]; // Return the coordinate if it already exists
        }
        return new Coordinate2D(i + direction.i, j + direction.j, coordinateGrid); // Return a new coordinate if it doesn't exist
    }

    public List<Coordinate2D> Adjacent(int[][] grid, Coordinate2D[][] coordinateGrid)
    {
        List<Coordinate2D> adjacent = new List<Coordinate2D>();
        foreach (Coordinate2D direction in directions)
        {
            Coordinate2D adjacentNode = this.Move(direction, grid, coordinateGrid);
            if (adjacentNode != null)
            {
                adjacent.Add(adjacentNode);
            }
        }
        return adjacent;
    }

    public bool IsValidMove(int _i, int _j, Coordinate2D[][] coordinateGrid)
    {
        //Debug.Log(String.Format("{0}, {1}", i, j));
        return (_i < coordinateGrid.Length && _i >= 0 && _j < coordinateGrid[0].Length && _j >= 0);
    }

    public bool IsEmpty(int _i, int _j, int[][] grid)
    {
        //Debug.Log(String.Format("{0}, {1}", i, j));
        return (grid[_i][_j] == 0);
    }

    public void SetHeuristic(float _heuristic)
    {
        if (_heuristic < heuristic || heuristic == 0)
        {
            heuristic = _heuristic;
        }
    }

    public void SetDistance(float _distance)
    {
        if (_distance < distance || distance == 0)
        {
            distance = _distance;
        }
    }
}
