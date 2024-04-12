using System.IO;
using UnityEngine;
using System;
using System.Reflection;

namespace UnityEditor.ProjectWindowCallback
{
    public class CreateScriptTools
    {
        const string rootMenu = "Assets/" + BootConfig.company + "/Script/";
        const string rootTemplate = "Assets/Editor/Base/ScriptTemplates/";

        static string addSuffix;

        [MenuItem(itemName: rootMenu + "Pure", isValidateFunction: false, priority: 0)]
        public static void CreateCSFromTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(rootTemplate + "Pure.cs.txt", "Pure.cs");
        }

        [MenuItem(itemName: rootMenu + "Behaviour", isValidateFunction: false, priority: 1)]
        public static void CreateBehaviourScript()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(rootTemplate + "BehaviourScript.cs.txt", "Behaviour.cs");
        }

        [MenuItem(itemName: rootMenu + "Static", isValidateFunction: false, priority: 2)]
        public static void CreateStaticScript()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(rootTemplate + "Static.cs.txt", "Static.cs");
        }

        [MenuItem(itemName: rootMenu + "Singleton", isValidateFunction: false, priority: 6)]
        public static void CreateSingletonFromTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(rootTemplate + "Singleton.cs.txt", "Singleton.cs");
        }

        [MenuItem(itemName: rootMenu + "(..)Enum", isValidateFunction: false, priority: 10)]
        public static void CreateEnumFromTemplate()
        {
            MyCreateScriptFormTemplate("_Enum.cs", "Enum.cs.txt", "Enum");
        }

        [MenuItem(itemName: rootMenu + "(..)Configs", isValidateFunction: false, priority: 11)]
        public static void CreateConfigFromTemplate()
        {
            MyCreateScriptFormTemplate("_Configs.cs", "Config.cs.txt", "Configs");
        }

        [MenuItem(itemName: rootMenu + "(..)Info", isValidateFunction: false, priority: 12)]
        public static void CreateInfoFromTemplate()
        {
            MyCreateScriptFormTemplate("_Info.cs", "Info.cs.txt", "Info");
        }

        private static void MyCreateScriptFormTemplate(string defaultName, string templateName, string suffic)
        {
            addSuffix = suffic;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                icon: EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                instanceID: 0,
                endAction: ScriptableObject.CreateInstance<MyCreateScript>(),
                pathName: defaultName,
                resourceFile: rootTemplate + templateName);
        }

        internal class MyCreateScript : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                Type doCreateScriptAssetType = Assembly.Load("UnityEditor.CoreModule")
                    .GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset");

                object doCreateScriptAssetObj = Activator.CreateInstance(doCreateScriptAssetType);

                MethodInfo methodAction = doCreateScriptAssetType.GetMethod("Action");

                methodAction.Invoke(doCreateScriptAssetObj, new object[] { instanceId, pathName, resourceFile });

                string srcPath = pathName;
                if (srcPath.EndsWith(".cs") && !srcPath.Contains(addSuffix + ".cs"))
                {
                    string[] split = srcPath.Split(".cs");
                    string desPath = split[0] + addSuffix + ".cs";

                    Debug.Log("MyCreateScript " + srcPath + " To " + desPath);
                    File.Move(srcPath, desPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}