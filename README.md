# pbutils: useful cross-platform utilities

## TL;DR

- `pbcopy.exe`/`pbpaste.exe`: modify clipboard from the command-line
- `bringtofront.exe`: bring a Microsoft Windows application to the foreground based on its window-class
- `tnet.exe`: simple telnet client that replaces the default Windows Telnet client


## Rationale

When I was working on OSX I got familiar with `pbcopy` and `pbpaste`, which enable clipboard manipulation from the command line. When I later used Linux I used `xsel`, which serves the same purpose, and created bash aliases to accomplish the same tasks:

````bash
    alias pbcopy='xsel --clipboard --input'
    alias pbpaste='xsel --clipboard --output'
````

And then when I moved to Windows, I found [pasteboard](https://github.com/ghuntley/pasteboard), by Geoffrey Huntley, to which I made minor edits for correctness. Boy, I should do a PR!

But the tools in a programmer's toolbox are never sharp enough, so I went on to add `bringtofront.exe` because WinMerge likes to hide in the background. I created `tnet.exe` because Windows `telnet.exe` has a 30-second connect timeout and doesn't respond to Ctrl-C or Ctrl-Break.
