using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public int mazeWidth;
    public int mazeHeight;

    private bool[,] visited;
    private GameObject[,] mazeCells;

    void Start()
    {
        visited = new bool[mazeWidth, mazeHeight];
        mazeCells = new GameObject[mazeWidth, mazeHeight];

        // Generate the maze
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // Initialize the maze with walls
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                visited[i, j] = false;

                // Create a wall
                GameObject wall = Instantiate(wallPrefab, new Vector3(i, 0.5f, j), Quaternion.identity);
                mazeCells[i, j] = wall;
            }
        }

        // Start the maze generation from a random cell
        int startX = Random.Range(0, mazeWidth);
        int startY = Random.Range(0, mazeHeight);
        VisitCell(startX, startY);
    }

    void VisitCell(int x, int y)
    {
        visited[x, y] = true;

        // Shuffle the list of neighboring cells
        List<Vector2Int> neighbors = new List<Vector2Int>();
        neighbors.Add(new Vector2Int(x, y + 1));
        neighbors.Add(new Vector2Int(x, y - 1));
        neighbors.Add(new Vector2Int(x + 1, y));
        neighbors.Add(new Vector2Int(x - 1, y));
        neighbors.Sort((a, b) => Random.Range(-1, 2));

        // Visit each unvisited neighboring cell recursively
        foreach (Vector2Int neighbor in neighbors)
        {
            if (neighbor.x >= 0 && neighbor.x < mazeWidth && neighbor.y >= 0 && neighbor.y < mazeHeight && !visited[neighbor.x, neighbor.y])
            {
                // Remove the wall between the current cell and the neighboring cell
                if (neighbor.x == x)
                {
                    if (neighbor.y > y)
                    {
                        Destroy(mazeCells[x, y].transform.Find("North Wall").gameObject);
                        Destroy(mazeCells[neighbor.x, neighbor.y].transform.Find("South Wall").gameObject);
                    }
                    else
                    {
                        Destroy(mazeCells[x, y].transform.Find("South Wall").gameObject);
                        Destroy(mazeCells[neighbor.x, neighbor.y].transform.Find("North Wall").gameObject);
                    }
                }
                else
                {
                    if (neighbor.x > x)
                    {
                        Destroy(mazeCells[x, y].transform.Find("East Wall").gameObject);
                        Destroy(mazeCells[neighbor.x, neighbor.y].transform.Find("West Wall").gameObject);
                    }
                    else
                    {
                        Destroy(mazeCells[x, y].transform.Find("West Wall").gameObject);
                        Destroy(mazeCells[neighbor.x, neighbor.y].transform.Find("East Wall").gameObject);
                    }
                }

                // Visit the neighboring cell
                VisitCell(neighbor.x, neighbor.y);
            }
        }
    }
}
