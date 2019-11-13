using ECS.Components;
using ECS.Components.Combat;
using ECS.Components.Mecanim;
using ECS.Components.PlayerInputs;
using ECS.Components.PlayerInputs.Internal;
using MecanimBehaviors;
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
        private EntityQuery _moveAnimationQuery;
        private EntityQuery _jumpQuery;
        private EntityQuery _isJumpingQuery;
        private EntityQuery _startCrouchingQuery;
        private EntityQuery _stopCrouchingQuery;
        private EntityQuery _gravityQuery;
        private EntityQuery _controllerQuery;
        private EntityQuery _attackQuery;
        private EntityQuery _dashQuery;
        private EntityQuery _specialAttackQuery;

        protected override void OnCreate()
        {
            _bootstrapQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<CharacterController>(),
                    ComponentType.ReadWrite<PlayerInput>(),
                    ComponentType.ReadWrite<Transform>(), 
                    ComponentType.ReadWrite<Animator>(), 
                },
                None = new[]
                {
                    ComponentType.ReadWrite<PlayerMoveDirection>(),
                    ComponentType.ReadWrite<PlayerMoveInput>(),
                    ComponentType.ReadWrite<PlayerFeetPoint>(),
                    ComponentType.ReadWrite<CameraHorizontalAxis>(), 
                    ComponentType.ReadWrite<InitialPlayerDepth>(), 
                    ComponentType.ReadWrite<Dead>(), 
                }
            });

            _inputQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlayerInput>(),
                    ComponentType.ReadWrite<PlayerMoveInput>(),
                    ComponentType.ReadOnly<CameraHorizontalAxis>(), 
                },
                None = new []
                {
                    ComponentType.ReadWrite<Dead>(), 
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
                    ComponentType.ReadWrite<PlayerInputJump>(),
                    ComponentType.ReadWrite<PlayerInputCrouch>(), 
                    ComponentType.ReadWrite<Dead>(), 
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
                },
                None = new []
                {
                    ComponentType.ReadWrite<Dead>(), 
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
                    ComponentType.ReadOnly<PlayerInputJump>(),
                },
                None = new []
                {
                    ComponentType.ReadWrite<Dead>(), 
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
                    ComponentType.ReadWrite<PlayerIsGrounded>(),
                    ComponentType.ReadWrite<Dead>(), 
                }
            });

            _startCrouchingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimSetBool>(),
                    ComponentType.ReadOnly<MecanimIsCrouchingParameter>(),
                    ComponentType.ReadOnly<PlayerInputCrouch>(),
                },
                None = new []
                {
                    ComponentType.ReadWrite<PlayerIsCrouching>(),
                    ComponentType.ReadWrite<Dead>(), 
                }
            });
            
            _stopCrouchingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimSetBool>(),
                    ComponentType.ReadOnly<MecanimIsCrouchingParameter>(),
                    ComponentType.ReadWrite<PlayerIsCrouching>(), 
                },
                None = new []
                {
                    ComponentType.ReadWrite<PlayerInputCrouch>(),
                    ComponentType.ReadWrite<Dead>(), 
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
                    ComponentType.ReadOnly<InitialPlayerDepth>(), 
                    ComponentType.ReadOnly<PlayerSize>(), 
                }
            });

            _attackQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimTrigger>(),
                    ComponentType.ReadOnly<MecanimAttackParameter>(),
                    ComponentType.ReadOnly<PlayerInputAttack>(),
                },
                None = new []
                {
                    ComponentType.ReadWrite<Dead>(), 
                }
            });

            _dashQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<PlayerInputDash>(),
                    ComponentType.ReadOnly<Rotation>(),
                    ComponentType.ReadOnly<PlayerMoveDirection>(),
                    ComponentType.ReadOnly<Dash>(), 
                }
            });
            
            _specialAttackQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<MecanimTrigger>(),
                    ComponentType.ReadOnly<MecanimSpecialAttackParameter>(),
                    ComponentType.ReadOnly<PlayerInputSpecialAttack>(),
                },
                None = new []
                {
                    ComponentType.ReadWrite<Dead>(), 
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_bootstrapQuery)
                .ForEach((Entity entity,
                    CharacterController characterController,
                    PlayerInput playerInput,
                    Transform transform,
                    Animator animator) =>
                {
                    var feetPoint = characterController.center;
                    feetPoint.y -= characterController.height / 2f;
                    PostUpdateCommands.AddComponent(entity, new PlayerFeetPoint(feetPoint));
                    PostUpdateCommands.AddComponent(entity, new PlayerMoveInput());
                    PostUpdateCommands.AddComponent(entity, new PlayerMoveDirection());
                    PostUpdateCommands.AddComponent(entity, new CameraHorizontalAxis(Camera.main.transform.right.x));
                    PostUpdateCommands.AddComponent(entity, new InitialPlayerDepth(transform.position.z));
                    
                    if (playerInput.currentControlScheme == null)
                    {
                        playerInput.SwitchCurrentControlScheme($"Player{playerInput.playerIndex+1}_Keyboard", Keyboard.current);
                    }
                    
                    PlayerManager.instance.RegisterPlayer(transform, entity);
                    
                    void OnExitAction() => World.Active.GetExistingSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer().RemoveComponent<PlayerIsCrouching>(entity);
                    foreach (var isCrouchingBehavior in animator.GetBehaviours<StopCrouchingBehavior>())
                    {
                        isCrouchingBehavior.onExitAction = OnExitAction;
                    }

                    void OnStartAction() => World.Active.GetExistingSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer().AddComponent<PlayerInputDash>(entity);
                    void OnEndAction() => World.Active.GetExistingSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer().RemoveComponent<PlayerInputDash>(entity);
                    foreach (var playerDashBehavior in animator.GetBehaviours<PlayerDashBehavior>())
                    {
                        playerDashBehavior.onStartAction = OnStartAction;
                        playerDashBehavior.onEndAction = OnEndAction;
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
                        PostUpdateCommands.AddComponent<PlayerInputCrouch>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerInputCrouch>(entity);
                    }

                    if (playerInput.currentActionMap.asset["Jump"].triggered)
                    {
                        PostUpdateCommands.AddComponent<PlayerInputJump>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerInputJump>(entity);
                    }
                    
                    if (playerInput.currentActionMap.asset["Attack"].triggered)
                    {
                        PostUpdateCommands.AddComponent<PlayerInputAttack>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerInputAttack>(entity);
                    }
                    
                    if (playerInput.currentActionMap.asset["SpecialAttack"].triggered)
                    {
                        PostUpdateCommands.AddComponent<PlayerInputSpecialAttack>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerInputSpecialAttack>(entity);
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
                .ForEach((Entity entity,
                    DynamicBuffer<MecanimSetBool> mecanimSetBoolBuffer,
                    ref MecanimIsCrouchingParameter mecanimIsCrouchingParameter,
                    ref PlayerMoveDirection playerMoveDirection) =>
                {
                    PostUpdateCommands.AddComponent(entity, new PlayerIsCrouching());
                    mecanimSetBoolBuffer.Add(new MecanimSetBool(mecanimIsCrouchingParameter.value, true));
                    playerMoveDirection.value.x = 0f;
                });
            
            Entities.With(_stopCrouchingQuery)
                .ForEach((DynamicBuffer<MecanimSetBool> mecanimSetBoolBuffer,
                    ref MecanimIsCrouchingParameter mecanimIsCrouchingParameter) =>
                {
                    mecanimSetBoolBuffer.Add(new MecanimSetBool(mecanimIsCrouchingParameter.value, false));
                });

            Entities.With(_attackQuery).ForEach((DynamicBuffer<MecanimTrigger> mecanimTrigger, ref MecanimAttackParameter mecanimAttackParameter) =>
                {
                    mecanimTrigger.Add(new MecanimTrigger(mecanimAttackParameter.value));
                });

            Entities.With(_dashQuery).ForEach((ref Dash dash, ref PlayerMoveDirection playerMoveDirection, ref Rotation rotation) =>
            {
                playerMoveDirection.value.x = (dash.speed * math.forward(rotation.Value)).x;
            });

            Entities.With(_specialAttackQuery).ForEach((DynamicBuffer<MecanimTrigger> mecanimTrigger, ref MecanimSpecialAttackParameter mecanimSpecialAttackParameter) =>
            {
                mecanimTrigger.Add(new MecanimTrigger(mecanimSpecialAttackParameter.value));
            });
            
            Entities.With(_gravityQuery)
                .ForEach((ref PlayerMoveDirection playerMoveDirection, ref PlayerGravity playerGravity) => { playerMoveDirection.value.y -= playerGravity.value * deltaTime; });

            Entities.With(_controllerQuery)
                .ForEach((Entity entity,
                    CharacterController characterController,
                    ref PlayerMoveDirection playerMoveDirection,
                    ref PlayerFeetPoint playerFeetPoint,
                    ref InitialPlayerDepth initialPlayerDepth,
                    ref PlayerSize playerSize) =>
                {
                    var transform = characterController.transform;

                    characterController.center = transform.InverseTransformPoint(playerSize.center);
                    //characterController.radius = playerSize.radius;
                    characterController.height = playerSize.height;

                    characterController.Move(playerMoveDirection.value * deltaTime);
                    
                    var position = transform.position;
                    position.z         = initialPlayerDepth.value;
                    transform.position = position;

                    if (characterController.isGrounded)
                    {
                        PostUpdateCommands.AddComponent<PlayerIsGrounded>(entity);
                    }
                    else
                    {
                        PostUpdateCommands.RemoveComponent<PlayerIsGrounded>(entity);
                    }

                    var feetPoint = position + characterController.center;
                    feetPoint.y           -= characterController.height / 2f;
                    playerFeetPoint.value =  feetPoint;
                });
        }
    }
}