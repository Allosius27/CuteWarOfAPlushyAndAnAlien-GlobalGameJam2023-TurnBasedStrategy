using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePlayer : MonoBehaviour
{
    private int _baseActionPoints;

    [SerializeField] public int actionPoints = 2;
    [SerializeField] public int attackPoints = 1;
    [SerializeField] public int movementCost = 1;

    private void Awake()
    {
        _baseActionPoints = actionPoints;
    }

    public void ResetActionPoints()
    {
        actionPoints = _baseActionPoints;
    }
}
