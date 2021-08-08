using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    private float arrowSpeed = 50f;

    void Start() {
        StartCoroutine(DisableArrow(5f));
    }
    void Update() {
        Move();
    }
    public void Move() {
        Vector3 temp = transform.position;
        temp.x += arrowSpeed * Time.deltaTime;
        transform.position = temp;

    }

    public float Speed {
        get {
            return arrowSpeed;
        }
        set{
            arrowSpeed = value;
        }
    }

    void OnTriggerEnter2D(Collider2D target) {
		if (target.gameObject.tag == MyTags.BAT_TAG) {
            gameObject.SetActive (false);

		}
        if(target.gameObject.tag == MyTags.GROUND_TAG) {
            gameObject.SetActive (false);

        }
	}
    IEnumerator DisableArrow(float timer) {
        yield return new WaitForSeconds (timer);
        gameObject.SetActive(false);
    }

}
