using UnityEngine;

// �������� ���� �� �ִ� Ÿ�Ե��� ���������� ������ �ϴ� �������̽�
public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}