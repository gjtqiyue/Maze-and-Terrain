using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour {

    public static readonly int ROW_LENGTH = 8;
    public static readonly float GRID_LENGTH = 5f;

    public static MazeGenerator instance = null;

    public Vector3 origin;
    public WallScript wall;
    public GameObject floor;
    public Collectable collectable;

    private ArrayList maze;
    private Dictionary<int, List<Cell>> mazeCell;
    private int rowIndex = 0;
    private int uniqueId = 0;
    private int[] wallInfo;

    protected class Cell
    {
        public int m_id;
        public bool m_frontWall;
        public bool m_leftWall;

        public Cell(int id, bool hasFront, bool hasLeft)
        {
            m_id = id;
            m_frontWall = hasFront;
            m_leftWall = hasLeft;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

   

    private void Start()
    {
        wallInfo = new int[ROW_LENGTH];
        mazeCell = new Dictionary<int, List<Cell>>();
        maze = new ArrayList();

        //for (int i=0; i < 5; i++)
        //{
        //    Debug.Log(getUniqueId());
        //}
        
    }

    public void RandomGenerate()
    {
        Cell[] row;

        // create a row based on the rowindex
        if (rowIndex == 0)
        {
            row = CreateRow(0);

            row = Merge(row);

            // add the complete row to the map
            maze.Add(row);
            rowIndex++;

            row = ExtendWall(row);

            string rowString = "";
            
            
            for (int i = 0; i < row.Length - 1; i++)
            {
                if (row[i].m_id != row[i + 1].m_id)
                {
                    rowString = string.Concat(rowString, row[i].m_id);
                    rowString = string.Concat(rowString, "|");
                }
                else
                {
                    rowString = string.Concat(rowString, row[i].m_id);
                }
            }
            rowString = string.Concat(rowString, row[7].m_id);
            print(rowString);
            PrintWallInfo();
        }
        else
        {
            row = CreateRow(3);

            string wallString = "";
            for (int i = 0; i < row.Length - 1; i++)
            {
                if (row[i].m_id != row[i + 1].m_id)
                {
                    wallString = string.Concat(wallString, row[i].m_id);
                    wallString = string.Concat(wallString, "|");
                }
                else
                {
                    wallString = string.Concat(wallString, row[i].m_id);
                }
            }
            wallString = string.Concat(wallString, row[7].m_id);
            print(wallString);

            Cell[] mergeCell = Merge(row);

            // add the complete row to the map
            maze.Add(row);
            rowIndex++;

            row = ExtendWall(row);

            string mergeString = "";
            for (int i = 0; i < row.Length - 1; i++)
            {
                if (mergeCell[i].m_id != mergeCell[i + 1].m_id)
                {
                    mergeString = string.Concat(mergeString, mergeCell[i].m_id);
                    mergeString = string.Concat(mergeString, "|");
                }
                else
                {
                    mergeString = string.Concat(mergeString, mergeCell[i].m_id);
                }
            }
            mergeString = string.Concat(mergeString, mergeCell[7].m_id);
            print(mergeString);

            PrintWallInfo();
        }


        // construct the maze based on the row
        GenerateRow(rowIndex);
    }

    private void PrintWallInfo()
    {
        string wall = "";
        for (int i = 0; i < 8; i++)
        {
            if (wallInfo[i] == 1)
            {
                wall = string.Concat(wall, "1|");
            }
            else
            {
                wall = string.Concat(wall, "0|");
            }
        }
        print(wall);
    }

    private Cell[] CreateRow(int rowType)
    {
        Cell[] newRow = new Cell[ROW_LENGTH];
        switch (rowType)
        {
            // create the first row
            case 0:
                for (int i = 0; i < newRow.Length; i++)
                {
                    int id = getUniqueId();
                    newRow[i] = new Cell(id, true, true);


                    // create new list
                    mazeCell.Add(newRow[i].m_id, new List<Cell>());
                    mazeCell[id].Add(newRow[i]);
                }

                return newRow;

   
            // create inner row
            default:
                    // create the second row based on the testRow
                    for (int i = 0; i < wallInfo.Length; i++)
                    {

                        switch (wallInfo[i])
                        {
                            // if it's a opening from previous row, then we create a cell that has the same set number as the previous one
                            case 0:
                                Cell[] previousRow = (Cell[])maze[maze.Count - 1];
                                int oldId = previousRow[i].m_id;
                                newRow[i] = new Cell(oldId, true, true);
                                mazeCell[oldId].Add(newRow[i]);
                                break;
                            // else if there is a wall blocked the way, we set this cell to a new id
                            case 1:
                                int newId = getUniqueId();
                                newRow[i] = new Cell(newId, true, true);
                                mazeCell[newId].Add(newRow[i]);
                                break;
                            default:
                                break;
                        }
                    }

                    return newRow;
            }
    }

    private Cell[] Merge(Cell[] row)
    {
        // check the row
        // if they are not in the same set then randomly decide if merge or not
        for (int i = 0; i < row.Length - 1; i++)
        {
            // if they are not same
            if (row[i].m_id != row[i + 1].m_id)
            {
                int choice = Random.Range(0, 3);
                
                // merge
                if (choice == 1)
                {
                    // move everything in that set to the other set
                    int newId = row[i].m_id;
                    int oldId = row[i + 1].m_id;
                    row[i].m_leftWall = false;
                    foreach (Cell cell in mazeCell[oldId])
                    {
                        Cell temp = cell;
                        temp.m_id = newId;

                        mazeCell[newId].Add(temp);
                    }

                    mazeCell[oldId].Clear();
                }
                               
            }
            else
            {
                row[i].m_leftWall = false;
            }
        }
        return row;   
    }

    private Cell[] ExtendWall(Cell[] row)
    {
        // determine vertical connection randomly
        // record all the set number in the row
        Dictionary<int, List<Cell>> map = new Dictionary<int, List<Cell>>();

        for (int i = 0; i < row.Length; i++)
        {
            int setId = row[i].m_id;

            // create a new list if it's not exist
            if (!map.ContainsKey(setId))
            {
                map.Add(setId, new List<Cell>());
            }
            // add the element in the list

            map[setId].Add(row[i]);
        }

        // determine a opening randomly for each set
        foreach (int key in map.Keys)
        {
            int randomOpening = Random.Range(0, map[key].Count);

            map[key][randomOpening].m_frontWall = false;

        }

        // do the rest
        // get a array of the info of walls of next row
        
        for (int i = 0; i < row.Length; i++)
        {
            if (row[i].m_frontWall)
            {
                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    row[i].m_frontWall = false;
                        wallInfo[i] = 0;
                }
                else
                        wallInfo[i] = 1;
            }
            else
                    wallInfo[i] = 0;
        }

        return row;
    }

    private int getUniqueId()
    {
        int validId = 0;
        // check the maze Cell set to find any unused set number
        if (mazeCell.Keys.Count > 0)
        {
            foreach (int key in mazeCell.Keys)
            {
                if (mazeCell[key].Count == 0)
                    validId = key;
            }
        }

        if (validId != 0)
        {
            return validId;
        }
        else
        {
            uniqueId++;
            return uniqueId;
        }
    }

    // generate the row in the world based on the row created
    private void GenerateRow(int rowIndex)
    {
        Vector3 startPoint = origin + Vector3.forward * GRID_LENGTH * rowIndex;

        // create floor
        for (int i = 0; i < ROW_LENGTH; i++)
        {
            Instantiate(floor, startPoint + Vector3.left * GRID_LENGTH * i, Quaternion.identity);

            // generate collectable randomly on the floor
            int result = Random.Range(0, 3);
            if (result == 1)
            {
                Instantiate(collectable, startPoint + Vector3.left * GRID_LENGTH * i + new Vector3(0f, 1f, 0f), Quaternion.identity);
            }
        }

        //create right side wall
        Instantiate<WallScript>(wall, startPoint + new Vector3((GRID_LENGTH / 2), (GRID_LENGTH / 2), 0f), Quaternion.Euler(0, 90, 0));

        //create walls in the row
        Cell[] row = (Cell[])maze[rowIndex - 1];
        for (int i = 0; i < row.Length; i++)
        {
            // if two cells are not in the same set
            // generate the left wall
            if (row[i].m_leftWall)
            {
                Vector3 spawnPoint = startPoint + Vector3.left * GRID_LENGTH * i + new Vector3(-(GRID_LENGTH / 2), (GRID_LENGTH / 2), 0f);
                Instantiate<WallScript>(wall, spawnPoint, Quaternion.Euler(0, 90, 0));
            }

            // generate the front wall
            if (row[i].m_frontWall)
            {
                Vector3 spawnPoint = startPoint + Vector3.left * GRID_LENGTH * i + new Vector3(0, (GRID_LENGTH / 2), (GRID_LENGTH / 2));
                Instantiate<WallScript>(wall, spawnPoint, Quaternion.identity);
            }
        }

        //create left side wall
        //Vector3 endPoint = startPoint + Vector3.left * GRID_LENGTH * (ROW_LENGTH - 1) + new Vector3(-(GRID_LENGTH / 2), (GRID_LENGTH / 2), 0f);
        //Instantiate<WallScript>(wall, endPoint, Quaternion.Euler(0, 90, 0));
    }

    internal bool EndMaze()
    {
        if (rowIndex > 0)
        {
            Cell[] lastRow = new Cell[ROW_LENGTH];

            lastRow = CreateRow(3);

            EndMerge(lastRow);

            maze.Add(lastRow);
            rowIndex++;

            GenerateRow(rowIndex);

            return true;
        }

        return false;
    }

    private void EndMerge(Cell[] lastRow)
    {
        // merge everything together, no wall in between
        for (int i = 0; i < lastRow.Length - 1; i++)
        {
            // if they are not same
            if (lastRow[i].m_id != lastRow[i + 1].m_id)
            {

                // move everything in that set to the other set
                int newId = lastRow[i].m_id;
                int oldId = lastRow[i + 1].m_id;
                foreach (Cell cell in mazeCell[oldId])
                {
                    Cell temp = cell;
                    temp.m_id = newId;

                    mazeCell[newId].Add(temp);
                }

                mazeCell[oldId].Clear();
            }
            lastRow[i].m_leftWall = false;
        }
        lastRow[7].m_leftWall = true;
    }
}
