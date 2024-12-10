using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Sprite))]
public class SpritePreviewDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Label �� Object Field ǥ��
        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        Rect objectFieldRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label);
        property.objectReferenceValue = EditorGUI.ObjectField(objectFieldRect, property.objectReferenceValue, typeof(Sprite), false);

        // Sprite �̸����� ���� ����
        if (property.objectReferenceValue is Sprite sprite)
        {
            Texture2D texture = sprite.texture;
            if (texture != null)
            {
                Rect previewRect = new Rect(
                    position.x, // �ʵ�� ������ X ��ǥ
                    position.y + EditorGUIUtility.singleLineHeight + 5,     // Object Field �Ʒ�
                    position.width,                                         // �ʵ�� ������ �ʺ�
                    position.width/2                                        // ���簢������ ũ�� ����
                );

                // UV ��ǥ ���
                Rect spriteRect = sprite.rect;
                Rect texCoords = new Rect(
                    spriteRect.x / texture.width,
                    spriteRect.y / texture.height,
                    spriteRect.width / texture.width,
                    spriteRect.height / texture.height
                );

                // Sprite �̸����� �׸���
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