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

    public GameObject LogInPopUp; // 로그인 창
    public GameObject SignInPopUp; // 회원가입 창
    public GameObject QaAPopUp; // 문의사항 창
    public GameObject Quit; //  주의사항 창
    public GameObject SignUpSuccessPopup; // 회원가입 성공 알림 창

    public string scenename;

    FirebaseAuth auth = null;
    FirebaseUser user = null;

    // 기기 연동이 되어 있는 상태인지 체크
    private bool signedIn = false;

    void Awake()
    {
        // 초기화
        auth = FirebaseAuth.DefaultInstance;

        // 유저의 로그인 정보에 어떠한 변경점이 생기면 실행되게 이벤트를 걸어준다.
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void StartBtn()
    {
        UIManager.instance.LoadScene(scenename);
        Debug.Log("LoadScene");
    }

    // 로그인 창 활성화
    public void LoginOpen()
    {
        LogInPopUp.SetActive(true);
        SigninClose();
    }

    // 로그인 창 비활성화
    public void LoginClose()
    {
        LogInPopUp.SetActive(false);
    }

    // 회원가입 창 활성화
    public void SigninOpen()
    {
        SignInPopUp.SetActive(true);
        LoginClose();
    }

    // 회원가입 창 비활성화
    public void SigninClose()
    {
        SignInPopUp.SetActive(false);
    }

    // 문의사항 창 활성화
    public void QnAOpne()
    {
        QaAPopUp.SetActive(true);
    }
    
    // 문의사항 창 비활성화
    public void QnAClose()
    {
        QaAPopUp.SetActive(false);
    }

    // 경고창 활성화
    public void QuitPopUpOpen()
    {
        Quit.SetActive(true);
    }

    // 경고창 비활성화
    public void QuitClose()
    {
        Quit.SetActive(false);
    }

    // 계정 로그인에 어떠한 변경점이 발생시 진입.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            // 연동된 계정과 기기의 계정이 같다면 true를 리턴한다. 
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

    // 익명 로그인 상태 체크
    bool IsAnonymousUser()
    {
        return auth.CurrentUser != null && auth.CurrentUser.IsAnonymous;
    }

    // 익명 로그인 버튼 클릭시 실행할 함수
    public void AnonyLogin()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if(task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                loginResult_Lonin.text = "익명 로그인 실패";
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error : " + task.Exception);
                loginResult_Lonin.text = "익명 로그인 실패";
                return;
            }

            // 익명 로그인 연동 결과
            Firebase.Auth.AuthResult newUser = task.Result;
            Debug.LogFormat("User signed in succesfullty : {0} ({1})", newUser.User.DisplayName, newUser.User.UserId);
            loginResult_Lonin.text = "익명로그인";

            // Coroutine 호출
            StartCoroutine(LoadSceneAfterLogin());
                      
        });
    }

    // Coroutine으로 씬 전환
    IEnumerator LoadSceneAfterLogin()
    {
        // 1 프레임 대기 후 씬 로드 (메인 스레드에서 실행됨)
        yield return null;
        SceneManager.LoadScene(scenename);
    }

    // (이메일)회원가입 버튼 클릭시 실행할 함수
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
            loginResult_Sign.text = "이메일이나 비밀번호가 비어있습니다.";
            Debug.Log("이메일이나 비밀번호가 비어있습니다.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                loginResult_Sign.text = "회원가입 실패";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                loginResult_Sign.text = "회원가입 실패";
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.User.DisplayName, newUser.User.UserId);
            SignUpSuccessPopup.SetActive(true);
        });
    }

    // 회원가입 성공창 닫기
    public void SignUpSuccessPopupClose()
    {
        SignUpSuccessPopup.SetActive(false);
    }

    // (이메일)로그인 버튼 클릭시 실행할 함수
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
                loginResult_Lonin.text = "로그인 실패";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                loginResult_Lonin.text = "로그인 실패";
                return;
            }

            Firebase.Auth.AuthResult newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.User.DisplayName, newUser.User.UserId);
            loginResult_Lonin.text = "로그인 성공";

            // Coroutine 호출
            StartCoroutine(LoadSceneAfterLogin());

        });
    }

    // 연동 해제
    public void LogoutBtn()
    {
        if(auth.CurrentUser != null)
        {
            if(IsAnonymousUser())
            {
                UIManager.instance.Delete_btn();
                Debug.Log("게임 데이터가 삭제되었습니다.");
            }
            auth.SignOut();
            UIManager.instance.LoadScene(scenename);
        }
    }

    // 연동 계정 삭제
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