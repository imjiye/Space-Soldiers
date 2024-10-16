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

    // �͸�α��� -> �̸��� �α���
    public void onAnonyToEmail()
    {
        if (emailPanel == null)
        {
            Debug.Log("�̸��� ��ȯâ�� �����ϴ�.");
            return;
        }
        emailPanel.SetActive(true);
    }

    // �̸����г� ��Ȱ��ȭ
    public void Close()
    {
        emailPanel.SetActive(false);
    }

    // �̸��� �α��� ������ �޾ƿ� �� ���
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
                // �ڷ�ƾ�� ����Ͽ� ���� �����忡�� UI ������Ʈ ����
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
        // ���� �����忡�� ����Ǵ� �ڷ�ƾ
        yield return null;
        FailOpen();
    }

    IEnumerator SuccessOpenCoroutine()
    {
        // ���� �����忡�� ����Ǵ� �ڷ�ƾ
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