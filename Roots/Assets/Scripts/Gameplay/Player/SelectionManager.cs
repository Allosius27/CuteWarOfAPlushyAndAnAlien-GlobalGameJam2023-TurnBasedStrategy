using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    #region Fields
    public Hex previousHex;
    public Hex playerHex;
    public float distanceFromSource;
    Hex selectedHex;
    List<Vector3Int> _neighbours = new List<Vector3Int>();

    #endregion

    #region UnityInspector

    [SerializeField] private Camera _mainCamera;

    public LayerMask selectionMask;

    public UnityEvent<GameObject> OnUnitSelected;
    public UnityEvent<GameObject> TerrainSelected;

    public UnitManager unitManager;

    public HexGrid hexGrid;

    public GameObject RacineGO;

    #endregion

    #region Behaviour

    private void Awake()
    {
        if(_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
    }

    public void HandleClick(Vector3 mousePosition)
    {
        previousHex = selectedHex;
        GameObject result;
        if(FindTarget(mousePosition, out result))
        {
            selectedHex = result.GetComponent<Hex>();

            if (UnitSelected(result))
            {
                OnUnitSelected?.Invoke(result);
            }
            else
            {
                if (!unitManager.selectedUnit)
                {
                    HandleTerrainClick(result);
                }
                TerrainSelected?.Invoke(result);
            }

        }
    }

    public void HandleTerrainClick(GameObject result)
    {
        if(unitManager.selectedUnit)
        {
            selectedHex.DisableHighlight();

            foreach (Vector3Int neighbour in _neighbours)
            {
                hexGrid.GetTileAt(neighbour).DisableHighlight();
            }

            //TerrainSelected?.Invoke(result);
            //return;
        }
       

        selectedHex.DisableHighlight();

        foreach (Vector3Int neighbour in _neighbours)
        {
            hexGrid.GetTileAt(neighbour).DisableHighlight();
        }
        if (selectedHex.typeOnCase == TypeOnCase.None )
        {
            _neighbours = hexGrid.GetNeighboursFor(selectedHex.HexCoords);
            //BFSResult bfsResult = GraphSearch.BFSGetRange(hexGrid, selectedHex.HexCoords, 20);
            //_neighbours = new List<Vector3Int>(bfsResult.GetRangePositions());

            foreach (Vector3Int neighbour in _neighbours)
            {
                //hexGrid.GetTileAt(neighbour).EnableHighlight();
            }

            
        }

        for (int i = 0; i < _neighbours.Count; i++) // Est ce que 1 des voisins est bien relié?
        {
            Debug.Log(hexGrid.GetTileAt(_neighbours[i]).typeOnCase);
            if ((hexGrid.GetTileAt(_neighbours[i]).typeOnCase == TypeOnCase.Root || hexGrid.GetTileAt(_neighbours[i]).typeOnCase == TypeOnCase.Player) && GameCore.Instance.EnemiesTurn == false)
            {
                if (Vector3.Magnitude(selectedHex.HexCoords - playerHex.HexCoords) <= distanceFromSource) // Relier a une certaine distance du player
                {
                    if (selectedHex.typeOnCase == TypeOnCase.None && GameCore.Instance.player.actionsPoints >= GameCore.Instance.player.createRootCost) // si rien sur la case dinstantiation
                    {
                        selectedHex.ShowHideActionUI(false);
                        selectedHex.GoOnCase = Instantiate(RacineGO, selectedHex.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                        selectedHex.typeOnCase = TypeOnCase.Root;
                        selectedHex.hexType = HexType.Obstacle;

                        selectedHex.team = Team.Root;
                        selectedHex.ChangeMaterial(true);
                        GameCore.Instance.player.TakeAction(GameCore.Instance.player.createRootCost);

                        return;
                    }
                }
            }
        }
        //if (selectedHex.typeOnCase==TypeOnCase.Root)
        //{
        //    selectedHex.ShowHideActionUI(true);
        //}

    }

    public void HandleRightClick(Vector3 mousePosition) //Destroy
    {
        previousHex = selectedHex;
        GameObject result;
        if (FindTarget(mousePosition, out result))
        {
            selectedHex = result.GetComponent<Hex>();

            if (selectedHex.typeOnCase == TypeOnCase.Root)
            {
                Destroy(selectedHex.GoOnCase);
                selectedHex.typeOnCase = TypeOnCase.None;

                DestroyNotConnectedTiles();
            }
        }
    }



    public List<Vector3Int> TestConnectionRecursif(bool continued, Vector3Int actualTile, List<Vector3Int> titles)
    {
        if (continued==false)
        {
            return titles;
        }

        List<Vector3Int> _neighbours = hexGrid.GetNeighboursFor(actualTile);
        for (int i = 0; i < _neighbours.Count; i++)
        {
            if (!titles.Contains(_neighbours[i]) && hexGrid.GetTileAt(_neighbours[i]).typeOnCase==TypeOnCase.Root)
            {
                titles.Add(_neighbours[i]);
                TestConnectionRecursif(true, _neighbours[i], titles);
            }
        }
        return titles;
    }

    public void DestroyNotConnectedTiles()
    {
        List<Vector3Int> tilesConnected = new List<Vector3Int>();
        tilesConnected = TestConnectionRecursif(true, playerHex.HexCoords, tilesConnected);


        foreach (var tile in hexGrid.GetDic())
        {
            if (!tilesConnected.Contains(tile.Key) && tile.Value.typeOnCase == TypeOnCase.Root)
            {
                Hex queu = tile.Value;
                Destroy(queu.GoOnCase);
                queu.typeOnCase = TypeOnCase.None;
            }
        }
    }


    private bool UnitSelected(GameObject result)
    {
        return result.GetComponent<Unit>() != null;
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        if(Physics.Raycast(ray, out hit, 100, selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }

    #endregion
}
