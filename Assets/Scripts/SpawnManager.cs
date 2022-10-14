using UnityEngine;
using Unity.Netcode;
using Core.Singleton;

public class SpawnManager : NetworkSingleton<SpawnManager>
{
    #region Fields
    [SerializeField]
    GameObject objectPrefab;

    int maxObjectsToInstantiate = 10;
    #endregion

    #region Unity methods
    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public methods
    public void SpawnObjects()
    {
        if(!IsServer) return;

        for(int i = 0; i < maxObjectsToInstantiate; i++)
        {
            GameObject tempObject = Instantiate(objectPrefab,
                                                new Vector3(Random.Range(-10, 10), 10f, Random.Range(-10, 10)), 
                                                Quaternion.identity);

            tempObject.GetComponent<NetworkObject>().Spawn();
        }
    }
    #endregion
}
