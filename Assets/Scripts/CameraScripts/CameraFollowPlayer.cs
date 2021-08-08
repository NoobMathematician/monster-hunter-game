using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour{
    public float cameraSpeed = 0.3f;
    public float resetSpeed = 0.5f;
	public Bounds cameraBounds;
	private Transform player;
	private float offsetZ;
    private bool followingPlayer;
	private Vector3 lastPlayerPosition;
	private Vector3 currentVelocity;


	void Awake() {
		BoxCollider2D collider2D = GetComponent<BoxCollider2D> ();
		collider2D.size = new Vector2 (Camera.main.aspect * 2f * Camera.main.orthographicSize, 15f);
		cameraBounds = collider2D.bounds;
	}

	void Start () {
		player = GameObject.FindGameObjectWithTag (MyTags.PLAYER_TAG).transform;
		lastPlayerPosition = player.position;
		offsetZ = (transform.position - player.position).z;
		followingPlayer = true;
	}
	
	void FixedUpdate () {
		if (followingPlayer) {
			Vector3 aheadTargetPos = player.position + Vector3.forward * offsetZ;

			// if (aheadTargetPos.x >= transform.position.x) {
				Vector3 newCameraPosition = Vector3.SmoothDamp (transform.position, aheadTargetPos,
					ref currentVelocity, cameraSpeed);

				transform.position = new Vector3 (newCameraPosition.x, transform.position.y,
					newCameraPosition.z);

				lastPlayerPosition = player.position;
			// }
		}
	}

}
