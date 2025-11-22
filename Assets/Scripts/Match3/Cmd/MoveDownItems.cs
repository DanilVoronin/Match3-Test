using DG.Tweening;
using UnityEngine;
using Zenject;
using Match3.PlayingField;

namespace Match3.Cmd
{
    /// <summary>
    /// Перемещает элементы в пустые клетки
    /// </summary>
    [CreateAssetMenu(fileName = "MoveDownItems", menuName = "Match3Cmd/MoveDownItems")]
    public class MoveDownItems : Match3CmdBase
    {

        [field: SerializeField, Tooltip("Скорость анимации")]
        public float Speed { get; private set; }

        private IMatch3PlayingField _match3PlayingField;

        [Inject]
        private void Construct(IMatch3PlayingField match3PlayingField)
        {
            _match3PlayingField = match3PlayingField;
        }

        public override void Execute(CmdCallback cmdCallback)
        {
            Sequence sequence = DOTween.Sequence();

            for (int y = 1; y < _match3PlayingField.PlayingField.GetLength(1); y++)
            {
                for (int x = 0; x < _match3PlayingField.PlayingField.GetLength(0); x++)
                {
                    if (_match3PlayingField.PlayingField[x, y] != null &&
                        _match3PlayingField.PlayingField[x, y - 1] == null)
                    {
                        int down = y - 1;
                        while (down >= 0 && _match3PlayingField.PlayingField[x, down] == null)
                        {
                            down--;
                        }
                        if (down < 0) down = 0;
                        if (_match3PlayingField.PlayingField[x, down] != null) down++;

                        Match3ItemField match3ItemField = _match3PlayingField.PlayingField[x, y];

                        _match3PlayingField.PlayingField[ x, down] = _match3PlayingField.PlayingField[x, y];
                        _match3PlayingField.PlayingField[x, y] = null;

                        Vector2 pos = new Vector2(x,down);

                        sequence.Join(match3ItemField.transform.DOMove(pos, Speed));
                    }
                }
            }
            sequence.OnComplete(() => cmdCallback?.Invoke(this));
        }
    }
}
