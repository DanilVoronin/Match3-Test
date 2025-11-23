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
            Match3ItemField[,] oldField = field.Clone() as Match3ItemField[,];
            Match3ItemField[,] newField = Match3FieldGenerator.FillField<Match3ItemField>(oldField, PrefabList);

            int n = 1;
            for (int y = 0; y < newField.GetLength(1); y++)
            {
                for (int x = 0; x < newField.GetLength(1); x++)
                {
                    if (newField[x, y] != null)
                    {
                        Sequence sequenceItem = DOTween.Sequence();

                        Match3ItemField item = _prefabDictionary[newField[x, y]].GetObject<Match3ItemField>().Type as Match3ItemField;
                        field[x, y] = item;

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
    }

    public static class Match3FieldGenerator
    {
        public static T[,] FillField<T>(T[,] field, List<T> availableObjects)
        {
            if (field == null || availableObjects == null || availableObjects.Count == 0)
                return null;


            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            T[,] newT = new T[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Если ячейка уже заполнена, пропускаем
                    if (field[i, j] != null && !field[i, j].Equals(default(T)))
                        continue;

                    // Получаем случайный объект, исключая те, что создают комбинации
                    newT[i,j] = GetRandomObjectWithoutMatches(field, availableObjects, i, j);
                    field[i, j] = newT[i, j];
                }
            }

            return newT;
        }

        private static T GetRandomObjectWithoutMatches<T>(T[,] field, List<T> availableObjects, int row, int col)
        {
            int maxAttempts = 50; // Защита от бесконечного цикла
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                T candidate = availableObjects[Random.Range(0, availableObjects.Count)];

                // Проверяем, не создает ли кандидат комбинацию
                if (!CreatesHorizontalMatch(field, candidate, row, col) &&
                    !CreatesVerticalMatch(field, candidate, row, col))
                {
                    return candidate;
                }

                attempts++;
            }

            // Если не удалось найти подходящий объект, возвращаем случайный
            return availableObjects[Random.Range(0, availableObjects.Count)];
        }

        private static bool CreatesHorizontalMatch<T>(T[,] field, T candidate, int row, int col)
        {
            // Проверяем слева
            if (col >= 2)
            {
                T left1 = field[row, col - 1];
                T left2 = field[row, col - 2];

                if (!IsDefault(left1) && !IsDefault(left2) &&
                    left1.GetType() == left2.GetType() && left1.GetType() == candidate.GetType())
                    return true;
            }

            // Проверяем справа
            int cols = field.GetLength(1);
            if (col <= cols - 3)
            {
                T right1 = field[row, col + 1];
                T right2 = field[row, col + 2];

                if (!IsDefault(right1) && !IsDefault(right2) &&
                    right1.GetType() == right2.GetType() && right1.GetType() == candidate.GetType())
                    return true;
            }

            // Проверяем с обеих сторон
            if (col > 0 && col < cols - 1)
            {
                T left = field[row, col - 1];
                T right = field[row, col + 1];

                if (!IsDefault(left) && !IsDefault(right) &&
                    left.GetType() == right.GetType() && left.GetType() == candidate.GetType())
                    return true;
            }

            return false;
        }

        private static bool CreatesVerticalMatch<T>(T[,] field, T candidate, int row, int col)
        {
            // Проверяем сверху
            if (row >= 2)
            {
                T top1 = field[row - 1, col];
                T top2 = field[row - 2, col];

                if (!IsDefault(top1) && !IsDefault(top2) &&
                    top1.GetType() == top2.GetType() && top1.GetType() == candidate.GetType())
                    return true;
            }

            // Проверяем снизу
            int rows = field.GetLength(0);
            if (row <= rows - 3)
            {
                T bottom1 = field[row + 1, col];
                T bottom2 = field[row + 2, col];

                if (!IsDefault(bottom1) && !IsDefault(bottom2) &&
                    bottom1.GetType() == bottom2.GetType() && bottom1.GetType() == candidate.GetType())
                    return true;
            }

            // Проверяем с обеих сторон по вертикали
            if (row > 0 && row < rows - 1)
            {
                T top = field[row - 1, col];
                T bottom = field[row + 1, col];

                if (!IsDefault(top) && !IsDefault(bottom) &&
                    top.GetType() == bottom.GetType() && top.GetType() == candidate.GetType())
                    return true;
            }

            return false;
        }

        private static bool IsDefault<T>(T obj)
        {
            return obj == null || obj.Equals(default(T));
        }
    }
}
