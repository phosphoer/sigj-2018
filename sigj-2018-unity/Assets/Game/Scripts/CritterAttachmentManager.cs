using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureAttachmentDescriptor
{
  public string AttachmentType = "";
  public string UISingularName = "";
  public string UIPluralName = "";
  public GameObject[] Variations = new GameObject[0];
}

public class CritterAttachmentManager : MonoBehaviour {

  public Vector2 ScaleMinMax = new Vector2(1.2f, .8f);
  public Vector2 RotateMinMax = new Vector2(0f, 90f);

  List<ProtoAttachPoint> attachPoints = new List<ProtoAttachPoint>();
  List<GameObject> attachments = new List<GameObject>();

  // Use this for initialization
  void Start()
  {

  }

  public void SpawnAttachments(CreatureDescriptor SpawnDescriptor)
  {
    // Clear off any previous attachments
    if (attachments.Count > 0) {
      for (int i = 0; i < attachments.Count; i++) {
        Destroy(attachments[i]);
      }
      attachments.Clear();
    }

    // Gather all available attach points
    attachPoints.Clear();
    attachPoints.AddRange(transform.GetComponentsInChildren<ProtoAttachPoint>(false));

    // Shuffle the indices of the attach points
    int[] shuffledAttachPointIndices= ArrayUtilities.MakeShuffledIntSequence(0, attachPoints.Count - 1);

    // Spawn attachments
    int attachmentCount = System.Math.Min(SpawnDescriptor.Attachments.Length, attachPoints.Count);
    for (int attachmentIndex = 0; attachmentIndex < attachmentCount; attachmentIndex++) {
      int attachPointIndex = shuffledAttachPointIndices[attachmentIndex];
      ProtoAttachPoint AttachPoint = attachPoints[attachPointIndex];
      GameObject AttachmentPrefab= SpawnDescriptor.Attachments[attachmentIndex];

      GameObject attachment = Instantiate(AttachmentPrefab, AttachPoint.transform) as GameObject;
      attachment.transform.localPosition = Vector3.zero;
      attachment.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(RotateMinMax.x, RotateMinMax.y));
      attachment.transform.localScale *= Random.Range(ScaleMinMax.x, ScaleMinMax.y);
      attachments.Add(attachment);
    }
  }
}
