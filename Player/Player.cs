using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	[SerializeField] public float speed;
	[SerializeField] private GameObject bullet;
	[SerializeField] private GameObject bulletLeft;
	[SerializeField] private GameObject bulletRed;
	[SerializeField] private GameObject bulletLeftRed;
	[SerializeField] private GameObject grenade;
	[SerializeField] SpriteRenderer yellowMuzzleR;
	[SerializeField] SpriteRenderer yellowMuzzleL;
	[SerializeField] public Text ammo;
	[SerializeField] public Text grenadesLeft;
	[SerializeField] public Text livesLeft;
	[SerializeField] public int health = 3;
	[SerializeField] public Text clips;
	[SerializeField] private GameObject droid;
	[SerializeField] public Text droidCount;
	[SerializeField] private GameObject reloadText;
	[SerializeField] private int ammoCount;
	[SerializeField] private int totalClips;
	[SerializeField] private int grenades;
	[SerializeField] private int totalDroids;
	[SerializeField] private GameObject explode;

	[SerializeField] public AudioClip machineGun;
	[SerializeField] public AudioClip reload;
	[SerializeField] public AudioClip playerDeath;
	[SerializeField] public AudioClip playerHurt;
	[SerializeField] public AudioClip playerSword;
	[SerializeField] public AudioClip repairMech;
	[SerializeField] public AudioClip grenadeItem;
	[SerializeField] public AudioClip droidItem;
	[SerializeField] public AudioClip wrenchItem;
	[SerializeField] public AudioClip lifeItem;
	[SerializeField] public AudioClip speedBoostItem;
	[SerializeField] public AudioClip shotgunSpecialShooting;
	[SerializeField] public AudioClip shotgunSpecialItem;
	[SerializeField] public AudioClip ammoItem;
	[SerializeField] public AudioClip ammoEmpty;
	
	[SerializeField] private SoundManager soundMng;
	private SpriteRenderer sprite;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rigid;
	private Animator anim;

	private int grenadeCount;
	private bool shotGunSpecial = false;
	public bool facingRight = true;
	private bool isRunning = false;
	private bool immortal = false;
	private float immortalTime = 3.0f;
	private bool isDead = false;
	private bool isReloading = false;
	private bool isAbleToShoot = false;

	void Start () {

		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();
		ammo.text = "" + ammoCount;
		reloadText.SetActive(false);
		grenadeCount = grenades;
		AmmoOnScreen();
		DroidsOnScreen();
		GrenadesOnScreen();

	}
	
	void FixedUpdate () {
		if (!isDead && GameManager.Instance.PlayerIsAlive && !isReloading){
			Movement();
			if (rigid.velocity.magnitude > 0){
				isRunning = true;
			}
			else {
				isRunning = false;
			}
		}
		else if (isDead || isReloading){
			// transform.position = new Vector3(21.09f, -2.8f, 30);
			Move(0f, 0f);
			anim.SetBool("Attack", false);
			anim.SetBool("Shoot", false);
			anim.SetBool("RunShoot", false);
			anim.SetBool("Grenade", false);
			yellowMuzzleR.enabled = false;	
			yellowMuzzleL.enabled = false;
			rigid.Sleep();
		}
		if (!GameManager.Instance.PlayerIsAlive){
			// Death();
		}
		if (ammoCount <= 0){
			isAbleToShoot = false;
		}
		if(ammoCount >= 1){
			isAbleToShoot = true;
		}
	}

	//_____________________________________________Movement

	public void Movement(){
		float move = Input.GetAxisRaw("Horizontal");
		float up = Input.GetAxisRaw("Vertical");
		Flip(move);

		//boundaries for player
		if (transform.position.y >= -2.8f){
			transform.position = new Vector3 (transform.position.x, -2.8f, 30);
		}
		if (transform.position.y <= -6.00f){
			transform.position = new Vector3 (transform.position.x, -6.00f, 30);
		}
		if (transform.position.x >= 169.98f){
			transform.position = new Vector3 (169.98f, transform.position.y, 30);
		}
		if (transform.position.x <= 3.5f){
			transform.position = new Vector3 ( 3.5f, transform.position.y, 30);
		}
		rigid.velocity = new Vector2(move * speed, up * speed);
		Move(move, up);

		if (Input.GetMouseButton(0)){
			if (ammoCount >= 1 && isAbleToShoot && !GameManager.Instance.WaveMenu){
				if (isRunning){
					anim.Play("RunShoot");
					anim.SetBool("RunShoot", true);
					anim.SetBool("Shoot", false);
				}
				else if (!isRunning) {
					anim.SetBool("Shoot", true);
					anim.SetBool("RunShoot", false);
					anim.Play("Shoot");
				}
			YellowGunShotAnimation();
			}
			else {
				soundMng.PlaySfx(ammoEmpty);
			}
		}
		else if (Input.GetMouseButtonUp(0)){
			anim.SetBool("RunShoot", false);	
			anim.SetBool("Shoot", false);
			yellowMuzzleR.enabled = false;	
			yellowMuzzleL.enabled = false;	
		}
		else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftShift)){
			Attack();
			soundMng.PlaySfx(playerSword);
		}
		else if (Input.GetKeyDown("f") && GameManager.Instance.WaveOver){
			if (grenades >= 1){
				ThrowGrenade();
				soundMng.PlaySfx(grenadeItem);
			}
		}
		else if(Input.GetKeyDown("r") && totalClips >= 1 && !isReloading && ammoCount <  15){
			anim.SetTrigger("Reload");
			totalClips -= 1;
			ammoCount = 30;
			isReloading = true;
			reloadText.SetActive(false);
			AmmoOnScreen();
			StartCoroutine(TurnReloadFalse());
			soundMng.PlaySfx(reload);
			
		}
		else if(Input.GetKeyDown("space") && totalDroids >= 1 && GameManager.Instance.WaveOver){
			Instantiate(droid,transform.position, Quaternion.identity);
			anim.SetTrigger("Reload");
			totalDroids --;
			GameManager.Instance.DroidIsSpawned = true;
			soundMng.PlaySfx(droidItem);
			DroidsOnScreen();
		}
		
		else{
			anim.SetBool("Attack", false);
			anim.SetBool("Shoot", false);
			anim.SetBool("RunShoot", false);
			anim.SetBool("Grenade", false);
			yellowMuzzleL.enabled = false;
			yellowMuzzleR.enabled = false;
		}
	}

	public void Move(float move, float up){
		anim.SetFloat("Move", Mathf.Abs(move + up));
	}

	void Flip(float move){
		if(move > 0){
			sprite.flipX = false;
			facingRight = true;
		}
		else if(move < 0){
			sprite.flipX = true;
			facingRight = false;
		}
	}

	//______________________________________________Bullet Instantiate

	void CreateBullet(){
		if (isAbleToShoot){
			if (!shotGunSpecial){
				if (facingRight){
					Instantiate(bullet, transform.position, Quaternion.identity);
				}
				else {
					Instantiate(bulletLeft, transform.position, Quaternion.identity);
				}
				soundMng.PlaySfx(machineGun);
			}
			else if(shotGunSpecial){
				if (facingRight){
					Instantiate(bulletRed, transform.position, Quaternion.identity);
				}
				else if(!facingRight){
					Instantiate(bulletLeftRed, transform.position, Quaternion.identity);
				}
				soundMng.PlaySfx(shotgunSpecialShooting);
			}
			if (GameManager.Instance.WaveOver){
				ammoCount -= 1;
				AmmoOnScreen();
			}
			else {
				
			}
		}
	}

	//__________________________________________Attack functions

	void Attack(){
		rigid.Sleep();
		StartCoroutine(WakeUpRigidBody());
		anim.SetBool("Attack",true);
	}

	void Shoot(){
		anim.SetBool("Shoot", true);	
	}

	void YellowGunShotAnimation(){
		if (ammoCount >= 1){
			if (sprite.flipX){
				yellowMuzzleL.enabled = true;
				yellowMuzzleR.enabled = false;
			}
			else if (!sprite.flipX) {
				yellowMuzzleL.enabled = false;
				yellowMuzzleR.enabled = true;	
			}	
		}
		else {
			yellowMuzzleL.enabled = false;
			yellowMuzzleR.enabled = false;
		}
	}

	IEnumerator WakeUpRigidBody(){
		yield return new WaitForSeconds(.5f);
		rigid.WakeUp();
	}

	private void AttackedByBat(){
		speed -= 5;
		StartCoroutine(BatSlowedSpeed());
	}

	IEnumerator BatSlowedSpeed(){
		yield return new WaitForSeconds(5);
		speed = 16;
	}
	//___________________________________________OnScreenAmmo

	void ThrowGrenade(){
		grenades --;
		grenadeCount -= 1;
		anim.SetBool("Grenade", true);
		Instantiate(grenade, transform.position, Quaternion.identity);
		GrenadesOnScreen();
	}

	void AmmoOnScreen(){
		if (ammoCount <= 0){
			ammoCount = 0;
			ammo.text = "" + ammoCount;
			if (totalClips <= 0){
				totalClips = 0;
				clips.text = "" + totalClips;
				reloadText.SetActive(true);
				// StartCoroutine(ReloadFlash());
			}
			else {
				clips.text = "" + totalClips;
				reloadText.SetActive(true);
			}
		}
		else if (ammoCount >= 1){
			ammo.text = "" + ammoCount;
			if (totalClips <= 0){
				totalClips = 0;
				clips.text = "" + totalClips;
			}
			else {
				clips.text = "" + totalClips;
			}
		}
	}

	void GrenadesOnScreen(){
		grenadesLeft.text = "" + grenadeCount;
	}

	void DroidsOnScreen(){
		droidCount.text = "" + totalDroids;
	}

	IEnumerator TurnReloadFalse(){
		yield return new WaitForSeconds(0.8f);
		isReloading = false;
	}

	//_____________________________________________Collision

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "EnemyAttack" && !immortal){
			if (health >= 1){
				health -= 1;
				livesLeft.text = "" + health;
				anim.SetTrigger("HurtTrigger");
				rigid.WakeUp();
				StartForce();
				immortal = true;
				soundMng.PlaySfx(playerHurt);
				StartCoroutine(Invincible());

			}
			else if (other.tag == "EnemyAttack" && !immortal){
				Death();
			}
		}
		if (other.tag == "DroidItem"){
			totalDroids += 1;
			DroidsOnScreen();
			soundMng.PlaySfx(droidItem);
			Destroy(other.gameObject);
		}
		if (other.tag == "BombItem"){
			grenades += 1;
			grenadeCount += 1;
			GrenadesOnScreen();
			soundMng.PlaySfx(grenadeItem);
			Destroy(other.gameObject);
		}
		if (other.tag == "LifeItem"){
			health += 1;
			livesLeft.text = "" + health;
			soundMng.PlaySfx(lifeItem);
			Destroy(other.gameObject);
		}
		if (other.tag == "Wrench"){
			GameManager.Instance.PlayerHasWrench = true;
			soundMng.PlaySfx(wrenchItem);
			Destroy(other.gameObject);
		}
		if (other.tag == "Ammo"){
			totalClips += 1;
			AmmoOnScreen();
			soundMng.PlaySfx(ammoItem);
			Destroy(other.gameObject);
		}
		if (other.tag == "ShotGunSpecial"){
			shotGunSpecial = true;
			totalClips += 2;
			AmmoOnScreen();
			Destroy(other.gameObject);
			soundMng.PlaySfx(shotgunSpecialItem);
			StartCoroutine(ShotGunSpecialTime());
		}
		if (other.tag == "Mech"){
			if (GameManager.Instance.PlayerHasWrench){
				RepairMech();
				soundMng.PlaySfx(repairMech);
			}
		}
		if (other.tag == "Bat"){
			AttackedByBat();
			livesLeft.text = "" + health;
			anim.SetTrigger("HurtTrigger");
			rigid.WakeUp();
			StartForce();
			immortal = true;
			soundMng.PlaySfx(playerHurt);
			StartCoroutine(Invincible());
		}
		if (other.tag == "SpeedBoost"){
			speed = 26;
			Destroy(other.gameObject);
			soundMng.PlaySfx(speedBoostItem);
			StartCoroutine(HasSpeedBoost());
		}
		
	}

	IEnumerator HasSpeedBoost(){
		yield return new WaitForSeconds(15);
		speed = 16;
	}

	private void StartForce(){
		if (sprite.flipX){
			rigid.AddForce(Vector2.right * 9, ForceMode2D.Impulse);
			StartCoroutine(StopForce());
		}
		else if (!sprite.flipX){
			rigid.AddForce(Vector2.left * 9, ForceMode2D.Impulse);
			StartCoroutine(StopForce());
		}
	}

	IEnumerator StopForce(){
		yield return new WaitForSeconds(0.2f);
			rigid.Sleep();
	}

	void RepairMech(){
		isReloading = true;
		GameManager.Instance.PlayerHasWrench = false;
		GameManager.Instance.PlayerIsRepairing = true;
		StartCoroutine(RepairingAnimation());
	}

	IEnumerator RepairingAnimation(){
		yield return new WaitForSeconds(0.5f);
		anim.SetTrigger("Reload");
		StartCoroutine(TurnReloadFalse());
		yield return new WaitForSeconds(0.5f);
		anim.SetTrigger("Reload");

	}

	IEnumerator ShotGunSpecialTime(){
		yield return new WaitForSeconds(35);
			shotGunSpecial = false;
	}

	IEnumerator Invincible(){
		StartCoroutine(IndicateImmortal());
		while(immortal){
			yield return new WaitForSeconds(.1f);
			sprite.enabled = false;
			yield return new WaitForSeconds(.1f);
			sprite.enabled = true;	
		}
	}

	IEnumerator IndicateImmortal(){
		yield return new WaitForSeconds(immortalTime);
		immortal = false;
	}

	void Death(){
		anim.SetTrigger("Dead");
		isDead = true;
		boxCollider.enabled = false;
		soundMng.PlaySfx(playerDeath);
		StartCoroutine(PlayerDiedResetMenu());
	}
	
	IEnumerator PlayerDiedResetMenu(){
		yield return new WaitForSeconds(1.8f);
		SceneManager.LoadScene("GameOverPlayerDied");
	}
}
