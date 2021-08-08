using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScale : MonoBehaviour {
   void Start () {
		SpriteRenderer spiteRenderer = GetComponent<SpriteRenderer> ();	

		transform.localScale = new Vector3 (1, 1, 1);

		float width = spiteRenderer.sprite.bounds.size.x;
		float height = spiteRenderer.sprite.bounds.size.y;

		float worldScreenHeight = Camera.main.orthographicSize * 2f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		Vector3 tempScale = transform.localScale;
		tempScale.x = worldScreenWidth / width + 0.1f;
		tempScale.y = worldScreenHeight / height + 0.1f;

		transform.localScale = tempScale;

	}
	
   



}
