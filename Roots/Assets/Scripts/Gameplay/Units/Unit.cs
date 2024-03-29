using AllosiusDevCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    #region Fields

    public GlowHighlight glowHighlight { get; protected set; }
    private Queue<Vector3> pathPositions = new Queue<Vector3>();
    public FeedbacksData feedbacksData;
    public FeedbacksReader FeedbacksReader;
    #endregion

    #region Properties

    public Hex cell;

    public int MovementPoints { get => movementPoints; }

    #endregion

    #region UnityInspector

    [SerializeField] public bool isActive = true;

    [SerializeField]
    private int movementPoints = 20;

    [SerializeField]
    private float movementDuration = 1, rotationDuration = 0.3f;

    #endregion

    #region Events

    public event Action<Unit> MovementFinished;

    #endregion

    #region Behaviour

    private void Awake()
    {
        glowHighlight = GetComponent<GlowHighlight>();

        
    }

    public virtual void Start()
    {
        CreaturePlayer creaturePlayer = GetComponent<CreaturePlayer>();

        if(creaturePlayer != null)
        {
            cell.hexType = HexType.Obstacle;


            cell.ChangeMaterial(false);
        }
    }

    public virtual void UnlockCreature()
    {
        
    }

    public void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        glowHighlight.ToggleGlow();
    }

    public void MoveThroughPath(List<Vector3> currentPath)
    {
        Debug.Log("MoveThroughPath");
        pathPositions = new Queue<Vector3>(currentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        StartCoroutine( SpawnFb());
        StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
    }

    private IEnumerator SpawnFb()
    {
        int i = 5;
        while (i!=0)
        {
            i--;
            FeedbacksReader.ReadFeedback(feedbacksData);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator RotationCoroutine(Vector3 endPosition, float rotationDuration)
    {
        Quaternion startRotation = transform.rotation;
        endPosition.y = transform.position.y;
        Vector3 direction = endPosition - transform.position;
        Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
        {
            float timeElapsed = 0;
            while (timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / rotationDuration; // 0-1
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }
            transform.rotation = endRotation;
        }
        StartCoroutine(MovementCoroutine(endPosition));
    }

    private IEnumerator MovementCoroutine(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;
        float timeElapsed = 0;

        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            

            yield return null;
        }
        transform.position = endPosition;
       

        if (pathPositions.Count > 0)
        {
            Debug.Log("Selecting the next position!");
            StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
        }
        else
        {
            Debug.Log("Movement finished!");
            MovementFinished?.Invoke(this);
        }
    }

    #endregion
}
