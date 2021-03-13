using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    private int scoreValue = 0;

    public Text lives;
    private int livesValue = 3;

    public Text winText;
    public Text loseText;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    Animator anim;

    private bool facingRight = true;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;



    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("state", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("state", 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("state", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("state", 0);
        }
        
        if (isOnGround == false)
        {
            anim.SetInteger("state", 2);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        
         isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if (scoreValue == 4)
            {
                transform.position = new Vector2(37, 0);
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
                
            }
            if (scoreValue == 8)
            {
                winText.text = "You Win! Game created by Ryan.";
                musicSource.clip = musicClipTwo;
                musicSource.Stop();
                musicSource.clip = musicClipOne;
                musicSource.Play();
            }
        }
        else if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
            if (livesValue <= 0)
            {
                loseText.text = "You lose. :(";
                Destroy(this);
            }
        }
        
    }

    

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag =="Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
    
    
}