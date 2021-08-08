using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour {
    
    private Text lifeText;
    private int lifeCount;

    private bool canDamage;

    void Awake() {
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        lifeCount = 1;
        lifeText.text = "x" + lifeCount;

        canDamage = true;

    }

    void Start() {
        Time.timeScale = 1f;
    }
    public void DealDamage() {
        if(canDamage) {
            lifeCount --;

            if(lifeCount >= 0) {
            lifeText.text = "x" + lifeCount;
            }

            if(lifeCount == 0) {
                Time.timeScale = 0f;
                StartCoroutine(RestartGame());
            }

            canDamage = false;
            StartCoroutine(WaitForDamage());
        }
        
    }

    IEnumerator RestartGame() {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene("GamePlay");
    }

    IEnumerator WaitForDamage() {
        yield return new WaitForSeconds(2f);
        canDamage = true;
    }


}
