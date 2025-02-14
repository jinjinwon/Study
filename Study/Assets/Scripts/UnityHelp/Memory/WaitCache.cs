using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.Memory
{
    public static class WaitCache
    {
        // WaitForSeconds ĳ��: �� ������ ���� Ű�� ���
        private static readonly Dictionary<float, WaitForSeconds> waitForSecondsCache = new Dictionary<float, WaitForSeconds>();

        // WaitForSecondsRealtime ĳ��
        private static readonly Dictionary<float, WaitForSecondsRealtime> waitForSecondsRealtimeCache = new Dictionary<float, WaitForSecondsRealtime>();

        // ���ڰ� ���� ��� ��ü���� �� ���� �����Ͽ� ����
        private static readonly WaitForFixedUpdate waitForFixedUpdateInstance = new WaitForFixedUpdate();
        private static readonly WaitForEndOfFrame waitForEndOfFrameInstance = new WaitForEndOfFrame();

        /// <summary>
        /// ������ �ð�(��)��ŭ ����ϴ� WaitForSeconds ��ü�� ��ȯ�մϴ�.
        /// �̹� ������ ��ü�� ������ �����մϴ�.
        /// </summary>
        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (!waitForSecondsCache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                waitForSecondsCache[seconds] = wait;
            }
            return wait;
        }

        /// <summary>
        /// ������ �ð�(��)��ŭ ����ϴ� WaitForSecondsRealtime ��ü�� ��ȯ�մϴ�.
        /// �̹� ������ ��ü�� ������ �����մϴ�.
        /// </summary>
        public static WaitForSecondsRealtime GetWaitForSecondsRealtime(float seconds)
        {
            if (!waitForSecondsRealtimeCache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSecondsRealtime(seconds);
                waitForSecondsRealtimeCache[seconds] = wait;
            }
            return wait;
        }

        /// <summary>
        /// WaitForFixedUpdate ��ü�� ��ȯ�մϴ�.
        /// </summary>
        public static WaitForFixedUpdate WaitForFixedUpdate => waitForFixedUpdateInstance;

        /// <summary>
        /// WaitForEndOfFrame ��ü�� ��ȯ�մϴ�.
        /// </summary>
        public static WaitForEndOfFrame WaitForEndOfFrame => waitForEndOfFrameInstance;
    }
}
