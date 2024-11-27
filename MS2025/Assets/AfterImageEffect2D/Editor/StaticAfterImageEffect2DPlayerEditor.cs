//  StaticAfterImageEffect2DPlayerEditor.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using UnityEditor;
using UnityEngine;

namespace AIE2D {

  /// <summary>
  /// StaticAfterImageEffect2DPlayerのInspectroの表示を設定するクラス
  /// </summary>
  [CustomEditor(typeof(StaticAfterImageEffect2DPlayer))]
  public class StaticAfterImageEffect2DPlayerEditor : AfterImageEffect2DPlayerBaseEditor {

    //=================================================================================
    //更新
    //=================================================================================

    //Inspectorを更新する
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      //更新開始
      serializedObject.Update();
      EditorGUI.BeginChangeCheck();

      //StaticAfterImageEffect2DPlayerを取得
      StaticAfterImageEffect2DPlayer player = target as StaticAfterImageEffect2DPlayer;

      //変更を戻せるようにUndoに登録
      Undo.RecordObject(player, player.gameObject.name + " (StaticAfterImageEffect2DPlayer)");

      //基本情報を設定するGUIを表示
      ShowBasicGUI();

      //残像の表示間隔
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        SerializedProperty spanProperty = serializedObject.FindProperty("_span");
        spanProperty.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(_isJapanese ? "残像の表示間隔" : "Span", spanProperty.floatValue));
      }EditorGUILayout.EndVertical();

      //残像の表示時間
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        SerializedProperty duractionProperty = serializedObject.FindProperty("_duraction");
        duractionProperty.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(_isJapanese ? "残像の表示時間" : "Duraction", duractionProperty.floatValue));
      }EditorGUILayout.EndVertical();

      //消える時にフェードにかかる時間
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        SerializedProperty fadeTimeProperty = serializedObject.FindProperty("_fadeTime");
        fadeTimeProperty.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(_isJapanese ? "消える時にフェードにかかる時間" : "Fade Time", fadeTimeProperty.floatValue));
      }EditorGUILayout.EndVertical();

      //色の設定
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        bool isSameColor = ShowIsSameColorGUI();

        int colorNum = 1;
        if(!isSameColor){
          SerializedProperty colorNumProperty = serializedObject.FindProperty("_colorNum");
          colorNumProperty.intValue = Mathf.Max(1, EditorGUILayout.IntField(_isJapanese ? "残像の色の数" : "Color Num", colorNumProperty.intValue));
          colorNum = colorNumProperty.intValue;
        }

        ShowColorGUI(player, isSameColor, colorNum);
      }
      EditorGUILayout.EndVertical();

      //マテリアルの設定
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        MaterialType materialType = ShowMaterialTypeGUI();

        int materialNum = 1;
        if(materialType == MaterialType.Individual){
          SerializedProperty materialNumProperty = serializedObject.FindProperty("_materialNum");
          materialNumProperty.intValue = Mathf.Max(1, EditorGUILayout.IntField(_isJapanese ? "残像のマテリアルの数" : "Material Num", materialNumProperty.intValue));
          materialNum = materialNumProperty.intValue;
        }

        ShowMaterialGUI(player, materialType, materialNum);
      }EditorGUILayout.EndVertical();

      //変更されていたら残像を作り直す
      if (Application.isPlaying && EditorGUI.EndChangeCheck()) {
        player.CreateAfterImage();
      }

      //更新を反映
      serializedObject.ApplyModifiedProperties();
    }

  }

}