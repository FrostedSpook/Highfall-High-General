using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private CharacterStats bossStats;
    public Slider slider;

    void OnEnable()
    {
        bossStats = boss.GetComponent<CharacterStats>();
        bossStats.healthChanged += SetPercentage;
    }
    void OnDisable()
    {
        bossStats.healthChanged -= SetPercentage;
    }

    private void SetPercentage(int health, int maxhealth)
    {

        slider.value = (float)health/(float)maxhealth;
    }
}
