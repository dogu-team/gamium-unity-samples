using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    // demonstrates how to create a UI with text elements

    public class MailScreen : MenuScreen
    {
        public static event Action<int> DeleteClicked;
        public static event Action<int> UndeleteClicked;
        public static event Action<int, Vector2> ClaimRewardClicked;
        public static event Action<int> MarkedAsRead;
        public static event Action<int> InboxMessageUpdated;
        public static event Action<int> DeletedMessageUpdated;

        [Header("Mail Messages")]
        [Tooltip("Mail Message Asset to instantiate ")]
        [SerializeField] VisualTreeAsset m_MailMessageAsset;

        [Header("Sprites")]
        [SerializeField] Sprite m_NewMailIcon;
        [SerializeField] Sprite m_OldMailIcon;
        [SerializeField] GameIconsSO m_GameIcons;

        [Header("Tabbed Menu")]
        [SerializeField] TabbedMenu m_TabbedMenu;

        // string IDs
        const string k_InboxParent = "mail-inbox-parent";
        const string k_DeletedParent = "mail-deleted-parent";

        // locates elements to update with ScriptableObject data
        const string k_NewMail = "mail-item-new";
        const string k_Subject = "mail-item-subject";
        const string k_Date = "mail-item-date";
        const string k_Badge = "mail-item-badge";

        const string k_InboxTab = "mail-inbox-mailtab";
        const string k_DeletedTab = "mail-deleted-mailtab";

        const string k_InboxSubject = "mail-message-title";
        const string k_InboxMessageText = "mail-message-text";
        const string k_InboxAttachment = "mail-message-attachment";
        const string k_InboxGiftIcon = "mail-gift-icon";
        const string k_InboxGiftAmount = "mail-gift-amount";

        const string k_FrameBorder = "mail-frame__border";
        const string k_FrameBar = "mail-frame__bar";
        const string k_Footer = "mail-right-footer";
        const string k_ClaimButton = "mail-gift-button";
        const string k_DeleteButton = "mail-delete-button";

        const string k_DeletedSubject = "mail-deleted-message-title";
        const string k_DeletedMessageText = "mail-deleted-message-text";
        const string k_DeletedAttachment = "mail-deleted-message-attachment";
        const string k_UndeleteButton = "mail-undelete-button";
        const string k_MailNoMessages = "mail-no-messages";

        // class names
        const string k_MailMessageClass = "mail-message";
        const string k_MailMessageSelectedClass = "mail-message-selected";
        const string k_MailMessageDeletedClass = "mail-message-deleted";
        const string k_GiftDeletedClass = "mail-gift-button--deleted";

        const string k_FrameBarUnclaimedClass = "mail-frame_bar--unclaimed";
        const string k_FrameBarClaimedClass = "mail-frame_bar--claimed";
        const string k_FrameBorderUnclaimedClass = "mail-frame_border--unclaimed";
        const string k_FrameBorderClaimedClass = "mail-frame_border--claimed";

        const string k_MailNoMessagesClass = "mail-no-messages";
        const string k_MailNoMessagesInactiveClass = "mail-no-messages--inactive";

        const string k_ResourcePath = "GameData/GameIcons";

        VisualElement m_InboxParent;
        VisualElement m_DeletedParent;
        VisualElement m_ClaimButton;
        Button m_DeleteButton;
        Button m_UndeleteButton;
        VisualElement m_Footer;
        VisualElement m_FrameBorder;
        VisualElement m_FrameBar;

        VisualElement m_InboxTab;
        VisualElement m_DeletedTab;

        Label m_InboxSubject;
        Label m_InboxMessageText;
        VisualElement m_InboxAttachment;
        Label m_InboxGiftAmount;
        VisualElement m_InboxGiftIcon;

        Label m_DeletedSubject;
        Label m_DeletedMessageText;
        VisualElement m_DeletedAttachment;
        Label m_MailNoMessages;

        // currently selected mail item (from the currently selected mailbox tab), defaults to top item
        int m_CurrentMessageIndex = 0;
        const float transitionTime = 0.1f;


        void Start()
        {
            if (m_TabbedMenu == null)
                m_TabbedMenu = GetComponent<TabbedMenu>();

            if (m_TabbedMenu == null)
                Debug.LogWarning("MailScreen.Start: missing TabbedMenu component");

            if (m_GameIcons == null)
                m_GameIcons = Resources.Load<GameIconsSO>(k_ResourcePath);
        }

        // listen for events
        void OnEnable()
        {
            TabbedMenuController.TabSelected += OnTabSelected;

            MailController.InboxUpdated += OnInboxUpdated;
            MailController.DeletedUpdated += OnDeleteBoxUpdated;
            MailController.IndexReset += OnIndexReset;
            MailController.MessageShown += OnMessageShown;
            MailController.ShowEmptyMessage += OnShowEmptyMessage;
        }

        void OnDisable()
        {
            TabbedMenuController.TabSelected -= OnTabSelected;

            MailController.InboxUpdated -= OnInboxUpdated;
            MailController.DeletedUpdated -= OnDeleteBoxUpdated;
            MailController.IndexReset -= OnIndexReset;
            MailController.MessageShown -= OnMessageShown;
            MailController.ShowEmptyMessage -= OnShowEmptyMessage;
        }

        public override void ShowScreen()
        {
            // select the Inbox tab when showing the menu screen
            base.ShowScreen();

            m_TabbedMenu?.SelectFirstTab();
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            m_ClaimButton = m_Root.Q(k_ClaimButton);
            m_DeleteButton = m_Root.Q<Button>(k_DeleteButton);
            m_UndeleteButton = m_Root.Q<Button>(k_UndeleteButton);

            m_InboxParent = m_Root.Q(k_InboxParent);
            m_DeletedParent = m_Root.Q(k_DeletedParent);

            m_InboxTab = m_Root.Q(k_InboxTab);
            m_DeletedTab = m_Root.Q(k_DeletedTab);
            m_InboxSubject = m_Root.Q<Label>(k_InboxSubject);

            m_InboxMessageText = m_Root.Q<Label>(k_InboxMessageText);
            m_InboxAttachment = m_Root.Q(k_InboxAttachment);
            m_InboxGiftIcon = m_Root.Q(k_InboxGiftIcon);
            m_InboxGiftAmount = m_Root.Q<Label>(k_InboxGiftAmount);

            m_Footer = m_Root.Q(k_Footer);
            m_FrameBorder = m_Root.Q(k_FrameBorder);
            m_FrameBar = m_Root.Q(k_FrameBar);

            m_DeletedSubject = m_Root.Q<Label>(k_DeletedSubject);
            m_DeletedMessageText = m_Root.Q<Label>(k_DeletedMessageText);
            m_DeletedAttachment = m_Root.Q(k_DeletedAttachment);

            m_MailNoMessages = m_Root.Q<Label>(k_MailNoMessages);

        }

        protected override void RegisterButtonCallbacks()
        {
            m_ClaimButton?.RegisterCallback<ClickEvent>(ClaimReward);
            m_DeleteButton?.RegisterCallback<ClickEvent>(DeleteMailMessage);
            m_UndeleteButton?.RegisterCallback<ClickEvent>(UndeleteMailMessage);
        }


        void UpdateMailbox(List<MailMessageSO> messageList, VisualElement parentElement)
        {
            parentElement.Clear();
            ClearMailContents();

            if (messageList.Count == 0)
                return;

            foreach (MailMessageSO msg in messageList)
            {
                if (msg != null)
                    CreateMailMessage(msg, parentElement);
            }

            m_CurrentMessageIndex = 0;

            // wait for the Layout to build before showing mail contents
            parentElement.RegisterCallback<GeometryChangedEvent>(evt => ShowCurrentMailContents());
        }

        public void ResetCurrentIndex()
        {
            m_CurrentMessageIndex = 0;
            HighlightFirstMessage();
            MarkMailElementAsRead(GetFirstMailElement());
        }

        // generate one mailbox item (left panel)
        void CreateMailMessage(MailMessageSO mailData, VisualElement mailboxContainer)
        {
            if (mailboxContainer == null || mailData == null || m_MailMessageAsset == null)
            {
                return;
            }

            // instantiate the VisualTreeAsset of the mail message
            // note: this creates an extra Visual Element "template container" above the instance
            TemplateContainer instance = m_MailMessageAsset.Instantiate();

            // assign mail message class to first child of TemplateContainer (the mail message)
            instance.hierarchy[0].AddToClassList(k_MailMessageClass);

            mailboxContainer.Add(instance);
            instance.RegisterCallback<ClickEvent>(ClickMessage);

            ReadMailData(mailData, instance);

        }
        // get data from ScriptableObject
        void ReadMailData(MailMessageSO mailData, TemplateContainer instance)
        {
            // read ScriptableObject data
            Label subjectLine = instance.Q<Label>(k_Subject);
            subjectLine.text = mailData.SubjectLine;

            Label date = instance.Q<Label>(k_Date);
            date.text = mailData.date;

            VisualElement badge = instance.Q<VisualElement>(k_Badge);
            badge.visible = mailData.isImportant;

            VisualElement newIcon = instance.Q<VisualElement>(k_NewMail);
            newIcon.style.backgroundImage = (mailData.isNew) ? new StyleBackground(m_NewMailIcon) : new StyleBackground(m_OldMailIcon);
        }

        // process a clicked item in the mailbox
        void ClickMessage(ClickEvent evt)
        {
            // the clicked mail item
            VisualElement clickedElement = evt.currentTarget as VisualElement;

            // use this index to retrieve the message body 
            m_CurrentMessageIndex = clickedElement.parent.IndexOf(clickedElement);
            ShowCurrentMailContents();

            // highlight and mark the mail message read 
            MarkMailElementAsRead(clickedElement);

            VisualElement backgroundElement = clickedElement.Q(className: k_MailMessageClass);
            HighlightMessage(backgroundElement);

            AudioManager.PlayDefaultButtonSound();
        }

        void HighlightMessage(VisualElement elementToHighlight)
        {
            if (elementToHighlight == null)
                return;

            // deselect all other visuals
            GetAllMailElements().
                Where((element) => element.ClassListContains(k_MailMessageSelectedClass)).
                ForEach(UnhighlightMessage);

            elementToHighlight.AddToClassList(k_MailMessageSelectedClass);
        }

        void UnhighlightMessage(VisualElement elementToUnhighlight)
        {
            if (elementToUnhighlight == null)
                return;

            elementToUnhighlight.RemoveFromClassList(k_MailMessageSelectedClass);
        }

        void HighlightFirstMessage()
        {
            VisualElement firstElement = GetFirstMailElement();
            if (firstElement != null)
            {
                HighlightMessage(firstElement);
            }
        }

        void DeleteMailMessage(ClickEvent evt)
        {
            // change selectors to play transition
            VisualElement elemToDelete = GetSelectedMailMessage().parent;
            elemToDelete.AddToClassList(k_MailMessageDeletedClass);

            StartCoroutine(DeleteMailMessageRoutine());
        }

        IEnumerator DeleteMailMessageRoutine()
        {
            AudioManager.PlayDefaultButtonSound();

            // wait for transition
            yield return new WaitForSeconds(transitionTime);

            // tells the Mail Presenter/Controller to delete the current message, then rebuild the interface
            DeleteClicked.Invoke(m_CurrentMessageIndex);

            m_InboxAttachment.style.backgroundImage = null;
            ResetCurrentIndex();
            ShowCurrentMailContents();
        }

        void UndeleteMailMessage(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();

            VisualElement elemToDelete = GetSelectedMailMessage().parent;

            UndeleteClicked.Invoke(m_CurrentMessageIndex);
            m_DeletedAttachment.style.backgroundImage = null;

            ResetCurrentIndex();
            ShowCurrentMailContents();
        }

        // changes unread icon (NewIcon) to read
        void MarkMailElementAsRead(VisualElement messageElement)
        {
            if (messageElement == null)
                return;

            MarkedAsRead.Invoke(m_CurrentMessageIndex);

            VisualElement newIcon = messageElement.Q<VisualElement>(k_NewMail);
            newIcon.style.backgroundImage = new StyleBackground(m_OldMailIcon);
        }

        // get mailbox parent, depending on current tab
        VisualElement GetMailboxParent()
        {
            if (m_TabbedMenu.IsTabSelected(m_InboxTab))
            {
                return m_InboxParent;
            }
            return m_DeletedParent;
        }

        // show the contents of the currently selected mailbox item
        void ShowCurrentMailContents()
        {
            if (m_InboxTab != null && m_TabbedMenu != null && m_TabbedMenu.IsTabSelected(m_InboxTab))
            {
                InboxMessageUpdated?.Invoke(m_CurrentMessageIndex);
            }
            else
            {
                DeletedMessageUpdated?.Invoke(m_CurrentMessageIndex);
            }
        }

        // fill the right panel with the email text
        void ShowMailContents(MailMessageSO msg)
        {
            // empty message, nothing in current mailbox
            if (msg == null)
            {
                ShowEmptyMessage();
                return;
            }

            ShowNoMessages(false);
            // inbox message
            if (!msg.isDeleted)
            {
                m_InboxMessageText.style.display = DisplayStyle.Flex;
 
                m_InboxSubject.text = msg.subjectLine;
                m_InboxMessageText.text = msg.emailText;
                m_InboxAttachment.style.display = DisplayStyle.Flex;
                m_InboxAttachment.style.backgroundImage = new StyleBackground(msg.emailPicAttachment);

                m_InboxGiftAmount.text = msg.rewardValue.ToString();
                Sprite giftIcon = m_GameIcons.GetShopTypeIcon(msg.rewardType);
                m_InboxGiftIcon.style.backgroundImage = new StyleBackground(giftIcon);


                m_InboxGiftAmount.style.display = (!msg.isClaimed && msg.rewardValue > 0) ? DisplayStyle.Flex : DisplayStyle.None;
                m_InboxGiftIcon.style.display = (!msg.isClaimed && msg.rewardValue > 0) ? DisplayStyle.Flex : DisplayStyle.None;
                m_InboxGiftAmount?.RemoveFromClassList(k_GiftDeletedClass);
                m_InboxGiftIcon?.RemoveFromClassList(k_GiftDeletedClass);

                m_ClaimButton?.SetEnabled(!msg.isClaimed);
                m_DeleteButton?.SetEnabled(true);

                if (msg.isClaimed)
                    ShowFooter(false);
                else
                    ShowFooter(true);
            }
            // deleted message
            else
            {
                
                m_DeletedSubject.text = msg.subjectLine;
                m_DeletedMessageText.style.display = DisplayStyle.Flex;
                m_DeletedMessageText.text = msg.emailText;
                m_DeletedAttachment.style.display = DisplayStyle.Flex;
                m_DeletedAttachment.style.backgroundImage = new StyleBackground(msg.emailPicAttachment);

                m_UndeleteButton?.SetEnabled(true);
            }
        }

        void ShowEmptyMessage()
        {

            // hide the attachment (so Scrollview appears correctly)
            m_InboxMessageText.style.display = DisplayStyle.None;
            m_DeletedMessageText.style.display = DisplayStyle.None;
            m_InboxAttachment.style.display = DisplayStyle.None;
            m_DeletedAttachment.style.display = DisplayStyle.None;

            // show "no message selected" 
            ShowNoMessages(true);

            // hide the footer
            ShowFooter(false);
        }

        void ClearMailContents()
        {
            // if we delete the last message/create a new mailbox, clear the message text
            m_InboxSubject.text = string.Empty;
            m_InboxMessageText.text = string.Empty;
            m_InboxGiftAmount.text = string.Empty;
            m_DeletedSubject.text = string.Empty;
            m_DeletedMessageText.text = string.Empty;
        }

        VisualElement GetFirstMailElement()
        {
            VisualElement parentElement = GetMailboxParent();
            if (parentElement != null)
            {
                return parentElement.Query<VisualElement>(className: k_MailMessageClass);
            }
            return null;
        }

        // get all VisualElements with mail message class
        UQueryBuilder<VisualElement> GetAllMailElements()
        {
            return m_Root.Query<VisualElement>(className: k_MailMessageClass);
        }

        VisualElement GetSelectedMailMessage()
        {
            return m_Root.Query<VisualElement>(className: k_MailMessageSelectedClass);
        }

        void ShowNoMessages(bool state)
        {
            if (state)
            {
                m_MailNoMessages.RemoveFromClassList(k_MailNoMessagesInactiveClass);
                m_MailNoMessages.AddToClassList(k_MailNoMessagesClass);
                m_UndeleteButton?.SetEnabled(false);
                m_DeleteButton?.SetEnabled(false);

            }
            else
            {
                m_MailNoMessages.RemoveFromClassList(k_MailNoMessagesClass);
                m_MailNoMessages.AddToClassList(k_MailNoMessagesInactiveClass);
                m_UndeleteButton?.SetEnabled(true);
                m_DeleteButton?.SetEnabled(true);
            }
        }

        void ShowFooter(bool state)
        {
            m_Footer.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;

            // show the frame border and bar
            if (state)
            {
                m_FrameBar.RemoveFromClassList(k_FrameBarClaimedClass);
                m_FrameBar.AddToClassList(k_FrameBarUnclaimedClass);

                m_FrameBorder.RemoveFromClassList(k_FrameBorderClaimedClass);
                m_FrameBorder.AddToClassList(k_FrameBorderUnclaimedClass);
            }
            // hide the frame border and bar
            else
            {
                m_FrameBar.RemoveFromClassList(k_FrameBarUnclaimedClass);
                m_FrameBar.AddToClassList(k_FrameBarClaimedClass);

                m_FrameBorder.RemoveFromClassList(k_FrameBorderUnclaimedClass);
                m_FrameBorder.AddToClassList(k_FrameBorderClaimedClass);
            }
        }

        // tell the MailManager to claim the gift
        void ClaimReward(ClickEvent evt)
        {
            VisualElement clickedElement = evt.currentTarget as VisualElement;
            Vector2 screenPos = clickedElement.worldBound.center;

            StartCoroutine(ClaimRewardRoutine());
            ClaimRewardClicked?.Invoke(m_CurrentMessageIndex, screenPos);

            AudioManager.PlayDefaultButtonSound();
        }

        // plays short animation when receiving reward
        IEnumerator ClaimRewardRoutine()
        {
            m_Footer.style.display = DisplayStyle.None;
            m_InboxGiftAmount?.AddToClassList(k_GiftDeletedClass);
            m_InboxGiftIcon?.AddToClassList(k_GiftDeletedClass);
            yield return new WaitForSeconds(transitionTime);
            ShowFooter(false);
            m_ClaimButton?.SetEnabled(false);
        }

        // event-handling methods

        void OnMessageShown(MailMessageSO msg)
        {
            if (msg != null)
            {
                ShowMailContents(msg);
            }
        }

        // select the top mail item when switching tabs
        void OnTabSelected()
        {
            if (m_TabbedMenu.IsTabSelected(m_InboxTab) || m_TabbedMenu.IsTabSelected(m_DeletedTab))
            {
                ResetCurrentIndex();
                ShowCurrentMailContents();
            }
        }

        void OnInboxUpdated(List<MailMessageSO> inbox)
        {
            UpdateMailbox(inbox, m_InboxParent);
            m_Footer.style.display = DisplayStyle.None;
            m_ClaimButton.SetEnabled(false);
            m_DeleteButton.SetEnabled(false);
        }

        void OnDeleteBoxUpdated(List<MailMessageSO> deleted)
        {
            UpdateMailbox(deleted, m_DeletedParent);
        }

        void OnIndexReset()
        {
            ResetCurrentIndex();
        }

        void OnShowEmptyMessage()
        {
            ShowEmptyMessage();
        }
    }
}