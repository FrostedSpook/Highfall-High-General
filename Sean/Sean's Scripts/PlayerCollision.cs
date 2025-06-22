using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CharacterStats stats = other.GetComponent<CharacterStats>();
            if (stats.isHit == false)
                stats.DealDamage(stats, damage);
        }
    }
    
}
