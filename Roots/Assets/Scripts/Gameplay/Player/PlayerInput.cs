using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    #region UnityInspector

    public UnityEvent<Vector3> PointerClick;

    #endregion

    #region Behaviour

    private void Update()
    {
        DetectMouseClick();
    }

    private void DetectMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            PointerClick?.Invoke(mousePos);
        }
    }

    #endregion
}
