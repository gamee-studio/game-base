# Game base for Gamee
## Table of contents
- [Introduction](#Introduce)
- [Installation](#Install)

## Introduce
- This is a basic casual game base for intern and junior, it's integrated with [ads](https://github.com/gamee-studio/ads), [notification](https://github.com/pancake-llc/local-notification), [firebase tracking](https://github.com/pancake-llc/firebase-app), [firebase remote config](https://github.com/pancake-llc/firebase-remote-config)
- The project has simple data stream flow to help you to handle the game easier.

## Install
- Version: **Unity 2021.3.0f1**
- Type select: **Android**

## Documents
### Sound Controller
<details><summary>Adding new sound</summary>
<p>

- Add sound by adding new **SoundType** in file **SoundConfig.cs** then click **Update sound list** in **SoundConfig scriptable object**.
![image](https://user-images.githubusercontent.com/88299194/171227540-bb29f744-2e3c-4d64-8bad-07094f2fc9bb.png)
![image](https://user-images.githubusercontent.com/88299194/171226912-166151c1-c0f8-4730-ac9f-636a8070eae5.png)
  
</p>
</details>

<details><summary>Playing sound</summary>
<p>

```SoundController.Instance.PlayBackground(SoundType.Background)``` or ```SoundController.Instance.PlayFX(SoundType.Win)```
  
</p>
</details>

