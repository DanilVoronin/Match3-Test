using UnityEngine;

namespace Match3.Cmd
{
    /// <summary>
    /// Базовая команда
    /// </summary>
    public abstract class Match3CmdBase : ScriptableObject
    {
        public delegate void CmdCallback(Match3CmdBase match3CmdBase);
        public abstract void Execute(CmdCallback cmdCallback);
    }
}
