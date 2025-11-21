using Match3.Cmd;
using UnityEngine;
using System.Collections.Generic;
using Zenject;
using Match3.PlayingField;

namespace Match3.Inputs
{
    /// <summary>
    /// Пользовательский ввод
    /// отвечает за ввод команд
    /// </summary>
    public class Match3UserInput : MonoBehaviour
    {
        private Match3Manager _match3Manager;
        private IMatch3PlayingField _match3PlayingField;

        #region Очереди команд
        //Все эти списки нужно вывести в отдельный класс
        //и gd будет править их из конфига 

        [SerializeField, Tooltip("Создание игрового поля")] 
        private List<Match3CmdBase> _createField;

        [SerializeField, Tooltip("Меняет элементы местами")]
        private List<Match3CmdBase> _swapPlaces;

        [SerializeField, Tooltip("Ошибка смены элементов")]
        private List<Match3CmdBase> _errorSwapPlaces;
        #endregion


        public Match3ItemField First { get => _first; }
        public Match3ItemField Last { get => _last; }

        [SerializeField] private Match3ItemField _first;
        [SerializeField] private Match3ItemField _last;

        [Inject]
        private void Construct(
            Match3Manager match3Manager,
            IMatch3PlayingField match3PlayingField)
        {
            _match3Manager = match3Manager;
            _match3PlayingField = match3PlayingField;
        }

        private async void Start()
        {
            while (_match3Manager.CurrentMatch3State != Match3State.WaitingUserInput) 
                await System.Threading.Tasks.Task.Yield();


            _match3Manager.SetQueueCommand(new Queue<Match3CmdBase>(_createField));
        }

        private void Update()
        {
            if (_match3Manager.CurrentMatch3State != Match3State.WaitingUserInput) return;
            FindTarget();
        }

        private void FindTarget()
        {
            //Реализуем простой механизм выбора элементов
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(
                    Camera.main.ScreenPointToRay(Input.mousePosition),
                    out var hit))
                {
                    if (hit.collider.TryGetComponent<Match3ItemField>(out var item))
                    {
                        if (First is null)
                        {
                            _first = item;
                            First.Enter();
                            return;
                        }
                        else
                        {
                            //Разные элементы пытаемся поменять местами
                            First.Exit();
                            if (First != item)
                            {
                                //Проверяем расположение элементов рядом или нет
                                if (_match3PlayingField.PlayingField.AreNeighbors(First, item))
                                {
                                    _last = item;

                                    _match3PlayingField.SwapPlaces(ref _first, ref _last);

                                    //Проверка совпадений после перемещения
                                    //Это всё нужно перенести в команду и прерывать ее, но в рамках тестового много реализации
                                    if (_match3PlayingField.FindAllMatches().Count > 0)
                                    {
                                        print("Обмен");
                                        _match3Manager.SetQueueCommand(new Queue<Match3CmdBase>(_swapPlaces));
                                    }
                                    else
                                    {
                                        print("Ошибка обмена");
                                        _match3PlayingField.SwapPlaces(ref _first, ref _last);
                                        _match3Manager.SetQueueCommand(new Queue<Match3CmdBase>(_errorSwapPlaces));
                                    }
                                }
                                else
                                {
                                    CleatTarget();
                                }
                            }
                            else
                            {
                                CleatTarget();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Уберает цель
        /// </summary>
        public void CleatTarget()
        {
            _first = null;
            _last = null;
        }
    }
}
