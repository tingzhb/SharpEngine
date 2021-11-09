﻿using System;
using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine {
    class Program {
        static void Main(string[] args) {
            // Initialize and configure
            var window = CreateWindow();

            LoadTriangleIntoBuffer();

            CreateShaderProgram();

            // Engine rendering loop
            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // React to window changes
                glDrawArrays(GL_TRIANGLES, 0, 3);
                glFlush();
            }
        }
        
        private static unsafe void LoadTriangleIntoBuffer() {
            float[] vertices = new float[]
            {
                -.5f, -.5f, 0f,
                .5f, -.5f, 0f,
                0f, .5f, 0f
            };

            // Load vertices into a buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            unsafe {
                fixed (float* vertex = &vertices[0]) {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
                }
                glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), NULL);
            }
            glEnableVertexAttribArray(0);
        }
        
        private static void CreateShaderProgram() {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/red-triangle.vert"));
            glCompileShader(vertexShader);

            // Create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/red-triangle.frag"));
            glCompileShader(fragmentShader);

            // Create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }
        
        private static Window CreateWindow() {
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.False);

            // Create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}