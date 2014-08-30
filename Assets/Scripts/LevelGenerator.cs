using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
	public float width;
	public float startPos;
	public int padding = 2;
	public int cloudSpawnChance = 5;
	public float cloudSpawnRate = 3;
	public float minCouldHeight = 4;
	public float maxCloudHeight = 10;
	public float spikeSpawnChance = 10;
	public float minSpikeSpacing = 2;
	public float maxGroupSize = 3;
	public float loopLimit = 100;

	public bool reGen = false;

	private float _curPos;
	private float _nextCloudSpawn;
	private float _nextSpikePos;
	private ObjectPoolerWorld _groundObjectPool;
	private ObjectPoolerWorld _grassObjectPool;
	private ObjectPoolerWorld _cloudObjectPool;
	private ObjectPoolerWorld _spikeObjectPool;
	private List<GameObject> _spawnedObjects = new List<GameObject>();
	private List<GameObject> _spawnedGround = new List<GameObject>();
	private List<GameObject> _staticObsticles = new List<GameObject>();
	private Player _player;
	private Transform _thisTransform;
	private bool _isPaused;
	private float _prePausedTime;

	public void Pause()
	{
		_isPaused = true;
		_prePausedTime = Time.time;
	}

	public void UnPause()
	{
		_isPaused = false;
		float dTime = Time.time - _prePausedTime;
		_nextCloudSpawn += dTime;
	}

	void Start () 
	{
		_thisTransform = transform;
		_groundObjectPool = GameObject.Find("_GroundObjects").GetComponent<ObjectPoolerWorld>();
		_grassObjectPool = GameObject.Find("_GrassObjects").GetComponent<ObjectPoolerWorld>();
		_cloudObjectPool = GameObject.Find("_CloudObjects").GetComponent<ObjectPoolerWorld>();
		_spikeObjectPool = GameObject.Find("_SpikeObjects").GetComponent<ObjectPoolerWorld>();
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		_curPos = startPos;
	}

	void Update()
	{
		if(_isPaused)
			return;
		//Loop();
		if(reGen)
		{
			foreach(GameObject g in _spawnedObjects)
			{
				g.SetActive(false);
			}
			foreach(GameObject g in _spawnedGround)
			{
				g.SetActive(false);
			}
			_spawnedObjects.Clear();
			_spawnedGround.Clear();
			_curPos = _curPos - (2*padding*width);
			reGen = false;
		}
		GenerateGround();
		GenerateSpikes();
		GenerateClouds();
		CleanUpObjects();
		CleanUpGround();
		CleanUpStaticObstacles();
	}

	void GenerateSpikes()
	{
		if(_curPos < _nextSpikePos)
			return;
		bool canSpawn = true;
		if((int)Random.Range(0, spikeSpawnChance) == 0)
		{
			foreach(GameObject g in _staticObsticles)
			{
				if(g.tag == "Spikes")
				{
					if(g.transform.position.x == _curPos)
					{
						canSpawn = false;
						Debug.Log("overlap");
						break;
					}
				}
			}
		}else
			canSpawn = false;

		if(canSpawn)
		{
			int groupSize = (int)Random.Range(1, maxGroupSize);
			for(int i = 0; i < groupSize; i++)
			{
				_staticObsticles.Add(_spikeObjectPool.Instantiate(new Vector3(_curPos + (width*i), 0), Quaternion.identity));
			}
			_nextSpikePos = _curPos + (width*groupSize) + minSpikeSpacing;
		}
	}

	void GenerateClouds()
	{
		if(_nextCloudSpawn > Time.time)
			return;
		if(Random.Range(0, cloudSpawnChance) == 0)
		{
			_spawnedObjects.Add(_cloudObjectPool.Instantiate(new Vector3(_curPos, Random.Range(minCouldHeight, maxCloudHeight)), Quaternion.identity));
			_nextCloudSpawn = Time.time + cloudSpawnRate;
		}
	}

	void GenerateGround()
	{
		while(SpawnGround())
		{
			_spawnedGround.Add(_groundObjectPool.Instantiate(new Vector3(_curPos, -2), Quaternion.identity));
			_spawnedGround.Add(_grassObjectPool.Instantiate(new Vector3(_curPos, 0), Quaternion.identity));
			_curPos += width;
		}
	}

	bool SpawnGround()
	{
		bool ret = true;
		foreach(GameObject g in _spawnedGround)
		{
			if(g.transform.position.x > _thisTransform.position.x + (padding*width))
			{
				ret = false;
			}
		}
		return ret;
	}

	void CleanUpObjects()
	{
		for(int i = 0; i < _spawnedObjects.Count; i++)
		{
			if(_spawnedObjects[i].transform.position.x < _thisTransform.position.x - (padding*width))
			{
				_spawnedObjects[i].SetActive(false);
				_spawnedObjects.RemoveAt(i);
			}
		}
	}

	void CleanUpStaticObstacles()
	{
		for(int i = 0; i < _staticObsticles.Count; i++)
		{
			if(_staticObsticles[i].transform.position.x < _thisTransform.position.x - (padding*width))
			{
				_staticObsticles[i].SetActive(false);
				_staticObsticles.RemoveAt(i);
			}
		}
	}
	void CleanUpGround()
	{
		for(int i = 0; i < _spawnedGround.Count; i++)
		{
			if(_spawnedGround[i].transform.position.x < _thisTransform.position.x - (padding*width))
			{
				_spawnedGround[i].SetActive(false);
				_spawnedGround.RemoveAt(i);
			}
		}
	}

	void Loop()
	{
		if(_thisTransform.position.x > loopLimit)
		{
			float curX = _thisTransform.position.x;
			foreach(GameObject g in _spawnedObjects)
			{
				Vector3 newPos = g.transform.position;
				newPos.x = startPos + (newPos.x - curX);;
				Debug.Log(newPos);
				g.transform.position = newPos;
			}
			
			foreach(GameObject g in _spawnedGround)
			{
				
				Vector3 newPos = g.transform.position;
				newPos.x = (newPos.x - curX);
				Debug.Log(newPos);
				g.transform.position = newPos;
			}
			for(int i = 0; i < _spawnedGround.Count; i++)
			{
				if(_spawnedGround[i].transform.position.x > (padding*width))
				{
					_spawnedGround[i].SetActive(false);
					_spawnedGround.RemoveAt(i);
				}
			}
			_curPos = startPos;
			_player.Loop();
		}
	}

}
