using ECS.Components;
using ECS.Components.Mecanim;
using ECS.Components.PlayerInputs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ECS.Systems
{
    public class PlayerInputSystem : ComponentSystem
    {
        private EntityQuery _bootstrapQuery;
        private EntityQuery _inputQuery;
        private EntityQuery _moveQuery;
        private EntityQuery _rotateQuery;
        private EntityQuery _moveAnimationQuery;
        private EntityQuery _jumpQuery;
        private EntityQuery _isJumpingQuery;
        private EntityQuery _crouchQuery;
        private EntityQuery _gravityQuery;
        private EntityQuery _controllerQuery;

        protected override void OnCreate()
        {
            _bootstrapQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new []
                {
                    ComponentType.ReadWrite<CharacterController>(), 
                },
                None = new[]
                {
                    ComponentType.ReadOnly<PlayerMoveDirection>(),
                    ComponentType.ReadOnly<PlayerMoveInput>(),
                    ComponentType.ReadOnly<PlayerJumpInput>(),
                    ComponentType.ReadOnly<PlayerCrouchInput>(),
                    ComponentType.ReadOnly<PlayerIsGrounded>(),
                    ComponentType.ReadOnly<PlayerFeetPoint>(), 
                }
            });

            _inputQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlayerInput>(),
                    ComponentType.ReadWrite<PlayerMoveInput>(),
                    ComponentType.ReadWrite<PlayerJumpInput>(),
                    ComponentType.ReadWrite<PlayerCrouchInput>(),
                    ComponentType.ReadOnly<PlayerIsGrounded>(),
                }
            });

            _moveQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimSetBool>(),
                    ComponentType.ReadOnly<MecanimIsWalkingParameter>(),
                    ComponentType.ReadWrite<PlayerMoveDirection>(),
                    ComponentType.ReadOnly<PlayerMoveSpeed>(),
                    ComponentType.ReadOnly<PlayerIsGrounded>(),
                    ComponentType.ReadOnly<PlayerMoveInput>(),
                }
            });

            _rotateQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Transform>(),
                    ComponentType.ReadOnly<PlayerMoveInput>(),
                }
            });

            _moveAnimationQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimSetFloat>(),
                    ComponentType.ReadOnly<MecanimMoveDirectionParameter>(),
                    ComponentType.ReadOnly<MecanimMoveSpeedParameter>(),
                    ComponentType.ReadOnly<PlayerMoveInput>(),
                    ComponentType.ReadOnly<PlayerMoveSpeed>(),
                }
            });

            _jumpQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimTrigger>(),
                    ComponentType.ReadOnly<MecanimJumpParameter>(),
                    ComponentType.ReadOnly<PlayerIsGrounded>(),
                    ComponentType.ReadWrite<PlayerMoveDirection>(),
                    ComponentType.ReadOnly<PlayerJumpSpeed>(),
                    ComponentType.ReadOnly<PlayerJumpInput>(),
                }
            });

            _isJumpingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimSetBool>(),
                    ComponentType.ReadOnly<MecanimIsJumpingParameter>(),
                    ComponentType.ReadOnly<PlayerFeetPoint>(), 
                    ComponentType.ReadOnly<PlayerIsGrounded>(), 
                }
            });

            _crouchQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimSetBool>(),
                    ComponentType.ReadOnly<MecanimIsCrouchingParameter>(),
                    ComponentType.ReadOnly<PlayerIsGrounded>(),
                    ComponentType.ReadOnly<PlayerCrouchInput>(),
                }
            });

            _gravityQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlayerMoveDirection>(),
                    ComponentType.ReadOnly<PlayerGravity>(),
                }
            });

            _controllerQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<CharacterController>(),
                    ComponentType.ReadOnly<PlayerMoveDirection>(),
                    ComponentType.ReadWrite<PlayerIsGrounded>(),
                    ComponentType.ReadWrite<PlayerFeetPoint>(),
                    ComponentType.ReadOnly<Translation>(), 
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_bootstrapQuery)
                .ForEach((Entity entity, CharacterController characterController) =>
                {
                    var feetPoint = characterController.center;
                    feetPoint.y -= characterController.height / 2f;
                    PostUpdateCommands.AddComponent(entity, new PlayerFeetPoint(feetPoint));
                    PostUpdateCommands.AddComponent(entity, new PlayerMoveInput());
                    PostUpdateCommands.AddComponent(entity, new PlayerJumpInput());
                    PostUpdateCommands.AddComponent(entity, new PlayerCrouchInput());
                    PostUpdateCommands.AddComponent(entity, new PlayerIsGrounded{value = true});
                    PostUpdateCommands.AddComponent(entity, new PlayerMoveDirection());
                });

            Entities.With(_inputQuery)
                .ForEach((PlayerInput playerInput,
                    ref PlayerMoveInput moveInput,
                    ref PlayerJumpInput jumpInput,
                    ref PlayerCrouchInput crouchInput,
                    ref PlayerIsGrounded playerIsGrounded) =>
                {
                    crouchInput.value = playerInput.actions["Crouch"].ReadValue<float>() > 0;
                    
                    if(playerIsGrounded.value && crouchInput.value == false)
                    {
                        moveInput.value = playerInput.actions["Move"].ReadValue<float>();
                        jumpInput.value = playerInput.actions["Jump"].triggered;
                    }
                });

            Entities.With(_moveQuery)
                .ForEach((DynamicBuffer<MecanimSetBool> mecanimSetBoolBuffer,
                    ref MecanimIsWalkingParameter mecanimIsWalkingParameter,
                    ref PlayerMoveDirection playerMoveDirection,
                    ref PlayerMoveSpeed playerMoveSpeed,
                    ref PlayerIsGrounded playerIsGrounded,
                    ref PlayerMoveInput playerMoveInput) =>
                {
                    if (playerIsGrounded.value)
                    {
                        playerMoveDirection.value = new Vector3(playerMoveInput.value * playerMoveSpeed.value, 0f, 0f);
                    }
                });
            
            Entities.With(_rotateQuery).ForEach((Transform transform, ref PlayerMoveInput playerMoveInput) =>
            {
                if(playerMoveInput.value != 0f)
                {
                    transform.rotation = Quaternion.LookRotation(new Vector3(playerMoveInput.value, 0f, 0f), Vector3.up);
                }
            });
            
            Entities.With(_moveAnimationQuery).ForEach((DynamicBuffer<MecanimSetFloat> mecanimSetFloatBuffer,
                ref MecanimMoveDirectionParameter mecanimMoveDirectionParameter,
                ref MecanimMoveSpeedParameter mecanimMoveSpeedParameter,
                ref PlayerMoveSpeed playerMoveSpeed,
                ref PlayerMoveInput playerMoveInput) =>
            {
                mecanimSetFloatBuffer.Add(new MecanimSetFloat(mecanimMoveDirectionParameter.value, playerMoveInput.value));
                
                mecanimSetFloatBuffer.Add(playerMoveInput.value != 0f
                    ? new MecanimSetFloat(mecanimMoveSpeedParameter.value, playerMoveSpeed.value)
                    : new MecanimSetFloat(mecanimMoveSpeedParameter.value, 1f));
            });

            Entities.With(_jumpQuery)
                .ForEach((DynamicBuffer<MecanimTrigger> mecanimTriggerBuffer,
                    ref MecanimJumpParameter mecanimJumpParameter,
                    ref PlayerIsGrounded playerIsGrounded,
                    ref PlayerJumpInput playerJumpInput,
                    ref PlayerMoveDirection playerMoveDirection,
                    ref PlayerJumpSpeed playerJumpSpeed) =>
                {
                    if (playerJumpInput.value && playerIsGrounded.value)
                    {
                        mecanimTriggerBuffer.Add(new MecanimTrigger(mecanimJumpParameter.value));
                        playerMoveDirection.value.y += playerJumpSpeed.value;
                    }
                });
            
            float deltaTime = Time.deltaTime;
            
            Entities.With(_isJumpingQuery)
                .ForEach((DynamicBuffer<MecanimSetBool> mecanimSetBoolBuffer,
                    ref MecanimIsJumpingParameter mecanimIsJumpingParameter,
                    ref PlayerFeetPoint playerFeetPoint,
                    ref PlayerIsGrounded playerIsGrounded,
                    ref PlayerGravity playerGravity) =>
                {
                    bool isJumping = !playerIsGrounded.value;
                    if (isJumping)
                    {
                        isJumping = !Physics.Raycast(playerFeetPoint.value, Vector3.down, playerGravity.value * deltaTime * 2f);
                    }
                    mecanimSetBoolBuffer.Add(new MecanimSetBool(mecanimIsJumpingParameter.value, isJumping));
                });

            Entities.With(_crouchQuery)
                .ForEach((DynamicBuffer<MecanimSetBool> mecanimSetBoolBuffer,
                    ref MecanimIsCrouchingParameter mecanimIsCrouchingParameter,
                    ref PlayerIsGrounded playerIsGrounded,
                    ref PlayerCrouchInput playerCrouchInput) =>
                {
                    if (playerIsGrounded.value)
                    {
                        mecanimSetBoolBuffer.Add(new MecanimSetBool(mecanimIsCrouchingParameter.value, playerCrouchInput.value));
                    }
                });

            Entities.With(_gravityQuery)
                .ForEach((ref PlayerMoveDirection playerMoveDirection, ref PlayerGravity playerGravity) => { playerMoveDirection.value.y -= playerGravity.value * deltaTime; });

            Entities.With(_controllerQuery)
                .ForEach((CharacterController characterController,
                    ref PlayerMoveDirection playerMoveDirection,
                    ref PlayerIsGrounded playerIsGrounded,
                    ref PlayerFeetPoint playerFeetPoint,
                    ref Translation translation) =>
                {
                    characterController.Move(playerMoveDirection.value * deltaTime);
                    playerIsGrounded.value = characterController.isGrounded;
                    
                    var feetPoint = translation.Value + (float3)characterController.center;
                    feetPoint.y -= characterController.height / 2f;
                    playerFeetPoint.value = feetPoint;
                });
        }
    }
}