using UnityEngine;
using System.Collections;
using GX;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

[ExecuteInEditMode]
public class RoleController : MonoBehaviour
{
#if false
	public Color _Color = Color.white;
	public float _RimWidth = 0.7f;
	public Color _RimColor = Color.white;
#else 
	public Color _RimColor = new Color(1, 1, 1, 0.5f);
	public Color _InnerColor = new Color(1, 1, 1, 0.5f);
	[Range(0.0f, 1.0f)]
	public float _InnerColorPower = 1f;
	[Range(0.0f, 5.0f)]
	public float _RimPower = 5f;
	[Range(0.0f, 8.0f)]
	public float _AlphaPower = 8.0f;
	[Range(0.0f, 10.0f)]
	public float _AllPower = 10.0f;

    [Header("GlowParameter")]
    public Color _Color = Color.white;
    [Range(0f,1f)]
    public float _Cutoff = 0.5f;
    [Range(0.5f,8.0f)]
    public float _Power = 3f;
#endif

	List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();

	public IEnumerable<SkinnedMeshRenderer> GetSkinnedMeshRenderer(Transform transform)
	{
		if (transform == null)
			return null;
		else
			return transform.GetComponentsDescendant<SkinnedMeshRenderer>();
	}
#if false
	void OnGUI()
	{
		GUI.Label(new Rect(100, 10, 100, 30), "鼠标右击");
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			if (this.skinnedMeshRenderer == null)
				return;
			// 设置发光材质
#if false
			Hashtable shaderParams = new Hashtable();
			shaderParams.Add("_Color", _Color);
			shaderParams.Add("_RimColor", _RimColor);
			shaderParams.Add("_RimWidth", _RimWidth);

			MaterialController.SetLightMaterial(skinnedMeshRenderer, 2, shaderParams);
#else
			Hashtable shaderParams = new Hashtable();
			shaderParams.Add("_RimColor", _RimColor);
			shaderParams.Add("_InnerColor", _InnerColor);
			shaderParams.Add("_InnerColorPower", _InnerColorPower);
			shaderParams.Add("_RimPower", _RimPower);
			shaderParams.Add("_AlphaPower", _AlphaPower);
			shaderParams.Add("_AllPower", _AllPower);

			MaterialController.SetLightMaterial(skinnedMeshRenderer, 1, shaderParams);
#endif
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (this.skinnedMeshRenderer == null)
				return;
			// 取消设置发光材质
			MaterialController.CancelSetLightMaterial(skinnedMeshRenderer);
		}
	}
#else
	public void ShowLightEffect()
	{
		this.skinnedMeshRenderers.AddRange(this.GetSkinnedMeshRenderer(this.transform).ToArray());
		if (this.skinnedMeshRenderers.Count <= 0)
			return;
		Hashtable shaderParams = new Hashtable();
		shaderParams.Add("_RimColor", _RimColor);
		shaderParams.Add("_InnerColor", _InnerColor);
		shaderParams.Add("_InnerColorPower", _InnerColorPower);
		shaderParams.Add("_RimPower", _RimPower);
		shaderParams.Add("_AlphaPower", _AlphaPower);
		shaderParams.Add("_AllPower", _AllPower);
		this.skinnedMeshRenderers.ForEach(i =>
		{
			MaterialController.SetLightMaterial(i, 1, shaderParams);
		});
		
	}

    public void ShowOutlineGlow()
    {
        this.skinnedMeshRenderers.AddRange(this.GetSkinnedMeshRenderer(this.transform).ToArray());
        if (this.skinnedMeshRenderers.Count <= 0)
            return;
        Hashtable shaderParams = new Hashtable();
        shaderParams.Add("_Color", _Color);
        shaderParams.Add("_Cutoff", _Cutoff);
        shaderParams.Add("_Power", _Power);
        this.skinnedMeshRenderers.ForEach(i =>
        {
            MaterialController.SetLightMaterial(i, CustomShader.OUTLINE_GLOW, shaderParams);
        });
    }

    public void ShowEdgeGlow()
    {
        this.skinnedMeshRenderers.AddRange(this.GetSkinnedMeshRenderer(this.transform).ToArray());
        if (this.skinnedMeshRenderers.Count <= 0)
            return;
        Hashtable shaderParams = new Hashtable();
        shaderParams.Add("_RimColor", _RimColor);
        shaderParams.Add("_InnerColor", _InnerColor);
        shaderParams.Add("_InnerColorPower", _InnerColorPower);
        shaderParams.Add("_RimPower", _RimPower);
        shaderParams.Add("_AlphaPower", _AlphaPower);
        shaderParams.Add("_AllPower", _AllPower);
        this.skinnedMeshRenderers.ForEach(i =>
        {
            MaterialController.SetLightMaterial(i, CustomShader.Edge_GLOW, shaderParams);
        });
    }

	public void HideLightEffect()
	{
		if (this.skinnedMeshRenderers.Count <= 0)
			return;
		this.skinnedMeshRenderers.ForEach(i =>
		{
			MaterialController.CancelSetLightMaterial(i);
			MaterialController.CancelSetMaterial(i,CustomShader.OUTLINE_GLOW);
			MaterialController.CancelSetMaterial(i,CustomShader.Edge_GLOW);
		});
		this.skinnedMeshRenderers.Clear();
	}
#endif
}
