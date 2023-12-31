using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerDetection : MonoBehaviour
{
    [Header("Elements")]
    PlayerMovement playerMovement;

    [Header("Settings")]
    [SerializeField] private float detectionRadius;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.IsGameState())
            DetectStuff();
    }

    private void DetectStuff()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var collider in detectedObjects)
        {
            if (collider.CompareTag("Warzone Enter"))
            {
                EnteredWarzoneCallback(collider);
            }
            else if(collider.CompareTag("Finish"))
            {
                HitFinishLine();
            }

            if(collider.TryGetComponent(out Checkpoint checkpoint))
            {
                checkpoint.Interact();
            }
            

        }
    }

    private void HitFinishLine()
    {
        playerMovement.HitFinishLine();
    }

    private void EnteredWarzoneCallback(Collider warzoneTriggerCollider)
    {
        Warzone warzone = warzoneTriggerCollider.GetComponentInParent<Warzone>();
        playerMovement.EnteredWarzoneCallback(warzone);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    
}
