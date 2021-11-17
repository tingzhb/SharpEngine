namespace SharpEngine {
	public class Physics {
		public Scene scene;
		
		public Physics(Scene scene) {
			this.scene = scene;
		}
		public void Update(float deltaTime) {
			var gravitationalAcceleration = Vector.Down * 0.981f;
			for (int i = 0; i < this.scene.shapes.Count; i++) {
				Shape shape = this.scene.shapes[i];
				
				//Linear Velocity
				shape.Transform.Position += shape.velocity * deltaTime;
				// a = F/m 
				var acceleration = shape.linearForce / shape.mass;
				// Add Gravity to Acceleration
				acceleration += gravitationalAcceleration * shape.gravityScale;
				// Linear Acceleration
				shape.Transform.Position += acceleration * deltaTime * deltaTime / 2;
				shape.velocity += acceleration * deltaTime;
			}
		}
	}
}
