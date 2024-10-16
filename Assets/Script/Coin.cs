using UnityEngine;

// ���� ������ ������Ű�� ���� ������ ��
public class Coin : MonoBehaviour, IItem
{
    public int score = 50; // ������ ����
    public int coin = 100; // ������ ����

    public void Use(GameObject target)
    {

        SoundManager.instance.PlaySFX("coin");
        GameManager.instance.AddScore(score);
        GameManager.instance.AddCoin(coin);

        Destroy(gameObject);
    }

}