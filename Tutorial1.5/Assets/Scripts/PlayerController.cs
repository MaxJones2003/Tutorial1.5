using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    private Rigidbody2D rd2d;
    public float speed;
    public int isSprinting = 0;
    public TextMeshProUGUI score;
    private int scoreNum = 0;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreNum.ToString();
    }
   
    
    void FixedUpdate()
    {
      float hozMovement = Input.GetAxis("Horizontal");
      float vertMovement = Input.GetAxis("Vertical");
      rd2d.AddForce(new Vector2(hozMovement * speed, 0));
    }
    void Update()
    {
      animate();
      
    }
    private void animate()
    {
      if(Input.GetKeyDown(KeyCode.R))
      {
         isSprinting = 1;
      }
      if(Input.GetKeyDown(KeyCode.F))
      {
        isSprinting = 0;
      }
      if (Input.GetKeyDown(KeyCode.D))
      {
        if (isSprinting == 1)
        {
          anim.SetInteger("state", 2);
        }
        else if (isSprinting == 0)
        {
          anim.SetInteger("state", 1);
        }
      }

     if (Input.GetKeyUp(KeyCode.D))
      {
        anim.SetInteger("state", 0);
      }

      if (Input.GetKeyDown(KeyCode.A))
      {
        if (isSprinting == 1)
        {
          anim.SetInteger("state", 4);
        }
        else if (isSprinting == 0)
        {
          anim.SetInteger("state", 3);
        }
      }

      if (Input.GetKeyUp(KeyCode.A))
      {
        anim.SetInteger("state", 5);
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.collider.tag == "coin")
      {
        scoreNum += 1;
        score.text = scoreNum.ToString();
        Destroy(collision.collider.gameObject);
      }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
      if(collision.collider.tag == "ground")
      {
        if(Input.GetKey(KeyCode.W))
        {
          rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
        }
      }
    }
   }
