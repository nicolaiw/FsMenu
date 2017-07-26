# FsMenu

> A very small DSL to create an interactive cli.

### Nuget

Inc.

### Usage

#### The DSL:

```fsharp
// Creates a new menu
let Menu = Sub

// Render sub menu
let (+>) name entry : (string * MenuEntry) = (name,entry)

// Execute action and exit
let (=>) s f = (s, Action (fun () -> f(); Exit))

// Execute action and render the previous menu
let (<+) s f = (s, Action (fun () -> f(); NavigateBack))

// Execute action and render the menu where you come frome
let (<+=) s f = (s, Action (fun () -> f(); Stay))
```

#### Example:

```fsharp

let testFunc() = 
    printfn "selected Sub Sub 3"
    printf "handle some input: "
    let input = Console.ReadLine()
    // Do some stuff with input
    ()

printfn "Use Keys: UP-Arrow DOWN-Arrow ENTER BACK-SPACE\n"

let mutable yesOrNo = ""

let test =
    Menu [
        "Item 1" => (fun () -> printf "selected Item 1")
        "Item 2" +>
           Menu [ 
               "Sub 1" +>
                       Menu [
                           "Sub Sub 1" => (fun () -> printf "selected Sub Sub 1")
                           "Sub Sub 2" +>
                                        Menu [
                                                "yes" <+ (fun () -> yesOrNo <- "--yes")
                                                "no " <+ (fun () -> yesOrNo <- "--no") ]
                           "Sub Sub 3" <+ testFunc]
               "Sub 2" => (fun () -> printf "selected Sub 2")
               "Sub 3" => (fun () -> printf "exec some command with param %s" yesOrNo)]] 
               

render test "<--"
```

> Instead of `(fun () -> printf ...` you could pass any `unit -> unit` function.


### Will turn into

![](https://github.com/nicolaiw/FsMenu/blob/master/misc/sample.gif)

### Another example

[Here](https://github.com/nicolaiw/FsMenu/tree/master/misc)

### Build

+ On Windows run build.cmd
+ On Linux run build.sh

### TODO ( contributes are very welcome :D )
+ Add Fake build (F# Make) and CI (AppVeyor, Travis)
+ Create NuGet Package using Paket
+ Add release notes
+ Add are render function which will take a color (mybe just for the emphasizer and/or for the whole entry)
+ Clean build.fsx