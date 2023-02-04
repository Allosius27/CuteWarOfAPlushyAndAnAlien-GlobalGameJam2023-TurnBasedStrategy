using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : AllosiusDevUtilities.Singleton<GameCore>
{
    #region Properties

    public UnitManager unitManager { get; protected set; }

    public Player playerRoot { get; protected set; }

    public bool CreaturesTurn { get; set; } = false;

    public List<Unit> creatures = new List<Unit>();

    #endregion

    protected override void Awake()
    {
        base.Awake();

        playerRoot = FindObjectOfType<Player>();

        unitManager = FindObjectOfType<UnitManager>();
    }

    public void CheckEnemiesTurnState()
    {
        if(CheckEnemiesTurn() == false)
        {
            CreaturesTurn = false;

            playerRoot.ResetActionPoints();
        }
        else
        {
            CreaturesTurn = true;
        }
    }

    public void SetEnemiesTurn()
    {
        CreaturesTurn = true;

        for (int i = 0; i < creatures.Count; i++)
        {
            creatures[i].ResetActionPoints();
        }
    }

    public bool CheckEnemiesTurn()
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            if(creatures[i].actionPoints > 0)
            {
                return true;
            }
        }

        return false;
    }
}
