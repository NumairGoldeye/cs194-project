using UnityEngine;
using System.Collections;



/*

Debugger:

Allows us to separate our logging 
Note: this should be compiled first

Usage: 

Set the debugFlag by looking for the Debugger Object under the GlobalScript
GameObject in the Scene hierarchy. Every time you synch with github
you will have to reset this variable.

If you want All debug things to show up, 
then just set debugFlag to "All"

//Set debugFlag to 'foo' in inspector

Debugger.Log('foo', "Some string");// Prints "some string"
Debugger.Log('bar', "Some other String"); // Prints nothing

//Set debugFlag to 'All' in inspector

Debugger.Log('foo', "Some string");// Prints "some string"
Debugger.Log('bar', "Some other string"); // Prints "some other string"


*/
public class Debugger : MonoBehaviour {

	// Static
	public static Debugger instance;
	public static string currentFlag;
	public static string lastFlag;
	public static string allFlag = "All";

	// Public instance
	// Set this in the game inspector before you press play in the scene
	public string debugFlag = "All";


	// There needs to be a gameobject so that you can set the flag in the
	// inspector
	void Awake(){
		Debugger.instance = this;
		Debugger.SetFlag(debugFlag);
	}

	// try not to use SetFlag, change the flag in the debugger so 
	// your calls to SetFlag don't run in your code and mess with
	// other people's debugging
	public static void SetFlag(string newFlag){
		currentFlag = newFlag;
	}

	// Swaps the flag with the last flag. If you call this twice,
	// nothing happens. 
	public static void RevertFlag(){
		string temp = currentFlag;
		currentFlag = lastFlag;
		lastFlag = currentFlag;
	}

	public static void Log(string flag, string message){
		if (flag == currentFlag || currentFlag == allFlag){
			Debug.Log(message);
		}
	}

	public static void Log(int flag, string message ){
		Log(flag.ToString(), message);
	}


}
