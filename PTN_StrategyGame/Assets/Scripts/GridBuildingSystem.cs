using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem instance;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static Dictionary<TileType,TileBase> tileBases = new Dictionary<TileType,TileBase>();

    private Building temp;
    private UnityEngine.Vector3 prevPos;
    private BoundsInt prevArea;

    private bool isPlaced = false; // Başlangıçta henüz bir bina yerleştirilmediği varsayılır.
    private bool isPlacing = false; // Tıklanarak yerleştirme modunu temsil eder
    private GameObject buildingPrefab; // Yerleştirilecek objenin prefabı
    private UnityEngine.Vector3 targetPosition;

    #region Unity Methods
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null); 
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "stone")); 
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green")); 
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
    }

    void Update()
    {
        if (!temp)
        {
            return;
        }

        if (isPlacing)
        {
            HandleObjectPlacement();
        }
        else if (Input.GetMouseButtonDown(0)) // Eğer butona basılmışsa
        {
            if (!EventSystem.current.IsPointerOverGameObject(0))
            {
                UnityEngine.Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition = gridLayout.CellToLocalInterpolated(gridLayout.WorldToCell(touchPos));
                isPlacing = true;
            }
        }

    }

    #endregion

    #region Tile Management
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z]; 
        int counter = 0;
        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }
    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z; 
        TileBase[] tileArray = new TileBase[size]; 
        FillTiles(tileArray, type); 
        tilemap.SetTilesBlock(area, tileArray);
    }
    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for(int i = 0; i<arr.Length; i++) 
        {
            arr[i] = tileBases[type];
        }
    }


    #endregion

    #region Building Placement

    private void HandleObjectPlacement()
    {
        if (!temp)
        {
            return;
        }

        UnityEngine.Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

        if (prevPos != cellPos)
        {
            temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos
                + new UnityEngine.Vector3(.5f, .5f, 0f)); //Vector
            prevPos = cellPos;
            FollowBuilding();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
                isPlacing = false; // Yerleştirme modunu kapat
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearArea();
            Destroy(temp.gameObject);
            isPlacing = false; // Yerleştirme modunu kapat
        }
    }

    public void StartPlacing(GameObject buildingPrefab)
    {
        if (!isPlaced) // Eğer henüz bir bina yerleştirilmediyse...
        {
            this.buildingPrefab = buildingPrefab;
            temp = Instantiate(buildingPrefab, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity).GetComponent<Building>();

            FollowBuilding();

            isPlacing = true; // Yerleştirme modunu aç
        }
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;
        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);
        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArr = GetTilesBlock(area, MainTilemap);
        foreach(var b in baseArr)
        {
            if(b!= tileBases[TileType.White])
            {
                Debug.Log("Cannot place here!");
                return false;
            }
        }
        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }
    #endregion

    public enum TileType
    {
        Empty,
        White,
        Green,
        Red
    }

}
