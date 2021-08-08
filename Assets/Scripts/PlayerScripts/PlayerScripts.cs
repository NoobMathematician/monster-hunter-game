
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour {

    public Transform swordAttackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public float speed_NoWeapon = 8f;
    public float speed_WithWeapon = 5f;
    public float jump_Velocity = 15f;
    public bool withWeapon;
    public bool withBow;
    public bool withSword;
    public bool noWeapon;
    public bool isGrounded;
    public bool isJumping;
    public string currentAnimation;

    public Transform playerPosition;
    public LayerMask groundLayer;

    private Rigidbody2D playerBody;
    private Animator playerAnim;
    public GameObject Arrow;
    public Transform ShootPoint;

    void Awake() {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnim =GetComponent<Animator>();
        currentAnimation = MyTags.PLAYER_NOWEAPON_IDLE;
    }
    void Start(){
        withWeapon = false;
        withBow = false;
        withSword = false;
        noWeapon = true;
        isJumping = false;
    }
    void Update(){
        CheckIfGrounded();
        PlayerJump();
        ChangeWeapon();
        PlayerAttack();
    }

    void FixedUpdate(){
        PlayerWalk();
    }
    void PlayerWalk() {
        float h = Input.GetAxisRaw("Horizontal");
        
            if(h > 0) {
                playerBody.velocity = new Vector2 (speed_NoWeapon, playerBody.velocity.y);
                ChangeDirection(0.65f); 
            } else if(h < 0) {
                    playerBody.velocity = new Vector2 (-speed_NoWeapon, playerBody.velocity.y);
                ChangeDirection(-0.65f);

            } else { 
                playerBody.velocity = new Vector2(0f, playerBody.velocity.y);
            }
            playerAnim.SetInteger("Speed", Mathf.Abs((int)playerBody.velocity.x));
        
    }

    void ChangeDirection(float direction) {
        Vector3 temp = transform.localScale;
        temp.x = direction;
        transform.localScale = temp;

    }
    void CheckIfGrounded() {
        isGrounded = Physics2D.Raycast(playerPosition.position, Vector2.down, 0.1f, groundLayer);

        if(isGrounded) {
            if(isJumping) {
                isJumping = false;
            }
            if(noWeapon) {
                playerAnim.SetBool("Player_NoWeapon_Jump", false);                
            } 
            if(withBow) {
                playerAnim.SetBool("Player_Bow_Jump", false);                
            } 
            if(withSword){
                playerAnim.SetBool("Player_Sword_Jump", false);                
            }        
        }
    }
    
    void PlayerJump() { 
        if(isGrounded) {
            if(Input.GetKey(KeyCode.Space)) {
                isJumping = true;
                playerBody.velocity = Vector2.up * jump_Velocity;
                
                if(noWeapon) {
                    playerAnim.SetBool("Player_NoWeapon_Jump", true);                
                }
                if (withBow) {
                    playerAnim.SetBool("Player_Bow_Jump", true);                
                }
                if(withSword){
                    playerAnim.SetBool("Player_Sword_Jump", true);                
                }
            }
        }
    }
    void ChangeWeapon() {
        if(Input.GetKeyDown(KeyCode.F)) {
            withWeapon = true;
            withBow = false;
            withSword = true;
            noWeapon = false;
            ChangeAnimation(MyTags.PLAYER_SWORD_IDLE);

        } 
        if(Input.GetKeyDown(KeyCode.G)) {
            withWeapon = true;
            withBow = true;
            withSword = false;
            noWeapon = false;
            ChangeAnimation(MyTags.PLAYER_BOW_IDLE);
        }
        if(Input.GetKeyDown(KeyCode.H)) {
            withWeapon = false;
            withBow = false;
            withSword = false;
            noWeapon = true;
            ChangeAnimation(MyTags.PLAYER_NOWEAPON_IDLE);
        }
            
    }
    void ChangeAnimation(string newAnimation) {
        if(currentAnimation == newAnimation) return;
        playerAnim.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    void PlayerAttack() {
        if(Input.GetKeyDown(KeyCode.C)) {
            if(withBow) {
                playerAnim.Play(MyTags.PLAYER_BOW_ATTACK);
                GameObject arrow = Instantiate(Arrow, ShootPoint.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().Speed *= transform.localScale.x;
            }
            if(withSword) {
                playerAnim.Play(MyTags.PLAYER_SWORD_ATTACK2);
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordAttackPoint.position, attackRange, enemyLayer);

                foreach(Collider2D enemy in hitEnemies) {
                    enemy.GetComponentInChildren<Animator>().Play("Bat_Die");
                    enemy.GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0, -5f);
                    
                    enemy.GetComponentInParent<Rigidbody2D>().gameObject.SetActive(false);
                }

            }
        } 
    }
    
        // IEnumerator Dead(float timer) {
        // yield return new WaitForSeconds(timer);
        // gameObject.SetActive(false);
        // }











}

