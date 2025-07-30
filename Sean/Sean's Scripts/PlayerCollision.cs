using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public int minDamage;
    public int maxDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CharacterStats stats = other.GetComponent<CharacterStats>();
            if (stats.isHit == false)
                stats.DealDamage(stats, minDamage, maxDamage);
        }
    }
    
}
