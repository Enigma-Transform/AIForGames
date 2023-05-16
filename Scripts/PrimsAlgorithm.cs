using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Code is developed using the help of a tutorial Master Procedural Maze & Dungeon Generation by Penny De Byl, Michael Bridges. 
public class AreaLocationA
{
    public int x;
    public int z;

    public AreaLocationA(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}
public class PrimsAlgorithm : MonoBehaviour

{
    //Creating a list to keep track of the direction the crawler is traveling in
    public List<AreaLocationA> direction = new List<AreaLocationA>()
    {
        new AreaLocationA(1,0),
        new AreaLocationA(0,1),
        new AreaLocationA(-1,0),
        new AreaLocationA(0,-1)
    };
    public GameObject[] Cube;
    public int width, depth;
    public GameObject goalPrefab; // assign the goal prefab in the Inspector
    public Transform startPrefab;
    public float distanceFromGoal = 1000f;
    public int[,] area;
    public int scale = 6;
    public void buttonPress()
    {

        InitArea();
        Generate();
        DrawArea();
    }
    private void Start()
    {
        InitArea();
        Generate();
        DrawArea();
    }

  
    public  void Generate()
    {

        int x = 2;
        int z = 2;
        area[x, z] = 0;
        //Adding the area of the 4 given positions up down left and right.
        List<AreaLocationA> walls = new List<AreaLocationA>();
        walls.Add(new AreaLocationA(x + 1, z));
        walls.Add(new AreaLocationA(x - 1, z));
        walls.Add(new AreaLocationA(x, z + 1));
        walls.Add(new AreaLocationA(x, z - 1));

     

        //This condition will keep the while loop going as long as the count of the walls list is greater than 0

        while (walls.Count > 0)
        {
            //We are generation a random no to pick the position at whcih the wall will be palced.
            int randomWall = Random.Range(0, walls.Count);
            x = walls[randomWall].x;
            z = walls[randomWall].z;
            //We are then removing this wall from the list.
            walls.RemoveAt(randomWall);
            if (CountSquareNeighbours(x, z) == 1)
            {
                area[x, z] = 0;
                walls.Add(new AreaLocationA(x + 1, z));
                walls.Add(new AreaLocationA(x - 1, z));
                walls.Add(new AreaLocationA(x, z + 1));
                walls.Add(new AreaLocationA(x, z - 1));
            }

        }

    }


    public void InitArea()
    {
        area = new int[depth, width];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                area[x, z] = 1; //1wall 0= finish your code
              
            }
        }

       
        

       
    }
    

    //Responsible for drawin the maze of a given depth and width.
    public void DrawArea()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (area[z, x] == 1)
                {
                    Vector3 pos = new Vector3(x*scale, 0, z*scale);
                    GameObject walls = Instantiate(Cube[Random.Range(0, Cube.Length)], pos, Quaternion.identity);
                    walls.transform.localScale = new Vector3(scale, scale, scale);

                    walls.transform.position = pos;
                }


            }
        }
      
    }

    //Keeps a count and returns the neighbours of where the crawler is moving.Up,down,left,right. no diagonals
    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1)
        {
            return 5;
        }
        if (area[x - 1, z] == 0)
        {
            count++;
        }
        if (area[x + 1, z] == 0)
        {
            count++;
        }
        if (area[x, z + 1] == 0)
        {
            count++;
        }
        if (area[x, z - 1] == 0)
        {
            count++;
        }
        return count;
    }
   
}
