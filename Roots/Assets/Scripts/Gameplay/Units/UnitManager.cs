using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private HexGrid hexGrid;

    [SerializeField]
    private MovementSystem movementSystem;

    

    [SerializeField]
    public Unit selectedUnit;
    private Hex previouslySelectedHex;

    public void HandleUnitSelected(GameObject unit)
    {
        if (GameCore.Instance.EnemiesTurn == false)
            return;

        Unit unitReference = unit.GetComponent<Unit>();

        Debug.Log(unitReference);

        if (selectedUnit != null && selectedUnit.cell != null)
        {
            List<Vector3Int> neighbours = hexGrid.GetNeighboursFor(selectedUnit.cell.HexCoords);
            if (unitReference.cell != null && neighbours.Contains(unitReference.cell.HexCoords) && GameCore.Instance.creaturePlayer.actionPoints >= GameCore.Instance.creaturePlayer.attackCost)
            {
                if (unitReference is CreatureMovable)
                {
                    unitReference.UnlockCreature();
                    GameCore.Instance.creaturePlayer.actionPoints -= GameCore.Instance.creaturePlayer.attackCost;
                    ClearOldSelection();
                    GameCore.Instance.CheckEnemiesTurnState();
                    return;
                }

            }
        }

        if (unitReference.isActive == false)
        {
            return;
        }

        if (CheckIfTheSameUnitSelected(unitReference))
            return;

        PrepareUnitForMovement(unitReference);
    }

    private bool CheckIfTheSameUnitSelected(Unit unitReference)
    {
        if (this.selectedUnit == unitReference)
        {
            ClearOldSelection();
            return true;
        }
        return false;
    }

    public void HandleTerrainSelected(GameObject hexGO)
    {
        if (selectedUnit == null || GameCore.Instance.EnemiesTurn == false)
        {
            return;
        }

        Hex selectedHex = hexGO.GetComponent<Hex>();

        if(selectedUnit.cell != null)
        {
            List<Vector3Int> neighbours = hexGrid.GetNeighboursFor(selectedUnit.cell.HexCoords);
            if (neighbours.Contains(selectedHex.HexCoords) && GameCore.Instance.creaturePlayer.actionPoints >= GameCore.Instance.creaturePlayer.attackCost)
            {
                if(selectedHex.typeOnCase == TypeOnCase.Root)
                {
                    GameCore.Instance.selectionManager.HandleDestruction(selectedHex);
                    GameCore.Instance.creaturePlayer.actionPoints -= GameCore.Instance.creaturePlayer.attackCost;
                    ClearOldSelection();
                    GameCore.Instance.CheckEnemiesTurnState();
                    return;
                }
                
            }
        }
        

        if (HandleHexOutOfRange(selectedHex.HexCoords) || HandleSelectedHexIsUnitHex(selectedHex.HexCoords))
            return;

        HandleTargetHexSelected(selectedHex);


        //Hak
    }

    private void PrepareUnitForMovement(Unit unitReference)
    {
        if (this.selectedUnit != null)
        {
            ClearOldSelection();
        }

        this.selectedUnit = unitReference;
        this.selectedUnit.Select();
        movementSystem.ShowRange(this.selectedUnit, this.hexGrid);
    }

    private void ClearOldSelection()
    {
        previouslySelectedHex = null;
        this.selectedUnit.Deselect();
        movementSystem.HideRange(this.hexGrid);
        this.selectedUnit = null;

    }

    private void HandleTargetHexSelected(Hex selectedHex)
    {
        if(GameCore.Instance.creaturePlayer.actionPoints >= GameCore.Instance.creaturePlayer.movementCost)
        {
            if (previouslySelectedHex == null || previouslySelectedHex != selectedHex)
            {
                previouslySelectedHex = selectedHex;

                movementSystem.ShowPath(selectedHex.HexCoords, this.hexGrid);
            }
            else
            {
                if(selectedUnit.cell != null)
                {
                    selectedUnit.cell.hexType = HexType.Default;
                }

                selectedUnit.cell = selectedHex;

                if (selectedUnit.cell != null)
                {
                    selectedUnit.cell.hexType = HexType.Obstacle;
                }

                movementSystem.MoveUnit(selectedUnit, this.hexGrid);
                GameCore.Instance.creaturePlayer.actionPoints -= GameCore.Instance.creaturePlayer.movementCost;
                GameCore.Instance.EnemiesTurn = false;
                selectedUnit.MovementFinished += ResetTurn;
                ClearOldSelection();

            }
        }
        
    }

    private bool HandleSelectedHexIsUnitHex(Vector3Int hexPosition)
    {
        if (hexPosition == hexGrid.GetClosestHex(selectedUnit.transform.position))
        {
            selectedUnit.Deselect();
            ClearOldSelection();
            return true;
        }
        return false;
    }

    private bool HandleHexOutOfRange(Vector3Int hexPosition)
    {
        if (movementSystem.IsHexInRange(hexPosition) == false)
        {
            Debug.Log("Hex Out of range!");
            return true;
        }
        return false;
    }

    private void ResetTurn(Unit selectedUnit)
    {

        selectedUnit.MovementFinished -= ResetTurn;
        GameCore.Instance.CheckEnemiesTurnState();
    }
}
