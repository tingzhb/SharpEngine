using System;
using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static float[] vertices = new float[] {
            // vertex 1 x, y, z
            -.5f, -.5f, 0f,
            // vertex 2 x, y, z
            .5f, -.5f, 0f,
            // vertex 3 x, y, z
            0f, .5f, 0f
        };

        private const int vertexX = 0;
        private const int vertexY = 1;
        private const int vertexSize = 3;
        
        private static double radians;
        
        static void Main(string[] args) {
            var window = CreateWindow();

            LoadTriangleIntoBuffer();

            CreateShaderProgram();

            // engine rendering loop
            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render();
                
                // Move Right
                for (int i = vertexX; i < vertices.Length; i+= vertexSize) {
                    vertices[i] += 0.0001f;
                }
                
                // Move Down
                for (int i = vertexY; i < vertices.Length; i+= vertexSize) {
                    vertices[i] -= 0.0001f;
                }
                
                // vertices[0] = Convert.ToSingle(Math.Sin(radians)) * -0.5f;
                // vertices[1] = Convert.ToSingle(Math.Cos(radians)) * -0.5f;
                //
                // vertices[3] = Convert.ToSingle(Math.Cos(radians)) * 0.5f;
                // vertices[4] = Convert.ToSingle(Math.Sin(radians)) * -0.5f;
                //
                // vertices[6] = Convert.ToSingle(Math.Sin(radians)) * 0.5f;
                // vertices[7] = Convert.ToSingle(Math.Cos(radians)) * 0.5f;


                // vertices[0] = Convert.ToSingle(vertices[0] * Math.Cos(radians) - vertices[1] * Math.Sin(radians));
                // vertices[1] = Convert.ToSingle(vertices[0] * Math.Sin(radians) + vertices[1] * Math.Cos(radians));
                //
                // vertices[3] = Convert.ToSingle(vertices[3] * Math.Cos(radians) - vertices[4] * Math.Sin(radians));
                // vertices[4] = Convert.ToSingle(vertices[3] * Math.Sin(radians) + vertices[4] * Math.Cos(radians));
                //
                // vertices[6] = Convert.ToSingle(vertices[6] * Math.Cos(radians) - vertices[7] * Math.Sin(radians));
                // vertices[7] = Convert.ToSingle(vertices[6] * Math.Sin(radians) + vertices[7] * Math.Cos(radians));
                //
                // Console.WriteLine(vertices[7]);
                //
                // radians += 0.000001;

                UpdateTriangleBuffer();
            }
        }
        private static void Render() {
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length/vertexSize);
            glFlush();
        }
        private static void ClearScreen() {
            glClearColor(.0f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        static void CreateShaderProgram() {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/red-triangle.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/green-triangle.frag"));
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
            UpdateTriangleBuffer();
            glVertexAttribPointer(0, vertexSize, GL_FLOAT, false, vertexSize * sizeof(float), NULL);

            glEnableVertexAttribArray(0);
        }
        
        static unsafe void UpdateTriangleBuffer() {
            fixed (float* vertex = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
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
            Glfw.WindowHint(Hint.Doublebuffer, Constants.False);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}