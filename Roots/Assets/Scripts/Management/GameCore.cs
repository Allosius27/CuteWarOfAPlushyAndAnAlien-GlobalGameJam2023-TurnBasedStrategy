using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : AllosiusDevUtilities.Singleton<GameCore>
{
    #region Properties

    public UnitManager unitManager { get; protected set; }

    public Player player { get; protected set; }

    public bool EnemiesTurn { get; set; } = false;

    public List<Unit> enemies = new List<Unit>();

    #endregion

    protected override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<Player>();

        unitManager = FindObjectOfType<UnitManager>();
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

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].ResetActionPoints();
        }
    }

    public bool CheckEnemiesTurn()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].actionPoints > 0)
            {
                return true;
            }
        }

        return false;
    }
}
