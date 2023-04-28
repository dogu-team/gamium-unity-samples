using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentSetBehaviour : MonoBehaviour
{
    [System.Serializable]
    public struct AttachmentSet
    {
        public string attachmentSetName;
        public Transform attachmentJoint;
        public GameObject attachmentItem;
    }

    public AttachmentSet[] attachments;
}
