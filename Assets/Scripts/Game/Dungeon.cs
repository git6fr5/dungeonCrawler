using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Dungeon : MonoBehaviour
{
    /* --- Debug --- */
    [HideInInspector] public bool DEBUG_dungeon = true;


    /* --- Components --- */

    // Generators
    public Block block;
    public Path path;

    // Feedback
    public Text textBox;
    public Tilemap tilemap;
    public TileBase[] tileArray; // 0:Empty, 1:Center, 2:Left, 3:Right, 4:Top, 5:Bottom 6:TopRight 7:TopLeft 8:TopRoundy 9:BottomLeft 10:BottomRight 11:BottomRoundy 12:LeftRoundy 13:RightRoundy
    public GameObject[] terrain; // 0:Inner, 1:Wall, 2:Corner;
    public Transform terrainParent;

    /* --- Internal Variables --- */

    // Grid 
    public int sizeVertical = 50;
    public int sizeHorizontal = 50;
    [HideInInspector] public int[][] grid;
    [HideInInspector] public List<int[][]> nodes = new List<int[][]>();

    // Offset
    protected int horOffset = 0;
    protected int vertOffset = 0;
    public int numBlockRows = 3;
    public int numBlockColumns = 3;
    float minPathScale = 0.2f;

    /* --- Unity Methods --- */
    public virtual void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            nodes = new List<int[][]>();
            SetOffset((int)sizeHorizontal / 2, (int)sizeVertical / 2);
            SetGrid();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("1"))
        {
            FillBlocksOrderly();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("q"))
        {
            /*nodes = new List<int[][]>();
            SetOffset((int)sizeHorizontal / 2, (int)sizeVertical / 2);
            SetGrid();
            FillBlocksOrderly();*/
            SetTerrain();
        }
        if (Input.GetKeyDown("2"))
        {
            FillBlocksRandomly();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("3"))
        {
            AddBlock(0.2f, 0.2f, new float[] { 0.5f, 0f } );
            AddBlock(0.2f, 0.2f, new float[] { 0.5f, 1f });
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("4"))
        {
            ConnectAllOrderly();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("5"))
        {
            ConnectAllRandomly();
            PrintGrid();
            SetTilemap();
        }
    }

    /* --- Methods --- */
    public void SetOffset(int x, int y)
    {
        horOffset = x;
        vertOffset = y;
    }

    public virtual void SetGrid()
    {
        grid = new int[sizeVertical][];
        for (int i = 0; i < sizeVertical; i++)
        {
            grid[i] = new int[sizeHorizontal];
        }
    }

    void FillBlocksOrderly()
    { 
        int numPathHor = numBlockColumns - 1;
        float totalPathLengthHor = numPathHor * minPathScale;
        if (totalPathLengthHor > 0.5f) { return; }

        int numPathVert = numBlockRows - 1;
        float totalPathLengthVert = numPathVert * minPathScale;
        if (totalPathLengthVert > 0.5f) { return; }

        float blockScaleHor = (1 - totalPathLengthHor) / numBlockColumns;
        float blockScaleVert = (1 - totalPathLengthVert) / numBlockRows;

        for (int i = 0; i < numBlockRows; i++)
        {
            for (int j = 0; j < numBlockRows; j++)
            {
                float anchorOffsetHor = blockScaleHor * (j + Random.Range(0.5f, 0.8f)) + minPathScale * j / Random.Range(1f, 2f);
                float anchorOffsetVert = blockScaleVert * (i + Random.Range(0.35f, 0.65f)) + minPathScale * i / Random.Range(1f, 2f);
                float[] anchor = new float[] { anchorOffsetVert, anchorOffsetHor };
                AddBlock(blockScaleVert, blockScaleHor, anchor);
            }
        }
    }

    void FillBlocksRandomly()
    {
        float anchorOffsetHor = 0f;
        float anchorOffsetVert = 0f;
        float blockScale = 0.2f;

        for (int i = 0; i < numBlockRows; i++)
        {
            for (int j = 0; j < numBlockRows; j++)
            {
                float[] anchor = new float[] { anchorOffsetVert + Random.Range(0f, blockScale), anchorOffsetHor };
                AddBlock(blockScale, blockScale, anchor);
                anchorOffsetHor = anchorOffsetHor + Random.Range(1.2f * blockScale, 2 * blockScale);
                if (anchorOffsetHor > 1f)
                {
                    anchorOffsetHor = 0f + Random.Range(0f, blockScale);
                    anchorOffsetVert = anchorOffsetVert + blockScale * 2;
                }
            }
        }
    }

    void AddBlock(float scaleVert, float scaleHor, float[] anchor)
    {
        block.Construct((int)(sizeVertical * scaleVert), (int)(sizeHorizontal * scaleHor));
        int[][] subGrid = block.grid;

        AttachToGrid(subGrid, anchor);
    }

    void AttachToGrid(int[][] subGrid, float[] _anchor)
    {
        int[] anchor = new int[2];
        anchor[0] = (int)(_anchor[0] * (grid.Length - subGrid.Length));
        anchor[1] = (int)(_anchor[1] * (grid[0].Length - subGrid[0].Length));

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

        int[] leftNode = new int[2] { block.leftNode[0] + anchor[0], block.leftNode[1] + anchor[1] };
        int[] rightNode = new int[2] { block.rightNode[0] + anchor[0], block.rightNode[1] + anchor[1] };
        int[] topNode = new int[2] { block.topNode[0] + anchor[0], block.topNode[1] + anchor[1] };
        int[] bottomNode = new int[2] { block.bottomNode[0] + anchor[0], block.bottomNode[1] + anchor[1] };

        nodes.Add(new int[4][] { leftNode, topNode, rightNode, bottomNode });
    }

    void ConnectAllOrderly()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            int rightIndex = i + 1;
            if (rightIndex % numBlockColumns < i % numBlockColumns) { }
            else
            {
                path.ConnectBlocks(grid, nodes[i], nodes[rightIndex]);
            }

            int bottomIndex = i + numBlockColumns;
            if (bottomIndex >= nodes.Count) { }
            else
            {
                path.ConnectBlocks(grid, nodes[i], nodes[bottomIndex]);
            }
        }
    }

    void ConnectAllRandomly()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            int[][] randomNode = nodes[Random.Range(0, i)];
            if (nodes[i] != randomNode)
            {
                path.ConnectBlocks(grid, nodes[i], randomNode);
            }
        }
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

    protected void SetTilemap()
    {
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < sizeHorizontal; j++)
            {
                if (tileArray[grid[i][j]]) { tilemap.SetTile(new Vector3Int(j - horOffset, -(i - vertOffset), 0), tileArray[grid[i][j]]); }
            }
        }
    }

    protected void SetTerrain()
    {
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < sizeHorizontal; j++)
            {
                if (grid[i][j] == 1) { Instantiate(terrain[0], new Vector3(j - horOffset, -(i - vertOffset), 0), Quaternion.identity, null); }

                Quaternion rotationLeft = Quaternion.identity;
                Quaternion rotationRight = Quaternion.identity;
                Quaternion rotationTop = Quaternion.identity;
                Quaternion rotationBottom = Quaternion.identity;

                rotationLeft.eulerAngles = new Vector3(-90, 0, 0);
                rotationRight.eulerAngles = new Vector3(90, 90, -90);
                rotationTop.eulerAngles = new Vector3(0, 90, -90);
                rotationBottom.eulerAngles = new Vector3(180, 90, -90);

                GameObject _object = null;
                if (grid[i][j] == 2) { _object = Instantiate(terrain[1], new Vector3(j - horOffset, -(i - vertOffset), 0), rotationLeft, null); }
                if (grid[i][j] == 3) { _object = Instantiate(terrain[1], new Vector3(j - horOffset, -(i - vertOffset), 0), rotationRight, null); }
                if (grid[i][j] == 4) { _object = Instantiate(terrain[1], new Vector3(j - horOffset, -(i - vertOffset), 0), rotationTop, null); }
                if (grid[i][j] == 5) { _object = Instantiate(terrain[1], new Vector3(j - horOffset, -(i - vertOffset), 0), rotationBottom, null); }

                if (_object) { _object.AddComponent<BoxCollider>(); }
            }
        }
    }

}
