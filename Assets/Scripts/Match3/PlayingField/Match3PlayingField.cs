
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
                _playingField.GetPositionElement(itemX, out var indexItemY) != null)
            {
                _playingField[indexItemX.Value.x, indexItemX.Value.y] = itemY;
                _playingField[indexItemY.Value.x, indexItemY.Value.y] = itemX;
            }
        }
    }
}
