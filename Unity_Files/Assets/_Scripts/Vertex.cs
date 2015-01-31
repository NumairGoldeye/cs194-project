using System;
using System.Collections.Generic;

/** A vertex is defined by the tiles that surround it, and its city and settlement.*/
public class Vertex {
	
	// Note: Every vertex has a city and settlment assocaiated with it,
	// but they are not necessarily active.
	public CityClass city { get; set; }
	public SettlementClass settlement { get; set; }
	
	public List<TileClass> tiles;
	public List<PortClass> ports;
	
	public Vertex (List<TileClass> tiles, List<PortClass> ports, CityClass city, SettlementClass settlement) {
		this.tiles = tiles;
		this.ports = ports;
		this.city = city;
		this.settlement = settlement;
	}
	
//	public bool hasCity() {
//		// TODO: When city has "isBuilt" method, call it.
//	}
	
	public bool hasSettlement() {
		return settlement.isBuilt();
	}

}