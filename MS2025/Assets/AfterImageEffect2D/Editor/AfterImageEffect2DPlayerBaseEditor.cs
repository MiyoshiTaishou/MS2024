//  AfterImageEffect2DPlayerBaseEditor.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AIE2D {

  /// <summary>
  /// AfterImageEffect2DBaseのInspectroの表示を設定するクラス
  /// </summary>
  [CustomEditor(typeof(AfterImageEffect2DPlayerBase))]
  public class AfterImageEffect2DPlayerBaseEditor : Editor {

    //表示を日本語にするか
    protected static bool _isJapanese = false;
    private   static string IS_JAPANESE_SAVE_KEY = "AfterImageEffect2DPlayerEditor_IsJapanese";

    //=================================================================================
    //初期化
    //=================================================================================

    private void OnEnable() {
      _isJapanese = !string.IsNullOrEmpty(EditorUserSettings.GetConfigValue(IS_JAPANESE_SAVE_KEY));
    }

    //=================================================================================
    //更新
    //=================================================================================

    //Inspectorを更新する
    public override void OnInspectorGUI() {

      //表示を日本語にするか
      bool wasJapanese = _isJapanese;
      _isJapanese = EditorGUILayout.ToggleLeft("Japanese", _isJapanese);

      //変更があったら保存
      if (_isJapanese != wasJapanese) {
        EditorUserSettings.SetConfigValue(IS_JAPANESE_SAVE_KEY, _isJapanese ? "True" : "");
      }

    }

    //基本情報を設定するGUIを表示
    protected void ShowBasicGUI(){
      //残像を付ける対象のレンダラー
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        SerializedProperty targetSpriteRendererProperty = serializedObject.FindProperty("_targetSpriteRenderer");
        targetSpriteRendererProperty.objectReferenceValue = EditorGUILayout.ObjectField(_isJapanese ? "残像を付ける対象" : "Target Sprite Renderer", targetSpriteRendererProperty.objectReferenceValue, typeof(SpriteRenderer), true) as SpriteRenderer;
      }EditorGUILayout.EndVertical();

      //Time.timescaleを無視するか
      EditorGUILayout.BeginVertical(GUI.skin.box);{
        SerializedProperty ignoreTimeScaleProperty = serializedObject.FindProperty("_ignoreTimeScale");
        ignoreTimeScaleProperty.boolValue = EditorGUILayout.ToggleLeft(_isJapanese ? "Time.timescaleを無視するか" : "Ignore Time Scale", ignoreTimeScaleProperty.boolValue);
      }EditorGUILayout.EndVertical();
    }

    //残像の色を全部同じにするかを設定するGUIを表示
    protected bool ShowIsSameColorGUI() {
      SerializedProperty isSameColorProperty = serializedObject.FindProperty("_isSameColor");
      isSameColorProperty.boolValue = EditorGUILayout.ToggleLeft(_isJapanese ? "残像の色を全部同じにする" : "Is Same Color", isSameColorProperty.boolValue);
      serializedObject.ApplyModifiedProperties();//一旦反映(isSameColorPropertyとplayerでズレが生じると次の色の設定で問題が起こるため)

      return isSameColorProperty.boolValue;
    }

    //色を設定するGUIを表示
    protected void ShowColorGUI(AfterImageEffect2DPlayerBase player, bool isSameColor, int colorNum) {
      EditorGUI.indentLevel++;{
        if (isSameColor) {
          player.SetColorIfneeded(EditorGUILayout.ColorField(_isJapanese ? "残像の色" : "After Image Color", player.GetColor(0)));
        }
        else {
          List<Color> colorList = new List<Color>();
          for (int i = 0; i < colorNum; i++) {
            string noText = (i + 1).ToString();
            colorList.Add(EditorGUILayout.ColorField(_isJapanese ? noText + "つめの残像の色" : "Color" + noText, player.GetColor(i)));
          }
          player.SetColorListIfneeded(colorList);
        }

      }EditorGUI.indentLevel--;
    }

    //マテリアルの種類を設定するGUIを表示
    protected MaterialType ShowMaterialTypeGUI() {
      SerializedProperty currentMaterialTypeProperty = serializedObject.FindProperty("_materialType");
      MaterialType currentMaterialType = (MaterialType)Enum.ToObject(typeof(MaterialType), currentMaterialTypeProperty.enumValueIndex);
      serializedObject.ApplyModifiedProperties();//一旦反映(currentMaterialTypeとplayerでズレが生じると次のマテリアルの設定で問題が起こるため)

      if (_isJapanese) {
        currentMaterialTypeProperty.enumValueIndex = EditorGUILayout.Popup("マテリアルの設定", currentMaterialTypeProperty.enumValueIndex, new string[] { "対象のレンダラーと同じ", "全部同じのを設定", "個別に設定" });
        currentMaterialType = (MaterialType)Enum.ToObject(typeof(MaterialType), currentMaterialTypeProperty.enumValueIndex);
      }
      else {
        currentMaterialType = (MaterialType)EditorGUILayout.EnumPopup("Material Type", currentMaterialType);
        currentMaterialTypeProperty.enumValueIndex = (int)currentMaterialType;
      }

      return currentMaterialType;
    }

    //マテリアルを設定するGUIを表示
    protected void ShowMaterialGUI(AfterImageEffect2DPlayerBase player, MaterialType materialType, int materialNum) {
      EditorGUI.indentLevel++;{
        
        if (materialType == MaterialType.SameTarget) {
          player.SetMaterialIfneeded(null);
        }
        else if (materialType == MaterialType.Collective) {
          player.SetMaterialIfneeded(EditorGUILayout.ObjectField(_isJapanese ? "残像のマテリアル" : "After Image Material", player.GetMaterial(0), typeof(Material), true) as Material);
        }
        else if (materialType == MaterialType.Individual) {
          List<Material> materialList = new List<Material>();
          for (int i = 0; i < materialNum; i++) {
            string noText = (i + 1).ToString();
            materialList.Add(EditorGUILayout.ObjectField(_isJapanese ? noText + "つめの残像のマテリアル" : "Material" + noText, player.GetMaterial(i), typeof(Material), true) as Material);
          }
          player.SetMaterialListIfneeded(materialList);
        }

      }EditorGUI.indentLevel--;
    }

  }

}