using UnityEngine;
using Unity.Netcode;

namespace Core.Singleton 
{
    public class NetworkSingleton<T> : NetworkBehaviour where T : Component
    {
        static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if(objs.Length > 0)
                    { _instance = objs[0]; }
                    if(objs.Length > 1)
                    {
                        Debug.LogError("There`s more than one " + typeof(T).Name + " in the scene!");
                    }
                    if(_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = string.Format("_{0}", typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
}
