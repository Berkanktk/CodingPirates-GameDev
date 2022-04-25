using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_shoot : MonoBehaviour
{
    [SerializeField] GameObject fireball;
    [SerializeField] int fireRate = 500;

    Animator animator;
    Quaternion rotation;
    Vector3 pos;
    int rate;

    void Start()
    {
        animator = GetComponent<Animator>();
        rate = 100;
        rotation = transform.rotation * Quaternion.Euler(0, 0, 180);
        pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
    }

    void Update()
    {
        if (rate == 50)
        {
            // Animate shoot
            animator.SetTrigger("Shoot");
        }

        if (rate-- <= 0)
        {
            // Shoot
            rate = fireRate;
            Instantiate(fireball, pos, rotation);
        }

    }
}
