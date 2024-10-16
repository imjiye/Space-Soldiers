using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using System.Threading;

using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;

public class Login : MonoBehaviour
{

    [SerializeField] string email;
    [SerializeField] string password;

    public InputField inputTextEmail_Sign;
    public InputField inputTextEmail_Lonin;
    public InputField inputTextPassword_Sign;
    public InputField inputTextPassword_Lonin;
    public Text loginResult_Sign;
    public Text loginResult_Lonin;

    public GameObject LogInPopUp; // �α��� â
    public GameObject SignInPopUp; // ȸ������ â
    public GameObject QaAPopUp; // ���ǻ��� â
    public GameObject Quit; //  ���ǻ��� â
    public GameObject SignUpSuccessPopup; // ȸ������ ���� �˸� â

    public string scenename;

    FirebaseAuth auth = null;
    FirebaseUser user = null;

    // ��� ������ �Ǿ� �ִ� �������� üũ
    private bool signedIn = false;

    void Awake()
    {
        // �ʱ�ȭ
        auth = FirebaseAuth.DefaultInstance;

        // ������ �α��� ������ ��� �������� ����� ����ǰ� �̺�Ʈ�� �ɾ��ش�.
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void StartBtn()
    {
        UIManager.instance.LoadScene(scenename);
        Debug.Log("LoadScene");
    }

    // �α��� â Ȱ��ȭ
    public void LoginOpen()
    {
        LogInPopUp.SetActive(true);
        SigninClose();
    }

    // �α��� â ��Ȱ��ȭ
    public void LoginClose()
    {
        LogInPopUp.SetActive(false);
    }

    // ȸ������ â Ȱ��ȭ
    public void SigninOpen()
    {
        SignInPopUp.SetActive(true);
        LoginClose();
    }

    // ȸ������ â ��Ȱ��ȭ
    public void SigninClose()
    {
        SignInPopUp.SetActive(false);
    }

    // ���ǻ��� â Ȱ��ȭ
    public void QnAOpne()
    {
        QaAPopUp.SetActive(true);
    }
    
    // ���ǻ��� â ��Ȱ��ȭ
    public void QnAClose()
    {
        QaAPopUp.SetActive(false);
    }

    // ���â Ȱ��ȭ
    public void QuitPopUpOpen()
    {
        Quit.SetActive(true);
    }

    // ���â ��Ȱ��ȭ
    public void QuitClose()
    {
        Quit.SetActive(false);
    }

    // ���� �α��ο� ��� �������� �߻��� ����.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            // ������ ������ ����� ������ ���ٸ� true�� �����Ѵ�. 
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                UnityEngine.Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                UnityEngine.Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    // �͸� �α��� ���� üũ
    bool IsAnonymousUser()
    {
        return auth.CurrentUser != null && auth.CurrentUser.IsAnonymous;
    }

    // �͸� �α��� ��ư Ŭ���� ������ �Լ�
    public void AnonyLogin()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if(task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                loginResult_Lonin.text = "�͸� �α��� ����";
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error : " + task.Exception);
                loginResult_Lonin.text = "�͸� �α��� ����";
                return;
            }

            // �͸� �α��� ���� ���
            Firebase.Auth.AuthResult newUser = task.Result;
            Debug.LogFormat("User signed in succesfullty : {0} ({1})", newUser.User.DisplayName, newUser.User.UserId);
            loginResult_Lonin.text = "�͸�α���";

            // Coroutine ȣ��
            StartCoroutine(LoadSceneAfterLogin());
                      
        });
    }

    // Coroutine���� �� ��ȯ
    IEnumerator LoadSceneAfterLogin()
    {
        // 1 ������ ��� �� �� �ε� (���� �����忡�� �����)
        yield return null;
        SceneManager.LoadScene(scenename);
    }

    // (�̸���)ȸ������ ��ư Ŭ���� ������ �Լ�
    public void JoinBtnOnClick()
    {
        email = inputTextEmail_Sign.text;
        password = inputTextEmail_Sign.text;

        Debug.Log("email: " + email + ", password: " + password);

        CreateUser();
    }


    void CreateUser()
    {
        if(email.Length < 1 || password.Length < 4)
        {
            loginResult_Sign.text = "�̸����̳� ��й�ȣ�� ����ֽ��ϴ�.";
            Debug.Log("�̸����̳� ��й�ȣ�� ����ֽ��ϴ�.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                loginResult_Sign.text = "ȸ������ ����";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                loginResult_Sign.text = "ȸ������ ����";
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.User.DisplayName, newUser.User.UserId);
            SignUpSuccessPopup.SetActive(true);
        });
    }

    // ȸ������ ����â �ݱ�
    public void SignUpSuccessPopupClose()
    {
        SignUpSuccessPopup.SetActive(false);
    }

    // (�̸���)�α��� ��ư Ŭ���� ������ �Լ�
    public void LoginBtn()
    {
        email = inputTextEmail_Lonin.text;
        password = inputTextPassword_Lonin.text;

        Debug.Log("email : " + email + ", password : " + password);

        LoginUser();
    }

    void LoginUser()
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                loginResult_Lonin.text = "�α��� ����";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                loginResult_Lonin.text = "�α��� ����";
                return;
            }

            Firebase.Auth.AuthResult newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.User.DisplayName, newUser.User.UserId);
            loginResult_Lonin.text = "�α��� ����";

            // Coroutine ȣ��
            StartCoroutine(LoadSceneAfterLogin());

        });
    }

    // ���� ����
    public void LogoutBtn()
    {
        if(auth.CurrentUser != null)
        {
            if(IsAnonymousUser())
            {
                UIManager.instance.Delete_btn();
                Debug.Log("���� �����Ͱ� �����Ǿ����ϴ�.");
            }
            auth.SignOut();
            UIManager.instance.LoadScene(scenename);
        }
    }

    // ���� ���� ����
    public void UserDelete()
    {
        if(auth.CurrentUser != null)
        {
            UIManager.instance.Delete_btn();
            auth.CurrentUser.DeleteAsync();
            UIManager.instance.LoadScene(scenename);
        }
    }
}