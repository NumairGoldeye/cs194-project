using UnityEngine;
using System.Collections;


/*

ResourceType is a public enum that we should use to standardize
the resources we call everywhere


Usage:

--

ResourceType tileResource = TileClass.getMaterial();
Player p1 = Player.getPlayerById(1);
p1.addResource( tileResource );
--

-- 
// Prints each enum by name
foreach(string resName in Enum.GetNames(typeof(ResourceType))){
	Debug.Log(resName);	
}
--


*/

public enum ResourceType {sheep, wood, brick, ore, wheat, desert};

public class ResourceClass {

}
