using UnityEngine;

// �� ������ ����� �¾� ������
[CreateAssetMenu(menuName = "Scriptable/EnemyData", fileName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float health = 100f; // ü��
    public float damage = 20f; // ���ݷ�
    public float speed = 3f; // �̵� �ӵ�
}