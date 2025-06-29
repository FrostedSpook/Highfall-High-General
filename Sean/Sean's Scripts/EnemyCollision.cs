using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public int minDamage;
    public int maxDamage;
    bool isInCollision = false;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && isInCollision == false)
        {
            isInCollision = true;
            CharacterStats stats = other.GetComponent<CharacterStats>();

            stats.DealDamage(stats, minDamage, maxDamage);
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

