using UnityEngine;
using UnityEngine.Events;

namespace Match3.PlayingField
{
    /// <summary>
    /// Элемент игрового поля
    /// </summary>
    public class Match3ItemFieldBlue : Match3ItemField
    {
        public override Color GetColor() => Color.blue;
    }
}
