using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    private Rigidbody2D rd2d;
    public GameObject player;
    public float speed;
    public int isSprinting = 0;
    private bool facingRight = true;
    private float hMove;
    private float yMove;
    private float xVel;
    private float yVel;
    private float xMax;
    private float xMin;
    private bool isWalking;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public TextMeshProUGUI score;
    public TextMeshProUGUI lives;
    public TextMeshProUGUI win;
    public TextMeshProUGUI lose;
    public int scoreNum;
    private int livesCount;
    public GameObject respawn;
    public GameObject respawn2;
    public GameObject endGame;
    public GameObject endWalls;
    bool loseState;
    bool winState;
    private int stage = 1;
    public GameObject[] coins;
    public GameObject[] enemies;
    public AudioSource audioSource;
    public AudioClip backgroundSound;
    public AudioClip victorySound;
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        rd2d.transform.position = respawn.transform.position;

        for(int i = 0; i < 8; i++)
        {
          coins[i].SetActive(true);
        }
        for(int i = 0; i < 8; i++)
        {
          enemies[i].SetActive(true);
        }
        audioSource.clip = backgroundSound;
        audioSource.Play();
        audioSource.loop = true;
        win.enabled = false;
        lose.enabled = false;
        scoreNum = 0;
        livesCount = 3;
        lives.text = "Lives: " + livesCount.ToString();
        score.text = "Score: " + scoreNum.ToString();
        endWalls.SetActive(false);
    }
   
    
    void FixedUpdate()
    {
      float hozMovement = Input.GetAxis("Horizontal");
      float vertMovement = Input.GetAxis("Vertical");
      rd2d.AddForce(new Vector2(hozMovement * speed, 0));
      hMove = hozMovement;
      yMove = vertMovement;
      isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
      yVel = rd2d.velocity.y;
      xVel = rd2d.velocity.x;
      if(xVel > xMax) xMax = xVel;
      if(xVel < xMin) xMin = xVel;
      if(xVel <= 0) xMax = 0;
      if(xVel >= 0) xMin = 0;
      if(isWalking && isOnGround && Input.GetKeyDown(KeyCode.W))
      {
        anim.SetInteger("state", 3);
      }

      
    }
    void Update()
    {
      animate();
      if(yVel != 0 && yVel < 0 && !isOnGround)
      {
        anim.SetInteger("state", 4);
      }
      if (yMove == 0 && yVel == 0 && hMove == 0) anim.SetInteger("state", 0);

      if(rd2d.transform.position.y <= -65)
      {
        livesCount--;
        lives.text = "Lives: " + livesCount.ToString();
        if(stage == 1) rd2d.transform.position = respawn.transform.position;
        if(stage == 2) rd2d.transform.position = respawn2.transform.position;
        rd2d.velocity = Vector2.zero;
      }
      if(livesCount <= 0)
      {
        stage = 0;
        loseState = true;
        lose.enabled = true;
        endWalls.SetActive(true);
        scoreNum = 0;
        rd2d.transform.position = endGame.transform.position;
      }
      if(Input.GetKeyDown(KeyCode.Space) && loseState || Input.GetKeyDown(KeyCode.Space) && winState)
      {
        audioSource.Stop();
        rd2d.velocity = Vector2.zero;
        winState = false;
        loseState = false;
        Start();
      }
      if(scoreNum == 4 && stage == 1)
      {
        stage02();
        stage++;
      }
      if(scoreNum == 4 && stage == 2)
      {
        
        winState = true;
        win.enabled = true;
        lives.text = "";
        score.text = "";
        scoreNum = 0;
        endWalls.SetActive(true);
        rd2d.transform.position = endGame.transform.position;
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = victorySound;
        audioSource.Play();
      }
    }
    private void animate()
    {
      if (facingRight == false && hMove > 0)
      {
        flip();
      }

      else if (facingRight == true && hMove < 0)
      {
        flip();
      }

      if(Input.GetKeyDown(KeyCode.R))
      {
         isSprinting = 1;
      }

      if(Input.GetKeyDown(KeyCode.F))
      {
        isSprinting = 0;
      }


      if (xMax > 0 && isOnGround && hMove != 0)
      {
        isWalking = true;
        if (isSprinting == 1)
        {
          anim.SetInteger("state", 2);
        }
        else if (isSprinting == 0)
        {
          anim.SetInteger("state", 1);
        }
      }
      if (xMin < 0 && isOnGround && hMove != 0)
      {
        isWalking = true;
        if (isSprinting == 1)
        {
          anim.SetInteger("state", 2);
        }
        else if (isSprinting == 0)
        {
          anim.SetInteger("state", 1);
        }
      }

      if(yVel > 0 && !isOnGround)
      {
        anim.SetInteger("state", 3);
      }

      
      if(hMove == 0 && yMove == 0)
      {
        anim.SetInteger("state", 0);
      }

      if(isSprinting == 0)
      {
        speed = 25;
      }
      else if(isSprinting == 1)
      {
        speed = 50;
      }
    }
    private void stage02()
    {
        rd2d.transform.position = respawn2.transform.position;
        win.enabled = false;
        lose.enabled = false;
        scoreNum = 0;
        livesCount = 3;
        lives.text = "Lives: " + livesCount.ToString();
        score.text = "Score: " + scoreNum.ToString();
    }
    void flip()
    
    {
      facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
     
      if (collision.collider.tag == "bounce")
      {
        if(xVel < 0)
        {
          rd2d.velocity = Vector2.zero;
          rd2d.AddForce(new Vector2(10, 18), ForceMode2D.Impulse);
        }
        if(xVel > 0)
        {
          rd2d.velocity = Vector2.zero;
          rd2d.AddForce(new Vector2(-10, 18), ForceMode2D.Impulse);
        }
      }
    }
    private void OnCollisionStay2D(Collision2D collision)

    {
      if(collision.collider.tag == "ground" && isOnGround)
      {
        if(Input.GetKey(KeyCode.W))
        {
          rd2d.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        }
      }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
      if(other.CompareTag("enemy"))
      {
        livesCount--;
        lives.text = "Lives: " + livesCount.ToString();
        other.gameObject.SetActive(false);
      }
      if (other.tag == "coin")
      {
        scoreNum += 1;
        score.text = "Score: " + scoreNum.ToString();
        other.gameObject.SetActive(false);
      }
   }
}
