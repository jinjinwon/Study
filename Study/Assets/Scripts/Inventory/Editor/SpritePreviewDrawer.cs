using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Sprite))]
public class SpritePreviewDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Label 및 Object Field 표시
        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        Rect objectFieldRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label);
        property.objectReferenceValue = EditorGUI.ObjectField(objectFieldRect, property.objectReferenceValue, typeof(Sprite), false);

        // Sprite 미리보기 영역 설정
        if (property.objectReferenceValue is Sprite sprite)
        {
            Texture2D texture = sprite.texture;
            if (texture != null)
            {
                Rect previewRect = new Rect(
                    position.x, // 필드와 동일한 X 좌표
                    position.y + EditorGUIUtility.singleLineHeight + 5,     // Object Field 아래
                    position.width,                                         // 필드와 동일한 너비
                    position.width/2                                        // 직사각형으로 크기 설정
                );

                // UV 좌표 계산
                Rect spriteRect = sprite.rect;
                Rect texCoords = new Rect(
                    spriteRect.x / texture.width,
                    spriteRect.y / texture.height,
                    spriteRect.width / texture.width,
                    spriteRect.height / texture.height
                );

                // Sprite 미리보기 그리기
                GUI.DrawTextureWithTexCoords(previewRect, texture, texCoords);
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight + 5 + EditorGUIUtility.currentViewWidth/2;
    }
}