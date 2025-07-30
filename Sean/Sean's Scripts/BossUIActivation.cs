using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIActivation : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.SetActive(true);
        }
    }
}
