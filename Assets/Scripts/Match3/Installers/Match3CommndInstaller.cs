using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Match3.Cmd;

namespace Match3.Installers
{
    public class Match3CommndInstaller : MonoInstaller
    {
        [SerializeField] private List<Match3CmdBase> _commands;

        public override void InstallBindings()
        {
            foreach (Match3CmdBase match3CmdBase in _commands)
            {
                Container.Inject(match3CmdBase);
            }
        }
    }
}
