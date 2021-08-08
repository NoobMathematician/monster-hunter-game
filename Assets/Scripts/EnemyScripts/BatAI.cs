using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class BatAI : MonoBehaviour {
    public Transform target;
    private bool canMove;
    public float speed = 250f;
    public float nextWayPointDistance = 10f;

    public LayerMask playerLayer;
    Path batPath;
    int currentWayPoint = 0;
    bool endOfPath;

    Seeker seeker;
    private Rigidbody2D batBody;
    private Animator batAnim;

    private GameObject player;

    void Awake(){
        batBody = GetComponent<Rigidbody2D>();
        batAnim = GetComponentInChildren<Animator>();
    }
    void Start() {
        canMove = true;
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        player = GameObject.FindGameObjectWithTag(MyTags.PLAYER_TAG);

    }

    void UpdatePath(){
        if (seeker.IsDone()) {
            seeker.StartPath(batBody.position, target.position, PathComplete);
        }
    }

    void PathComplete( Path p) {
        if(!p.error) {
            batPath = p;
            currentWayPoint = 0;
        }
    }
    void FixedUpdate() {
        if(batPath == null) {
            return;
        }
        if(currentWayPoint >= batPath.vectorPath.Count) {
            endOfPath = true;
            return;
        } else {
            endOfPath = false;
        }
        Vector2 batDirection = ((Vector2)batPath.vectorPath[currentWayPoint] - batBody.position).normalized;

        Vector2 force = batDirection * speed * Time.deltaTime;

        if(canMove) {
            batBody.AddForce(force);

        }

        float distance = Vector2.Distance(batBody.position, batPath.vectorPath[currentWayPoint]);

        if(distance < nextWayPointDistance) {
            currentWayPoint ++;
        }

    }
    void Update() {

        if(canMove) {
            if(batBody.velocity.x >= 0.01f) {
           transform.localScale = new Vector3(0.5f, 0.6f, 1f); 
            }else if(batBody.velocity.x <= -0.01f) {
           transform.localScale = new Vector3(-0.5f, 0.6f, 1f); 
            }
        }
        
        if(Physics2D.OverlapCircle(transform.position, 0.5f, playerLayer)) {
            player.GetComponent<PlayerDamage>().DealDamage();
        }
        
    }

    IEnumerator Dead(float timer) {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D target) {
        if(target.tag == MyTags.ARROW_TAG || target.tag ==MyTags.SWORD_TAG) {
            if(tag == MyTags.BAT_TAG) {

                batAnim.Play("Bat_Die");
                batBody.velocity = new Vector2(0, -5f);
                canMove = false;
                StartCoroutine (Dead(1f));

                ScoreScripts.scoreValue += 10;
            }
        }

    }



}
