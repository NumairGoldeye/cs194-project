This project was created in Unity, which handled much of the graphics creation.

Instructions:
  1. This app will best be viewed in Firefox, and best with 2 different computers.
  2. Navigate both computers to https://s3-us-west-2.amazonaws.com/settlersofcatan/Web_Player.html
      (you may need to give the plugins permission to run).
  3. On one computer, click "Start Game".  Enter a name for your user, and for the game. We will call this computer the "Host"
  4. Once the above is done, on the other computer, click "Join Game".  Type in a different name in the box.  Then, click on the name of the gave you created in the previous step.  If you don't see it, hit "Refresh" until you do.
  5. On the Host, click "Start"
  6. Now you can play through a game!
  7. Refresh the page if you want to start over.

NOTES
  - It is possible to try a single-player game.  To do so, just skip step 4.  Some features, like trading with other players, won't work in this mode, but you will still be able to play repeated turns.
  - There is a console that allows you to cheat and give yourself resources and development cards by clicking the "plus", and remove them with the "minus"
  - The game will end at 10 victory points.  You can make this faster by clicking the "+" for Victory Point on the Dev Console.
  - The commit statistics page says Khalil/NumairGoldeye just has a few commits; for some reason it only shows some of them, but there are a lot more.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Rundown of the most important files.

Unity Files/Assets/SampleScene.unity: Contains all of the information for the graphics.  A large portion of
  our work was done in the Unity scene editor -- that work in unity is reflected here. 
  Not meant to be read by humans, but generated and used by Unity.	
 
The following are all Scripts, located in Unity Files/Assets/_Scripts/ and its subdirectories:

Game Setup and Networking Files
- StartGameManager.cs: Controls the "setup phase" of the game, where players place their
    initial roads and settlements.
- GameManager.cs: This file handles all of the interraction between the clients. It has a 
  networkView component attached to it (Unity specific) that allows it to send networking 
  calls to the other players. All of the functions that handle respond to these networking
  calls are in this file and allow us to maintain a synchronized state among the players.
  It also sets up the game board tiles and maintains the list of players. Finally, it contains
  information that helps the individual client identify their own player in the game
  and other various pieces of information that are relevant to the player.
- Other files: There are various networking calls made in files that update the state of the game
  or the state of players, i.e. adding resources and moving the knight. All of these calls
  utilize the Game Manager to send the necessary messages to the appropriate clients 
  and the Game Manager has the corresponding functions that handle these calls.

Board Representation Files: The following section contains the files that
    represent the state of the game board.
- BoardGraph.cs: The interface that contains the methods used for manipulating the graph
- ArrayBoardGraph.cs: An implementation of BoardGraph that represents the board as a list
    of Settlement locations, Road locations, and Tiles.
- StandardBoardGraph.cs: A singleton subclass of ArrayBoardGraph that represents our graph;
    it properly links the proper edges, verticies and tiles to each other.
- RoadClass.cs, SettlementClass.cs, PortClass.cs, and TileClass.cs: Each of these
    represents the information needed for its respective game element, and contain
    functions to perform their necessary tasks. The SettlementClass also represents the 
    city objects and the logic allowing them to be built.

Game State: These files represent various parts of the state of the game.
- Player.cs: Represents the state of a player, with things like their resource counts,
    their dev card counts, etc, and various functions to manipulate the state.
- TurnState.cs: The state of the current turn.  Manages the overall gameplay during a turn. 
  Primarily keeps track of the current player, which is essential to keeping all of the players
  in sync.

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
- BuyManager.cs: Manages buying one of the various buyable things in Catan.

Chat:
- ChatLog.cs: Handles everything related to the Chat Box. Has some networking calls that send 
  the messages to all of the clients.

ComboBox:
- There is a folder called "ComboBox".  None of this code was written by us (besides a few
    tweaks), credit goes to Kender on the unity developers forum.  Retrieved from:
    http://forum.unity3d.com/threads/a-working-stylable-combo-box-drop-down-list.264167/#post-1913899

AI: 
- The AI is close to working, but not quite there.  To see the work we've done, look at the files in the AI folder.
- To see what is there so far, create a game, but make one of the player's name "computer" (exactly, case-sensitive).
    That player will act automatically for a while, but eventually stop.
- AIBrain.cs: Interface with necessary AI functions
- AbstractAIBrain.cs: A little bit of implementation, makes it easy to subclass.
- RandomAIBrain.cs: An AIBrain that randomly chooses where to place objects.  Also cheats and gives itself extra resources
- SimpleRulesBasedAIBrain.cs: AIBrain that determines where best to place objects based on a few rules.

User Feedback
- Regarding the UI "the colors [of the hexagons on the board] are derpy". We fixed this by
    scanning all of the real board's tiles and using them instead of monocolor hexagons. 
