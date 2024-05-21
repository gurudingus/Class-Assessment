using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileLife = 3.0f;
    public int damageAmount = 1;

    void Start()
    {
        Destroy(gameObject, projectileLife);
    }

    void OnCollisionEnter(Collision collision)
    {
        TargetHealth targetHit = collision.gameObject.GetComponent<TargetHealth>();
        if(targetHit != null) 
        {
            targetHit.Damage(damageAmount);
        }
        Destroy(gameObject);
    }
}
