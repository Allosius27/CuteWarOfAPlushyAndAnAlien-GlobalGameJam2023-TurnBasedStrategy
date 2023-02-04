using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePlayer : MonoBehaviour
{
    private int _currentColorOwned;

    private int _baseActionPoints;

    [SerializeField] public int actionPoints = 2;
    //[SerializeField] public int attackPoints = 1;
    [SerializeField] public int movementCost = 1;
    [SerializeField] public int attackCost = 1;

    private void Awake()
    {
        _baseActionPoints = actionPoints;

        
    }

    private void Start()
    {
        UpdateColorCount();

    }

    public void ResetActionPoints()
    {
        actionPoints = _baseActionPoints;
    }

    private void UpdateColorCount()
    {
        if (GameCore.Instance.CurrentTurn >= GameCore.Instance.endGameTurn)
        {
            GameCore.Instance.gameCanvasManager.creaturePlayerScoreText.text = "?";
        }
        else
        {
            GameCore.Instance.gameCanvasManager.creaturePlayerScoreText.text = _currentColorOwned.ToString();
        }
    }

    public void ChangeColorOwned(int amount)
    {
        _currentColorOwned += amount;

        UpdateColorCount();
    }
}
