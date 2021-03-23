using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : Dungeon
{
    /* --- Components --- */
    public Text textBox;

    /* --- Internal Variables --- */
    public int sizeVertical = 20;
    public int sizeHorizontal = 20;
    [HideInInspector] public int[][] grid;
    [HideInInspector] public int[][] innerCoords;
    [HideInInspector] public int[][] wallLines = new int[4][];

    private float startTime;
    private float endTime;

    /* --- Unity Methods --- */
    void Start()
    {
        SetGrid();
        ConstructGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FillWalls();
        }
    }

    /* --- Methods --- */
    void SetGrid()
    {
        grid = new int[sizeVertical][];
        for (int i = 0; i < sizeVertical; i++)
        {
            grid[i] = new int[sizeHorizontal];
        }

        PrintGrid();
    }

    void ConstructGrid()
    {
        SetInnerBounds();
        FillInnerBounds();

        SetWallLine(0, 0, innerCoords[0][1], sizeVertical); // innerX running along Y
        SetWallLine(1, 0, innerCoords[0][0], sizeHorizontal); // innerY running along X
        SetWallLine(2, innerCoords[1][1], sizeHorizontal, sizeVertical); // upperX running along Y
        SetWallLine(3, innerCoords[1][0], sizeVertical, sizeHorizontal); // upperY running along X
        DrawWallLines();
    }

    void SetInnerBounds()
    {
        int[] coord1 = new int[] { Random.Range(1, (int)(sizeVertical / 2)), Random.Range(1, (int)(sizeHorizontal / 2)) };
        int[] coord2 = new int[] { Random.Range(coord1[0] + 2, sizeVertical - 1), Random.Range(coord1[1] + 2, sizeHorizontal - 1) };
        innerCoords = new int[2][] { coord1, coord2 };

        grid[coord1[0]][coord1[1]] = 2;
        grid[coord2[0]][coord2[1]] = 2;

        PrintGrid();
    }

    void FillInnerBounds()
    {
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < sizeHorizontal; j++)
            {
                if (CheckInner(i, j))
                {
                    grid[i][j] = 1;
                }
            }
        }

        PrintGrid();
    }

    bool CheckInner(int i, int j)
    {
        int[] coord1 = innerCoords[0];
        int[] coord2 = innerCoords[1];

        if (i < coord1[0] || i > coord2[0])
        {
            return false;
        }
        if (j < coord1[1] || j > coord2[1])
        {
            return false;
        }
        return true;
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
        for (int i = 0; i < sizeVertical; i++)
        {
            grid[i][wallLines[0][i]] = 3;
            grid[i][wallLines[2][i]] = 4;
        }
        for (int j = 0; j < sizeHorizontal; j++)
        {
            grid[wallLines[1][j]][j] = 5;
            grid[wallLines[3][j]][j] = 6;
        }
        PrintGrid();
    }

    void FillWalls()
    {
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < sizeHorizontal; j++)
            {
                int minHor = wallLines[0][i]; // 3
                int maxHor = wallLines[2][i]; // 4
                int minVert = wallLines[1][j]; // 5
                int maxVert = wallLines[3][j]; // 6
                if (j > minHor && j < maxHor && i > minVert && i < maxVert)
                {
                    grid[i][j] = 1;
                }
            } 
        }
        PrintGrid();
    }

    void PrintGrid()
    {
        string text = "";
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < sizeHorizontal; j++)
            {
                text = text + grid[i][j].ToString() + "  ";
            }
            text = text + "\n";
        }
        textBox.text = text;
    }
}
