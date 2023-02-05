using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMovable : Unit
{
    public MaterialStruct[] mats;

    public override void UnlockCreature()
    {
        isActive = true;

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].renderer.material = mats[i].matToApplied;
        }
    }
}

[Serializable]
public struct MaterialStruct
{
    public Material matToApplied;
    public Renderer renderer;
}
