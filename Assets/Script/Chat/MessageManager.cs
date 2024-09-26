using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Auth;
using System.Threading.Tasks; //Task문법을 사용할 예정
using TMPro;
using Firebase.Extensions;
using System;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    public InputField usernameInput;
    public InputField messageInput;

    public MessageBox messageBoxPrefab;
    public Transform messsageContentTrans;

    public ScrollRect scrollRect;

    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                FirebaseInit();
            }
            else
            {
                Debug.LogError(" ");
            }
        }
        );

        if (scrollRect == null)
        {
            scrollRect = GetComponent<ScrollRect>();
        }

    }

    private void FirebaseInit()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;

        DatabaseReference chatDB = FirebaseDatabase.DefaultInstance.GetReference("ChatMessage");
        chatDB.LimitToLast(1).ValueChanged += ReceiveMessage;
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;

        if (senderAuth != null)
        {
            user = senderAuth.CurrentUser;

            if (user != null)
            {
                Debug.Log(user.UserId);
            }
        }
    }

    public void ReadChatMessage()
    {
        DatabaseReference chatDB = FirebaseDatabase.DefaultInstance.GetReference("ChatMessage");
        chatDB.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("read error");
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(" " + snapshot.ChildrenCount);

                foreach (var data in snapshot.Children)
                {
                    Debug.Log(data.Key + "   " + data.Child("username").Value.ToString() + "   " + data.Child("message").Value.ToString());
                }
            }
        }
        );
    }

    public void SendChatMessage()
    {
        DatabaseReference chatDB = FirebaseDatabase.DefaultInstance.GetReference("ChatMessage");
        string key = chatDB.Push().Key;

        Dictionary<string, string> msgDic = new Dictionary<string, string>();
        msgDic.Add("username", usernameInput.text);
        msgDic.Add("message", messageInput.text);

        Dictionary<string, object> updateMsg = new Dictionary<string, object>();
        updateMsg.Add(key, msgDic);
        chatDB.UpdateChildrenAsync(updateMsg).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Updata Complete");
            }
        }
        );
    }

    public void ReceiveMessage(object sender, ValueChangedEventArgs e)
    {
        DataSnapshot snapshot = e.Snapshot;
        Debug.Log(" " + snapshot.ChildrenCount);
        foreach (var message in snapshot.Children)
        {
            Debug.Log(message.Key + "   " + message.Child("username").Value.ToString() + "   " + message.Child("message").Value.ToString());
            string userName = message.Child("username").Value.ToString();
            string msg = message.Child("message").Value.ToString();

            AddChatBox(userName, msg);           
        }
    }

    public void AddChatBox(string username, string message)
    {
        MessageBox mbox = Instantiate<MessageBox>(messageBoxPrefab, messsageContentTrans);
        mbox.SetMessage(username, message);

        StartCoroutine(AdjustScrollPosition(mbox.GetComponent<RectTransform>()));

    }

    private IEnumerator AdjustScrollPosition(RectTransform newMessagBox)
    {
        yield return new WaitForEndOfFrame();

        float newY = -newMessagBox.anchoredPosition.y - (newMessagBox.sizeDelta.y * newMessagBox.pivot.y);
        float contentHeight = messsageContentTrans.GetComponent<RectTransform>().sizeDelta.y;
        float ScrollViewHeight = scrollRect.GetComponent<RectTransform>().sizeDelta.y;
        newY = newY - (ScrollViewHeight * 0.5f);

        messsageContentTrans.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, newY);
    }

}
