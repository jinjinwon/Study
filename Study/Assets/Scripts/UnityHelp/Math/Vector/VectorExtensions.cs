using UnityEngine;

namespace UnityHelp.Math
{
    public static class VectorExtensions
    {
        // -----------------------
        // Vector2 관련 확장 기능
        // -----------------------

        /// <summary>
        /// 두 Vector2 사이의 각도를 (unsigned, 도 단위) 계산합니다.
        /// <para>사용 예시: AI 캐릭터가 플레이어와의 상대적 방향 차이를 판단하여 시야각 범위 내에 있는지 결정할 때 사용됩니다.</para>
        /// </summary>
        public static float AngleBetween(this Vector2 a, Vector2 b)
        {
            float dot = Vector2.Dot(a, b);
            float magProduct = a.magnitude * b.magnitude;
            if (magProduct < Mathf.Epsilon)
                return 0f;
            return Mathf.Acos(Mathf.Clamp(dot / magProduct, -1f, 1f)) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 두 Vector2 사이의 부호가 있는 각도를 계산합니다.
        /// 양수이면 a에서 b로 갈 때 반시계 방향, 음수이면 시계 방향입니다.
        /// <para>사용 예시: 캐릭터가 타겟을 향해 회전할 때 회전의 방향(시계/반시계)을 결정하는 로직에 사용됩니다.</para>
        /// </summary>
        public static float SignedAngle(this Vector2 from, Vector2 to)
        {
            float angle = from.AngleBetween(to);
            // 2D 외적 (스칼라 값)를 이용하여 부호 결정
            float cross = from.x * to.y - from.y * to.x;
            return angle * Mathf.Sign(cross);
        }

        /// <summary>
        /// Vector2 a를 b 방향으로 투영한 결과 벡터를 반환합니다.
        /// <para>사용 예시: 캐릭터의 이동 벡터를 경사면이나 특정 방향으로 제한하여 자연스러운 이동을 구현할 때 사용됩니다.</para>
        /// </summary>
        public static Vector2 ProjectOnto(this Vector2 a, Vector2 b)
        {
            float dot = Vector2.Dot(a, b);
            float magSqr = b.sqrMagnitude;
            if (magSqr < Mathf.Epsilon)
                return Vector2.zero;
            return (dot / magSqr) * b;
        }

        /// <summary>
        /// 두 Vector2가 이루는 평행사변형의 면적(절대값, 2D 외적 스칼라 값)을 반환합니다.
        /// <para>사용 예시: 2D 게임에서 두 벡터를 사용해 터치 영역이나 충돌 영역의 크기를 계산할 때 활용할 수 있습니다.</para>
        /// </summary>
        public static float ParallelogramArea(this Vector2 a, Vector2 b)
        {
            return Mathf.Abs(a.x * b.y - a.y * b.x);
        }

        /// <summary>
        /// 두 Vector2가 평행한지(외적 결과가 0에 가까운지) 판단합니다.
        /// <para>사용 예시: 장애물의 방향과 이동 경로가 평행한지 확인하여, 캐릭터의 이동 경로를 보정하거나 충돌 처리를 결정할 때 사용됩니다.</para>
        /// </summary>
        public static bool IsParallelTo(this Vector2 a, Vector2 b, float tolerance = 1e-6f)
        {
            return Mathf.Abs(a.x * b.y - a.y * b.x) < tolerance;
        }

        /// <summary>
        /// 두 Vector2가 수직인지(내적이 0에 가까운지) 판단합니다.
        /// <para>사용 예시: 충돌 시 캐릭터의 이동 방향과 벽면이 수직인지 판단하여, 반사 효과나 튕김 효과를 적용할 때 사용됩니다.</para>
        /// </summary>
        public static bool IsPerpendicularTo(this Vector2 a, Vector2 b, float tolerance = 1e-6f)
        {
            return Mathf.Abs(Vector2.Dot(a, b)) < tolerance;
        }

        /// <summary>
        /// 2D 벡터를 지정한 각도(도 단위)만큼 회전시킵니다.
        /// <para>사용 예시: 2D 슈팅 게임에서 발사체의 초기 방향을 회전시키거나, UI 아이콘의 회전 애니메이션 효과에 활용됩니다.</para>
        /// </summary>
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float rad = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }

        /// <summary>
        /// Vector2 a에서 b 방향의 성분(투영)을 제거한 거부(반투영) 벡터를 반환합니다.
        /// <para>사용 예시: 충돌 후 벡터의 수직 성분을 구해 반작용 또는 미끄러짐 효과를 적용할 때 사용됩니다.</para>
        /// </summary>
        public static Vector2 Rejection(this Vector2 a, Vector2 b)
        {
            Vector2 proj = a.ProjectOnto(b);
            return a - proj;
        }

        /// <summary>
        /// 2D 벡터 v를 주어진 법선 normal에 대해 반사(reflect)시킨 결과 벡터를 반환합니다.
        /// <para>사용 예시: 총알이나 레이저가 표면에 부딪혔을 때 반사되어 튕겨나가는 효과를 구현할 때 활용됩니다.</para>
        /// </summary>
        public static Vector2 Reflect(this Vector2 v, Vector2 normal)
        {
            // 반사 공식: v - 2 * (v dot normal) * normal
            return v - 2 * Vector2.Dot(v, normal) * normal;
        }

