using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry2D
{
    public enum Shape
    {
        ellipse, triangle
    }

    public static int[][] ConstructShape(Shape shape, int vertical, int horizontal)
    {
        switch (shape)
        {
            case Shape.ellipse:
                Debug.Log("Constructing Ellipse");
                return Ellipse(vertical, horizontal);
            case Shape.triangle:
                Debug.Log("Constructing Triangle");
                return Triangle(vertical, horizontal);
            default:
                Debug.Log("Unknown Shape");
                return new int[0][];
        }
    }

    public static int[][] Ellipse(int vertical, int horizontal)
    {
        // Initialize the grid
        int[][] circle = new int[vertical][];
        for (int i = 0; i < circle.Length; i++)
        {
            circle[i] = new int[horizontal];
        }

        float a = (float)horizontal / 2;
        float b = (float)vertical / 2;

        // Draw the circle
        for (int i = 0; i < vertical; i++)
        {
            for (int j = 0; j < horizontal; j++)
            {
                float x = (float)j - a;
                float y = (float)i - b;
                float ellipticalBoundary = (x * x / (a * a)) + (y * y / (b * b));
                if (Mathf.Abs(ellipticalBoundary) < 1) { circle[i][j] = 1; }
            }
        }

        return circle;
    }

    public static int[][] Triangle(int vertical, int horizontal)
    {
        // Initialize the grid
        int[][] triangle = new int[vertical][];
        for (int i = 0; i < triangle.Length; i++)
        {
            triangle[i] = new int[horizontal];
        }

        float center = (int)(horizontal / 3);
        float ratioA = (float)vertical / center;
        float ratioB = (float)vertical / (horizontal - center);

        // Draw the triangle
        for (int i = 0; i < vertical; i++)
        {
            for (int j = 0; j < horizontal; j++)
            {
                if (j < center && (vertical - i) < ratioA * j) { triangle[i][j] = 1; }
                if (j >= center && i > ratioB * j + center) { triangle[i][j] = 1; }
            }
        }
        
        return triangle;
    }
}
