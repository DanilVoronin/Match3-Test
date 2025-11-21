using UnityEngine;
using UnityEngine.Events;

namespace Match3.PlayingField
{
    /// <summary>
    /// Элемент игрового поля
    /// </summary>
    public class Match3ItemField : MonoBehaviour
    {
        [field : SerializeField, Tooltip("Id элемнта")]
        public string Id { get; private set; }

        [SerializeField] private UnityEvent OnEnter;
        [SerializeField] private UnityEvent OnExit;

        public void Enter() => OnEnter?.Invoke();
        public void Exit() => OnExit?.Invoke();
    }
}
