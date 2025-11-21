using System.Collections.Generic;

namespace Match3.PlayingField
{
    public interface IMatch3PlayingField
    {
        public Match3ItemField[,] PlayingField { get; }
        void CreateField(int w, int h);
        void Move(ref Match3ItemField match3ItemField, int x, int y);
        void SwapPlaces(ref Match3ItemField itemX, ref Match3ItemField itemY);
        public List<Index2D> FindAllMatches();
    }
}
