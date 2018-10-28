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
    public string currentMessage;
    //private GameComponent player;

    public Animator ani;
    public GameObject pc_GFX;
    [Range(0, 2)]
    public float JumpDelay;
    public bool grounded;
    public LayerMask layerMask;

    string lastHit = "Floor";

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        guiStyle.font = guiFont;
        guiStyle.fontSize = fontSize;
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
        if (col.gameObject.tag == "Grenade")
        {
            displayTime = 5.0f;
            displayMessage = true;
            currentMessage = teleportMessage;
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

/* 
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
    public AudioClip throwSound;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public GUIStyle guiStyle;
    public Font guiFont;
    public int fontSize;
    private float moveHorizontal;
    private float direction = 1;
    private float timeKeeper;
    private float displayTime;
    private float distToGround;
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
    public string currentMessage;
    //private GameComponent player;

    public Animator ani;
    public GameObject pc_GFX;
    [Range(0, 2)]
    public float JumpDelay;
    public bool grounded;
    public LayerMask layerMask;
    public LayerMask wallJumpLayerMask;

    bool touchingRight = false;
    bool touchingLeft = false;

    [Range(0, 10)]
    public float maxVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        guiStyle.font = guiFont;
        guiStyle.fontSize = fontSize;
        distToGround = collider.bounds.extents.y;
        grounded = true;
    }

    void Update()
    {
        //Debug.Log((touchingLeft || touchingRight));
        if (Input.GetKeyDown("r") && canTeleport)
        {
            ThrowGrenade();
        }
        if (Input.GetKeyDown("space") && JumpCount != MaxJumps && grounded)
        {
            StartCoroutine(Jump());
        }
        else if(Input.GetKeyDown("space") && (touchingLeft || touchingRight))
        {
            StartCoroutine(Jump());
        }
        if (Input.GetKeyDown("e") && canDash)
        {
            rb2d.velocity = new Vector2((direction * dashModifier), rb2d.velocity.y);
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
        //rb2d.velocity = Vector2.ClampMagnitude(new Vector2(0f, rb2d.velocity.magnitude), maxVelocity);
        if(rb2d.velocity.y < 0f)
        {
            rb2d.gravityScale = 3;
        }
        else
        {
            rb2d.gravityScale = 1;
        }
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

        //transform.Translate((Vector3.right * moveHorizontal) * Time.deltaTime * speed);
        rb2d.AddForce((Vector3.right * moveHorizontal) * Time.deltaTime * speed * 100f);


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

        //WallJump
        dist = .55f;
        RaycastHit2D hitLWJ = Physics2D.Raycast(transform.position, -Vector2.right, dist, wallJumpLayerMask);
        RaycastHit2D hitRWJ = Physics2D.Raycast(transform.position, Vector2.right, dist, wallJumpLayerMask);
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + dist, transform.position.y, transform.position.z), Color.green);
        Debug.DrawLine(transform.position, new Vector3(transform.position.x - dist, transform.position.y, transform.position.z), Color.green);
        //Debug.Log(hitLWJ.collider.gameObject);
        if(hitLWJ)
        {
            touchingLeft = true;
        }
        else
        {
            touchingLeft = false;
        }

        if (hitRWJ)
        {
            touchingRight = true;
        }
        else
        {
            touchingRight = false;
        }
    }

    void ThrowGrenade()
    {
        // Create the Bullet from the Bullet Prefab

        if (!(GameObject.FindGameObjectsWithTag("VaporTeleport").Length > 0))
        {
            Vector3 directionalPosition = new Vector3(bulletSpawn.position.x + direction,
                                                      bulletSpawn.position.y,
                                                      bulletSpawn.position.z);
            var bullet = (GameObject)Instantiate(
            bulletPrefab,
            directionalPosition,
            bulletSpawn.rotation);
            AudioSource.PlayClipAtPoint(throwSound, this.transform.position);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(6 * direction, 6);

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
        
        if ((col.gameObject.tag == "Jumpable" && rb2d.velocity.y == 0))
        {
            JumpCount = 0;
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
        if (col.gameObject.tag == "Grenade")
        {
            displayTime = 5.0f;
            displayMessage = true;
            currentMessage = teleportMessage;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 1.0f);
    }

    IEnumerator Jump()
    {
        if(touchingRight || touchingLeft)
        {
            ani.SetTrigger("WallJump");
        }
        else
        {
            ani.SetTrigger("Jump");
        }
        yield return new WaitForSeconds(JumpDelay);
        if (touchingRight)
        {
            Debug.Log("WallJump Right");
            //WallJump Right
            Vector3 up = new Vector3(-.3f,1f,0f);
            rb2d.AddForce(up * jumpForce*1.3f, ForceMode2D.Impulse);
            JumpCount++;
        }
        else if(touchingLeft)
        {
            Debug.Log("WallJump Left");
            //WallJump Left
            Vector3 up = new Vector3(.3f, 1f, 0f);
            rb2d.AddForce(up * jumpForce * 1.3f, ForceMode2D.Impulse);
            JumpCount++;
        }
        else
        {
            //Regular
            Vector3 up = transform.TransformDirection(Vector3.up);
            rb2d.AddForce(up * jumpForce, ForceMode2D.Impulse);
            JumpCount++;
        }
    }
}
*/

/*
 * using System.Collections;
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
public GameObject bulletPrefab;
public Transform bulletSpawn;
public GUIStyle guiStyle;
public Font guiFont;
public int fontSize;
private float moveHorizontal;
private float direction = 1;
private float timeKeeper;
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
public string currentMessage;
//private GameComponent player;

public Animator ani;
public GameObject pc_GFX;
[Range(0, 2)]
public float JumpDelay;

// Start is called before the first frame update
void Start()
{
    rb2d = GetComponent<Rigidbody2D>();
    guiStyle.font = guiFont;
    guiStyle.fontSize = fontSize;
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

    if (col.gameObject.tag == "Jumpable" || (canWalljump && col.gameObject.tag == "Wall"))
    {
        JumpCount = 0;
    }
    if (col.gameObject.tag == "Wall")
    {
        rb2d.gravityScale = 0.8f;
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f);
        }
    }
    else
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
    if (col.gameObject.tag == "Grenade")
    {
        displayTime = 5.0f;
        displayMessage = true;
        currentMessage = teleportMessage;
    }
}

IEnumerator Jump()
{
    ani.SetTrigger("Jump");
    yield return new WaitForSeconds(JumpDelay);
    Vector3 up = transform.TransformDirection(Vector3.up);
    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
    JumpCount++;
}
}*/
