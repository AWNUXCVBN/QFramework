﻿using UnityEngine;
using System.Collections;
using QFramework;

/// <summary>
/// 全局唯一继承于MonoBehaviour的单例类，保证其他公共模块都以App的生命周期为准
/// </summary>
public class App : QMonoSingleton<App>
{
    public delegate void LifeCircleCallback();

    public LifeCircleCallback onUpdate = null;
	public LifeCircleCallback onFixedUpdate = null;
	public LifeCircleCallback onLatedUpdate = null;
    public LifeCircleCallback onGUI = null;
    public LifeCircleCallback onDestroy = null;
    public LifeCircleCallback onApplicationQuit = null;

	void Awake()
	{
		// 确保不被销毁
		DontDestroyOnLoad(gameObject);

		mInstance = this;

		// 进入欢迎界面
		Application.targetFrameRate = 60;
	}
		
	void  Start()
    {
		CoroutineMgr.Instance ().StartCoroutine (ApplicationDidFinishLaunching());
	}

	IEnumerator ApplicationDidFinishLaunching()
	{
		// 做一些配置
		Setting.Load();
		Logger.Instance ();

		yield return GameManager.Instance ().Init ();

		yield return GameManager.Instance ().Launch ();

		yield return null;
	}


    void Update()
    {
        if (this.onUpdate != null)
            this.onUpdate();
    }

	void FixedUpdate()
	{
		if (this.onFixedUpdate != null)
			this.onFixedUpdate ();

	}

	void LatedUpdate()
	{
		if (this.onLatedUpdate != null)
			this.onLatedUpdate ();
	}

    void OnGUI()
    {
        if (this.onGUI != null)
            this.onGUI();
    }

	protected override void OnDestroy() 
    {
		base.OnDestroy ();

        if (this.onDestroy != null)
            this.onDestroy();
    }

    void OnApplicationQuit()
    {
        if (this.onApplicationQuit != null)
            this.onApplicationQuit();
    }
}
