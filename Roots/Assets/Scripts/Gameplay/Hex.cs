using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    #region Fields

    private HexCoordinates _hexCoordinates;

    #endregion

    #region Properties

    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    #endregion

    #region Behaviour

    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
    }

    #endregion
}
