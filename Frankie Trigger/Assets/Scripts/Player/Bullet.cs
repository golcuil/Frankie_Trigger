using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Vector3 velocity;
    [SerializeField] LayerMask enemiesMask;
    [SerializeField] private float detectionRadius;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckForEnemies();
    }

    private void Move()
    {
        transform.position += velocity * Time.deltaTime;
    }

    public void Configure(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    private void CheckForEnemies()
    {
        Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, detectionRadius, enemiesMask);

        foreach (var enemyCollider in detectedEnemies)
        {
            enemyCollider.GetComponent<Enemy>().TakeDamage();
        }
    }
}
