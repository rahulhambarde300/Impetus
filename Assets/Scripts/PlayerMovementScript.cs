using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPos;
    private Vector3 endPos;
    private Camera cam;
    private LineRenderer line;
    private bool collided = false;

    public float speed = 20;
    public LayerMask mask;


    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        line = GetComponent<LineRenderer>();
        line.SetWidth(0.1f, 0.1f);
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

                line.enabled = false;
                rb.velocity = direction * speed;
            }
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            //Debug.Log("Collided");
            if (!collided && rb.velocity != Vector2.zero)
            {
                collided = true;
                Debug.Log("Collide");
            }
            else if(collided && rb.velocity != Vector2.zero)
            {
                collided = false;
                rb.velocity = Vector2.zero;
            }
        }
    }

    
}
