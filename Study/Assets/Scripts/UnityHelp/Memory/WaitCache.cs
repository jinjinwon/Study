using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.Memory
{
    public static class WaitCache
    {
        // WaitForSeconds 캐싱: 초 단위의 값을 키로 사용
        private static readonly Dictionary<float, WaitForSeconds> waitForSecondsCache = new Dictionary<float, WaitForSeconds>();

        // WaitForSecondsRealtime 캐싱
        private static readonly Dictionary<float, WaitForSecondsRealtime> waitForSecondsRealtimeCache = new Dictionary<float, WaitForSecondsRealtime>();

        // 인자가 없는 대기 객체들은 한 번만 생성하여 재사용
        private static readonly WaitForFixedUpdate waitForFixedUpdateInstance = new WaitForFixedUpdate();
        private static readonly WaitForEndOfFrame waitForEndOfFrameInstance = new WaitForEndOfFrame();

        /// <summary>
        /// 지정한 시간(초)만큼 대기하는 WaitForSeconds 객체를 반환합니다.
        /// 이미 생성된 객체가 있으면 재사용합니다.
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
        /// 지정한 시간(초)만큼 대기하는 WaitForSecondsRealtime 객체를 반환합니다.
        /// 이미 생성된 객체가 있으면 재사용합니다.
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
        /// WaitForFixedUpdate 객체를 반환합니다.
        /// </summary>
        public static WaitForFixedUpdate WaitForFixedUpdate => waitForFixedUpdateInstance;

        /// <summary>
        /// WaitForEndOfFrame 객체를 반환합니다.
        /// </summary>
        public static WaitForEndOfFrame WaitForEndOfFrame => waitForEndOfFrameInstance;
    }
}
