using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    // UI panel at the bottom of the HomeScreen that shows prescribed text messages
    public class ChatWindow : MenuScreen
    {
        const string k_ChatText = "home-chat__text";
        const float k_delayBetweenKeys = 0.02f;
        const float k_delayBetweenLines = 1f;

        // chat name color
        const string k_tagOpen = "<color=green>";
        const string k_tagClose = "</color>";

        Label m_ChatText;
        WaitForSeconds WaitForKeyDelay;
        WaitForSeconds WaitForLineDelay;

        void OnEnable()
        {
            HomeScreenController.ShowChats += OnShowChats;
        }

        void OnDisable()
        {
            HomeScreenController.ShowChats -= OnShowChats;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            m_ChatText = m_Root.Q<Label>(k_ChatText);
        }

        void Start()
        {
            WaitForKeyDelay = new WaitForSeconds(k_delayBetweenKeys);
            WaitForLineDelay = new WaitForSeconds(k_delayBetweenLines);
        }

        // display the text chat ScriptableObjects
        void OnShowChats(List<ChatSO> chatData)
        {
            if (m_ChatText == null)
                return;

            StartCoroutine(ChatRoutine(chatData));
        }

        // iterate through the chat messages and show one at a time
        IEnumerator ChatRoutine(List<ChatSO> chatData)
        {
            foreach (ChatSO chatObject in chatData)
            {
                yield return StartCoroutine(AnimateMessageCoroutine(chatObject.chatname, chatObject.message));
                yield return WaitForLineDelay;
            }
            // repeat ad infinitum
            yield return StartCoroutine(ChatRoutine(chatData));
        }

        // increment the UI Element text with a small delay between each character
        IEnumerator AnimateMessageCoroutine(string chatName, string message)
        {

            m_ChatText.text = string.Empty;
            m_ChatText.text = k_tagOpen + " (" + chatName + ")" + k_tagClose + " ";

            foreach (char c in message.ToCharArray())
            {
                yield return WaitForKeyDelay;

                m_ChatText.text += c;
            }
        }
    }
}
