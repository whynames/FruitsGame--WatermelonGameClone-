# About

Fruit-like with modding support, made to learn how to handle mods  
I thought about using Addressables or AssetBundles, but chosen manual loading to enable modding without a need for Unity Engine  

# Features  

A game loop  
An ability to continue the last game  
A high score system with saving and loading  
A modding support  
An auto image size detection  

# Mods paths    
Windows: C:/Users/{userName}/AppData/LocalLow/R1nge/Fruit/Mods  
Android: /storage/emulated/{userId}/Android/data/com.R1nge.Fruit/files/Mods  

# Modding supports  
Fruit skins  
Fruit icons  
Container skin  
Music  
Drop chances    
In game background image  
Loading screen icon    
Loading screen background    
Character skin  
Merge sounds    
Sounds' volume

# Modding DOES NOT support  
Points system  
Different fruits amount (!= 12)  
Scripting    

# Modding guide
The mod title should be the same as the folder name  
The game supports image of any size in PNG JPEG formats; each dimension must be, independently, a power of two  
But, to avoid lag spikes while loading images, it's better to stick to recommended sizes in px  
Fruit skin sprite = 256-512  
Fruit ui icon sprite = 128-256  
Container skin sprite = 256-512  
In game background = screen resolution in 16:9, 18:9, 21:9 ratio  
Loading screen background = screen resolution in 16:9, 18:9, 21:9 ratio  
Loading icon = 128-256  
Character skin = 128-512  
The game supports audio files in MP3 WAV OGG formats  

[Modding tool](https://r1nge.github.io)  

Look at an example mod in `StreamingAssets` folder  

# Useful links
[Image resize](https://www.iloveimg.com/resize-image)  
[Itch.io](https://r1nge.itch.io/moddable-fruit)

# Thanks
[aniketrajnish](https://github.com/aniketrajnish) for [Collider optimizer](https://github.com/aniketrajnish/Unity-Collider-Optimizer)  
[ionxeph](https://github.com/ionxeph) and [Cover Corp](https://cover-corp.com/en/company) for hololive fruit assets
