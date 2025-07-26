using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public int health = 50;
    public string objectType = "Unspecified";

    public bool destroyAnimationOver = false;
    public bool destroyWithoutAnimation = false;

    private bool ranOnce = false;

    void Start()
    {
        destroyAnimationOver = false;
        ranOnce=false;
    }

    void Update()
    {
        if(destroyAnimationOver)
        {
            DestroySelf();
        }

    }

    public void TakeDamage(int damage, Player playerClass)
    {
        Debug.Log($"{objectType}: Ouch");
        health -= damage;
        if (health <= 0)
        {
            if(ranOnce==false)
            {
                playerClass.AddInventory(objectType);
                Oof();
                ranOnce = true;
            }
        }
    }
    void Oof()
    {
        Debug.Log($"{objectType}: Oof called");
        if(destroyWithoutAnimation==true)
        {
            DestroySelf();
        }
        else
        {
            if(animator!=null)
            {
                animator.SetTrigger("Destroy");
            }
        }
    }

    void DestroySelf()
    {
        Debug.Log($"{objectType}: Destroying");
        Destroy(gameObject);
    }
}
