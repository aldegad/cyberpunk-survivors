using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{

    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    public float fireRate = 5f;

    [SerializeField]
    public float liftTime = 5f;

    public GameObject fireEffect;
    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (fireEffect != null)
        {
            GameObject fireObject = Instantiate(fireEffect, transform.position, transform.rotation);
            ParticleSystem fireParticle = fireObject.GetComponent<ParticleSystem>();
            Destroy(fireObject, fireParticle.main.duration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        Destroy(gameObject, liftTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0;

        ContactPoint contactPoint = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);

        if (hitEffect != null)
        {
            GameObject hitObject = Instantiate(hitEffect, contactPoint.point, rotation);
            ParticleSystem hitParticle = hitObject.GetComponent<ParticleSystem>();
            Destroy(hitObject, hitParticle.main.duration);
        }

        Destroy(gameObject);
    }
}
