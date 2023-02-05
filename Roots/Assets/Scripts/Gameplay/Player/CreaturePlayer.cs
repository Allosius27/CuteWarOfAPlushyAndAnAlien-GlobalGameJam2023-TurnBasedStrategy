using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreaturePlayer : MonoBehaviour
{
    private int _currentColorOwned;

    private int _baseActionPoints;

    public int CurrentColorOwned => _currentColorOwned;

    [SerializeField] public int actionPoints = 2;
    //[SerializeField] public int attackPoints = 1;
    [SerializeField] public int movementCost = 1;
    [SerializeField] public int attackCost = 1;

    public List<Unit> unitsOwned = new List<Unit>();

    public GameObject Canvas;
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    bool start = true;

    private void Awake()
    {
        _baseActionPoints = actionPoints;

        
    }

    private void Start()
    {
        UpdateColorCount();

        Unit unit = GetComponent<Unit>();
        if(unit != null && unitsOwned.Contains(unit) == false)
        {
            unitsOwned.Add(unit);
        }

    }

    private void Update()
    {
        SetActionUI(actionPoints, start);
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



    public void SetActionUI(int number, bool start)
    {
        if (GameCore.Instance.player.actionsPoints==0)
        {
            start = false;
        }
        Canvas.SetActive(!start);
        Canvas.transform.rotation = Quaternion.Euler(51, 0, 0);

        if (number == 4)
        {
            image1.enabled = true;
            image2.enabled = true;
            image3.enabled = true;
            image4.enabled = true;
        }
        else if (number == 3)
        {
            image1.enabled = false;
            image2.enabled = true;
            image3.enabled = true;
            image4.enabled = true;
        }
        else if (number == 2)
        {
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = true;
            image4.enabled = true;
        }
        else if (number == 1)
        {
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = false;
            image4.enabled = true;
        }
        else
        {
            Canvas.SetActive(false);
        }
    }

}
