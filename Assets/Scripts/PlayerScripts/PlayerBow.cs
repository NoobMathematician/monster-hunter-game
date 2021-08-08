using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : PlayerScripts {

    public GameObject Arrow;

    private Rigidbody2D playerBody1;
    private Animator playerAnim1;
    public Transform ShootPoint;
    void Awake() {
        playerBody1 = GetComponent<Rigidbody2D>();
        playerAnim1 =GetComponent<Animator>();
    }

    void Update(){
        PlayerAttack();
        ChangeWeapon1();
    }

    void PlayerAttack() {
        if(Input.GetKeyDown(KeyCode.C)) {
            if(withBow) {
                playerAnim1.Play(MyTags.PLAYER_BOW_ATTACK);
                GameObject arrow = Instantiate(Arrow, ShootPoint.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().Speed *= transform.localScale.x;
            }
            if(withSword) {
                playerAnim1.Play(MyTags.PLAYER_SWORD_ATTACK2);
                    // DAMAGE
            }
        } 
    }
    void ChangeWeapon1() {
        if(Input.GetKeyDown(KeyCode.F)) {
            withWeapon = true;
            withBow = false;
            withSword = true;
            noWeapon = false;

        } 
        if(Input.GetKeyDown(KeyCode.G)) {
            withWeapon = true;
            withBow = true;
            withSword = false;
            noWeapon = false;
        }
        if(Input.GetKeyDown(KeyCode.H)) {
            withWeapon = false;
            withBow = false;
            withSword = false;
            noWeapon = true;
        }
            
    }
    

}
