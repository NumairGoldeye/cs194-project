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

	public void addMessage(string playerName, string message) {
		messages.Add( new Message(message, MessageType.playerChat));
		
		//TODO assign the owner of this game to the chat, not the player
		chatText.text += "\n" + playerName + ": " + message;

		// Does not work 
		if (playerName == GameManager.Instance.myPlayerName){
			chatInput.text = "";
		}
		
		FocusChatBox();
		RealignChatBox();
	}

	public void AddChatMessage(string s){
		if (s.Length > 0  && Input.GetKey(KeyCode.Return)){
			SendChatMessage(s, GameManager.Instance.myPlayerName);
		}
	}

	void SendChatMessage(string s, string playerName){
		GameManager.Instance.networkView.RPC ("syncChatMessage", RPCMode.All, playerName, s);
	}


	/// <summary>
	/// Use this to say things like "gained wheat"
	/// </summary>
	/// <param name="player">Player.</param>
	/// <param name="s">S.</param>
	private void AddChatEvent(Player player, string s) {
		messages.Add( new Message(s, MessageType.gameEvent));
		SendChatMessage(s, player.playerName);


		FocusChatBox();
		RealignChatBox();
	}


	public void ChatGainedResource(Player p, ResourceType r, int amount = 1){
		AddChatEvent(p, " has gained " + amount.ToString() + " " + r.ToString());
	}

	public void ChatRolledDice(int num){

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
