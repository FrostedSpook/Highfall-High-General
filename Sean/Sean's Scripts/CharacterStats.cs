using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public delegate void OnHealthChanged(int health, int maxHealth);
    public OnHealthChanged healthChanged;
    public int health;
    public int maxHealth;
    public bool isHit;

    public GameObject dmgPop;
    public Vector3 popUpOffset = new Vector3(0, 1, 0);

    //function to be called by other scripts
    public void DealDamage(CharacterStats target, int min, int max)
    {
        int amount = Random.Range(min, max);
        target.TakeDamage(amount);
        Vector3 ranOffset = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0);
        GameObject popUpRef = Instantiate(dmgPop, target.transform.position + popUpOffset + ranOffset, Quaternion.identity);
        popUpRef.GetComponentInChildren<TMP_Text>().text = amount.ToString();
        isHit = true;
        Invoke(nameof(CanBeHit), 0.7f);
    }

    void TakeDamage(int amount)
    {
        SetHealth(health - amount);
        healthChanged?.Invoke(health, maxHealth);
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
