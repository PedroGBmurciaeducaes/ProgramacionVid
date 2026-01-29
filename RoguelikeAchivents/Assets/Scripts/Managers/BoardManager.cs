using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }


   
    public FoodObject[] FoodPrefab;

    public WallObject[] WallPrefab;

    public Enemy[] EnemyPrefab;

    private CellData[,] m_BoardData;
    private Tilemap m_Tilemap;
    private Grid m_Grid;

    public ExitCellObject ExitCellPrefab;

    public ChestObject[] ChestPrefab;
    
    public ExpObject[] EXPprefab;



    public List<Vector2Int> m_EmptyCellsList;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    float WallDensity = 0.25f;      // Se establece el tanto por ciento de densidad de objetos en el nivel (ya dividido entre 100)
    float FoodDensity = 0.1f;
    float EnemyDensity = 0.025f;
    float EXPDensity = 0.05f;
    float ChestDensity = 0.02f;



    public void Init()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        //Initialize the list
        m_EmptyCellsList = new List<Vector2Int>();

        m_BoardData = new CellData[Width, Height];


        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();

                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    tile = WallTiles[UnityEngine.Random.Range(0, WallTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[UnityEngine.Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;

                    //this is a passable empty cell, add it to the list!
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        //remove the starting point of the player! It's not empty, the player is there
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));

        Vector2Int endCoord = new Vector2Int(Width - 2, Height - 2);
        AddObject(Instantiate(ExitCellPrefab), endCoord);
        m_EmptyCellsList.Remove(endCoord);

        GenerateWall(); // new line
        GenerateFood();
        GenerateEnemy();
        GenerateChest();
        GenerateEXP();
    }




    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width
            || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }

        return m_BoardData[cellIndex.x, cellIndex.y];
    }

 

    void GenerateFood()
    {
        int foodCount = Mathf.RoundToInt(FoodDensity * m_EmptyCellsList.Count);

        for (int i = 0; i < foodCount; ++i)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_EmptyCellsList.Count);
            int comidasRand = UnityEngine.Random.Range(0, FoodPrefab.Length);

            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            FoodObject newFood = Instantiate(FoodPrefab[comidasRand]);
            AddObject(newFood, coord);
        }
    }

    void GenerateWall()
    {
        int wallCount = Mathf.RoundToInt(WallDensity * m_EmptyCellsList.Count);
        int pesoCount = 0;

        foreach (WallObject e in WallPrefab)
        {
            pesoCount += e.peso;
        }

        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_EmptyCellsList.Count);

            int wallRand = UnityEngine.Random.Range(0, pesoCount);

            int pesoAcumulado = 0;

            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);                                     //No utilizamos el peso de los objetos como tal, estamos utilizando directamente valores.

            WallObject newWall;


            foreach (WallObject e in WallPrefab)
            {
                if (wallRand < pesoAcumulado + e.peso)
                {
                    newWall = Instantiate(e);
                    AddObject(newWall, coord);
                    break;
                }
              pesoAcumulado += e.peso;  
            }

        }
    }


    void GenerateEnemy()
    {

        int EnemyCount = Mathf.RoundToInt(EnemyDensity * m_EmptyCellsList.Count);
        int pesoCount = 0;

        foreach (Enemy e in EnemyPrefab)
        {
            pesoCount += e.peso;
        }


        for (int i = 0; i < EnemyCount; ++i)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            int enemyRand = UnityEngine.Random.Range(0, pesoCount);


            Enemy newEnemy;

            int pesoAcumulado = 0; 

            foreach (Enemy e in EnemyPrefab)
            { 
                if (enemyRand < pesoAcumulado + e.peso)
                {
                    newEnemy = Instantiate(e);
                    AddObject(newEnemy, coord);
                    break;
                }
                pesoAcumulado += e.peso;       
            }

        }
    }


    void GenerateChest()
    {

        int ChestCount = Mathf.RoundToInt(ChestDensity * m_EmptyCellsList.Count);
        int pesoCount = 0;

        foreach (ChestObject e in ChestPrefab)
        {
            pesoCount += e.peso;
        }


        for (int i = 0; i < ChestCount; ++i)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            int chestRand = UnityEngine.Random.Range(0, pesoCount);


            ChestObject newChest;

            int pesoAcumulado = 0;

            foreach (ChestObject e in ChestPrefab)
            {
                if (chestRand < pesoAcumulado + e.peso)
                {
                    newChest = Instantiate(e);
                    AddObject(newChest, coord);
                    break;
                }
                pesoAcumulado += e.peso;
            }

        }
    }

    void GenerateEXP()
    {
        int EXPCount = Mathf.RoundToInt(EXPDensity * m_EmptyCellsList.Count);
        int pesoCount = 0;

        foreach (WallObject e in WallPrefab)
        {
            pesoCount += e.peso;
        }

        for (int i = 0; i < EXPCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_EmptyCellsList.Count);

            int expRand = UnityEngine.Random.Range(0, pesoCount);

            int pesoAcumulado = 0;

            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);                                     //No utiliz objetos como tal, estamos utilizando directamente valores.

            ExpObject NewObjetoEXP;


            foreach (ExpObject e in EXPprefab)
            {
                if (expRand < pesoAcumulado + e.peso)
                {
                    NewObjetoEXP = Instantiate(e);
                    AddObject(NewObjetoEXP, coord);
                    break;
                }
                pesoAcumulado += e.peso;
            }

        }
    }




    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }


    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }



    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }



    public void Clean()
    {
        //no board data, so exit early, nothing to clean
        if (m_BoardData == null)
            return;


        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                var cellData = m_BoardData[x, y];

                if (cellData.ContainedObject != null)
                {
                    //CAREFUL! Destroy the GameObject NOT just cellData.ContainedObject
                    //Otherwise what you are destroying is the JUST CellObject COMPONENT
                    //and not the whole gameobject with sprite
                    Destroy(cellData.ContainedObject.gameObject);
                }

                SetCellTile(new Vector2Int(x, y), null);
            }
        }
    }


}