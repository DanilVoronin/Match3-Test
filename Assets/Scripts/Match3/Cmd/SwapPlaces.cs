using DG.Tweening;
using UnityEngine;
using Zenject;
using Match3.Inputs;

namespace Match3.Cmd
{
    /// <summary>
    /// Заполяет поле новыми игровыми объектами
    /// </summary>
    [CreateAssetMenu(fileName = "SwapPlaces", menuName = "Match3Cmd/SwapPlaces")]
    public class SwapPlaces : Match3CmdBase
    {

        [field: SerializeField, Tooltip("Скорость анимации")]
        public float Speed { get; private set; }


        private Match3UserInput _match3UserInput;

        [Inject]
        private void Construct(Match3UserInput match3UserInput)
        {
            _match3UserInput = match3UserInput;
        }

        public override void Execute(CmdCallback cmdCallback)
        {
            Sequence sequence = DOTween.Sequence();

            Sequence item1 = DOTween.Sequence();
            item1.Append(_match3UserInput.First.transform.DOMove(_match3UserInput.Last.transform.position, Speed));

            Sequence item2 = DOTween.Sequence();
            item2.Append(_match3UserInput.Last.transform.DOMove(_match3UserInput.First.transform.position, Speed));

            sequence.Join(item1);
            sequence.Join(item2);

            sequence.OnComplete(() => cmdCallback?.Invoke(this));

            _match3UserInput.CleatTarget();
        }
    }
}
