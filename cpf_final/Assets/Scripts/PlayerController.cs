using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    [SerializeField] float speed = 10;
    [SerializeField] float jumpPower = 200;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip itemSound;
    [SerializeField] AudioClip deadSound;
    [SerializeField] Text scoreText;
    [SerializeField] Text livesText;
    SpriteRenderer sr;
    Rigidbody2D rb;
    CapsuleCollider2D coll;
    Animator animator;
    AudioSource audioPlayer;
    int point;
    int lives;
    bool dead;
    Vector3 playerStartPos;

    // Start is called before the first frame update
    void Start() {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        point = PlayerPrefs.GetInt("Score");
        playerStartPos = transform.position;
        
        if (PlayerPrefs.HasKey("Lives"))
            lives = PlayerPrefs.GetInt("Lives");
        else
            lives = 3;
    }

    // Update is called once per frame
    void Update() {
        float dir = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetButtonDown("Jump");

        rb.velocity = new Vector2(dir *speed, rb.velocity.y);
        
        // Jump
        if (dir < 0)
            sr.flipX = true;
        if (dir > 0)
            sr.flipX = false;
        
        if (dir == 0)
            animator.SetBool("Run", false);
        else 
            animator.SetBool("Run", true);
        
        
        if (jump && GroundCheck()) {
            rb.AddForce(Vector3.up * jumpPower);
            audioPlayer.PlayOneShot(jumpSound, 1);
        }
        scoreText.text = "Score: " + point;
        livesText.text = "Lives:" + lives;
    }

    bool GroundCheck()
    {
        return Physics2D.CapsuleCast(coll.bounds.center, coll.bounds.size, 0f, 0f, Vector2.down, 0.2f, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dead)
        {
            if (collision.gameObject.CompareTag("Fruit"))
            {
                Animator animC = collision.gameObject.GetComponent<Animator>();
                animC.SetTrigger("Taken");
                //Destroy(collision.gameObject);
                audioPlayer.PlayOneShot(itemSound, 1);
                point += 10;    // Alle give 10 point
                if (collision.gameObject.name.Contains("Orange"))
                    point += 10;  // Nogle giver ekstra
                if (collision.gameObject.name.Contains("Bananas"))
                    point += 20;  // Nogle giver ekstra
                if (collision.gameObject.name.Contains("Strawberry"))
                    point += 30;  // Nogle giver ekstra
                if (collision.gameObject.name.Contains("Gem"))
                {
                    point += 90;  // Nogle giver ekstra
                    lives += 1;
                    PlayerPrefs.SetInt("Lives", lives);
                }
                PlayerPrefs.SetInt("Score", point);
            }

            if (collision.gameObject.CompareTag("Trap"))
            {
                dead = true;
                audioPlayer.PlayOneShot(deadSound, 1);
                //Destroy(gameObject, 1);
                animator.SetTrigger("Die");
                lives -= 1;
                PlayerPrefs.SetInt("Lives", lives);
                if (lives > 0)
                    Invoke("RestartLevel", 3);
                else
                    Invoke("RestartGame", 3);
            }
            if (collision.gameObject.CompareTag("Trampoline"))
            {
                Animator animC = collision.gameObject.GetComponent<Animator>();
                float forceY = Mathf.Abs(rb.velocity.y*jumpPower/8);
                if (forceY > 100)
                {
                    if (forceY > 800)
                        forceY = 800;
                    animC.SetTrigger("Jump");
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0, forceY));
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fan"))
        {
            float distX = collision.gameObject.transform.position.x - gameObject.transform.position.x;
            if (Mathf.Abs(distX) < 0.5f)
                distX = Mathf.Sign(distX) * 0.5f;
            float distY = Mathf.Abs(collision.gameObject.transform.position.y - gameObject.transform.position.y);
            float forceY = jumpPower / (7*distY * distY);
            float forceX = -jumpPower / (4*distX);
            if ((distY < 3) && (rb.velocity.y < 0))
                forceY *= 3;
            if (Mathf.Abs(forceY) > 450)
                forceY = Mathf.Sign(forceY)*450;
            rb.AddForce(new Vector2(forceX, forceY));
        }
    }
    private void RestartLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        animator.SetTrigger("Reborn");
        coll.enabled = true;
        sr.enabled = true;
        transform.position = new Vector3(playerStartPos.x, playerStartPos.y, playerStartPos.z);
        rb.velocity = Vector2.zero;
        dead = false;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }

}
