using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageScript : MonoBehaviour
{
    public float messageSpeed = 1.5f;
    public TextMeshPro message;

    private void Start()
    {
        message = transform.GetComponent<TextMeshPro>();
        StartCoroutine("ShowMessage");
    }

    /// <summary>
    ///  Сообщение постепенно тускнеет,движется вверх, а затем уничтожается
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowMessage()
    {
        for (float i = 1f; i >= 0; i -= 0.01f)
        {
            message.color = new Color(message.color.r, message.color.g, message.color.b, i);
            transform.Translate(0, messageSpeed * Time.deltaTime, 0);
            if (i <= 0.005f)
            {
                Destroy(gameObject);
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}