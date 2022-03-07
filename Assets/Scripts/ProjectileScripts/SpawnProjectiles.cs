using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{

    [SerializeField]
    private GameObject firePoint;

    [SerializeField]
    private List<GameObject> projectiles = new List<GameObject> ();

    private PlayerMovement playerMovement;
    private GameObject effectToSpawn;

    private float timeToFire = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        effectToSpawn = projectiles[0];
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + (1 / effectToSpawn.GetComponent<ProjectileMoveScript>().fireRate);
            spawnProjectile();
        }
    }

    void spawnProjectile() 
    {
        GameObject projectile;

        if (firePoint != null)
        {
            projectile = Instantiate(effectToSpawn, firePoint.transform.position, playerMovement.GetRotation());
        }
    }
}
