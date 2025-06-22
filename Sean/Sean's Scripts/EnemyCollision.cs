using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public int damage;
    bool isInCollision = false;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && isInCollision == false)
        {
            isInCollision = true;
            CharacterStats stats = other.GetComponent<CharacterStats>();

            stats.DealDamage(stats, damage);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && isInCollision == true)
        {
            isInCollision = false;
        }
    }
}

