using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

public class Board : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField, Range(0.0f, 2.0f)] private float spacing;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float creationTime;

    [SerializeField, Range(0.01f, 0.99f)] private float mineDensity;
    
    private Cell[,] cells;

    private void Awake()
    {
        cells = new Cell[gridSize.x, gridSize.y];
    }

    private void Start()
    {
        StartCoroutine(CreateBoard());
    }

    public IEnumerator CreateBoard()
    {
        yield return CreateCells();

        PlaceMines();
        AssignMineCount();
    }

    public void ResetBoard()
    {
        StopAllCoroutines();
        
        foreach (Cell cell in cells)
        {
            Destroy(cell.gameObject);
        }
        
        StartCoroutine(CreateBoard());
    }
    
    private IEnumerator CreateCells()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Cell cell = Instantiate(cellPrefab, GetCellPosition(x, y), Quaternion.identity, transform).GetComponent<Cell>();
                cell.CellPosition = new Vector2Int(x, y);
                cell.OnClicked += OpenCell;
                cells[x, y] = cell;
                
                yield return new WaitForSeconds(creationTime / (gridSize.x * gridSize.y));
            }
        }
    }

    private void PlaceMines()
    {
        int minesCount = Mathf.RoundToInt(gridSize.x * gridSize.y * mineDensity);

        for (int i = 0; i < minesCount;)
        {
            Cell cell = cells[Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)];
            if (!cell.IsMine)
            {
                cell.IsMine = true;
                i++;
            }
        }
    }

    private void AssignMineCount()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (cells[x, y].IsMine)
                    continue;
                
                List<Vector2Int> neighbours = GetNeighbours(x, y);
                for (int i = 0; i < neighbours.Count; i++)
                {
                    if (cells[neighbours[i].x, neighbours[i].y].IsMine)
                    {
                        cells[x, y].MineCount++;
                    }
                }
            }
        }
    }

    private List<Vector2Int> GetNeighbours(int x, int y)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;
                
                if (nx >= 0 && nx < gridSize.x && ny >= 0 && ny < gridSize.y)
                {
                    neighbours.Add(new Vector2Int(nx, ny));
                }
            }
        }
        return neighbours;
    }

    private void OpenCell(Vector2Int cellPosition)
    {
        Cell cell = cells[cellPosition.x, cellPosition.y];

        if (cell.IsOpen)
            return;
        
        cell.IsOpen = true;

        if (cell.MineCount > 0)
            return;
        
        List<Vector2Int> neighbours = GetNeighbours(cellPosition.x, cellPosition.y);
        foreach (Vector2Int neighbour in neighbours)
        {
            OpenCell(neighbour);
        }
    }

    private Vector3 GetCellPosition(int x, int y)
    {
        float width = gridSize.x + (gridSize.x - 1) * spacing;
        float height = gridSize.y + (gridSize.y - 1) * spacing;
        Vector3 topLeftCorner = transform.position - new Vector3(width / 2f - 0.5f, height / 2f - 0.5f, 0);
        Vector3 offset = new Vector3(x * (1 + spacing), y * (1 + spacing), 0);
        return topLeftCorner + offset;
    }

    private void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = 0.2f;
        Gizmos.color = color;
        
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Gizmos.DrawCube(GetCellPosition(x, y), Vector3.one);
            }
        }
    }
}
