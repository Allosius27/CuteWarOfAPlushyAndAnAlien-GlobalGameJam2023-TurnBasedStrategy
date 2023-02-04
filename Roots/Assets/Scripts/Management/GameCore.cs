using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : AllosiusDevUtilities.Singleton<GameCore>
{
    #region Properties

    public HexGrid hexGrid { get; protected set; }
    public UnitManager unitManager { get; protected set; }

    public Player player { get; protected set; }

    public CreaturePlayer creaturePlayer { get; protected set; }

    public bool EnemiesTurn { get; set; } = false;

    public List<Unit> enemies = new List<Unit>();

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
    }

    public void CheckEnemiesTurnState()
    {
        if(CheckEnemiesTurn() == false)
        {
            EnemiesTurn = false;

            player.ResetActionPoints();
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
        for (int i = 0; i < enemies.Count; i++)
        {
            if (creaturePlayer.actionPoints > 0)
            {
                return true;
            }
        }

        return false;
    }


    [ContextMenu("test")]
    public void SpawnItem()
    {
        if (1f *100 > Random.Range(1, 100))
        {
            Vector3 pos = new Vector3(Random.Range(leftTop.position.x, rightBotom.position.x), 0, Random.Range(rightBotom.position.z, leftTop.position.z));
            Vector3Int vector3Int = HexCoordinates.ConvertPositionToOffset(pos);
            Hex actualTile = hexGrid.GetTileAt(vector3Int);
            if (actualTile==null)
            {
                return;
            }
            pos = actualTile.transform.position + new Vector3(0, 1, 0);
            actualTile.GoOnCase = Instantiate(Item, pos, Quaternion.identity);
            actualTile.typeOnCase = TypeOnCase.Item;

            int type = Random.Range(1, 100);
            Debug.Log(type);
            if (type<=33)
            {
                actualTile.typeItem = TypeItem.Flaque;
            }
            else if (type <= 66)
            {
                actualTile.typeItem = TypeItem.Mine;
            }
            else
            {
                actualTile.typeItem = TypeItem.Pixels;
            }



        }

    }



     


}
