using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : MonoBehaviour
{
    public float jumpforce = 15;
    public float leftVelocity = 5;
    public float rightVelocity = 3;
    public int player = 1;
    public bool isGrounded = false;
    public bool facingleft = false;
    public bool facingright = false;
    public bool facingUp = false;
    private bool canDash = true;
    private float facingUpTimer = 0;
    public float upTime = 0.2f;
    public bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.6f;
    public float dashingCooldown = 1f;

    public float marging = 0.1F;

    // quand on prend la balle
    public float maxImmunity = 0.5f;
    private float immunityTimer;
    public bool immune = false;

    //private ParticleSystem particles;
    public ParticleSystem particles;

    public Color ballColor = Color.white;
    public SpriteRenderer spriteRenderer;
    private Color startingColor = Color.clear;

    public Collider2D triggerBall;

    public bool hasTheBall = false;
    
    // ANIMATOR
    public Animator animator;
  

    public GameObject ball;
    public Deplacement otherPlayer;

    public Rigidbody2D RB;
    public Collision2D collision;
    [SerializeField] private TrailRenderer tr;

    //descente de plateforme
    private bool goDown = false;
    private Collider2D ignoredCollision;
    public float ignoreTime = 0.2f;
    private bool isIgnoring = false;
    private float timerIgnore = 0;


    // Start is called before the first frame update
    void Start()
    {
        //otherPlayer = GameObject.FindGameObjectWithTag("Character").GetComponent<Deplacement>();
        if (player == 2){
            otherPlayer = GameObject.FindGameObjectWithTag("Character2").GetComponent<Deplacement>();
        } else if (player == 1){
            otherPlayer = GameObject.FindGameObjectWithTag("Character1").GetComponent<Deplacement>();
        }
        
        particles = GetComponentInChildren<ParticleSystem>();
        particles.Stop();

        //gameObject.GetComponent<ParticleSystem>().Stop();
        //particles.enabled = false;
        //particulesBall = this.gameObject.transform.GetChild(0).gameObject;
        //particulesBall.SetActive(false);

        immunityTimer = maxImmunity;
        
        startingColor = Color.red;//spriteRenderer.color;
        //RB = GetComponent<Rigidbody2D>();
        //RB.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    // Update is called once per frame
    void Update()
    {
        //OnCollisionEnter2D(collision);
        
        
        if (isDashing) {
            return;
        }


        if (player == 1) {
            Modularity("w", "a", "d", "s");
        }
        else {
            Modularity("up", "left", "right", "down");
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (facingUp){
            if (facingUpTimer < upTime){
            facingUpTimer += Time.deltaTime;
            }
            else{
                facingUp= false;
            }
        }
        //if (facingleft){
        animator.SetBool("isBraking", facingleft);
        	
        //} else {
        animator.SetBool("isGrounded", isGrounded);
        //}
        	
        facingleft = false;
        facingright = false;

        if (hasTheBall){
            //spriteRenderer.color = ballColor;
            //gameObject.GetComponent<ParticleSystem>().Play();
            //particles.Play();
            //particulesBall.SetActive(true);
        } else {
            //spriteRenderer.color = startingColor;
            //gameObject.GetComponent<ParticleSystem>().Stop();
            //particles.Stop();
            //particulesBall.SetActive(false);
        }

        if(immune){
            if(immunityTimer > 0){
                immunityTimer -= Time.deltaTime;
            } else {
                immunityTimer = maxImmunity;
                immune=false;
            }
        }

        if (isIgnoring){
            if(timerIgnore < ignoreTime){
                timerIgnore += Time.deltaTime;
            }
            else{
                timerIgnore = 0;
                isIgnoring = false;
                Physics2D.IgnoreCollision(RB.GetComponent<Collider2D>(), ignoredCollision, false);
            }
        }
        
        
    }

    private void Modularity(string keyup, string keyleft, string keyright, string keyDown) {
            if (Input.GetKeyDown(keyup)) {
                if (isGrounded) {
                    //RB.AddForce(Vector2.up * jumpforce);
                    //RB.velocity = Vector2.up * jumpforce;
                    //RB.velocity = new Vector2(RB.velocity.x, jumpforce);
                    RB.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                    isGrounded = false;
                    facingUp = true;
                    facingUpTimer = 0;
                }
            }
             if (Input.GetKey(keyleft)) {
                //if (isGrounded) {
                facingleft = true;
                facingright = false;
                //RB.velocity = Vector2.left * leftVelocity;
                RB.velocity = new Vector2(-leftVelocity,  RB.velocity.y);
                
            }
            if (Input.GetKey(keyright)) {
                //if (isGrounded) {
                facingleft = false;
                facingright = true;
                //}
                RB.velocity = new Vector2(rightVelocity,  RB.velocity.y);
            }

            if (Input.GetKey(keyDown)){
                if(isGrounded){
                    goDown = true;
                }
            } else {
                goDown = false;
            }
    }

    public void getTheBall(){
        hasTheBall = !hasTheBall;
        immune = true;
        if (hasTheBall){
        	Debug.Log("Got the Ball!");
        }
        else{
         	Debug.Log("Lost it ?");
         }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ground"))
        {
            if(goDown){
                Debug.Log("GOING DOWN");
                Physics2D.IgnoreCollision(RB.GetComponent<Collider2D>(), collision.collider);
                ignoredCollision = collision.collider;
                isIgnoring = true;
                isGrounded = false;
            }
            else{
                isGrounded = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Ball") && !immune)
        {
            Debug.Log("ball");
            Destroy(ball);
            getTheBall();
        }
        if (collision.gameObject.CompareTag("TriggerBall")){
            if (isDashing && !otherPlayer.immune){
                //Debug.Log("IN dashing");
                if (!hasTheBall && otherPlayer.hasTheBall){
                    getTheBall();
                    otherPlayer.getTheBall();
                }
            }
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = RB.gravityScale;
        RB.gravityScale = 0f;
        if (facingUp){
            if (facingleft){
                RB.velocity = new Vector2(-dashingPower,dashingPower);
            }
            else if (facingright){
                RB.velocity = new Vector2(dashingPower,dashingPower);
            }
            RB.velocity = Vector2.up * dashingPower;
        }
        else if (facingleft) {
            RB.velocity = Vector2.left * dashingPower;
        } else if (facingright){
            RB.velocity = Vector2.right * dashingPower;
        }
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        RB.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
