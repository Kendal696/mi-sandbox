# Unity VR Scripts Collection

This collection contains essential C# scripts for implementing VR functionality in Unity projects. Each script is designed to handle specific aspects of VR interaction and can be used independently or together to create a complete VR experience.

## Scripts Overview

### 1. VRControllerInput.cs
Handles basic VR controller input detection and processing.
- Tracks controller position and rotation
- Detects trigger and grip button presses
- Provides access to controller state and input values

### 2. VRTeleportation.cs
Implements teleportation mechanics for VR movement.
- Ray-based teleportation system
- Visual feedback with line renderer
- Valid/invalid location indicators
- Customizable teleport distance and layer masks

### 3. VRObjectInteraction.cs
Manages object interaction and manipulation in VR.
- Grab and release mechanics
- Physics-based object handling
- Throw mechanics with velocity tracking
- Layer-based interaction filtering

### 4. VRUIManager.cs
Handles VR-specific UI interaction.
- Ray-based UI interaction
- Hover and selection effects
- Button press detection
- Customizable interaction distances and colors

### 5. VRLocomotion.cs
Implements various VR movement systems.
- Smooth locomotion with thumbstick
- Snap turning
- Teleportation
- Ground detection

## Setup Instructions

1. Create a new Unity project with XR Interaction Toolkit package installed
2. Import these scripts into your project
3. Attach the scripts to appropriate GameObjects in your scene
4. Configure the serialized fields in the Inspector
5. Set up the required layers and physics settings

## Required Unity Packages

- XR Interaction Toolkit
- XR Plugin Management
- Input System

## Dependencies

- Unity 2020.3 or newer
- XR Interaction Toolkit 2.0.0 or newer
- Input System 1.0.0 or newer

## Usage Examples

### Basic Controller Setup
```csharp
// Attach VRControllerInput to your controller GameObject
var controllerInput = gameObject.AddComponent<VRControllerInput>();
```

### Teleportation Setup
```csharp
// Attach VRTeleportation to your XR Origin
var teleportation = xrOrigin.AddComponent<VRTeleportation>();
teleportation.rayInteractor = GetComponent<XRRayInteractor>();
```

### Object Interaction Setup
```csharp
// Attach VRObjectInteraction to your controller
var objectInteraction = controller.AddComponent<VRObjectInteraction>();
```

## Best Practices

1. Always use the XR Origin as the main player reference
2. Set up proper layer masks for interaction
3. Configure physics materials for grabbed objects
4. Use the Input System for controller input
5. Implement proper error handling for missing components

## Troubleshooting

- Ensure XR Interaction Toolkit is properly installed
- Check layer settings for interaction masks
- Verify controller input is working in the Input Debugger
- Make sure physics settings are configured correctly
- Check for missing component references in the Inspector

## Contributing

Feel free to modify and extend these scripts for your specific needs. Consider contributing improvements back to the community.

## License

This project is licensed under the MIT License - see the LICENSE file for details. 