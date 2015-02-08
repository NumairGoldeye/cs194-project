using System.Collections;
using System.Collections.Generic;

public class ArrayBoardGraph : BoardGraph {

	protected List<SettlementClass> settlements = new List<SettlementClass>();
	protected List<RoadClass> roads = new List<RoadClass>();
	protected List<TileClass> tiles = new List<TileClass>();

	public int RoadCount {
		get {
			return roads.Count;
		}
	}

	public int SettlementCount {
		get {
			return settlements.Count;
		}
	}

	public int TileCount {
		get {
			return tiles.Count;
		}
	}

	public SettlementClass getSettlement(int index) {
		return settlements[index];
	}

	public List<RoadClass> getConnectedRoads(SettlementClass settlement) {
		List<RoadClass> result = new List<RoadClass>();
		foreach (RoadClass rd in roads) {
			if (rd.settlement1 == settlement || rd.settlement2 == settlement) {
				result.Add(rd);
			}
		}
		return result;
	}

	public RoadClass getRoad(int index) {
		return roads[index];
	}

	public TileClass getTile(int index) {
		return tiles[index];
	}

	public List<RoadClass> getAdjacentRoads(RoadClass road) {
		List<RoadClass> result = new List<RoadClass> ();
		foreach (SettlementClass settlement in getSettlementsForRoad(road)) {
			foreach (RoadClass road2 in getConnectedRoads(settlement)) {
				if (road2 != road) {
					result.Add(road2);
				}
			}
		}
		return result;
	}

	public List<SettlementClass> getSettlementsForTile(TileClass tile) {
		return tile.getSettlements();
	}

	public List<SettlementClass> getSettlementsForRoad(RoadClass road) {
		List<SettlementClass> result = new List<SettlementClass> ();
		result.Add (road.settlement1);
		result.Add (road.settlement2);
		return result;
	}

	public List<TileClass> getTilesForSettlement(SettlementClass settlement) {
		List<TileClass> result = new List<TileClass> ();
		foreach (TileClass t in tiles) {
			if (t.getSettlements().Contains(settlement) && !result.Contains(t)) {
				result.Add(t);
			}
		}
		return result;
	}

	public int longestroad (List<RoadClass> road){

		return 0; 
	}
	
	public List<RoadClass> BuildableRoads(Player player){

		//case 1: new postions next to built roads
		RoadClass[] roads = player.GetRoads();
		List<RoadClass> result = new List<RoadClass> ();
		foreach (RoadClass r in roads) {
			List<RoadClass> adjacent = new List<RoadClass> ();
			adjacent = getAdjacentRoads(r);
			foreach(RoadClass r2 in adjacent){
				if(!r2.isBuilt() && !result.Contains(r2)){
					result.Add(r2);
				}
			}
		}

		//case 2: new positions next to built settlements
		SettlementClass[] settlements = player.GetSettlementss();
		foreach (SettlementClass set in settlements) {
			List<RoadClass> adjacent2 = new List<RoadClass> ();
			adjacent2 = getConnectedRoads(set);
			foreach(RoadClass r2 in adjacent2){
				if(!r2.isBuilt() && !result.Contains(r2)){
					result.Add(r2);
				}
			}
		}
		
		return result; 
	}
	
	public List<SettlementClass> BuildableSettlements(List<RoadClass> road, List<SettlementClass> settlement){
		return settlement; 

	}
	
	
	public List<SettlementClass> BuildableCity(List<SettlementClass> settlement){
		//cities are buildable where there are settlements but not city; 
		List<SettlementClass> result = new List<SettlementClass> ();
		foreach (SettlementClass set in settlements) {
			if (!set.isCity()) {
				result.Add(set);
			}
		}
		return result; 
	}

}
