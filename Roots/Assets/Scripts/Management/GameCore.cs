using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : AllosiusDevUtilities.Singleton<GameCore>
{
    #region Properties

    public Player player { get; protected set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<Player>();
    }
}
