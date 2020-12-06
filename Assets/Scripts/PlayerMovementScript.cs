using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPos;
    private Vector3 endPos;
    private Camera cam;
    private LineRenderer line;
    private bool collidedOnce = false;
    private bool canMove = true;

    public float speed = 20;
    public LayerMask mask;
    public int score = 0;
    public int jumps = 2;
    public Text scoreText;
    public Text jumpsText;


    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        //Set up line renderer
        line = GetComponent<LineRenderer>();
        line.SetWidth(0.1f, 0.1f);

        //Initialize the text gameobjects
        scoreText.text = "Score:" + score;
        jumpsText.text = "Jumps:" + jumps;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(Input.touchCount > 0 && canMove && jumps > 0)
        {
            Touch t = Input.GetTouch(0);

            if(t.phase == TouchPhase.Began)
            {
                //When the player starts dragging ball set initial position
                startPos = t.position;
            }

            if(t.phase == TouchPhase.Moved)
            {
                //When the player drag ball
                endPos = t.position;
                
                Vector2 direction = (startPos - endPos).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction,Mathf.Infinity, mask);

                if (hit.collider != null)
                {
                    //Calculate the distance between ball and the hit-point on ray casted target
                    Vector2 position = new Vector2(transform.position.x, transform.position.y);
                    float length = Vector2.Distance(position, hit.point);

                    //Show the path using line renderer
                    line.enabled = true;
                    line.SetPosition(0, position + GetComponent<CircleCollider2D>().radius * direction);
                    line.SetPosition(1, hit.point);

                    //Debug.Log("Length : " + length + " Position : " + position + " Hit point : " + hit.point);
                    //Debug.DrawRay(transform.position, direction * (length), Color.green);
                }
            }

            if(t.phase == TouchPhase.Ended)
            {
                //When the player stops dragging ball
                Vector2 direction = (startPos - endPos).normalized;

                //Disable line and set velocity in that direction
                line.enabled = false;
                rb.velocity = direction * speed;

                //Player can't move and number of jumps decremented by 1
                canMove = false;
                jumps--;
                jumpsText.text = "Jumps:" + jumps;
            }
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            //Debug.Log("Collided");
            if (!collidedOnce && rb.velocity != Vector2.zero)
            {
                collidedOnce = true;
                Debug.Log("Collide");
            }
            else if(collidedOnce && rb.velocity != Vector2.zero)
            {
                //Ball collided twice and stopped and can move again
                collidedOnce = false;
                rb.velocity = Vector2.zero;
                canMove = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            score += 1;
            scoreText.text = "Score:" + score;
            collision.transform.GetComponent<PickupScript>().Destroy();
        }
    }


}
