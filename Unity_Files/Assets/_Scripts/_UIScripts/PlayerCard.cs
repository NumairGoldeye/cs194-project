using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Base class For DevCard and ResourceClass
/// 
/// controsl the UI and the animations
/// </summary>
public class PlayerCard : MonoBehaviour {


	public DevCardType dType; // set in inspector
	public ResourceType rType;

	public bool isDev; // false = resourceClass
	protected Animator anim;
	protected Button btn; 
	protected PlayerHand hand;// sets in hand	
	protected int createHash;
	protected int useHash;
	protected bool animatedIn;


	// Use this for initialization
	public virtual void Start () {

		btn = gameObject.GetComponent<Button>();
		anim = GetComponent<Animator>();

		createHash = Animator.StringToHash("Base Layer.Created");
		useHash =  Animator.StringToHash("Base Layer.UseCard");


		animatedIn = false;
	}


	
	// Update is called once per frame
	void Update () {
	
	}


	public void AnimInBrute(){
		Debug.Log("FOOOOO");
		animatedIn = false;
		AnimIn();
	}

	public void AnimIn(){
		if (animatedIn) return;
//		Debug.Log ("fooooo");
		anim.Play(createHash);
		animatedIn = true;
	}

	public void AnimOut(){
//		Debug.Log ("animout");
//		btn.interactable = false;
		anim.SetBool("UseCard", true);
//		anim.Play(useHash);

		//TODO delete card after finished
	}
	
	private void _AnimOut(){

	}

	/// <summary>
	/// Called at the end of the UseCard animation event
	/// </summary>
	public void _DeleteSelf(){
		transform.SetParent(null);
		Destroy(gameObject);

		hand.RepositionCards();
	}

	public void SetHand(PlayerHand h){
		hand = h;
	}

	private IEnumerator WaitForAnimation ( Animation animation )
	{
		do
		{
			yield return null;
		} while ( animation.isPlaying );
	}
}
