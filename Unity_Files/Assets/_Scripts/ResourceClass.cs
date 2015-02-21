using UnityEngine;
using System.Collections;


/// <summary>
/// ResourceType is a public enum that we should use to standardize
///the resources we call everywhere
///
///
///Usage:
///
///--
///
///ResourceType tileResource = TileClass.getMaterial();
///Player p1 = Player.getPlayerById(1);
///p1.addResource( tileResource );
///--
///
///-- 
///// Prints each enum by name
///foreach(string resName in Enum.GetNames(typeof(ResourceType))){
///	Debug.Log(resName);	
///}
///--
/// </summary>
public enum ResourceType {sheep, wood, brick, ore, wheat, none};




/// <summary>
/// ResourceClass will represent a card in the player's hand..
/// </summary>
public class ResourceClass : PlayerCard{

	// Set in inspector


	public override void Start(){
		base.Start();

		isDev = false;
	}
//
//	void Update(){
//
//	}

}
