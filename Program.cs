﻿using System;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine {
    class Program {
        static void Main(string[] args) {
            // Initialize and configure
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

            float[] vertices = new float[] {
                -.5f, -.5f, 0f,
                .5f, -.5f, 0f,
                0f, .5f, 0f,
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

            string vertexShaderSource = @"
            #version 330 core
            in vec3 pos;

            void main() {
                gl_position = vec4(pos.x, pos.y, pos.z, 1.0, 0);
            ";
            
            // Create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, vertexShaderSource);
            glCompileShader(vertexShader);

            string fragmentShaderSource = @"
            #version 330 core
            out vec4 result;

            void main() {
                  result = vec4(1, 0, 0, 1);
            }
            ";
            
            // Create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, fragmentShaderSource);
            glCompileShader(fragmentShader);
            
            // Create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
            
            // Engine rendering loop
            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // React to window changes
                glDrawArrays(GL_TRIANGLES, 0, 3);
                glFlush();
            }
        }
    }
}
