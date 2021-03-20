using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : Collectible
{
    /* --- Overridden Variables --- */

    /* --- Additional Variables --- */
    public enum Material
    {
        Iron,
        Gold,
        Obsidian
    }

    public Material material;

    /* --- Overridden Methods --- */
}
