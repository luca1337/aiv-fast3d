﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Aiv.Fast3D
{
	public class PerspectiveCamera : Camera
	{
		private Vector3 position3;
		public Vector3 Position3
		{
			get
			{
				return this.position3;
			}
			set
			{
				this.position3 = value;
			}
		}

		public Vector3 Forward
		{
			get
			{
				return (quaternion * new Vector4(Vector3.UnitZ)).Xyz;
			}
		}

		public Vector3 Right
		{
			get
			{
				return Vector3.Cross(Forward, Up);
			}
		}

		public Vector3 Up
		{
			get
			{
				return (quaternion * new Vector4(Vector3.UnitY)).Xyz;
			}
		}

		private Quaternion quaternion;
		public Vector3 EulerRotation3
		{
			get
			{
				return Rotation3 * (float)(180f / Math.PI);
			}
			set
			{
				Rotation3 = value * (float)(Math.PI / 180f);
			}
		}

		public Vector3 Rotation3
		{
			get
			{
				return Utils.QuaternionToRotation(quaternion);
			}
			set
			{
				quaternion = Quaternion.FromEulerAngles(value);
			}
		}

		public Quaternion Quaternion
		{
			get
			{
				return quaternion;
			}

			set
			{
				quaternion = value;
			}
		}

		private float fov;
		private float zNear;
		private float zFar;
		private float aspectRatio;

		public override bool HasProjection
		{
			get
			{
				return true;
			}
		}

		public PerspectiveCamera(Vector3 position, Vector3 eulerRotation, float fov, float zNear, float zFar, float aspectRatio = 0)
		{
			this.position3 = position;
			this.EulerRotation3 = eulerRotation;
			this.fov = fov * (float)(Math.PI / 180f);
			this.zNear = zNear;
			this.zFar = zFar;
			this.aspectRatio = aspectRatio;
			if (this.aspectRatio == 0)
			{
				this.aspectRatio = Window.Current.aspectRatio;
			}
		}

		public override Matrix4 Matrix()
		{
			return Matrix4.LookAt(position3, position3 + this.Forward, this.Up);

		}

		public override Matrix4 ProjectionMatrix()
		{
			float fovY = fov / aspectRatio;
			return Matrix4.CreatePerspectiveFieldOfView(fovY, aspectRatio, zNear, zFar);
		}
	}
}
