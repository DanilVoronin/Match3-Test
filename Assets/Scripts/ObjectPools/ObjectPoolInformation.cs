using UnityEngine;

namespace ObjectPools
{
    /// <summary>
    /// Информация о пуле объектов
    /// </summary>
    public class ObjectInformation
    {
        /// <summary>
        /// Игровой объект в пуле
        /// </summary>
        public GameObject GameObject;
        /// <summary>
        /// Объект конкретного типа что с ним связан
        /// </summary>
        public object Type;
    }
}
