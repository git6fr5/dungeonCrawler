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

    /* --- Internal Variables --- */

    // Grid 
    public int sizeVertical = 40;
    public int sizeHorizontal = 40;
    [HideInInspector] public int[][] grid;

    // Offset
    protected int horOffset = 20;
    protected int vertOffset = 20;
    float anchorOffsetHor = 0f;
    float anchorOffsetVert = 0f;
    float blockScale = 0.2f;

    /* --- Unity Methods --- */
    public virtual void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            block.SetOffset(-20, 20);
            block.Construct(20, 20, true, true);
        }
        if (Input.GetKeyDown("2"))
        {
            path.SetOffset(10, 20);
            path.Construct(5, 20, 1, true, true);
        }
        if (Input.GetKeyDown("3"))
        {
            SetGrid();
            PrintGrid();
            SetTilemap();
        }
        if (Input.GetKeyDown("4"))
        {
            float[] anchor = new float[] { anchorOffsetVert + Random.Range(0f, blockScale), anchorOffsetHor };
            AddBlock(blockScale, anchor);
            PrintGrid();
            SetTilemap();
            anchorOffsetHor = anchorOffsetHor + Random.Range(1.2f*blockScale, 2*blockScale);
            if (anchorOffsetHor > 1f)
            {
                anchorOffsetHor = 0f + Random.Range(0f, blockScale);
                anchorOffsetVert = anchorOffsetVert + blockScale*2;
            }
            /*if (anchorOffsetVert + blockScale * 0.8f > 1f)
            {
                anchorOffsetHor = 0f;
                anchorOffsetHor = 0f;
                SetGrid();
            }*/
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

    void AddBlock(float scale, float[] anchor)
    {
        block.Construct((int) (sizeVertical * scale), (int) (sizeHorizontal * scale));
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
    }

    protected void PrintGrid()
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

}
