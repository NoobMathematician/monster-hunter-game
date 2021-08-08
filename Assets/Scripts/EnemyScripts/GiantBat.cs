using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBat : MonoBehaviour {
	private float speed = 3f;

    private Rigidbody2D gBatBody;

	private Animator anim;

	private Vector3 moveDirection = Vector3.left;
	private Vector3 originPosition;
	private Vector3 endPosition;

	public GameObject batBomd;
	public LayerMask playerLayer;
	private bool isAttacking;

	private bool canMove;


	void Awake() {
		gBatBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void Start () {
		originPosition = transform.position;
		originPosition.x += 4.5f;

		endPosition = transform.position;
		endPosition.x -= 4.5f;

		canMove = true;

        InvokeRepeating("isAttackingtoFalse", 1f, 0.5f);
	}
	
    void isAttackingtoFalse() {
        isAttacking = false;
    }
	void Update () {
		BatFly ();
		DropBomb();
	}

	void BatFly() {
		if (canMove) {
			transform.Translate (moveDirection * speed * Time.smoothDeltaTime);

			if (transform.position.x >= originPosition.x) {
				moveDirection = Vector3.left;

				ChangeDirection (-1f);

			} else if (transform.position.x <= endPosition.x) {
				moveDirection = Vector3.right; 

				ChangeDirection (1f);

			}

		}
	}

	void ChangeDirection(float direction) {
		Vector3 tempScale = transform.localScale;
		tempScale.x = direction;
		transform.localScale = tempScale;
	}

	void DropBomb() {
		if (!isAttacking) {
			if (Physics2D.Raycast (transform.position, Vector2.down, Mathf.Infinity, playerLayer)) {
				Instantiate (batBomd, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
				isAttacking = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D target) {
		if (target.tag == MyTags.ARROW_TAG) {
			anim.Play ("GiantBat_Die");

			GetComponent<BoxCollider2D> ().isTrigger = true;
			gBatBody.bodyType = RigidbodyType2D.Dynamic;

			canMove = false;

			StartCoroutine (BatDead ());
			
			ScoreScripts.scoreValue += 10;


		}
	}
    IEnumerator BatDead() {
		yield return new WaitForSeconds (3f);
		gameObject.SetActive (false);
	}



}
