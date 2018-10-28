// The MIT License (MIT)
// Copyright (c) 2017 David Evans @phosphoer
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "new-build-definition", menuName = "Boat Game/Build Definition")]
public class BuildDefinition : ScriptableObject
{
  public string LocationPathName = "relative/path/build.exe";
  public string ProductNameOverride;
  public string CompanyNameOverride;
  public BuildTarget BuildTarget;
  public BuildTargetGroup BuildTargetGroup;
  public SceneField[] Scenes;

  [ContextMenu("Build")]
  public void Build()
  {
    BuildPipeline.BuildPlayer(ApplyBuildSettings());
  }

  [ContextMenu("Copy Scenes From Build Settings")]
  public void CopySceneListFromBuild()
  {
    Scenes = new SceneField[EditorBuildSettings.scenes.Length];
    for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
    {
      var sceneBuildSetting = EditorBuildSettings.scenes[i];
      SceneAsset sceneAsset = (SceneAsset)AssetDatabase.LoadAssetAtPath(sceneBuildSetting.path, typeof(SceneAsset));
      Scenes[i] = new SceneField();
      Scenes[i].sceneAsset = sceneAsset;
    }
  }

  [ContextMenu("Apply Build Settings")]
  public BuildPlayerOptions ApplyBuildSettings()
  {
    // Set basic options
    BuildPlayerOptions buildOptions = new BuildPlayerOptions();
    buildOptions.targetGroup = BuildTargetGroup;
    buildOptions.target = BuildTarget;
    buildOptions.locationPathName = LocationPathName;

    if (!string.IsNullOrEmpty(ProductNameOverride)) PlayerSettings.productName = ProductNameOverride;
    if (!string.IsNullOrEmpty(CompanyNameOverride)) PlayerSettings.companyName = CompanyNameOverride;

    // Build scene list
    List<string> sceneList = new List<string>();
    foreach (SceneField scene in Scenes)
      sceneList.Add(AssetDatabase.GetAssetPath(scene.sceneAsset));

    buildOptions.scenes = sceneList.ToArray();

    return buildOptions;
  }
}

#endif