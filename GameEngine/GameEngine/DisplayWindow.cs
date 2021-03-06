﻿using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace GameEngine
{
    class DisplayWindow : GameWindow
    {
        int pgmID;
        int vsID;
        int fsID;

        int attribute_vcol;
        int attribute_vpos;
        int uniform_mview;

        int vbo_position;
        int vbo_color;
        int ibo_elements;
        int vbo_mview;

        Matrix4[] mViewData;

        int[] indiceData;
        PointData pointData;

        float xMouseRotation;
        float yMouseRotation;

        int startX;
        int startY;

        int prevX;
        int prevY;

        bool drag;

        public DisplayWindow(int width, int height,string filePath)
            : base(width,height, new GraphicsMode(32,24,0,4))
        {
            pointData = new PointData(filePath);
            xMouseRotation = 0.0f;
            yMouseRotation = 0.0f;
            prevX = 0;
            prevY = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            initProgram();

            indiceData = new int[pointData.Size];
            for (int i = 0; i < indiceData.Length;i++ )
            {
                indiceData[i] = i;
            }

            mViewData = new Matrix4[]
            {
                Matrix4.Identity
            };

            Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(20f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
 
 
            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);


            GL.DrawElements(PrimitiveType.Points, pointData.Size, DrawElementsType.UnsignedInt, 0);
 
 
            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_vcol);
 
 
            GL.Flush();
            SwapBuffers();
           
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(pointData.VertData.Length * Vector3.SizeInBytes), pointData.VertData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(pointData.ColorData.Length * Vector3.SizeInBytes), pointData.ColorData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, true, 0, 0);

            #region Transformation
            
            if(drag)
            {
                xMouseRotation += Mouse.X - prevX;
                yMouseRotation += Mouse.Y - prevY;

                prevX = Mouse.X;
                prevY = Mouse.Y;
            }

            mViewData[0] = Matrix4.CreateRotationY(0.005f * xMouseRotation)
                * Matrix4.CreateRotationX(0.005f * yMouseRotation)
                * Matrix4.CreateTranslation(0.0f, -5.0f, -20.0f)
                * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);

            #endregion Transformation

            GL.UniformMatrix4(uniform_mview, false, ref mViewData[0]);

            GL.UseProgram(pgmID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indiceData.Length * sizeof(int)), indiceData, BufferUsageHint.StaticDraw);
        }

        void initProgram()
        {
            pgmID = GL.CreateProgram();
            loadShader("vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            loadShader("fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            attribute_vpos = GL.GetAttribLocation(pgmID, "vPosition");
            attribute_vcol = GL.GetAttribLocation(pgmID, "vColor");
            uniform_mview = GL.GetUniformLocation(pgmID, "modelview");

            if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1)
            {
                Console.WriteLine("Error binding attributes");
            }

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);
            GL.GenBuffers(1, out ibo_elements);
        }

        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            drag = true;
            prevX = e.X;
            prevY = e.Y;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            drag = false;
        }
    }
}
