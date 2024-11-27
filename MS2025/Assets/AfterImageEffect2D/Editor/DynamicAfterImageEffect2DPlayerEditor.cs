//  DynamicAfterImageEffect2DPlayerEditor.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AIE2D {

  /// <summary>
  /// DynamicAfterImageEffect2DPlayerのInspectroの表示を設定するクラス
  /// </summary>
  [CustomEditor(typeof(DynamicAfterImageEffect2DPlayer))]
  public class DynamicAfterImageEffect2DPlayerEditor : AfterImageEffect2DPlayerBaseEditor {

    //=================================================================================
    //更新
    //=================================================================================

    //Inspectorを更新する
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      //更新開始
      serializedObject.Update();
      EditorGUI.BeginChangeCheck();

      //DynamicAfterImageEffect2DPlayerを取得
      DynamicAfterImageEffect2DPlayer player = target as DynamicAfterImageEffect2DPlayer;

      //変更を戻せるようにUndoに登録
      Undo.RecordObject(player, player.gameObject.name + " (DynamicAfterImageEffect2DPlayer)");

      //基本情報を設定するGUIを表示
      ShowBasicGUI();

      //残像の数
      int colorAndMaterialNum = 0;
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        SerializedProperty afterImageNumProperty = serializedObject.FindProperty("_afterImageNum");
        afterImageNumProperty.intValue = Mathf.Max(1, EditorGUILayout.IntField(_isJapanese ? "残像の数" : "After Image Num", afterImageNumProperty.intValue));
        colorAndMaterialNum = afterImageNumProperty.intValue;
      }EditorGUILayout.EndVertical();

      //残像の間の長さ
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        SerializedProperty intervalProperty = serializedObject.FindProperty("_interval");
        intervalProperty.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(_isJapanese ? "残像の間の長さ" : "Interval", intervalProperty.floatValue));
      }EditorGUILayout.EndVertical();

      //残像を表示する距離の範囲
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        SerializedProperty distanceRangeProperty = serializedObject.FindProperty("_distanceRange");
        distanceRangeProperty.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(_isJapanese ? "残像を表示する距離の範囲" : "Distance Range", distanceRangeProperty.floatValue));
      }EditorGUILayout.EndVertical();

      //色の設定
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        bool isSameColor = ShowIsSameColorGUI();
        ShowColorGUI(player, isSameColor, colorAndMaterialNum);
      }EditorGUILayout.EndVertical();

      //マテリアルの設定
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        MaterialType materialType = ShowMaterialTypeGUI();
        ShowMaterialGUI(player, materialType, colorAndMaterialNum);
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