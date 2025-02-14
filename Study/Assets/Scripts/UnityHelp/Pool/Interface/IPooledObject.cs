namespace UnityHelp.Pool
{
    public interface IPooledObject
    {
        /// <summary>
        /// ������Ʈ�� Ǯ���� ������ �� ȣ��˴ϴ�.
        /// </summary>
        void OnSpawn();

        /// <summary>
        /// ������Ʈ�� Ǯ�� ��ȯ�� �� ȣ��˴ϴ�.
        /// </summary>
        void OnReturn();
    }
}