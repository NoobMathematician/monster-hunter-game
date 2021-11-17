# monster-hunter-game
My game project

## About this project:
+ You can check out the game demo here:
[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/W2hMtJkdQ08/0.jpg)](https://www.youtube.com/watch?v=W2hMtJkdQ08)

### 1. Make the Player movement

I implemented Player movement with Rigidbody. The basic forward and backward movement code is:

```
playerBody.velocity = new Vector2 (speed, playerBody.velocity.y);
```

For the Player to jump, firstly he must be on the ground (*isGrounded*) which is checked by *Physics2D.Raycast* vector2 down from the player position. Then, the basic code of player jump when *Space* key is pressed is:
```
void PlayerJump() { 
        if(isGrounded) {
            if(Input.GetKey(KeyCode.Space)) {
                playerBody.velocity = Vector2.up * jump_Velocity;                
            }
        }
}
```
I noted that when the Player is carrying the Bow, he should move slower than he does without any weapon. So I used two float speeds for the Player: *speedWithoutBow* and *speedWithBow*. His speeds *withBow* and "*!withBow* are coded accordingly. Also, a function called *ChangeDirection(float direction)* is used so his face and body is in the same direction of his movement.


### 2. Make the Player shoot
When the Player is carrying the Bow (*withBow*), he should be able to shoot an arrow each time the key C is pressed. So, here is my code:

```
    void PlayerAttack() {
        if(Input.GetKeyDown(KeyCode.C)) {
            if(withBow) {
                playerAnim.Play(MyTags.PLAYER_BOW_ATTACK);
                GameObject arrow = Instantiate(Arrow, ShootPoint.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().Speed *= transform.localScale.x;
            }
        } 
    }
```
Here I give the Arrow game object a *Tag*, which is important to dectect collision later on.

### 3. Make the first enemy: the small bat

#### 3.1 Let the small bat automatically chasing the Player

This small bat is really interesting. It can automatically locate and fly to the Player and do damage. 

After attaching the *Pathfinder* script to the game object *FindPath* and the *Seeker* script to the game object *BabyBat* (the small bat), we can use a very powerful namespace *Pathfinding*  which contains classes *seeker* and *Path*.

```
using Pathfinding;
```
```
Seeker seeker;
```
```
Path batPath
```
Firstly, let's make a batPath from the bat to the Player using this code, with starting point is the bat position (*batBody.position*), the ending point is the Player position (*target.position*) and PathComplete is a callback which will be called when the path has completed :

```
seeker.StartPath(batBody.position, target.position, PathComplete);
```
Secondly, we make the bat follows the path (move from the current wavepoint to the next wavepoint until the end of the path):

```
    void FixedUpdate() {
       ............
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
 ```
Finally, we make the path updated so it always follows the Player (using *InvokeRepeating()*) :

```
void Start() {
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
void UpdatePath(){
        if (seeker.IsDone()) {
            seeker.StartPath(batBody.position, target.position, PathComplete);
        }
    }
```
If we use the full *AstarPathfindingProject* package, we can have the same result (the bat automatically chasing the Player) by only using some short lines of code. However, I follow this instruction to make my own AI as it helps me understand the process fully.

#### 3.2 Let the small bat die when it is hit by the arrow

I use OnTriggerEnter2D here. Also, the bat should disappear after 1 second of his death (using *StartCoroutine()*). Example code:

```
void OnTriggerEnter2D(Collider2D target) {
        if(target.tag == MyTags.ARROW_TAG) {
            if(tag == MyTags.BAT_TAG) {
                batAnim.Play("Bat_Die");
                batBody.velocity = new Vector2(0, -5f);
                canMove = false;
                StartCoroutine (Dead(1f));
            }
        }
    }
```
### 4. Make the second enemy: the giant bat

The idea is that a giant bat will fly on the sky from point A to point B. When it sees the Player below it, it will drop some stones (the Player will take damage if he is hit by the stone).

For the giant bat to fly from point A to point B, firstly I initiate the virtual points A and B:
```
void Start () {
		originPosition = transform.position;
		originPosition.x += 4.5f;

		endPosition = transform.position;
		endPosition.x -= 4.5f;
	}
```
Then, the giant bat needs to move between those two points, with *ChangeDirection()* is used to make sure the bat face and body are in the same direction with its movement. Example code:

```
void BatFly() {
		if (canMove) {
			transform.Translate (moveDirection * speed * Time.smoothDeltaTime);
			if (transform.position.x >= originPosition.x) {
				moveDirection = Vector3.left;
				ChangeDirection ();
			} else if (transform.position.x <= endPosition.x) {
				moveDirection = Vector3.right; 
				ChangeDirection ();
			}
		}
}

```
Finally, when the giant bat detects the Player below it (using *Physics2D.Raycast*), it will drop the stone:

```
void DropBomb() {
		  if (Physics2D.Raycast (transform.position, Vector2.down, Mathf.Infinity, playerLayer)) {
				Instantiate (batBomd, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
		}
	}
```
I found one problem with this *DropBomd()* method: the giant bat constantly drop too many bombs (stones) each time it attacks the Player. I want it to drop just a few stones each time. So, I use the boolean *isAttacking* to make sure that the it will only drop the stone when *!isAttacking*. Then, I limited the frequency of *!isAttacking* to 1 time for every 0.5 seconds:
```
void Start () {
   InvokeRepeating("isAttackingtoFalse", 1f, 0.5f);
}
	
Void isAttackingtoFalse() {
   isAttacking = false;
}
```
By doing this, the giant bat only drops 2 stones each time it detects the Player, instead of tens of stones as before.

Similar to the small bat, the giant bat dies when it is hit by the arrow.

### 5. Make the third enemy: the spider

For the spider, my idea is that it will move along the ground until it does not detect collision with the ground. This should be done by using *Physics2D.Raycast* Vector2.down to the ground. 

If the spider hits the Player from the right or the left, it will damage the Player. This also should be done by using *Physics2D.Raycast* Vector2.left and Vector2.right respectively searching for the playerLayer LayerMask.

However, I want to make one thing new here: if the Player jumps on the spider, the spider will die and the Player jumps back to the air. And this is my solution:

```
void CheckCollision() {
  Collider2D topHit = Physics2D.OverlapCircle (top_Collision.position, 0.2f, playerLayer);
    if (topHit != null) {
			if (topHit.gameObject.tag == MyTags.PLAYER_TAG) {
					topHit.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (topHit.gameObject.GetComponent<Rigidbody2D>().velocity.x, 7f);
					spiderBody.velocity = new Vector2 (0, 0);
					spiderAnim.Play ("SpiderDie");
          StartCoroutine(Dead(0.2f));
			}
		}
}
```
### 6. Make the Player take damage

Firstly, I give the Player three lives. Each time the *DealDamage()* method is call (he takes damage from the enemy), his *lifeCount* will be reduced my one. WHen his *lifeCount* equals to 0, the game will be restarted:

```
public void DealDamage() {
  if(canDamage) {
    lifeCount --;
    if(lifeCount >= 0) {
      lifeText.text = "x" + lifeCount;
    }
     if(lifeCount == 0) {
        StartCoroutine(TimeScale());
        StartCoroutine(RestartGame());
     }
     canDamage = false;
       StartCoroutine(WaitForDamage());
   } 
```
This is list of events when he takes damage: 
+ When he is touch by the small bat
+ When he is hit by the stone (from the giant bat)
+ When he is left-hit or right-hit by the spider

In each of those events, the method *DealDamage()* should be called. This is an example, when the stone hit the Player:

```
void OnCollisionEnter2D(Collision2D target) {
	if (target.gameObject.tag == MyTags.PLAYER_TAG) {
             target.gameObject.GetComponent<PlayerDamage>().DealDamage();
	}
}

```
### 7. Create game levels, camera follow script, basic UI and main menu

Those are the last parts I did and I simply follow the instructions from instructors on Youtube. While following their instruction, I always make sure that I understand the process.

## Conclusion

Making a game from scratch is fun.
