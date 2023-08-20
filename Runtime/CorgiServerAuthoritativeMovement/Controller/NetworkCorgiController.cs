using MoreMountains.CorgiEngine;

namespace NewAntfarm {
    public class NetworkCorgiController : CorgiController, IProcessState, INetworkDeltaTime {
        private float deltaTime;

        // Update is called once per frame
        protected override void Update() {
            // do not call base.Update() because it calls EveryFrame()
            // For networked characters, we need to let PRN control exection of that method in the Processor implementation
        }

        protected override void FixedUpdate() {
            // do not call base.FixedUpdate() because it calls EveryFrame()
            // For networked characters, we need to let PRN control exection of that method in the Processor implementation
        }

        public override float DeltaTime => deltaTime;

        void IProcessState.ProcessState() {
            EveryFrame();
        }

        public void SetNetworkDeltaTime(float deltaTime) => this.deltaTime = deltaTime;
    }
}
