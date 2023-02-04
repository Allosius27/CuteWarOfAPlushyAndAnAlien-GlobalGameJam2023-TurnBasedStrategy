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
}
