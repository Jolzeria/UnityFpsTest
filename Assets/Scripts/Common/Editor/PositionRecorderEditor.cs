using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelEditor))]
public class LevelEditorEditor : Editor
{
    private SerializedProperty spawnDatasProp;
    private GameObject tempGameObject; // 用于拖入 Target

    private void OnEnable()
    {
        spawnDatasProp = serializedObject.FindProperty("spawnDatas");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(spawnDatasProp, new GUIContent("靶子生成列表"), true);

        // 拖入Target创建并记录位置
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("拖入靶子创建并记录位置", EditorStyles.boldLabel);
        tempGameObject =
            (GameObject) EditorGUILayout.ObjectField("拖入 Target", tempGameObject, typeof(GameObject), true);

        if (tempGameObject != null)
        {
            AddSpawnPosition(tempGameObject.transform);
            tempGameObject = null; // 清空，以便继续拖入新的
        }
        
        if (spawnDatasProp.isArray)
        {
            for (int i = 0; i < spawnDatasProp.arraySize; i++)
            {
                SerializedProperty element = spawnDatasProp.GetArrayElementAtIndex(i);
                SerializedProperty targetProp = element.FindPropertyRelative("target");
                SerializedProperty spawnPositionProp = element.FindPropertyRelative("spawnPosition");

                // 监听 target 变化，自动更新 spawnPosition
                if (targetProp.objectReferenceValue is Transform targetTransform)
                {
                    spawnPositionProp.vector3Value = targetTransform.position;
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddSpawnPosition(Transform targetTransform)
    {
        spawnDatasProp.arraySize++; // 增加数组大小
        SerializedProperty newElement = spawnDatasProp.GetArrayElementAtIndex(spawnDatasProp.arraySize - 1);
        newElement.FindPropertyRelative("spawnTime").floatValue = 0f;
        newElement.FindPropertyRelative("spawnPosition").vector3Value = targetTransform.position;
        newElement.FindPropertyRelative("target").objectReferenceValue = targetTransform;

        serializedObject.ApplyModifiedProperties();
    }
}