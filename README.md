This project was created in Unity, which handled much of the graphics creation.

Rundown of the files:

SampleScene.unity: Contains all of the information for the graphics.  A large portion of
  our work was done in the Unity scene editor -- that work in unity is reflected here. 
  Not meant to be read by humans, but generated and used by Unity.	

Board Representation Files:
- BoardGraph.cs: The interface that contains the methods used for manipulating the graph
- ArrayBoardGraph.cs: An implementation of BoardGraph that represents the board as a list
    of Settlement locations, Road locations, and Tiles.
- StandardBoardGraph.cs: A singleton subclass of ArrayBoardGraph that represents our graph;
    it properly links the proper edges, verticies and tiles to each other.
- RoadClass.cs / SettlementClass.cs / PortClass.cs / TileClass.cs: Each of these represent 
    the information needed for their respective game element, and contain functions to
    perform their necessary tasks.
