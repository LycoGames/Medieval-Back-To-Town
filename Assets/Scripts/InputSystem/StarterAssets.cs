// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputSystem/StarterAssets.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @StarterAssets : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @StarterAssets()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""StarterAssets"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""f62a4b92-ef5e-4175-8f4c-c9075429d32c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""6bc1aaf4-b110-4ff7-891e-5b9fe6f32c4d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""2690c379-f54d-45be-a724-414123833eb4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""8c4abdf8-4099-493a-aa1a-129acec7c3df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""PassThrough"",
                    ""id"": ""980e881e-182c-404c-8cbf-3d09fdb48fef"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InventoryShowHide"",
                    ""type"": ""Button"",
                    ""id"": ""c70d543e-fc7b-445b-9a20-3e3b9f7063f3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BasicAttack"",
                    ""type"": ""Button"",
                    ""id"": ""a4431aaa-e70a-420f-867e-1b827f54713e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""59dd68cf-5ab9-4029-bd2e-99619b252989"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""959e0c6c-9eb3-4ed6-8439-ed4224f2895f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuestShowHide"",
                    ""type"": ""Button"",
                    ""id"": ""6ea02926-3e32-43d2-8943-c292a121b426"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Abilitiy1"",
                    ""type"": ""Button"",
                    ""id"": ""c9cfc826-98df-4cb2-9097-a9e9a7c25b90"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Abilitiy2"",
                    ""type"": ""Button"",
                    ""id"": ""6deb9895-9624-4231-9a73-f445429def91"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Abilitiy3"",
                    ""type"": ""Button"",
                    ""id"": ""5d89fdaa-1785-487c-8f4a-992f0b0abc08"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Abilitiy4"",
                    ""type"": ""Button"",
                    ""id"": ""3a62eec8-b2f1-40c3-b997-14eadb3097de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Abilitiy5"",
                    ""type"": ""Button"",
                    ""id"": ""29bba88a-9dff-49ff-953d-786e4bc6540d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Abilitiy6"",
                    ""type"": ""Button"",
                    ""id"": ""821ab966-450a-49fc-8a2d-dc1b748d9a6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""b7594ddb-26c9-4ba2-bd5a-901468929edc"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2063a8b5-6a45-43de-851b-65f3d46e7b58"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""64e4d037-32e1-4fb9-80e4-fc7330404dfe"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0fce8b11-5eab-4e4e-a741-b732e7b20873"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7bdda0d6-57a8-47c8-8238-8aecf3110e47"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bb94b405-58d3-4998-8535-d705c1218a98"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""929d9071-7dd0-4368-9743-6793bb98087e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""28abadba-06ff-4d37-bb70-af2f1e35a3b9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""45f115b6-9b4f-4ba8-b500-b94c93bf7d7e"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e2f9aa65-db06-4c5b-a2e9-41bc8acb9517"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed66cbff-2900-4a62-8896-696503cfcd31"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2(x=15,y=15)"",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1d171b6-19d8-47a6-ba3a-71b6a8e7b3c0"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),StickDeadzone,ScaleVector2(x=300,y=300)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1bd55a0b-761e-4ae4-89ae-8ec127e08a29"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f973413-5e27-4239-acee-38c4a63feeba"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc65b89f-9bd3-43fb-92af-d0d87ba5faa4"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8fcd86e-dcfd-4f88-8e93-b638cdbf3320"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b48297bd-60f1-4f8a-82df-2b4285938b86"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""InventoryShowHide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf210ce2-3473-4944-a504-850bbe2b5378"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""BasicAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7e3323e-ba10-413d-9504-234c62299e9a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8cc951b-392b-4732-82de-c5697f19268e"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""095aa851-53b5-4d76-ad29-fcd1bbdb2e7f"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""QuestShowHide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32ee8a30-9c39-4404-ab87-0b882680b194"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Abilitiy1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c5b98eb-e72e-44fe-b34d-d7133c38a396"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Abilitiy2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""72e07acc-0f85-44a8-b2e3-9b0bc2021ee6"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Abilitiy3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""acb3bff3-2dd8-43ca-a45b-9c1bf577e9a1"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Abilitiy4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08961263-5dd4-4e97-9c8a-a15ca880d2f1"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Abilitiy5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56f4eb20-428a-4fc7-b250-015c66812876"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Abilitiy6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Xbox Controller"",
            ""bindingGroup"": ""Xbox Controller"",
            ""devices"": []
        },
        {
            ""name"": ""PS4 Controller"",
            ""bindingGroup"": ""PS4 Controller"",
            ""devices"": []
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Sprint = m_Player.FindAction("Sprint", throwIfNotFound: true);
        m_Player_InventoryShowHide = m_Player.FindAction("InventoryShowHide", throwIfNotFound: true);
        m_Player_BasicAttack = m_Player.FindAction("BasicAttack", throwIfNotFound: true);
        m_Player_Roll = m_Player.FindAction("Roll", throwIfNotFound: true);
        m_Player_Interaction = m_Player.FindAction("Interaction", throwIfNotFound: true);
        m_Player_QuestShowHide = m_Player.FindAction("QuestShowHide", throwIfNotFound: true);
        m_Player_Abilitiy1 = m_Player.FindAction("Abilitiy1", throwIfNotFound: true);
        m_Player_Abilitiy2 = m_Player.FindAction("Abilitiy2", throwIfNotFound: true);
        m_Player_Abilitiy3 = m_Player.FindAction("Abilitiy3", throwIfNotFound: true);
        m_Player_Abilitiy4 = m_Player.FindAction("Abilitiy4", throwIfNotFound: true);
        m_Player_Abilitiy5 = m_Player.FindAction("Abilitiy5", throwIfNotFound: true);
        m_Player_Abilitiy6 = m_Player.FindAction("Abilitiy6", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Sprint;
    private readonly InputAction m_Player_InventoryShowHide;
    private readonly InputAction m_Player_BasicAttack;
    private readonly InputAction m_Player_Roll;
    private readonly InputAction m_Player_Interaction;
    private readonly InputAction m_Player_QuestShowHide;
    private readonly InputAction m_Player_Abilitiy1;
    private readonly InputAction m_Player_Abilitiy2;
    private readonly InputAction m_Player_Abilitiy3;
    private readonly InputAction m_Player_Abilitiy4;
    private readonly InputAction m_Player_Abilitiy5;
    private readonly InputAction m_Player_Abilitiy6;
    public struct PlayerActions
    {
        private @StarterAssets m_Wrapper;
        public PlayerActions(@StarterAssets wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Sprint => m_Wrapper.m_Player_Sprint;
        public InputAction @InventoryShowHide => m_Wrapper.m_Player_InventoryShowHide;
        public InputAction @BasicAttack => m_Wrapper.m_Player_BasicAttack;
        public InputAction @Roll => m_Wrapper.m_Player_Roll;
        public InputAction @Interaction => m_Wrapper.m_Player_Interaction;
        public InputAction @QuestShowHide => m_Wrapper.m_Player_QuestShowHide;
        public InputAction @Abilitiy1 => m_Wrapper.m_Player_Abilitiy1;
        public InputAction @Abilitiy2 => m_Wrapper.m_Player_Abilitiy2;
        public InputAction @Abilitiy3 => m_Wrapper.m_Player_Abilitiy3;
        public InputAction @Abilitiy4 => m_Wrapper.m_Player_Abilitiy4;
        public InputAction @Abilitiy5 => m_Wrapper.m_Player_Abilitiy5;
        public InputAction @Abilitiy6 => m_Wrapper.m_Player_Abilitiy6;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Sprint.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @InventoryShowHide.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventoryShowHide;
                @InventoryShowHide.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventoryShowHide;
                @InventoryShowHide.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventoryShowHide;
                @BasicAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBasicAttack;
                @BasicAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBasicAttack;
                @BasicAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBasicAttack;
                @Roll.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @Interaction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteraction;
                @QuestShowHide.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuestShowHide;
                @QuestShowHide.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuestShowHide;
                @QuestShowHide.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuestShowHide;
                @Abilitiy1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy1;
                @Abilitiy1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy1;
                @Abilitiy1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy1;
                @Abilitiy2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy2;
                @Abilitiy2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy2;
                @Abilitiy2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy2;
                @Abilitiy3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy3;
                @Abilitiy3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy3;
                @Abilitiy3.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy3;
                @Abilitiy4.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy4;
                @Abilitiy4.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy4;
                @Abilitiy4.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy4;
                @Abilitiy5.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy5;
                @Abilitiy5.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy5;
                @Abilitiy5.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy5;
                @Abilitiy6.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy6;
                @Abilitiy6.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy6;
                @Abilitiy6.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbilitiy6;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @InventoryShowHide.started += instance.OnInventoryShowHide;
                @InventoryShowHide.performed += instance.OnInventoryShowHide;
                @InventoryShowHide.canceled += instance.OnInventoryShowHide;
                @BasicAttack.started += instance.OnBasicAttack;
                @BasicAttack.performed += instance.OnBasicAttack;
                @BasicAttack.canceled += instance.OnBasicAttack;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @QuestShowHide.started += instance.OnQuestShowHide;
                @QuestShowHide.performed += instance.OnQuestShowHide;
                @QuestShowHide.canceled += instance.OnQuestShowHide;
                @Abilitiy1.started += instance.OnAbilitiy1;
                @Abilitiy1.performed += instance.OnAbilitiy1;
                @Abilitiy1.canceled += instance.OnAbilitiy1;
                @Abilitiy2.started += instance.OnAbilitiy2;
                @Abilitiy2.performed += instance.OnAbilitiy2;
                @Abilitiy2.canceled += instance.OnAbilitiy2;
                @Abilitiy3.started += instance.OnAbilitiy3;
                @Abilitiy3.performed += instance.OnAbilitiy3;
                @Abilitiy3.canceled += instance.OnAbilitiy3;
                @Abilitiy4.started += instance.OnAbilitiy4;
                @Abilitiy4.performed += instance.OnAbilitiy4;
                @Abilitiy4.canceled += instance.OnAbilitiy4;
                @Abilitiy5.started += instance.OnAbilitiy5;
                @Abilitiy5.performed += instance.OnAbilitiy5;
                @Abilitiy5.canceled += instance.OnAbilitiy5;
                @Abilitiy6.started += instance.OnAbilitiy6;
                @Abilitiy6.performed += instance.OnAbilitiy6;
                @Abilitiy6.canceled += instance.OnAbilitiy6;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_XboxControllerSchemeIndex = -1;
    public InputControlScheme XboxControllerScheme
    {
        get
        {
            if (m_XboxControllerSchemeIndex == -1) m_XboxControllerSchemeIndex = asset.FindControlSchemeIndex("Xbox Controller");
            return asset.controlSchemes[m_XboxControllerSchemeIndex];
        }
    }
    private int m_PS4ControllerSchemeIndex = -1;
    public InputControlScheme PS4ControllerScheme
    {
        get
        {
            if (m_PS4ControllerSchemeIndex == -1) m_PS4ControllerSchemeIndex = asset.FindControlSchemeIndex("PS4 Controller");
            return asset.controlSchemes[m_PS4ControllerSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnInventoryShowHide(InputAction.CallbackContext context);
        void OnBasicAttack(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnQuestShowHide(InputAction.CallbackContext context);
        void OnAbilitiy1(InputAction.CallbackContext context);
        void OnAbilitiy2(InputAction.CallbackContext context);
        void OnAbilitiy3(InputAction.CallbackContext context);
        void OnAbilitiy4(InputAction.CallbackContext context);
        void OnAbilitiy5(InputAction.CallbackContext context);
        void OnAbilitiy6(InputAction.CallbackContext context);
    }
}
