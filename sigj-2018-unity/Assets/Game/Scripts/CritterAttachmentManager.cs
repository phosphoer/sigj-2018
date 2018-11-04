using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureAttachmentDescriptor
{
  public string AttachmentType = "";
  public string UISingularName = "";
  public string UIPluralName = "";
  public float MaxAllowedPitchAngle = 90;
  public GameObject[] Variations = new GameObject[0];
}

public class CritterAttachmentManager : MonoBehaviour {

  public Vector2 ScaleMinMax = new Vector2(1.2f, .8f);
  public Vector2 RotateMinMax = new Vector2(0f, 90f);

  List<ProtoAttachPoint> attachPoints = new List<ProtoAttachPoint>();
  List<GameObject> attachments = new List<GameObject>();

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
    bool[] usedAttachPoint = new bool[attachPoints.Count];

    // Spawn each attachment using the first valid attach point we can find
    for (int attachmentIndex = 0; attachmentIndex < SpawnDescriptor.Attachments.Length; attachmentIndex++) {
      // Get the prefab and the constraints for the attachment
      CreatureAttachmentDescriptor attachmentDescriptor = SpawnDescriptor.AttachmentTypes[attachmentIndex];
      GameObject attachmentPrefab = SpawnDescriptor.Attachments[attachmentIndex];

      // Find the first spawn point that satisfies the constraints
      for (int shuffledListIndex = 0; shuffledListIndex < shuffledAttachPointIndices.Length; ++shuffledListIndex) {
        int attachPointIndex = shuffledAttachPointIndices[shuffledListIndex];

        // Skip any attach points that are already used
        if (usedAttachPoint[attachPointIndex])
          continue;

        // Skip any attach points that violate our max pitch constraint
        ProtoAttachPoint attachPoint = attachPoints[attachPointIndex];
        Vector3 attachPointUp= attachPoint.transform.forward;
        float attachPointPitch = Mathf.Abs(90.0f - Vector3.Angle(attachPointUp, Vector3.up));
        if (attachPointPitch > attachmentDescriptor.MaxAllowedPitchAngle)
          continue;

        // Spawn the attachment
        GameObject attachment = Instantiate(attachmentPrefab, attachPoint.transform) as GameObject;
        attachment.transform.localPosition = Vector3.zero;
        attachment.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(RotateMinMax.x, RotateMinMax.y));
        attachment.transform.localScale *= Random.Range(ScaleMinMax.x, ScaleMinMax.y);
        attachments.Add(attachment);

        // Mark the attach point as used
        usedAttachPoint[attachPointIndex] = true;

        // Stop searching for a valid attach point
        break;
      }
    }
  }
}
