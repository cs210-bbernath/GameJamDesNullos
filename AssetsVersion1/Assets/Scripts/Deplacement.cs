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
    private bool facingleft = false;
    public bool facingright = false;
    public bool facingUp = false;
    private bool canDash = true;
    private float facingUpTimer = 0;
    public float upTime = 0.2f;
    public bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.6f;
    public float dashingCooldown = 1f;

    public bool hasTheBall = false;

    public GameObject ball;
    public GameObject otherPlayer;

    public Rigidbody2D RB;
    public Collision2D collision;
    [SerializeField] private TrailRenderer tr;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //OnCollisionEnter2D(collision);
        if (isDashing) {
            return;
        }


        if (player == 1) {
            Modularity("w", "a", "d");
        }
        else {
            Modularity("up", "left", "right");
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
        facingleft = false;
        facingright = false;
        
        
    }

    private void Modularity(string keyup, string keyleft, string keyright) {
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
    }

    public void getTheBall(){
        hasTheBall = !hasTheBall;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Character")){
            if (isDashing){
                getTheBall();
                //otherPlayer.getTheBall();
            }
            //if one of them (at least) is dashing then
                // change the hasTheBall to NOT-hasTheBall
                // samething for the otherplayer
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Ball"))
        {
            getTheBall();
            Destroy(ball);
        }
        if (collision.gameObject.CompareTag("Character")){
            //getTheBall();
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
