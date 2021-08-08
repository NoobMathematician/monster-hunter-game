using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
private float speed = 3f;

    private Rigidbody2D elevatorBody;

	private Animator elevatorAnim;

	private Vector3 moveDirection = Vector3.up;
	private Vector3 originPosition;
	private Vector3 endPosition;


	private bool canMove;


	void Awake() {
		elevatorBody = GetComponent<Rigidbody2D> ();
		elevatorAnim = GetComponent<Animator> ();
	}

	void Start () {
		originPosition = transform.position;
		originPosition.y += 4f;

		endPosition = transform.position;
		endPosition.y -= 4f;
	}
	
	void FixedUpdate () {
		ElevatorMove ();
    }

	void ElevatorMove() {
		    transform.Translate (moveDirection * speed * Time.smoothDeltaTime);

			if (transform.position.y >= originPosition.y) {
				moveDirection = Vector3.down;


			} else if (transform.position.y <= endPosition.y) {
				moveDirection = Vector3.up;
			}

		
	}
    void ChangeDirection(float direction) {
		Vector3 tempScale = transform.localScale;
		tempScale.y = direction;
		transform.localScale = tempScale;
	}

}
