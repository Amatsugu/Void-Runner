using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
	public float tileWidth;
	public float startOffset;
	public int generationRange = 2;
	public float spikeSpawnChance = 10;
	public float minSpikeSpacing = 2;
	public float maxGroupSize = 3;
	public float loopLimit = 100;
	public ParticleSystem cloudSystem; 

	public bool reGen = false;

	private float _curGenerationPos;
	private float _nextSpikePos;
	private ObjectPoolerWorld _groundObjectPool;
	private ObjectPoolerWorld _grassObjectPool;
	private ObjectPoolerWorld _spikeObjectPool;
	private List<GameObject> _spawnedObjects = new List<GameObject>();
	private List<GameObject> _spawnedGround = new List<GameObject>();
	private List<GameObject> _staticObsticles = new List<GameObject>();
	private Player _player;
	private Transform _thisTransform;
	private bool _isPaused;
	private ParticleSystem.Particle[] _couldParticles;

	public void Pause()
	{
		_isPaused = true;
	}

	public void UnPause()
	{
		_isPaused = false;
	}

	void Start () 
	{
		_thisTransform = transform;
		_groundObjectPool = GameObject.Find("_GroundObjects").GetComponent<ObjectPoolerWorld>();
		_grassObjectPool = GameObject.Find("_GrassObjects").GetComponent<ObjectPoolerWorld>();
		_spikeObjectPool = GameObject.Find("_SpikeObjects").GetComponent<ObjectPoolerWorld>();
		_player = GetComponent<Player>();
		_curGenerationPos = startOffset;
		_couldParticles = new ParticleSystem.Particle[cloudSystem.maxParticles];
	}

	void Update()
	{
		if(_isPaused)
			return;
		Loop();
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
			_curGenerationPos = _curGenerationPos - (2*generationRange*tileWidth);
			reGen = false;
		}

		GenerateGround();
		//GenerateSpikes();
		//CleanUpObjects();
		CleanUpGround();
		//CleanUpStaticObstacles();
	}

	void GenerateSpikes()
	{
		if(_curGenerationPos < _nextSpikePos)
			return;
		bool canSpawn = true;
		if((int)Random.Range(0, spikeSpawnChance) == 0)
		{
			foreach(GameObject g in _staticObsticles)
			{
				if(g.tag == "Spikes")
				{
					if(g.transform.position.x == _curGenerationPos)
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
				_staticObsticles.Add(_spikeObjectPool.Instantiate(new Vector3(_curGenerationPos + (tileWidth*i), 0), Quaternion.identity));
			}
			_nextSpikePos = _curGenerationPos + (tileWidth*groupSize) + minSpikeSpacing;
		}
	}

	void GenerateGround()
	{
		while(SpawnGround())
		{
			_spawnedGround.Add(_groundObjectPool.Instantiate(new Vector3(_curGenerationPos, -2), Quaternion.identity));
			_spawnedGround.Add(_grassObjectPool.Instantiate(new Vector3(_curGenerationPos, 0), Quaternion.identity));
			_curGenerationPos += tileWidth;
		}
	}

	bool SpawnGround()
	{
		if (_spawnedGround.Count == 0)
			return true;
		if(_spawnedGround[_spawnedGround.Count-1].transform.position.x >= Mathf.Floor(_thisTransform.position.x) + (generationRange*tileWidth))
		{
			return false;
		}
		return true;
	}

	void CleanUpObjects()
	{
		while(_spawnedObjects[0].transform.position.x <= Mathf.Floor(_thisTransform.position.x) - (generationRange*tileWidth))
		{
			_spawnedObjects[0].SetActive(false);
			_spawnedObjects.RemoveAt(0);
		}
	}

	void CleanUpStaticObstacles()
	{
		while(_staticObsticles[0].transform.position.x <= Mathf.Floor(_thisTransform.position.x) - (generationRange*tileWidth))
		{
			_staticObsticles[0].SetActive(false);
			_staticObsticles.RemoveAt(0);
		}
	}

	void CleanUpGround()
	{
		while(_spawnedGround[0].transform.position.x <= Mathf.Floor(_thisTransform.position.x) - (generationRange * tileWidth))
		{
			_spawnedGround[0].SetActive(false);
			_spawnedGround.RemoveAt(0);
		}
	}

	void Loop()
	{
		if(Mathf.Floor(_thisTransform.position.x) >= loopLimit)
		{
			float curX = Mathf.Floor(_thisTransform.position.x);
			//Camera Move
			Transform cTrans = Camera.main.transform;
			Vector3 cPos = cTrans.position;
			cPos.x = (cPos.x - curX);
			cTrans.position = cPos;

			//Move Objects
			foreach(GameObject g in _spawnedObjects)
			{
				Vector3 newPos = g.transform.position;
				newPos.x = (newPos.x - curX);
				g.transform.position = newPos;
			}
			//Move ground
			foreach(GameObject g in _spawnedGround)
			{
				
				Vector3 newPos = g.transform.position;
				newPos.x = (newPos.x - curX);
				g.transform.position = newPos;
			}

			//Partilce Move
			int l = cloudSystem.GetParticles(_couldParticles);
			for (int i = 0; i < l; i++)
			{
				Vector3 newPos = _couldParticles[i].position;
				newPos.x = (newPos.x - curX);
				_couldParticles[i].position = newPos;
			}
			cloudSystem.SetParticles(_couldParticles, l);
			//Update info
			_curGenerationPos = startOffset;
			_player.Loop();
		}
	}

}
