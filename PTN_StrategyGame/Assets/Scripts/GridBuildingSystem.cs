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

    public bool isPlacing = false; 
    private GameObject buildingPrefab;
    private UnityEngine.Vector3 targetPosition;
    [SerializeField] GameObject cannotPlaceText;
    private BuildingFactory buildingFactory;

    public AudioSource buttonClickSound;

    #region Unity Methods
    private void Awake()
    {
        instance = this;
        buildingFactory= gameObject.AddComponent<BuildingFactory>();
    }

    void Start()
    {
        cannotPlaceText.SetActive(false);   
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
        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        if (isPlacing)
        {
            HandleObjectPlacement(); // If currently placing a building, handle building placement logic
        }

        // Check if left mouse button is clicked
        else if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject(0))
            {
                UnityEngine.Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition = gridLayout.CellToLocalInterpolated(gridLayout.WorldToCell(touchPos));
            }
        }
    }

    #endregion

    #region Tile Management

    // Gets an array of tile bases within a given area
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
    // Sets a block of tiles within a given area to a specified tile type
    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z; 
        TileBase[] tileArray = new TileBase[size]; 
        FillTiles(tileArray, type); 
        tilemap.SetTilesBlock(area, tileArray);
    }

    // Fills an array of tile bases with a specified tile type
    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for(int i = 0; i<arr.Length; i++) 
        {
            arr[i] = tileBases[type];
        }
    }

    #endregion

    #region Building Placement

    // Handle the logic for placing a building, updating its position, and responding to key inputs
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
                + new UnityEngine.Vector3(.5f, .5f, 0f)); 
            prevPos = cellPos;
            FollowBuilding();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
                isPlacing = false; 
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearArea();
            Destroy(temp.gameObject);
            isPlacing = false;
        }
    }

    // Start placing a building by instantiating it and preparing for placement
    public void StartPlacing(GameObject buildingPrefab)
    {
        if (!isPlacing)
        {
            this.buildingPrefab = buildingPrefab;
            temp = buildingFactory.CreateBuilding(buildingPrefab);
            
            PlayButtonClickSound();

            FollowBuilding();
            isPlacing = true; 
        }
    }

    public void PlayButtonClickSound()
    {
        buttonClickSound.Play();
    }

    // Clears the area on the temporary tilemap that represents the potential building placement
    public void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    // Updates the temporary tilemap based on the building's position and neighboring tiles
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

    // Checks if a given area is valid for building placement
    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArr = GetTilesBlock(area, MainTilemap);
        foreach(var b in baseArr)
        {
            if(b!= tileBases[TileType.White])
            {
                Debug.Log("Cannot place here!");
                cannotPlaceText.SetActive(true);
                StartCoroutine(DisableText());
                return false;
            }
        }
        return true;
    }

    // Coroutine to disables the "cannot place here" text after a short delay
    private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(1f);
        cannotPlaceText.SetActive(false);
    }

    // Sets the tiles in the area to be taken by a building on the main tilemap
    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }

    // Clears a specified area on the main tilemap
    public void ClearTilemapArea(BoundsInt area)
    {
        TileBase[] toClear = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(toClear, TileType.White);
        MainTilemap.SetTilesBlock(area, toClear);
    }

    #endregion

    // Enum defining different types of tiles
    public enum TileType
    {
        Empty,
        White,
        Green,
        Red
    }
}
