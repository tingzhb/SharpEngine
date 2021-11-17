namespace SharpEngine {
	public class Physics {
		public Scene scene;
		
		public Physics(Scene scene) {
			this.scene = scene;
		}
		public void Update(float deltaTime) {
			for (int i = 0; i < this.scene.shapes.Count; i++) {
				Shape shape = this.scene.shapes[i];
				shape.Transform.Position += shape.velocity * deltaTime;
				var acceleration = shape.linearForce / Shape.mass;
				shape.Transform.Position += acceleration * deltaTime * deltaTime / 2;
				shape.velocity += acceleration * deltaTime;
			}
		}
	}
}
