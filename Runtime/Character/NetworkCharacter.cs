using MoreMountains.CorgiEngine;
using System.Collections.Generic;
using UnityEngine;

namespace PRN2Corgi {

    public interface INetworkDeltaTime {
        void SetNetworkDeltaTime(float networkDeltaTime);
    }

    public class NetworkCharacter : Character, IProcessState, INetworkDeltaTime {

        private float networkDeltaTime;

        protected INetworkDeltaTime[] _networkAbilities;

        void IProcessState.ProcessState() => EveryFrame();    

        protected override void Update() {
            // do not call base.Update() because it calls EveryFrame()
            // For networked characters, we need to let PRN control exection of that method in the Processor implementation
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