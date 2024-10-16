using UnityEngine;

// 게임 점수를 증가시키고 돈을 모으는 용
public class Coin : MonoBehaviour, IItem
{
    public int score = 50; // 증가할 점수
    public int coin = 100; // 증가할 코인

    public void Use(GameObject target)
    {

        SoundManager.instance.PlaySFX("coin");
        GameManager.instance.AddScore(score);
        GameManager.instance.AddCoin(coin);

        Destroy(gameObject);
    }

}