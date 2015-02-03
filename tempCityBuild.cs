
/** Returns a list of indexes for vertexes where the given player can build a city. */
public List<int> possibleCitiesForPlayer(int playerId) {
  List<int> result = new List<int>();
  Player p = getPlayerForId(playerId);
  
  for (int i = 0; i < graph.VertexCount; i++) {
    if (canBuildCity(i, playerId)) {
      result.Add(i);
    }
  }
  return result;
}

private bool canBuildCity(int vertexId, int playerId) {
  Vertex v = graph.getVertex(vertexId);
  Player p = TurnState.players[playerId];
  return BuyManager.PlayerCanBuy(p, BuyableType.city) && 
    v.hasSettlement() && v.ownerId == playerId
}

/** Returns a list of indexes for vertexes where the given player can build a settlement. */
public List<int> possibleSettlementsForPlayer(int playerId) {
    List<int> result = new List<int>();
    Player p = getPlayerForId(playerId);
    
    for (int i = 0; i < graph.VertexCount; i++) {
      if (canBuildSettlement(i, playerId)) {
        result.Add(i);
      }
    }
    return result;
}

private bool canBuildSettlement(int vertexId, int playerId) {
  Vertex v = graph.getVertex(vertexId);
  // distanceToNearestBuilding to be written by Kevin
  return BuyManager.PlayerCanBuy(p, BuyableType.settlment) && !v.hasCity() && !v.hasSettlement() && 
    playerHasAdjacentRoad(v, playerId) && 2 >= distanceToNearestBuilding(v);
}

private bool playerHasAdjacentRoad(Vertex v, int playerId) {
  foreach (Edge e in graph.getEdges(v)) {
    if (e.hasRoad() && e.road.ownerId == playerId) {
      return true;
    }
  }
  return false;
}

/** Returns a list of indexes for Edges where the given player can build a road. */
public List<int> possibleRoadsForPlayer(int playerId) {
    List<int> result = new List<int>();
    Player p = getPlayerForId(playerId);
    
    for (int i = 0; i < graph.EdgeCount; i++) {
      if (canBuildRoad(i, playerId)) {
        result.Add(i);
      }
    }
    return result;
}

private bool canBuildRoad(int edgeId, int playerId) {
    Edge e = graph.getEdge(edgeId);
    if (e.hasRoad()) {
      return false;
    }
    return BuyManager.PlayerCanBuy(p, BuyableType.road) &&
      (playerHasAdjacentRoad(e, playerId) || playerHasAdjacentBuilding(e, playerId));
}

private bool playerHasAdjacentRoad(Edge e, int playerId) {
    List<Edge> adjacentEdges = getAdjacentEdges(e); // Kevin to write this function
    foreach (Edge adj in adjacentEdges) {
      RoadClass road = adj.road;
      if (road.isBuilt() && road.ownerId == playerId) {
        return true;
      }
    }
    return false;
}

private bool playerHasAdjacentBuilding(Edge e, int playerId) {
  return playerHasBuildingOnVertex(e.vertex1, playerId) ||
    playerHasBuildingOnVertex(e.vertex2, playerId);
}

private bool playerHasBuildingOnVertex(Vertex v, int playerId) {
  if (v.hasSettlement() && v.settlement.ownerId == playerId) {
    return true;
  } else if (v.hasCity() && v.city.ownderId == playerId) {
    return true;
  }
  return false;
}

/***************************** Actual Building ***********************/

public void buildRoad(Road road, Player p) {
  BuyManager.PurchaseForPlayer(BuyableType.road, p);
  road.ownerId = p.playerId;
  road.built = true;
  // TODO: How exactly does this work?
  road.setColor(p.color);
  road.makeVisible();
}

public void buildCity(CityClass city, SettlementClass settlment, Player p) {
  BuyManager.PurchaseForPlayer(BuyableType.city, p);
  settlement.built = false;
  city.built = true;
  city.ownerId = p.playerId
  // TODO: How exactly does this work?
  city.setColor(p.color);
  city.makeVisible();
  settlement.makeInvisible();
}

public void buildSettlment(SettlementClass settlment) {
  BuyManager.PurchaseForPlayer(BuyableType.settlement, p);
  settlement.ownerId = p.playerId;
  settlement.built = true;
  // TODO: How exactly does this work?
  settlement.setColor(p.color);
  settlement.makeVisible();
}