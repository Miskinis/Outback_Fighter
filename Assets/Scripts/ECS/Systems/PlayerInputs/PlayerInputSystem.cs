using ECS.Components;
using ECS.Components.Mecanim;
using ECS.Components.PlayerInputs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ECS.Systems.PlayerInputs
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
        private EntityQuery _startCrouchingQuery;
        private EntityQuery _stopCrouchingQuery;
        private EntityQuery _gravityQuery;
        private EntityQuery _controllerQuery;
        private EntityQuery _attackQuery;
        private EntityQuery _specialAttackQuery;

        protected override void OnCreate()
        {
            _bootstrapQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<CharacterController>(),
                    ComponentType.ReadWrite<PlayerInput>(),
                },
                None = new[]
                {
                    ComponentType.ReadOnly<PlayerMoveDirection>(),
                    ComponentType.ReadOnly<PlayerMoveInput>(),
                    ComponentType.ReadOnly<PlayerFeetPoint>(),
                }
            });

            _inputQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlayerInput>(),
                    ComponentType.ReadWrite<PlayerMoveInput>(),
                    ComponentType.ReadOnly<CameraHorizontalAxis>(), 
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
                },
                None = new []
                {
                    ComponentType.ReadOnly<PlayerIsJumping>(),
                    ComponentType.ReadOnly<PlayerIsCrouching>(), 
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
                    ComponentType.ReadOnly<PlayerIsJumping>(),
                }
            });

            _isJumpingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimSetBool>(),
                    ComponentType.ReadOnly<MecanimIsJumpingParameter>(),
                    ComponentType.ReadOnly<PlayerFeetPoint>(),
                },
                None = new []
                {
                    ComponentType.ReadOnly<PlayerIsGrounded>(),
                }
            });

            _startCrouchingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimSetBool>(),
                    ComponentType.ReadOnly<MecanimIsCrouchingParameter>(),
                    ComponentType.ReadOnly<PlayerIsCrouching>(),
                }
            });
            
            _stopCrouchingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimSetBool>(),
                    ComponentType.ReadOnly<MecanimIsCrouchingParameter>(),
                },
                None = new []
                {
                    ComponentType.ReadOnly<PlayerIsCrouching>(),
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
                    ComponentType.ReadWrite<PlayerFeetPoint>(),
                    ComponentType.ReadOnly<Translation>(),
                }
            });

            _attackQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimTrigger>(),
                    ComponentType.ReadOnly<MecanimAttackParameter>(),
                    ComponentType.ReadOnly<PlayerIsAttacking>(),
                }
            });
            
            _specialAttackQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimTrigger>(),
                    ComponentType.ReadOnly<MecanimSpecialAttackParameter>(),
                    ComponentType.ReadOnly<PlayerIsSpecialAttacking>(),
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_bootstrapQuery)
                .ForEach((Entity entity,
                    CharacterController characterController,
                    PlayerInput playerInput) =>
                {
                    var feetPoint = characterController.center;
                    feetPoint.y -= characterController.height / 2f;
                    PostUpdateCommands.AddComponent(entity, new PlayerFeetPoint(feetPoint));
                    PostUpdateCommands.AddComponent(entity, new PlayerMoveInput());
                    PostUpdateCommands.AddComponent(entity, new PlayerMoveDirection());
                    PostUpdateCommands.AddComponent(entity, new CameraHorizontalAxis(Camera.main.transform.right.x));
                    
                    if (playerInput.currentControlScheme == null)
                    {
                        playerInput.SwitchCurrentControlScheme($"Player{playerInput.playerIndex+1}_Keyboard", Keyboard.current);
                    }
                });

            Entities.With(_inputQuery)
                .ForEach((Entity entity,
                    PlayerInput playerInput,
                    ref PlayerMoveInput moveInput,
                    ref CameraHorizontalAxis cameraHorizontalAxis) =>
                {
                    moveInput.value = playerInput.currentActionMap.asset["Move"].ReadValue<float>() * cameraHorizontalAxis.value;
                    
                    if (playerInput.currentActionMap.asset["Crouch"].ReadValue<float>() > 0)
                    {
                        PostUpdateCommands.AddComponent<PlayerIsCrouching>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerIsCrouching>(entity);
                    }

                    if (playerInput.currentActionMap.asset["Jump"].triggered)
                    {
                        PostUpdateCommands.AddComponent<PlayerIsJumping>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerIsJumping>(entity);
                    }
                    
                    if (playerInput.currentActionMap.asset["Attack"].triggered)
                    {
                        PostUpdateCommands.AddComponent<PlayerIsAttacking>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerIsAttacking>(entity);
                    }
                    
                    if (playerInput.currentActionMap.asset["SpecialAttack"].triggered)
                    {
                        PostUpdateCommands.AddComponent<PlayerIsSpecialAttacking>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerIsSpecialAttacking>(entity);
                    }
                });

            Entities.With(_moveQuery)
                .ForEach((DynamicBuffer<MecanimSetBool> mecanimSetBoolBuffer,
                    ref MecanimIsWalkingParameter mecanimIsWalkingParameter,
                    ref PlayerMoveDirection playerMoveDirection,
                    ref PlayerMoveSpeed playerMoveSpeed,
                    ref PlayerMoveInput playerMoveInput) =>
                {
                    playerMoveDirection.value.x = playerMoveInput.value * playerMoveSpeed.value;
                    playerMoveDirection.value.y = 0f;
                });

            Entities.With(_rotateQuery)
                .ForEach((Transform transform, ref PlayerMoveInput playerMoveInput) =>
                {
                    if (playerMoveInput.value != 0f)
                    {
                        transform.rotation = Quaternion.LookRotation(new Vector3(playerMoveInput.value, 0f, 0f), Vector3.up);
                    }
                });

            Entities.With(_moveAnimationQuery)
                .ForEach((DynamicBuffer<MecanimSetFloat> mecanimSetFloatBuffer,
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
                    ref PlayerMoveDirection playerMoveDirection,
                    ref PlayerJumpSpeed playerJumpSpeed) =>
                {
                    mecanimTriggerBuffer.Add(new MecanimTrigger(mecanimJumpParameter.value));
                    playerMoveDirection.value.y += playerJumpSpeed.value;
                });

            float deltaTime = Time.deltaTime;

            Entities.With(_isJumpingQuery)
                .ForEach((DynamicBuffer<MecanimSetBool> mecanimSetBoolBuffer,
                    ref MecanimIsJumpingParameter mecanimIsJumpingParameter,
                    ref PlayerFeetPoint playerFeetPoint,
                    ref PlayerGravity playerGravity) =>
                {
                    bool isJumping = !Physics.Raycast(playerFeetPoint.value, Vector3.down, playerGravity.value * deltaTime * 2f);
                    mecanimSetBoolBuffer.Add(new MecanimSetBool(mecanimIsJumpingParameter.value, isJumping));
                });

            Entities.With(_startCrouchingQuery)
                .ForEach((DynamicBuffer<MecanimSetBool> mecanimSetBoolBuffer,
                    ref MecanimIsCrouchingParameter mecanimIsCrouchingParameter,
                    ref PlayerMoveDirection playerMoveDirection) =>
                {
                    mecanimSetBoolBuffer.Add(new MecanimSetBool(mecanimIsCrouchingParameter.value, true));
                    playerMoveDirection.value.x = 0f;
                });
            
            Entities.With(_stopCrouchingQuery)
                .ForEach((DynamicBuffer<MecanimSetBool> mecanimSetBoolBuffer,
                    ref MecanimIsCrouchingParameter mecanimIsCrouchingParameter) =>
                {
                    mecanimSetBoolBuffer.Add(new MecanimSetBool(mecanimIsCrouchingParameter.value, false));
                });

            Entities.With(_gravityQuery)
                .ForEach((ref PlayerMoveDirection playerMoveDirection, ref PlayerGravity playerGravity) => { playerMoveDirection.value.y -= playerGravity.value * deltaTime; });

            Entities.With(_controllerQuery)
                .ForEach((Entity entity,
                    CharacterController characterController,
                    ref PlayerMoveDirection playerMoveDirection,
                    ref PlayerFeetPoint playerFeetPoint,
                    ref Translation translation) =>
                {
                    characterController.Move(playerMoveDirection.value * deltaTime);
                    if (characterController.isGrounded)
                    {
                        PostUpdateCommands.AddComponent<PlayerIsGrounded>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerIsGrounded>(entity);
                    }

                    var feetPoint = translation.Value + (float3) characterController.center;
                    feetPoint.y           -= characterController.height / 2f;
                    playerFeetPoint.value =  feetPoint;
                });

            Entities.With(_attackQuery)
                .ForEach((DynamicBuffer<MecanimTrigger> mecanimTrigger,
                    ref MecanimAttackParameter mecanimAttackParameter,
                    ref PlayerIsAttacking playerAttackInput) =>
                {
                    mecanimTrigger.Add(new MecanimTrigger(mecanimAttackParameter.value));
                });

            Entities.With(_specialAttackQuery)
                .ForEach((DynamicBuffer<MecanimTrigger> mecanimTrigger,
                    ref MecanimSpecialAttackParameter mecanimSpecialAttackParameter,
                    ref PlayerIsSpecialAttacking playerSpecialAttackInput) =>
                {
                    mecanimTrigger.Add(new MecanimTrigger(mecanimSpecialAttackParameter.value));
                });
        }
    }
}