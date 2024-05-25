using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using static UnityEditor.AddressableAssets.Settings.AddressableAssetSettings;

public static class ManagedAddressable
{
	static Tree<DirectoryInfo> rootTree;
	static AddressableAssetSettings addressSetting;
	static AddressableConfig addressConfig;

    [MenuItem(ProjectConfigs.company + "/Asset/OpenGroup", false, 4)]
    public static void OpenGroup()
    {
        Type typeReflection =
            Assembly.Load("Unity.Addressables.Editor").GetType("UnityEditor.AddressableAssets.GUI.AddressableAssetsWindow");

        MethodInfo methodAction = typeReflection.GetMethod("Init", BindingFlags.Static | BindingFlags.NonPublic);
        methodAction.Invoke(null, null);
    }

    [MenuItem(ProjectConfigs.company + "/Asset/BuildBundle", false, 3)]
	public static void BuildBundle()
	{
		ClearRefreshGroup();
        BuildPlayerContent();
	}

	private static void ClearMissGroup()
	{
		if (addressSetting == null)
		{
			string backPth = Application.dataPath + "/../Tool/AddressableAssetSettings.asset";
			string desPath = Application.dataPath + "/AddressableAssetsData/AddressableAssetSettings.asset";

			if (File.Exists(desPath))
			{
				File.Delete(desPath);
            }
			File.Copy(backPth, desPath);
			File.Copy(backPth + ".meta", desPath + ".meta");
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
		}

		List<int> missingGroupsIndices = new();
		for (int i = 0; i < addressSetting.groups.Count; i++)
		{
			var g = addressSetting.groups[i];
			if (g == null)
				missingGroupsIndices.Add(i);
		}
		if (missingGroupsIndices.Count > 0)
		{
			for (int i = missingGroupsIndices.Count - 1; i >= 0; i--)
			{
				addressSetting.groups.RemoveAt(missingGroupsIndices[i]);
			}
		}
	}

	[MenuItem(ProjectConfigs.company + "/Asset/RefreshGroup(Clear)", false, 2)]
	public static void ClearRefreshGroup()
	{
        addressSetting = AddressableAssetSettingsDefaultObject.Settings;

        ClearMissGroup();

        for (int i = addressSetting.groups.Count - 1; i >= 0; i--)
        {
            addressSetting.RemoveGroup(addressSetting.groups[i]);
        }

        for (int i = addressSetting.GetLabels().Count; i > 0; i--)
        {
            addressSetting.RemoveLabel(addressSetting.GetLabels()[i - 1]);
        }
		FastRefreshGroup();
    }

    [MenuItem(ProjectConfigs.company + "/Asset/RefreshGroup(Auto)", true, 2)]
    static bool AutoRefreshGroupCheck()
    {
		bool isCheck = EditorPrefs.GetBool(AssetAuditing.ToggleAutoRefreshGroup, true);
		Menu.SetChecked(ProjectConfigs.company + "/Asset/RefreshGroup(Auto)", isCheck);
		return true;
    }

    [MenuItem(ProjectConfigs.company + "/Asset/RefreshGroup(Auto)", false, 2)]
    static void AutoRefreshGroup()
    {
        bool isCheck = EditorPrefs.GetBool(AssetAuditing.ToggleAutoRefreshGroup, true);
		EditorPrefs.SetBool(AssetAuditing.ToggleAutoRefreshGroup, !isCheck);
		FastRefreshGroup();
    }

    [MenuItem(ProjectConfigs.company + "/Asset/RefreshGroup(Fast)", false, 1)]
	public static void FastRefreshGroup()
	{
		addressSetting = AddressableAssetSettingsDefaultObject.Settings;

        addressConfig = AssetDatabase.LoadAssetAtPath<AddressableConfig>("Assets/Editor/Base/Asset/AddressableConfig.asset");

        ClearMissGroup();
		string assetsPath = Application.dataPath + "/";

		DirectoryInfo artDirectory = new("Assets/" + addressConfig.rootFolder);
		
		rootTree = new Tree<DirectoryInfo>(artDirectory);

		RecursiveDirectory(rootTree);
		List<Tree<DirectoryInfo>> allDirectoryTree = rootTree.GetAll();

		for (int j = 0; j < allDirectoryTree.Count; j++)
		{
			Tree<DirectoryInfo> directoryTree = allDirectoryTree[j];
			DirectoryInfo directory = directoryTree.Data;
			List<FileInfo> topFiles = TopFiles(directory);
			
			if (topFiles.Count > 0)
			{
                string groupName = directory.FullName.Replace("\\", "/").Replace(assetsPath, string.Empty);
               
				bool isStrip = false;
				for (int s = 0; s < addressConfig.stripFolder.Length; s++)
				{
					string strip = addressConfig.stripFolder[s];
					if (groupName.IndexOf(strip) == 0)
					{
						isStrip = true;
                        break;
					}
                }
				if (isStrip)
				{
					continue;
				}

                groupName = groupName.Replace("/", "-");
                groupName = groupName.Replace("Art-", "");

                AddressableAssetGroup aag = CreateOrGetGroup(groupName, false, false);

				for (int i = 0; i < topFiles.Count; i++)
				{
					FileInfo file = topFiles[i];
					string assetPath = "Assets/" + file.FullName.Replace("\\", "/").Replace(assetsPath, string.Empty);
					string guid = AssetDatabase.AssetPathToGUID(assetPath);

					AddressableAssetEntry addressableAssetEntry = addressSetting.CreateOrMoveEntry(guid, aag, false, false);

					addressableAssetEntry.address = assetPath;
					addressableAssetEntry.labels?.Clear();
					addressableAssetEntry.SetLabel(aag.name, true, true, false);
				}
			}
		}
		rootTree = null;

		List<AddressableAssetGroup> allGroups = addressSetting.groups;
		for (int i = allGroups.Count - 1; i >= 0; i--)
		{
			AddressableAssetGroup group = allGroups[i];
			if (group.entries.Count < 1)
			{
				allGroups.Remove(group);
			}
		}

		addressSetting.SetDirty(ModificationEvent.BatchModification, null, true, true);
		addressSetting = null;
		AssetDatabase.Refresh();
		AssetDatabase.SaveAssets();
	}

	static void RecursiveDirectory(Tree<DirectoryInfo> directoryTreeParent)
	{
		DirectoryInfo[] directoryInfos = directoryTreeParent.Data.GetDirectories();
		for (int i = 0; i < directoryInfos.Length; i++)
		{
			Tree<DirectoryInfo> directoryTreeChild = new (directoryInfos[i]);
			directoryTreeParent.AddChild(directoryTreeChild);
			RecursiveDirectory(directoryTreeChild);
		}
	}

	static List<FileInfo> TopFiles(DirectoryInfo directory)
	{
		return directory.GetFiles("*", SearchOption.TopDirectoryOnly)
			.Where(f => (Path.GetExtension(f.Name) != ".meta" && !f.Name.EndsWith(".DS_Store"))).ToList();
	}

	static AddressableAssetGroup CreateOrGetGroup(string groupName, bool readOnly = false, bool postEvent = false)
	{
		AddressableAssetGroup aag = addressSetting.FindGroup(groupName);

		if (aag == null)
		{
			AddressableAssetGroupTemplate groupTemplate = (AddressableAssetGroupTemplate)addressSetting.GetGroupTemplateObject(0);
			aag = addressSetting.CreateGroup(groupName, false, readOnly, postEvent, groupTemplate.SchemaObjects);
		}
		return aag;
	}
}