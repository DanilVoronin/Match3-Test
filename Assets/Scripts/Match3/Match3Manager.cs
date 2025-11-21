using Match3.Cmd;
using Match3.PlayingField;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Match3
{
    /// <summary>
    /// Манаджер игры
    /// </summary>
    public class Match3Manager : MonoBehaviour
    {
        //Эти значения нужно вывести в конфиг и
        //натсраивать в зависимости от уровн
        [Tooltip("Высота поля")]
        public int Height;
        [Tooltip("Ширина поля")]
        public int Width;

        /// <summary>
        /// Текущее состояние
        /// </summary>
        public Match3State CurrentMatch3State 
        {
            get => _currentMatch3State;
            private set
            {
                if (_currentMatch3State != value)
                {
                    switch (value)
                    {
                        case Match3State.Initialization:
                            if (_currentMatch3State is Match3State.None) { _currentMatch3State = value; return; }
                            break;
                        case Match3State.WaitingGameStart:
                            if (_currentMatch3State is Match3State.Initialization) { _currentMatch3State = value; return; }
                            break;
                        case Match3State.GameStart:
                            if (_currentMatch3State is Match3State.WaitingGameStart) { _currentMatch3State = value; return; }
                            break;
                        case Match3State.WaitingUserInput:
                            if (_currentMatch3State is Match3State.GameStart ||
                                _currentMatch3State is Match3State.CompletingExecutionCommands) { _currentMatch3State = value; return; }
                            break;
                        case Match3State.ExecutingCommand:
                            if (_currentMatch3State is Match3State.WaitingUserInput) { _currentMatch3State = value; return; }
                            break;
                        case Match3State.CompletingExecutionCommands:
                            if (_currentMatch3State is Match3State.ExecutingCommand) { _currentMatch3State = value; return; }
                            break;
                        default: break;
                    }
                }
            }
        }

        private IMatch3PlayingField _match3PlayingField;

        private Queue<Match3CmdBase> _commandQueue; //Это можно вынести в менедре команд

        [SerializeField] private Match3State _currentMatch3State;

        [Inject]
        private void Construct(IMatch3PlayingField match3PlayingField)
        {
            _match3PlayingField = match3PlayingField;
        }

        private void Start()
        {
            //Настройку и запуск можно сделать из вне, для теста запускаем сразу
            CurrentMatch3State = Match3State.Initialization;
        }

        private void Update()
        {
            switch (_currentMatch3State)
            {
                case Match3State.Initialization: Initialization(); break;
                case Match3State.WaitingGameStart: WaitingGameStart(); break;
                case Match3State.GameStart: GameStart(); break;
                case Match3State.WaitingUserInput: WaitingUserInput(); break;
                case Match3State.ExecutingCommand: ExecutingCommand(); break;
                case Match3State.CompletingExecutionCommands: CompletingExecutionCommands(); break;
                default: break;
            }
        }

        private void Initialization()
        {
            CurrentMatch3State = Match3State.WaitingGameStart;
        }
        private void WaitingGameStart()
        {
            CurrentMatch3State = Match3State.GameStart;
        }
        private void GameStart()
        {
            _match3PlayingField.CreateField(Width, Height);
            CurrentMatch3State = Match3State.WaitingUserInput;
        }
        private void WaitingUserInput()
        {

        }
        private void ExecutingCommand()
        {

        }
        private void CompletingExecutionCommands()
        {
            _currentMatch3State = Match3State.WaitingUserInput;
        }

        /// <summary>
        /// Команды принимаются только в состоянии ввода
        /// Даже если это стартовое создание поля
        /// </summary>
        public void SetQueueCommand(Queue<Match3CmdBase> queueList)
        {
            if (queueList is null ||
                queueList.Count == 0 ||
                _currentMatch3State != Match3State.WaitingUserInput) return;

            _commandQueue = queueList;
            _commandQueue.Dequeue().Execute(OnCompleteCommand);

            _currentMatch3State = Match3State.ExecutingCommand;
        }

        private void OnCompleteCommand(Match3CmdBase match3CmdBase)
        {
            if (_commandQueue != null && _commandQueue.Count > 0)
            {
                _commandQueue.Dequeue().Execute(OnCompleteCommand);
            }
            else
            { 
                _currentMatch3State = Match3State.CompletingExecutionCommands;
            }
        }
    }
}
