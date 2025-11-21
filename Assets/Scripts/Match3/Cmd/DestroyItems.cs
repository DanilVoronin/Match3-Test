using DG.Tweening;
using UnityEngine;
using Zenject;
using Match3.Inputs;
using Match3.PlayingField;
using System.Collections.Generic;

namespace Match3.Cmd
{
    /// <summary>
    /// Заполяет поле новыми игровыми объектами
    /// </summary>
    [CreateAssetMenu(fileName = "DestroyItems", menuName = "Match3Cmd/DestroyItems")]
    public class DestroyItems : Match3CmdBase
    {

        [field: SerializeField, Tooltip("Скорость анимации")]
        public float Speed { get; private set; }

        private Match3UserInput _match3UserInput;
        private IMatch3PlayingField _match3PlayingField;

        [Inject]
        private void Construct(
            Match3UserInput match3UserInput,
            IMatch3PlayingField match3PlayingField)
        {
            _match3UserInput = match3UserInput;
            _match3PlayingField = match3PlayingField;
        }

        public override void Execute(CmdCallback cmdCallback)
        {
            List<Index2D> list = _match3PlayingField.FindAllMatches();

            if (list.Count > 0)
            {
                Sequence sequence = DOTween.Sequence();

                foreach (Index2D index2D in list)
                {
                    Transform tr = _match3PlayingField.PlayingField[index2D.x, index2D.y].transform;

                    sequence.Join(tr.DOScale(Vector3.zero, Speed));
                    _match3PlayingField.PlayingField[index2D.x, index2D.y] = null;
                }

                sequence.OnComplete(() => cmdCallback?.Invoke(this));
            }
            else
            {
                cmdCallback?.Invoke(this);
            }
        }
    }
}
