using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : Dungeon
{
    /* --- Overridden Unity Methods --- */
    public override void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            grid = Circle(Random.Range(20, 30));
            PrintGrid();
            SetTilemap();
        }
    }

    /* --- Additional Unity Methods --- */

    /* --- Overridden Methods --- */

    /* --- Additional Methods --- */

    int[][] Circle(int radius)
    {
        int[][] circle = new int[radius][];
        for (int i = 0; i < circle.Length; i++)
        {
            circle[i] = new int[radius];
        }
        return circle;
    }
}
