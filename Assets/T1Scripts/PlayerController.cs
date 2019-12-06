using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text levelText;
    public Text lifeText;
    public Text countText;
    public Text winText;
    public float speed;

    private int level;
    private int lives;
    private int count;
    private Rigidbody2D rb2d;

    void Start()
    {
        level = 1;
        lives = 3;
        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
        setCountText();
        winText.text = "";
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(movement * speed);

        if (lives == 0)
        {
            transform.position = new Vector2(0.0f, 100.0f);
        }

        if (count == 20)
        {
            transform.position = new Vector2(100.0f, 0.0f);
        }

        if (Input.GetKey("escape"))
            Application.Quit();
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            setCountText();
        }

        else if (other.gameObject.CompareTag("Enemy")){
            other.gameObject.SetActive(false);
            lives -= 1;
            setCountText();
        }

        if (count == 12)
        {
            //An attempt to stop the player from carrying their momentum to the next level.
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector2 revMov = new Vector2(moveHorizontal, moveVertical);
            transform.position = new Vector2(100.0f, 100.0f);
            rb2d.AddForce(-10 * (revMov * speed));
            level += 1;
            setCountText();
        }

        if (lives == 0)
        {
            transform.position = new Vector2(0.0f, 100.0f);
            speed = 0;
        }

        if (count == 20)
        {
            transform.position = new Vector2(100.0f, 0.0f);
            speed = 0;
        }

    }

    void setCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 20)
        {
            winText.text = "You Win! Game created by Devin Fitzgerald.";
        }

        lifeText.text = "Lives: " + lives.ToString();
        if(lives <= 0)
        {
            winText.text = "Game Over. Game created by Devin Fitzgerald.";
        }

        levelText.text = "Level " + level.ToString();

        if (lives == 0)
        {
            levelText.text = "FATAL ERROR";
        }

        if (count == 20)
        {
            levelText.text = "Mission Complete";
        }
    }
}
