using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    #region Fields
    public GameObject GoOnCase;

    public TypeOnCase typeOnCase;

    private HexCoordinates _hexCoordinates;

    #endregion

    #region Properties

    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    #endregion

    #region UnityInspector

    [SerializeField] private GlowHighlight _glowHighlight;

    [SerializeField] private HexType _hexType;

    #endregion

    #region Behaviour

    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
        _glowHighlight = GetComponent<GlowHighlight>();
    }

    public int GetCost()
        => _hexType switch
        {
            HexType.Difficult => 20,
            HexType.Default => 10,
            HexType.Road => 5,
            _ => throw new Exception($"Hex of type {_hexType} not supported")
        };

    public bool IsObstacle()
    {
        return this._hexType == HexType.Obstacle;
    }

    public void EnableHighlight()
    {
        _glowHighlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        _glowHighlight.ToggleGlow(false);
    }

    public void ResetHighlight()
    {
        _glowHighlight.ResetGlowHighlight();
    }

    public void HighlightPath()
    {
        _glowHighlight.HighlightValidPath();
    }

    #endregion
}



public enum TypeOnCase
{
    None,
    Player,
    Root,
    Enemy
}

public enum HexType
{
    None,
    Default,
    Difficult,
    Road,
    Water,
    Obstacle,
}
