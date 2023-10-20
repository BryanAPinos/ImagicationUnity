using UnityEngine;
using UnityEngine.UI;


using System.Collections;


namespace Imagication
{
	public class EventPlayerUI : MonoBehaviour
	{
		#region Private Fields

		[Tooltip("Pixel offset from the player target")]
		[SerializeField]
		private Vector3 screenOffset = new Vector3(0f, 0f, 0f);

		[Tooltip("UI Text to display Player's Name")]
		[SerializeField]
		public Text playerNameText;


		EventPlayerManager target;

		float characterControllerHeight;
		Transform targetTransform;
		Renderer targetRenderer;
		CanvasGroup _canvasGroup;
		Vector3 targetPosition;

		#endregion

		#region MonoBehaviour Callbacks

		// /// <summary>
		// /// MonoBehaviour method called on GameObject by Unity during early initialization phase
		// /// </summary>
		void Awake()
		{
			_canvasGroup = this.GetComponent<CanvasGroup>();

			this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
		}

		// /// <summary>
		// /// MonoBehaviour method called on GameObject by Unity on every frame.
		// /// update the health slider to reflect the Player's health
		// /// </summary>
		void Update()
		{
			// Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
			if (target == null)
			{
				Destroy(this.gameObject);
				return;
			}
		}

		// /// <summary>
		// /// MonoBehaviour method called after all Update functions have been called. This is useful to order script execution.
		// /// In our case since we are following a moving GameObject, we need to proceed after the player was moved during a particular frame.
		// /// </summary>
		void LateUpdate()
		{

			// Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
			if (targetRenderer != null)
			{
				this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
			}

			// #Critical
			// Follow the Target GameObject on screen.
			if (targetTransform != null)
			{
				targetPosition = targetTransform.position;
				targetPosition.y += characterControllerHeight;
				targetPosition.y += 3f;

				this.transform.position = Camera.main.WorldToScreenPoint(targetPosition);
			}
		}
		#endregion

		#region Public Methods
		// /// <summary>
		// /// Assigns a Player Target to Follow and represent.
		// /// </summary>
		// /// <param name="target">Target.</param>
		public void SetTarget(EventPlayerManager _target)
		{
			if (_target == null)
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
				return;
			}

			// Cache references for efficiency because we are going to reuse them.
			this.target = _target;

			targetTransform = this.target.GetComponent<Transform>();
			targetRenderer = this.target.GetComponentInChildren<Renderer>();
			// targetRenderer = this.target.GetComponent<Renderer>();

			CharacterController _characterController = this.target.GetComponent<CharacterController>();
			// Get data from the Player that won't change during the lifetime of this Component
			if (_characterController != null)
			{
				characterControllerHeight = _characterController.height;
			}

			if (playerNameText != null)
			{
				playerNameText.text = this.target.photonView.Owner.NickName;
			}
		}
		#endregion
	}
}