# MouseTester 🖱️

A fast, lightweight, and cross-platform Mouse Polling Rate Tester application built with C# and Silk.NET.

Designed to accurately measure your mouse's reporting rate (Hz) in real-time, supporting standard and high-performance gaming mice up to 1000 Hz and beyond.

![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20macOS%20%7C%20Linux-lightgrey)
![Tech](https://img.shields.io/badge/built%20with-Silk.NET-purple)

## Features ✨

-   **Real-time Hz Monitoring:** accurate polling rate calculation using high-precision timestamps.
-   **Raw Input Support:** attempts to bypass OS filtering for the most direct hardware readings.
-   **Cross-Platform:**
    -   **Windows:** Supports Raw Input via SDL/GLFW backend.
    -   **macOS / Linux:** Adaptive fallback to standard event handling for broad compatibility.
-   **Visual Feedback:**
    -   Color-coded Hz display (Green for >900 Hz, Blue for >3000 Hz).
    -   Clean, hardware-accelerated UI using ImGui.
-   **Zero Bloat:** No installation required, just run the executable.

## How to Use 🚀

1.  **Launch the application** (`MouseTester.exe` on Windows or `./MouseTester` on Mac/Linux).
2.  **Click inside the window** to ensure it has focus.
3.  **Move your mouse continuously** in circles or zig-zags.
4.  Observe the **Current Hz** value on the screen.
    -   *Standard mice:* ~125 Hz
    -   *Gaming mice:* ~1000 Hz (1ms)
    -   *High-end gear:* 2000 - 8000 Hz (supported on capable hardware)
5.  Press **ESC** to exit.

## Requirements 📋

-   **.NET 8.0 Runtime** (or newer) if running the framework-dependent version.
-   *(No dependencies for self-contained builds)*

## Troubleshooting 🛠️

-   **macOS Users:** If the Hz counter stays at 0, verify that the application has **Input Monitoring** permissions in *System Settings > Privacy & Security*.
-   **Low Polling Rate:** Ensure your mouse software (Logitech G Hub, Razer Synapse, etc.) is set to the desired Hz profile.
