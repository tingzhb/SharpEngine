﻿using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{        
    public class Triangle {
        private Vertex[] vertices;
        public Triangle(Vertex[] vertices) {
            this.vertices = vertices;
            CurrentScale = 1f;
        }
        public float CurrentScale {get; private set;}

        public Vector GetMaxBounds() {
            
            var max = vertices[0].position;
            for (var i = 0; i < vertices.Length; i++) {
                max = Vector.Max(max, vertices[i].position);
            }
            return max;
        }

        public Vector GetMinBounds() {
            var min = vertices[0].position;
            for (var i = 0; i < vertices.Length; i++) {
                min = Vector.Min(min, vertices[i].position);
            }
            return min;
        }

        public void Scale(float multiplier) {
            // Find center of object
            var center = (GetMinBounds() + GetMaxBounds()) / 2;
                
            // Move object to center
            for (var i = 0; i < vertices.Length; i++) {
                vertices[i].position -= center;
            }
                
            // Scale down to X% and up
            for (var i = 0; i < vertices.Length; i++) {
                vertices[i].position *= multiplier;
            }
            
            // Move object back
            for (var i = 0; i < vertices.Length; i++) {
                vertices[i].position += center;
            }
            this.CurrentScale *= multiplier;
        }
        public void Move(Vector direction) {
            for (var i = 0; i < vertices.Length; i++) {
                vertices[i].position += direction;
            }
        }
        public unsafe void Render() {
            fixed (Vertex* vertex = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
        }
    }
    class Program {
        private static Triangle triangle = new Triangle(
            new Vertex[] {
                new Vertex(new Vector(-.1f, -.1f), Color.Green),
                new Vertex(new Vector(.1f, -.1f), Color.Blue),
                new Vertex(new Vector(-.0f, .1f), Color.Red),
            
                new Vertex(new Vector(-.4f, -.4f), Color.Red),
                new Vertex(new Vector(-.2f, -.4f), Color.Green),
                new Vertex(new Vector(-.3f, -.2f), Color.Blue),
            }
       );

        static void Main(string[] args) {
            
            var window = CreateWindow();

            LoadTriangleIntoBuffer();

            CreateShaderProgram();
            var direction = new Vector(0.015f,0.02f);
            var multiplier = 0.9f;
            // engine rendering loop
            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);
                
                triangle.Scale(multiplier);
                
                if (triangle.CurrentScale <= 0.2f) {
                    multiplier = 1.05f;
                }
                
                if (triangle.CurrentScale >= 2f) {
                    multiplier = 0.95f;
                }
                
                triangle.Move(direction);
                

                if (triangle.GetMaxBounds().x >= 1 && direction.x > 0 || triangle.GetMinBounds().x <= -1 && direction.x < 0) {
                    direction.x *= -1;
                }
                
                if (triangle.GetMaxBounds().y >= 1 && direction.y >= 0 || triangle.GetMinBounds().y <= -1 && direction.y <= 0) {
                    direction.y *= -1;
                }
            }
        }
        
        private static void Render(Window window) {
            triangle.Render();
            Glfw.SwapBuffers(window);
        }
        private static void ClearScreen() {
            glClearColor(.0f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        static void CreateShaderProgram() {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/position-color.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/vertex-color.frag"));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }

        static unsafe void LoadTriangleIntoBuffer() {

            // load the vertices into a buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }

        static Window CreateWindow() {
            // initialize and configure
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}