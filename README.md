# DiffPatchWpf

DiffPatchWpf  is a windows binary patch maker tool with nice and simple interface writed in C#.

Based in a C\C++ library and command-line tools for Diff & Patch between binary files.

Compare two binary files and save the differences in new file patch.ips also named delta differential.

Support large files and limit memory requires when diff & patch.

Apply the patch in another binary fast and easy.

you can make and patch any files for : srs, ecu , bsi, dash , rooms , iso, image , etc ....  very fast.


example: 

load orifile.bin  file 1 
load modfile.bin  file 2
click diff button for save differences in file patch.ips

now you can apply this patch in other files

load otherfile.bin file1
load patch.ips
click patch button for save file orifile-patched.bin



This code uses this project HDiffPatch for some of its functions.

https://github.com/sisong/HDiffPatch



![alt tag](https://github.com/reproteq/DiffPatchWpf/blob/main/DiffPatchWpf-screenshoot.png) 



https://www.youtube.com/watch?v=EqFH9XTSpN8
