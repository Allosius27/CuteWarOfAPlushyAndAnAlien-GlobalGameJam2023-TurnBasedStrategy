using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private HexGrid hexGrid;

    [SerializeField]
    private MovementSystem movementSystem;

    public GameObject GoMine;

    [SerializeField]
    public Unit selectedUnit;
    private Hex previouslySelectedHex;
    Hex selectedHex;
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

        //if (GameCore.Instance.EnemiesTurn)
        //{
        //    if (this.selectedUnit == unitReference || GameCore.Instance.creaturePlayer.unitsOwned.Contains(unitReference))
        //    {
        //        ClearOldSelection();
        //        return true;
        //    }
        //}
        //else
        //{
        //    if (this.selectedUnit == unitReference)
        //    {
        //        ClearOldSelection();
        //        return true;
        //    }
        //}
        return false;
    }

    public void HandleTerrainSelected(GameObject hexGO)
    {
        if (selectedUnit == null || GameCore.Instance.EnemiesTurn == false)
        {
            return;
        }

        selectedHex = hexGO.GetComponent<Hex>();

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
        if (selectedHex.typeOnCase == TypeOnCase.Item ) 
        {
            if (selectedHex.typeItem == TypeItem.Flaque)
            {
                Destroy(selectedHex.GoOnCase);
                selectedHex.typeOnCase = TypeOnCase.None;
                selectedHex.team = Team.Creature;
                selectedHex.ChangeMaterial(false);
                List<Vector3Int> listAColorer = hexGrid.GetNeighboursFor(selectedHex.HexCoords);
                for (int ii = 0; ii < listAColorer.Count; ii++)
                {
                    hexGrid.GetTileAt(listAColorer[ii]).typeItem = TypeItem.None;
                    hexGrid.GetTileAt(listAColorer[ii]).typeOnCase = TypeOnCase.None;
                    hexGrid.GetTileAt(listAColorer[ii]).team = Team.Creature;
                    hexGrid.GetTileAt(listAColorer[ii]).ChangeMaterial(false);
                }
            }
            else if (selectedHex.typeItem == TypeItem.Pixels)
            {
                Destroy(selectedHex.GoOnCase);
                selectedHex.typeOnCase = TypeOnCase.None;
                selectedHex.typeItem = TypeItem.None;
                selectedHex.team = Team.Creature;
                selectedHex.ChangeMaterial(false);

                for (int ii = 0; ii < 10; ii++)
                {
                    Vector3 pos = new Vector3(Random.Range(GameCore.Instance.leftTop.position.x, GameCore.Instance.rightBotom.position.x), 0, Random.Range(GameCore.Instance.rightBotom.position.z, GameCore.Instance.leftTop.position.z));
                    Vector3Int vector3Int = HexCoordinates.ConvertPositionToOffset(pos);
                    Hex actualTile = hexGrid.GetTileAt(vector3Int);
                    if (actualTile != null)
                    {
                        actualTile.typeOnCase = TypeOnCase.None;
                        actualTile.typeItem = TypeItem.None;
                        actualTile.team = Team.Creature;
                        actualTile.ChangeMaterial(false);
                    }
                    
                }

            }
            else if (selectedHex.typeItem == TypeItem.Mine)
            {
                selectedHex.team = Team.Creature;
                Destroy(selectedHex.GoOnCase);
                selectedHex.GoOnCase = Instantiate(GoMine, selectedHex.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                selectedHex.typeItem = TypeItem.None;
            }


        }






        selectedUnit.MovementFinished -= ResetTurn;
        GameCore.Instance.CheckEnemiesTurnState();
    }
}
