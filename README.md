# **HoloLens Fitness Game**

## **Overview**
This is a **fitness game for HoloLens** that leverages **real-time step and cadence data** from an Android app. The game encourages players to stay active while interacting with a **virtual dog companion** that reacts based on movement patterns.

## **Features**
- **Real-time Step & Cadence Tracking:** Receives live data from an Android app.
- **Dynamic Dog Behavior:** The dog adapts based on the player’s cadence.
- **Interactive Levels:** Brisk walking, jogging, and running phases.
- **Visual & Audio Feedback:** Provides on-screen notifications and sound effects.
- **HoloLens Mixed Reality Integration:** Enhances immersion with spatial interactions.

## **Tech Stack**
- **Unity Engine**
- **C# (Unity Scripting)**
- **MRTK (Mixed Reality Toolkit)** for HoloLens development
- **Google Fit API** (Android step & cadence tracking)
- **Socket Communication** for real-time data transfer

## **Installation**
### **Prerequisites**
- **HoloLens 2 device**
- **Android phone with Google Fit installed**
- **Unity (latest LTS version recommended)**
- **MRTK (Mixed Reality Toolkit) installed in Unity**

### **Setup Steps**
1. **Clone this repository:**
   ```bash
   git clone https://gitlab.com/your-repo/hololens-fitness-game.git
   ```
2. **Open the project in Unity.**
3. **Ensure MRTK is properly configured for HoloLens.**
4. **Set up the Android app to send step & cadence data to the HoloLens.**
5. **Deploy the Unity build to HoloLens via USB or Wi-Fi.**
6. **Run the Android app and start the game on HoloLens to experience real-time fitness tracking.**

## **Usage**
1. **Launch the game on HoloLens.**
2. **Start the Android app and grant necessary permissions.**
3. **Move around to see real-time updates in the game.**
4. **The virtual dog companion will react based on your cadence.**
5. **Complete all levels by reaching the required cadence thresholds.**

## **Permissions**
The app requires the following permissions:
- `internetClient` (HoloLens network communication)
- `android.permission.ACTIVITY_RECOGNITION` (Google Fit integration)
- `android.permission.BODY_SENSORS` (Heart rate data, if applicable)

## **Known Issues**
- **Latency in Data Sync:** Minor delays may occur in step count updates.
- **Dog Animation Glitches:** Some transitions may need fine-tuning.
- **HoloLens Tracking Drift:** Positional accuracy might vary in large open spaces.

## **Future Improvements**
- **Improved AI for Dog Behavior**
- **Enhanced Multiplayer Interaction**
- **Additional Workout Challenges**
- **Integration with More Fitness APIs**


