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
    public GameObject successed;
    public GameObject failed;

    private void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        successed.SetActive(false);
        failed.SetActive(false);
    }

    // 익명로그인 -> 이메일 로그인
    public void onAnonyToEmail()
    {
        if (emailPanel == null)
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
        if (id.text.Length < 1 || pw.text.Length < 4)
        {
            FailOpen();
            //return;
        }

        Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(id.text, pw.text);
        auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task =>
        {

            if (task.IsCanceled || task.IsFaulted)
            {
                // 코루틴을 사용하여 메인 스레드에서 UI 업데이트 실행
                StartCoroutine(FailOpenCoroutine());
                return;
            }

            var authResult = task.Result;
            user = authResult.User;
            StartCoroutine(SuccessOpenCoroutine());
        });
    }

    IEnumerator FailOpenCoroutine()
    {
        // 메인 스레드에서 실행되는 코루틴
        yield return null;
        FailOpen();
    }

    IEnumerator SuccessOpenCoroutine()
    {
        // 메인 스레드에서 실행되는 코루틴
        yield return null;
        SuccessOpen();
    }

    public void SuccessOpen()
    {
        Debug.Log("SuccessOpen");
        successed.SetActive(true);
    }

    public void SuccessClose()
    {
        successed.SetActive(false);
    }

    public void FailOpen()
    {
        Debug.Log("FailOpen");
        failed.SetActive(true);
    }

    public void FailClose()
    {
        failed.SetActive(false);
    }
}