using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text usernameLabel;
    public Text messageLabel;

    public void SetMessage(string username, string message)
    {
        usernameLabel.text = username; 
        messageLabel.text = message;
    }
}
