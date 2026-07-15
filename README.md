<div align="center">

# 🥽 Virt Meeting Room
### Collaborative Multi-User Virtual Reality Meeting Room

**Built with Unity 6 · Photon PUN2 · Meta Quest 2 · XR Interaction Toolkit**

[![Unity](https://img.shields.io/badge/Unity-6000.4.0f1-black?logo=unity)](https://unity.com/)
[![Photon](https://img.shields.io/badge/Photon-PUN2-blue)](https://www.photonengine.com/)
[![Platform](https://img.shields.io/badge/Platform-Meta%20Quest%202-purple)](https://www.meta.com/quest/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

</div>

---

## 📌 Overview

**Virt Meeting Room** is a real-time multi-user VR application built for the **Meta Quest 2**, where up to 6 remote participants can share the same virtual meeting space simultaneously.

Users can join a room with a code and password, sit at conference chairs, walk around the room, interact with a shared whiteboard, view presentation slides on a shared screen, and communicate through spatial voice — all in real time over Photon networking.

---

## ✨ Features

| Feature | Status |
|---|---|
| Photon PUN2 multiplayer (up to 6 players) | ✅ |
| Login scene with room code + password | ✅ |
| Player spawning at designated chair positions | ✅ |
| Idle, walk, and sit animations (Mixamo) | ✅ |
| Chair sit/stand interaction (range-based, A button) | ✅ |
| Networked avatar with head + hand tracking | ✅ |
| Two Bone IK for anatomical arm movement | ✅ |
| Shared whiteboard (draw + erase via marker) | ✅ |
| Presentation screen with slide switching | ✅ |
| Photon Voice spatial audio | 🔧 In Progress |
| VR controller UI interaction (Quest 2) | 🔧 In Progress |
| Finger tracking | 📋 Planned |

---

## 🛠️ Tech Stack

- **Engine** — Unity 6000.4.0f1
- **Networking** — Photon PUN 2
- **Voice** — Photon Voice SDK
- **VR SDK** — XR Interaction Toolkit + XR Plugin Management (Oculus)
- **Avatar animations** — Mixamo (Idle, Walk, Sit)
- **IK** — Unity Animation Rigging (Two Bone IK)
- **UI** — TextMesh Pro + World Space Canvas
- **3D Models** — Custom assets + Sketchfab imports
- **Target Device** — Meta Quest 2 (Android, ARM64)

---

## 🗂️ Project Structure

```
Assets/
├── 3D models/          # Imported room assets, avatars, props
│   ├── laptop/
│   └── macbook-laptop/
├── Animations/         # Mixamo animation clips (idle, walk, sit)
├── Materials/          # Room and model materials
├── Photon/             # PUN2 and Photon Voice SDK
├── Resources/          # Player prefab (required for PhotonNetwork.Instantiate)
├── Scenes/
│   ├── login.unity     # Login + room creation/join
│   └── Main.unity      # VR meeting room
├── Scripts/
│   ├── RoomManager.cs       # Photon connection, room create/join
│   ├── PlayerSpawner.cs     # Networked player instantiation
│   ├── NetworkPlayer.cs     # Head + hand sync, avatar root movement
│   ├── PlayerMovement.cs    # WASD + XR joystick locomotion
│   ├── ChairSit.cs          # Range-based chair sit/stand system
│   ├── WhiteboardManager.cs # RenderTexture drawing + Photon RPC sync
│   ├── WhiteboardDrawer.cs  # Controller raycast → whiteboard input
│   ├── PresentationScreen.cs# Slide display + networked navigation
│   └── GameSession.cs       # Static session data across scenes
├── Settings/           # XR and input settings
├── XRI/                # XR Interaction Toolkit Starter Assets
└── Sprites/
```

---

## 🚀 Getting Started

### Prerequisites

- Unity 6000.4.0f1
- Meta Quest 2 with Developer Mode enabled
- Android Build Support module installed in Unity Hub
- Photon PUN2 (already included in project)
- ADB (for manual APK installation)

### Setup

```bash
# Clone the repo
git clone https://github.com/k-artik-k/Virt-Meeting_Room-final.git

# Open in Unity Hub → Add project → Select folder
```

1. Open Unity Hub → Add the cloned project → Open with **Unity 6000.4.0f1**
2. Go to `Assets/Photon/PhotonUnityNetworking/Resources/PhotonServerSettings`
3. Paste your **Photon App ID** (PUN) — create one free at [photonengine.com](https://www.photonengine.com/)
4. Set **Fixed Region** to `asia` (or leave blank for auto)
5. Go to **File → Build Settings** → Switch Platform to **Android**
6. Go to **Edit → Project Settings → XR Plug-in Management → Android** → tick **Oculus**
7. Connect Quest 2 via USB → Enable USB Debugging on headset
8. **Build and Run** or build APK and sideload via ADB:

```bash
adb install path/to/VirtMeetingRoom.apk
```

---

## 🎮 Controls

| Action | Quest 2 | Editor (keyboard) |
|---|---|---|
| Move | Left thumbstick | WASD |
| Rotate | Right thumbstick | Q / E |
| Sit on chair (when in range) | A button | Space |
| Stand up | A button / move | Space / WASD |
| Draw on whiteboard | Right trigger (hold marker) | — |
| Erase whiteboard | Left trigger | — |
| Next slide | Right trigger near screen | — |

---

## 🌐 Multiplayer Flow

```
Player A                    Photon Cloud                Player B
   │                             │                          │
   ├─── ConnectUsingSettings ───▶│                          │
   │◀── OnConnectedToMaster ─────│                          │
   ├─── CreateRoom("ROOM01") ───▶│                          │
   │◀── OnCreatedRoom ───────────│                          │
   │                             │◀── JoinRoom("ROOM01") ───┤
   │                             │─── OnJoinedRoom ────────▶│
   │◀── PlayerB spawned ─────────│──────────────────────────┤
   │                             │                          │
   │◀══ Position/Rotation sync (PhotonView) ══════════════▶│
   │◀══ Whiteboard draws (PunRPC AllBuffered) ════════════▶│
   │◀══ Slide changes (PunRPC AllBuffered) ═══════════════▶│
```

---

## 📅 Development Timeline

| Day | Date | Milestone |
|---|---|---|
| 1 | 11 May | Unity + Photon fundamentals study |
| 2–4 | 12–14 May | Environment setup, Unity 6 migration |
| 5 | 15 May | Photon Cloud connection working |
| 6–7 | 18–19 May | Presentation screen + multi-scene setup |
| 8 | 20 May | Login UI built |
| 9 | 21 May | Photon networking integrated into team build |
| 10 | 22 May | NetworkPlayer, VoiceSetup scripts |
| 11–12 | 25–26 May | Quest 2 hardware testing, avatar bugs |
| 13 | 27 May | New avatar (Sketchfab), Mixamo animations |
| 14–17 | 29 May–3 Jun | Chair sit system, controller fixes, XR setup |
| 18 | 4 Jun | Fresh project — login to main scene working |
| 19–20 | 5–8 Jun | 3D models with materials, XR Origin, scripts |
| 21 | 9 Jun | Quest 2 build, chair interaction, spawn system |
| 22–23 | 10–11 Jun | Whiteboard + presentation screen, VR UI |
| 24 | 12 Jun | Multiplayer stable, IK arms, marker drawing |

---

## 🐛 Known Issues

- VR controller UI interaction in login scene requires further testing on device
- Photon server timeout can occur if Main scene load exceeds ~20s on low-end hardware
- Finger tracking not yet implemented (controller position tracking only)
- Whiteboard persistence for late-joining players requires additional buffered RPC handling

---

## 📁 Related Repository

Previous project iteration (Unity 2022 LTS, OVR-based):
[k-artik-k/Virtual_MeetingRoom](https://github.com/k-artik-k/Virtual_MeetingRoom)

---

## 👤 Author

**Kartikeya Raj Pappula**
B.Tech CSE — GITAM University, Visakhapatnam
[GitHub](https://github.com/k-artik-k) · [LinkedIn](https://linkedin.com/in/kartikeya-pappula) · [Instagram](https://instagram.com/kartikeya.design)

---

<div align="center">
Built as part of Problem Statement 5 — Collaborative Multi-User VR Meeting Room
</div>
