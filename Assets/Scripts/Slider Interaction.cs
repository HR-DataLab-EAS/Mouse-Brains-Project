using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Jump;

namespace MenuNamespace.Sliders
{
    public class SliderInteraction : MonoBehaviour
    {
        [Header("Movement Settings")]
        public Slider moveSlider; // Reference to the slider
        public TMP_Text moveText; // Reference to the text that displays the current value of the slider

        [Header("Rotation Settings")]
        public Slider turnSlider; 
        public TMP_Text turnText;

        [Header("Jump Settings")]
        public Slider jumpSlider; 
        public TMP_Text jumpText;

        [Header("Providers")]
        public ContinuousMoveProvider moveProvider;
        public ContinuousTurnProvider turnProvider;
        public JumpProvider upProvider;
        public JumpProvider downProvider;

        [Header("Default Values")] // Default values for movement speed, rotation speed, and jump height
        private float moveDefault = 5f;
        private float turnDefault = 180f;
        private float jumpDefault = 1f;

        public void MovementSpeed(float value) {
            moveProvider.moveSpeed = value; // Set the move speed of the provider to the value from the slider
            moveText.text = value.ToString("0"); // Update the text to show the current value of the slider, formatted to 0 decimal places
        }

        public void RotationSpeed(float value) {
            turnProvider.turnSpeed = value;
            turnText.text = value.ToString("0");
        }

        public void JumpHeight(float value) {
            upProvider.jumpHeight = value;
            downProvider.jumpHeight = -value;
            jumpText.text = value.ToString("0");
        }

        public void RestoreAllDefaults() // Method to restore all sliders to their default values
        {
            moveSlider.value = moveDefault;
            turnSlider.value = turnDefault;
            jumpSlider.value = jumpDefault;
        }
    }
}