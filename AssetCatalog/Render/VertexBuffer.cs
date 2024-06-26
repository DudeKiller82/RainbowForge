﻿using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace AssetCatalog.Render
{
	public class VertexBuffer : IDisposable
	{
		private readonly BufferUsageHint _hint;

		public int ElementBufferId = -1;
		public int NormalBufferId = -1;

		public int NumElements;
		public int TexCoordBufferId = -1;
		public int VaoId = -1;
		public int VertexBufferId = -1;

		public bool Initialized { get; private set; }

		public VertexBuffer(BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			_hint = hint;
		}

		public void InitializeVbo(Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, uint[] elements)
		{
			try
			{
				if (VaoId == -1)
					GL.GenVertexArrays(1, out VaoId);
				GL.BindVertexArray(VaoId);

				// Normal Array Buffer
				{
					// Generate Array Buffer Id
					if (NormalBufferId == -1)
						GL.GenBuffers(1, out NormalBufferId);

					// Bind current context to Array Buffer ID
					GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferId);

					// Send data to buffer
					GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (normals.Length * Vector3.SizeInBytes),
						normals,
						_hint);

					// Validate that the buffer is the correct size
					GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize,
						out int bufferSize);
					if (normals.Length * Vector3.SizeInBytes != bufferSize)
						throw new ApplicationException("Normal array not uploaded correctly");

					// Clear the buffer Binding
					GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				}

				// Uv Array Buffer
				{
					// Generate Array Buffer Id
					if (TexCoordBufferId == -1)
						GL.GenBuffers(1, out TexCoordBufferId);

					// Bind current context to Array Buffer ID
					GL.BindBuffer(BufferTarget.ArrayBuffer, TexCoordBufferId);

					// Send data to buffer
					GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (texCoords.Length * Vector2.SizeInBytes),
						texCoords,
						_hint);

					// Validate that the buffer is the correct size
					GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize,
						out int bufferSize);
					if (texCoords.Length * Vector2.SizeInBytes != bufferSize)
						throw new ApplicationException("Uv array not uploaded correctly");

					// Clear the buffer Binding
					GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				}

				// Vertex Array Buffer
				{
					// Generate Array Buffer Id
					if (VertexBufferId == -1)
						GL.GenBuffers(1, out VertexBufferId);

					// Bind current context to Array Buffer ID
					GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);

					// Send data to buffer
					GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (vertices.Length * Vector3.SizeInBytes),
						vertices,
						_hint);

					// Validate that the buffer is the correct size
					GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize,
						out int bufferSize);
					if (vertices.Length * Vector3.SizeInBytes != bufferSize)
						throw new ApplicationException("Vertex array not uploaded correctly");

					// Clear the buffer Binding
					GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				}

				// Element Array Buffer
				{
					// Generate Array Buffer Id
					if (ElementBufferId == -1)
						GL.GenBuffers(1, out ElementBufferId);

					// Bind current context to Array Buffer ID
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferId);

					// Send data to buffer
					GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) (elements.Length * sizeof(uint)),
						elements,
						_hint);

					// Validate that the buffer is the correct size
					GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize,
						out int bufferSize);
					if (elements.Length * sizeof(uint) != bufferSize)
						throw new ApplicationException("Element array not uploaded correctly");

					// Clear the buffer Binding
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
				}

				GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);
				GL.EnableVertexAttribArray(0);
				GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

				GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferId);
				GL.EnableVertexAttribArray(1);
				GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

				GL.BindBuffer(BufferTarget.ArrayBuffer, TexCoordBufferId);
				GL.EnableVertexAttribArray(2);
				GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);

				GL.BindVertexArray(0);

				Initialized = true;
			}
			catch (ApplicationException ex)
			{
				Debug.WriteLine($"VertexBuffer/{VaoId}", $"{ex.Message}");
			}
			finally
			{
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			}

			// Store the number of elements for the DrawElements call
			NumElements = elements.Length;
		}


		public void Render(PrimitiveType type = PrimitiveType.Quads)
		{
			if (VaoId == -1)
				return;

			GL.BindVertexArray(VaoId);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferId);
			GL.DrawElements(type, NumElements, DrawElementsType.UnsignedInt, IntPtr.Zero);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		private void ReleaseUnmanagedResources()
		{
			GL.DeleteBuffer(ElementBufferId);
			GL.DeleteBuffer(NormalBufferId);
			GL.DeleteBuffer(VertexBufferId);
			GL.DeleteBuffer(TexCoordBufferId);
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}
	}
}