using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

public class AnonyOfChange : MonoBehaviour
{
    FirebaseAuth auth = null;

    FirebaseUser user = null;

    public GameObject emailPanel;

    private void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // 익명로그인 -> 이메일 로그인
    public void onAnonyToEmail()
    {
        if(emailPanel == null)
        {
            Debug.Log("이메일 전환창이 없습니다.");
            return;
        }
        emailPanel.SetActive(true);
    }

    // 이메일패널 비활성화
    public void Close()
    {
        emailPanel.SetActive(false);
    }

    // 이메일 로그인 정보를 받아온 후 사용
    public InputField id;
    public InputField pw;
    public void onEmailChangeSwich()
    {
        if(id.text.Length < 1 ||  pw.text.Length < 1)
        {
            Debug.Log("이메일이나 비밀번호가 비어있습니다.");
            return;
        }

        Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(id.text, pw.text);
        auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task =>
        {
            if(task.IsCanceled)
            {
                Debug.Log("Email Login task.IsCanceled");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.Log("Email Login task.Faulted");
                return;
            }

            var authResult = task.Result;
            user = authResult.User;

            Debug.LogFormat("Firebase Eamil user created successfully : {0} ({1})", user.DisplayName, user.UserId);
        });
    }
}
