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

	public bool reGen = false;

	private float _curPos;
	private float _nextCloudSpawn;
	private ObjectPoolerWorld _groundObjectPool;
	private ObjectPoolerWorld _grassObjectPool;
	private ObjectPoolerWorld _cloudObjectPool;
	private List<GameObject> _spawnedObjects = new List<GameObject>();

	void Start () 
	{
		_groundObjectPool = GameObject.Find("_GroundObjects").GetComponent<ObjectPoolerWorld>();
		_grassObjectPool = GameObject.Find("_GrassObjects").GetComponent<ObjectPoolerWorld>();
		_cloudObjectPool = GameObject.Find("_CloudObjects").GetComponent<ObjectPoolerWorld>();
		_curPos = startPos;
	}

	void Update()
	{
		if(reGen)
		{
			foreach(GameObject g in _spawnedObjects)
			{
				g.SetActive(false);
			}
			_spawnedObjects.Clear();
			_curPos = _curPos - (2*padding*width);
			reGen = false;
		}
		GenerateGround();
		GenerateClouds();
		CleanUp();
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
		if(SpawnGround())
		{
			_spawnedObjects.Add(_groundObjectPool.Instantiate(new Vector3(_curPos, 0), Quaternion.identity));
			_spawnedObjects.Add(_grassObjectPool.Instantiate(new Vector3(_curPos, 1), Quaternion.identity));
			_curPos += width;
		}
	}

	bool SpawnGround()
	{
		bool ret = true;
		foreach(GameObject g in _spawnedObjects)
		{
			if(g.transform.position.x > transform.position.x + (padding*width))
			{
				ret = false;
			}
		}
		return ret;
	}

	void CleanUp()
	{
		for(int i = 0; i < _spawnedObjects.Count; i++)
		{
			if(_spawnedObjects[i].transform.position.x < transform.position.x - (padding*width))
			{
				_spawnedObjects[i].SetActive(false);
				_spawnedObjects.RemoveAt(i);
			}
		}
	}

}
