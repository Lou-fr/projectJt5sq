using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerMovementStates : IState
    {
        protected PlayerStateMachine stateMachine;
        protected PlayerGroundedData movementData;
        protected PlayerAirborneData airborneData;
        public static Action<int> SwapCharTo = delegate { };
        public PlayerMovementStates(PlayerStateMachine playerMovementStateMachine)
        {
            stateMachine = playerMovementStateMachine;
            movementData = stateMachine.Character.groundedData;
            airborneData = stateMachine.Character.airboneData;
            InitializeData();
        }

        #region IState
        private void InitializeData()
        {
            SetBaseRotationData();
        }

        public virtual void Enter()
        {
#if UNITY_EDITOR
            Debug.Log($"STATE MACHINE Entering State {GetType().Name} , ");
#endif
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
            if (stateMachine.Player.layerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);
                return;
            }
        }
        public virtual void OnTriggerExit(Collider collider)
        {

            if (stateMachine.Player.layerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExited(collider);
                return;
            }
        }
        #endregion
        #region Main
        private void ReadMovementInput()
        {
            stateMachine.reasubleData.MovementInput = stateMachine.Player.Input.playerActions.movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            if (stateMachine.reasubleData.MovementInput == Vector2.zero || stateMachine.reasubleData.MovementSpeedModifier == 0f) return;
            Vector3 movementDirection = GetMovementInputDirection();
            float targetRotationYAngle = Rotate(movementDirection);
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            float movementSpeed = GetMovementSpeed();
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
            stateMachine.Player.rigidBody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
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
            angle += stateMachine.Player.mainCameraTransform.eulerAngles.y;
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
            stateMachine.assignedAnimator.SetBool(animationHash, true);
            //stateMachine.assignedNetworkAnimator.SetTrigger(animationHash,true);
        }
        protected void StopAnimation(int animationHash)
        {
            stateMachine.assignedAnimator.SetBool(animationHash, false);
            //stateMachine.assignedNetworkAnimator.SetTrigger(animationHash, false);
        }
        protected void changeAnimationCount(int animationHash, int number)
        {
            stateMachine.assignedAnimator.SetInteger(animationHash, number);
        }
        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = stateMachine.Player.rigidBody.linearVelocity;

            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
        }
        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, stateMachine.Player.rigidBody.linearVelocity.y, 0f);
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
            float currentYAngle = stateMachine.Player.rigidBody.rotation.eulerAngles.y;
            if (currentYAngle == stateMachine.reasubleData.CurrentTargetRotation.y) return;
            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.reasubleData.CurrentTargetRotation.y, ref stateMachine.reasubleData.DampedTargetRotationCurrentVelocity.y, stateMachine.reasubleData.TimeToReachTargetRotation.y - stateMachine.reasubleData.DampedTargetRotationPassedTime.y);
            stateMachine.reasubleData.DampedTargetRotationPassedTime.y += Time.deltaTime;
            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
            stateMachine.Player.rigidBody.MoveRotation(targetRotation);
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
            stateMachine.Player.rigidBody.linearVelocity = Vector3.zero;
        }
        protected void ResetVerticalVelocity()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            stateMachine.Player.rigidBody.linearVelocity = playerHorizontalVelocity;
        }
        protected virtual void AddInputActionCallbacks()
        {
            stateMachine.Player.Input.playerActions.WalkToggle.started += OnWalkToggleStarted;
            stateMachine.Player.Input.playerActions.SwitchCharater1.started += SwitchCharater1;
            stateMachine.Player.Input.playerActions.SwitchCharater2.started += SwitchCharater2;
            stateMachine.Player.Input.playerActions.SwitchCharater3.started += SwitchCharater3;
            stateMachine.Player.Input.playerActions.SwitchCharater4.started += SwitchCharater4;
        }
        protected virtual void RemoveInputActionCallbacks()
        {
            stateMachine.Player.Input.playerActions.WalkToggle.started -= OnWalkToggleStarted;
            stateMachine.Player.Input.playerActions.SwitchCharater1.started -= SwitchCharater1;
            stateMachine.Player.Input.playerActions.SwitchCharater2.started -= SwitchCharater2;
            stateMachine.Player.Input.playerActions.SwitchCharater3.started -= SwitchCharater3;
            stateMachine.Player.Input.playerActions.SwitchCharater4.started -= SwitchCharater4;
        }
        protected void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            stateMachine.Player.rigidBody.AddForce(-playerHorizontalVelocity * stateMachine.reasubleData.MovementDecelerationforce, ForceMode.Acceleration);
        }
        protected void DecelerateVertically()
        {
            Vector3 playerVertivalVelocity = GetPlayerVerticalVelocity();

            stateMachine.Player.rigidBody.AddForce(-playerVertivalVelocity * stateMachine.reasubleData.MovementDecelerationforce, ForceMode.Acceleration);
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
        protected virtual void SwitchCharater1(InputAction.CallbackContext context)
        {
            SwapCharTo?.Invoke(0);
        }
        protected virtual void SwitchCharater2(InputAction.CallbackContext context)
        {
            SwapCharTo?.Invoke(1);
        }
        protected virtual void SwitchCharater3(InputAction.CallbackContext context)
        {
            SwapCharTo?.Invoke(2);
        }
        protected virtual void SwitchCharater4(InputAction.CallbackContext context)
        {
            SwapCharTo?.Invoke(3);
        }
        #endregion
    }
}
