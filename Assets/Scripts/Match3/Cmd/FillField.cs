using DG.Tweening;
using Match3.PlayingField;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ObjectPools;

namespace Match3.Cmd
{
    /// <summary>
    /// Заполяет поле новыми игровыми объектами
    /// </summary>
    [CreateAssetMenu(fileName = "FillField", menuName = "Match3Cmd/FillField")]
    public class FillField : Match3CmdBase
    {
        [field : SerializeField, Tooltip("Интервал между элементами")] 
        public float Interval { get; private set; }

        [field: SerializeField, Tooltip("Скорость появления")]
        public float Speed { get; private set; }

        [field: SerializeField, Tooltip("Список префабов")]
        public List<Match3ItemField> PrefabList { get; private set; }


        private IMatch3PlayingField _match3PlayingField;
        private Dictionary<Match3ItemField, ObjectPool> _prefabDictionary;
        
        [Inject]
        private void Construct(
            IMatch3PlayingField match3PlayingField,
            ManagerPools managerPools)
        {
            _match3PlayingField = match3PlayingField;
            _prefabDictionary = new Dictionary<Match3ItemField, ObjectPool>();

            foreach (Match3ItemField match3ItemField in PrefabList)
                _prefabDictionary.Add(match3ItemField, managerPools.GetObjectPoolByPrefab<Match3ItemField>(match3ItemField.gameObject));
        }

        public override void Execute(CmdCallback cmdCallback)
        {
            //Потом сделать проверку чтобы после 
            //заполнения были ходы

            Sequence sequence = DOTween.Sequence();

            Match3ItemField[,] field = _match3PlayingField.PlayingField;

            int n = 1;
            for (int y = 0; y < field.GetLength(1); y++)
            {
                for (int x = 0; x < field.GetLength(1); x++)
                {
                    if (field[x, y] == null)
                    {
                        Sequence sequenceItem = DOTween.Sequence();

                        Match3ItemField item = GetRandobMatch3ItemField();
                        item.transform.transform.position = new Vector2(x, y);
                        item.transform.transform.localScale = Vector3.zero;
                        item.gameObject.SetActive(true);

                        sequenceItem.AppendInterval(Interval * n);
                        sequenceItem.Append(item.transform.DOScale(Vector3.one, Speed));
                        sequence.Join(sequenceItem);
                        n++;
                    }
                }
            }
            sequence.OnComplete(() => cmdCallback?.Invoke(this));
        }

        private Match3ItemField GetRandobMatch3ItemField()
        {
            return _prefabDictionary[PrefabList[Random.Range(0, PrefabList.Count)]]
                   .GetObject<Match3ItemField>().Type as Match3ItemField;
        }
    }
}
