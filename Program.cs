using System;
using GLFW;

namespace SharpEngine
{
	class Program {
		static float Lerp(float from, float to, float t) {
			return from + (to - from) * t;
		}

		static float GetRandomFloat(Random random, float min = 0, float max = 1) {
			return Lerp(min, max, (float)random.Next() / int.MaxValue);
		}
        
		static void Main(string[] args) {

			var random = new Random();
			var window = new Window();
			var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
			var scene = new Scene();
			var physics = new Physics(scene);
			window.Load(scene);
			
			for (var i = 0; i < 10; i++) {
				var circle = new Circle(material);
				var radius = GetRandomFloat(random, 0.3f);
				circle.Transform.CurrentScale = new Vector(radius, radius, 1f);
				circle.Transform.Position = new Vector(GetRandomFloat(random, -1f), GetRandomFloat(random, -1), 0f);
				circle.velocity = -circle.Transform.Position.Normalize() * GetRandomFloat(random, 0.15f, 0.3f);
				circle.Mass = MathF.PI * radius * radius;
				scene.Add(circle);
			}
			
			// var shape = new Triangle(material);
			// shape.Transform.CurrentScale = new Vector(.5f, 1f, 1f);
			// scene.Add(shape);
			//
			// var ground = new Rectangle(material);
			// ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
			// ground.Transform.Position = new Vector(0f, -1f);
			// ground.gravityScale = 0;
			// scene.Add(ground);
			//
			//
			// var rectangle = new Rectangle(material);
			// rectangle.Transform.CurrentScale = new Vector(1f, 1f, 1f);
			// rectangle.Transform.Position = new Vector(-0.2f, -0.2f);
			// scene.Add(rectangle);
			
			// var circle = new Circle(material);
			// circle.Transform.Position = Vector.Left;
			// circle.velocity = Vector.Right;
			// circle.Mass = 1f;
			// scene.Add(circle);
			//
			// var otherCircle = new Circle(material);
			// otherCircle.Transform.Position = Vector.Right * 0.5f;
			// otherCircle.Mass = 2f;
			// scene.Add(otherCircle);

			// engine rendering loop
			const int fixedStepNumberPerSecond = 30;
			const float fixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
			const int maxStepsPerFrame = 5;
			var previousFixedStep = 0.0;
			while (window.IsOpen()) {
				var stepCount = 0;
				while (Glfw.Time > previousFixedStep + fixedDeltaTime && stepCount++ < maxStepsPerFrame) {
					previousFixedStep += fixedDeltaTime;
					physics.Update(fixedDeltaTime);
				}
				window.Render();
			}
		}
	}
}
