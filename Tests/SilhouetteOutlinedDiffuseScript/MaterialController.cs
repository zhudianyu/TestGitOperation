﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CustomShader
{
    OUTLINE_GLOW = 0,
    Edge_GLOW,
}
public class MaterialController
{
	private static string LIGHT_SHADER_PATH_1 = "Custom/LightShader";
	private static string LIGHT_SHADER_PATH_2 = "Custom/LightShader1";
    private static string OUTLINE_GLOW = "Custom/OutlineGlow";
    private static string Edge_GLOW = "Custom/EdgeGlow";
	private static string LIGHT_MATERIAL_NAME = "LightMaterial";
    
	/// <summary>
	/// 设置发光材质
	/// </summary>
	/// <param name="renderer">Renderer.</param>
	public static void SetLightMaterial(Renderer renderer, int materialIndex = 1, Hashtable shaderParams = null)
	{
		if (renderer == null)
			return;

		string shaderName = LIGHT_SHADER_PATH_1;
		if (materialIndex == 2) shaderName = LIGHT_SHADER_PATH_2;
        if (materialIndex == 3) shaderName = OUTLINE_GLOW;
        if (materialIndex == 4) shaderName = Edge_GLOW;

		SetMaterial(renderer, Shader.Find(shaderName), LIGHT_MATERIAL_NAME, shaderParams);
	}

    public static void SetLightMaterial(Renderer renderer,CustomShader type,Hashtable shaderParams = null)
    {
        if (renderer == null) return;
        string shaderName = GetShaderName(type);
        SetMaterial(renderer, Shader.Find(shaderName), LIGHT_MATERIAL_NAME, shaderParams);
    }

    private static string GetShaderName(CustomShader type)
    {
        switch (type)
        {
            case CustomShader.OUTLINE_GLOW:return OUTLINE_GLOW;
            case CustomShader.Edge_GLOW:return Edge_GLOW;
            default:
                return LIGHT_SHADER_PATH_1;
        }
    }

    /// <summary>
	/// 取消设置发光材质
	/// </summary>
	/// <returns><c>true</c> if cancel set gray material the specified renderer; otherwise, <c>false</c>.</returns>
	/// <param name="renderer">Renderer.</param>
	public static void CancelSetLightMaterial(Renderer renderer)
	{
		if (renderer == null)
			return;

		CancelSetMaterial(renderer, LIGHT_MATERIAL_NAME);
	}

    public static void CancelSetMaterial(Renderer renderer,CustomShader type)
    {
        if (renderer == null)
            return;

        CancelSetMaterial(renderer, GetShaderName(type));
    }

	/// <summary>
	/// 添加材质
	/// </summary>
	/// <param name="renderer">Renderer.</param>
	/// <param name="shader">Shader.</param>
	/// <param name="materialName">Material name.</param>
	private static void SetMaterial(Renderer renderer, Shader shader, string materialName, Hashtable shaderParams)
	{
		if (renderer == null || shader == null || string.IsNullOrEmpty(materialName))
			return;

		if (GetMaterial(renderer, materialName) == null)
		{
			Material[] sharedMaterials = renderer.sharedMaterials;
			int materialLength = sharedMaterials.Length;

			Material[] materialList = new Material[materialLength * 2];
			for (int index = 0; index < materialLength; index++)
			{
				materialList[index] = sharedMaterials[index];
				Material material = new Material(shader);
				material.name = materialName;

				if (material.HasProperty("_MainTex"))
				{
					if (shaderParams != null && shaderParams.ContainsKey("_MainTex"))
					{
						material.SetTexture("_MainTex", (Texture)shaderParams["_MainTex"]);
					}
					else
					{
						if (sharedMaterials[index].HasProperty("_MainTex"))
						{
							Texture mainTexture = sharedMaterials[index].GetTexture("_MainTex");
							if (mainTexture != null) material.SetTexture("_MainTex", mainTexture);
						}
					}
				}

				if (material.HasProperty("_BumpMap"))
				{
					if (shaderParams != null && shaderParams.ContainsKey("_BumpMap"))
					{
						material.SetTexture("_BumpMap", (Texture)shaderParams["_BumpMap"]);
					}
					else
					{
						if (sharedMaterials[index].HasProperty("_BumpMap"))
						{
							Texture bumpMap = sharedMaterials[index].GetTexture("_BumpMap");
							if (bumpMap != null) material.SetTexture("_BumpMap", bumpMap);
						}
					}
				}

				if (shaderParams != null)
				{
					foreach (DictionaryEntry dictionaryEntry in shaderParams)
					{
						if (material.HasProperty(dictionaryEntry.Key.ToString()))
						{
							if (dictionaryEntry.Value.GetType() == typeof(float))
							{
								material.SetFloat(dictionaryEntry.Key.ToString(), (float)dictionaryEntry.Value);
							}
							else if (dictionaryEntry.Value.GetType() == typeof(Color))
							{
								material.SetColor(dictionaryEntry.Key.ToString(), (Color)dictionaryEntry.Value);
							}
						}
					}
				}

				materialList[index + materialLength] = material;
			}
			renderer.sharedMaterials = materialList;
		}
	}

	/// <summary>
	/// 移除材质
	/// </summary>
	/// <returns><c>true</c> if cancel set material the specified renderer materialName; otherwise, <c>false</c>.</returns>
	/// <param name="renderer">Renderer.</param>
	/// <param name="materialName">Material name.</param>
	private static void CancelSetMaterial(Renderer renderer, string materialName)
	{
		if (renderer == null || string.IsNullOrEmpty(materialName))
			return;

		if (GetMaterial(renderer, materialName))
		{
			IList<Material> sharedMaterials = new List<Material>();
			foreach (Material material in renderer.sharedMaterials)
			{
				if (material.name != materialName)
				{
					sharedMaterials.Add(material);
				}
			}
			int materialLength = sharedMaterials.Count;

			Material[] materialList = new Material[materialLength];
			for (int index = 0; index < materialLength; index++)
			{
				materialList[index] = sharedMaterials[index];
			}
			renderer.sharedMaterials = materialList;
		}
	}


	/// <summary>
	/// 获取材质
	/// </summary>
	/// <returns>The material.</returns>
	/// <param name="renderer">Renderer.</param>
	/// <param name="grayMaterialName">Gray material name.</param>
	private static Material GetMaterial(Renderer renderer, string materialName)
	{
		Material[] materialList = renderer.sharedMaterials;
		if (materialList == null || materialList.Length == 0)
			return null;

		foreach (Material material in materialList)
		{
			if (material.name == materialName)
				return material;
		}

		return null;
	}
}
