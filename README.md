This project was created in Unity, which handled much of the graphics creation.

Rundown of the most important files:

SampleScene.unity: Contains all of the information for the graphics.  A large portion of
  our work was done in the Unity scene editor -- that work in unity is reflected here. 
  Not meant to be read by humans, but generated and used by Unity.	
  
Game Setup Files
- StartGameManager.cs: Controls the "setup phase" of the game, where players place their
    initial roads and settlements.
- GameManager.cs: This file handles all of the interraction between the clients. It has a 
  networkView component attached to it (Unity specific) that allows it to send networking 
  calls to the other players. All of the functions that handle respond to these networking
  calls are in this file and allow us to maintain a synchronized state among the players.
  It also sets up the game board tiles and maintains the list of players. Finally, it contains
  information that helps the individual client identify their own player in the game
  and other various pieces of information that are relevant to the player.

Board Representation Files: The following section contains the files that
    represent the state of the game board.
- BoardGraph.cs: The interface that contains the methods used for manipulating the graph
- ArrayBoardGraph.cs: An implementation of BoardGraph that represents the board as a list
    of Settlement locations, Road locations, and Tiles.
- StandardBoardGraph.cs: A singleton subclass of ArrayBoardGraph that represents our graph;
    it properly links the proper edges, verticies and tiles to each other.
- RoadClass.cs, SettlementClass.cs, PortClass.cs, and TileClass.cs: Each of these
    represents the information needed for its respective game element, and contain
    functions to perform their necessary tasks.

Game State: These files represent various parts of the state of the game.
- Player.cs: Represents the state of a player, with things like their resource counts,
    their dev card counts, etc, and various functions to manipulate the state.
- TurnState.cs: The state of the current turn.  Manages the overall gameplay during a turn.

UI:
- UIManager.cs: Starts the UI for the game and manages when the main parts of it should show up.
- PlayerHand.cs: Represents the display for the cards in a player's hand.

Trading:
- TradeConsole.cs: Handles displaying the various panels used for trading.
- TradeManager.cs: Handles the logic for trading resources.
- TradeCounter.cs: A specialized Dictionary that acts as a counter for resources.
- ConfirmTradeButton.cs, RejectTradeButton.cs, EnterPlayerTrade.cs, EnterPortTrade.cs,
    PlayerTradeButton.cs, TradeRemoveResource.cs, TradeAddResource.cs: Each of these
    contains the logic for its corresponding button in the trade console.

Buying:
-BuyManager.cs: Manages buying one of the various buyable things in Catan.

Chat:
- ChatLog.cs: Handles everything related to the Chat Box.

ComboBox:
- There is a folder called "ComboBox".  None of this code was written by us (besides a few
    tweaks), credit goes to Kender on the unity developers forum.  Retrieved from:
    http://forum.unity3d.com/threads/a-working-stylable-combo-box-drop-down-list.264167/#post-1913899

AI: 
- ArrayBoardGraph.cs: the data structure of the board representation and graph algorithms for complex AI functions. The second half of the file includes an implementation of AI ("computer") mode that takes into account specific turn by turn strategy to optimize AI's winning strategy according to the board structure, the resources he has, and other opponents' current strategies. These AI alagorithms direct the computer mode when and where to build, what overall strategies to use, and when and what to trade with the house at the AI's turn.  

User Feedback
- Regarding the UI "the colors [of the hexagons on the board] are derpy". We fixed this by
    scanning all of the real board's tiles and using them instead of monocolor hexagons. 
