using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] GameObject gameobj;
    [SerializeField] bool ModeSet;
    [SerializeField] AudioClip clickSound;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            AudioSource audioPlayer = GetComponent<AudioSource>();
            audioPlayer.PlayOneShot(clickSound, 1);

            BoxCollider2D coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;

            Animator animC = gameObject.GetComponent<Animator>();
            animC.enabled = false;

            gameobj.SetActive(ModeSet);
        }
    }
}
