using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;

namespace Main._Project.Scripts.Utilities.Pool
{
    public class PoolSystem : SingletonMonoBehaviour<PoolSystem>
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        protected override void Awake()
        {
            base.Awake();
            SpawnPool();
        }

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        void SpawnPool()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.AddComponent<PoolableObject>().PoolTag = pool.tag;
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnGameObject(string poolTag)
        {
            if (!poolDictionary.ContainsKey(poolTag))
            {
                Debug.LogWarning("Pool with tag " + poolTag + " doesn't exist.");
                return null;
            }
            
            if (poolDictionary[poolTag].Count == 0)
            {
                Debug.LogWarning("Pool is empty, adding a new object to pool.");

                Pool pool = pools.Find(p => p.tag == poolTag);
                if (pool != null)
                {
                    GameObject newObj = Instantiate(pool.prefab);
                    newObj.SetActive(false);
                    newObj.AddComponent<PoolableObject>().PoolTag = pool.tag;
                    poolDictionary[poolTag].Enqueue(newObj);
                }
                else
                {
                    Debug.LogError("Pool with tag " + poolTag + " not found in pool list.");
                    return null;
                }
            }

            GameObject objectToSpawn = poolDictionary[poolTag].Dequeue();
            objectToSpawn.SetActive(true);
            return objectToSpawn;
        }
    
        public void ReturnToPool(string poolTag, GameObject obj) => poolDictionary[poolTag].Enqueue(obj);
    }
}