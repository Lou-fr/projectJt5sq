using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerMovementStates : IState
    {
        protected PlayerStateMachine stateMachine;
        protected PlayerGroundedData movementData;
        protected PlayerAirborneData airborneData;
        public PlayerMovementStates(PlayerStateMachine playerMovementStateMachine)
        {
            stateMachine = playerMovementStateMachine;
            movementData = stateMachine._Player.data.groundedData;
            airborneData = stateMachine._Player.data.airboneData;
            InitializeData();
        }
        #region IState
        private void InitializeData()
        {
            SetBaseRotationData();
        }

        public virtual void Enter()
        {
            Debug.Log("State " + GetType().Name);
            AddInputActionCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionCallbacks();
        }
        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void PhysicUpdate()
        {
            Move();
        }

        public virtual void Update()
        {

        }
        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }
        public virtual void OnTriggerEnter(Collider collider)
        {
            if (stateMachine._Player.layerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);
                return;
            }
        }
        public void OnTriggerExit(Collider collider)
        {
            if (stateMachine._Player.layerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExited(collider);
                return;
            }
        }
        #endregion
        #region Main
        private void ReadMovementInput()
        {
            stateMachine.reasubleData.MovementInput = stateMachine._Player.Input.playerActions.movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            if (stateMachine.reasubleData.MovementInput == Vector2.zero || stateMachine.reasubleData.MovementSpeedModifier == 0f) return;
            Vector3 movementDirection = GetMovementInputDirection();
            float targetRotationYAngle = Rotate(movementDirection);
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            float movementSpeed = GetMovementSpeed();
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
            stateMachine._Player.rigidBody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }

        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);
            RotateTowardsTargetRotation();
            return directionAngle;
        }
        private float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            if (directionAngle < 0f) directionAngle += 360f;
            return directionAngle;
        }
        private float AddCameraRotationToAngle(float angle)
        {
            angle += stateMachine._Player.mainCameraTransform.eulerAngles.y;
            if (angle > 360f) angle -= 360f;
            return angle;
        }

        private void UpdateTargetRotationData(float directionAngle)
        {
            stateMachine.reasubleData.CurrentTargetRotation.y = directionAngle;
            stateMachine.reasubleData.DampedTargetRotationPassedTime.y = 0f;
        }
        #endregion
        #region Reusable Methods
        protected void StartAnimation(int animationHash)
        {
            stateMachine._Player.animator.SetBool(animationHash,true);
        }
        protected void StopAnimation(int animationHash)
        {
            stateMachine._Player.animator.SetBool(animationHash, false);
        }
        protected void changeAnimationCount(int animationHash, int number)
        {
            stateMachine._Player.animator.SetInteger(animationHash, number);
        }
        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = stateMachine._Player.rigidBody.linearVelocity;

            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
        }
        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, stateMachine._Player.rigidBody.linearVelocity.y, 0f);
        }

        protected float GetMovementSpeed()
        {
            return movementData.BaseSpeed * stateMachine.reasubleData.MovementSpeedModifier * stateMachine.reasubleData.MovementOnSlopesSpeedModifier;
        }

        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(stateMachine.reasubleData.MovementInput.x, 0f, stateMachine.reasubleData.MovementInput.y);
        }
        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = stateMachine._Player.rigidBody.rotation.eulerAngles.y;
            if (currentYAngle == stateMachine.reasubleData.CurrentTargetRotation.y) return;
            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.reasubleData.CurrentTargetRotation.y, ref stateMachine.reasubleData.DampedTargetRotationCurrentVelocity.y, stateMachine.reasubleData.TimeToReachTargetRotation.y - stateMachine.reasubleData.DampedTargetRotationPassedTime.y);
            stateMachine.reasubleData.DampedTargetRotationPassedTime.y += Time.deltaTime;
            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
            stateMachine._Player.rigidBody.MoveRotation(targetRotation);
        }
        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);
            if (shouldConsiderCameraRotation) directionAngle = AddCameraRotationToAngle(directionAngle);

            if (directionAngle != stateMachine.reasubleData.CurrentTargetRotation.y) UpdateTargetRotationData(directionAngle);
            return directionAngle;
        }
        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        protected void ResetVelocity()
        {
            stateMachine._Player.rigidBody.linearVelocity = Vector3.zero;
        }
        protected void ResetVerticalVelocity()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            stateMachine._Player.rigidBody.linearVelocity =playerHorizontalVelocity;
        }
        protected virtual void AddInputActionCallbacks()
        {
            stateMachine._Player.Input.playerActions.WalkToggle.started += OnWalkToggleStarted;
        }
        protected virtual void RemoveInputActionCallbacks()
        {
            stateMachine._Player.Input.playerActions.WalkToggle.started -= OnWalkToggleStarted;
        }
        protected void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            stateMachine._Player.rigidBody.AddForce(-playerHorizontalVelocity * stateMachine.reasubleData.MovementDecelerationforce, ForceMode.Acceleration);
        }
        protected void DecelerateVertically()
        {
            Vector3 playerVertivalVelocity = GetPlayerVerticalVelocity();

            stateMachine._Player.rigidBody.AddForce(-playerVertivalVelocity * stateMachine.reasubleData.MovementDecelerationforce, ForceMode.Acceleration);
        }
        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);
            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }
        protected bool IsMovingUp(float minimuVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y > minimuVelocity;
        }
        protected bool IsMovingDown(float minimuVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y < -minimuVelocity;
        }
        protected void SetBaseRotationData()
        {
            stateMachine.reasubleData.RotationData = movementData.BaseRotationData;
            stateMachine.reasubleData.TimeToReachTargetRotation = stateMachine.reasubleData.RotationData.TargetRotationReachTime;
        }
        protected virtual void OnContactWithGround(Collider collider)
        {

        }
        protected virtual void OnContactWithGroundExited(Collider collider)
        {

        }
        #endregion
        #region Input Methods

        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            stateMachine.reasubleData.ShouldWalk = !stateMachine.reasubleData.ShouldWalk;
        }
        #endregion
    }
}
