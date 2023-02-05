using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : AllosiusDevUtilities.Singleton<GameCore>
{
    #region Fields

    private int _currentTurn;

    #endregion

    #region Properties

    public int CurrentTurn => _currentTurn;

    public HexGrid hexGrid { get; protected set; }
    public UnitManager unitManager { get; protected set; }
    public SelectionManager selectionManager { get; protected set; }

    public Player player { get; protected set; }

    public CreaturePlayer creaturePlayer { get; protected set; }

    public GameCanvasManager gameCanvasManager { get; protected set; }

    public bool EnemiesTurn { get; set; } = false;

    #endregion

    #region UnityInspector

    public int partyDuration = 15;

    public int endGameTurn = 10;

    public  Transform leftTop;
    public  Transform rightBotom;
    public GameObject Item;
    public GameObject ItemFlaque;
    public GameObject ItemPixels;
    public GameObject ItemMine;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<Player>();
        creaturePlayer = FindObjectOfType<CreaturePlayer>();

        unitManager = FindObjectOfType<UnitManager>();
        hexGrid = FindObjectOfType<HexGrid>();
        selectionManager = FindObjectOfType<SelectionManager>();

        gameCanvasManager = FindObjectOfType<GameCanvasManager>();
    }

    private void Start()
    {
        UpdateTurn();
    }

    public void NewTurn()
    {
        _currentTurn++;
        UpdateTurn();
    }

    public void UpdateTurn()
    {
        gameCanvasManager.remainingTurnsText.text = "Tours restants : " + (partyDuration - _currentTurn).ToString();
    }

    public void CheckEnemiesTurnState()
    {
        if(CheckEnemiesTurn() == false)
        {
            EnemiesTurn = false;

            player.ResetActionPoints();
            NewTurn();
        }
        else
        {
            EnemiesTurn = true;
        }
    }

    public void SetEnemiesTurn()
    {
        SpawnItem();
        EnemiesTurn = true;

        creaturePlayer.ResetActionPoints();
    }

    public bool CheckEnemiesTurn()
    {
        if (creaturePlayer.actionPoints > 0)
        {
            return true;
        }

        return false;
    }


    public void SpawnItem()
    {
        if (1f *100 > Random.Range(1, 100))
        {
            Vector3 pos = new Vector3(Random.Range(leftTop.position.x, rightBotom.position.x), 0, Random.Range(rightBotom.position.z, leftTop.position.z));
            Vector3Int vector3Int = HexCoordinates.ConvertPositionToOffset(pos);
            Hex actualTile = hexGrid.GetTileAt(vector3Int);
            if (actualTile == null || actualTile.typeOnCase == TypeOnCase.Player || actualTile.typeOnCase == TypeOnCase.Enemy || actualTile.typeOnCase == TypeOnCase.Root || actualTile.team == Team.Creature || actualTile.team == Team.Root)
            {
                SpawnItem();
                return;
            }
                pos = actualTile.transform.position + new Vector3(0, 1, 0);
            actualTile.GoOnCase = Instantiate(Item, pos, Quaternion.identity);
            actualTile.typeOnCase = TypeOnCase.Item;

            int type = Random.Range(1, 100);
            Debug.Log(type);
            if (type<=50)
            {
                actualTile.typeItem = TypeItem.Flaque;
            }
            else
            {
                actualTile.typeItem = TypeItem.Pixels;
            }



        }

    }



     


}
