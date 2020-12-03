using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPos;
    private Vector3 endPos;
    private Camera cam;
    private bool collided = false;

    public float speed = 20;
    public LayerMask mask;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if(t.phase == TouchPhase.Began)
            {
                //When the player starts dragging ball
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
                    Vector2 position = new Vector2(transform.position.x, transform.position.y);
                    float length = Vector2.Distance(position, hit.point);
                    Debug.Log("Length : " + length + " Position : " + position + " Hit point : " + hit.point);
                    Debug.DrawRay(transform.position, direction * (length), Color.green);
                }
            }

            if(t.phase == TouchPhase.Ended)
            {
                //When the player stops dragging ball
                Vector2 direction = (startPos - endPos).normalized;

                rb.velocity = direction * speed;
            }
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            //Debug.Log("Collided");
            if (!collided)
            {
                collided = true;
            }
            else
            {
                collided = false;
                rb.velocity = Vector2.zero;
            }
        }
    }
}
