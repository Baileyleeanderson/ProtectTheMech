using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField] private GameObject[] enemies;
	[SerializeField] private int waveNumber;
	[SerializeField] private int totalEnemies;
	[SerializeField] private int enemiesPerSpawn;
	[SerializeField] private Text currentWave;
	[SerializeField] private GameObject playBtn;
	[SerializeField] private Vector3[] spawnPoints;
	[SerializeField] private GameObject wrenchOnScreen;
	[SerializeField] private GameObject gameOverScreen;
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private Transform playerSpawnPos;
	[SerializeField] private AudioSource soundMng;
	[SerializeField] private GameObject controlScreen;

	[SerializeField] private AudioClip[] songs;
	
	public GameObject player;
	
	private static GameManager instance;
	private bool waveMenu = true;
	private bool playerIsAlive = true;
	private bool waveOver = true;
	private bool droidIsSpawned = false;
	private bool playerHasWrench = false;
	private bool playerIsRepairing = false;
	private bool mechIsAlive = true;

	public int WaveNumber {
		get { return waveNumber;
	}
		set {
			waveNumber = value;
		}
	}
	public int TotalEnemies {
		get {
			return totalEnemies;
		}
		set {
			totalEnemies = value;
		}
	}
	public static GameManager Instance{
		get {
			if(instance == null){

			}
			return instance;
		}
	}
	public bool WaveMenu {
		get {
			return waveMenu;
		}
	}
	public bool PlayerIsAlive {
		get {
			return playerIsAlive;
		}
		set {
			playerIsAlive = value;
		}
	}
	public bool WaveOver {
		get {
			return waveOver;
		}
	}
	public bool DroidIsSpawned {
		get {
			return droidIsSpawned;
		}
		set{
			droidIsSpawned = value;
		}
	}
	public bool PlayerHasWrench {
		get {
			return playerHasWrench;
		}
		set{
			playerHasWrench = value;
		}
	}
	public bool PlayerIsRepairing {
		get {
			return playerIsRepairing;
		}
		set{
			playerIsRepairing = value;
		}
	}
	public bool MechIsAlive {
		get {
			return mechIsAlive;
		}
		set{
			mechIsAlive = value;
		}
	}

	private void Awake(){
		instance = this;
	}

	private void Start() {
		playBtn.SetActive(true);
		wrenchOnScreen.SetActive(false);
		soundMng.clip = songs[0];
		soundMng.Play();
	}

	void FixedUpdate(){

		if (mechIsAlive){
			playerIsAlive = true;	
		}
		if (totalEnemies <= 0){
			SetCurrentGameState();
		}
		if (playerHasWrench == true){
			wrenchOnScreen.SetActive(true);
		}
		if (playerHasWrench == false){
			wrenchOnScreen.SetActive(false);
		}
	}

	//________________________________________SpawnEnemy

	public void SpawnEnemy(){
		StartCoroutine(SpawnEnemiesDifferentTime());
	}

	IEnumerator SpawnEnemiesDifferentTime(){
		for (int i = 1; i <= enemiesPerSpawn; i++){
			int randomSpawnPoint = Random.Range(0,2);
			int randomTime = Random.Range(0, 4);
			int randomEnemy = Random.Range(0, 3);

			if (randomEnemy == 0){
				yield return new WaitForSeconds(randomTime);
				Instantiate(enemies[0], spawnPoints[randomSpawnPoint], Quaternion.identity);
			}
			else if (randomEnemy == 1){
				yield return new WaitForSeconds(randomTime);
				Instantiate(enemies[1], spawnPoints[randomSpawnPoint], Quaternion.identity);
			}
			else if (randomEnemy == 2 && waveNumber >= 5){
				yield return new WaitForSeconds(randomTime);
				Instantiate(enemies[2], spawnPoints[randomSpawnPoint], Quaternion.identity);
			}
			else if (randomEnemy == 2 && waveNumber <= 4){
				yield return new WaitForSeconds(randomTime);
				int tempRandomEnemy = Random.Range(0, 2);
				Instantiate(enemies[tempRandomEnemy], spawnPoints[randomSpawnPoint], Quaternion.identity);
			}
		}
		if (waveNumber % 3 == 0){
			for (int bat = 0; bat < waveNumber; bat++){
				int randomSpawnPoint = Random.Range(0,2);
				int randomTime = Random.Range(0, 2);
				yield return new WaitForSeconds(randomTime);
				Instantiate(enemies[3], spawnPoints[randomSpawnPoint], Quaternion.identity);
			}
		}		
	}

	//_________________________________________SetCurrentStatus
	
	public void SetCurrentGameState(){
		
		playBtn.SetActive(true);
		waveNumber += 1;
		currentWave.text = "Wave: " + waveNumber;
		enemiesPerSpawn = waveNumber + 1;
		waveMenu = true;
		waveOver = false;
		totalEnemies = enemiesPerSpawn;
	}

	//_________________________________________Deactivate button

	public void DeactivePlaybtn(){
		playBtn.SetActive(false);
		currentWave.text = "Wave: " + waveNumber;
		waveMenu = false;
		waveOver = true;
		MechIsAlive = true;
		SpawnEnemy();
		if (waveNumber == 5){
			soundMng.clip = songs[1];
			soundMng.Play();
		}
		else if (waveNumber == 10 && waveNumber < 15){
			soundMng.clip = songs[2];
			soundMng.Play();
		}
		else if (waveNumber >= 15 && waveNumber % 5 == 0){
			if (!waveMenu){
				int randSong = Random.Range(0,4);
				soundMng.clip = songs[randSong];
				soundMng.Play();
			}
		}
	}

	public void ShowControls(){
		controlScreen.SetActive(true);
	}

	public void HideControls(){
		controlScreen.SetActive(false);
	}

}
