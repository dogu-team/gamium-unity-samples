using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UIToolkitDemo
{
    // Presenter/Controller for simulated mail messages
    // Contains non-UI logic and sends data from the ScriptableObjects to the MailScreen.
    public class MailController : MonoBehaviour
    {
        // events
        public static event Action<List<MailMessageSO>> InboxUpdated;
        public static event Action<List<MailMessageSO>> DeletedUpdated;
        public static event Action IndexReset;
        public static event Action<MailMessageSO> MessageShown;
        public static event Action<MailMessageSO, Vector2> RewardClaimed;
        public static event Action ShowEmptyMessage;

        [SerializeField]
        [Tooltip("Path within the Resources folders for MailMessage ScriptableObjects.")]
        string m_ResourcePath = "GameData/MailMessages";

        // mail messages stored as ScriptableObjects to simulate mail data
        List<MailMessageSO> m_MailMessages = new List<MailMessageSO>();
        List<MailMessageSO> m_InboxMessages = new List<MailMessageSO>();
        List<MailMessageSO> m_DeletedMessages = new List<MailMessageSO>();

        void OnEnable()
        {
            MailScreen.MarkedAsRead += OnMarkedAsRead;
            MailScreen.ClaimRewardClicked += OnClaimReward;
            MailScreen.DeleteClicked += OnDeleteMessage;
            MailScreen.UndeleteClicked += OnUndeleteMessage;
            MailScreen.InboxMessageUpdated += OnInboxMessageUpdated;
            MailScreen.DeletedMessageUpdated += OnDeletedMessageUpdated;
        }

        void OnDisable()
        {
            MailScreen.MarkedAsRead -= OnMarkedAsRead;
            MailScreen.ClaimRewardClicked -= OnClaimReward;
            MailScreen.DeleteClicked -= OnDeleteMessage;
            MailScreen.UndeleteClicked -= OnUndeleteMessage;
        }

        void Start()
        {
            LoadMailMessages();
            UpdateView();
        }

        void LoadMailMessages()
        {
            m_MailMessages.Clear();

            // load the ScriptableObjects from the Resources directory (default = Resources/GameData/MailMessages)
            m_MailMessages.AddRange(Resources.LoadAll<MailMessageSO>(m_ResourcePath));

            // separate lists for easier display
            m_InboxMessages = m_MailMessages.Where(x => !x.isDeleted).ToList();
            m_DeletedMessages = m_MailMessages.Where(x => x.isDeleted).ToList();
        }

        // show the mailboxes in the MailScreen interface
        void UpdateView()
        {
            //sort and generate elements from MailScreen
            m_InboxMessages = SortMailbox(m_InboxMessages);
            m_DeletedMessages = SortMailbox(m_DeletedMessages);

            // update event

            InboxUpdated?.Invoke(m_InboxMessages);
            DeletedUpdated?.Invoke(m_DeletedMessages);
            IndexReset?.Invoke();
        }

        // order messages by validated Date property
        List<MailMessageSO> SortMailbox(List<MailMessageSO> originalList)
        {
            return originalList.OrderBy(x => x.Date).Reverse().ToList();
        }

        // returns one mail message from the inbox by index
        MailMessageSO GetInboxMessage(int index)
        {
            if (index < 0 || index >= m_InboxMessages.Count)
                return null;

            return m_InboxMessages[index];
        }

        MailMessageSO GetDeletedMessage(int index)
        {
            if (index < 0 || index >= m_DeletedMessages.Count)
                return null;

            return m_DeletedMessages[index];
        }

        void MarkMessageAsRead(int indexToRead)
        {
            MailMessageSO msgToRead = GetInboxMessage(indexToRead);

            if (msgToRead != null && msgToRead.isNew)
            {
                msgToRead.isNew = false;
            }

        }
        void DeleteMessage(int indexToDelete)
        {
            MailMessageSO msgToDelete = GetInboxMessage(indexToDelete);

            if (msgToDelete == null)
                return;

            // mark as deleted move from Inbox to Deleted List
            msgToDelete.isDeleted = true;
            m_DeletedMessages.Add(msgToDelete);
            m_InboxMessages.Remove(msgToDelete);

            // rebuild the interface
            UpdateView();
        }

        private void OnUndeleteMessage(int indexToUndelete)
        {
            MailMessageSO msgToUndelete = GetDeletedMessage(indexToUndelete);
            if (msgToUndelete == null)
                return;

            msgToUndelete.isDeleted = false;
            m_DeletedMessages.Remove(msgToUndelete);
            m_InboxMessages.Add(msgToUndelete);

            // rebuild the interface
            UpdateView();
        }

        // event-handling methods
        void OnDeleteMessage(int index)
        {
            DeleteMessage(index);
        }

        void OnClaimReward(int indexToClaim, Vector2 screenPos)
        {
            MailMessageSO messageWithReward = GetInboxMessage(indexToClaim);

            if (messageWithReward == null)
                return;

            RewardClaimed?.Invoke(messageWithReward, screenPos);

            messageWithReward.isClaimed = true;
        }

        void OnMarkedAsRead(int index)
        {
            MarkMessageAsRead(index);
        }

        void OnDeletedMessageUpdated(int index)
        {
            MailMessageSO msg = GetDeletedMessage(index);

            if (msg != null)
                MessageShown?.Invoke(msg);
            else
                ShowEmptyMessage?.Invoke();
        }

        void OnInboxMessageUpdated(int index)
        {
            MailMessageSO msg = GetInboxMessage(index);

            if (msg != null)
            {
                MessageShown?.Invoke(msg);
            }
            else
                ShowEmptyMessage?.Invoke();
            
        }
    }
}