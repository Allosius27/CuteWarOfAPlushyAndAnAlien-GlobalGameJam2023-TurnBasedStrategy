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
}
