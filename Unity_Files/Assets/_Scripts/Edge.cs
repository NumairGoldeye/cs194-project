/** An edge is defined by the two verticies it connects and the road on it. */
public class Edge {
	public Vertex vertex1 { private set; get; }
	public Vertex vertex2 { private set; get; }
	public RoadClass road  { private set; get; }
	
	public Edge (Vertex vertex1, Vertex vertex2) {
		this.vertex1 = vertex1;
		this.vertex2 = vertex2;
		this.road = null;
	}
	
	public bool hasRoad() {
		return road.isBuilt();
	}
}