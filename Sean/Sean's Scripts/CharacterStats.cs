using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int health;
    public int maxhealth;
    public bool isHit;

    //function to be called by other scripts
    public void DealDamage(CharacterStats target, int amount)
    {
        target.TakeDamage(amount);

        isHit = true;
        Invoke(nameof(CanBeHit), 0.7f);
    }

    void TakeDamage(int amount)
    {
        SetHealth(health - amount);
    }

    void SetHealth(int newAmount)
    {
        health = newAmount;
        if (health <= 0)
        {
            if (gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }

    private void CanBeHit()
    {
        isHit = false;
    }

}
