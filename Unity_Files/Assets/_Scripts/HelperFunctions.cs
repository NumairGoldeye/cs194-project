using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HelperFunctions{

	public static void Shuffle<T>(this List<T> list)  //from http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
	{   
		for (int i = list.Count - 2; i >= 0; i--) { 
			int k = Random.Range (i, list.Count);
			T value = list[k];  
			list[k] = list[i];  
			list[i] = value;  
		}  
	}
}
