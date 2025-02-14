using UnityEngine;

namespace UnityHelp.Math
{
    public static class VectorExtensions
    {
        // -----------------------
        // Vector2 ���� Ȯ�� ���
        // -----------------------

        /// <summary>
        /// �� Vector2 ������ ������ (unsigned, �� ����) ����մϴ�.
        /// <para>��� ����: AI ĳ���Ͱ� �÷��̾���� ����� ���� ���̸� �Ǵ��Ͽ� �þ߰� ���� ���� �ִ��� ������ �� ���˴ϴ�.</para>
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
        /// �� Vector2 ������ ��ȣ�� �ִ� ������ ����մϴ�.
        /// ����̸� a���� b�� �� �� �ݽð� ����, �����̸� �ð� �����Դϴ�.
        /// <para>��� ����: ĳ���Ͱ� Ÿ���� ���� ȸ���� �� ȸ���� ����(�ð�/�ݽð�)�� �����ϴ� ������ ���˴ϴ�.</para>
        /// </summary>
        public static float SignedAngle(this Vector2 from, Vector2 to)
        {
            float angle = from.AngleBetween(to);
            // 2D ���� (��Į�� ��)�� �̿��Ͽ� ��ȣ ����
            float cross = from.x * to.y - from.y * to.x;
            return angle * Mathf.Sign(cross);
        }

        /// <summary>
        /// Vector2 a�� b �������� ������ ��� ���͸� ��ȯ�մϴ�.
        /// <para>��� ����: ĳ������ �̵� ���͸� �����̳� Ư�� �������� �����Ͽ� �ڿ������� �̵��� ������ �� ���˴ϴ�.</para>
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
        /// �� Vector2�� �̷�� ����纯���� ����(���밪, 2D ���� ��Į�� ��)�� ��ȯ�մϴ�.
        /// <para>��� ����: 2D ���ӿ��� �� ���͸� ����� ��ġ �����̳� �浹 ������ ũ�⸦ ����� �� Ȱ���� �� �ֽ��ϴ�.</para>
        /// </summary>
        public static float ParallelogramArea(this Vector2 a, Vector2 b)
        {
            return Mathf.Abs(a.x * b.y - a.y * b.x);
        }

        /// <summary>
        /// �� Vector2�� ��������(���� ����� 0�� �������) �Ǵ��մϴ�.
        /// <para>��� ����: ��ֹ��� ����� �̵� ��ΰ� �������� Ȯ���Ͽ�, ĳ������ �̵� ��θ� �����ϰų� �浹 ó���� ������ �� ���˴ϴ�.</para>
        /// </summary>
        public static bool IsParallelTo(this Vector2 a, Vector2 b, float tolerance = 1e-6f)
        {
            return Mathf.Abs(a.x * b.y - a.y * b.x) < tolerance;
        }

        /// <summary>
        /// �� Vector2�� ��������(������ 0�� �������) �Ǵ��մϴ�.
        /// <para>��� ����: �浹 �� ĳ������ �̵� ����� ������ �������� �Ǵ��Ͽ�, �ݻ� ȿ���� ƨ�� ȿ���� ������ �� ���˴ϴ�.</para>
        /// </summary>
        public static bool IsPerpendicularTo(this Vector2 a, Vector2 b, float tolerance = 1e-6f)
        {
            return Mathf.Abs(Vector2.Dot(a, b)) < tolerance;
        }

        /// <summary>
        /// 2D ���͸� ������ ����(�� ����)��ŭ ȸ����ŵ�ϴ�.
        /// <para>��� ����: 2D ���� ���ӿ��� �߻�ü�� �ʱ� ������ ȸ����Ű�ų�, UI �������� ȸ�� �ִϸ��̼� ȿ���� Ȱ��˴ϴ�.</para>
        /// </summary>
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float rad = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }

        /// <summary>
        /// Vector2 a���� b ������ ����(����)�� ������ �ź�(������) ���͸� ��ȯ�մϴ�.
        /// <para>��� ����: �浹 �� ������ ���� ������ ���� ���ۿ� �Ǵ� �̲����� ȿ���� ������ �� ���˴ϴ�.</para>
        /// </summary>
        public static Vector2 Rejection(this Vector2 a, Vector2 b)
        {
            Vector2 proj = a.ProjectOnto(b);
            return a - proj;
        }

        /// <summary>
        /// 2D ���� v�� �־��� ���� normal�� ���� �ݻ�(reflect)��Ų ��� ���͸� ��ȯ�մϴ�.
        /// <para>��� ����: �Ѿ��̳� �������� ǥ�鿡 �ε����� �� �ݻ�Ǿ� ƨ�ܳ����� ȿ���� ������ �� Ȱ��˴ϴ�.</para>
        /// </summary>
        public static Vector2 Reflect(this Vector2 v, Vector2 normal)
        {
            // �ݻ� ����: v - 2 * (v dot normal) * normal
            return v - 2 * Vector2.Dot(v, normal) * normal;
        }

