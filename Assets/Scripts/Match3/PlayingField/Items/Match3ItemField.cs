using UnityEngine;
using UnityEngine.Events;

namespace Match3.PlayingField
{
    /// <summary>
    /// Элемент игрового поля
    /// </summary>
    public class Match3ItemField : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnEnter;
        [SerializeField] private UnityEvent OnExit;

        public void Enter() => OnEnter?.Invoke();
        public void Exit() => OnExit?.Invoke();

        public virtual Color GetColor() =>Color.black;
    }
}
