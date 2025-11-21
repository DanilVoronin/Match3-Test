using Match3.PlayingField;
using ObjectPools;
using Zenject;
using UnityEngine;
using Match3.Inputs;

namespace Match3.Installers
{
    public class Match3Installer : MonoInstaller
    {
        [SerializeField] private Match3Manager _match3Manager;
        [SerializeField] private ManagerPools _managerPools;
        [SerializeField] private Match3UserInput _match3UserInput;

        public override void InstallBindings()
        {
            Container.Bind<IMatch3PlayingField>().To<Match3PlayingField>().AsSingle();

            Container.Bind<ManagerPools>().FromInstance(_managerPools).AsSingle();
            Container.Bind<Match3Manager>().FromInstance(_match3Manager).AsSingle();
            Container.Bind<Match3UserInput>().FromInstance(_match3UserInput).AsSingle();
        }
    }
}
