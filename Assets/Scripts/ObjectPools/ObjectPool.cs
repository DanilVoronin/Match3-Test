using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ObjectPools
{
    /// <summary>
    /// Пул игровых объектов, например пуль
    /// </summary>
    [AddComponentMenu("ObjectPools/ObjectPool")]
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab; // Префаб объекта, который будет использоваться в пуле
        [SerializeField] private int initialSize = 10; // Начальный размер пула

        private Queue<ObjectInformation> pool = new Queue<ObjectInformation>();
        private int n = 0;

        [Inject] private DiContainer _diContainer;

        /// <summary>
        /// Инициализация пула
        /// </summary>
        /// <param name="prefab">Объект используемый пулом</param>
        public void Init<T>(GameObject prefab)
        {
            this.prefab = prefab;
            Init<T>();
        }

        /// <summary>
        /// Инициализация пулла
        /// </summary>
        public void Init<T>()
        {
            // Заполняем пул объектами при запуске
            for (int i = 0; i < initialSize; i++)
            {
                ObjectInformation obj = CreateNewObject<T>();
                pool.Enqueue(obj);
            }
        }

        private ObjectInformation CreateNewObject<T>()
        {
            GameObject obj = _diContainer.InstantiatePrefab(prefab, transform);
            
            obj.name = $"{prefab.name}_{n}";
            n++;

            ObjectInformation objectInformation = new ObjectInformation()
            {
                GameObject = obj,
                Type = obj.GetComponent<T>()
            };
            obj.SetActive(false); // Делаем объект неактивным

            ReturnObjectPool returnObjectPool = obj.AddComponent<ReturnObjectPool>();
            returnObjectPool.Init(this, objectInformation);

            return objectInformation;
        }

        /// <summary>
        /// Возвразает информацию о выданном объекте вмести с запрошенным типом
        /// Необходимо включить объект послк получения
        /// </summary>
        /// <typeparam name="T">Запрашиваемый тип</typeparam>
        /// <returns></returns>
        public ObjectInformation GetObject<T>()
        {
            if (pool.Count > 0)
            {
                ObjectInformation obj = pool.Dequeue();
                //Не включаем объект сразу, что бы успеть его сконфигурирова
                //Необходимо включить объект послк получения
                return obj;
            }
            else
            {
                // Если пул пуст, создаем новый объект
                return CreateNewObject<T>();
            }
        }

        /// <summary>
        /// Возвращает объект в пулл
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnObject(ObjectInformation obj)
        {
            if (pool.Contains(obj))
            {
                Debug.LogError("pool.Contains(obj)");
                return;
            }

            obj.GameObject.SetActive(false); // Деактивируем объект
            pool.Enqueue(obj); // Возвращаем объект в пул
        }
    }
}
