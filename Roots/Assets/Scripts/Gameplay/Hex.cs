using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Hex : MonoBehaviour
{
    #region Fields
    [HideInInspector] public GameObject GoOnCase;
    public GameObject PrefabAction1;
    public GameObject PrefabAction2;
    public GameObject PrefabAction3;

    public TypeOnCase typeOnCase;

    public TypeRoot typeRoot;

    public GameObject ActionUI;
    public Button Action1;
    public Button Action2;
    public Button Action3;

    private HexCoordinates _hexCoordinates;

    #endregion

    #region Properties

    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    #endregion

    #region UnityInspector

    [SerializeField] private GlowHighlight _glowHighlight;

    [SerializeField] public HexType hexType;

    #endregion

    #region Behaviour

    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
        _glowHighlight = GetComponent<GlowHighlight>();
    }
    private void Start()
    {
        ListenerUI();
    }
    public int GetCost()
        => hexType switch
        {
            HexType.Difficult => 20,
            HexType.Default => 10,
            HexType.Road => 5,
            _ => throw new Exception($"Hex of type {hexType} not supported")
        };

    public bool IsObstacle()
    {
        return this.hexType == HexType.Obstacle;
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

    public void ShowHideActionUI(bool show)
    {
        ActionUI.SetActive(show);
    }

    public void ListenerUI()
    {
        Action1.onClick.AddListener(() => OnClickAction1());
        Action2.onClick.AddListener(() => OnClickAction2());
        Action3.onClick.AddListener(() => OnClickAction3());
    }

    public void OnClickAction1()
    {
        Destroy(GoOnCase);
        GoOnCase = Instantiate(PrefabAction1, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        typeOnCase = TypeOnCase.Root;
        typeRoot = TypeRoot.Attack;
    }

    public void OnClickAction2()
    {
        Destroy(GoOnCase);
        GoOnCase = Instantiate(PrefabAction2, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        typeOnCase = TypeOnCase.Root;
        typeRoot = TypeRoot.Defence;
    }

    public void OnClickAction3()
    {
        Destroy(GoOnCase);
        GoOnCase = Instantiate(PrefabAction3, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        typeOnCase = TypeOnCase.Root;
        typeRoot = TypeRoot.Other;
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
public enum TypeRoot
{
    None,
    Normal,
    Attack,
    Defence,
    Other

}
