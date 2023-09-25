using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Settings")]
    private Vector3 velocity;
    [SerializeField] LayerMask playerMask;
    [SerializeField] private float detectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        Move();
        CheckForPlayer();
    }

    private void Move()
    {
        transform.position += velocity * Time.deltaTime;
    }

    public void Configure(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    private void CheckForPlayer()
    {
        Collider[] detectedPlayer = Physics.OverlapSphere(transform.position, detectionRadius, playerMask);

        foreach (var playerCollider in detectedPlayer)
        {
            playerCollider.GetComponent<PlayerMovement>().TakeDamage();
        }
    }
}
