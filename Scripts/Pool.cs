//Jordan Black 2017

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour 
{

	public static Pool Instance;

	//an archive of the various pools
	private Dictionary<string, List<GameObject>> poolArchive;

	//a list of the individual prefabs for each archive, so that the effect lists can grow
	public List<GameObject> archivePrefabs = new List<GameObject>();

	void Awake()
	{

        //singleton
		if(Instance == null)
        {
			Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

		poolArchive = new Dictionary<string, List<GameObject>>();
	}

	public GameObject CreatePool(GameObject prefab, int size = 1)
    {
        GameObject g = null;

		//Try to find an existing pool before creating a new one.
		if(poolArchive.ContainsKey(prefab.name))
        {
			//Debug.LogFormat("Creating pool for {0}", prefab.name);

			for(int j = 0; j < size; j++){
				g = Instantiate(prefab) as GameObject;
				poolArchive[prefab.name].Add(g);
				g.SetActive(false);
			}

		}
      else
      {
			//Debug.LogFormat("Creating new archive and pool for {0}", prefab.name);
			List<GameObject> effectList = new List<GameObject>();
			for(int k = 0; k < size; k++)
      {
				g = Instantiate(prefab) as GameObject;
				effectList.Add(g);
				g.SetActive(false);
			}
			poolArchive.Add(prefab.name, effectList);
			archivePrefabs.Add(prefab);
		}
        
        return g;

    }
    
	public void ResetPools()
  {
		poolArchive.Clear();
	}

	public List<GameObject> GetArchive(string name)
  {
		if(poolArchive.ContainsKey(name))
    {
			return poolArchive[name];
		}
    else
    {
			return null;
		}
	}

	public GameObject GetPooledObjectInArchive(string name)
  {

		if(poolArchive.ContainsKey(name))
    {
			//try to return a non-activated gameobject in the archived pool
			if(poolArchive[name].Count > 0)
      {
				List<GameObject> gList;
				poolArchive.TryGetValue(name, out gList);
				foreach(GameObject g in gList)
        {
					if(!g.activeInHierarchy)
          {
                        //Debug.Log("(Pool) Using existing " + g.name);
						return g;
					}
				}

			}

			//can grow
			if(archivePrefabs.Count > 0)
      {
				for(int i = 0; i < archivePrefabs.Count; i++)
        {
					if(archivePrefabs[i].name == name)
          {
						GameObject g = Instantiate(archivePrefabs[i]) as GameObject;
						poolArchive[name].Add(g);
                        //Debug.Log("(Pool) Adding new " + g.name);
						return g;
					}
				}
			}
		}
    else
    {
			Debug.LogWarningFormat("No pool archive for {0}", name);
			return null;
		}

		Debug.LogWarningFormat("No pooled object found for {0}", name);
		return null;
	}

	//Because of how dictionaries are created, sometimes we need to check if it exists first
	public bool ArchiveExists(string name)
  {
		return poolArchive.ContainsKey(name);
	}
}
