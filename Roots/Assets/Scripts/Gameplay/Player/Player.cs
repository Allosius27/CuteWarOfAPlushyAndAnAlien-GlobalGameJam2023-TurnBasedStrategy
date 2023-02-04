using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields

    private int _currentColorOwned;

    private float _baseActionPoints;

    #endregion

    #region UnityInspector

    public float actionsPoints = 3f;

    public float createRootCost = 1f;

    public Hex hexAssociated;

    #endregion

    #region Behaviour

    private void Awake()
    {
        _baseActionPoints = actionsPoints;

        
    }

    private void Start()
    {
        UpdateColorCount();

        hexAssociated.hexType = HexType.Obstacle;


        hexAssociated.ChangeMaterial(true);
    }

    public void TakeAction(float cost)
    {
        actionsPoints -= cost;

        if(actionsPoints <= 0)
        {
            GameCore.Instance.SetEnemiesTurn();


        }
    }

    public void ResetActionPoints()
    {
        actionsPoints = _baseActionPoints;
    }

    private void UpdateColorCount()
    {
        if(GameCore.Instance.CurrentTurn >= GameCore.Instance.endGameTurn)
        {
            GameCore.Instance.gameCanvasManager.rootPlayerScoreText.text = "?";
        }
        else
        {
            GameCore.Instance.gameCanvasManager.rootPlayerScoreText.text = _currentColorOwned.ToString();
        }
    }

    public void ChangeColorOwned(int amount)
    {
        _currentColorOwned += amount;

        UpdateColorCount();
    }

    #endregion
}
