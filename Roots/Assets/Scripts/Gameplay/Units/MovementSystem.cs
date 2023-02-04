using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    #region Fields

    private BFSResult _movementRange = new BFSResult();
    private List<Vector3Int> _currentPath = new List<Vector3Int>();

    #endregion

    #region Properties

    public BFSResult MovementRange => _movementRange;

    #endregion

    #region Behaviour

    public void HideRange(HexGrid hexGrid)
    {
        foreach (Vector3Int hexPosition in _movementRange.GetRangePositions())
        {
            hexGrid.GetTileAt(hexPosition).DisableHighlight();
        }
        _movementRange = new BFSResult();
    }

    public void ShowRange(Unit selectedUnit, HexGrid hexGrid)
    {
        CalculateRange(selectedUnit, hexGrid);

        Vector3Int unitPos = hexGrid.GetClosestHex(selectedUnit.transform.position);

        foreach (Vector3Int hexPosition in _movementRange.GetRangePositions())
        {
            if (unitPos == hexPosition)
                continue;
            hexGrid.GetTileAt(hexPosition).EnableHighlight();
        }
    }

    public void CalculateRange(Unit selectedUnit, HexGrid hexGrid)
    {
        _movementRange = GraphSearch.BFSGetRange(hexGrid, hexGrid.GetClosestHex(selectedUnit.transform.position), selectedUnit.MovementPoints);
    }


    public void ShowPath(Vector3Int selectedHexPosition, HexGrid hexGrid)
    {
        if (_movementRange.GetRangePositions().Contains(selectedHexPosition))
        {
            foreach (Vector3Int hexPosition in _currentPath)
            {
                hexGrid.GetTileAt(hexPosition).ResetHighlight();
            }
            _currentPath = _movementRange.GetPathTo(selectedHexPosition);
            foreach (Vector3Int hexPosition in _currentPath)
            {
                hexGrid.GetTileAt(hexPosition).HighlightPath();
            }
        }
    }

    public void MoveUnit(Unit selectedUnit, HexGrid hexGrid)
    {
        Debug.Log("Moving unit " + selectedUnit.name);
        selectedUnit.MoveThroughPath(_currentPath.Select(pos => hexGrid.GetTileAt(pos).transform.position).ToList());
        foreach (Vector3Int hexPosition in _currentPath)
        {
            StartCoroutine(CoroutineColoration(hexPosition));
        }

    }

    private IEnumerator CoroutineColoration(Vector3Int hexPosition)
    {
        yield return new WaitForSeconds(0.2f);

        GameCore.Instance.hexGrid.GetTileAt(hexPosition).ChangeMaterial(false);
        
        GameCore.Instance.hexGrid.GetTileAt(hexPosition)._glowHighlight.InitOriginalMaterials();
    }

    public bool IsHexInRange(Vector3Int hexPosition)
    {
        return _movementRange.IsHexPositionInRange(hexPosition);
    }


    #endregion
}