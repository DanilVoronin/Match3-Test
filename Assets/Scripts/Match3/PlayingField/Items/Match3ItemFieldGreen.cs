using UnityEngine;
using UnityEngine.Events;

namespace Match3.PlayingField
{
    /// <summary>
    /// Элемент игрового поля
    /// </summary>
    public class Match3ItemFieldGreen : Match3ItemField
    {
        public override Color GetColor() => Color.green;
    }
}