        /// <summary>
        /// ��(this ����)�� linePoint1, linePoint2�� ���ǵ� �� ������ �ִ� �Ÿ��� ����մϴ�.
        /// <para>��� ����: ĳ���Ͱ� Ư�� ���(��: ���� �Ǵ� Ʈ��)���� ��� ������ �����Ͽ� ����(snap) ��ɿ� Ȱ���� �� ���˴ϴ�.</para>
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
        /// �־��� 2D ���Ϳ� ������ ���͸� ��ȯ�մϴ�.
        /// <para>��� ����: 2D ���� ��꿡�� ���� ���� �Ǵ� ���� ��꿡 Ȱ��Ǿ�, ǥ���� ��糪 ȸ�� ȿ���� ������ �� ���˴ϴ�.</para>
        /// </summary>
        public static Vector2 Perpendicular(this Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }

        // -----------------------
        // Vector3 ���� Ȯ�� ���
        // -----------------------

        /// <summary>
        /// �� Vector3 ������ ������ (unsigned, �� ����) ����մϴ�.
        /// <para>��� ����: 3D ���ӿ��� �� ��ü ���� ���� ���̸� ����ϰų�, ī�޶��� ȸ�� �� Ÿ���� �ý��ۿ� Ȱ��˴ϴ�.</para>
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
        /// Vector3 a�� b �������� ������ ��� ���͸� ��ȯ�մϴ�.
        /// <para>��� ����: ���̳� �ӵ��� Ư�� �� �Ǵ� ��鿡 ���� ������ ��, ���� ��꿡 Ȱ��˴ϴ�.</para>
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
        /// �� Vector3�� ��������(������ ũ�Ⱑ 0�� �������) �Ǵ��մϴ�.
        /// <para>��� ����: 3D ȯ�濡�� ��ü�� ���� ���¸� Ȯ���ϰų�, �浹 ó�� �� ���� ���θ� �˻��� �� ���˴ϴ�.</para>
        /// </summary>
        public static bool IsParallelTo(this Vector3 a, Vector3 b, float tolerance = 1e-6f)
        {
            return Vector3.Cross(a, b).sqrMagnitude < tolerance;
        }

        /// <summary>
        /// �� Vector3�� ��������(������ 0�� �������) �Ǵ��մϴ�.
        /// <para>��� ����: 3D �浹 �����̳� �ݻ� ȿ���� ������ ��, ��ü�� ���� ���θ� Ȯ���ϴ� �� ���˴ϴ�.</para>
        /// </summary>
        public static bool IsPerpendicularTo(this Vector3 a, Vector3 b, float tolerance = 1e-6f)
        {
            return Mathf.Abs(Vector3.Dot(a, b)) < tolerance;
        }

        /// <summary>
        /// �� Vector3�� �̷�� ����纯���� ����(���� ������ ũ��)�� ��ȯ�մϴ�.
        /// <para>��� ����: 3D ���ӿ��� ����� ���̸� �����ϰų�, �������� �浹 ��꿡 Ȱ���� �� ���˴ϴ�.</para>
        /// </summary>
        public static float ParallelogramArea(this Vector3 a, Vector3 b)
        {
            return Vector3.Cross(a, b).magnitude;
        }

        /// <summary>
        /// Vector3 a���� b ������ ����(����)�� ������ �ź�(������) ���͸� ��ȯ�մϴ�.
        /// <para>��� ����: 3D ���� ��꿡�� �浹 �� ���� ���� ������ ���� ���ۿ� ���͸� ����� �� ���˴ϴ�.</para>
        /// </summary>
        public static Vector3 Rejection(this Vector3 a, Vector3 b)
        {
            Vector3 proj = a.ProjectOnto(b);
            return a - proj;
        }

        /// <summary>
        /// ��(this ����)�� linePoint1, linePoint2�� ���ǵ� 3D �� ������ �ִ� �Ÿ��� ����մϴ�.
        /// <para>��� ����: 3D ȯ�濡�� ��ü�� Ư�� ���(��: ����, Ʈ��)�κ��� ��� ������ ������ �� Ȱ��˴ϴ�.</para>
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
        /// 3D ��������, ������ ȸ�� ��(axis)�� �������� �� ���� ������ ��ȣ �ִ� ������ ����մϴ�.
        /// <para>��� ����: 3D ĳ���Ͱ� Ư�� ��(��: Y��)�� �������� Ÿ���� ���� ȸ���� ��, ȸ�� ����� ������ �����ϴ� �� ���˴ϴ�.</para>
        /// </summary>
        public static float SignedAngle(this Vector3 from, Vector3 to, Vector3 axis)
        {
            float unsignedAngle = AngleBetween(from, to);
            float sign = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(from, to)));
            return unsignedAngle * sign;
        }
    }
}
