# DiffPatchWpf

DiffPatchWpf  is a windows binary patch maker tool.

Simple and nice interface writed in C#.
 
Compare two binary files and save the differences in new file patch.txt also named delta differential.

Support large files and limit memory requires when diff & patch.

Apply the patch in another binary fast and easy.

you can make and patch any files for : srs, ecu , bsi, dash , rooms , iso, image , etc ....  very fast.


example: 

load orifile.bin  file 1 

load modfile.bin  file 2

click diff button for save differences in file patch.txt

now you can apply this patch in other files

load otherfile.bin file1

load patch.ips

click patch button for save file orifile-patched.bin





![alt tag](https://github.com/reproteq/DiffPatchWpf/blob/main/DiffPatchWpf-screenshoot.png) 



https://youtu.be/EpyuF4t5MWk

Microsoft Visual Studio Community 2019

Versión 16.7.7

.NETFramework,Version=v4.7.2

Tested in windows 10x64bits


Notes!

To forget problems, use filenames that do not contain spaces or strange characters and everything will be fine.

Thanks.
