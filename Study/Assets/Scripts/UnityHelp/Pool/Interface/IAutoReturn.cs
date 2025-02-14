namespace UnityHelp.Pool
{
    public interface IAutoReturn
    {
        /// <summary>
        /// 오브젝트가 자동 반환되기까지의 지연 시간(초)입니다.
        /// </summary>
        float AutoReturnDelay { get; }

        /// <summary>
        /// 자동 반환 처리를 위한 초기화 작업을 수행합니다.
        /// (예를 들어, 코루틴을 시작하는 로직)
        /// </summary>
        void StartAutoReturn();
    }
}