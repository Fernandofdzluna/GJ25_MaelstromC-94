using Cinemachine;
using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.UI;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		public float stablishedSpeed;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp;

		// cinemachine
		[Space(20)]
		public CinemachineVirtualCamera camaraCinemachine;
		CinemachineBasicMultiChannelPerlin perlinNoise;
		public Vector2 amplitudFrecuencyStopped;
		public Vector2 amplitudFrecuencyWalking;
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
        private float _verticalVelocity;
		//private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

        [Space(20)]
        public GameObject mascaraRespiracion;
		public GameObject BarraRoja;
        RectTransform barraRojaRespiración;
		float maxBarraRojaWidth;
		public bool hasMascaraRespiracion;
		public float tiempoBucalRespirador = 30;
		float unidadBarraRoja;

        [Space(20)]
        GameObject pickedObject;
		Transform hands;
		bool objectPicked;
		public bool onEscalera;

		bool zonaBombonasVisitada = false;

		Collider colliderPlayer;
		GameObject Escalera;
		public bool ahogandose = false;
		public bool bombonasCompletas = false;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;
		public Aguantar aguantar_script;
		private Animator animatorPlayer;
		private GameManager gameManager;

		private const float _threshold = 0.01f;

		public AudioClip audioWalkClip, audioStairsClip, audioBotellaClip, audioBreathWaterClip, audioDeathClip,
			audioWaterWalkClip, audioBreathClip, audioItemEquipClip, audioDoorClip, audioAhogarseClip;
		public bool audioClipPlayed, audioClipPlayed2, audioClipPlayed3 = false;

		private bool IsCurrentDeviceMouse
		{
			get
			{
#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}

			hands = _mainCamera.gameObject.transform.GetChild(0).gameObject.transform;
            colliderPlayer = this.gameObject.GetComponent<Collider>();
			onEscalera = false;

			hasMascaraRespiracion = false;
			mascaraRespiracion.SetActive(false);

			if (BarraRoja == null)
			{
                Debug.LogError("Barra roja no encontrada");
            }
			barraRojaRespiración = BarraRoja.GetComponent<RectTransform>();
			maxBarraRojaWidth = barraRojaRespiración.sizeDelta.x;
			unidadBarraRoja = maxBarraRojaWidth / tiempoBucalRespirador;

			animatorPlayer = this.gameObject.GetComponent<Animator>();

			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			zonaBombonasVisitada = false;

			stablishedSpeed = MoveSpeed;
			bombonasCompletas = false;

        }

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			perlinNoise = camaraCinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}

		private void Update()
        {
            Move();

            if (!hasMascaraRespiracion && ahogandose)
            {
                perlinNoise.m_AmplitudeGain += 0.3f * Time.deltaTime;
                perlinNoise.m_FrequencyGain += 0.3f * Time.deltaTime;
            }
            else
			{
                if (!onEscalera)
                {
                    if (_input.move != Vector2.zero)
                    {
                        //Caminar
                        perlinNoise.m_AmplitudeGain = amplitudFrecuencyWalking.x;
                        perlinNoise.m_FrequencyGain = amplitudFrecuencyWalking.y;

						//SoundFXManager.instance.PlaySoundFXCLip(audioWalkClip, transform, 1f);
						if(audioClipPlayed2 == false)
                        {
							audioClipPlayed2 = true;
							StartCoroutine(Second());
                        }
					}
                    else
                    {
                        //Quieto
                        perlinNoise.m_AmplitudeGain = amplitudFrecuencyStopped.x;
                        perlinNoise.m_FrequencyGain = amplitudFrecuencyStopped.y;

						//SoundFXManager.instance.PlaySoundFXCLip(audioBreathClip, transform, 1f);
						if (audioClipPlayed3 == false)
						{
							audioClipPlayed3 = true;
							StartCoroutine(Third());
						}
					}
                }
                else
                {
                    perlinNoise.m_AmplitudeGain = amplitudFrecuencyStopped.x;
                    perlinNoise.m_FrequencyGain = amplitudFrecuencyStopped.y;
				}
            }

            if (objectPicked)
			{
				pickedObject.transform.localPosition = Vector3.zero;
				pickedObject.transform.eulerAngles = Vector3.zero;
			}
		}

		private void LateUpdate()
		{
			CameraRotation();
        }

        private void CameraRotation()
		{
            if (!onEscalera)
            {
                // if there is an input
                if (_input.look.sqrMagnitude >= _threshold)
                {
                    //Don't multiply mouse input by Time.deltaTime
                    float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                    _cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
                    _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

                    // clamp our pitch rotation
                    _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                    // Update Cinemachine camera target pitch
                    CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                    // rotate the player left and right
                    transform.Rotate(Vector3.up * _rotationVelocity);
                }
            }
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (!onEscalera)
			{
				if (_input.move != Vector2.zero)
				{
					// move
					inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;

					//SoundFXManager.instance.PlaySoundFXCLip(audioWalkClip, transform, 1f);
				}

				// move the player
				_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

				//SoundFXManager.instance.PlaySoundFXCLip(audioWalkClip, transform, 1f);
			}
			else
			{
				if (_input.move != Vector2.zero)
				{
					inputDirection = transform.up * _input.move.y;
				}

                // move the player
                _controller.Move(inputDirection.normalized * (3 * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		public void CheckRayCast(string tecla)
		{
			RaycastHit hit;

			Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.TransformDirection(Vector3.forward) * 4, Color.red);
			int mask = 1 << LayerMask.NameToLayer("SuelosEscaleras");

            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.TransformDirection(Vector3.forward), out hit, 4, ~mask))
			{
				switch (hit.collider.tag)
				{
					case "Interactuable":
						if (tecla == "clickIzq")
						{
							pickedObject = hit.collider.gameObject;
							pickedObject.transform.parent = hands.transform;
							objectPicked = true;
						}
                        break;
					case "Escalera":
						if (tecla == "E")
						{
							Escalera = hit.collider.gameObject;
							Transform ButtonEscalera = Escalera.transform.GetChild(0).transform;
							Transform MidEscalera = Escalera.transform.GetChild(1).transform;
							Transform TopEscalera = Escalera.transform.GetChild(2).transform;
							float distanceButton = Vector3.Distance(this.transform.position, ButtonEscalera.transform.position);
							float distanceMid = Vector3.Distance(this.transform.position, MidEscalera.transform.position);
							float distanceTop = Vector3.Distance(this.transform.position, TopEscalera.transform.position);
							if (distanceTop < distanceMid && distanceTop < distanceButton)
							{
                                //Top escalera cerca de player
                                CharacterController controller = this.GetComponent<CharacterController>();
								SoundFXManager.instance.PlaySoundFXCLip(audioStairsClip, transform, 1f);
								controller.enabled = false;
                                this.gameObject.transform.position = TopEscalera.position;
                                controller.enabled = true;
                                this.transform.eulerAngles = new Vector3(0, 90, 0);
                                onEscalera = true;
							}
							else if (distanceButton < distanceMid && distanceButton < distanceTop)
							{
								//Button escalera cerca de player
								CharacterController controller = this.GetComponent<CharacterController>();
								SoundFXManager.instance.PlaySoundFXCLip(audioStairsClip, transform, 1f);
								controller.enabled = false;
								this.gameObject.transform.position = ButtonEscalera.position;
                                controller.enabled = true;
                                this.transform.eulerAngles = new Vector3(0, 90, 0);
                                onEscalera = true;
							}
							else
							{
                                //Mid escalera cerca de player
                                CharacterController controller = this.GetComponent<CharacterController>();
								SoundFXManager.instance.PlaySoundFXCLip(audioStairsClip, transform, 1f);
								controller.enabled = false;
                                this.gameObject.transform.position = MidEscalera.position;
                                controller.enabled = true;
                                this.transform.eulerAngles = new Vector3(0,90,0);
                                onEscalera = true;
							}

                            colliderPlayer.excludeLayers = LayerMask.GetMask("SuelosEscaleras");
						}
						break;
					case "Respirador":
						if (!hasMascaraRespiracion)
						{
							mascaraRespiracion.SetActive(true);
							hasMascaraRespiracion = true;
							tiempoBucalRespirador = 30;
							SoundFXManager.instance.PlaySoundFXCLip(audioBreathWaterClip, transform, 1f);
							Breath(true);
                            Destroy(hit.collider.gameObject);
						}
						else
						{
							if(tiempoBucalRespirador < 30)
							{
								tiempoBucalRespirador = 30;
								Breath(true);
								SoundFXManager.instance.PlaySoundFXCLip(audioAhogarseClip, transform, 1f);
								Destroy(hit.collider.gameObject);
								Breath(true);
							}
						}
						break;
					case "Escotilla":
						if (tecla == "E")
						{
							if (bombonasCompletas == true)
							{
								hit.collider.gameObject.GetComponent<Animator>().SetTrigger("Rotar");

								SoundFXManager.instance.PlaySoundFXCLip(audioDoorClip, transform, 0.8f);

								gameManager.EscapeSubmarine();
                            }
                        }
						break;
				}
			}
		}

		public void LeaveObject()
		{
            if (objectPicked)
            {
                RaycastHit hit;
                int mask = 1 << LayerMask.NameToLayer("SuelosEscaleras");
                if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.TransformDirection(Vector3.forward), out hit, 4, ~mask) && hit.collider.tag == "MaquinaBombonas")
                {
					Animator animatorBox = hit.collider.GetComponent<Animator>();
					if (animatorBox.GetBool("FirstBombona") == false)
					{
						animatorBox.SetBool("FirstBombona", true);
						objectPicked = false;
						Destroy(pickedObject);
						gameManager.UpgradeBombonasNumber(1);

						SoundFXManager.instance.PlaySoundFXCLip(audioBotellaClip, transform, 0.5f);
					}
					else if(animatorBox.GetBool("SecBombona") == false)
					{
                        animatorBox.SetBool("SecBombona", true);
                        objectPicked = false;
                        Destroy(pickedObject);
                        gameManager.UpgradeBombonasNumber(1);

						SoundFXManager.instance.PlaySoundFXCLip(audioBotellaClip, transform, 0.5f);
					}
                }
				else
                {
                    objectPicked = false;
                    hands.transform.DetachChildren();
                    pickedObject.transform.position = this.gameObject.transform.position;

					SoundFXManager.instance.PlaySoundFXCLip(audioItemEquipClip, transform, 1f);
				}
            }
        }

		public void LeaveEscalera()
		{
            Transform ButtonEscalera = Escalera.transform.GetChild(0).transform;
            Transform MidEscalera = Escalera.transform.GetChild(1).transform;
            Transform TopEscalera = Escalera.transform.GetChild(2).transform;
            float distanceButton = Vector3.Distance(this.transform.position, ButtonEscalera.transform.position);
            float distanceMid = Vector3.Distance(this.transform.position, MidEscalera.transform.position);
            float distanceTop = Vector3.Distance(this.transform.position, TopEscalera.transform.position);
            if (distanceTop < distanceMid && distanceTop < distanceButton)
            {
                //Top escalera cerca de player
                CharacterController controller = this.GetComponent<CharacterController>();
                controller.enabled = false;
				this.gameObject.transform.position = TopEscalera.position;
                controller.enabled = true;
                onEscalera = false;
			}
            else if (distanceButton < distanceMid && distanceButton < distanceTop)
            {
                //Button escalera cerca de player
                CharacterController controller = this.GetComponent<CharacterController>();
                controller.enabled = false;
				this.gameObject.transform.position = ButtonEscalera.position;
                controller.enabled = true;
                onEscalera = false;
			}
            else
            {
                //Mid escalera cerca de player
                CharacterController controller = this.GetComponent<CharacterController>();
                controller.enabled = false;
				this.gameObject.transform.position = MidEscalera.position;
                controller.enabled = true;
                onEscalera = false;
			}

            colliderPlayer.excludeLayers = LayerMask.GetMask("Nothing");
        }

		public void Breath(bool add)
		{
			if (add)
			{
                barraRojaRespiración.sizeDelta = new Vector2(maxBarraRojaWidth, barraRojaRespiración.sizeDelta.y);

				SoundFXManager.instance.PlaySoundFXCLip(audioBreathWaterClip, transform, 1f);
				if (audioClipPlayed == false)
				{
					audioClipPlayed = true;
					StartCoroutine(Once());
				}
			}
			else
			{
                barraRojaRespiración.sizeDelta -= new Vector2(unidadBarraRoja, 0);
				if (tiempoBucalRespirador <= 0)
                {
                    mascaraRespiracion.SetActive(false);
                    hasMascaraRespiracion = false;
					aguantar_script.ApplyWithout(true);
					SoundFXManager.instance.PlaySoundFXCLip(audioAhogarseClip, transform, 1f);
				}
            }
		}

		public void DeathPlayer()
		{
			StarterAssetsInputs starterAssetsInputs = this.gameObject.GetComponent<StarterAssetsInputs>();
			starterAssetsInputs.enabled = false;
			animatorPlayer.enabled = true;
			animatorPlayer.SetTrigger("Death");
			SoundFXManager.instance.PlaySoundFXCLip(audioDeathClip, transform, 1f);
			gameManager.EndGame(false);
			this.enabled = false;
		}

        private void OnTriggerEnter(Collider other)
        {
			if (other.gameObject.transform.name == "SalaBombonas" && zonaBombonasVisitada == false)
			{
				zonaBombonasVisitada = true;
				gameManager.ChangeSubmarineScreens();
            }
        }

		public IEnumerator Once()
        {
			SoundFXManager.instance.PlaySoundFXCLip(audioWaterWalkClip, transform, 1f);
			//audioClipPlayed = false;
			yield return new WaitForSeconds(1.5f);
			audioClipPlayed = false;
		}
		public IEnumerator Second()
		{
			SoundFXManager.instance.PlaySoundFXCLip(audioWalkClip, transform, 1f);
			yield return new WaitForSeconds(1);
			audioClipPlayed2 = false;
		}
		public IEnumerator Third()
		{
			SoundFXManager.instance.PlaySoundFXCLip(audioBreathClip, transform, 1f);
			yield return new WaitForSeconds(2);
			audioClipPlayed3 = false;
		}
	}
}