/** An edge is defined by the two verticies it connects and the road on it. */
public class Edge {
	public Vertex vertex1 { private set; get; }
	public Vertex vertex2 { private set; get; }
	public int vertex1Index { private set; get; }
	public int vertex2Index { private set; get; }

	public RoadClass road  { private set; get; }

	// TODO: Consider, is it ideal to have both the actual verticies and the indicies?  Is
	// there a way to infer the information from eachother without breaking abstraction?
	public Edge (Vertex vertex1, Vertex vertex2, int vertex1Index, int vertex2Index, RoadClass road) {
		this.vertex1 = vertex1;
		this.vertex2 = vertex2;
		this.vertex1Index = vertex1Index;
		this.vertex2Index = vertex2Index;
		this.road = road;
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

	/* Compares based on the vertex indicies.  Will not work as expected if verticies and indicies are not consistent. */
	public bool Equals(Edge other) {
		return this.vertex1Index == other.vertex1Index
			&& this.vertex2Index == other.vertex2Index;
	}
}