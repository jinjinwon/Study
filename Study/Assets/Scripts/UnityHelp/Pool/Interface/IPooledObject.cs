namespace UnityHelp.Pool
{
    public interface IPooledObject
    {
        /// <summary>
        /// 오브젝트가 풀에서 스폰될 때 호출됩니다.
        /// </summary>
        void OnSpawn();

        /// <summary>
        /// 오브젝트가 풀에 반환될 때 호출됩니다.
        /// </summary>
        void OnReturn();
    }
}