using System.Collections.Generic;
using System.Linq;

namespace Match3.PlayingField
{
    /// <summary>
    /// Игровое поле
    /// </summary>
    public class Match3PlayingField : IMatch3PlayingField
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        public Match3ItemField[,] PlayingField { get => _playingField; }
        private Match3ItemField[,] _playingField;

        public void CreateField(int w, int h)
        {
            _playingField = new Match3ItemField[w, h];
        }
        public void Move(ref Match3ItemField match3ItemField, int x, int y)
        {
            if (_playingField[x, y] is null)
            {
                if (_playingField.GetPositionElement(match3ItemField,out var index) != null)
                {
                    _playingField[index.Value.x, index.Value.y] = null;
                }
                _playingField[x, y] = match3ItemField;
            }
        }
        public void SwapPlaces(ref Match3ItemField itemX, ref Match3ItemField itemY)
        {
            if (_playingField.GetPositionElement(itemX, out var indexItemX) != null &&
                _playingField.GetPositionElement(itemY, out var indexItemY) != null)
            {
                _playingField[indexItemX.Value.x, indexItemX.Value.y] = itemY;
                _playingField[indexItemY.Value.x, indexItemY.Value.y] = itemX;
            }
        }

        public List<Index2D> FindAllMatches()
        {
            List<Index2D> matches = new List<Index2D>();
            int width = _playingField.GetLength(0);
            int height = _playingField.GetLength(1);

            List<Index2D> row = new List<Index2D>();

            // Проверяем горизонтальные последовательности
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //В ряду есть элементы
                    if (row.Count > 0)
                    {
                        Index2D last = row.Last();
                        if (_playingField[last.x, last.y].Id != _playingField[x, y].Id)
                        {
                            if (row.Count > 2) AddItemsToList(matches,row);
                            row.Clear();
                        }
                    }
                    
                    row.Add(new Index2D(x,y));
                }
            }

            if (row.Count > 2) AddItemsToList(matches, row);
            row.Clear();

            // Проверяем вертикальные последовательности
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //В ряду есть элементы
                    if (row.Count > 0)
                    {
                        Index2D last = row.Last();
                        if (_playingField[last.x, last.y].Id != _playingField[x, y].Id)
                        {
                            if (row.Count > 2) AddItemsToList(matches, row);
                            row.Clear();
                        }
                    }

                    row.Add(new Index2D(x, y));
                }
            }

            if (row.Count > 2) AddItemsToList(matches, row);
            row.Clear();

            return matches;
        }

        private void AddItemsToList(List<Index2D> target, List<Index2D> row)
        {
            foreach (var item in row)
            {
                if (!target.Contains(item)) target.Add(item);
            }
        }
    }
}
