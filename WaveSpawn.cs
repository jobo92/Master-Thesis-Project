using UnityEngine;
using System.Collections;
public class WaveSpawn : MonoBehaviour {

	public int waveSize;
	public GameObject enemyPrefab;
	public GameObject runnerEnemyPrefab;
	public GameObject armorEnemyPrefab;
	public GameObject toughEnemyPrefab;
	public GameObject stunEnemyPrefab;
	public GameObject pathIndicatorParticlePrefab;
	public float enemyInterval;
	public Transform spawnPoint;
	public float startTime;
	public Transform parentTransform;
	int enemyCount=0;
	public bool enemyWavesNotStarted = true;
	public Transform[] wayPoints;
	public WaveTrackKeeper wTK;

	void Start ()
    {
		// InvokeRepeating("SpawnEnemy",startTime,EnemyInterval);
		wTK = GameObject.Find("GameManager").GetComponent<WaveTrackKeeper>();
	}

	void Update()
	{
		/*if(enemyCount == waveSize)
		{
			CancelInvoke("SpawnEnemy");
		}*/
	}

	public void StartSpawningEnemies()
	{
		//Debug.Log("enemyWavesNotStarted: " + enemyWavesNotStarted);
		//Debug.Log("check to start spawning");
		if (enemyWavesNotStarted == true)
		{
			enemyWavesNotStarted = false;
			//Debug.Log("1start spawning");
			InvokeRepeating("SpawnEnemy", startTime, enemyInterval);
			//SpawnEnemy2();

			//Uncomment the next line to repeatedly spawn the path indicator, might need some  tweeking
			//InvokeRepeating("SpawnPathIndicator", startTime, enemyInterval);
			Invoke("SpawnPathIndicator", startTime);
			//Debug.Log("2start spawning");
		}
	}

	public IEnumerator ConditionSpawnEnemies(float intervalTime, GameObject[] typeOfEnemy)
	{
		for(int i = 0; i < typeOfEnemy.Length; i++)
        {
			GameObject enemy = GameObject.Instantiate(typeOfEnemy[i], spawnPoint.transform.position, spawnPoint.transform.rotation, parentTransform.transform.parent) as GameObject;
			enemy.GetComponent<EnemyOriginal>().waypoints = wayPoints;
			wTK.EnemySpawned();
			yield return new WaitForSeconds(intervalTime);
		}
	}

	void SpawnEnemy()
	{
		enemyCount++;
		//Debug.Log("spawning enemies");
		enemyCount++;
		/*Debug.Log("spawnPoint= " + spawnPoint);
		Debug.Log("parent= " + parentTransform);
		Debug.Log("EnemyPrefab= " + enemyPrefab);*/
		//Vector3 pos = spawnPoint.transform.position + new Vector3(0, 0.06f, 0);
		GameObject enemy = GameObject.Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, parentTransform.transform.parent) as GameObject;
		enemy.GetComponent<EnemyOriginal>().waypoints = wayPoints;
		//Debug.Log("Enemy spawned");
    }

	void SpawnEnemy2()
	{
		enemyCount++;
		//Debug.Log("spawning enemies");
		enemyCount++;
		/*Debug.Log("spawnPoint= " + spawnPoint);
		Debug.Log("parent= " + parentTransform);
		Debug.Log("EnemyPrefab= " + enemyPrefab);*/
		//Vector3 pos = spawnPoint.transform.position + new Vector3(0, 0.06f, 0);
		GameObject enemy = GameObject.Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, parentTransform.transform.parent) as GameObject;
		enemy.GetComponent<EnemyOriginal>().waypoints = wayPoints;

		//Debug.Log("Enemy spawned");
	}

	public void SpawnPathIndicator()
    {
		GameObject pathIndicator = GameObject.Instantiate(pathIndicatorParticlePrefab, spawnPoint.transform.position + new Vector3(0, 0.04f, 0), spawnPoint.transform.rotation, parentTransform.transform.parent) as GameObject;
		pathIndicator.GetComponent<PathIndicator>().waypoints = wayPoints;
	}
}
