using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Board_Manager : MonoBehaviour
{
public class CellData
    {
        public bool passable;
        public Cell_Object ContainedObject;
    }

private CellData[,] m_BoardData;

private Tilemap m_Tilemap;


public int Height;
public int Width;
public Tile[] GroundTiles;
public Tile[] WallTiles;

private Grid m_Grid;

public Food_Object FoodPrefab;
public Food_Object_Gr FoodGrPrefab;

public Wall_Object WallPrefab;

public Exit_Object ExitCellPrefab;

private List<Vector2Int> m_emptyCells;

public Player_Controller PController;
public int SpawnY;
public int SpawnX;

    public void MapGeneration()
    {
        m_Grid = GetComponentInChildren<Grid>();

        m_Tilemap = GetComponentInChildren<Tilemap>();//busca tilemap //
                                                      //en el hijo de BoardManager, <<Tilemap>>//

        m_BoardData = new CellData[Height, Width];  //Declaro cuales van a ser la longitud de los arrays, el tamaño del escenario

        m_emptyCells = new List<Vector2Int>();

        for (int y = 0; y < Height; ++y) //recorre las columnas del tablero
        {
            for (int x = 0; x < Width; ++x) //recorre las filas del tablero
            {
                Tile tile; //declaro un tile genérico
                m_BoardData[x, y] = new CellData(); //creo nuevos datos para cada celda del tablero para poder decidir si son pasables

                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1) //si está en un borde
                {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)]; //el tile será un muro
                    m_BoardData[x, y].passable = false; //el valor de celda es no pasable
                }
                else //si no está en los bordes
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];  //el tile será uno de los del suelo
                    m_BoardData[x, y].passable = true; //El valor de celda es pasable
                    m_emptyCells.Add(new Vector2Int(x, y));
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile); //dibuja ese tile en la casilla actual
            }
        }
        m_emptyCells.Remove(new Vector2Int(1, 1));
        Vector2Int endCoord = new Vector2Int(Height - 2, Width - 2);
        AddObject(Instantiate(ExitCellPrefab), endCoord);
        m_emptyCells.Remove(endCoord);
        GenerateWall();
        SpawnComida();
    }


    public Vector3 CellToWorld ( Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }
    public CellData GetCelldata (Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height) 
        { return null; }

         return m_BoardData[cellIndex.x, cellIndex.y];
    }

    public void SpawnComida()
    {
        int foodCount = Random.Range(3,5);

        for (int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range (0, m_emptyCells.Count);
            Vector2Int coord = m_emptyCells[randomIndex];

            m_emptyCells.RemoveAt(randomIndex);
            Food_Object newFood = Instantiate(FoodPrefab);
            AddObject(newFood,coord);
        }     
    }
    void GenerateWall()
    {
        int wallCount = Random.Range(4, 8);

        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = Random.Range(0, m_emptyCells.Count);
            Vector2Int coord = m_emptyCells[randomIndex];

            m_emptyCells.RemoveAt(randomIndex);
            Wall_Object newWall = Instantiate(WallPrefab);
            AddObject(newWall, coord);
        }
    } 

    void AddObject (Cell_Object obj,Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex) 
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x,cellIndex.y, 0));    
    }
    public void Clean()
    {
        if(m_BoardData == null) 
        
            return;

        for(int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var cellData = m_BoardData[x, y];
                if (cellData.ContainedObject != null)
                {
                    Destroy(cellData.ContainedObject.gameObject);
                }

                SetCellTile(new Vector2Int (x, y), null);
            }
        }

    }
}
