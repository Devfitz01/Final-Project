using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour
{
    Animator anim;

    private bool facingRight = true;
    private Rigidbody2D rd2d;
    private int scoreValue = 0;
    private int bugScore = 0;
    private int lifeValue = 3;
    private int levelCount = 1;

    public float speed;
    public Text score;
    public Text lives;
    public Text gameOver;
    public Text level;
    public AudioClip victory;
    public AudioClip coin;
    public AudioClip jump;
    public AudioClip quick;
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        UpdateText();
        gameOver.text = "";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if(hozMovement != 0.0)
        {
            anim.SetInteger("State", 1);
        }
        else
        {
            anim.SetInteger("State", 0);
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (lifeValue == 0)
        {
            score.text = "";
            level.text = "";
            lives.text = "";
            transform.position = new Vector2(0.0f, 100.0f);
            speed = 0;
        }

        if (scoreValue == 8)
        {
            score.text = "";
            level.text = "";
            lives.text = "";
            transform.position = new Vector2(100.0f, 0.0f);
            speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Final Project");

            bugScore = 0;
            scoreValue = 0;
            levelCount = 1;
            lifeValue = 3;
            gameOver.text = "";
            UpdateText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            musicSource.clip = coin;
            musicSource.Play();
            other.gameObject.SetActive(false);
            bugScore += 1;
            scoreValue += 1;
            UpdateText();
        }

        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lifeValue -= 1;
            UpdateText();
        }

        else if (other.gameObject.CompareTag("Fish"))
        {
            musicSource.clip = quick;
            musicSource.Play();
            other.gameObject.SetActive(false);
            speed = speed * 1.25F;
        }

        if (bugScore == 4)
        {
            //An attempt to stop the player from carrying their momentum to the next level.
            transform.position = new Vector2(100.0f, 100.0f);
            bugScore += 1;
            levelCount += 1;
            lifeValue = 3;
            UpdateText();
        }
    }

        private void OnCollisionStay2D(Collision2D collision)
    {
        anim.SetBool("inAir", false);
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                musicSource.clip = jump;
                musicSource.Play();
                anim.SetBool("inAir", true);
                anim.SetInteger("State", 2);
                rd2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
            }
        }
        else
        {
            anim.SetInteger("State", 2);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void UpdateText()
    {
        score.text = "Coins: " + scoreValue.ToString();
        if (scoreValue >= 8)
        {
            musicSource.clip = victory;
            musicSource.Play();
            gameOver.text = "Press R to reset. Game created by Devin Fitzgerald.";
        }

        lives.text = "Lives: " + lifeValue.ToString();
        if (lifeValue <= 0)
        {
            gameOver.text = "Press R to reset. Game created by Devin Fitzgerald.";
        }

        level.text = "Level " + levelCount.ToString();
    }
}