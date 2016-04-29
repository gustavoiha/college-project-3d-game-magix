﻿using UnityEngine;
using System.Collections;

public class BossShieldScript : MonoBehaviour {

	// boss object
	public GameObject boss;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startBoss(){
		//Debug.Log ("starting boss!");
		Transform bossPosition = GameObject.FindGameObjectWithTag ("BossPosition").transform;

		GameObject newBoss = Instantiate (boss, bossPosition.position, bossPosition.rotation) as GameObject;

		Destroy (bossPosition.gameObject);

		Destroy (gameObject);

	}
}