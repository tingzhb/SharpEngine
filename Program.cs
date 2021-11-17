using System;
using System.Security.Cryptography.X509Certificates;
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
            
			var window = new Window();
			var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
			var scene = new Scene();
			var physics = new Physics(scene);
			window.Load(scene);

			// var shape = new Triangle(material);
			// shape.Transform.CurrentScale = new Vector(.5f, 1f, 1f);
			// scene.Add(shape);
			//
			// var ground = new Rectangle(material);
			// ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
			// ground.Transform.Position = new Vector(0f, -1f);
			// scene.Add(ground);
			//
			//
			// var rectangle = new Rectangle(material);
			// rectangle.Transform.CurrentScale = new Vector(1f, 1f, 1f);
			// rectangle.Transform.Position = new Vector(-0.2f, -0.2f);
			// scene.Add(rectangle);
			
			var notCircle = new Circle(material);
			notCircle.Transform.Position = Vector.Left + Vector.Backward;
			notCircle.linearForce = Vector.Right;
			scene.Add(notCircle);
			
			var circle = new Circle(material);
			circle.Transform.Position = Vector.Left;
			circle.velocity = Vector.Right;
			scene.Add(circle);
			
			

			// engine rendering loop
			var direction = new Vector(0f, -0.003f);
			const int fixedStepNumberPerSecond = 30;
			const float fixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
			const float movementSpeed = 0.5f;
			double previousFixedStep = 0.0;
			while (window.IsOpen()) {
				while (Glfw.Time > previousFixedStep + fixedDeltaTime) {
					previousFixedStep += fixedDeltaTime;
					physics.Update(fixedDeltaTime);
				}
				window.Render();
			}
		}
	}
}
