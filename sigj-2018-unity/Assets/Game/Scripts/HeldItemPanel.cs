using System;
using System.Text;
using UnityEngine;

public class HeldItemPanel : MonoBehaviour
{
  [SerializeField]
  private TMPro.TMP_Text _itemDescText = null;

  private void Start()
  {
    HandDragController.DragStart += OnDragStart;
    HandDragController.DragStop += OnDragStop;
    OnDragStop();
  }

  private void OnDestroy()
  {
    HandDragController.DragStart -= OnDragStart;
    HandDragController.DragStop -= OnDragStop;
  }

  private void OnDragStart(GameObject obj)
  {
    CritterController critter = obj.GetComponent<CritterController>();
    ItemDescriptor itemDescriptor = obj.GetComponent<ItemDescriptor>();
    if (critter != null)
    {
      CreatureDescriptor dna = critter.GetDNA();
      StringBuilder descStringBuilder = new StringBuilder();
      descStringBuilder.AppendLine(string.Format("Shape: {0}", CritterConstants.GetCreatureShapeDisplayString(dna.Shape)));
      descStringBuilder.AppendLine(string.Format("Color: {0}", CritterConstants.GetCreatureColorDisplayString(dna.Color)));
      descStringBuilder.AppendLine(string.Format("Size: {0}", CritterConstants.GetCreatureSizeDisplayString(dna.Size)));

      var attachmentCountMap = dna.GetAttachmentTypeCounts();
      foreach (var countPair in attachmentCountMap)
      {
        if (countPair.Value > 0)
        {
          descStringBuilder.AppendLine(string.Format("{0}: {1}", countPair.Key.AttachmentType, countPair.Value));
        }
      }

      _itemDescText.SetText(descStringBuilder);
    }
    else if (itemDescriptor != null)
    {
      _itemDescText.SetText(itemDescriptor.Description);
    }
  }

  private void OnDragStop()
  {
    StringBuilder descStringBuilder = new StringBuilder();
    descStringBuilder.AppendLine("CREATURE INSPECTOR");
    descStringBuilder.AppendLine("ERROR: NO CREATURE DETECTED");
    descStringBuilder.AppendLine("PICK UP A CREATURE");
    _itemDescText.SetText(descStringBuilder);
  }
}