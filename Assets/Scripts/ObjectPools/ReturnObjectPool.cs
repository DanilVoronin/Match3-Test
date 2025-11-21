using UnityEngine;

namespace ObjectPools
{
    /// <summary>
    /// Возвращает игровой объект в пулл объектов при отключении
    /// </summary>
    [AddComponentMenu("ObjectPools/ReturnObjectPool")]
    public class ReturnObjectPool : MonoBehaviour
    {
        private ObjectPool objectPool;
        private ObjectInformation objectInformation;

        public void Init(ObjectPool objectPool, ObjectInformation objectInformation)
        { 
            this.objectPool = objectPool;
            this.objectInformation = objectInformation;


        }

        protected virtual void OnDisable()
        {
            objectPool.ReturnObject(objectInformation);
        }
    }
}
