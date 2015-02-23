using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// <summary>
/// Chat manager
/// 
/// Every thing can publish things to the ChatManager.
/// 
/// It'll 
/// 
/// </summary>
public class ChatLog : MonoBehaviour {

	//Set in inspector
	public GameObject chatInputObj;
	public GameObject chatPanelObj;
	public GameObject chatTextObj;
	public GameObject chatScrollObj;
	public EventSystem foo; 

	private InputField chatInput;
	private Text chatText;
	private ScrollRect chatScroll;
//	private LayoutElement textLayout;


	static ChatLog instance;

	public static ChatLog Instance {
		get {
			if (instance == null) {
				Debug.Log("found");
				instance = GameObject.FindObjectOfType<ChatLog>(); //hmmmm
			}
			return instance;
		}
	}


	public List<Message> messages;

	public enum MessageType {playerChat, gameEvent};

	public class Message{
		public string text;
		public MessageType type;

		public Message(string s, MessageType t){
			text = s;
			type = t;
		}

	}

	// Use this for initialization
	void Start () {
		messages = new List<Message>();
		chatInput = chatInputObj.GetComponent<InputField>();
		chatText = chatTextObj.GetComponent<Text>();
		chatScroll = chatScrollObj.GetComponent<ScrollRect>();
//		textLayout = chatTextObj.GetComponent<LayoutElement>();

//		chatText.text += "\n\n\n";
		RealignChatBox();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddChatMessage(string s){
		if (s.Length > 0  && Input.GetKey(KeyCode.Return)){
			messages.Add( new Message(s, MessageType.playerChat));

			//TODO assign the owner of this game to the chat, not the player
			chatText.text += "\n" + TurnState.currentPlayer.playerName + ": " + s;
			chatInput.text = "";

			FocusChatBox();

			RealignChatBox();
		}

	}

	/// <summary>
	/// Realigns the chat box to the bottom if its surrounding panel. Requires the ContentFitter -> preferredSize 
	/// attached 
	/// </summary>
	void RealignChatBox(){
		//			Left=position.x Right=sizeDelta.x PosY=position.y PosZ=position.z Height=sizeDelta.y
		
		
		
		//			Vector2 tempSD = chatText.rectTransform.sizeDelta;
		//
		//			Debug.Log(tempSD);
		//			Debug.Log(chatText.rectTransform.sizeDelta);
		//
		//			tempSD.y = textLayout.preferredHeight;
		//
		//			Debug.Log (textLayout.preferredHeight);
		//			Debug.Log(tempSD);
		//
		//			chatText.rectTransform.sizeDelta = new Vector2(tempSD.x, tempSD.y);
		//
		//			Debug.Log(tempSD);
		//			Debug.Log(chatText.rectTransform.sizeDelta);
		
		
		Vector2 temp = chatText.rectTransform.anchoredPosition;
		temp.y = chatText.rectTransform.sizeDelta.y;
		chatText.rectTransform.anchoredPosition = temp;

//		yield return null;

		chatScroll.velocity=new Vector2 (0f,1000f);
		
	}

	public void Test(){
//		Debug.Log(chatScroll.value);
	}


	public void FocusChatBox(){
		EventSystem.current.SetSelectedGameObject(chatInput.gameObject, null);
		chatInput.OnPointerClick(new PointerEventData(EventSystem.current));
	}

	void UpdateText(){

	}

	public static bool InChat(){
		if ( UIManager.state == MajorUIState.play )
			return true;

		return Instance.chatInput.isFocused;
	}


	public static void StaticReset(){
		instance = null;
	}


}
