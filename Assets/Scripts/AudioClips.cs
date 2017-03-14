using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

public static class ListExtensions {
	public static T GetAny<T>(this List<T> list) {
		return list[Random.Range(0, list.Count - 1)];
	}
}

public class AudioClips : MonoBehaviour {
	public static AudioClips Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<AudioClips>();
			}
			return instance;
		}
	}

	private static AudioClips instance;

	//----------------------------//
	// Abilities sound effects   //
	//--------------------------//
	[System.Serializable]
	public class Abilities_ {
		public List<AudioClip> Recharged;
		public List<AudioClip> Used;
		public List<AudioClip> RollingBallCracking;
		public List<AudioClip> RollingBallDestroyed;
		public List<AudioClip> WindGust;
	}
	public Abilities_ Abilities;


	//--------------------------//
	// Ambient sound effects   //
	//------------------------//
	[System.Serializable]
	public class Ambience_ {
		public List<AudioClip> OceanWaves;
	}
	public Ambience_ Ambience;


	//-----------------------------//
	// Cannonball sound effects   //
	//---------------------------//
	[System.Serializable]
	public class Cannonball_ {
		public List<AudioClip> SandImpact;
		public List<AudioClip> Whistling;
	}
	public Cannonball_ Cannonball;


	//----------------------------//
	// Game Over sound effects   //
	//--------------------------//
	[System.Serializable]
	public class GameOver_ {
		public List<AudioClip> Applause;
	}
	public GameOver_ GameOver;


	//-----------.--------------//
	// Pirates sound effects   //
	//------------------------//
	[System.Serializable]
	public class Pirates_ {
		public List<AudioClip> Death;
		public List<AudioClip> VoiceYarr;
	}
	public Pirates_ Pirates;


	//------------------------------//
	// Pirate ship sound effects   //
	//----------------------------//
	[System.Serializable]
	public class PirateShip_ {
		public List<AudioClip> Fire;
	}
	public PirateShip_ PirateShip;


	//------------------------//
	// UI sound effects      //
	//----------------------//
	[System.Serializable]
	public class UI_ {
		public List<AudioClip> MenuBack;
		public List<AudioClip> MenuSelect;
	}
	public UI_ UI;


	//------------------------//
	// Walls sound effects   //
	//----------------------//
	[System.Serializable]
	public class Walls_ {
		public List<AudioClip> Destroyed;
		public List<AudioClip> Grabbed;
		public List<AudioClip> Rotation;
	}
	public Walls_ Walls;
}
