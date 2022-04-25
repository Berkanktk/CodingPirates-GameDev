using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    int maxLifeTime = 3000;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (maxLifeTime-- <= 0)
            Destroy(gameObject);

        transform.Translate(0, -5 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || (collision.gameObject.layer == LayerMask.NameToLayer("Ground")))
        {
            Destroy(gameObject);
        }
    }
}
