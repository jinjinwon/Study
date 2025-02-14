namespace UnityHelp.Pool
{
    public interface IAutoReturn
    {
        /// <summary>
        /// ������Ʈ�� �ڵ� ��ȯ�Ǳ������ ���� �ð�(��)�Դϴ�.
        /// </summary>
        float AutoReturnDelay { get; }

        /// <summary>
        /// �ڵ� ��ȯ ó���� ���� �ʱ�ȭ �۾��� �����մϴ�.
        /// (���� ���, �ڷ�ƾ�� �����ϴ� ����)
        /// </summary>
        void StartAutoReturn();
    }
}