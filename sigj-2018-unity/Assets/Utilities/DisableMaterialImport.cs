#if UNITY_EDITOR

using UnityEditor;

public class DisableMaterialImport : AssetPostprocessor
{
  public void OnPreprocessModel()
  {
    ModelImporter modelImporter = (ModelImporter)base.assetImporter;
    if (modelImporter != null)
    {
      modelImporter.importMaterials = false;
    }
  }
}

#endif