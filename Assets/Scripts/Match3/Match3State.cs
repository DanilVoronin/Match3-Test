namespace Match3
{
    /// <summary>
    /// Состояние Match3
    /// </summary>
    [System.Serializable]
    public enum Match3State
    {
        None,
        /// <summary>
        /// Инициализация
        /// </summary>
        Initialization,
        /// <summary>
        /// Ожидания инициали запуска игры
        /// </summary>
        WaitingGameStart,
        /// <summary>
        /// Запуск игры
        /// </summary>
        GameStart,
        /// <summary>
        /// Ожидание пользовательского ввода
        /// </summary>
        WaitingUserInput,
        /// <summary>
        /// Выполнение команды
        /// </summary>
        ExecutingCommand,
        /// <summary>
        /// Завершение выполнения команд
        /// </summary>
        CompletingExecutionCommands
    }
}
