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

    // �͸�α��� -> �̸��� �α���
    public void onAnonyToEmail()
    {
        if(emailPanel == null)
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
        if(id.text.Length < 1 ||  pw.text.Length < 1)
        {
            Debug.Log("�̸����̳� ��й�ȣ�� ����ֽ��ϴ�.");
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