        /// <summary>
        /// 점(this 벡터)과 linePoint1, linePoint2로 정의된 선 사이의 최단 거리를 계산합니다.
        /// <para>사용 예시: 캐릭터가 특정 경로(예: 도로 또는 트랙)에서 벗어난 정도를 측정하여 스냅(snap) 기능에 활용할 때 사용됩니다.</para>
        /// </summary>
        public static float DistanceToLine(this Vector2 point, Vector2 linePoint1, Vector2 linePoint2)
        {
            Vector2 lineDir = linePoint2 - linePoint1;
            float area = Mathf.Abs((point - linePoint1).x * lineDir.y - (point - linePoint1).y * lineDir.x);
            float baseLength = lineDir.magnitude;
            if (baseLength < Mathf.Epsilon)
                return 0f;
            return area / baseLength;
        }

        /// <summary>
        /// 주어진 2D 벡터와 수직인 벡터를 반환합니다.
        /// <para>사용 예시: 2D 물리 계산에서 접선 방향 또는 법선 계산에 활용되어, 표면의 경사나 회전 효과를 구현할 때 사용됩니다.</para>
        /// </summary>
        public static Vector2 Perpendicular(this Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }

        // -----------------------
        // Vector3 관련 확장 기능
        // -----------------------

        /// <summary>
        /// 두 Vector3 사이의 각도를 (unsigned, 도 단위) 계산합니다.
        /// <para>사용 예시: 3D 게임에서 두 객체 간의 방향 차이를 계산하거나, 카메라의 회전 및 타겟팅 시스템에 활용됩니다.</para>
        /// </summary>
        public static float AngleBetween(this Vector3 a, Vector3 b)
        {
            float dot = Vector3.Dot(a, b);
            float magProduct = a.magnitude * b.magnitude;
            if (magProduct < Mathf.Epsilon)
                return 0f;
            return Mathf.Acos(Mathf.Clamp(dot / magProduct, -1f, 1f)) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Vector3 a를 b 방향으로 투영한 결과 벡터를 반환합니다.
        /// <para>사용 예시: 힘이나 속도를 특정 축 또는 평면에 맞춰 분해할 때, 물리 계산에 활용됩니다.</para>
        /// </summary>
        public static Vector3 ProjectOnto(this Vector3 a, Vector3 b)
        {
            float dot = Vector3.Dot(a, b);
            float magSqr = b.sqrMagnitude;
            if (magSqr < Mathf.Epsilon)
                return Vector3.zero;
            return (dot / magSqr) * b;
        }

        /// <summary>
        /// 두 Vector3가 평행한지(외적의 크기가 0에 가까운지) 판단합니다.
        /// <para>사용 예시: 3D 환경에서 객체의 정렬 상태를 확인하거나, 충돌 처리 시 평행 여부를 검사할 때 사용됩니다.</para>
        /// </summary>
        public static bool IsParallelTo(this Vector3 a, Vector3 b, float tolerance = 1e-6f)
        {
            return Vector3.Cross(a, b).sqrMagnitude < tolerance;
        }

        /// <summary>
        /// 두 Vector3가 수직인지(내적이 0에 가까운지) 판단합니다.
        /// <para>사용 예시: 3D 충돌 판정이나 반사 효과를 결정할 때, 객체의 수직 여부를 확인하는 데 사용됩니다.</para>
        /// </summary>
        public static bool IsPerpendicularTo(this Vector3 a, Vector3 b, float tolerance = 1e-6f)
        {
            return Mathf.Abs(Vector3.Dot(a, b)) < tolerance;
        }

        /// <summary>
        /// 두 Vector3가 이루는 평행사변형의 면적(외적 벡터의 크기)을 반환합니다.
        /// <para>사용 예시: 3D 게임에서 평면의 넓이를 추정하거나, 기하학적 충돌 계산에 활용할 때 사용됩니다.</para>
        /// </summary>
        public static float ParallelogramArea(this Vector3 a, Vector3 b)
        {
            return Vector3.Cross(a, b).magnitude;
        }

        /// <summary>
        /// Vector3 a에서 b 방향의 성분(투영)을 제거한 거부(반투영) 벡터를 반환합니다.
        /// <para>사용 예시: 3D 물리 계산에서 충돌 후 남은 수직 성분을 구해 반작용 벡터를 계산할 때 사용됩니다.</para>
        /// </summary>
        public static Vector3 Rejection(this Vector3 a, Vector3 b)
        {
            Vector3 proj = a.ProjectOnto(b);
            return a - proj;
        }

        /// <summary>
        /// 점(this 벡터)과 linePoint1, linePoint2로 정의된 3D 선 사이의 최단 거리를 계산합니다.
        /// <para>사용 예시: 3D 환경에서 객체가 특정 경로(예: 도로, 트랙)로부터 벗어난 정도를 측정할 때 활용됩니다.</para>
        /// </summary>
        public static float DistanceToLine(this Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
        {
            Vector3 lineDir = linePoint2 - linePoint1;
            float area = Vector3.Cross(point - linePoint1, lineDir).magnitude;
            float baseLength = lineDir.magnitude;
            if (baseLength < Mathf.Epsilon)
                return 0f;
            return area / baseLength;
        }

        /// <summary>
        /// 3D 공간에서, 지정된 회전 축(axis)을 기준으로 두 벡터 사이의 부호 있는 각도를 계산합니다.
        /// <para>사용 예시: 3D 캐릭터가 특정 축(예: Y축)을 기준으로 타겟을 향해 회전할 때, 회전 방향과 각도를 결정하는 데 사용됩니다.</para>
        /// </summary>
        public static float SignedAngle(this Vector3 from, Vector3 to, Vector3 axis)
        {
            float unsignedAngle = AngleBetween(from, to);
            float sign = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(from, to)));
            return unsignedAngle * sign;
        }
    }
}
