using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject Canvas;
    public Image image1;
    public Image image2;
    public Image image3;

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
    private void Update()
    {
       // SetActionUI(actionsPoints);
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
    public void SetActionUI(float number)
    {
        Canvas.SetActive(true);

       
        if (number == 3)
        {
            image1.enabled = true;
            image2.enabled = true;
            image3.enabled = true;
        }
        else if (number == 2)
        {
            image1.enabled = false;
            image2.enabled = true;
            image3.enabled = true;
        }
        else if (number == 1)
        {
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = true;
        }
        else 
        {
            Canvas.SetActive(false);
        }

    }
    #endregion
}
