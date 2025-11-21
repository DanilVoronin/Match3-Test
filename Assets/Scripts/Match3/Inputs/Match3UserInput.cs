using Match3.Cmd;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace Match3.Inputs
{
    /// <summary>
    /// Пользовательский ввод
    /// отвечает за ввод команд
    /// </summary>
    public class Match3UserInput : MonoBehaviour
    {
        private Match3Manager _match3Manager;

        //Надо создавать
        [SerializeField]
        private FillField _fillField;

        [Inject]
        private void Construct(Match3Manager match3Manager)
        {
            _match3Manager = match3Manager;
        }

        private async void Start()
        {
            while (_match3Manager.CurrentMatch3State != Match3State.WaitingUserInput) 
                await System.Threading.Tasks.Task.Yield();


            _match3Manager.SetQueueCommand(new Queue<Match3CmdBase>(new List<Match3CmdBase>() { _fillField }));
        }
    }
}
