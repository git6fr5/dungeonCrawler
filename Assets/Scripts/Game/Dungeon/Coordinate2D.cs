using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate2D
{
    [HideInInspector] public int i;
    [HideInInspector] public int j;
    [HideInInspector] public float distance = 1000000;
    [HideInInspector] public Coordinate2D endPath;

    public Coordinate2D(int _i, int _j)
    {
        i = _i;
        j = _j;
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

    public Coordinate2D Move(Coordinate2D direction)
    {
        return new Coordinate2D(i + direction.i, j + direction.j);
    }

    public List<Coordinate2D> Adjacent()
    {
        List<Coordinate2D> adjacent = new List<Coordinate2D>();
        foreach (Coordinate2D direction in directions)
        {
            adjacent.Add(this.Move(direction));
        }
        return adjacent;
    }

    public bool IsValid(int[][] grid)
    {
        //Debug.Log(String.Format("{0}, {1}", i, j));
        return (i < grid.Length && i > 0 && j < grid[0].Length && j > 0);
    }

    public bool IsEmpty(int[][] grid)
    {
        //Debug.Log(String.Format("{0}, {1}", i, j));
        return (grid[i][j] == 0);
    }

    public List<Coordinate2D> MakePathTo(int[][] grid, Coordinate2D _endPath, float _distance, List<Coordinate2D> trail)
    {
        distance = _distance;
        endPath = _endPath;
        grid[i][j] = 15;
        trail.Add(this);
        return trail;
    }
}
