using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields

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

    #endregion
}
