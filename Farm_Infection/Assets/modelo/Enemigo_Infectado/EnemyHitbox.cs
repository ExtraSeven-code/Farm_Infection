using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.TakeDamage(damage);
        }
    }
}
