using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TilePlacementController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    private bool interactPressed;
    
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Tilemap buildTilemap;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Collider2D buildArea;

    [Header("Tile Selection")] 
    public bool isErasing;
    [SerializeField] private TileBase selectedTile;

    public int maxTileCount = 30;
    public int tileCount;

    private void Start()
    {
        tileCount = maxTileCount;
    }

    private void Update()
    {
        if (!interactPressed)
            return;

        interactPressed = false;

        //Don't interact with the world when clicking UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (!isErasing)
        {
            PlaceTile();
            
            UIManager.Instance.SetTileCountUI(tileCount);
        }
        else
        {
            EraseTile();
            
            UIManager.Instance.SetTileCountUI(tileCount);
        }
    }

    private Vector3Int GetMouseCell()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = -mainCamera.transform.position.z;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        return buildTilemap.WorldToCell(worldPosition);
    }

    private bool CanPlaceTile(Vector3Int cell)
    {
        Vector3 cellCenter = buildTilemap.GetCellCenterWorld(cell);

        // Check build area
        if (!buildArea.OverlapPoint(cellCenter))
            return false;

        // Don't overwrite existing environment
        if (groundTilemap.HasTile(cell))
            return false;

        // Don't overwrite another placed tile
        if (buildTilemap.HasTile(cell))
            return false;

        return true;
    }

    private void PlaceTile()
    {
        if (selectedTile == null)
            return;

        if (tileCount <= 0)
        {
            Debug.Log("OUT OF TILES OH NOOOO");
            return;
        }
            

        Vector3Int cell = GetMouseCell();

        if (!CanPlaceTile(cell))
            return;

        buildTilemap.SetTile(cell, selectedTile);
        tileCount--;
    }

    private void EraseTile()
    {
        Vector3Int cell = GetMouseCell();

        if (buildTilemap.HasTile(cell))
        {
            buildTilemap.SetTile(cell, null);
            tileCount++;
        }
    }
    
    

    public void ToggleBuild()
    {
        isErasing = false;
    }
    
    public void ToggleErase()
    {
        isErasing = true;
    }
    
    private void OnEnable()
    {
        interactAction.action.Enable();
        interactAction.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        interactAction.action.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interactPressed = true;
    }
}
