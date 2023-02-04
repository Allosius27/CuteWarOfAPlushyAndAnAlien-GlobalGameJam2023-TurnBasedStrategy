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

    #region UnityInspector

    [SerializeField] private GlowHighlight _glowHighlight;

    #endregion

    #region Behaviour

    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
        _glowHighlight = GetComponent<GlowHighlight>();
    }

    public void EnableHighlight()
    {
        _glowHighlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        _glowHighlight.ToggleGlow(false);
    }

    #endregion
}
