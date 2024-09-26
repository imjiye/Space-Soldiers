//using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun; // »ç¿ëÇÒ ÃÑ

    private PlayerInput playerInput;
    public Animator playerAnimator;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateUI();
    }

    // Åº¾à UI °»½Å
    private void UpdateUI()
    {
        if (gun != null && UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }
}