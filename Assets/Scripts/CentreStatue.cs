using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CentreStatue : MonoBehaviour
{
    public static CentreStatue instance;

    [SerializeField] float maxHealth;
    [SerializeField] float health;
    [SerializeField] Image healthImage;

    private bool isDead;

    void Start()
    {
        instance = this;
        health = maxHealth;   
    }

    // Update is called once per frame
    void Update()
    {
        healthImage.rectTransform.sizeDelta = new Vector2(health * 4f, healthImage.rectTransform.sizeDelta.y);
    }

    public void TakeDamage(string data)
    {
        string[] dataSplit = data.Split('_');
        health -= float.Parse(dataSplit[1]);
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        DeathAnimation.instance.BeginAnimation();
    }

}
