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

	public override bool Equals(System.Object o) {
		Edge other = o as Edge;
		if ((object)other == null) {
			return false;
		}
		return this.Equals(other);
	}

	public bool Equals(Edge other) {
		return this.vertex1 == other.vertex1 && this.vertex2 == other.vertex2 && this.road == other.road;
	}
}