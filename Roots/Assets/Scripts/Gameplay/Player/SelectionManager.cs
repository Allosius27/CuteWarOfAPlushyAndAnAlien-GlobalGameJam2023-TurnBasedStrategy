using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    #region Fields

    List<Vector3Int> _neighbours = new List<Vector3Int>();

    #endregion

    #region UnityInspector

    [SerializeField] private Camera _mainCamera;

    public LayerMask selectionMask;

    public HexGrid hexGrid;

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
        GameObject result;
        if(FindTarget(mousePosition, out result))
        {
            Hex selectedHex = result.GetComponent<Hex>();

            selectedHex.DisableHighlight();

            foreach(Vector3Int neighbour in _neighbours)
            {
                hexGrid.GetTileAt(neighbour).DisableHighlight();
            }

            _neighbours = hexGrid.GetNeighboursFor(selectedHex.HexCoords);

            foreach (Vector3Int neighbour in _neighbours)
            {
                hexGrid.GetTileAt(neighbour).EnableHighlight();
            }

            Debug.Log($"Neighbours for {selectedHex.HexCoords} are:");
            foreach (Vector3Int neighbourPos in _neighbours)
            {
                Debug.Log(neighbourPos);
            }
        }
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
