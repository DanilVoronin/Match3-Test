using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ObjectPools
{
    /// <summary>
    /// Управляет пулами игровых объектов создавая общую базу пулов
    /// Предоставляет доступ к путу через 
    /// </summary>
    [AddComponentMenu("ObjectPools/ManagerPools")]
    public class ManagerPools : MonoBehaviour
    {
        private Dictionary<string, ObjectPool> objectPools = new Dictionary<string, ObjectPool>();

       [Inject] private DiContainer _diContainer;

        private void Awake()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Возвращает пул игровых объектов по префабу
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public ObjectPool GetObjectPoolByPrefab<T>(GameObject prefab)
        {
            if (!objectPools.ContainsKey(prefab.name))
            {
                GameObject pool = new GameObject(prefab.name);
                pool.transform.parent = transform;
                pool.transform.position = Vector3.zero;
                pool.transform.rotation = Quaternion.identity;

                ObjectPool objectPool = _diContainer.InstantiateComponent<ObjectPool>(pool);
                objectPool.Init<T>(prefab);

                objectPools.Add(prefab.name, objectPool);
                return objectPool;
            }
            else
            {
                return objectPools[prefab.name];
            }
        }
    }
}
