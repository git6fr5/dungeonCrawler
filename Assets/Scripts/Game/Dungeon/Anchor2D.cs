using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor2D
{
    float vertical; float horizontal;

    public Anchor2D(float _vertical, float _horizontal)
    {
        vertical = _vertical;
        horizontal = _horizontal;
    }

    // center
    public static Anchor2D center = new Anchor2D(0.5f, 0.5f);

    // corners
    public static Anchor2D northEast = new Anchor2D(0f, 1f);
    public static Anchor2D northWest = new Anchor2D(0f, 0f);
    public static Anchor2D southEast = new Anchor2D(1f, 1f);
    public static Anchor2D southWest = new Anchor2D(1f, 0f);

    // cardinals
    public static Anchor2D north = new Anchor2D(0f, 0.5f);
    public static Anchor2D south = new Anchor2D(1f, 0.5f);
    public static Anchor2D east = new Anchor2D(0.5f, 0f);
    public static Anchor2D west = new Anchor2D(0.5f, 1f);

    public static Anchor2D[] anchors = new Anchor2D[] { center, north, northEast, east, southEast, south, southWest, west, northWest };

    /* --- Methods --- */

    public int[] AnchorToGrid(int[][] grid, int[][] subGrid)
    {
        int[] _anchor = new int[2];
        _anchor[0] = (int)(vertical * (grid.Length - subGrid.Length));
        _anchor[1] = (int)(horizontal * (grid[0].Length - subGrid[0].Length));
        return _anchor;
    }

}
