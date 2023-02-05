using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Hex : MonoBehaviour
{
    #region Fields

    private Material _initialMat;

    public GameObject GoOnCase;

    public GameObject PrefabAction1;
    public GameObject PrefabAction2;
    public GameObject PrefabAction3;

    public TypeOnCase typeOnCase;

    public TypeRoot typeRoot;

    public TypeItem typeItem;

    public Team team;

    public GameObject ActionUI;

    public MeshRenderer meshColor;
    public MeshStruct[] meshs;
    

    [HideInInspector] public Button Action1;//
    [HideInInspector] public Button Action2;//
    [HideInInspector] public Button Action3;//

    private HexCoordinates _hexCoordinates;

    #endregion

    #region Properties

    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    #endregion

    #region UnityInspector

    [SerializeField] public GlowHighlight _glowHighlight;

    [SerializeField] public HexType hexType;

    public Material creatureMat, rootMat;

    #endregion

    #region Behaviour

    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
        _glowHighlight = GetComponent<GlowHighlight>();

        if(meshs.Length > 0)
        {
            for (int i = 0; i < meshs.Length; i++)
            {
                if (meshs[i].parent.activeSelf)
                {
                    meshColor = meshs[i].mesh;
                    break;
                }
            }
        }
        
    }
    private void Start()
    {
        _initialMat = meshColor.material;

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

    public void ChangeMaterial(bool isRacineColor)
    {
        if(team == Team.Root)
        {
            GameCore.Instance.player.ChangeColorOwned(-1);
        }
        else if(team == Team.Creature)
        {
            GameCore.Instance.creaturePlayer.ChangeColorOwned(-1);
        }

        if (isRacineColor)
        {
            meshColor.material = rootMat;
            _glowHighlight.InitOriginalMaterials();

            GameCore.Instance.player.ChangeColorOwned(1);

            team = Team.Root;
        }
        else
        {
            meshColor.material = creatureMat;
            _glowHighlight.InitOriginalMaterials();

            GameCore.Instance.creaturePlayer.ChangeColorOwned(1);

            team = Team.Creature;
        }

    }

    public void NeutralMaterial()
    {
        if (team == Team.Root)
        {
            GameCore.Instance.player.ChangeColorOwned(-1);
        }
        else if (team == Team.Creature)
        {
            GameCore.Instance.creaturePlayer.ChangeColorOwned(-1);
        }

        meshColor.material = _initialMat;
        _glowHighlight.InitOriginalMaterials();

        team = Team.None;
    }


    #endregion
}


public enum Team
{
    None,
    Root, 
    Creature
}


public enum TypeOnCase
{
    None,
    Player,
    Root,
    Enemy,
    Item
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


public enum TypeItem
{
    None,
    Flaque,
    Pixels

}

[Serializable]
public struct MeshStruct
{
    public MeshRenderer mesh;
    public GameObject parent;
}