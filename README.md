# DotNetFs

![Develop status badge](https://travis-ci.org/mikelcaz/DotNetFs.svg?branch=develop) Develop\
![Master status badge](https://travis-ci.org/mikelcaz/DotNetFs.svg?branch=master) Master

### Disclaimer

This project is a WIP. Use with caution.

### TL;DR

Are you using the `System.IO` namespace and its classes, **Path** and **Directory**, because you need these methods?:
- `Path.GetDirectoryName(string path)`
- `Directory.GetParent(string path)`

Maybe what you **really** want is something like:
- `Path.GetParentName(string path)`
- `Directory.GetDirectory(string path)`

Then replace `System.IO` with `DotNetFs` and `DotNetFs.Io`.

### Motivation

Using some .NET classes from `System.IO` (Path, Directory and File), I found two problems:

- Those mimics the design of MS-DOS/Windows.
- Part of the API works in a unexpected way.

The former point is a minor issue, but working in cross-platform projects is weird, because most operating systems are "UNIX-like" in their file systems, being Windows the odd one.

However, the latter soon became a problem. For example, let's say I want to use [`Path.GetDirectoryName(string path)`](https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getdirectoryname?view=netframework-4.7.2). But that method:

- Returns `null` if the path is a root directory (why?).
- Returns `string.Empty` if the path has not contains directory information.
- Returns the whole path (w/o the directory separator) if the path **seems** a directory.
- Returns the parent directory path otherwise.

> Note how when the method can't say if the path is a directory or a "file" (i.e. a non-directory file), it takes for granted that it must be the second one.

Did you understand a word? What a mess of semantic! You'll be introducing a ton of sutile bugs into your code in no time...

Plus, this method can't achieve its goal. Something similar happens with `Directory.GetParent(string name)`: those methods seems to be in the wrong class.

At the end, I decide to making a library to hide the ugly boilerplate code that talks to the `System.IO` classes, and provide sane semantics.

### Copyright and license

Copyright © 2018 Mikel Cazorla Pérez

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
