using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIToolkitDemo
{
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/MailMessages/MailMessageGameData", menuName = "UIToolkitDemo/MailMessage", order = 5)]
    public class MailMessageSO : ScriptableObject
    {
        // appears in left 
        public string sender;

        // appears as a title 
        public string subjectLine;

        // format: MM/dd/yyyy
        public string date;

        // body of email text
        [TextArea]
        public string emailText;

        // image at end of email
        public Sprite emailPicAttachment;

        // footer of email shows a free shopItem
        public uint rewardValue;

        // type of free shopItem
        public ShopItemType rewardType;

        // has the gift been claimed
        public bool isClaimed;

        // important messages show a badge next to sender
        public bool isImportant;

        // has not been read
        public bool isNew;

        // deleted messages appear in the second tab
        public bool isDeleted;

        const int maxSubjectLine = 14;

        // validate DateTime for sorting
        public DateTime Date
        {
            get
            {
                DateTime dt;

                if (DateTime.TryParse(date, out dt))
                {
                    String.Format("{0:MM/dd/yyyy}", dt);
                }
                else
                {
                    dt = new DateTime();
                }

                return dt;
            }
        }

        public string SubjectLine
        {
            get
            {
                if (string.IsNullOrEmpty(subjectLine))
                {
                    return "...";
                }
                return (subjectLine.Length < maxSubjectLine) ? subjectLine : subjectLine.Substring(0, Math.Min(subjectLine.Length, maxSubjectLine)) + "...";
            }


            //return value.Substring(0, Math.Min(value.Length, maxLength));
        }


    }
}