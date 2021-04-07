using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class Dungeon2D : MonoBehaviour
{
    /* --- Debug --- */

    /* --- Events --- */

    /* --- Components --- */

    // Pathfinder
    public Pathfind2D pathfind2D;

    // Feedback
    public Text textBox;
    public Tilemap tilemap;
    public TileBase[] tileArray; // 0:Empty, 1:Center, 2:Left, 3:Right, 4:Top, 5:Bottom 6:TopRight 7:TopLeft 8:TopRoundy 9:BottomLeft 10:BottomRight 11:BottomRoundy 12:LeftRoundy 13:RightRoundy

    /* --- Internal Variables --- */

    // Grid 
    int sizeVertical = 50;
    int sizeHorizontal = 50;
    [HideInInspector] public int[][] grid;
    List<Coordinate2D> nodes = new List<Coordinate2D>();

    // Offset
    int horOffset = 0;
    int vertOffset = 0;

    /* --- Unity Methods --- */
    void Start()
    {
        InitializeGrid();
        InitializeTileMap();
    }

    void Update()
    {
        SetTilemap();
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int gridY = -(int)mousePos.y + vertOffset;
            int gridX = (int)mousePos.x + horOffset;
            print(gridY.ToString() + ", " + gridX.ToString());
            if (gridX < sizeHorizontal && gridX > 0 && gridY < sizeVertical && gridY > 0)
            {
                grid[gridY][gridX] = 1;
            }
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int gridY = -(int)mousePos.y + vertOffset;
            int gridX = (int)mousePos.x + horOffset;
            print(gridY.ToString() + ", " + gridX.ToString());
            if (gridX < sizeHorizontal && gridX > 0 && gridY < sizeVertical && gridY > 0)
            {
                if (grid[gridY][gridX] != 14) 
                {
                    grid[gridY][gridX] = 14;
                    nodes.Add(new Coordinate2D(gridY, gridX));
                }
            }
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("s"))
        {
            AddShape(Geometry2D.Shape.ellipse, Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), RandomAnchor());
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("c"))
        {
            AddShape(Geometry2D.Shape.triangle, Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), RandomAnchor());
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("p"))
        {
            pathfind2D.AStar(grid, nodes);
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("0"))
        {
            InitializeGrid();
            InitializeTileMap();
        }
    }

    /* --- Methods --- */
    void InitializeGrid()
    {
        grid = new int[sizeVertical][];
        for (int i = 0; i < sizeVertical; i++)
        {
            grid[i] = new int[sizeHorizontal];
        }
        PrintGrid();
    }

    void InitializeTileMap()
    {
        horOffset = (int)sizeHorizontal / 2;
        vertOffset = (int)sizeVertical / 2;
        SetTilemap();
    }

    void AddShape(Geometry2D.Shape shape, float scaleVert, float scaleHor, Anchor2D anchor)
    {
        int[][] subGrid = Geometry2D.ConstructShape(shape, (int)(sizeVertical * scaleVert), (int)(sizeHorizontal * scaleHor));
        AttachToGrid(subGrid, anchor);
    }

    void AttachToGrid(int[][] subGrid, Anchor2D _anchor)
    {
        int[] anchor = _anchor.AnchorToGrid(grid, subGrid);

        for (int i = 0; i < subGrid.Length; i++)
        {
            for (int j = 0; j < subGrid[0].Length; j++)
            {
                if (subGrid[i][j] != 0)
                {
                    grid[i + anchor[0]][j + anchor[1]] = subGrid[i][j];
                }
            }
        }
    }

    Anchor2D RandomAnchor()
    {
        return Anchor2D.anchors[Random.Range(0, Anchor2D.anchors.Length)];
    }

    public void PrintGrid()
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

    public void SetTilemap()
    {
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < sizeHorizontal; j++)
            {
                Vector3Int pos = new Vector3Int(j - horOffset, -(i - vertOffset), 0);
                TileBase tileBase = tileArray[grid[i][j]] ?? tileArray[0]; ;
                tilemap.SetTile(pos, tileBase);
                /*if (grid[i][j] == 15)
                {
                    Color color = tilemap.GetColor(pos);
                    float alpha = 0.25f;
                    color = new Color(1f, 1f, 1f, (color.a + alpha) % 1f);
                    tilemap.SetTileFlags(pos, TileFlags.None);
                    tilemap.SetColor(pos, color);
                }*/
            }
        }
    }
}
