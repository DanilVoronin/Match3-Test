using DG.Tweening;
using Match3.PlayingField;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ObjectPools;
using Match3.Inputs;

namespace Match3.Cmd
{
    /// <summary>
    /// Заполяет поле новыми игровыми объектами
    /// </summary>
    [CreateAssetMenu(fileName = "ErrorSwapPlaces", menuName = "Match3Cmd/ErrorSwapPlaces")]
    public class ErrorSwapPlaces : Match3CmdBase
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
            item1.Append(_match3UserInput.First.transform.DOMove(_match3UserInput.First.transform.position, Speed));

            Sequence item2 = DOTween.Sequence();
            item2.Append(_match3UserInput.Last.transform.DOMove(_match3UserInput.First.transform.position, Speed));
            item2.Append(_match3UserInput.Last.transform.DOMove(_match3UserInput.Last.transform.position, Speed));

            sequence.Append(item1);
            sequence.Append(item2);

            sequence.OnComplete(() => cmdCallback?.Invoke(this));
        }
    }
}
