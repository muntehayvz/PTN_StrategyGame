using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolManager
{
	private readonly List<GameObject> pooledObjects = new List<GameObject>();
        
	private readonly List<GameObject> inactiveObjects = new List<GameObject>();

    // Constructor: Initializes tags for inactive objects
    public PoolManager()
	{
		for (var index = 0; index < inactiveObjects.Count; index++)
		{
			inactiveObjects[index].tag = index.ToString();
		}
	}

    // Sets the list of pooled objects for object reuse
    public void SetPooledObjects(List<GameObject> units)
	{
		this.pooledObjects.Clear();
		foreach (var unit in units)
		{
			this.pooledObjects.Add(unit);
		}
	}

    // Deactivates and adds a game object to the pool of inactive objects.
    public void AddObjectToPool(GameObject gameObject)
	{
		gameObject.SetActive(false);
		inactiveObjects.Add(gameObject);
	}

    // Retrieves an inactive object from the pool based on index, activates it, and returns it
    // If no matching inactive object is found, creates and returns a new instance
    public GameObject GetObjectFromPool(int index)
	{ 
		foreach (var item in inactiveObjects)
		{
			if (item.name.Equals(pooledObjects[index].name))
			{
				item.SetActive(true);
				inactiveObjects.Remove(item);
				return item;
			}
		}

		var newObj = Object.Instantiate(pooledObjects[index]);
		newObj.name = pooledObjects[index].name;
		newObj.SetActive(true);
		return newObj;

	}
    // Returns the size of the pooled objects list
    public int GetPoolSize()
	{
		return pooledObjects.Count;
	}
}