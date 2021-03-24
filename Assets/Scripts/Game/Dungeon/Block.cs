using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Block : Dungeon
{
    /* --- Debug --- */
    [HideInInspector] public bool DEBUG_block = true;

    /* --- Components --- */

    /* --- Internal Variables --- */

    // Nodes
    public int[] leftNode = new int[2];
    public int[] rightNode = new int[2];
    public int[] topNode = new int[2];
    public int[] bottomNode = new int[2];


    // Construction
    [HideInInspector] public int[][] innerCoords;
    [HideInInspector] public int[][] wallLines = new int[4][];

    /* --- Unity Methods --- */
    public override void Update()
    {
        if (!DEBUG_block) { return; }
        if (Input.GetKeyDown("1"))
        {
            SetInnerBounds();
            FillInnerBounds();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("2"))
        {
            SetWallLine(0, 0, innerCoords[0][1], sizeVertical); // innerX running along Y
            SetWallLine(1, 0, innerCoords[0][0], sizeHorizontal); // innerY running along X
            SetWallLine(2, innerCoords[1][1], sizeHorizontal, sizeVertical); // upperX running along Y
            SetWallLine(3, innerCoords[1][0], sizeVertical, sizeHorizontal); // upperY running along X
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
        if (Input.GetKeyDown("5"))
        {
            CleanWallLines();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("6"))
        {
            SetNodes();
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
    public void Construct(int _sizeVertical, int _sizeHorizontal, bool printGrid = false, bool printTileMap = false)
    {
        sizeVertical = _sizeVertical;
        sizeHorizontal = _sizeHorizontal;

        SetGrid();

        SetInnerBounds();
        SetWallLine(0, 0, innerCoords[0][1], sizeVertical); // innerX running along Y
        SetWallLine(1, 0, innerCoords[0][0], sizeHorizontal); // innerY running along X
        SetWallLine(2, innerCoords[1][1], sizeHorizontal, sizeVertical); // upperX running along Y
        SetWallLine(3, innerCoords[1][0], sizeVertical, sizeHorizontal); // upperY running along X
        DrawWallLines();

        FillWalls();
        SetCorners();
        CleanWallLines();

        SetNodes();

        if (printGrid) { PrintGrid(); }
        if (printTileMap) { SetTilemap(); }
    }

    public override void SetGrid()
    {
        grid = new int[sizeVertical][];
        for (int i = 0; i < sizeVertical; i++)
        {
            grid[i] = new int[sizeHorizontal];
        }
    }

    void SetInnerBounds()
    {
        int[] coord1 = new int[] { Random.Range(1, (int)(sizeVertical / 2)), Random.Range(1, (int)(sizeHorizontal / 2)) };
        int[] coord2 = new int[] { Random.Range(coord1[0] + 2, sizeVertical - 1), Random.Range(coord1[1] + 2, sizeHorizontal - 1) };
        innerCoords = new int[2][] { coord1, coord2 };

        grid[coord1[0]][coord1[1]] = 2;
        grid[coord2[0]][coord2[1]] = 2;

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
            grid[i][wallLines[0][i]] = 2;
            grid[i][wallLines[2][i]] = 3;
        }
        for (int j = 0; j < sizeHorizontal; j++)
        {
            grid[wallLines[1][j]][j] = 4;
            grid[wallLines[3][j]][j] = 5;
        }
    }

    void FillWalls()
    {
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < sizeHorizontal; j++)
            {
                int minHor = wallLines[0][i]; // 3 left
                int maxHor = wallLines[2][i]; // 4 right
                int minVert = wallLines[1][j]; // 5 top
                int maxVert = wallLines[3][j]; // 6 bottom
                if (j > minHor && j < maxHor && i > minVert && i < maxVert)
                {
                    grid[i][j] = 1;
                }
            }
        }
    }

    void CleanWallLines()
    {
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < sizeHorizontal; j++)
            {
                CleanWallCell(i, j);
            }
        }
    }

    void CleanWallCell(int i, int j)
    {
        for (int m = -1; m < 2; m++)
        {
            for (int n = -1; n < 2; n++)
            {
                if (i + m < sizeVertical && i + m > 0 && j + n < sizeHorizontal && j + n > 0)
                {
                    if (grid[i + m][j + n] == 1)
                    {
                        return;
                    }
                }
            }
        }
        grid[i][j] = 0;
    }

    void SetCorners()
    {
        for (int j = 1; j < sizeHorizontal - 1; j++)
        {
            int[] i = new int[] { wallLines[1][j], wallLines[3][j] };
            CheckCorner(i[0], j, new int[] { 0, 1 }, new int[] { 6, 7, 8 }); // right/left corners along top wall
            CheckCorner(i[1], j, new int[] { 0, 1 }, new int[] { 9, 10, 11 }); // right/left corners along top wall
        }

        for (int i = 1; i < sizeVertical - 1; i++)
        {
            int[] j = new int[] { wallLines[0][i], wallLines[2][i] };
            CheckCorner(i, j[0], new int[] { 1, 0 }, new int[] { 10, 7, 12 }); // top/bottom corners along right wall
            CheckCorner(i, j[1], new int[] { 1, 0 }, new int[] { 9, 6, 13 }); // top/bottom corners along left wall
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

    void SetNodes()
    {
        leftNode = SetNode(sizeVertical, "left");
        rightNode = SetNode(sizeVertical, "right");
        topNode = SetNode(sizeHorizontal, "top");
        bottomNode = SetNode(sizeHorizontal, "bottom");

        grid[leftNode[0]][leftNode[1]] = 14;
        grid[rightNode[0]][rightNode[1]] = 14;
        grid[topNode[0]][topNode[1]] = 14;
        grid[bottomNode[0]][bottomNode[1]] = 14;
    }

    int[] SetNode(int axisLength, string node)
    {
        List<int[]> possibleNodes = new List<int[]>();
        for (int i = 0; i < axisLength; i++)
        {
            if (node == "left")
            {
                if (grid[i][wallLines[0][i]] == 2)
                {
                    possibleNodes.Add(new int[2] { i, wallLines[0][i] });
                }
            }
            else if (node == "right")
            {
                if (grid[i][wallLines[2][i]] == 3)
                {
                    possibleNodes.Add(new int[2] { i, wallLines[2][i] });
                }
            }
            else if (node == "top")
            {
                if (grid[wallLines[1][i]][i] == 4)
                {
                    possibleNodes.Add(new int[2] { wallLines[1][i], i });
                }
            }
            else if (node == "bottom")
            {
                if (grid[wallLines[3][i]][i] == 5)
                {
                    possibleNodes.Add(new int[2] { wallLines[3][i], i });
                }
            }
        }
        return possibleNodes[Random.Range(1, possibleNodes.Count)];
    }
}

