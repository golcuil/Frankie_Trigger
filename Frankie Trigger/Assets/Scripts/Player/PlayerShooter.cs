using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject shootingLine;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private Transform bulletsParent;

    [Header("Settings")]
    [SerializeField] private float bulletSpeed;
    private bool canShoot;
    

    private void Awake()
    {
        PlayerMovement.onEnteredWarzone += EnteredWarzoneCallback;
        PlayerMovement.onExitedWarzone += ExitedWarzoneCallback;
        PlayerMovement.onDied += DiedCallback;
    }
    private void OnDestroy()
    {
        PlayerMovement.onEnteredWarzone -= EnteredWarzoneCallback;
        PlayerMovement.onExitedWarzone -= ExitedWarzoneCallback;
        PlayerMovement.onDied -= DiedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetShootingLineVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
            ManageShooting();
    }

    private void EnteredWarzoneCallback()
    {
        SetShootingLineVisibility(true);
        canShoot = true;
    }

    private void ExitedWarzoneCallback()
    {
        SetShootingLineVisibility(false);
        canShoot = false;
    }

    private void SetShootingLineVisibility(bool visibility)
    {
        shootingLine.SetActive(visibility);
    }

    private void ManageShooting()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }
    
    public void Shoot()
    {
        Vector3 direction = bulletSpawnPosition.right;
        direction.z = 0;

        Bullet bulletInstance = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity, bulletsParent);

        bulletInstance.Configure(direction * bulletSpeed);
    }

    private void DiedCallback()
    {
        SetShootingLineVisibility(false);
        canShoot = false;
    }
}
