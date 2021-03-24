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
        if (Input.GetKeyDown("1"))
        {
            SetCenterLine();
            DrawCenterLine();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("2"))
        {
            SetWallLine(0, center - maxWidth, center, length); // innerX running along Y
            SetWallLine(1, center, center + maxWidth, length); // innerY running along X
            DrawWallLines();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("3"))
        {
            FillWalls();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("4"))
        {
            SetCorners();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("0"))
        {
            SetGrid();
            PrintGrid();
            SetTilemap();
        }
    }

    /* --- Methods --- */
    public void Construct(int _maxWidth, int _length, int _direction, bool printGrid = false, bool printTileMap = false)
    {
        maxWidth = _maxWidth;
        length = _length;
        direction = _direction;
        center = (int)(maxWidth * variance / 2);

        SetGrid();
        SetCenterLine();
        DrawCenterLine();
        SetWallLine(0, center - maxWidth, center, length); // innerX running along Y
        SetWallLine(1, center, center + maxWidth, length); // innerY running along X
        DrawWallLines();
        FillWalls();
        SetCorners();

        if (printGrid) { PrintGrid(); }
        if (printTileMap) { SetTilemap(); }
    }

    public override void SetGrid()
    {
        int[] shapeList = new int[] { 0, 0 };
        shapeList[direction] = maxWidth * variance;
        shapeList[(direction + 1) % 2] = length;
        sizeVertical = shapeList[0];
        sizeHorizontal = shapeList[1];

        grid = new int[sizeVertical][];
        for (int i = 0; i < sizeVertical; i++)
        {
            grid[i] = new int[sizeHorizontal];
        }
    }

    void SetCenterLine()
    {
        int prevPoint = center;
        int[] line = new int[length];

        for (int i = 0; i < length; i++)
        {
            int point = prevPoint + Random.Range(-1, 2);
            if (point < center - maxWidth)
            {
                point = point + 1;
            }
            else if (point > center + maxWidth)
            {
                point = point - 1;
            }
            line[i] = point;
        }
        centerLine = line;
    }

    void DrawCenterLine()
    {
        for (int i = 0; i < length; i++)
        {
            if (direction == 1)
            {
                grid[i][centerLine[i]] = 1;
            }
            else if (direction == 0)
            {
                grid[centerLine[i]][i] = 1;
            }
        }
    }

    void SetWallLine(int index, int innerBound, int upperBound, int axisLength)
    {
        int prevPoint = Random.Range(innerBound, upperBound);
        int[] wallLine = new int[axisLength];
        for (int i = 0; i < axisLength; i++)
        {
            int point = prevPoint + Random.Range(-1, 2);
            if (point > upperBound - 1) { point = point - 1; }
            if (point < innerBound + 1) { point = point + 1; }
            wallLine[i] = point;
            prevPoint = point;
        }
        wallLines[index] = wallLine;
    }

    void DrawWallLines()
    {
        for (int i = 0; i < length; i++)
        {
            if (direction == 1)
            {
                grid[i][wallLines[0][i]] = 2;
                grid[i][wallLines[1][i]] = 3;
            }
            else if (direction == 0)
            {
                grid[wallLines[0][i]][i] = 4;
                grid[wallLines[1][i]][i] = 5;
            }
        }
    }

    void FillWalls()
    {
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < sizeHorizontal; j++)
            {
                int k;
                int l;
                if (direction == 0)
                {
                    k = j;
                    l = i; 
                }
                else
                {
                    k = i;
                    l = j;
                }
                int min = wallLines[0][k]; // 3 left
                int max = wallLines[1][k]; // 4 right

                if (l > min && l < max)
                {
                    grid[i][j] = 1;
                }
            }
        }
    }

    void SetCorners()
    {
        for (int l = 1; l < length-1; l++)
        {
            int[] k = new int[] { wallLines[0][l], wallLines[1][l] };
            if (direction == 0)
            {
                CheckCorner(k[0], l, new int[] { 0, 1 }, new int[] { 6, 7, 8 }); // right/left corners along top wall
                CheckCorner(k[1], l, new int[] { 0, 1 }, new int[] { 9, 10, 11 }); // right/left corners along top wall
            }
            if (direction == 1)
            {
                CheckCorner(l, k[0], new int[] { 1, 0 }, new int[] { 10, 7, 12 }); // top/bottom corners along right wall
                CheckCorner(l, k[1], new int[] { 1, 0 }, new int[] { 9, 6, 13 }); // top/bottom corners along left wall
            }
        }
    }

    void CheckCorner(int i, int j, int[] indexes, int[] values)
    {
        int value = -1;
        if (grid[i + indexes[0]][j + indexes[1]] == 0)
        {
            value = 0;
        }
        if (grid[i - indexes[0]][j - indexes[1]] == 0)
        {
            value = 1;
        }
        if (grid[i - indexes[0]][j - indexes[1]] == 0 && grid[i + indexes[0]][j + indexes[1]] == 0)
        {
            value = 2;
        }

        if (value == -1)
        {
            return;
        }

        grid[i][j] = values[value];
    }
}
