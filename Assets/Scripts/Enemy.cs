using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    public bool showDestroy = false;
    public string objectType = "Unspecified";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        showDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage, Player playerClass)
    {
        //Debug.Log("Slime: Ouch");
        health -= damage;
        if (health <= 0)
        {
            playerClass.AddInventory(objectType);
            Die();
        }
    }
    void Die()
    {
        // Handle death (destroy, play animation, etc.)
        showDestroy = true;
        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
