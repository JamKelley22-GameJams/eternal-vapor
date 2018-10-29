using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Movement : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    public float dashTime;
    public float dashModifier;
    public float messageXOffset;
    public float messageYOffset;
    public float timeKeeper;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public GUIStyle guiStyle;
    public Font guiFont;
    public int fontSize;
    private float moveHorizontal;
    private float direction = 1;
    private float displayTime;
    private Collider2D collider;
    private Rigidbody2D rb2d;
    private int JumpCount = 0;
    public int MaxJumps = 1;
    public bool canWalljump = false;
    public bool canDash = false;
    public bool canTeleport = false;
    public bool displayMessage;
    public string walljumpMessage;
    public string dashMessage;
    public string teleportMessage;
    public string winMessage;
    public string currentMessage;
    //private GameComponent player;

    public Animator ani;
    public GameObject pc_GFX;
    [Range(0, 2)]
    public float JumpDelay;
    public bool grounded;
    public LayerMask layerMask;

    string lastHit = "Floor";

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        guiStyle.font = guiFont;
        guiStyle.fontSize = fontSize;

        startPos = gameObject.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown("r") && canTeleport)
        {
            ThrowGrenade();
        }
        if (Input.GetKeyDown("space") && JumpCount != MaxJumps)
        {
            StartCoroutine(Jump());
        }
        if (Input.GetKeyDown("e") && canDash)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x + (direction * dashModifier), rb2d.velocity.y);
            timeKeeper = dashTime;
        }
        if (timeKeeper > 0)
        {
            timeKeeper = timeKeeper - Time.deltaTime;
        }
        if (timeKeeper < 0)
        {
            timeKeeper = 0;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        if (displayTime > 0.0)
        {
            displayTime = displayTime - Time.deltaTime;
        }
        else
        {
            displayMessage = false;
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        ani.SetInteger("Horizontal", (int)moveHorizontal);
        Vector3 s = pc_GFX.transform.eulerAngles;
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            pc_GFX.transform.eulerAngles = new Vector3(s.x, (moveHorizontal > 0) ? -56.09f : 75f, s.z);
        }

        if (moveHorizontal != 0)
        {
            direction = moveHorizontal;
        }

        transform.Translate((Vector3.right * moveHorizontal) * Time.deltaTime * speed);

        float dist = .4f;
        float charWidth = .2f;
        Vector3 left = new Vector3(transform.position.x + charWidth, transform.position.y, transform.position.z);
        Vector3 right = new Vector3(transform.position.x - charWidth, transform.position.y, transform.position.z);
        RaycastHit2D hitL = Physics2D.Raycast(left, -Vector2.up, dist, layerMask);
        RaycastHit2D hitR = Physics2D.Raycast(right, -Vector2.up, dist, layerMask);
        Debug.DrawRay(left, -Vector3.up, Color.green);
        Debug.DrawRay(right, -Vector3.up, Color.green);

        if (hitL || hitR)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        ani.SetBool("Grounded", grounded);

    }

    void ThrowGrenade()
    {
        // Create the Bullet from the Bullet Prefab

        if (!(GameObject.FindGameObjectsWithTag("Grenade").Length > 0))
        {
            Vector3 directionalPosition = new Vector3(bulletSpawn.position.x + direction,
                                                      bulletSpawn.position.y,
                                                      bulletSpawn.position.z);
            var bullet = (GameObject)Instantiate(
            bulletPrefab,
            directionalPosition,
            bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(6 * direction, 6);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bullet.GetComponent<Rigidbody2D>().velocity.x, bullet.GetComponent<Rigidbody2D>().velocity.y + 2.5f);

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }
    }

    void OnGUI()
    {
        if (displayMessage)
        {
            GUI.Label(new Rect((Screen.width / 2) - messageXOffset, (Screen.height / 2) - messageYOffset, 400f, 200f), currentMessage, guiStyle);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Jumpable")
        {
            JumpCount = 0;
            lastHit = "Floor";
        }
        else if((canWalljump && col.gameObject.tag == "Wall"))
        {
            JumpCount = 0;
            lastHit = "Wall";
        }
        Debug.Log(lastHit);
        if (col.gameObject.tag == "Wall")
        {
            rb2d.gravityScale = 0.8f;
            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f);
            }
        } else
        {
            rb2d.gravityScale = 1.0f;
        }
        if (col.gameObject.tag == "Walljump")
        {
            displayTime = 5.0f;
            displayMessage = true;
            currentMessage = walljumpMessage;
        }
        if (col.gameObject.tag == "Dash")
        {
            displayTime = 5.0f;
            displayMessage = true;
            currentMessage = dashMessage;
        }
        if (col.gameObject.tag == "VaporTeleport")
        {
            displayTime = 5.0f;
            displayMessage = true;
            currentMessage = teleportMessage;
        }
        if (this.gameObject.tag == "Last")
        {
            displayTime = 100.0f;
            displayMessage = true;
            currentMessage = winMessage;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            Manager.Instance.DoPlayerReturn(gameObject,startPos,true);
        }
    }

    IEnumerator Jump()
    {
        if(lastHit == "Floor")
        {
            ani.SetTrigger("Jump");
            yield return new WaitForSeconds(JumpDelay);
            Vector3 up = transform.TransformDirection(Vector3.up);
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            JumpCount++;
        }
        else
        {
            ani.SetTrigger("WallJump");
            yield return new WaitForSeconds(JumpDelay);
            Vector3 up = transform.TransformDirection(Vector3.up);
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            JumpCount++;
        }
    }
}