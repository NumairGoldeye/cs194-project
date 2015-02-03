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

	public List<RoadClass> getRoads(int settlementIndex) {
		SettlementClass s = getSettlement(settlementIndex);
		return getRoads(s);
	}

	public List<RoadClass> getRoads(SettlementClass settlement) {
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
}
