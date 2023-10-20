// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraWork.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the Camera work to follow the player
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Imagication
{
	/// <summary>
	/// Camera work. Follow a target
	/// </summary>
	public class CameraWork : MonoBehaviour
	{
		#region Private Fields
		[Tooltip("The distance in the local x-z plane to the target")]
		[SerializeField]
		private float distance = 7.0f;

		[Tooltip("The height we want the camera to be above the target")]
		[SerializeField]
		private float height = 3.0f;

		[Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
		[SerializeField]
		private Vector3 centerOffset = Vector3.zero;

		[Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
		[SerializeField]
		private bool followOnStart = false;

		[Tooltip("The Smoothing for the camera to follow the target")]
		[SerializeField]
		private float smoothSpeed = 0.125f;

		// cached transform of the target
		Transform cameraTransform;
		Transform miniCameraTransform;

		// maintain a flag internally to reconnect if target is lost or camera is switched
		bool isFollowing;

		// Cache for camera offset
		Vector3 cameraOffset = Vector3.zero;
		Vector3 minicameraOffset = Vector3.zero;
		public float turnSpeed = 45f;

		public float Speed = 5;
		#endregion

		#region MonoBehaviour Callbacks
		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during initialization phase
		/// </summary>
		void Start()
		{
			// Start following the target if wanted.
			if (followOnStart)
			{
				OnStartFollowing();
			}
		}

		void LateUpdate()
		{
			// The transform target may not destroy on level load, 
			// so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
			if (cameraTransform == null && isFollowing)
			{
				OnStartFollowing();
			}

			if (isFollowing)
			{
				// Get the current mouse input
				PointerEventData eventData = new PointerEventData(EventSystem.current);
				eventData.position = Input.mousePosition;

				// Get all results hit by the raycast--
				// A raycast is a line you draw from your camera to a point in the world
				// Its use here is to check if the mouse is over any UI elements
				var results = new List<RaycastResult>();
				EventSystem.current.RaycastAll(eventData, results);

				// Mouse0: Left Click
				// If the mouse is over any UI elements, don't move the camera
				if (Input.GetKey(KeyCode.Mouse0) && results.Count == 0)
				{
					Leash();
				}
				else
					Follow();
			}

		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Raises the start following event. 
		/// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
		/// </summary>

		public void OnStartFollowing()
		{
			Camera _miniMapCameraWork = GameObject.Find("Minimap Cam").GetComponent<Camera>();
			cameraTransform = Camera.main.transform;
			miniCameraTransform = _miniMapCameraWork.transform;
			isFollowing = true;

			// we don't smooth anything, we go straight to the right camera shot
			Cut();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Follow the target smoothly
		/// </summary>
		void Follow()
		{
			cameraOffset.z = -distance;
			cameraOffset.y = height;
			minicameraOffset.z = 0;
			minicameraOffset.y = 60;

			// Set positions to be the same as the target, but offset by the calculated offset distance
			cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);
			miniCameraTransform.position = Vector3.Lerp(miniCameraTransform.position, this.transform.position + this.transform.TransformVector(minicameraOffset), 1);
			
			// Set the camera to look at the center of the target
			cameraTransform.LookAt(this.transform.position + centerOffset);
			miniCameraTransform.LookAt(this.transform.position);

		}

		void Leash()
		{
			cameraOffset.z = -distance;
			cameraOffset.y = height;
			minicameraOffset.z = 0;
			minicameraOffset.y = 60;

			// See above
			cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);
			miniCameraTransform.position = Vector3.Lerp(miniCameraTransform.position, this.transform.position + this.transform.TransformVector(minicameraOffset), 1);

			// Euler angles are the rotation of an object in 3D space
			cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x, transform.eulerAngles.y, cameraTransform.eulerAngles.z);

			// Get the axis (direction) of the mouse movement
			float mouseY = Input.GetAxis("Mouse Y");

			float xRotation = cameraTransform.eulerAngles.x;
			// If the mouse is moving up
			if (mouseY > 0)
			{
				// If the camera is looking up or down, rotate it
				if (xRotation - mouseY < 11.6 || xRotation - mouseY > 330)
					cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x - mouseY, transform.eulerAngles.y, cameraTransform.eulerAngles.z);
			}
			else if (mouseY < 0)
			{
				if (xRotation - mouseY < 11.6 || xRotation - mouseY > 330)
					cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x - mouseY, transform.eulerAngles.y, cameraTransform.eulerAngles.z);
			}

		}

		void Cut()
		{
			cameraOffset.z = -distance;
			cameraOffset.y = height;
			minicameraOffset.z = 0;
			minicameraOffset.y = 60;

			cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);
			miniCameraTransform.position = this.transform.position + this.transform.TransformVector(minicameraOffset);

			// Set the camera to look at the center of the target
			// Note that this method is a cut, meaning that it will not smoothly transition
			cameraTransform.LookAt(this.transform.position + centerOffset);
			miniCameraTransform.LookAt(this.transform.position);
		}
		#endregion
	}
}