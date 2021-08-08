using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {
    public float spiderSpeed = 3f;
    private Rigidbody2D spiderBody;
    private Animator spiderAnim;

    public LayerMask GroundLayer;
    public LayerMask playerLayer;

    private bool moveRight;
    private bool canMove;

    public Transform down_Collision, top_Collision, left_Collision, right_Collision;
    
    void Awake(){
        spiderBody = GetComponent<Rigidbody2D>();
        spiderAnim = GetComponent<Animator>();
    }
    void Start(){
        moveRight = true;
        canMove = true;
    }
    void Update(){
        if(canMove) {
            if(moveRight) {
            spiderBody.velocity = new Vector2(spiderSpeed, spiderBody.velocity.y);
            } else {
            spiderBody.velocity = new Vector2(-spiderSpeed, spiderBody.velocity.y);
            }
        }
        
        CheckCollision();
    }

    void CheckCollision() {

		RaycastHit2D leftCollision = Physics2D.Raycast (left_Collision.position, Vector2.left, 0.1f, playerLayer);

		RaycastHit2D rightCollision = Physics2D.Raycast (right_Collision.position, Vector2.right, 0.1f, playerLayer);

		Collider2D topHit = Physics2D.OverlapCircle (top_Collision.position, 0.2f, playerLayer);


        if (topHit != null) {
			if (topHit.gameObject.tag == MyTags.PLAYER_TAG) {
					topHit.gameObject.GetComponent<Rigidbody2D> ().velocity =
						new Vector2 (topHit.gameObject.GetComponent<Rigidbody2D>().velocity.x, 7f);

					canMove = false;
					spiderBody.velocity = new Vector2 (0, 0);

					spiderAnim.Play ("SpiderDie");

                    ScoreScripts.scoreValue += 1;


                    StartCoroutine(Dead(0.2f));
				
			}
		}

        if(leftCollision) {
            if(leftCollision.collider.gameObject.tag == MyTags.PLAYER_TAG) {
                leftCollision.collider.gameObject.GetComponent<PlayerDamage>().DealDamage();

            }
        }
        if(rightCollision) {
            if(rightCollision.collider.gameObject.tag == MyTags.PLAYER_TAG) {
                rightCollision.collider.gameObject.GetComponent<PlayerDamage>().DealDamage();

            }
        }

        if(!Physics2D.Raycast(down_Collision.position, Vector2.down, 0.1f, GroundLayer)) {
            ChangeDirection();
        }
    }

    void ChangeDirection() {

        moveRight = !moveRight;

        Vector3 tempScale = transform.localScale;

		if (moveRight) {
			tempScale.x = Mathf.Abs (tempScale.x);

		} else {
			tempScale.x = -Mathf.Abs (tempScale.x);

		}

		transform.localScale = tempScale;
    }

    void OnTriggerEnter2D(Collider2D target) {
        if(target.tag == MyTags.ARROW_TAG || target.tag ==MyTags.SWORD_TAG) {
            if(tag == MyTags.SPIDER_TAG) {

                spiderAnim.Play("SpiderDie");
                spiderBody.velocity = new Vector2(0, -5f);
                canMove = false;
                StartCoroutine (Dead(1f));

            }
        }
    }
    IEnumerator Dead(float timer) {
		yield return new WaitForSeconds (timer);
		gameObject.SetActive (false);
	}


}
