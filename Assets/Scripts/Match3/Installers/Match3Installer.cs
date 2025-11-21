using Match3.PlayingField;
using ObjectPools;
using Zenject;
using UnityEngine;

namespace Match3.Installers
{
    public class Match3Installer : MonoInstaller
    {
        [SerializeField] private Match3Manager _match3Manager;
        [SerializeField] private ManagerPools _managerPools;

        public override void InstallBindings()
        {
            Container.Bind<IMatch3PlayingField>().To<Match3PlayingField>().AsSingle();

            Container.Bind<ManagerPools>().FromInstance(_managerPools).AsSingle();
            Container.Bind<Match3Manager>().FromInstance(_match3Manager).AsSingle();
        }
    }
}
