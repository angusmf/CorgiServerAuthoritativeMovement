using MoreMountains.CorgiEngine;
using PRN;
using System.Collections.Generic;
using UnityEngine;

namespace PRN2Corgi {

    public interface INetworkDeltaTime {
        void SetNetworkDeltaTime(float networkDeltaTime);
    }

    public class NetworkCharacter : Character, IProcessState, INetworkDeltaTime {

        private float networkDeltaTime;

        protected INetworkDeltaTime[] _networkAbilities;

        private bool isOwner;
        //private NetworkRole role;

        //private bool config0 => role != NetworkRole.OWNER && role != NetworkRole.HOST && role != NetworkRole.SERVER;
        //private bool config2 => role != NetworkRole.OWNER && role != NetworkRole.SERVER;
        //private bool config3 => role != NetworkRole.OWNER;
        //private bool config4 => role != NetworkRole.HOST && role != NetworkRole.SERVER;
        //private bool config5 => role != NetworkRole.HOST;
        //private bool config6 => role != NetworkRole.SERVER;
        //private bool config7 => true;


        //private bool config1 => role != NetworkRole.OWNER && role != NetworkRole.HOST;
        //private bool Role => config1;

        void IProcessState.ProcessState() => ProcessTick();


        public virtual void ProcessTick() {
            if (!isOwner) { 
            //if (Role) {
                EveryFrame();
            }

            if (Time.timeScale != 0f) {
                ProcessAbilities();
                LateProcessAbilities();
                HandleCameraTarget();
            }

            // we send our various states to the animator.		 
            UpdateAnimators();
            RotateModel();
        }

        protected override void EveryFrame() {
            HandleCharacterStatus();

            // we process our abilities
            EarlyProcessAbilities();
        }


        protected override void Update() {
            // do not call base.Update() because it calls EveryFrame()
            // For networked characters, we need to let PRN control exection of that method in the Processor implementation
            if (isOwner) { 
                    EveryFrame();
            }
        }

        protected override void ProcessAbilities() {

            foreach (INetworkDeltaTime ability in _networkAbilities) {
                ability.SetNetworkDeltaTime(networkDeltaTime);
            }

            foreach (CharacterAbility ability in _characterAbilities) {
                if (ability.enabled && ability.AbilityInitialized) {
                    ability.ProcessAbility();
                }
            }
        }

        public override void CacheAbilities() {
            base.CacheAbilities();

            List<INetworkDeltaTime> networkAbilities = new List<INetworkDeltaTime>();
            foreach (CharacterAbility characterAbility in _characterAbilities) {
                var networkAbility = characterAbility as INetworkDeltaTime;
                if (networkAbility != null) {
                    networkAbilities.Add(networkAbility);
                }
            }
            _networkAbilities = networkAbilities.ToArray();
        }

        protected override void HandleCameraTarget() {
            CameraTarget.transform.localPosition = Vector3.Lerp(CameraTarget.transform.localPosition, _cameraTargetInitialPosition + _cameraOffset, networkDeltaTime * CameraTargetSpeed);
        }

        protected override void RotateModel() {
            if (!RotateModelOnDirectionChange) {
                return;
            }

            if (ModelRotationSpeed > 0f) {
                CharacterModel.transform.localEulerAngles = Vector3.Lerp(CharacterModel.transform.localEulerAngles, _targetModelRotation, networkDeltaTime * ModelRotationSpeed);
            }
            else {
                CharacterModel.transform.localEulerAngles = _targetModelRotation;
            }
        }

        public void SetInputManager(InputManager inputManager, bool isOwner) {
            base.SetInputManager(inputManager);
            this.isOwner = isOwner;            
        }

        internal void SetFacingRight(bool isFacingRight) {
            if (IsFacingRight != isFacingRight) {
                Flip();
            }
        }

        public void SetNetworkDeltaTime(float networkDeltaTime) {
            this.networkDeltaTime = networkDeltaTime;
        }
    }
}